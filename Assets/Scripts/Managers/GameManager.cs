using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
//Manages the game state, such as starting and ending the game, keeping track of the score, and handling game over conditions.
//Currently it keep tracks of total survival time, which influnces strength of the enemies
// The score is simply the original max hitpoint of enemy defeated, which is added to the score
public enum GameState { Playing, GameOver, Paused }
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState currentState { get; private set; }

    void Awake()
    {
        print("GameManager Awake Ran");
        if (Instance != null && Instance != this)
        {
            print("GameManager Instance already exists, destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        print("GameManager Start Ran");
        UpdateBestTimeDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBestTimeDisplay();
    }

    private void UpdateBestTimeDisplay()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateBestTime(PlayerPrefs.GetInt("BestTurn", 0));
        }
    }

    public void ChangeState(GameState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.HideAllMenus();
        }

        switch (currentState)
        {
            case GameState.Playing:
                UpdateBestTimeDisplay();
                break;
            case GameState.GameOver:
                //save the score to player prefs if higher than the previous score
                int previousBest = PlayerPrefs.GetInt("BestTurn", 0);
                if (TurnManager.Instance.turnCount > previousBest)
                {
                    PlayerPrefs.SetInt("BestTurn", TurnManager.Instance.turnCount);
                }
                Time.timeScale = 0f;
                UIManager.Instance.ShowGameOver(TurnManager.Instance.turnCount, PlayerPrefs.GetInt("BestTurn", 0));
                break;
            case GameState.Paused:
                Time.timeScale = 0f; // Pause the game
                break;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
