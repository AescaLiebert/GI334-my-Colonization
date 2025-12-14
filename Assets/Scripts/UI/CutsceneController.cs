using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private RectTransform background;

    [Header("Settings")]
    [TextArea(3, 10)]
    [SerializeField] private string narrativeText;
    [SerializeField] private float typeSpeed = 0.05f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Vector2 moveDirection = new Vector2(1, -1); // Default random drift

    private Coroutine typingCoroutine;
    private Coroutine movingCoroutine;

    private void OnEnable()
    {
        // Randomize direction slightly
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        moveDirection = new Vector2(randomX, randomY).normalized;

        if (textComponent != null)
        {
            textComponent.text = ""; // Clear initial text
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypewriterEffect());
        }

        if (background != null)
        {
            if (movingCoroutine != null) StopCoroutine(movingCoroutine);
            movingCoroutine = StartCoroutine(MoveBackground());
        }
    }

    private void OnDisable()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        if (movingCoroutine != null) StopCoroutine(movingCoroutine);
    }

    private IEnumerator TypewriterEffect()
    {
        foreach (char c in narrativeText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    private IEnumerator MoveBackground()
    {
        while (true)
        {
            if (background != null)
            {
                background.anchoredPosition += moveDirection * moveSpeed * Time.deltaTime;
            }
            yield return null;
        }
    }

    // Public method to set text if needed dynamically
    public void SetNarrativeText(string text)
    {
        narrativeText = text;
    }
}
