using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class BombBall : EnemyBall
{

    [Header("Explosion Settings")]
    private float explosionMagnitude = 3f;
    private int explosionDamage = 3;
    private float explosionForce = 3f;
    private bool isTicking = false;

    public override void SetUp(int hitpoints, BallColor color, BallType type = BallType.Bomb)
    {
        base.SetUp(hitpoints, color, BallType.Bomb);
        if (spriteRenderer != null)
        {
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f); //for now use slightly larger to indicate
        }
    }

    private IEnumerator DetonationRoutine()
    {
        isTicking = true;
        
        // Optional: PVZ Cherry Bomb visual effect (flashing/contracting)
        // For now, we will just flash colors to show the warning
        float timer = 0;
        Color originalColor = spriteRenderer.color;

        while (timer < 1.5f) //use this as explosion timer for now
        {
            // Make it flash
            float lerp = Mathf.PingPong(timer * 5f, 1f); 
            spriteRenderer.color = Color.Lerp(originalColor, Color.white, lerp);
            timer += Time.deltaTime;
            yield return null;
        }

        // Boom time
        Explode();
        isTicking = false;
        
        //time for a short time
        yield return new WaitForSeconds(0.1f);
        // cannot destroy immediately because turnmanager checking. So should basically make it non existence and not interactable but not destroy it
        gameObject.SetActive(false);
        rb.linearVelocity = Vector2.zero; // Stop any movement immediately
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (!isTicking && hitpoints <= 0)
        {
            StartCoroutine(DetonationRoutine());
        }
    }

    public override bool IsBusy()
    {
        if (isTicking) return true; // Consider the bomb busy while it's ticking down to explosion
        return base.IsBusy();
    }

    private void Explode()
    {
        Debug.Log($"Bomb {name} exploded!");
        //expand sprite to indicate explosion
        spriteRenderer.transform.localScale *= explosionMagnitude; //TODO:later we can use like lerp or something to handle

        // Find all colliders in the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionMagnitude/2f);

        foreach (Collider2D col in colliders)
        {
            if (col.gameObject == this.gameObject) continue;

            Vector2 dir = (col.transform.position - transform.position).normalized;

            // Damage and push other enemies
            EnemyBall enemy = col.GetComponent<EnemyBall>();
            if (enemy != null)
            {
                enemy.rb.AddForce(dir * explosionForce, ForceMode2D.Impulse);
                enemy.TakeDamage(explosionDamage); // This can cause chain-reaction explosions!
            }

            // Damage and push player
            PlayerBall player = col.GetComponent<PlayerBall>();
            if (player != null)
            {
                player.rb.AddForce(dir * explosionForce, ForceMode2D.Impulse);
                // TODO: Implement player damage logic
                // player.TakeDamage(explosionDamage);
            }
        }
    }
}