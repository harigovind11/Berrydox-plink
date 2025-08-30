using UnityEngine;
using TMPro;
using System.Collections;

public class ComboDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private float pulseScale = 1.5f;   // How big it gets on update
    [SerializeField] private float animationSpeed = 10f; // How fast it animates

    private Coroutine currentAnimation;

    private void Awake()
    {
        // Ensure the component is linked, if not, try to find it on this GameObject.
        if (comboText == null)
        {
            comboText = GetComponent<TextMeshProUGUI>();
        }
        // Start with the text hidden
        comboText.alpha = 0;
    }

    private void OnEnable()
    {
        GameEvents.OnComboUpdated += HandleComboUpdated;
        GameEvents.OnComboBroken += HandleComboBroken;
    }

    private void OnDisable()
    {
        GameEvents.OnComboUpdated -= HandleComboUpdated;
        GameEvents.OnComboBroken -= HandleComboBroken;
    }

    private void HandleComboUpdated(int comboCount)
    {
        // We typically don't show a "1x" combo. You can change this if you want.
        if (comboCount < 2) return;

        // Update the text
        comboText.text = $"{comboCount}x";

        // Start the pulse animation
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        currentAnimation = StartCoroutine(AnimatePulse());
    }

    private void HandleComboBroken()
    {
        // Start the fade-out animation
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        currentAnimation = StartCoroutine(AnimateFadeOut());
    }

    private IEnumerator AnimatePulse()
    {
        // Make the text visible and pop it to a larger scale
        comboText.alpha = 1;
        transform.localScale = Vector3.one * pulseScale;

        // Animate it smoothly back to its normal size
        while (transform.localScale.x > 1.05f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * animationSpeed);
            yield return null;
        }
        transform.localScale = Vector3.one; // Snap to final scale
    }

    private IEnumerator AnimateFadeOut()
    {
        // Animate the alpha smoothly to zero
        while (comboText.alpha > 0.05f)
        {
            comboText.alpha = Mathf.Lerp(comboText.alpha, 0, Time.deltaTime * animationSpeed);
            yield return null;
        }
        comboText.alpha = 0; // Snap to final alpha
    }
}