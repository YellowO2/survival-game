using System.Collections;
using UnityEngine;


public enum TurnPhase { Spawn, PlayerAim, WaitPhysicsSettle, Resolving }
public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    public EnemyInstantiator enemyInstantiator;
    public TurnPhase phase { get; private set; }
    private int maxEnemiesAllowed = 5;
    public int turnCount { get; private set; } = 0;


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
        StartTurn();
    }

    void StartTurn()
    {
        turnCount++;
        phase = TurnPhase.Spawn;
        enemyInstantiator.GenerateMultipleEnemies(2);
        int currentEnemies = FindObjectsByType<EnemyBall>(FindObjectsSortMode.None).Length;
        UIManager.Instance.UpdateTurnAndEnemyCount(currentEnemies, maxEnemiesAllowed, turnCount);
        phase = TurnPhase.PlayerAim;
    }
    public void OnShotFired()
    {
        if (phase != TurnPhase.PlayerAim) return;
        phase = TurnPhase.WaitPhysicsSettle;
        StartCoroutine(WaitForPhysicsToSettle());
    }
    public void OnPhysicsSettled()
    {
        Debug.Log("Physics settled, starting resolve phase.");
        if (phase != TurnPhase.WaitPhysicsSettle) return;
        phase = TurnPhase.Resolving;
        StartCoroutine(ResolvePhaseRoutine());
    }

    private IEnumerator ResolvePhaseRoutine()
    {
        EnemyBall[] allEnemies = FindObjectsByType<EnemyBall>(FindObjectsSortMode.None);

        foreach (EnemyBall enemy in allEnemies)
        {
            if (enemy != null && enemy.hitpoints <= 0)
            {
                enemy.Die(); // This will handle the destruction
            }
        }

        yield return new WaitForEndOfFrame(); // this is because enemies take one frame to be destroyed so we need to wait.

        EnemyBall[] remainingEnemies = FindObjectsByType<EnemyBall>(FindObjectsSortMode.None);

        UIManager.Instance.UpdateTurnAndEnemyCount(remainingEnemies.Length, maxEnemiesAllowed, turnCount);

        if (remainingEnemies.Length > maxEnemiesAllowed)
        {
            GameManager.Instance.ChangeState(GameState.GameOver);
        }
        else
        {
            StartTurn();
        }
    }

    private IEnumerator WaitForPhysicsToSettle()
    {
        yield return new WaitForSeconds(1f);

        // Find all balls currently in the game
        BaseBall[] allBalls = FindObjectsByType<BaseBall>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        bool isSettled = false;

        while (!isSettled)
        {
            isSettled = true;
            foreach (BaseBall ball in allBalls)
            {
                if (ball.IsBusy())
                {
                    isSettled = false;
                    break; // No need to check the rest this frame
                }
            }
            yield return null;
        }
        OnPhysicsSettled();
    }


}