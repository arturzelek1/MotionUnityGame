using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10; // Maksymalne zdrowie gracza
    private int currentHealth;

    public Slider healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            // Logika końca gry lub restartu sceny
            Debug.Log("Gracz zginął!");
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
