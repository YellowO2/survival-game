using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerBall : BaseBall
{
    public float speed {get; private set;} = 10f;
    private int hitPoints = 3;

    void Start()
    {
        UIManager.Instance.UpdatePlayerHealth(hitPoints);
    }

    void Update()
    {
        transform.up = InputManager.Instance.aimDirection.normalized;
    }
    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdatePlayerHealth(hitPoints);
        }

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
