using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player; // Cel (gracz)
    public float speed = 1f; // Prędkość ruchu

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Kierunek do gracza
            Vector3 direction = new Vector3(0, 0, player.position.z - transform.position.z).normalized;

            // Porusz przeciwnika
            transform.position += direction * speed * Time.deltaTime;

            // Obróć przeciwnika w stronę gracza
            //transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }
}
