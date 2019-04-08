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

    private Animator anim;

    Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform> ();
        anim = GetComponent<Animator>();
        // anim.SetBool("isRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
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

        if (health <= 0) 
        {
            Destroy(gameObject);
        }

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);

        Vector3 enemyPlayerDifferenceVector = transform.position - playerTransform.position;
        float enemyPlayerAngle = (Mathf.Atan2(enemyPlayerDifferenceVector.y, enemyPlayerDifferenceVector.x) * Mathf.Rad2Deg) / 180;
        enemyPlayerAngle = Mathf.Round(enemyPlayerAngle * 2) / 2; // Round to nearest 0.5
        anim.SetFloat("Direction", enemyPlayerAngle);
        anim.SetFloat("Magnitude", enemyPlayerDifferenceVector.magnitude);
        // Debug.Log(this + "moving towards player at diection: " + enemyPlayerAngle);
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
