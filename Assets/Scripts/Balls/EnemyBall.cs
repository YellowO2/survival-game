using UnityEditor.Callbacks;
using UnityEngine;




public class EnemyBall : BaseBall
{
    //Properties of an enemy
    public int hitpoints { get; private set; } = 1;
    public BallColor color { get; private set; }
    public BallType type { get; private set; }
    public TMPro.TextMeshPro hitpointsText;
    protected SpriteRenderer spriteRenderer;

    public virtual void SetUp(int hitpoints, BallColor color, BallType type = BallType.Normal)
    {
        this.hitpoints = hitpoints;
        this.color = color;
        this.type = type;
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
    public virtual void TakeDamage(int damage)
    {
        hitpoints -= damage;
        hitpointsText.text = hitpoints.ToString();
        TurnManager.Instance.RecordDamage(damage);
    }

    public void ApplyImpact(Vector2 sourcePosition, Vector2 contactPoint)
    {
        Vector2 dir = ((Vector2)transform.position - sourcePosition).normalized;
        float bonusForce = 8f;

        rb.AddForce(dir * bonusForce, ForceMode2D.Impulse); // Push away from the source

        if (GameFeelManager.Instance != null && spriteRenderer != null)
        {
            GameFeelManager.Instance.HitStop(0.07f);
            GameFeelManager.Instance.ShakeCamera(0.1f, 0.1f);
            GameFeelManager.Instance.SpawnHitEffect(contactPoint, spriteRenderer.color);
        }
    }

    public virtual void Die() //only for calling during the resolve phase
    {

        GameFeelManager.Instance.SpawnDeathEffect(transform.position, spriteRenderer.color);
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
        // Check if we hit another enemy or the player
        EnemyBall otherEnemy = collision.gameObject.GetComponent<EnemyBall>();
        TakeDamage(1);

        if (otherEnemy != null)
        {
            // Only apply impact if colors match
            if (this.color == otherEnemy.color)
            {
                ApplyImpact(otherEnemy.transform.position, collision.GetContact(0).point);
            }
        }

        PlayerBall player = collision.gameObject.GetComponent<PlayerBall>();
        if (player != null)
        {
            // Different color, player takes damage (same-color is handled by OnTriggerEnter2D now)
            if (player.currentColor != this.color)
            {
                player.TakeDamage(1);
            }
        }

        // If it gets collided with again while already having 0 health, just destroy it immediately
        if (hitpoints <= 0)
        {
            Die();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        // Triggers happen when this enemy is the same color as the player and therefore set to act as a pickup
        PlayerBall player = collider.GetComponent<PlayerBall>();
        if (player != null)
        {
            // If the player's color matches the enemy's color, the player swallows the enemy and it doesn't bounce
            if (player.currentColor == this.color)
            {
                player.Swallow(this);
            }
        }
    }


}
