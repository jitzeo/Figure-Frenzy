using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]

public class PlayerBullet : MonoBehaviour
{
    public GameObject hitEffect;
    private GameObject UIManager;

    public GameObject heartUpPrefab;

    void Start()
    {
        UIManager = GameObject.Find("UIManager");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // A lot of information can be gathered in this function

        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.05f);
        Destroy(gameObject);

        if (collision.gameObject.layer == 8)
        {
            UIManager.GetComponent<UIManager>().AddScore(10);
            Destroy(collision.gameObject);
            SoundManager.PlaySound(SoundManager.Sound.enemyHit);
            DataStorage.hits++;

            int randomHeart = Random.Range(1, 101);
            if(randomHeart <= 3)
            {
                Instantiate(heartUpPrefab, collision.transform.position, Quaternion.identity);
            }
        } else if (collision.gameObject.layer == 7)
        {
            DataStorage.hits++;
            SoundManager.PlaySound(SoundManager.Sound.bulletHit);
        }
        
    }
 }
