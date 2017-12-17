using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wasp : AliveEnemy
{

    protected override void Awake()
    {
        base.Awake();
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        attackCD = 1.5f;
        health = 31f;
        speed = 8f;
        damage = 4;
        attackRange = 1f;
        detectionRange = 25f;
    }

    protected override IEnumerator Attack()
    {
        player.GetComponent<PlayerController>().RemoveHealth(damage * damageMultiplier);
        isOnCD = true;
        yield return new WaitForSeconds(attackCD);
        isOnCD = false;
    }
}
