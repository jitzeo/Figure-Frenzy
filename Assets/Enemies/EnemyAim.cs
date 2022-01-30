using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviour
{

    public Rigidbody2D rb;
    private Transform player;

    Vector2 movement;
    Vector2 playerPos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        playerPos = player.transform.position;
        // playerPos = cam.ScreenToWorldPoint(player.transform.position);
    }

    private void FixedUpdate()
    {
        Vector2 lookDir = playerPos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}
