using UnityEngine;
using System.Collections.Generic;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] private List<FruitData> spawnableFruits;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private float dropBoundsX = 2.5f;

    // --- LOGIC CHANGE ---
    // We now track the current and next fruit data separately.
    private FruitData currentFruitData;
    private FruitData nextFruitData;
    private Fruit heldFruit;

    void Start()
    {
        GameEvents.OnGameOver += HandleGameOver;
        
        // Prepare the first two fruits at the start of the game.
        currentFruitData = GetRandomFruitData();
        nextFruitData = GetRandomFruitData();

        // Update the UI to show the *correct* next fruit.
        GameEvents.TriggerNextFruitChanged(nextFruitData.fruitSprite);
        
        // Spawn the first fruit for the player to hold.
        SpawnHeldFruit();
    }

    private void OnDestroy() 
    {
        GameEvents.OnGameOver -= HandleGameOver;
    }

    void Update()
    {
        if (heldFruit == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float clampedX = Mathf.Clamp(mousePos.x, -dropBoundsX, dropBoundsX);
        heldFruit.transform.position = new Vector3(clampedX, dropPoint.position.y, 0);

        // Drop on click
        if (Input.GetMouseButtonDown(0))
        {
            DropFruit();
        }
    }
    
    private FruitData GetRandomFruitData()
    {
        return spawnableFruits[Random.Range(0, spawnableFruits.Count)];
    }

    private void SpawnHeldFruit()
    {
        heldFruit = FruitPool.Instance.Get();
        heldFruit.SetFruitData(currentFruitData);
        // Disable physics while holding
        heldFruit.GetComponent<Rigidbody2D>().isKinematic = true;
        heldFruit.GetComponent<Collider2D>().enabled = false;
    }

    private void DropFruit()
    {
        if (heldFruit == null) return;
        
        // Enable physics
        heldFruit.GetComponent<Rigidbody2D>().isKinematic = false;
        heldFruit.GetComponent<Collider2D>().enabled = true;
        heldFruit = null;
        
        // After a short delay, prepare for the next drop.
        Invoke(nameof(PrepareNextDrop), 0.5f);
    }


    void PrepareNextDrop()
    {
    
        currentFruitData = nextFruitData;

        nextFruitData = GetRandomFruitData();

        GameEvents.TriggerNextFruitChanged(nextFruitData.fruitSprite);

        SpawnHeldFruit();
    }
    
    private void HandleGameOver()
    {
        if (heldFruit != null) 
        {
            FruitPool.Instance.ReturnToPool(heldFruit);
            heldFruit = null;
        }
        this.enabled = false; 
    }
}