using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
//Controls where and when to spawn enemies, as well as the strength of the enemies based on the player's survival time. The spawn rate and enemy strength can be adjusted to create a challenging gameplay experience that scales with the player's progress.
public class EnemyInstantiator : MonoBehaviour
{
    // Enemy prefab to instantiate
    public GameObject enemyPrefab;
    public float spawnRate = 3f;
    private float timeSinceLastSpawn = 0f;
    private float baseEnemySpeed = 1f;
    private float enemySpeedIncreaseRate = 0.5f; // Increase enemy speed by 0.1 every 5 seconds

     void Start()
    {
        // Optionally, you can initialize any necessary variables or settings here
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnRate)
        {
            GenerateEnemy();
            timeSinceLastSpawn = 0f;
        }
    }

    private int GenerateEnemyHitpoints()
    {
        // Generate hitpoints based on survival time
        float survivalTime = GameManager.Instance.survivalTime;
        int hitpoints = Mathf.FloorToInt(survivalTime / 20f) + Random.Range(1, 4); // Increase hitpoints every 10 seconds
        return hitpoints;
    }

    private void GenerateEnemy()
    {
        float width = 8f;
        float halfWidth = width / 2f;

        // Randomly select one of the four edges (0=top, 1=bottom, 2=left, 3=right)
        int edge = Random.Range(0, 4);
        UnityEngine.Vector3 spawnPosition;

        switch (edge)
        {
            case 0: // Top edge
                spawnPosition = new UnityEngine.Vector3(
                    Random.Range(-halfWidth, halfWidth),
                    halfWidth,
                    0f);
                break;
            case 1: // Bottom edge
                spawnPosition = new UnityEngine.Vector3(
                    Random.Range(-halfWidth, halfWidth),
                    -halfWidth,
                    0f);
                break;
            case 2: // Left edge
                spawnPosition = new UnityEngine.Vector3(
                    -halfWidth,
                    Random.Range(-halfWidth, halfWidth),
                    0f);
                break;
            default: // Right edge
                spawnPosition = new UnityEngine.Vector3(
                    halfWidth,
                    Random.Range(-halfWidth, halfWidth),
                    0f);
                break;
        }

        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, UnityEngine.Quaternion.identity).GetComponent<Enemy>();
        enemy.SetUp(GenerateEnemyHitpoints(), baseEnemySpeed + enemySpeedIncreaseRate * (GameManager.Instance.survivalTime / 5f)); // Increase speed every 5 seconds
    }

    // private void generateNewSpawnRate()
    // {
    //     // Decrease spawn rate based on survival time
    //     float survivalTime = GameManager.Instance.survivalTime;
    //     spawnRate = Mathf.Max(0.1f, 1f - survivalTime / 60f); // Decrease spawn rate every 60 seconds, with a minimum of 0.1 seconds
    // }
}
