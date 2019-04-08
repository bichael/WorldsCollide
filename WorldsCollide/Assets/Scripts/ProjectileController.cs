using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float xSpeed = 0f;
    private float ySpeed = 0f;
    public int damage = 1; // Used in Enemy.OnTriggerEnter2D()
    public float speed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        position.x += xSpeed;
        position.y += ySpeed;
        transform.position = position;
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(5f);
        Destroy (gameObject);
    }

    public void SetProjectileVector(string direction)
    {
        if (direction == "Up")
            ySpeed = speed;
        else if (direction == "Down")
            ySpeed = -speed;
        else if (direction == "Left")
            xSpeed = -speed;
        else if (direction == "Right")
            xSpeed = speed;
    }
}
