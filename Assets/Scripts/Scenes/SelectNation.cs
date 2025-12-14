using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SelectNation : MonoBehaviour
{
    [SerializeField]
    private Sprite[] leaderSprites;

    [SerializeField]
    private Image leaderImage;

    [SerializeField]
    private string nextScene = "Map01";

    [SerializeField]
    private int index = 0;

    [SerializeField]
    private GameObject blackPanel;

    [SerializeField]
    private GameObject[] panels;

    [Header("Nation Selection Visuals")]
    [SerializeField] private GameObject[] nationCards; // Assign the 5 Nation Card Prefabs here
    [SerializeField] private Sprite[] nationLeaderSprites;
    [SerializeField] private Image nationLeaderImage;
    [SerializeField] private float visualFadeDuration = 0.5f;

    private Coroutine leaderFadeCoroutine;
    private Coroutine[] cardFadeCoroutines;

    [SerializeField]
    private float fadeDuration = 1.0f;

    private void Start()
    {
        // Initialize helpers
        if (nationCards != null)
        {
            cardFadeCoroutines = new Coroutine[nationCards.Length];
        }

        // Initialize display based on default index
        UpdateVisuals(index);
    }

    public void SelectEuropeNation(int i)
    {
        index = i;
        Settings.playerNationId = index;

        UpdateVisuals(index);
    }

    private void UpdateVisuals(int newIndex)
    {
        // 1. Nation Info Text
        if (nationLeaderImage != null && nationLeaderSprites != null && newIndex >= 0 && newIndex < nationLeaderSprites.Length)
        {
            nationLeaderImage.sprite = nationLeaderSprites[newIndex];
        }

        // 2. Leader Image Transition
        if (leaderImage != null && leaderSprites != null && newIndex >= 0 && newIndex < leaderSprites.Length)
        {
            if (leaderFadeCoroutine != null) StopCoroutine(leaderFadeCoroutine);
            leaderFadeCoroutine = StartCoroutine(TransitionLeaderImage(leaderSprites[newIndex]));
        }

        // 3. Nation Card Highlights (NonSelectHighlight)
        if (nationCards != null)
        {
            for (int k = 0; k < nationCards.Length; k++)
            {
                if (nationCards[k] == null) continue;

                // Find the child named "NonSelectHighlight"
                Transform t = nationCards[k].transform.Find("NonSelectHighlight");
                if (t != null)
                {
                    Image highlightImg = t.GetComponent<Image>();
                    if (highlightImg != null)
                    {
                        // Selected = 0 (Off), Others = 0.5 (On/Darkened)
                        float targetAlpha = (k == newIndex) ? 0f : 0.5f;

                        if (cardFadeCoroutines[k] != null) StopCoroutine(cardFadeCoroutines[k]);
                        cardFadeCoroutines[k] = StartCoroutine(FadeImageAlpha(highlightImg, targetAlpha));
                    }
                }
            }
        }
    }

    private IEnumerator TransitionLeaderImage(Sprite newSprite)
    {
        // Fade Out
        float t = 0f;
        Color startColor = leaderImage.color;
        // Assuming we want to keep RGB and just alpha fade, or fade to black? 
        // Usually simpler to fade Alpha to 0, swap, Alpha to 1.
        
        while (t < visualFadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(startColor.a, 0f, t / visualFadeDuration);
            leaderImage.color = new Color(startColor.r, startColor.g, startColor.b, a);
            yield return null;
        }

        // Swap
        leaderImage.sprite = newSprite;

        // Fade In
        t = 0f;
        while (t < visualFadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(0f, 1f, t / visualFadeDuration);
            leaderImage.color = new Color(startColor.r, startColor.g, startColor.b, a);
            yield return null;
        }
        leaderImage.color = new Color(startColor.r, startColor.g, startColor.b, 1f);
    }

    private IEnumerator FadeImageAlpha(Image img, float targetAlpha)
    {
        float startAlpha = img.color.a;
        float t = 0f;
        Color c = img.color;
        
        while (t < visualFadeDuration)
        {
            t += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t / visualFadeDuration);
            img.color = new Color(c.r, c.g, c.b, newAlpha);
            yield return null;
        }
        img.color = new Color(c.r, c.g, c.b, targetAlpha);
    }

    public void StartGame()
    {
        StartCoroutine(TransitionToKingCommand());
    }

    private IEnumerator TransitionToKingCommand()
    {
        // Ensure black panel is active
        blackPanel.SetActive(true);
        Image blackImage = blackPanel.GetComponent<Image>();
        if (blackImage == null) blackImage = blackPanel.AddComponent<Image>();
        
        // Initial setup: make sure it's transparent or set to fade
        // Assuming we want to Fade IN (to black) -> Show Panel -> Fade OUT (to scene)
        
        // 1. Fade IN to Black
        yield return StartCoroutine(FadeBlackPanel(blackImage, 0f, 1f, fadeDuration));

        // 2. Show Kings Command Panel (Hidden behind black layer now)
        ShowKingsCommand();

        // 3. Fade OUT from Black (Revealing Kings Command)
        yield return StartCoroutine(FadeBlackPanel(blackImage, 1f, 0f, fadeDuration));
        
        blackPanel.SetActive(false);
    }

    private IEnumerator FadeBlackPanel(Image image, float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        Color c = image.color;
        while (time < duration)
        {
            time += Time.deltaTime;
            float a = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            image.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
        image.color = new Color(c.r, c.g, c.b, endAlpha);
    }

    public void ShowKingsCommand()
    {
        // Hide others?
        // panels[0].SetActive(true);
        // Better to ensure others are closed or just open this one on top
        // Assuming Logic: Panels[0] is KingCommand
        SetPanelActive(0);
    }

    public void ShowPrepareToLeave()
    {
        SetPanelActive(1);
    }

    public void ShowLeftThePort()
    {
        SetPanelActive(2);
    }

    public void FinishCutscene()
    {
        SceneTransitionManager.Instance.LoadScene(nextScene);
    }

    private void SetPanelActive(int activeIndex)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            bool isActive = (i == activeIndex);

            if (isActive)
            {
                CutsceneController cutscene = panels[i].GetComponent<CutsceneController>();
                if (cutscene != null)
                {
                    string text = GetNarrativeText(i);
                    cutscene.SetNarrativeText(text);
                }
            }

            panels[i].SetActive(isActive);
        }
    }

    private string GetNarrativeText(int index)
    {
        switch (index)
        {
            case 0: return "Your Majesty has decreed. We are to sail West, beyond the known charts. The glory of the Empire rests on our success.";
            case 1: return "Supplies are loaded. The crew says their goodbyes. The wind is favorable.";
            case 2: return "The harbor fades into the mist. Ahead lies only the open ocean... and destiny.";
            default: return "";
        }
    }
}
