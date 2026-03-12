using System;
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
        hitPointText.text = hitPoints.ToString();
    }

    void Move()
    {
        Vector2 movement = moveDir * speed;
        if(InputManager.Instance.isAttacking)
        {
            movement = movement * 0.2f; // Reduce speed by half when attacking
            Time.timeScale = 0.5f; // Slow down time when attacking
        }
        
        rb.linearVelocity = movement;

        transform.up = InputManager.Instance.aimDirection.normalized;

    }

    public void TakeDamage(int damage)
    {
        // Handle player taking damage here (e.g., reduce health, check for game over, etc.)
        hitPoints -= damage;
        hitPointText.text = hitPoints.ToString();
        if (hitPoints <= 0)
        {
            Debug.Log("Player has died!");
            // Handle player death and hide the player object
            gameObject.SetActive(false);
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            print("Player collided with enemy!");
            Enemy enemy = other.GetComponent<Enemy>();
            TakeDamage(enemy.hitpoints);
            Destroy(other.gameObject);
        }
    }

}
