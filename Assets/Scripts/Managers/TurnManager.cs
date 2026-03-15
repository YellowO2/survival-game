using Unity.VisualScripting;
using UnityEngine;

public enum TurnPhase { Spawn, PlayerAim, WaitPhysicsSettle, Resolving }
public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    public EnemyInstantiator enemyInstantiator;
    public TurnPhase phase { get; private set; }
    int turnIndex = 0;
    bool shotTaken = false;


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
        turnIndex++;
        shotTaken = false;
        phase = TurnPhase.Spawn;
        enemyInstantiator.GenerateMultipleEnemies(3);
        phase = TurnPhase.PlayerAim;
    }
    public void OnShotFired()
    {
        if (phase != TurnPhase.PlayerAim) return;
        shotTaken = true;
        phase = TurnPhase.WaitPhysicsSettle;
    }
    public void OnPhysicsSettled()
    {
        if (phase != TurnPhase.WaitPhysicsSettle) return;
        phase = TurnPhase.Resolving;
       
    }

    
}