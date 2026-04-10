using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerBall : BaseBall
{
    public float speed {get; private set;} = 15f;
    public BallColor currentColor { get; private set; } = BallColor.Red;
    private SpriteRenderer spriteRenderer;
    private int hitPoints = 5; //im not sure what this is for yet

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        UpdateColorVisuals();
    }

    void Update()
    {
        transform.up = InputManager.Instance.aimDirection.normalized;

        // Pressing Right Click or Space to switch colors
        // if (UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.rightButton.wasPressedThisFrame || UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
        // {
        //     SwitchColor();
        // }
    }

    public void SwitchColor()
    {
        // Cycle between Red, Blue, Green (0, 1, 2)
        int colorCount = 3; 
        currentColor = (BallColor)(((int)currentColor + 1) % colorCount);
        UpdateColorVisuals();
    }

    private void UpdateColorVisuals()
    {
        if (spriteRenderer == null) return;
        
        switch (currentColor)
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
        }
    }
    public void Swallow(EnemyBall enemy)
    {
        hitPoints += enemy.hitpoints;
        // Increase size slightly based on gained hitpoints
        transform.localScale += new Vector3(0.1f, 0.1f, 0f) * enemy.hitpoints;
        
        enemy.Die();
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        // Decrease size slightly when taking damage, maintaining a minimum size constraint
        transform.localScale = Vector3.Max(new Vector3(0.5f, 0.5f, 1f), transform.localScale - new Vector3(0.1f, 0.1f, 0f) * damage);

        // if (UIManager.Instance != null)
        // {
        //     UIManager.Instance.UpdatePlayerHealth(hitPoints);
        // }

        if (hitPoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        GameManager.Instance.ChangeState(GameState.GameOver);
    }

}
