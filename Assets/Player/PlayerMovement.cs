using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public static float speedMultiplier = 1f;

    public Rigidbody2D rb;
    public Camera cam;
    public SpriteRenderer sprite;

    Vector2 movement;
    Vector2 mousePos;

    public bool invincible = false;
    private float invincibiltyDuration = 0.6f;
    private float invincibilityFrame = 0.1f;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        if (UIManager.gameOver == false)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * speedMultiplier * Time.fixedDeltaTime);
            Vector2 lookDir = mousePos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }

    public IEnumerator InvincibilityFrames()
    {
        invincible = true;
        for (float i = 0f; i < invincibiltyDuration; i += invincibilityFrame) {
            if (sprite.color == Color.white)
            {
                sprite.color = new Color(255, 255, 255, 0);
            }
            else
            {
                sprite.color = Color.white;
            }
            yield return new WaitForSeconds(invincibilityFrame);
        }
        invincible = false;
        sprite.color = Color.white;
    }

    public void StartInvincibility()
    {
        StartCoroutine(InvincibilityFrames());
    }
}
