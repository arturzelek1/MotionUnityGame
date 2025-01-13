using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCount : MonoBehaviour
{
    public static ScoreCount Instance;

    private int Player1Score = 0;
    private int Player2Score = 0;

    public TMP_Text player1ScoreText;
    public TMP_Text player2ScoreText;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        { 
            Destroy(gameObject);
        }
    }

    public void AddScore(string playerTag,int points)
    {
        if(playerTag == "Player")
        {
            Player1Score += points;
        }
        else if(playerTag == "Player2")
        {
            Player2Score += points;
        }
        UpdateScoreDisplay();
    }
   
    public int GetScore(string playerTag)
    {
        if (playerTag == "Player")
        {
            Debug.Log(GetScore(playerTag));
            return Player1Score;
        }
        else if (playerTag == "Player2")
        {
            Debug.Log(GetScore(playerTag));
            return Player2Score;
        }
        return 0;
    }

    public void ResetScore()
    {
        Player2Score = 0;
        Player1Score = 0;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        player1ScoreText.text = "Player 1: " + Player1Score.ToString();
        player2ScoreText.text = "Player 2: " + Player2Score.ToString();
    }
}
