using System;
using NUnit.Framework;
using UnityEngine;
//Belongs to the player, responsible for instantiating bullets and setting their properties

public class BulletInstantiator : MonoBehaviour
{
    // Bullet prefab to instantiate
    public GameObject bulletPrefab;
    public AimConeIndicator aimConeIndicator;

    //Properties of a bullet
    public float speed = 10f;
    public float size = 0.2f;
    public float damage = 1f;
    public float cooldown = 0.5f;
    public float bulletLifetime = 2f;

    private float timeSinceLastShoot = 0f;
    private bool isCharging = false;


    void Start()
    {

    }

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
        if (timeSinceLastShoot >= cooldown && InputManager.Instance.isAttacking && !isCharging)
        {
            isCharging = true;
            aimConeIndicator.SetCharging(true);
            print("start charging");
        }
        else if(isCharging && !InputManager.Instance.isAttacking)
        {
            print("shoot. Charged for " + timeSinceLastShoot + " seconds.");
            Shoot();
            timeSinceLastShoot = 0f;
            isCharging = false;
            aimConeIndicator.SetCharging(false);
        }
        else
        {
            timeSinceLastShoot += Time.deltaTime;
        }
    }
}
