using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gary : MonoBehaviour
{
    public int health;
    public int damage;
    private float timeBtwDamage = 1.5f;
    public GameObject bullet; // Should be 'None' for melee and should have no null check issues.

    public Slider healthBar;
    private GameObject player;
    private Animator anim;
    public bool phase1;
    public bool phase2;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");        
        anim = GetComponent<Animator>();
        phase1 = true;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health;

        if (health <= 5) 
        {
            anim.SetTrigger("StageTwo");
            phase1 = false;
            phase2 = true;
        }

        if (health <= 0) 
        {
            anim.SetTrigger("Death");
            phase2 = false;
            gameObject.SetActive(false); // TODO come up with better handling of death for Gary.
        }
    }

    public void AttemptAttack()
    {
        Vector3 enemyPlayerDifferenceVector = transform.position - player.transform.position;
        float enemyPlayerAngle = (Mathf.Atan2(enemyPlayerDifferenceVector.y, enemyPlayerDifferenceVector.x) * Mathf.Rad2Deg) / 180;
        // Shoot projectile in 8 directions towards player.
        // Rotate bullet sprite to go with enemy direction (facing player)
        Quaternion fixedDirection = Quaternion.identity;
        fixedDirection.eulerAngles = new Vector3(0, 0, 90 - (enemyPlayerAngle * 180)); // Multiply by 180 to convert to degrees.  Offset by 90 just cuz.
        Vector2 projectilePosition = transform.position;
        projectilePosition.y -= 0.5f; // offset from Gary to avoid shooting from chest.  This makes upward shots fail, but those should never happen.
        Vector3 target = player.transform.position - transform.position;
        GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
        go.GetComponent<Rigidbody2D>().velocity = target;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("EnemyProjectile"))
        {
            TakeDamage(col.gameObject.GetComponent<ProjectileController>().damage);
            Destroy(col.gameObject);
        }
    }

    void TakeDamage(int amount)
    {
        health -= amount;
    }
}
