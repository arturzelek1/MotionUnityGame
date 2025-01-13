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
    }
    public void PlayGame()
    {
        if(menuCanvas != null && selectGameCanvas != null)
        {
            menuCanvas.gameObject.SetActive(false);
            selectGameCanvas.gameObject.SetActive(true);
        }
    }

    public void PlayOnePlayerGame()
    {
        if (OnePlayerScene != null)
        {
            SceneManager.LoadScene("OnePlayerScene");
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
