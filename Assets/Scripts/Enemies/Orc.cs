using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : EnemyController {



    protected void Awake()
    {
        isSpirit = false;
        player = GameObject.FindGameObjectWithTag("Player1");
        targetTransform = player.transform;
        attackCD = 2f;
        health = 10f;
        speed = 5f;
        damage = 2;
        attackRange = 1f;
        detectionRange = 10f;
    }

    protected override IEnumerator Attack()
    {
        player.GetComponent<Player1Controller>().RemoveHealth(damage);
        isOnCD = true;
        yield return new WaitForSeconds(attackCD);
        isOnCD = false;
    }

}
