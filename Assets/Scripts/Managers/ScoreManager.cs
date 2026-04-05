using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int score { get; private set; }
    private int currentTurnCombo;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ResetCombo()
    {
        currentTurnCombo = 0;
    }

    public void AddScore(int basePoints)
    {
        currentTurnCombo++;
        score += basePoints * currentTurnCombo;
    }
}
