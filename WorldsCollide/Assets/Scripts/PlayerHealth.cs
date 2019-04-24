using System.Collections;
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
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);

    AudioSource playerAudio;
    Player playerMovement;
    bool isDead;
    bool damaged;

    void Awake ()
    {
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<Player>();
        currentHealth = startingHealth;
        healthUI = GameObject.FindGameObjectWithTag("HealthUI");
    }

    // Update is called once per frame
    void Update()
    {
        if (damaged)
        {
            damageImage.color = flashColor;
        } else {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;

        if (damageCooldown <= 0 && !isDead)
        {
            canBeAttacked = true;
        } else 
        {
            damageCooldown -= Time.deltaTime;
            canBeAttacked = false;
        }
        
    }

    public bool CanBeAttacked()
    {
        return canBeAttacked;
    }

    public void TakeDamage(int amount)
    {
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
        Debug.Log("Player took " + amount + " damage.");
        AssertPlayerHealthEqualsHearts(currentHealth);
        playerAudio.Play();
        
        if (currentHealth <= 0 && !isDead) 
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        canBeAttacked = false;
        playerAudio.clip = deathClip;
        playerAudio.Play();
        playerMovement.enabled = false;
    }

    void AssertPlayerHealthEqualsHearts(int hp)
    {
        int numberOfHeartsInGUI = healthUI.transform.childCount;
        if (currentHealth < numberOfHeartsInGUI)
            for (int i = 0; i < (numberOfHeartsInGUI - currentHealth); i++)
                healthUI.transform.GetChild((startingHealth-i) - 1).GetComponent<Image>().enabled = false;
        else if (currentHealth > numberOfHeartsInGUI)
            for (int i = 0; i < (currentHealth - numberOfHeartsInGUI); i++)
                healthUI.transform.GetChild((startingHealth-i) - 1).GetComponent<Image>().enabled = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (currentHealth > 0) // Also known as "if not dead"
            if (col.gameObject.tag.Equals("EnemyProjectile"))
                TakeDamage(col.gameObject.GetComponent<ProjectileController>().damage);
    }
}
