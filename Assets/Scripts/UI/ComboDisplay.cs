using UnityEngine;
using TMPro;
using System.Collections;

public class ComboDisplay : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private CanvasGroup comboCanvasGroup;

    [Header("Animation Settings")]
    [SerializeField] private float effectDuration = 1.2f;
    [SerializeField] private float moveUpDistance = 50f;
    [SerializeField] private float scaleFactor = 1.5f;

    private Vector3 startPosition;
    private Coroutine comboAnimationCoroutine;

    private void Awake()
    {
        if (comboText != null)
        {
            startPosition = comboText.transform.localPosition;
        }
    }

    private void OnEnable()
    {
        GameEvents.OnComboUpdated += ShowComboEffect;

    }

    private void OnDisable()
    {
        GameEvents.OnComboUpdated -= ShowComboEffect;
    }

    private void ShowComboEffect(int comboCount)
    {
        // Only show the effect for combos of 2x or higher
        if (comboCount > 1)
        {
            // If an animation is already playing, stop it first.
            if (comboAnimationCoroutine != null)
            {
                StopCoroutine(comboAnimationCoroutine);
            }
            comboAnimationCoroutine = StartCoroutine(ComboAnimationCoroutine(comboCount));
        }
        else
        {
            // If the combo is 1 or breaks, instantly hide it.
            if (comboCanvasGroup != null)
            {
                comboCanvasGroup.alpha = 0;
            }
        }
    }

    private IEnumerator ComboAnimationCoroutine(int comboCount)
    {
        // 1. Reset the state before starting the animation
        comboText.transform.localPosition = startPosition;
        comboText.transform.localScale = Vector3.one;
        comboCanvasGroup.alpha = 1f; // Make it instantly visible
        comboText.text = $"{comboCount}x";

        // 2. Define the start and end points for the animation
        Vector3 targetPosition = startPosition + new Vector3(0, moveUpDistance, 0);
        Vector3 targetScale = Vector3.one * scaleFactor;

        // 3. Animate over the specified duration
        float elapsedTime = 0f;
        while (elapsedTime < effectDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / effectDuration; // A value from 0 to 1

            // Lerp (linearly interpolate) the properties based on progress
            comboText.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, progress);
            comboText.transform.localScale = Vector3.Lerp(Vector3.one, targetScale, progress);
            comboCanvasGroup.alpha = Mathf.Lerp(1f, 0f, progress); // Fade from 1 to 0

            yield return null; // Wait for the next frame
        }

        // 4. Ensure it's fully hidden at the end
        comboCanvasGroup.alpha = 0f;
    }
}