using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Button restartButton;
    
    void Start()
    {
        restartButton.onClick.AddListener(restartGame);
    }

    void restartGame()
    {
        GameManager.Instance.RestartGame();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
