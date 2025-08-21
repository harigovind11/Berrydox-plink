using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    private float timeInTrigger = 0f;
    private const float timeToEndGame = 1.5f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            timeInTrigger += Time.deltaTime;
            if (timeInTrigger >= timeToEndGame)
            {
                GameEvents.TriggerGameOver();
                // Disable this script to only trigger game over once
                this.enabled = false; 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            timeInTrigger = 0f;
        }
    }
}