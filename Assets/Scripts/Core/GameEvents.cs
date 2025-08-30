using System;
using UnityEngine;

public static class GameEvents
{
    // To play a sound when the player drops a fruit.
    public static event Action OnFruitDropped;
    public static void TriggerFruitDropped()=> OnFruitDropped?.Invoke();
    
    /// <summary>
    /// Event raised when two fruits merge
    /// The FruitData is the new fruit that was created
    /// </summary>
    public static event Action<FruitData,Vector3> OnFruitMerged;
    public static void TriggerFruitMerged(FruitData newFruit,Vector3 position) => OnFruitMerged?.Invoke(newFruit,position);
    
    // Combo events
    public static event Action<int> OnComboUpdated;
    public static void TriggerComboUpdated(int comboCount) => OnComboUpdated?.Invoke(comboCount);

    public static event Action OnComboBroken;
    public static void TriggerComboBroken() => OnComboBroken?.Invoke();

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