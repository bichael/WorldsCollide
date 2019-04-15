using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int meleeDamage;
    public float meleeRange = .1f;

    Animator anim;
    GameObject player;
    PlayerHealth playerHealthScript;
    bool playerInRange;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealthScript = player.transform.parent.GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) 
        {
            Destroy(gameObject);
        }
        
        if (timeBtwAttack <= 0 && playerInRange && playerHealthScript.CanBeAttacked())
            AttemptAttack();
        else
            timeBtwAttack -= Time.deltaTime;

        if (dazedTime <= 0)
        {
            speed = initSpeed;
        } else 
        {
            speed = 0;
            dazedTime -= Time.deltaTime;
            // Enemy is idle (in "dazed" state after being attacked)
            anim.SetFloat("Magnitude", 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        Vector3 enemyPlayerDifferenceVector = transform.position - player.transform.position;
        float enemyPlayerAngle = (Mathf.Atan2(enemyPlayerDifferenceVector.y, enemyPlayerDifferenceVector.x) * Mathf.Rad2Deg) / 180;
        enemyPlayerAngle = Mathf.Round(enemyPlayerAngle * 2) / 2; // Round to nearest 0.5
        anim.SetFloat("Direction", enemyPlayerAngle);
        anim.SetFloat("Magnitude", enemyPlayerDifferenceVector.magnitude);
    }

    public void TakeDamage(int damage)
    {
        dazedTime = startDazedTime;
        health -= damage;
        Debug.Log(this + " took " + damage + " damage");
    }

    private void AttemptAttack()
    {
       timeBtwAttack = startTimeBtwAttack;
       // animator.SetTrigger("Attacking");
       Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(transform.position, meleeRange, playerLayer);
       Debug.Log("Player found in " + this + "'s melee hitbox: " + playerToDamage.Length);
       for (int i=0; i < playerToDamage.Length; i++) // There is only one GameObject in the Player layer, but this logic is fine
       {
           if (playerHealthScript.currentHealth > 0)
               playerHealthScript.TakeDamage(meleeDamage);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("PlayerProjectile"))
        {
            TakeDamage(col.gameObject.GetComponent<ProjectileController>().damage);
            Destroy(col.gameObject);
        }
        else if (col.gameObject == player)
        {
            playerInRange = true;
            Debug.Log("player entered " + this + " range.");
        }
    }

    void OnTriggerExit2D(Collider2D col)
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
    }
}
