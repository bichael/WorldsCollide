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
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");        
        anim = GetComponent<Animator>();
        phase1 = true;
        speed = bullet.GetComponent<ProjectileController>().speed;
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
        enemyPlayerAngle = Mathf.Round(enemyPlayerAngle * 4) / 4; // Round to nearest 0.25
        // Rotate bullet sprite to go with enemy direction (facing player)
        Quaternion fixedDirection = Quaternion.identity;
        fixedDirection.eulerAngles = new Vector3(0, 0, 90 - (enemyPlayerAngle * 180)); // Multiply by 180 to convert to degrees.  Offset by 90 just cuz.
        Vector2 projectilePosition = transform.position;
        Vector3 target = player.transform.position - transform.position;
        if (enemyPlayerAngle == 0) // if facing left
        {
            projectilePosition.x -= 0.5f; // offset from player pivot (to avoid shooting from chest)
            GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
            // go.GetComponent<ProjectileController>().SetProjectileVector("Left");
            go.gameObject.GetComponent<ProjectileController>().SetProjectileVector("None");
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * speed;
        }
        else if (enemyPlayerAngle == -0.5f) // if facing up
        {
            projectilePosition.y += 0.5f;
            GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
            // go.GetComponent<ProjectileController>().SetProjectileVector("Up");
            go.gameObject.GetComponent<ProjectileController>().SetProjectileVector("None");
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * speed;
        }
        else if ((enemyPlayerAngle == 1) || (enemyPlayerAngle == -1)) // if facing right
        {
            projectilePosition.x += 0.5f;
            GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
            // go.GetComponent<ProjectileController>().SetProjectileVector("Right");
            go.gameObject.GetComponent<ProjectileController>().SetProjectileVector("None");
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * speed;
        }
        else if (enemyPlayerAngle == 0.5f) // if facing down
        {
            projectilePosition.y -= 0.5f;
            GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
            // go.GetComponent<ProjectileController>().SetProjectileVector("Down");
            go.gameObject.GetComponent<ProjectileController>().SetProjectileVector("None");
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * speed;
        }
        else if (enemyPlayerAngle == -0.25) // if facing up-left
        {
            projectilePosition.y += 0.5f;
            projectilePosition.x -= 0.5f;
            GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
            // go.GetComponent<ProjectileController>().SetProjectileVector("UpLeft");
            go.gameObject.GetComponent<ProjectileController>().SetProjectileVector("None");
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * speed;
        }
        else if (enemyPlayerAngle == -0.75) // if facing up-right
        {
            projectilePosition.y += 0.5f;
            projectilePosition.x += 0.5f;
            GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
            // go.GetComponent<ProjectileController>().SetProjectileVector("UpRight");
            go.gameObject.GetComponent<ProjectileController>().SetProjectileVector("None");
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * speed;
        }
        else if (enemyPlayerAngle == 0.75) // if facing down-right
        {
            projectilePosition.y -= 0.5f;
            projectilePosition.x += 0.5f;
            GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
            // go.GetComponent<ProjectileController>().SetProjectileVector("DownRight");
            go.gameObject.GetComponent<ProjectileController>().SetProjectileVector("None");
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * speed;
        }
        else if (enemyPlayerAngle == 0.25)// if facing down-left
        {
            projectilePosition.y -= 0.5f;
            projectilePosition.x -= 0.5f;
            GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
            // go.GetComponent<ProjectileController>().SetProjectileVector("DownLeft");
            go.gameObject.GetComponent<ProjectileController>().SetProjectileVector("None");
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("EnemyProjectile"))
        {
            // TakeDamage(col.gameObject.GetComponent<ProjectileController>().damage);
            TakeDamage(1);
            Destroy(col.gameObject);
        }
    }

    void TakeDamage(int amount)
    {
        health -= amount;
    }
}
