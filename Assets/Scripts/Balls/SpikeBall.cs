using UnityEngine;

public class SpikeBall : EnemyBall
{

    public override void SetUp(int hitpoints, BallColor color, BallType type = BallType.Spike)
    {
        base.SetUp(hitpoints, color, BallType.Spike);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        PlayerBall player = collision.gameObject.GetComponent<PlayerBall>();
        if (player != null)
        {
            // player.TakeDamage(spikeDamage);
        }
    }
}