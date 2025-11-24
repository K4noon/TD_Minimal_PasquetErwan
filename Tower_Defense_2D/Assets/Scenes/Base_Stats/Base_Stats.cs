using UnityEngine;

public class Base_Stats : MonoBehaviour
{
    int health = 100;

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) // Condition to check if health is zero or below
        {
            Debug.Log("Game Over"); // Log "Game Over" to the console
        }
    }
}
