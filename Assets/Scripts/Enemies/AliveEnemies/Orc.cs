using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : AliveEnemy {

    protected override void Awake()
    {
        base.Awake();
        attackCD = 2f;
        health = 10f;
        speed = 5f;
        damage = 2;
        attackRange = 1f;
        detectionRange = 10f;
    }

    protected override IEnumerator Attack()
    {
        player.GetComponent<PlayerController>().RemoveHealth(damage);
        isOnCD = true;
        yield return new WaitForSeconds(attackCD);
        isOnCD = false;
    }

}
