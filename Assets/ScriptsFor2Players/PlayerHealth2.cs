using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth2 : MonoBehaviour
{
    public int maxHealth = 10; // Maksymalne zdrowie gracza
    private int currentHealth;

    public Slider healthBar;
    public GameManager2 gameManager2;
    public ObjectSpawnerFor2Players ObjectSpawner;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        if (gameManager2 == null)
        {
            gameManager2 = FindObjectOfType<GameManager2>();
            if (gameManager2 == null)
            {
                Debug.LogError("Brak GameManager w tej scenie!");
            }
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Debug.Log("Gracz zginął!");
            if (ObjectSpawner != null)
            {
                Debug.Log("Zatrzymywanie spawnowania obiektów...");
                ObjectSpawnerFor2Players.Instance.StopSpawning();
            }
            gameManager2.GameEnd();
        }

    }
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }
 
}
