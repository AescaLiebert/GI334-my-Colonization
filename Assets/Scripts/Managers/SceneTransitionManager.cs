using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager instance;
    public static SceneTransitionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneTransitionManager>();
                if (instance == null)
                {
                    Debug.Log("SceneTransitionManager: Creating new instance.");
                    GameObject obj = new GameObject("SceneTransitionManager");
                    instance = obj.AddComponent<SceneTransitionManager>();
                    if (instance == null) Debug.LogError("SceneTransitionManager: AddComponent returned null!");
                }
                else
                {
                     // Debug.Log("SceneTransitionManager: Found existing instance.");
                }
            }
            return instance;
        }
    }

    [SerializeField]
    private float fadeDuration = 1.0f;

    private Canvas fadeCanvas;
    private Image fadeImage;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SetupFadeUI();
    }

    private void SetupFadeUI()
    {
        // Create Canvas
        GameObject canvasObj = new GameObject("TransitionCanvas");
        fadeCanvas = canvasObj.AddComponent<Canvas>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.sortingOrder = 9999; // Ensure it's on top
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        DontDestroyOnLoad(canvasObj);

        // Create Image
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform, false);
        fadeImage = imageObj.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0); // Start transparent
        fadeImage.raycastTarget = false;
        
        // Stretch image to fill screen
        RectTransform rect = fadeImage.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        fadeImage.raycastTarget = true;

        // Fade In (to black)
        yield return StartCoroutine(Fade(1f, fadeDuration));

        // Load Scene
        yield return SceneManager.LoadSceneAsync(sceneName);

        // Fade Out (from black)
        yield return StartCoroutine(Fade(0f, fadeDuration));

        fadeImage.raycastTarget = false;
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        if (fadeImage == null) SetupFadeUI();

        float startAlpha = fadeImage.color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }

    // New methods for manual control
    public void SetAlpha(float alpha)
    {
        if (fadeImage == null) SetupFadeUI();
        fadeImage.color = new Color(0, 0, 0, alpha);
        // Ensure raycast blocks if opaque
        fadeImage.raycastTarget = alpha > 0;
    }

    public void FadeOut(float duration)
    {
         StartCoroutine(FadeOutRoutine(duration));
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        yield return StartCoroutine(Fade(0f, duration));
        fadeImage.raycastTarget = false;
    }
}
