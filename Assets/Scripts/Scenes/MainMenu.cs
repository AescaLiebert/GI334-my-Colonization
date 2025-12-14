using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    [Header("UI References")]
    [SerializeField] private RectTransform gameTitle;
    [SerializeField] private RectTransform framePanel;

    [Header("Animation Settings")]
    [SerializeField] private float titleDropDuration = 1.5f;
    [SerializeField] private float panelScaleDuration = 1.0f;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float titleStartYOffset = 500f; // Start above screen

    private void Start()
    {
        // 1. Black Screen Fade out
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.SetAlpha(1f); // Start black
            SceneTransitionManager.Instance.FadeOut(fadeDuration);
        }

        // 2. GameTitle dropping in
        if (gameTitle != null)
        {
            StartCoroutine(AnimateTitle());
        }

        // 3. FramePanel scaling 0 -> 1
        if (framePanel != null)
        {
            StartCoroutine(AnimatePanel());
        }
    }

    private IEnumerator AnimateTitle()
    {
        Vector2 finalPos = gameTitle.anchoredPosition;
        Vector2 startPos = finalPos + new Vector2(0, titleStartYOffset);
        
        gameTitle.anchoredPosition = startPos;

        float time = 0f;
        while (time < titleDropDuration)
        {
            time += Time.deltaTime;
            float t = time / titleDropDuration;
            // Use a simple ease out bounce or smooth step
            t = t * t * (3f - 2f * t); // SmoothStep
            
            gameTitle.anchoredPosition = Vector2.Lerp(startPos, finalPos, t);
            yield return null;
        }
        gameTitle.anchoredPosition = finalPos;
    }

    private IEnumerator AnimatePanel()
    {
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = new Vector3(0.75f, 0.75f, 0.75f);
        framePanel.localScale = startScale;

        float time = 0f;
        while (time < panelScaleDuration)
        {
            time += Time.deltaTime;
            float t = time / panelScaleDuration;
            
            framePanel.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }
        framePanel.localScale = targetScale;
    }

    public void LoadNextScene()
    {
        Debug.Log($"MainMenu: Loading next scene '{nextScene}'");
        if (SceneTransitionManager.Instance == null) Debug.LogError("MainMenu: SceneTransitionManager.Instance is null!");
        SceneTransitionManager.Instance.LoadScene(nextScene);
    }
}
