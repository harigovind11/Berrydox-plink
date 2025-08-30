using UnityEngine;

public class Fruit : MonoBehaviour
{
    public FruitData fruitData { get; private set; }

    private SpriteRenderer _spriteRenderer;
    private Collider2D _Collider;
    


    public bool hasMerged { get; set; }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _Collider = GetComponent<Collider2D>();
    }

    public void SetFruitData(FruitData data)
    {
        fruitData = data;
        
        _spriteRenderer.sprite = data.fruitSprite;
        transform.name = $"Fruit_{data.fruitIndex}";

        transform.localScale = Vector3.one * data.size;
        
    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasMerged) return;

        if (collision.gameObject.CompareTag("Fruit"))
        {
            Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();
            if (otherFruit != null && !otherFruit.hasMerged && otherFruit.fruitData.fruitIndex == this.fruitData.fruitIndex)
            {
                MergeService.Instance.ProcessMerge(this, otherFruit);
            }
    
        }
    }
}