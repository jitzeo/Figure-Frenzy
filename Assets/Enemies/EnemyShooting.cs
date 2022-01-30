using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting: MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    SpriteRenderer enemySprite;
    Color enemyColor;

    SpriteRenderer bulletSprite;
    Color bulletColor;

    public float bulletForce;
    public float shotInterval;

    void Start()
    {
        InvokeRepeating("Shoot", Random.Range(1f,2f), shotInterval);
        enemySprite = GetComponent<SpriteRenderer>();
        enemyColor = enemySprite.color;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //bulletSprite = bullet.GetComponent<SpriteRenderer>();
        //bulletSprite.color = Color.green;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        SoundManager.PlaySound(SoundManager.Sound.enemyShoot);
    }
}
