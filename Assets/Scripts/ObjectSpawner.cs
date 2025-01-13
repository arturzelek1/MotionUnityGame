using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectSpawner : MonoBehaviour
{
    //Singleton to have global acces to that variable
    public static ObjectSpawner Instance {  get; private set; }

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

    private void OnEnable()
    {
        // Subskrypcja eventu ładowania sceny
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Odsubskrybowanie eventu
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Sprawdzamy nazwę sceny lub inne warunki
        if (scene.name == "OnePlayerScene")
        {
            Debug.Log($"Scene loaded: {scene.name}. Starting spawning...");
            StartSpawning();
        }
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

    public void StopSpawning()
    {
        if (isSpawning)
        {
            Debug.Log("StopSpawning wywołane");
            isSpawning = false;
            CancelInvoke("SpawnObject");
        }
        else
        {
            Debug.Log("Spawning już zatrzymane");
        }
    }
}
