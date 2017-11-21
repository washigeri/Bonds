using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketController : MonoBehaviour {

     
    public float damageDoneMultiplier;
    public float damageReceivedMultiplier;
    public float enemySpeedMultiplier;
    public float enemySpeedMultiplierDuration;
    public float speedMultiplier;

    private bool hasOwner;
    private bool isPlayerSet;

    private PlayerController player;


    protected virtual void Awake()
    {
        hasOwner = false;
        isPlayerSet = false;
        player = null;
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButtonDown("InteractP1") || Input.GetButtonDown("InteractP2"))
        {
            if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                playerController.DropTrinket();
                gameObject.transform.parent = collision.gameObject.transform.Find("Hand");
                transform.localPosition = Vector3.zero;
                hasOwner = true;
                player = playerController;
            }
        }
    }

    protected void Update()
    {
        if (hasOwner)
        {
            if (!isPlayerSet)
            {
                SetPlayer();
            }
        }
    }

    private void SetPlayer()
    {
        player.SetDamageDoneMultiplier(player.GetDamageDoneMultiplier() * damageDoneMultiplier);
        player.SetDamageReceivedMultiplier(player.GetDamageReceivedMultiplier() * damageReceivedMultiplier);
        player.SetMaxSpeed(player.GetMaxSpeed() * speedMultiplier);
        player.SetEnemySpeedMultiplier(player.GetEnemySpeedMultiplier() * enemySpeedMultiplier);
        player.SetEnemySpeedMultiplierDuration(Mathf.Max(enemySpeedMultiplierDuration, player.GetEnemySpeedMultiplierDuration()));
        isPlayerSet = true;
    }

    public void SetOwner(bool hasOwner)
    {
        this.hasOwner = hasOwner;
    }
}
