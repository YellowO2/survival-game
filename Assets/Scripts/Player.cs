using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerBall : BaseBall
{
    public float speed {get; private set;} = 10f;
    public TMPro.TextMeshPro hitPointText;


    void Start()
    {
    }

    void Update()
    {
        transform.up = InputManager.Instance.aimDirection.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            print("Player collided with enemy!");
            // Enemy enemy = other.GetComponent<Enemy>();
            // TakeDamage(enemy.hitpoints);
            // Destroy(other.gameObject);
        }
    }

}
