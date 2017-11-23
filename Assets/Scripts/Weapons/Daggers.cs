﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daggers : WeaponController
{

    private BoxCollider2D bCollider2D;

    protected override void Awake()
    {
        bCollider2D = GetComponent<BoxCollider2D>();
        damage = 4;
        range = 2;
        speed = 10;
        weaponName = "Daggers";
        base.Awake();
    }

    protected override void Update()
    {
        bCollider2D.enabled = (!isOnGlobalCoolDown && (isAttacking >= 0)) || !hasOwner;
        base.Update();
    }

    protected override IEnumerator WeakAttack()
    {
        //Debug.Log("Weak attack");
        isAttacking = 0;
        yield return new WaitForSeconds(0.25f);
        isAttacking = -1;
        isOnGlobalCoolDown = true;
        yield return new WaitForSeconds(globalCD);
        isOnGlobalCoolDown = false;
    }

    protected override IEnumerator StrongAttack()
    {
        yield return new WaitForSeconds(0f);
    }


    protected override IEnumerator SkillP1()
    {
        yield return new WaitForSeconds(0f);
    }

    protected override IEnumerator SkillP2()
    {
        yield return new WaitForSeconds(0f);
    }
}
