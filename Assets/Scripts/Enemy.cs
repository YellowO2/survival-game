using UnityEditor.Callbacks;
using UnityEngine;

public class Enemy : BaseBall
{
    //Properties of an enemy
    private float speed;
    public int hitpoints = 1;
    public int originalHitpoints; // the original hitpoints of the enemy, which is used to calculate score when the enemy is defeated
    public TMPro.TextMeshPro hitpointsText;
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
        rb = GetComponent<Rigidbody2D>();
        InitialiseMovment();
    }

    // Decrease hitpoints when hit by a bullet
    public void TakeDamage(int damage)
    {
        hitpoints -= damage;
        hitpointsText.text = hitpoints.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            TakeDamage(1);
    }


}
