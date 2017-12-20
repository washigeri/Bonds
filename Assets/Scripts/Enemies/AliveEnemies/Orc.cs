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
        attackRange = 1.5f;
        detectionRange = 10f;
        isFlying = false;
    }

    protected override IEnumerator Attack()
    {
        player.GetComponent<PlayerController>().RemoveHealth(damage * damageMultiplier);
        isOnCD = true;
        yield return new WaitForSeconds(attackCD);
        isOnCD = false;
    }

}
