using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KALI : MonoBehaviour
{
    private GameObject player;
    private Animator anim;
    private AudioSource garyAudio;
    private AudioSource sfxAudio;
    private int enemiesPerWave = 2;

    // public AudioClip kaliHurtClip;
    public int health;
    public Slider healthBar;
    public float projectileSpeed = 0.5f;
    public bool enemiesAlive;
    public bool introTalkDone;
    public bool outroTalkDone;
    public int botsKilled;
    private int botsSent = 0;
    public GameObject kaliBot;
    public GameObject exitTeleporter;   
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");        
        anim = GetComponent<Animator>();
        garyAudio = GetComponent<AudioSource>();
        sfxAudio = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health;

        // if (health <= 5) 
        // {
        //     anim.SetTrigger("StageTwo");
        // }

        if (health <= 0) 
        {
            anim.SetTrigger("Death");
            gameObject.SetActive(false);
            if (exitTeleporter != null)
            {
                exitTeleporter.GetComponent<SpriteRenderer>().enabled = true;
                exitTeleporter.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    public void AttemptAttack()
    {
        if (!enemiesAlive)
        {
            enemiesAlive = true;
            botsSent += enemiesPerWave;
            SpawnEnemiesOverTime(enemiesPerWave, 1f);
        }
    }

    void SpawnEnemiesOverTime(int quantity, float duration)
    {
        float interval = duration/quantity;
        enemiesAlive = true;
        enemiesPerWave += 1;
        // --> spawn one and wait interval before coninuing (firing next)
        for (int i=0; i<quantity; i++)
            Invoke("ShootEnemyAtPlayer", interval * i);
    }

    void ShootEnemyAtPlayer()
    {
        // Spawn enemies with 50/50 chance of attack type.
        WeaponType weaponType = (Random.Range(0, 2) == 0) ? WeaponType.MELEE : WeaponType.PROJECTILE;
        // Give them a vector towards the player.
        Vector3 enemyPlayerDifferenceVector = transform.position - player.transform.position;
        float enemyPlayerAngle = (Mathf.Atan2(enemyPlayerDifferenceVector.y, enemyPlayerDifferenceVector.x) * Mathf.Rad2Deg) / 180;
        // Shoot projectile in 8 directions towards player.
        enemyPlayerAngle = Mathf.Round(enemyPlayerAngle * 4) / 4; // Round to nearest 0.25
        // Rotate bullet sprite to go with enemy direction (facing player)
        Quaternion fixedDirection = Quaternion.identity;
        fixedDirection.eulerAngles = new Vector3(0, 0, 90 - (enemyPlayerAngle * 180)); // Multiply by 180 to convert to degrees.  Offset by 90 just cuz.
        Vector2 projectilePosition = transform.position;
        Vector3 target = (player.transform.position - transform.position).normalized;
        if (enemyPlayerAngle == 0) // if facing left
        {
            projectilePosition.x -= 0.5f; // offset from player pivot (to avoid shooting from chest)
            GameObject go = (GameObject)Instantiate (kaliBot, projectilePosition, fixedDirection);
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * projectileSpeed;
            go.gameObject.GetComponent<Enemy>().weaponType = weaponType;
        }
        else if (enemyPlayerAngle == -0.5f) // if facing up
        {
            projectilePosition.y += 0.5f;
            GameObject go = (GameObject)Instantiate (kaliBot, projectilePosition, fixedDirection);
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * projectileSpeed;
            go.gameObject.GetComponent<Enemy>().weaponType = weaponType;
        }
        else if ((enemyPlayerAngle == 1) || (enemyPlayerAngle == -1)) // if facing right
        {
            projectilePosition.x += 0.5f;
            GameObject go = (GameObject)Instantiate (kaliBot, projectilePosition, fixedDirection);
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * projectileSpeed;
            go.gameObject.GetComponent<Enemy>().weaponType = weaponType;
        }
        else if (enemyPlayerAngle == 0.5f) // if facing down
        {
            projectilePosition.y -= 0.5f;
            GameObject go = (GameObject)Instantiate (kaliBot, projectilePosition, fixedDirection);
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * projectileSpeed;
            go.gameObject.GetComponent<Enemy>().weaponType = weaponType;
        }
        else if (enemyPlayerAngle == -0.25) // if facing up-left
        {
            projectilePosition.y += 0.5f;
            projectilePosition.x -= 0.5f;
            GameObject go = (GameObject)Instantiate (kaliBot, projectilePosition, fixedDirection);
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * projectileSpeed;
            go.gameObject.GetComponent<Enemy>().weaponType = weaponType;
        }
        else if (enemyPlayerAngle == -0.75) // if facing up-right
        {
            projectilePosition.y += 0.5f;
            projectilePosition.x += 0.5f;
            GameObject go = (GameObject)Instantiate (kaliBot, projectilePosition, fixedDirection);
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * projectileSpeed;
            go.gameObject.GetComponent<Enemy>().weaponType = weaponType;
        }
        else if (enemyPlayerAngle == 0.75) // if facing down-right
        {
            projectilePosition.y -= 0.5f;
            projectilePosition.x += 0.5f;
            GameObject go = (GameObject)Instantiate (kaliBot, projectilePosition, fixedDirection);
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * projectileSpeed;
            go.gameObject.GetComponent<Enemy>().weaponType = weaponType;
        }
        else if (enemyPlayerAngle == 0.25)// if facing down-left
        {
            projectilePosition.y -= 0.5f;
            projectilePosition.x -= 0.5f;
            GameObject go = (GameObject)Instantiate (kaliBot, projectilePosition, fixedDirection);
            go.gameObject.GetComponent<Rigidbody2D>().velocity = target * projectileSpeed;
            go.gameObject.GetComponent<Enemy>().weaponType = weaponType;
        }
    }

    public void IncrementBotsKilled()
    {
        botsKilled++;
        if (botsKilled == botsSent)
        {
            TakeDamage(2);
            enemiesAlive = false;
        }
    }

    void TakeDamage(int amount)
    {
        // sfxAudio.clip = kaliHurtClip;
        sfxAudio.Play();
        health -= amount;
    }
}
