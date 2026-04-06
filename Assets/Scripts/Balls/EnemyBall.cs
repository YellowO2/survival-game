using UnityEditor.Callbacks;
using UnityEngine;

public enum BallColor { Red, Blue, Green }
public enum BallType { Normal, Bomb, Spike }


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
        float bonusForce = 2.5f;

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

        if (otherEnemy != null)
        {
            // Only deduct health if colors match
            if (this.color == otherEnemy.color)
            {
                TakeDamage(1);
                ApplyImpact(otherEnemy.transform.position, collision.GetContact(0).point);
            }
        }

        PlayerBall player = collision.gameObject.GetComponent<PlayerBall>();
        if (player != null)
        {
            // Only take damage and apply impact if the player's color matches the enemy's color!
            if (player.currentColor == this.color)
            {
                TakeDamage(1);
                ApplyImpact(player.transform.position, collision.GetContact(0).point);
            }
        }
    }


}
