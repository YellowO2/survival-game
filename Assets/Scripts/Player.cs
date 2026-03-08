using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody2D rb;
    private Vector2 moveDir;
    public InputActionReference moveAction;
    public int hitPoints = 10;
    public TMPro.TextMeshPro hitPointText;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Move()
    {
        Vector2 movement = moveDir * speed;
        rb.linearVelocity = movement;
    }

    public void TakeDamage(int damage)
    {
        // Handle player taking damage here (e.g., reduce health, check for game over, etc.)
        hitPoints -= damage;
        hitPointText.text = hitPoints.ToString();
        if (hitPoints <= 0)
        {
            Debug.Log("Player has died!");
            // Handle player death 
            GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
        }
    }

    void Update()
    {
        moveDir = moveAction.action.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Move();
    }

}
