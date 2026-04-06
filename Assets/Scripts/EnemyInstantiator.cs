
using UnityEngine;
//Controls where and when to spawn enemies, as well as the strength of the enemies based on the player's survival time. The spawn rate and enemy strength can be adjusted to create a challenging gameplay experience that scales with the player's progress.
public class EnemyInstantiator : MonoBehaviour
{
    // Enemy prefab to instantiate
    [Header("Prefabs")]
    public GameObject enemyPrefab;
    public GameObject bombPrefab;
    public GameObject spikePrefab;
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
        // Pick a dominant color for this spawn cluster to encourage chains
        int dominantColorIndex = Random.Range(0, 4);

        // Pick a random central point for this cluster
        float halfLength = spawnAreaLength / 2f;
        Vector2 clusterCenter = new Vector2(Random.Range(-halfLength, halfLength), Random.Range(-halfLength, halfLength));
        float clusterRadius = Mathf.Max(3f, count * enemyRadius); 

        for (int i = 0; i < count; i++)
        {
            // 75% chance to use the dominant color, 25% chance for a random color
            int colorIndex = (Random.value < 0.75f) ? dominantColorIndex : Random.Range(0, 4);
            
            Vector3 spawnPos = GetSpawnPositionNear(clusterCenter, clusterRadius, enemyRadius);
            // Fallback to anywhere on the board if the cluster is too crowded
            if (spawnPos == Vector3.zero)
            {
                spawnPos = GetSpawnPosition(enemyRadius);
            }

            GenerateEnemy(spawnPos, colorIndex);
        }
    }

    public void GeneratePoolTriangle(Vector3 center)
    {
        int rows = 3; // 1+2+3 = 6 enemies
        // Using enemyRadius for spacing calculation
        float ballSpacing = enemyRadius * 1.5f; 

        // Offset to center the triangle cluster around `center` position 
        float yStartOffset = -((rows - 1) * ballSpacing * 0.866f) / 3f; // Optional center offset

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col <= row; col++)
            {
                float xOffset = (col - (row / 2f)) * ballSpacing;
                float yOffset = row * ballSpacing * 0.866f + yStartOffset; 

                Vector3 spawnPosition = center + new Vector3(xOffset, yOffset, 0);

                int colorIndex = Random.Range(0, 4);
                
                // we will stick to mostly regular enemies for the break opening.
                EnemyBall enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<EnemyBall>();
                enemy.SetUp(GenerateEnemyHitpoints(), ColorFromIndex(colorIndex));
            }
        }
    }

    public void GenerateEnemy(Vector3 spawnPosition, int predeterminedColorIndex = -1)
    {
        int colorIndex = predeterminedColorIndex >= 0 ? predeterminedColorIndex : Random.Range(0, 4);

        // Decide which prefab to spawn based on probability
        GameObject prefabToSpawn = enemyPrefab;
        if (Random.value <= 0.2 && bombPrefab != null)
        {
            prefabToSpawn = bombPrefab;
        }
        else if (Random.value <= 0.4 && spikePrefab != null)
        {
            prefabToSpawn = spikePrefab;
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

    public Vector3 GetSpawnPositionNear(Vector2 clusterCenter, float clusterRadius, float enemyRadius)
    {
        float halfLength = spawnAreaLength / 2f;
        for (int i = 0; i < maxAttempts; i++)
        {
            // Pick a random spot within the cluster radius
            Vector2 randomOffset = Random.insideUnitCircle * clusterRadius;
            Vector2 potentialPoint = clusterCenter + randomOffset;
            
            // Constrain it inside the spawn area bounds
            potentialPoint.x = Mathf.Clamp(potentialPoint.x, -halfLength, halfLength);
            potentialPoint.y = Mathf.Clamp(potentialPoint.y, -halfLength, halfLength);

            // check if nothing will collide if spawned there
            Collider2D overlappingCollider = Physics2D.OverlapCircle(potentialPoint, enemyRadius + 0.4f);

            if (overlappingCollider == null)
            {
                // We found an empty spot!
                return potentialPoint;
            }
        }
        
        return Vector3.zero; // Return zero vector if no spot is found near
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
