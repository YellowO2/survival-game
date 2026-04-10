using System;
using UnityEngine;
//Belongs to the player, responsible for instantiating bullets and setting their properties

public class BallLauncher : MonoBehaviour
{
    public AimConeIndicator aimConeIndicator;
    public TMPro.TextMeshPro cooldownText;
    public PlayerBall playerBall;

    void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnAttackStarted += HandleAttackStarted;
            InputManager.Instance.OnAttackEnded += HandleAttackEnded;
        }
    }

    void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnAttackStarted -= HandleAttackStarted;
            InputManager.Instance.OnAttackEnded -= HandleAttackEnded;
        }
    }

    private void HandleAttackStarted()
    {
        Debug.Log("Attack started");
        aimConeIndicator.showAim(true);

    }

    private void HandleAttackEnded()
    {
        Debug.Log("Attack ended");
        aimConeIndicator.showAim(false);
        Shoot();
    }

    void Shoot()
    {
        playerBall.rb.AddForce(transform.up * playerBall.speed, ForceMode2D.Impulse);
        TurnManager.Instance.OnShotFired();
    }
}
