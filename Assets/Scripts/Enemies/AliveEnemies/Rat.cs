using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : AliveEnemy {

    protected override void Awake()
    {
        base.Awake();
        attackCD = 1f;
        health = 21f;
        speed = 15f;
        damage = 2;
        attackRange = 1f;
        detectionRange = 20f;
    }

    protected override IEnumerator Attack()
    {
        player.GetComponent<PlayerController>().RemoveHealth(damage * damageMultiplier);
        isOnCD = true;
        yield return new WaitForSeconds(attackCD);
        isOnCD = false;
    }
}
