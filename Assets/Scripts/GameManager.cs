using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Canvas GameUI;
    public Canvas EndGameUI;

    void Start()
    {
        GameUI.gameObject.SetActive(true);
        EndGameUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void GameEnd()
    {
        GameUI.gameObject.SetActive(false);
        EndGameUI.gameObject.SetActive(true);   
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuTitle");
    }
}
