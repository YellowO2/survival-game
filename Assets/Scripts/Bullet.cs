using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Properties of a bullet
    public float speed = 1f;
    public float damage = 1f;
    public float lifetime = 2f;

    public void SetUp(float damage, float speed, float size, float lifetime)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        this.transform.localScale = new Vector3(size, size, size);
    }

    // Update is called once per frame
    void Update()
    {
        // go forward in the direction it's facing
        transform.Translate(speed * Time.deltaTime * transform.up);
    }
}
