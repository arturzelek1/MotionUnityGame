using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public string playerTag;
    public int pointsPerHit = 10;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            if (playerTag == "Player")
            {
                ScoreCount.Instance.AddScore("Player",pointsPerHit);
                Destroy(other.gameObject);
            }
            else if (playerTag == "Player2")
            {
                ScoreCount.Instance.AddScore("Player2", pointsPerHit);
                Destroy(other.gameObject);
            }
        }
    }
}

