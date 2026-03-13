using System;
using NUnit.Framework;
using UnityEngine;
//Belongs to the player, responsible for instantiating bullets and setting their properties

public class BulletInstantiator : MonoBehaviour
{
    // Bullet prefab to instantiate
    public GameObject bulletPrefab;
    public AimConeIndicator aimConeIndicator;
    public TMPro.TextMeshPro cooldownText;

    //Properties of a bullet
    private float speed = 25f;
    private float size = 0.2f;
    private float damage = 1f;
    private float cooldownTime = 2f;
    private float cooldownCounter = 0f;
    private float bulletLifetime = 2f;
    private bool isCharging = false;

    void Shoot()
    {
        //Instantiate a bullet and set its properties
        GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.up * 0.5f, transform.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetUp(damage, speed, size, bulletLifetime);
        Destroy(bullet, bulletLifetime);
    }

    void Update()
    {
        //shoot if cooldown is over and if look 
        if (cooldownCounter <= 0 && InputManager.Instance.isAttacking && !isCharging)
        {
            isCharging = true;
            Time.timeScale = 0.2f; // Slow down time when attacking
            aimConeIndicator.SetCharging(true);
            print("start charging");
        }
        else if(isCharging && !InputManager.Instance.isAttacking)
        {
            Shoot();
            isCharging = false;
            aimConeIndicator.SetCharging(false);
            cooldownText.text = cooldownTime.ToString("F1") + "s";
            cooldownCounter = cooldownTime;
            Time.timeScale = 1f; // Restore normal time scale
        }
        else
        {
            if (cooldownCounter > 0)
            {
                cooldownCounter -= Time.deltaTime;
                cooldownText.text = cooldownCounter.ToString("F1") + "s";
            }
        }
    }
}
