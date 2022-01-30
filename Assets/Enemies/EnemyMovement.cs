using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed;
    private float radius = 3f;
    private float angle = 0f;
    public Rigidbody2D rb;
    private bool enemy3() 
    {
        return this.gameObject.tag == "Enemy3";
    }
    private bool enemy4()
    {
        return this.gameObject.tag == "Enemy4";
    }
    
    Vector2 movement;
    void Start()
    {
        if (enemy3())
        {
            movement.Set(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            movement = movement.normalized;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemy3())
        {
            if (collision.gameObject.tag == "Border")
            {
                Vector3 collisionPoint = collision.GetContact(0).point;
                Vector2 diff = abs(transform.position - collisionPoint);
                if (diff.x > diff.y)
                {
                    movement.x = -movement.x;
                }
                else
                {
                    movement.y = -movement.y;
                }
                //Debug.Log("diff: " + diff + "\t Updated movement: " + movement);
            }
        }
        else if (enemy4())
        {

        }
    }
    private void FixedUpdate()
    {
        if (enemy4())
        {
            movement.Set(Mathf.Cos(angle), Mathf.Sin(angle));
            rb.MovePosition(rb.position + movement * radius * moveSpeed * Time.fixedDeltaTime);
            //Debug.Log("movement = " + angle + " radius = " + radius + " dt = " + Time.fixedDeltaTime + " angle = " + angle + " magnitude = " + (movement * radius * Time.fixedDeltaTime).magnitude + " angle update = " + (movement * radius * Time.fixedDeltaTime).magnitude / radius);
            angle = angle + (movement * radius * moveSpeed * Time.fixedDeltaTime).magnitude / radius;

            if (angle > (2 * Mathf.PI))
            {
                angle -= 2 * Mathf.PI;
            }

        }
        else
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public Vector2 abs(Vector2 v2) {
        v2.Set(Mathf.Abs(v2.x), Mathf.Abs(v2.y));
        return v2;
    }
}
