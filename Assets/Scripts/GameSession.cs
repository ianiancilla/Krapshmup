using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    // variables
    int playerScore = 0;

    private void Awake()
    {
        SetUpSingleton();        
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore() { return playerScore; }
    public void AddToScore(int score)
    {
        playerScore += score;
    }
    public void SubtractFromScore(int score)
    {
        playerScore -= score;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

}
