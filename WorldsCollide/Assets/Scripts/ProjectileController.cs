using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float xSpeed = 0f;
    private float ySpeed = 0f;
    public int damage = 1; // Used in Enemy.OnTriggerEnter2D()
    public float speed = 0.05f;
    public float range;
    public float daze_mod = 0.0f;
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
        yield return new WaitForSeconds(0.1f * range);
        Destroy (gameObject);
    }

    // Alternate unused approach related to shooting a projectile directly at the player.
    // public void SetEnemyProjectileVector(Vector3 enemyPlayerDifferenceVector)
    // {
    //     // The below conditional blocks will have projectiles shoot 4 diagonal directions.
    //     if (enemyPlayerDifferenceVector.x < 0)
    //         xSpeed = speed;
    //     else if (enemyPlayerDifferenceVector.x > 0)
    //         xSpeed = -speed;
    //     if (enemyPlayerDifferenceVector.y < 0)
    //         ySpeed = speed;
    //     else if (enemyPlayerDifferenceVector.y > 0)
    //         ySpeed = -speed;
    // }

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
        else if (direction == "UpLeft")
        {
            xSpeed = -speed;
            ySpeed = speed;
        }
        else if (direction == "UpRight")
        {
            xSpeed = speed;
            ySpeed = speed;
        }
        else if (direction == "DownLeft")
        {
            xSpeed = -speed;
            ySpeed = -speed;
        }
        else if (direction == "DownRight")
        {
            xSpeed = speed;
            ySpeed = -speed;
        } else if (direction == "None")
        {
            xSpeed = 0;
            ySpeed = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Debug.Log("Collision detected IN NEW SCRIPT!");
        if (col.gameObject.tag.Equals("Player"))
            Destroy(gameObject);
    }
}
