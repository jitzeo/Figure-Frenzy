using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[System.Obsolete]

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public List<GameObject> _bulletList;
    public List<string> _bulletListOnShot;

    public float bulletForce = 20f;

    private Vector2 mousePos;
    public Camera cam;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && UIManager.gameOver == false)
        {
            DataStorage.clicks++;
            Shoot();
        }

        _bulletList = _bulletList.Where(e => e != null).ToList();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        DataStorage.shots++;
        
        _bulletList.Add(bullet);
        _bulletListOnShot.Add("[" + ((Vector2)(firePoint.up * bulletForce)).ToString() + ", " +  mousePos.ToString() + "]");

        SoundManager.PlaySound(SoundManager.Sound.playerShoot);
        
        //Debug.Log(firePoint.up);
    }
}
