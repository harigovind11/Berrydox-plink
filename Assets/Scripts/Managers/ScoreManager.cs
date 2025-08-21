using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int currentScore = 0;

    void Start()
    {
        // Subscribe to the event
        GameEvents.OnFruitMerged += HandleFruitMerged;
    }

    private void OnDestroy()
    {
        // Always unsubscribe when the object is destroyed
        GameEvents.OnFruitMerged -= HandleFruitMerged;
    }

    private void HandleFruitMerged(FruitData mergedFruitData)
    {
        currentScore += mergedFruitData.points;
        // Raise an event to let the UI know the score has changed
        GameEvents.TriggerScoreUpdated(currentScore);
    }
}