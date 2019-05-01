using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureDeflect : MonoBehaviour
{
    private GameObject gary;
    public float speed = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        gary = GameObject.FindGameObjectWithTag("Gary");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("EnemyProjectile"))
        {
            if (gary.GetComponent<Gary>().phase2) // If in phase 2, just destroy the projectile that collided.
            {
                Destroy(col.gameObject);
                return;
            }
            // Send colliding object on vector towards boss
            Vector3 target = (gary.transform.position - col.gameObject.transform.position).normalized;
            // col.gameObject.transform.position = Vector2.MoveTowards(col.gameObject.transform.position, target, speed * Time.deltaTime);
            ProjectileController pc = col.gameObject.GetComponent<ProjectileController>();
            pc.SetProjectileVector("None");
            col.gameObject.GetComponent<Rigidbody2D>().velocity = target * (pc.speed * 1.5f);
            Destroy(gameObject); // Destroy furniture that reflected the projectile.
        }
    }
}
