using UnityEngine;

public enum BallColor { Red, Blue, Green }
public enum BallType { Normal, Bomb, Spike }
public abstract class BaseBall : MonoBehaviour
{
    public Rigidbody2D rb { get; protected set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // A handy method for your TurnManager to check if this specific ball has stopped
    public virtual bool IsBusy()
    {
        //check if ball is not yet destroyed first
        if (this == null) return false;
        return rb.linearVelocity.sqrMagnitude > 0.01f;
    }
}