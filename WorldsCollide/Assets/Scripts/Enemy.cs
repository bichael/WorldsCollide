using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState{IDLE_STATIC,IDLE_ROAMER,IDLE_PATROL,INSPECT,ATTACK,DEAD,NONE}
public enum WeaponType{MELEE,PROJECTILE}
public class Enemy : MonoBehaviour
{
    public int health;
    public float initSpeed;
    private float speed;
    private float dazedTime;
    public float startDazedTime;
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public LayerMask playerLayer;
    AudioSource enemyAudio;
    AudioSource sfxAudio;
    public AudioClip enemyHurtClip;
    public AudioClip enemyMeleeClip;
    public AudioClip enemyShootClip;
    public int meleeDamage;
    public float meleeRange = .1f;
    public float sightRange = 1f;
    public float disengageRange = 2f;

    Animator anim;
    GameObject player;
    PlayerHealth playerHealthScript;
    Player ply;
    bool playerInRange; // Becomes true when player enters enemy collider; close enough to melee.
    bool inSightRange; // Becomes true when player enters sight FOV, becomes false when player exits sight FOV.
    bool aggro; // Becomes true when player enters sight FOV, becomes false when player exits disengage FOV.

	public WeaponType weaponType = WeaponType.MELEE;
	// public EnemyState idleState = EnemyState.IDLE_STATIC;
    public GameObject bullet; // Should be 'None' for melee and should have no null check issues.

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // playerHealthScript = player.transform.GetComponent<PlayerHealth>();
        ply = player.GetComponent<Player>();
        playerHealthScript = player.GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        sfxAudio = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) 
        {
            Destroy(gameObject);
        }

        // Note:  The following function is called on every frame, for every enemy... This could be a memory hog; might need spawning/despawning system.
        // If player is within FOV, begin to follow.  Also only check to attack if player is in FOV.
        Collider2D[] sightSearch = Physics2D.OverlapCircleAll(transform.position, sightRange, playerLayer);
        if ((sightSearch.Length > 0) && (ply.time_stopped == false))
        {
            aggro = true; // Player has entered sight FOV and enemy will chase until exits disengage FOV
            inSightRange = true;
        }
        else
        {
            inSightRange = false;
        }
        if (aggro)
        {
            //Check if player is far enough to disengage
            Collider2D[] disengageSearch = Physics2D.OverlapCircleAll(transform.position, disengageRange, playerLayer);
            if (disengageSearch.Length > 0)
            {
                // Projectile enemies will stand still and shoot the player until they exit the sight range.
                // Both projectile and melee enemies will follow the player until he exits the disengage range.
                if (weaponType == WeaponType.PROJECTILE)
                {
                    if (timeBtwAttack <= 0 && inSightRange && playerHealthScript.CanBeAttacked())
                    {
                        AttemptAttack();
                    }
                    else if (!inSightRange)
                    {
                        timeBtwAttack -= Time.deltaTime;
                        // Move towards the player.
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        Vector3 enemyPlayerDifferenceVector = transform.position - player.transform.position;
                        float enemyPlayerAngle = (Mathf.Atan2(enemyPlayerDifferenceVector.y, enemyPlayerDifferenceVector.x) * Mathf.Rad2Deg) / 180;
                        enemyPlayerAngle = Mathf.Round(enemyPlayerAngle * 2) / 2; // Round to nearest 0.5
                        anim.SetFloat("Direction", enemyPlayerAngle);
                        anim.SetFloat("Magnitude", enemyPlayerDifferenceVector.magnitude);
                    }
                    else
                    {
                        timeBtwAttack -= Time.deltaTime;
                    }
                }
                else if (weaponType == WeaponType.MELEE)
                {
                    // Move towards the player.
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    Vector3 enemyPlayerDifferenceVector = transform.position - player.transform.position;
                    float enemyPlayerAngle = (Mathf.Atan2(enemyPlayerDifferenceVector.y, enemyPlayerDifferenceVector.x) * Mathf.Rad2Deg) / 180;
                    enemyPlayerAngle = Mathf.Round(enemyPlayerAngle * 2) / 2; // Round to nearest 0.5
                    anim.SetFloat("Direction", enemyPlayerAngle);
                    anim.SetFloat("Magnitude", enemyPlayerDifferenceVector.magnitude);

                    if (timeBtwAttack <= 0 && playerInRange && playerHealthScript.CanBeAttacked())
                    {
                        AttemptAttack();
                    }
                    else
                    {
                        timeBtwAttack -= Time.deltaTime;
                    }
                }
            }
            else 
            {
                aggro = false; // Player has left both enemy FOVs and enemy should now rest.
            }
        } 

        if ((dazedTime <= 0) && (ply.time_stopped == false))
        {
            speed = initSpeed;
        } else 
        {
            speed = 0;
            dazedTime -= Time.deltaTime;
            // Enemy is idle (in "dazed" state after being attacked)
            anim.SetFloat("Magnitude", 0);
        }
    }

    public void TakeDamage(int damage)
    {
        // AudioSource.PlayClipAtPoint(enemyHurtClip, transform.position);
        dazedTime = startDazedTime;
        health -= damage;
        sfxAudio.clip = enemyHurtClip;
        sfxAudio.Play();
        Debug.Log(this + " took " + damage + " damage");
    }
    public void TakeProjDamage(int damage,float mod)
    {
        // AudioSource.PlayClipAtPoint(enemyHurtClip, transform.position);
        dazedTime = startDazedTime * mod;
        health -= damage;
        sfxAudio.clip = enemyHurtClip;
        sfxAudio.Play();
        Debug.Log(this + " took " + damage + " damage");
    }

    private void AttemptAttack()
    {
        // animator.SetTrigger("Attacking");
        if (weaponType == WeaponType.PROJECTILE)
        {
            enemyAudio.clip = enemyShootClip;
            enemyAudio.Play();
            Vector3 enemyPlayerDifferenceVector = transform.position - player.transform.position;
            float enemyPlayerAngle = (Mathf.Atan2(enemyPlayerDifferenceVector.y, enemyPlayerDifferenceVector.x) * Mathf.Rad2Deg) / 180;

            // Shoot projectile in 4 directions towards player.
            // enemyPlayerAngle = Mathf.Round(enemyPlayerAngle * 2) / 2; // Round to nearest 0.5
            enemyPlayerAngle = Mathf.Round(enemyPlayerAngle * 4) / 4; // Round to nearest 0.25

            // Rotate bullet sprite to go with enemy direction (facing player)
            Quaternion fixedDirection = Quaternion.identity;
            fixedDirection.eulerAngles = new Vector3(0, 0, 90 - (-enemyPlayerAngle * 180)); // Multiply by 180 to convert to degrees.  Offset by 90 just cuz.
            // GameObject go = (GameObject)Instantiate (bullet, transform.position, fixedDirection);
            Vector2 projectilePosition = transform.position;
            if (enemyPlayerAngle == 0) // if facing left
            {
                projectilePosition.x -= 0.15f; // offset from player pivot (to avoid shooting from chest)
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                go.GetComponent<ProjectileController>().SetProjectileVector("Left");
            }
            else if (enemyPlayerAngle == -0.5f) // if facing up
            {
                projectilePosition.y += 0.2f;
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                go.GetComponent<ProjectileController>().SetProjectileVector("Up");
            }
            else if ((enemyPlayerAngle == 1) || (enemyPlayerAngle == -1)) // if facing right
            {
                projectilePosition.x += 0.15f;
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                go.GetComponent<ProjectileController>().SetProjectileVector("Right");
            }
            else if (enemyPlayerAngle == 0.5f) // if facing down
            {
                projectilePosition.y -= 0.2f;
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                go.GetComponent<ProjectileController>().SetProjectileVector("Down");
            }
            else if (enemyPlayerAngle == -0.25) // if facing up-left
            {
                projectilePosition.y += 0.15f;
                projectilePosition.x -= 0.15f;
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                go.GetComponent<ProjectileController>().SetProjectileVector("UpLeft");
            }
            else if (enemyPlayerAngle == -0.75) // if facing up-right
            {
                projectilePosition.y += 0.15f;
                projectilePosition.x += 0.15f;
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                go.GetComponent<ProjectileController>().SetProjectileVector("UpRight");
            }
            else if (enemyPlayerAngle == 0.75) // if facing down-right
            {
                projectilePosition.y -= 0.15f;
                projectilePosition.x += 0.15f;
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                go.GetComponent<ProjectileController>().SetProjectileVector("DownRight");
            }
            else if (enemyPlayerAngle == 0.25)// if facing down-left
            {
                projectilePosition.y -= 0.15f;
                projectilePosition.x -= 0.15f;
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, fixedDirection);
                go.GetComponent<ProjectileController>().SetProjectileVector("DownLeft");
            }

            // // ALTERNATE APPROACH: Shoot projectile directly at player.
            // Quaternion fixedDirection = Quaternion.identity;
            // fixedDirection.eulerAngles = new Vector3(0, 0, 90 - (-enemyPlayerAngle * 180)); // Multiply by 180 to convert to degrees.  Offset by 90 just cuz.
            // GameObject go = (GameObject)Instantiate (bullet, transform.position, fixedDirection);
            // go.GetComponent<Rigidbody2D>().velocity = -enemyPlayerDifferenceVector;
        }
        else if (weaponType == WeaponType.MELEE)
        {
            enemyAudio.clip = enemyMeleeClip;
            enemyAudio.Play();
            // timeBtwAttack = startTimeBtwAttack;
            Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(transform.position, meleeRange, playerLayer);
            Debug.Log("Player found in " + this + "'s melee hitbox: " + playerToDamage.Length);
            for (int i=0; i < playerToDamage.Length; i++) // There is only one GameObject in the Player layer, but this logic is fine
            {
                if (playerHealthScript.currentHealth > 0)
                    playerHealthScript.TakeDamage(meleeDamage);
            }
        }
        timeBtwAttack = startTimeBtwAttack;
    }

    // void OnCollisionEnter2D(Collision2D col)
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("PlayerProjectile"))
        {
            TakeProjDamage(col.gameObject.GetComponent<ProjectileController>().damage, col.gameObject.GetComponent<ProjectileController>().daze_mod);
            Destroy(col.gameObject);
        }
        else if (col.gameObject == player)
        {
            playerInRange = true;
            Debug.Log("player entered " + this + " range.");
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject == player)
        {
            playerInRange = true;
            Debug.Log("player exited " + this + " range.");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(transform.position, disengageRange);
    }
}
