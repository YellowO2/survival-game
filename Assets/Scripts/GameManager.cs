using System;
using UnityEngine;
//Manages the game state, such as starting and ending the game, keeping track of the score, and handling game over conditions.
//Currently it keep tracks of total survival time, which influnces strength of the enemies
// The score is simply the original max hitpoint of enemy defeated, which is added to the score
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    //Properties of the game manager
    public float survivalTime = 0f;
    public int score = 0;
    public TMPro.TextMeshProUGUI scoreText;

    void Awake()
    {
        print("GameManager Awake Ran");
        scoreText.text = "Score: " + score;
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        survivalTime += Time.deltaTime;
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }
}
