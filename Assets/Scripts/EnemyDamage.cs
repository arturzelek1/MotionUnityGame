using UnityEngine;

public class EnemyThreshold : MonoBehaviour
{
    public float thresholdZ = 1.0f; // Próg na osi Z
    public int damage = 1; // Ilość punktów zdrowia do odjęcia

    void Update()
    {
        if (transform.position.z < 9)
        {
            // Znajdź gracza i zadaj mu obrażenia
            GameObject player = GameObject.FindWithTag("Player");
            
            if (player != null)
            {
                Debug.Log("Zadano damage");
                
                player.GetComponent<PlayerHealth>().TakeDamage(damage);
                
            }

            Destroy(gameObject);
            
        }
    }
}
