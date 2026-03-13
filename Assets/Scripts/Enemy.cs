using UnityEditor.Callbacks;
using UnityEngine;
// The enemies are what the player has to shoot off. They carry a number that we can see as hitpoints 
// The enemies will simple move downwards, i.e. towards the player, and the player has to shoot them before they reach the player. If they reach the player, the game is over.
public class Enemy : MonoBehaviour
{
    //Properties of an enemy
    private float speed;
    public int hitpoints = 1; // the enemy size scales with hitpoints, the bigger the hitpoints, the bigger the enemy
    // text mesh pro to display hitpoints
    public int originalHitpoints; // the original hitpoints of the enemy, which is used to calculate score when the enemy is defeated
    public TMPro.TextMeshPro hitpointsText;
    private Rigidbody2D rb;
    private float size;

    void InitialiseMovment()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.linearVelocity = randomDirection * speed;
    }

    public void SetUp(int hitpoints, float speed)
    {
        this.hitpoints = hitpoints;
        this.speed = speed;
        this.originalHitpoints = hitpoints;
        hitpointsText.text = hitpoints.ToString();
        // scale the enemy with hitpoints
        size = hitpoints*0.5f; // Adjust the exponent and multiplier to get the desired scaling effect
        transform.localScale = new Vector3(Mathf.Sqrt(size), Mathf.Sqrt(size), 1);
        rb = GetComponent<Rigidbody2D>();
        InitialiseMovment();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Decrease hitpoints when hit by a bullet
    public void TakeDamage(int damage)
    {
        hitpoints -= damage;
        hitpointsText.text = hitpoints.ToString();
        size = hitpoints * 0.5f;
        transform.localScale = new Vector3(Mathf.Sqrt(size), Mathf.Sqrt(size), 1);
        if (hitpoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    // // collision detection with bullets
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Bullet"))
    //     {
    //         Bullet bullet = collision.gameObject.GetComponent<Bullet>();
    //         TakeDamage((int)bullet.damage);
    //         Destroy(collision.gameObject);
    //     }
    // }


}
