using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int currentScore = 0;

    void OnEnable()
    {
        GameEvents.OnFruitMerged += HandleFruitMerged;
    }

    private void OnDisable()
    {
        GameEvents.OnFruitMerged -= HandleFruitMerged;
    }

    private void HandleFruitMerged(FruitData mergedFruitData,Vector3 mergedFruitPosition)
    {
        int multiplier = ComboManager.Instance.GetComboMultiplier();
        currentScore += mergedFruitData.points * multiplier;
        // Raise an event to let the UI know the score has changed
        GameEvents.TriggerScoreUpdated(currentScore);
    }
    
    
}