using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : AliveEnemy {

    protected override void Awake()
    {
        base.Awake();
        attackCD = 2f;
        health = 100f;
        speed = 5f;
        damage = 2;
        attackRange = 1f;
        detectionRange = 10f;
    }

    protected override IEnumerator Attack()
    {
        if (!player.GetComponent<PlayerController>().isGod)
        {
            //Debug.Log("damage done : " + damage * player.GetComponent<PlayerController>().GetDamageReceivedMultiplier() * damageMultiplier + " instead of " + damage);
            player.GetComponent<PlayerController>().RemoveHealth(damage * player.GetComponent<PlayerController>().GetDamageReceivedMultiplier() * damageMultiplier);
        }
        isOnCD = true;
        yield return new WaitForSeconds(attackCD);
        isOnCD = false;
    }

}
