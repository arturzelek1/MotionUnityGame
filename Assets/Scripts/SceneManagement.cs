using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public Canvas menuCanvas;
    public Canvas gameCanvas;
    public ObjectSpawner objectSpawner;

    private void Start()
    {
        gameCanvas.gameObject.SetActive(false);

        if (objectSpawner != null)
        {
            objectSpawner.isSpawning = false;
        }
    }
    public void PlayGame()
    {
        if (menuCanvas != null && gameCanvas != null)
        {
            menuCanvas.gameObject.SetActive(false);
            gameCanvas.gameObject.SetActive(true);

            if (objectSpawner != null)
            {
                objectSpawner.StartSpawning();
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
