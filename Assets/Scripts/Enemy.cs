using UnityEngine;
// The enemies are what the player has to shoot off. They carry a number that we can see as hitpoints 
// The enemies will simple move downwards, i.e. towards the player, and the player has to shoot them before they reach the player. If they reach the player, the game is over.
public class Enemy : MonoBehaviour
{
    //Properties of an enemy
    public float speed = 1f;
    public int hitpoints = 1; // the enemy size scales with hitpoints, the bigger the hitpoints, the bigger the enemy
    // text mesh pro to display hitpoints
    public int originalHitpoints; // the original hitpoints of the enemy, which is used to calculate score when the enemy is defeated
    public TMPro.TextMeshPro hitpointsText;


    void Start()
    {
        hitpointsText.text = hitpoints.ToString();
        originalHitpoints = hitpoints;
        // scale the enemy with hitpoints
        transform.localScale = new Vector3(Mathf.Sqrt(hitpoints), Mathf.Sqrt(hitpoints), 1);
    }

    public void SetUp(int hitpoints, float speed)
    {
        this.hitpoints = hitpoints;
        this.speed = speed;
        this.originalHitpoints = hitpoints;
        hitpointsText.text = hitpoints.ToString();
        // scale the enemy with hitpoints
        transform.localScale = new Vector3(Mathf.Sqrt(hitpoints), Mathf.Sqrt(hitpoints), 1);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the enemy downwards
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    // Decrease hitpoints when hit by a bullet
    public void TakeDamage(int damage)
    {
        hitpoints -= damage;
        hitpointsText.text = hitpoints.ToString();
        transform.localScale = new Vector3(Mathf.Sqrt(hitpoints), Mathf.Sqrt(hitpoints), 1);
        if (hitpoints <= 0)
        {
            GameManager.Instance.AddScore(originalHitpoints);
            Destroy(gameObject);
        }
    }

    // collision detection with bullets
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            TakeDamage((int)bullet.damage);
            Destroy(collision.gameObject);
        }
    }


}
