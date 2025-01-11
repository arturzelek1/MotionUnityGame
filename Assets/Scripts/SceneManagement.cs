using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public Canvas menuCanvas;
    public Canvas selectGameCanvas;
    public Scene OnePlayerScene;
    public Scene TwoPlayersScene;

    private void Start()
    {
        selectGameCanvas.gameObject.SetActive(false);

        /*if (objectSpawner != null)
        {
            objectSpawner.isSpawning = false;
        }*/
    }
    public void PlayGame()
    {
        if(menuCanvas != null && selectGameCanvas != null)
        {
            menuCanvas.gameObject.SetActive(false);
            selectGameCanvas.gameObject.SetActive(true);
        }
        /*if(OnePlayerScene != null) 
        {
            //menuCanvas.gameObject.SetActive(false);
            //gameCanvas.gameObject.SetActive(true);
            SceneManager.LoadScene("OnePlayerScene");
           /* if (objectSpawner != null)
            {
                objectSpawner.StartSpawning();
            }
        }*/
    }

    public void PlayOnePlayerGame()
    {
        if (OnePlayerScene != null)
        {
            SceneManager.LoadScene("OnePlayerScene");
            if (ObjectSpawner.Instance != null)
            {
                ObjectSpawner.Instance.StartSpawning();
            }
            else
            {
                Debug.Log("Singleton wrong initialized");
            }
        }
    }

    public void PlayTwoPlayersGame() 
    {
        if (TwoPlayersScene != null)
        {
            SceneManager.LoadScene("TwoPlayersScene");
            if(ObjectSpawnerFor2Players.Instance != null)
            {
                ObjectSpawnerFor2Players.Instance.StartSpawning();
            }
            else
            {
                Debug.Log("Singleton wrong initialized");
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
