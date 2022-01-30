using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class HeartUp : MonoBehaviour
{
    private GameObject UIManager;
    public SpriteRenderer sprite;

    private float flashDuration = 1f;
    private float flashFrame = 0.1f;


    private void Start()
    {
        UIManager = GameObject.Find("UIManager");

        int i = Random.RandomRange(1, 11);
        //Debug.Log(i);
        if (i <= 2)
        {
            StartCoroutine(destroyHeart(3f));
        } else
        {
            StartCoroutine(destroyHeart(27f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            UIManager.GetComponent<UIManager>().hitPoints(+1);
            DestroyObject(gameObject);
            SoundManager.PlaySound(SoundManager.Sound.heal);
        }
    }

    private IEnumerator destroyHeart(float t)
    {
        yield return new WaitForSeconds(t);
        for (float i = 0f; i < flashDuration*2; i += flashFrame * 2)
        {
            if (sprite.color == Color.white)
            {
                sprite.color = new Color(255, 255, 255, 0);
            }
            else
            {
                sprite.color = Color.white;
            }
            yield return new WaitForSeconds(flashFrame * 2);
        }
        for (float i = 0f; i < flashDuration; i += flashFrame)
        {
            if (sprite.color == Color.white)
            {
                sprite.color = new Color(255, 255, 255, 0);
            }
            else
            {
                sprite.color = Color.white;
            }
            yield return new WaitForSeconds(flashFrame);
        }
        if (gameObject)
        {
            Destroy(gameObject);
        }
    }
}
