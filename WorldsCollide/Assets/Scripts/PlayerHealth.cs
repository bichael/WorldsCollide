﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth;
    public int currentHealth;
    private float damageCooldown;
    public float damageGracePeriod;
    private bool canBeAttacked;
    private GameObject healthUI;
    public Image damageImage;
    public AudioClip playerHurtClip;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);

    AudioSource playerAudio;
    Player playerMovement;
    bool isDead;
    bool damaged;
    List<Image> heartsInUI;

    void Awake ()
    {
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<Player>();
        currentHealth = startingHealth;
        healthUI = GameObject.FindGameObjectWithTag("HealthUI");
        GetHeartsFromUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (damageImage != null)
        {
            if (damaged)
            {
                damageImage.color = flashColor;
            } else {
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }
            damaged = false;
        }

        if (damageCooldown <= 0 && !isDead)
        {
            canBeAttacked = true;
        } else 
        {
            damageCooldown -= Time.deltaTime;
            canBeAttacked = false;
        }
        
    }

    // Creates the list of original hearts to be used for hiding later when damaged.
    private void GetHeartsFromUI()
    {
        if (healthUI == null)
            return;
        heartsInUI = new List<Image>();
        foreach (Transform heartImageChild in healthUI.transform)
            heartsInUI.Add(heartImageChild.GetComponent<Image>());
    }

    public bool CanBeAttacked()
    {
        return canBeAttacked;
    }

    public void TakeDamage(int amount)
    {
        if (playerMovement.sodaBuff == true){
            int sodaDodge = Random.Range(0,2);
            amount -= sodaDodge;
        }
        if (playerMovement.blocking == true) 
        {
            amount -= 1;
        }

        // Early exit if damage has been reduced to 0.
        if(amount == 0)
            return;

        canBeAttacked = false;
        damaged = true;
        damageCooldown = damageGracePeriod;
        currentHealth -= amount;
        // Avoid negative health.
        if (currentHealth < 0)
            currentHealth = 0;
        Debug.Log("Player took " + amount + " damage.");
        AssertPlayerHealthEqualsHearts(true);
        playerAudio.clip = playerHurtClip;
        playerAudio.Play();
        
        if (currentHealth <= 0 && !isDead) 
        {
            Death();
        }
    }

    private void Heal(int amount)
    {
        currentHealth += amount;
        AssertPlayerHealthEqualsHearts(false);
    }

    void Death()
    {
        isDead = true;
        canBeAttacked = false;
        playerAudio.clip = deathClip;
        playerAudio.Play();
        playerMovement.enabled = false;
    }

    // If damage, make sure all hearts from currentHealth to startingHealth are hidden.  If heal, make sure all hearts from 0 to currentHealth are shown.
    // The basis of this approach is that we want to avoid recalculating how many hearts are shown in the UI each time this is called bc it's messy.
    void AssertPlayerHealthEqualsHearts(bool decreaseHealth)
    {
        if (decreaseHealth)
            for (int i = 0; i < (startingHealth - currentHealth); i++) // (startingHealth - currentHealth) = Numbers of hearts that should be disabled, starting from the end.
            {
                if (healthUI != null)
                    heartsInUI[(startingHealth - 1) - i].enabled = false; // Subtract 1 because list index starts at 0.
            }
        else
            for (int i = 0; i < currentHealth; i++)
            {
                if (healthUI != null)
                    heartsInUI[i].enabled = true;
            }
    }

    // void OnCollisionEnter2D(Collision2D col)
    // {
    //     if (currentHealth > 0) // Also known as "if not dead"
    //         if (col.gameObject.tag.Equals("EnemyProjectile"))
    //             TakeDamage(col.gameObject.GetComponent<ProjectileController>().damage);
    // }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (currentHealth > 0) // Also known as "if not dead"
        {
            if (col.gameObject.tag.Equals("EnemyProjectile"))
            {
                TakeDamage(col.gameObject.GetComponent<ProjectileController>().damage);
                Destroy(col.gameObject);
            } else if (col.gameObject.tag.Equals("HealthPickUp"))
            {
                if (currentHealth < startingHealth) {
                    Heal(1);
                    col.gameObject.SetActive(false);
                }
            }
        }
    }
}
