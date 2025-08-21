using UnityEngine;
using System.Collections.Generic;

public class FruitPool : MonoBehaviour
{
    public static FruitPool Instance { get; private set; }

    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private int initialPoolSize = 20;

    private Queue<Fruit> pool = new Queue<Fruit>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            AddNewFruitToPool();
        }
    }

    private Fruit AddNewFruitToPool()
    {
        GameObject newFruitObj = Instantiate(fruitPrefab, transform);
        Fruit newFruit = newFruitObj.GetComponent<Fruit>();
        newFruitObj.SetActive(false);
        pool.Enqueue(newFruit);
        return newFruit;
    }

    public Fruit Get()
    {
        if (pool.Count == 0)
        {
            AddNewFruitToPool();
        }

        Fruit fruit = pool.Dequeue();
        fruit.gameObject.SetActive(true);
        return fruit;
    }

    public void ReturnToPool(Fruit fruit)
    {
        fruit.gameObject.SetActive(false);
        fruit.hasMerged = false;
        // Reset physics
        Rigidbody2D rb = fruit.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        pool.Enqueue(fruit);
    }
}