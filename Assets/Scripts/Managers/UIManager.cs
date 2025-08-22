using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image nextFruitImage;
    [SerializeField] private GameObject gameOverPanel;

    void Start()
    {
        // Subscribe to events
        GameEvents.OnScoreUpdated += UpdateScoreText;
        GameEvents.OnNextFruitChanged += UpdateNextFruitImage;
        GameEvents.OnGameOver += ShowGameOverPanel;
        
        // Initial setup
        gameOverPanel.SetActive(false);
        UpdateScoreText(0);
    }

    private void OnDestroy()
    {
        // Unsubscribe
        GameEvents.OnScoreUpdated -= UpdateScoreText;
        GameEvents.OnNextFruitChanged -= UpdateNextFruitImage;
        GameEvents.OnGameOver -= ShowGameOverPanel;
    }

    private void UpdateScoreText(int score)
    {
        scoreText.text = $"{score}";
            
    }

    private void UpdateNextFruitImage(Sprite sprite)
    {
        nextFruitImage.sprite = sprite;
    }

    private void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
}