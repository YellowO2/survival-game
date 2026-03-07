using UnityEngine;
//Belongs to the player, responsible for instantiating bullets and setting their properties

public class BulletInstantiator : MonoBehaviour
{
    // Bullet prefab to instantiate
    public GameObject bulletPrefab;

    //Properties of a bullet
    public float speed = 10f;
    public float size = 0.2f;
    public float damage = 1f;
    public float cooldown = 0.5f;
    public float bulletLifetime = 2f;

    private float timeSinceLastShoot = 0f;


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
        //shoot if cooldown is over
        if (timeSinceLastShoot >= cooldown)
        {
            Shoot();
            timeSinceLastShoot = 0f;
        }
        else
        {
            timeSinceLastShoot += Time.deltaTime;
        }
    }
}
