using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + GameManager.Instance.survivalTime.ToString("F2") + "s";
    }
}
