using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MergeService : MonoBehaviour
{
    public static MergeService Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ProcessMerge(Fruit fruitA, Fruit fruitB)
    {
        // To prevent double merges, only the fruit with the higher Instance ID proceeds
        if (fruitA.GetInstanceID() < fruitB.GetInstanceID()) return;

        fruitA.hasMerged = true;
        fruitB.hasMerged = true;

        Vector3 mergePosition = (fruitA.transform.position + fruitB.transform.position) / 2;
        FruitPool.Instance.ReturnToPool(fruitA);
        FruitPool.Instance.ReturnToPool(fruitB);

        FruitData nextFruitData = fruitA.fruitData.nextFruitData;
        if (nextFruitData != null)
        {
            Fruit newFruit = FruitPool.Instance.Get();
            newFruit.transform.position = mergePosition;
            newFruit.SetFruitData(nextFruitData);
            
            // Trigger the merge event for other systems
            GameEvents.TriggerFruitMerged(nextFruitData,mergePosition);
        }
    }
}