using System;
using UnityEngine;

public static class GameEvents
{
    // Event raised when two fruits merge
    // The FruitData is the new fruit that was created
    public static event Action<FruitData> OnFruitMerged;
    public static void TriggerFruitMerged(FruitData newFruit) => OnFruitMerged?.Invoke(newFruit);

    // Event raised when the game is over
    public static event Action OnGameOver;
    public static void TriggerGameOver() => OnGameOver?.Invoke();

    // Event to notify UI to update score
    public static event Action<int> OnScoreUpdated;
    public static void TriggerScoreUpdated(int newScore) => OnScoreUpdated?.Invoke(newScore);

    // Event to notify UI about the next fruit
    public static event Action<Sprite> OnNextFruitChanged;
    public static void TriggerNextFruitChanged(Sprite fruitSprite) => OnNextFruitChanged?.Invoke(fruitSprite);
}