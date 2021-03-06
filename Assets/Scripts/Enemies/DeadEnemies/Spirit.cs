﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : DeadEnemy {

    protected override void Awake()
    {
        base.Awake();
        attackCD = 1.5f;
        health = 59f;
        speed = 5f;
        damage = 4;
        attackRange = 2f;
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
