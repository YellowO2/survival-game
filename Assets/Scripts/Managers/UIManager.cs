using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    public TextMeshProUGUI hitPointText;
    public TextMeshProUGUI bestTimeText;
    public TextMeshProUGUI enemiesCountText;

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