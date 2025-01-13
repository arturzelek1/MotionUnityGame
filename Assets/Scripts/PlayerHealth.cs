using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10; // Maksymalne zdrowie gracza
    private int currentHealth;

    public Slider healthBar;
    public GameManager gameManager;
    public ObjectSpawner ObjectSpawner;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            if (gameManager == null)
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
            if (ObjectSpawner == null)
            {
                Debug.Log("Zatrzymywanie spawnowania obiektów...");
                ObjectSpawner.Instance.StopSpawning();
            }
            gameManager.GameEnd();
        }
       
    }
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth /maxHealth;
        }
    }
}
