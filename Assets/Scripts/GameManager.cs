using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Canvas GameUI;
    public Canvas EndGameUI;
    // Start is called before the first frame update
    void Start()
    {
        GameUI.gameObject.SetActive(true);
        EndGameUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void GameEnd()
    {
        GameUI.gameObject.SetActive(false);
        EndGameUI.gameObject.SetActive(true);
    }
}
