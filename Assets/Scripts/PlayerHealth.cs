using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 10; // Maksymalne zdrowie gracza

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Logika końca gry lub restartu sceny
            Debug.Log("Gracz zginął!");
        }
    }
}
