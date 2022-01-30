using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]

public class EnemyBullet : MonoBehaviour
{
    public GameObject hitEffect;
    private GameObject UI;
    private Transform player;
    private bool missed;

    private void Start()
    {
        UI = GameObject.Find("UIManager");
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    [System.Obsolete]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // A lot of information can be gathered in this function
        
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.05f);
        Destroy(gameObject);

        if (collision.gameObject.tag == "Player")
        {
            if (!UIManager.gameOver && !player.GetComponent<PlayerMovement>().invincible) {
                SoundManager.PlaySound(SoundManager.Sound.playerHit);
                UI.GetComponent<UIManager>().hitPoints(-1);
                player.GetComponent<PlayerMovement>().StartInvincibility();

                UI.GetComponent<UIManager>().damageScreen.SetActive(true);
                UIManager.damage = true;
                UIManager.damageTime = Time.time;
            }
            DataStorage.nearHits--;

            //SoundManager.PlaySound(SoundManager.Sound.enemyHit);
        }
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= 1.2f && !missed){
            DataStorage.nearHits++;
            missed = true;
        }
    }
}
