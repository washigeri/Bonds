using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCollider : MonoBehaviour {

    private float enterDamage;
    private float stayDamage;

    private bool player1Entered;
    private bool player2Entered;

    private float timeBetweenTwoHits;

    private void Awake()
    {
        enterDamage = 25f;
        stayDamage = 0.5f;
        player1Entered = false;
        player2Entered = false;
        timeBetweenTwoHits = 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player1"))
        {
            if (!player1Entered)
            {
                collision.gameObject.GetComponent<PlayerController>().RemoveHealth(enterDamage);
                StartCoroutine(WaitUntilNewHit(1));
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().RemoveHealth(stayDamage);
            }
        }
        if (collision.gameObject.CompareTag("Player2"))
        {
            if (!player2Entered)
            {
                collision.gameObject.GetComponent<PlayerController>().RemoveHealth(enterDamage);
                StartCoroutine(WaitUntilNewHit(2));
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().RemoveHealth(stayDamage);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            collision.gameObject.GetComponent<PlayerController>().RemoveHealth(stayDamage);
        }
    }

    private IEnumerator WaitUntilNewHit(int player)
    {
        if(player == 1)
        {
            player1Entered = true;
            yield return new WaitForSeconds(timeBetweenTwoHits);
            player1Entered = false;
        }
        else
        {
            player2Entered = true;
            yield return new WaitForSeconds(timeBetweenTwoHits);
            player2Entered = false;
        }
    }


}
