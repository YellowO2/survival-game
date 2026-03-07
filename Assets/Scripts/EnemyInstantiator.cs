using Unity.VisualScripting;
using UnityEngine;
//Is at the top of the portrait mobile screen. Instantiates the enemy prefab at random x positions at the top of the screen, and the enemies will move downwards towards the player.
public class EnemyInstantiator : MonoBehaviour
{
    // Enemy prefab to instantiate
    public GameObject enemyPrefab;
    public float spawnRate = 3f;
    private float timeSinceLastSpawn = 0f;

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
        int hitpoints = Mathf.FloorToInt(survivalTime / 10f) + Random.Range(1, 10); // Increase hitpoints every 10 seconds
        return hitpoints;
    }

    private void GenerateEnemy()
    {
        float width = transform.localScale.x;
        // Instantiate enemy at random x position at the top of the screen
        float randomX = Random.Range(transform.position.x - width / 2f, transform.position.x + width / 2f);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, 0f); // Assuming screen height is 6 units
        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
        enemy.SetUp(GenerateEnemyHitpoints(), Random.Range(0.5f, 2f)); // Set up enemy with random hitpoints and speed
    }

    // private void generateNewSpawnRate()
    // {
    //     // Decrease spawn rate based on survival time
    //     float survivalTime = GameManager.Instance.survivalTime;
    //     spawnRate = Mathf.Max(0.1f, 1f - survivalTime / 60f); // Decrease spawn rate every 60 seconds, with a minimum of 0.1 seconds
    // }
}
