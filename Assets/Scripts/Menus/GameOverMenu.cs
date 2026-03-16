using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Button restartButton;
    public TextMeshProUGUI BestText;
    public TextMeshProUGUI CurrentText;
    
    void Start()
    {
        restartButton.onClick.AddListener(restartGame);
    }

    void restartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void SetUp(int survivedTurns, int bestTurns)
    {
        CurrentText.text = "You Survived " + survivedTurns + " Turns!";
    }
}
