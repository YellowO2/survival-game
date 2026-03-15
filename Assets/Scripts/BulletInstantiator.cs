using System;
using UnityEngine;
//Belongs to the player, responsible for instantiating bullets and setting their properties

public class BulletInstantiator : MonoBehaviour
{
    // Bullet prefab to instantiate
    public GameObject bulletPrefab;
    public AimConeIndicator aimConeIndicator;
    public TMPro.TextMeshPro cooldownText;

    //Properties of a bullet
    private float speed = 10f;
    private float size = 1f;
    private float damage = 1f;
    private float bulletLifetime = 2f;
    private bool isAiming = false;

    void Shoot()
    {
        if (TurnManager.Instance.phase != TurnPhase.PlayerAim)
        {
            return;
        }

        //Instantiate a bullet and set its properties
        GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.up * 0.5f, transform.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetUp(damage, speed, size, bulletLifetime);
        Destroy(bullet, bulletLifetime);
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
