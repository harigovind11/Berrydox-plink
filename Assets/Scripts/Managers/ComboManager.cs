
using System;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance {get; private set;}

    [SerializeField] private float comboTimeWindow = 2.0f;

    public int ComboCount {get; private set;}
    private float comboTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnFruitMerged += HandleFruitMerged;
    }

    private void OnDisable()
    {
        GameEvents.OnFruitMerged -= HandleFruitMerged;
    }
    
    private void Update()
    {
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                BreakCombo();
            }
        }
    }
    private void HandleFruitMerged(FruitData data, Vector3 position)
    {
        // A merge occurred, so increase the combo and reset the timer
        ComboCount++;
        comboTimer = comboTimeWindow;
        GameEvents.TriggerComboUpdated(ComboCount);
    }
    private void BreakCombo()
    {
        if (ComboCount > 1) // Only broadcast if it was a real combo
        {
            GameEvents.TriggerComboBroken();
        }
        ComboCount = 0;
    }
    public int GetComboMultiplier()
    {
        if (ComboCount < 2) return 1;
        if (ComboCount < 5) return 2;
        if (ComboCount < 10) return 3;
        return 4; // Max multiplier
    }
}
