using UnityEngine;

public abstract class BaseBall : MonoBehaviour
{
    public Rigidbody2D rb { get; protected set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public bool IsMoving()
    {
        return rb.linearVelocity.sqrMagnitude > 0.001f || Mathf.Abs(rb.angularVelocity) > 0.1f;
    }
}