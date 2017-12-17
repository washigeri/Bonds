using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGhost : DeadEnemy {

    protected override void Awake()
    {
        base.Awake();
        attackCD = 3f;
        health = 100f;
        speed = 2f;
        damage = 8;
        attackRange = 2f;
        detectionRange = 7f;
    }

    protected override IEnumerator Attack()
    {

        player.GetComponent<PlayerController>().RemoveHealth(damage * damageMultiplier);
        isOnCD = true;
        yield return new WaitForSeconds(attackCD);
        isOnCD = false;
    }
}
