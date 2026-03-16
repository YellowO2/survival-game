using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    public TextMeshProUGUI hitPointText;
    public TextMeshProUGUI bestTimeText;
    public TextMeshProUGUI enemiesCountText;
    public TextMeshProUGUI scoreText;

    [Header("Menus")]
    public GameOverMenu gameOverScreen;
    public GameObject inGameMenu;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        UpdatePlayerHealth(3); // Assuming player starts with 3 health
        UpdateBestTime(0); // Assuming no best time at start
        UpdateTurnAndEnemyCount(0, 5, 0); // Assuming starting with 0 enemies and 0 turn
        UpdateScore(0); // Assuming starting with 0 score
    }

    public void UpdatePlayerHealth(int currentHealth)
    {
        if (hitPointText != null)
        {
            hitPointText.text = currentHealth.ToString();
        }
    }

    public void UpdateBestTime(int bestTurn)
    {
        if (bestTimeText != null)
        {
            // F2 formatting for int is slightly unconventional but matching original code
            bestTimeText.text = "Best: " + bestTurn.ToString("F2") + "s";
        }
    }

    public void UpdateTurnAndEnemyCount(int enemyCount, int maxEnemies, int turn)
    {
        if (enemiesCountText != null)
        {
            enemiesCountText.text = "Enemies: " + enemyCount + "/" + maxEnemies + "\nTurn: " + turn;
        }
    }

    public void UpdateScore(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + newScore.ToString();
        }
    }

    public void ShowGameOver(int turnCount, int bestTurnCount)
    {
        HideAllMenus();
        if (gameOverScreen != null)
        {
            gameOverScreen.SetUp(turnCount, bestTurnCount);
            gameOverScreen.gameObject.SetActive(true);
        }
    }

    public void ToggleInGameMenu(bool show)
    {
        if (inGameMenu != null)
        {
            inGameMenu.SetActive(show);
        }
    }

    public void HideAllMenus()
    {
        if (gameOverScreen != null) gameOverScreen.gameObject.SetActive(false);
        if (inGameMenu != null) inGameMenu.SetActive(false);
    }
}