using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : AliveEnemy {

    protected override void Awake()
    {
        base.Awake();
        attackCD = 2.5f;
        health = 120f;
        speed = 3f;
        damage = 10;
        attackRange = 1.5f;
        detectionRange = 10f;
    }

    protected override IEnumerator Attack()
    {
        player.GetComponent<PlayerController>().RemoveHealth(damage * damageMultiplier);
        isOnCD = true;
        yield return new WaitForSeconds(attackCD);
        isOnCD = false;
    }
}
