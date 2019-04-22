﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    private float direction;
    // public float speed;
    // public bool weaponEquipped;
    public GameObject weapon;
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    private float timeBtwProject;
    public float startTimeBtwProject;
    public float timeBtwShield;
    public float startTimeBtwShield;
    public Transform attackPos;
    public LayerMask enemyLayer;
    public float meleeRange;
    public int meleeDamage;
    public GameObject bullet;
    public bool blocking;
    public bool attacking;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerAnimatorAndPosition();
        if (timeBtwShield <= 0)
        {
            blocking = false;
            AttemptPlayerShield();
        }
        else
        {
            timeBtwShield -= Time.deltaTime;
        }
        if (timeBtwAttack <= 0) // If this checks for KeyCode.Space instead, it fails to register sometimes.
            AttemptPlayerAttack();
        else
            timeBtwAttack -= Time.deltaTime;

        if (timeBtwProject <= 0)
            AttemptPlayerProjectile();
        else
            timeBtwProject -= Time.deltaTime;

    }

    void SetAnimatorVariables(Vector3 movementVector)
    {
        float horz = movementVector.x;
        float vert = movementVector.y;
        direction = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) / Mathf.PI;
        animator.SetFloat("Horizontal", horz);
        animator.SetFloat("Vertical", vert);
        animator.SetFloat("Direction", direction);
    }

    void UpdatePlayerAnimatorAndPosition()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f);
        animator.SetFloat("Magnitude", movement.magnitude);
        if (movement != Vector3.zero) // Avoid player always facing "0" direction when idle
        {
            SetAnimatorVariables(movement);
            transform.position = transform.position + movement * Time.deltaTime;
            
            // set melee hitbox direction
            float absX = Mathf.Abs(movement.x);
            float absY = Mathf.Abs(movement.y);
            if (absX > absY) // Player is moving more horz than vert
            {
                if (movement.x > 0) // Player is moving right
                {
                    attackPos.position = transform.position + (0.1f * Vector3.right); // Set weapon to be on the right of the player
                } 
                else if (movement.x < 0)
                {
                    attackPos.position = transform.position + (0.1f * Vector3.left); // Set weapon to be on the left of the player
                }
            }
            else // Player is moving more vert than horz
            {
                if (movement.y > 0) 
                {
                    attackPos.position = transform.position + (0.1f * Vector3.up); // Set weapon above player
                }
                else if (movement.y < 0)
                {
                    attackPos.position = transform.position + (0.1f * Vector3.down); 
                }
            }
        }
    }

    void AttemptPlayerShield()
    {
        if(Input.GetKey(KeyCode.L) && (attacking == false)){
            //animator.SetTrigger("ShieldUp");
            blocking = true;
            timeBtwShield = startTimeBtwShield;
        }
        /*
        if(Input.GetKeyUp(KeyCode.X)){
            blocking = false;
        }
        */

    }

    void AttemptPlayerAttack()
    {
        if (Input.GetKey(KeyCode.J))
        {
            // player can attack
            blocking = false;
            animator.SetTrigger("Attacking");
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, meleeRange, enemyLayer);
            Debug.Log("# enemies found in Player melee hitbox: " + enemiesToDamage.Length);
            for (int i=0; i < enemiesToDamage.Length; i++)
            {
                // damage enemy
                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(meleeDamage);
            }
            timeBtwAttack = startTimeBtwAttack;
        }
    }

    void AttemptPlayerProjectile()
    {
        if (Input.GetKey(KeyCode.K))
        {
            blocking = false;
            animator.SetTrigger("CastingFireball");
            Vector2 projectilePosition = transform.position;
            //if (direction == -0.5f) // if facing left
            if ((direction < -0.25)&(direction >= -0.75))
            {
                projectilePosition.x -= 0.15f; // offset from player pivot (to avoid shooting from chest)
                projectilePosition.y += 0.15f; // offset from player feet pivot
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, Quaternion.identity);
                go.GetComponent<ProjectileController>().SetProjectileVector("Left");
            }
            //else if (direction == 0) // if facing up
            else if ((direction < 0.25)&(direction >= -0.25))
            {
                projectilePosition.y += 0.15f;
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, Quaternion.identity);
                go.GetComponent<ProjectileController>().SetProjectileVector("Up");
            }
            //else if (direction == 0.5f) // if facing right
            else if ((direction < 0.75)&(direction >= 0.25))
            {
                projectilePosition.x += 0.15f;
                projectilePosition.y += 0.15f;
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, Quaternion.identity);
                go.GetComponent<ProjectileController>().SetProjectileVector("Right");
                
            }
            //else if (direction == 1) // if facing down
            else if ((direction < 1.25)&(direction >= 0.75))
            {
                projectilePosition.y -= 0.15f;
                GameObject go = (GameObject)Instantiate (bullet, projectilePosition, Quaternion.identity);
                go.GetComponent<ProjectileController>().SetProjectileVector("Down");
            }
            timeBtwProject = startTimeBtwProject;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, meleeRange);
    }



}
