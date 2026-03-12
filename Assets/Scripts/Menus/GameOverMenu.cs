using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Button restartButton;
    public TextMeshProUGUI finalTimeText;
    public TextMeshProUGUI bestTimeText;
    
    void Start()
    {
        restartButton.onClick.AddListener(restartGame);
    }

    void restartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void SetUp(float finalTime, float bestTime)
    {
        finalTimeText.text = "Final: " + finalTime.ToString("F2") + "s";
        bestTimeText.text = "Best: " + bestTime.ToString("F2") + "s";
    }
}
