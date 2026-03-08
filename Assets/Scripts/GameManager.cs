using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
//Manages the game state, such as starting and ending the game, keeping track of the score, and handling game over conditions.
//Currently it keep tracks of total survival time, which influnces strength of the enemies
// The score is simply the original max hitpoint of enemy defeated, which is added to the score
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState { Playing, GameOver, Paused }
    public GameState currentState { get; private set; }
    //Properties of the game manager
    public float survivalTime = 0f;
    public int score = 0;
    public TMPro.TextMeshProUGUI scoreText;
    public GameObject gameOverScreen;
    public GameObject inGameMenu;

    void Awake()
    {
        print("GameManager Awake Ran");
        scoreText.text = "Score: " + score;
        if (Instance != null && Instance != this)
        {
            print("GameManager Instance already exists, destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        survivalTime += Time.deltaTime;
    }


    public  void ChangeState (GameState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;
        HideAllMenus();
        switch (currentState)
        {
            case GameState.Playing:
                // Handle playing state (e.g., resume game, enable player controls, etc.)
                break;
            case GameState.GameOver:
                // Handle game over state (e.g., show game over screen, disable player controls, etc.)
                Time.timeScale = 0f;
                gameOverScreen.SetActive(true);

                break;
            case GameState.Paused:
                // Handle paused state (e.g., show pause menu, disable player controls, etc.)
                Time.timeScale = 0f; // Pause the game
                break;
        }
    }

    private void HideAllMenus()
    {
        gameOverScreen.SetActive(false);
        inGameMenu.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }
}
