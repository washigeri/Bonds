using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketController : MonoBehaviour {

     
    public float damageDoneMultiplier;
    public float damageReceivedMultiplier;
    public float enemySpeedMultiplier;
    public float enemySpeedMultiplierDuration;
    public float speedMultiplier;
    public float enemyBleedPercentage;
    public float enemyBleedDuration;

    private bool hasOwner;
    //private bool isPlayerSet;

    private PlayerController player;


    protected virtual void Awake()
    {
        hasOwner = false;
        //isPlayerSet = false;
        player = null;
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (!hasOwner)
        {
            if (Input.GetButtonDown("InteractP1"))
            {
                if (collision.gameObject.CompareTag("Player1"))
                {
                    SwapTrinket(collision);
                }
            }
            else if (Input.GetButtonDown("InteractP2"))
            {
                if (collision.gameObject.CompareTag("Player2"))
                {
                    SwapTrinket(collision);
                }
            }
        }
    }

    private void SwapTrinket(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<PlayerController>();
        player.DropTrinket();
        gameObject.transform.parent = collision.gameObject.transform.Find("Hand");
        player.SetMyTrinket(this);
        hasOwner = true;
        transform.localPosition = Vector3.zero;
        ToggleSprite();
        SetPlayer();
        GameManager.gameManager.RemoveObjectToBeCleaned(gameObject.GetInstanceID());
    }

    public void ToggleSprite()
    {
        transform.GetComponent<SpriteRenderer>().enabled = !transform.GetComponent<SpriteRenderer>().enabled;
        transform.GetComponent<BoxCollider2D>().enabled = !transform.GetComponent<BoxCollider2D>().enabled;
    }

    private void SetPlayer()
    {
        player.SetDamageDoneMultiplier(player.GetDamageDoneMultiplier() * damageDoneMultiplier);
        player.SetDamageReceivedMultiplier(player.GetDamageReceivedMultiplier() * damageReceivedMultiplier);
        player.SetSpeedMultiplier(player.GetSpeedMutiplier() * speedMultiplier);
        player.SetEnemySpeedMultiplier(player.GetEnemySpeedMultiplier() * enemySpeedMultiplier);
        player.SetEnemySpeedMultiplierDuration(Mathf.Max(enemySpeedMultiplierDuration, player.GetEnemySpeedMultiplierDuration()));
        player.SetEnemyBleedPercentage(enemyBleedPercentage);
        player.SetEnemyBleedDuration(enemyBleedDuration);
        //isPlayerSet = true;
    }

    public void ResetPlayer()
    {
        player.SetDamageDoneMultiplier(player.GetDamageDoneMultiplier() / damageDoneMultiplier);
        player.SetDamageReceivedMultiplier(player.GetDamageReceivedMultiplier() / damageReceivedMultiplier);
        player.SetSpeedMultiplier(player.GetSpeedMutiplier() / speedMultiplier);
        player.SetEnemySpeedMultiplier(player.GetEnemySpeedMultiplier() / enemySpeedMultiplier);
        player.SetEnemyBleedPercentage(0f);
        player.SetEnemyBleedDuration(0f);
        player.SetEnemySpeedMultiplierDuration(0f);
    }

    public void SetOwner(bool hasOwner)
    {
        this.hasOwner = hasOwner;
    }
}
