using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Animator _anim;
    private AudioSource grenadeAudio;
    private Rigidbody2D _rb;
    
    public int damage = 1;
    public LayerMask enemyLayer;
    public AudioClip explosionClip;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyGrenade());
        _anim = GetComponent<Animator>();
        grenadeAudio = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyGrenade()
    {
        yield return new WaitForSeconds(0.75f);
        // Do hitscan and deal damage.
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, 1.0f, enemyLayer);
        Debug.Log("# enemies found in grenade hitbox: " + enemiesToDamage.Length);
        for (int i=0; i < enemiesToDamage.Length; i++)
        {
            // damage enemy
            enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
        }
        _anim.SetTrigger("Explode");
        _rb.velocity = Vector2.zero;
        grenadeAudio.clip = explosionClip;
        grenadeAudio.Play();
        yield return new WaitForSeconds(1.84f);
        Destroy (gameObject);
    }
}
