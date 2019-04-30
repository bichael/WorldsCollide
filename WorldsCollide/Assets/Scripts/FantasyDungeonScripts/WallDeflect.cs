using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDeflect : MonoBehaviour
{
    private GameObject gary;
    private GameObject player;
    public float speed = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        gary = GameObject.FindGameObjectWithTag("Gary");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("EnemyProjectile"))
        {
            if (gary.GetComponent<Gary>().phase1) // If in phase 1, just destroy the projectile that collided.
            {
                Destroy(col.gameObject);
                return;
            }
            // Send colliding object on vector towards boss // TODO change to ricochet to actually give difficulty?
            // Vector3 target = gary.transform.position - col.gameObject.transform.position;
            Vector3 target = player.transform.position - col.gameObject.transform.position;
            // col.gameObject.transform.position = Vector2.MoveTowards(col.gameObject.transform.position, target, speed * Time.deltaTime);
            ProjectileController pc = col.gameObject.GetComponent<ProjectileController>();
            pc.SetProjectileVector("None");
            col.gameObject.GetComponent<Rigidbody2D>().velocity = target * pc.speed;
        }
    }
}
