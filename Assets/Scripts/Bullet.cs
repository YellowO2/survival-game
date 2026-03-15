using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Properties of a bullet
    public float speed = 1f;
    public float damage = 1f;
    public float lifetime = 2f;
    private Rigidbody2D rb;

    public void SetUp(float damage, float speed, float size, float lifetime)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        this.transform.localScale = new Vector3(size, size, size);
        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; //this is to prevent bullets from passing through thin walls at high speed
        rb.linearVelocity = transform.up * speed;
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Enemy"))
    //     {
    //         // Handle damage to the enemy here
    //         Enemy enemy = other.GetComponent<Enemy>();
    //         if (enemy != null)
    //         {
    //             enemy.TakeDamage((int)damage);
    //         }
    //         Destroy(gameObject); // Destroy the bullet after hitting an enemy
    //     }
    //     else if (other.CompareTag("Wall"))
    //     {
    //         print("Bullet collided with wall!");
    //         if (bounceAvailable > 0)
    //         {
    //             bounceAvailable--;
    //             // Reflect the bullet's direction based on the collision normal
    //             Vector2 normal = other.ClosestPoint(transform.position) - (Vector2)transform.position;
    //             Vector2 newDirection = Vector2.Reflect(transform.up, normal.normalized);
    //             float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg - 90f;
    //             transform.rotation = Quaternion.Euler(0, 0, angle);
    //         }
    //         else
    //         {
    //             Destroy(gameObject); // Destroy the bullet if no bounces are left
    //         }
    //     }
    // }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("Bullet collided with enemy!");
            // Handle damage to the enemy here
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)damage);
            }
            TurnManager.Instance.OnPhysicsSettled();
            // Destroy(gameObject); // Destroy the bullet after hitting an enemy
        }
    }
}
