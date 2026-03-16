using UnityEngine;

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
        return rb.linearVelocity.sqrMagnitude > 0.01f;
    }
}