using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed = 0.5f;
    private float dazedTime;
    public float startDazedTime;

    private Animator anim;

    Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform> ();
        // anim = GetComponent<Animator>();
        // anim.SetBool("isRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (dazedTime <= 0)
        {
            speed = 0.5f;
        } else 
        {
            speed = 0;
            dazedTime -= Time.deltaTime;
        }

        if (health <= 0) 
        {
            Destroy(gameObject);
        }
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
    }

     public void TakeDamage(int damage)
     {
         dazedTime = startDazedTime;
         health -= damage;
         Debug.Log("Enemy took damage");
     }

    //  void OnTriggerEnter2D(Collider2D hit) {
    //      if (hit.name == "Player") {
    //          // Player can be hit
    //      }
    //  }
 
    //  void OnTriggerExit2D(Collider2D hit){
    //      if (hit.name == "Player") {
    //          // Player can no longer be hit
    //      }
    //  }
}
