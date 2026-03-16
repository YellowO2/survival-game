
using UnityEngine;
//Controls where and when to spawn enemies, as well as the strength of the enemies based on the player's survival time. The spawn rate and enemy strength can be adjusted to create a challenging gameplay experience that scales with the player's progress.
public class EnemyInstantiator : MonoBehaviour
{
    // Enemy prefab to instantiate
    [Header("Prefabs")]
    public GameObject enemyPrefab;
    public GameObject bombPrefab;
    private int maxAttempts = 15; // Maximum attempts to find a valid spawn position
    float spawnAreaLength = 10f; //length of each side of the square spawn area
    float enemyRadius = 1;

    private int GenerateEnemyHitpoints()
    {
        int hitpoints = Random.Range(1, 3 + Mathf.FloorToInt(TurnManager.Instance.turnCount / 5)); // Increase max hitpoints every 5 turns
        return hitpoints;
    }

    public void GenerateMultipleEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GenerateEnemy();
        }
    }

    public void GenerateEnemy()
    {
        int colorIndex = Random.Range(0, 4);
        UnityEngine.Vector3 spawnPosition = GetSpawnPosition(enemyRadius);

        // Decide which prefab to spawn based on probability
        GameObject prefabToSpawn = enemyPrefab;
        if (Random.value <= 0.2 && bombPrefab != null)
        {
            prefabToSpawn = bombPrefab;
        }
        EnemyBall enemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity).GetComponent<EnemyBall>();
        enemy.SetUp(GenerateEnemyHitpoints(), ColorFromIndex(colorIndex));
    }
    private static readonly BallColor[] AllColors =
        (BallColor[])System.Enum.GetValues(typeof(BallColor));

    public static BallColor ColorFromIndex(int index)
    {
        // Wrap index so any int works (negative/large)
        int safeIndex = ((index % AllColors.Length) + AllColors.Length) % AllColors.Length;
        return AllColors[safeIndex];
    }

    public Vector3 GetSpawnPosition(float enemyRadius)
    {
        float halfLength = spawnAreaLength / 2f;
        for (int i = 0; i < maxAttempts; i++)
        {
            // pick random spot
            float randomX = Random.Range(-halfLength, halfLength);
            float randomY = Random.Range(-halfLength, halfLength);
            Vector2 potentialPoint = new Vector2(randomX, randomY);

            // check if nothing will collide if spawned there
            Collider2D overlappingCollider = Physics2D.OverlapCircle(potentialPoint, enemyRadius + 0.4f); //add abit more radius as buffer

            if (overlappingCollider == null)
            {
                // We found an empty spot!
                return potentialPoint;
            }
        }

        print("Failed to find a valid spawn position after " + maxAttempts + " attempts.");
        return Vector2.zero;
    }
}
