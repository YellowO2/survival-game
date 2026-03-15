using UnityEditor.Callbacks;
using UnityEngine;

public enum BallColor { Red, Blue, Green}


public class Enemy : BaseBall
{
    //Properties of an enemy
    public int hitpoints = 1;
    public BallColor color { get; private set; }
    public int originalHitpoints; // the original hitpoints of the enemy
    public TMPro.TextMeshPro hitpointsText;
    private SpriteRenderer spriteRenderer;

    public void SetUp(int hitpoints, BallColor color)
    {
        this.hitpoints = hitpoints;
        this.originalHitpoints = hitpoints;
        this.color = color;
        hitpointsText.text = hitpoints.ToString();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ApplyVisual();
    }
    private void ApplyVisual()
    {
        if (spriteRenderer == null) return;

        switch (color)
        {
            case BallColor.Red:
                spriteRenderer.color = new Color(1f, 0.35f, 0.35f);
                break;
            case BallColor.Blue:
                spriteRenderer.color = new Color(0.35f, 0.6f, 1f);
                break;
            case BallColor.Green:
                spriteRenderer.color = new Color(0.35f, 1f, 0.45f);
                break;
            // case BallColor.Yellow:
            //     spriteRenderer.color = new Color(1f, 0.9f, 0.3f);
            //     break;
        }
    }

    // Decrease hitpoints when hit by a bullet
    public void TakeDamage(int damage)
    {
        hitpoints -= damage;
        hitpointsText.text = hitpoints.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if we hit another enemy 9r the player
        Enemy otherEnemy = collision.gameObject.GetComponent<Enemy>();

        if (otherEnemy != null)
        {
            // Only deduct health if colors match
            if (this.color == otherEnemy.color)
            {
                TakeDamage(1);
            }
        }

        PlayerBall player = collision.gameObject.GetComponent<PlayerBall>();
        if (player != null)
        {
            TakeDamage(1);
        }
    }


}
