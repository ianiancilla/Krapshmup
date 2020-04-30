using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // configuration variables
    [SerializeField] float gameOverDelay = 1f;
    [SerializeField] KeyCode restartKey = KeyCode.Escape;

    // variables
    string startScene = "Start";
    string gameScene = "Game";
    string gameOverScene = "Game Over";
    string controlsScene = "Controls";

    private void Update()
    {
        RestartOnPress();
    }

    private void RestartOnPress()
    {
        if (Input.GetKeyDown(restartKey))
        {
            LoadStart();
        }
    }

    public void LoadStart()
    {
        if (FindObjectsOfType<GameSession>().Length > 0)
        {
            FindObjectOfType<GameSession>().ResetGame();
        }
        SceneManager.LoadScene(startScene);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(gameScene);
    }
    public void LoadGameOver()
    {
        StartCoroutine(GameOver());
    }
    public void LoadControls()
    {
        SceneManager.LoadScene(controlsScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(gameOverDelay);
        SceneManager.LoadScene(gameOverScene);
    }

}
