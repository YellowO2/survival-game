using System.Collections;
using UnityEngine;


public enum TurnPhase { Spawn, PlayerAim, WaitPhysicsSettle, Resolving }

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    public EnemyInstantiator enemyInstantiator;
    public TurnPhase phase { get; private set; }
    public PlayerBall player;
    private int maxEnemiesAllowed = 25;
    public int turnCount { get; private set; } = 0;
    public int turnDamage { get; private set; } = 0;
    public BallColor currentTurnPlayerColor { get; private set; }
    

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
        turnDamage = 0; // Reset turn damage for the new turn
        player.SwitchColor(); // alternate color every turn to add strategy
        currentTurnPlayerColor = player.currentColor;
        phase = TurnPhase.Spawn;

        if (turnCount == 1)
        {
            // Initial pool break setup
            enemyInstantiator.GeneratePoolTriangle(Vector3.zero); // Spawn triangle at center of arena
            
            // Move player to a typical break position (like the head string)
            PlayerBall player = FindFirstObjectByType<PlayerBall>();
            if (player != null)
            {
                player.transform.position = new Vector3(0, -5f, 0); // Position below the center
                if (player.rb != null)
                {
                    player.rb.linearVelocity = Vector2.zero;
                    player.rb.angularVelocity = 0f;
                }
            }
        }
        else
        {
            Debug.Log($"Spawning enemies for turn {turnCount}");
            enemyInstantiator.GenerateMultipleEnemies(3 + turnCount/5); // increase number every 5 turns.
        }
        Debug.Log($"Turn finish spawning.");
        int currentEnemies = FindObjectsByType<EnemyBall>(FindObjectsSortMode.None).Length;
        UIManager.Instance.UpdateTurnAndEnemyCount(currentEnemies, maxEnemiesAllowed, turnCount);
        UIManager.Instance.UpdateScore(ScoreManager.Instance.score);

        // Freeze non-matching colored balls so they act as solid obstacles
        EnemyBall[] allEnemies = FindObjectsByType<EnemyBall>(FindObjectsSortMode.None);
        foreach (EnemyBall enemy in allEnemies)
        {
            if (enemy != null && enemy.rb != null)
            {
                Collider2D col = enemy.GetComponent<Collider2D>();
                if (enemy.color != player.currentColor)
                {
                    enemy.rb.bodyType = RigidbodyType2D.Kinematic;
                    enemy.rb.linearVelocity = Vector2.zero;
                    enemy.rb.angularVelocity = 0f;
                    if (col != null) col.isTrigger = false;
                }
                else
                {
                    enemy.rb.bodyType = RigidbodyType2D.Dynamic;
                    if (col != null) col.isTrigger = true;
                }
            }
        }
        Debug.Log("change phase to player aim");
        phase = TurnPhase.PlayerAim;
    }
    public void OnShotFired()
    {
        if (phase != TurnPhase.PlayerAim) return;
        phase = TurnPhase.WaitPhysicsSettle;
        
        // Reset the combo at the start of every shot
        ScoreManager.Instance.ResetCombo();
        
        StartCoroutine(WaitForPhysicsToSettle());
    }

    public void RecordDamage(int amount)
    {
        turnDamage += amount;
        
        // Let the ScoreManager handle the combo math and adding the points
        ScoreManager.Instance.AddScore(amount);
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScore(ScoreManager.Instance.score);
        }
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
        // Must include Inactive objects, because objects like BombBall disable themselves (SetActive(false)) after exploding
        EnemyBall[] allEnemies = FindObjectsByType<EnemyBall>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (EnemyBall enemy in allEnemies)
        {
            if (enemy != null)
            {
                // Reset all balls back to dynamic and non-trigger before the next turn/spawns happen
                if (enemy.rb != null)
                {
                    enemy.rb.bodyType = RigidbodyType2D.Dynamic;
                }
                Collider2D col = enemy.GetComponent<Collider2D>();
                if (col != null)
                {
                    col.isTrigger = false;
                }

                if (enemy.hitpoints <= 0)
                {
                    enemy.Die(); // This will handle the destruction
                }
            }
        }
        
        Debug.Log("Before waiting...");
        yield return new WaitForEndOfFrame(); // this is because enemies take one frame to be destroyed so we need to wait.
        Debug.Log("After waiting...");
        // When counting remaining enemies, do NOT include inactive since they were destroyed/pending GC
        EnemyBall[] remainingEnemies = FindObjectsByType<EnemyBall>(FindObjectsSortMode.None);

        UIManager.Instance.UpdateTurnAndEnemyCount(remainingEnemies.Length, maxEnemiesAllowed, turnCount);
        
        Debug.Log($"checking turn result");  
        if (remainingEnemies.Length > maxEnemiesAllowed)
        {
            Debug.Log($"Before Game Over called.");
            GameManager.Instance.ChangeState(GameState.GameOver);
            Debug.Log($"Game Over called.");
        }
        else
        {
            Debug.Log($"Before StartTurn called.");
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