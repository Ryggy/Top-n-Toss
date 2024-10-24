using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounce : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.forward * speed; // Set initial velocity
    }

    public void Update()
    {
        // Top
        if(transform.position.y > 6)
        {
            // flip y direction

        }
        //Left
        if (transform.position.x < -6 )
        {
            // flip x direction

        }
        //Right
        if (transform.position.x > 6)
        {
            // flip x direction

        }
        //Bottom
        if (transform.position.y < -6)
        {
            // flip y direction

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       
        //ReflectObject(collision);

    }

    private void ReflectObject(Collision2D collision)
    {
        // Check if the projectile hit a wall
        if (collision.gameObject.CompareTag("Wall") && rb != null)
        {

            //// Calculate the reflection vector
            //Vector3 reflectDirection = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
            //rb.velocity = reflectDirection * speed; // Set new velocity

            // store the currecnt velocity
            var currentVelocity = rb.velocity;

            // set velocity to 0
            rb.velocity = new Vector2(0, 0);
            // find inverse of stored velocity
            rb.velocity = -currentVelocity;
            // set new velocity to the stored velocity

        }
    }
}
