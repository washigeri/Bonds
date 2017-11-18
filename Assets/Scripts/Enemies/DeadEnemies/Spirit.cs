using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : DeadEnemy {

    protected override void Awake()
    {
        base.Awake();
        attackCD = 2f;
        health = 100f;
        speed = 2.5f;
        damage = 2;
        attackRange = 1f;
        detectionRange = 10f;
    }

    protected override IEnumerator Attack()
    {
        if (!player.GetComponent<PlayerController>().isGod)
        {
            player.GetComponent<PlayerController>().RemoveHealth(damage * damageMultiplier);
        }
        isOnCD = true;
        yield return new WaitForSeconds(attackCD);
        isOnCD = false;
    }
}
