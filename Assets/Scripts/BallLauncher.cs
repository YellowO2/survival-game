using System;
using UnityEngine;
//Belongs to the player, responsible for instantiating bullets and setting their properties

public class BallLauncher : MonoBehaviour
{
    public AimConeIndicator aimConeIndicator;
    public TMPro.TextMeshPro cooldownText;
    public PlayerBall playerBall;
    private bool isAiming = false;

    void Shoot()
    {
        if (TurnManager.Instance.phase != TurnPhase.PlayerAim)
        {
            return;
        }
        playerBall.rb.AddForce(transform.up * playerBall.speed, ForceMode2D.Impulse);
        TurnManager.Instance.OnShotFired();
    }

    void Update()
    {
        if (InputManager.Instance == null)
        {
            return;
        }

        bool canShoot = TurnManager.Instance.phase == TurnPhase.PlayerAim;

        if (!canShoot)
        {
            return;
        }

        bool isAttackingNow = InputManager.Instance.isAttacking;

        if (isAttackingNow && !isAiming)
        {
            isAiming = true;
            aimConeIndicator.showAim(true);
        }else if (!isAttackingNow && isAiming) // indicate the moment attack button is released.
        {
            isAiming = false;
            aimConeIndicator.showAim(false);
            Shoot();
        }
    }
}
