using UnityEngine;
using System.Collections.Generic;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] private List<FruitData> spawnableFruits; // Only basic fruits, e.g., first 3-4
    [SerializeField] private Transform dropPoint;
    [SerializeField] private float dropBoundsX = 2.5f;

    private FruitData nextFruitToSpawn;
    private Fruit heldFruit;

    void Start()
    {
        // Listen for game over so we can stop spawning
        GameEvents.OnGameOver += HandleGameOver;
        PrepareNextFruit();
        SpawnFruit();
    }

    private void OnDestroy() {
        GameEvents.OnGameOver -= HandleGameOver;
    }

    void Update()
    {
        if (heldFruit == null) return;
        
        // Move held fruit via mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float clampedX = Mathf.Clamp(mousePos.x, -dropBoundsX, dropBoundsX);
        heldFruit.transform.position = new Vector3(clampedX, dropPoint.position.y, 0);

        // Drop on click
        if (Input.GetMouseButtonDown(0))
        {
            DropFruit();
        }
    }

    private void PrepareNextFruit()
    {
        nextFruitToSpawn = spawnableFruits[Random.Range(0, spawnableFruits.Count)];
        GameEvents.TriggerNextFruitChanged(nextFruitToSpawn.fruitSprite);
    }
    
    private void SpawnFruit()
    {
        heldFruit = FruitPool.Instance.Get();
        heldFruit.SetFruitData(nextFruitToSpawn);
        // Disable physics while holding
        heldFruit.GetComponent<Rigidbody2D>().isKinematic = true;
        heldFruit.GetComponent<Collider2D>().enabled = false;
    }

    private void DropFruit()
    {
        // Enable physics
        heldFruit.GetComponent<Rigidbody2D>().isKinematic = false;
        heldFruit.GetComponent<Collider2D>().enabled = true;
        heldFruit = null;
        
        Invoke(nameof(GetReadyForNextDrop), 0.5f);
    }

    void GetReadyForNextDrop()
    {
        PrepareNextFruit();
        SpawnFruit();
    }
    
    private void HandleGameOver()
    {
        if (heldFruit != null) {
            Destroy(heldFruit.gameObject); // Or return to pool
        }
        this.enabled = false; // Stop this script
    }
}