using UnityEngine;
//Takes damage when enemy enters this zone, which is at the bottom of the screen. 
// The damage taken is the remaining hitpoints of that enemy
public class DamageZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            int damage = enemy.hitpoints;

            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.TakeDamage(damage);

            //Destroy the enemy that entered the damage zone
            Destroy(collision.gameObject);
        }
    }
}
