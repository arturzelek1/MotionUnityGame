using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObject", 1f, 2f);
    }

    // Update is called once per frame
    void SpawnObject()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
    }
}
