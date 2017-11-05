using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : EnemyController {

    protected void Awake()
    {
        health = 10f;
        speed = 5f;
        damage = 2f;
        attackRange = 1f;
        detectionRange = 10f;
    }

    protected override IEnumerator Attack()
    {
        Debug.Log("J'attaque");
        yield return new WaitForSeconds(0f);
    }

}
