using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int playerScore = 0;

    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        int gameSessionCount = FindObjectsOfType<GameSession>().Length;

        if (gameSessionCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public int GetScore()
    {
        return playerScore;
    }

    public void AddToScore(int scoreValue)
    {
        playerScore += scoreValue;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
