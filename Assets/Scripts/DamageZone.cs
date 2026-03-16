using UnityEngine;
//Takes damage when enemy enters this zone, which is at the bottom of the screen. 
// The damage taken is the remaining hitpoints of that enemy
public class DamageZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyBall enemy = collision.gameObject.GetComponent<EnemyBall>();
            int damage = enemy.hitpoints;

            PlayerBall player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBall>();
            // player.TakeDamage(damage);

            //Destroy the enemy that entered the damage zone
            // Destroy(collision.gameObject);
        }
    }
}
