using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyThreshold : MonoBehaviour
{
    public float thresholdZ = 2.0f; // Próg na osi Z
    public int damage = 1; // Ilość punktów zdrowia do odjęcia
    public float detectionRadius = 2.0f;


    void Update()
    {
        
        if (transform.position.z < 9)
        {
            // Znajdź gracza i zadaj mu obrażenia
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject player2 = GameObject.FindGameObjectWithTag("Player2");

            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                Debug.Log("Odległość do gracza 1: " + distance);
                if (distance < 2.8)
                {
                    Debug.Log("Zadano damage graczowi 1");
                    Scene currentScene = SceneManager.GetActiveScene();
                    string sceneName = currentScene.name;

                    if (sceneName == "OnePlayerScene")
                    {
                        player.GetComponent<PlayerHealth>().TakeDamage(damage);
                    }
                    else if (sceneName == "TwoPlayersScene")
                    {
                        player.GetComponent<PlayerHealth2>().TakeDamage(damage);
                    }

                }
            }

                if (player2 != null)
                {
                    float distance = Vector3.Distance(transform.position, player2.transform.position);
                    Debug.Log("Odległość do gracza 2: " + distance);
                    if (distance < 2.8)
                    {
                        Scene currentScene = SceneManager.GetActiveScene();
                        string sceneName = currentScene.name;
                        Debug.Log("Zadano damage graczowi 2");
                        if (sceneName == "TwoPlayersScene")
                        {
                            player2.GetComponent<PlayerHealth2>().TakeDamage(damage);
                        }
                    }
                }

                Destroy(gameObject);
            
        }
    }
}
