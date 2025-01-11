using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerFor2Players : MonoBehaviour
{
    //Singleton to have global acces to that variable
    public static ObjectSpawnerFor2Players Instance { get; private set; }

    public GameObject objectToSpawn;
    public Transform[] spawnPoints;
    public bool isSpawning;

    //Checking if this is first instance of object spawner
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            InvokeRepeating("SpawnObject", 1f, 2f);
        }
    }

    // Update is called once per frame
    void SpawnObject()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
    }
}
