using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollision2P: MonoBehaviour
{
    public string playerTag;
    public int pointsPerHit = 10;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (playerTag == "Player")
            {
                ScoreCountFor1Player.Instance.AddScore("Player",pointsPerHit);
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

