using UnityEngine;

public class DynamicBackground : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The maximum distance the background can move from its starting position.")]
    [SerializeField] private float moveRange = 50f;

    [Tooltip("Approximately the time it will take to reach the target. A smaller value is faster.")]
    [SerializeField] private float smoothTime = 2.0f;

    [Tooltip("The maximum speed the background can move.")]
    [SerializeField] private float maxSpeed = 100f;

    private RectTransform rectTransform;
    private Vector2 initialPosition;
    private Vector2 targetPosition;
    private Vector2 currentVelocity;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (rectTransform == null)
        {
            Debug.LogError("DynamicBackground: No RectTransform found on this object!");
            enabled = false;
            return;
        }

        initialPosition = rectTransform.anchoredPosition;
        PickNewTarget();
    }

    private void Update()
    {
        // Move towards the target position
        rectTransform.anchoredPosition = Vector2.SmoothDamp(
            rectTransform.anchoredPosition,
            targetPosition,
            ref currentVelocity,
            smoothTime,
            maxSpeed
        );

        // Check if we are close enough to the target to pick a new one
        float distance = Vector2.Distance(rectTransform.anchoredPosition, targetPosition);
        if (distance < 1f)
        {
            PickNewTarget();
        }
    }

    private void PickNewTarget()
    {
        // Pick a random point within a circle around the initial position
        Vector2 randomOffset = Random.insideUnitCircle * moveRange;
        targetPosition = initialPosition + randomOffset;
    }
}
