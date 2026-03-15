using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
//Manages the game state, such as starting and ending the game, keeping track of the score, and handling game over conditions.
//Currently it keep tracks of total survival time, which influnces strength of the enemies
// The score is simply the original max hitpoint of enemy defeated, which is added to the score
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState { Playing, GameOver, Paused }
    public GameState currentState { get; private set; }
    //Properties of the game manager
    public TMPro.TextMeshProUGUI bestTimeText;
    public GameObject gameOverScreen;
    public GameObject inGameMenu;

    void Awake()
    {
        print("GameManager Awake Ran");
        bestTimeText.text = "Best: " + PlayerPrefs.GetFloat("BestSurvivalTime", 0f).ToString("F2") + "s";
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
    }

    // Update is called once per frame
    void Update()
    {
        bestTimeText.text = "Best: " + PlayerPrefs.GetFloat("BestSurvivalTime", 0f).ToString("F2") + "s";
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
                bestTimeText.text = "Best: " + PlayerPrefs.GetFloat("BestSurvivalTime", 0f).ToString("F2") + "s";
                break;
            case GameState.GameOver:
                //save the score to player prefs if higher than the previous score
                float previousBest = PlayerPrefs.GetFloat("BestSurvivalTime", 0f);
                Time.timeScale = 0f;
                gameOverScreen.GetComponent<GameOverMenu>().SetUp(69, PlayerPrefs.GetFloat("BestSurvivalTime", 0f));
                gameOverScreen.SetActive(true);

                break;
            case GameState.Paused:
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
}
