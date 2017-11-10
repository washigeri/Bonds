using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : AliveEnemy {

    protected override IEnumerator Attack()
    {
        yield return new WaitForSeconds(0f);
    }

    // Use this for initialization
    protected override void Awake ()
    {
        base.Awake();
        attackCD = 0f;
        health = 1000f;
        speed = 0f;
        damage = 0;
        attackRange = 0f;
        detectionRange = 0f;
    }

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
	}
}
