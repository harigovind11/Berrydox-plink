using UnityEngine;

[CreateAssetMenu(fileName = "FruitData", menuName = "Berrydox/Fruit Data")]
public class FruitData : ScriptableObject
{
    public int fruitIndex;
    public int points;
    public Sprite fruitSprite;
    public FruitData nextFruitData;
    public float size = 1.0f;
}