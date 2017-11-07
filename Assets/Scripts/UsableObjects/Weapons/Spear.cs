using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : WeaponController
{

    protected override void Awake()
    {
        weaponName = "Spear";
        base.Awake();
    }

    protected override IEnumerator WeakAttack()
    {
        Debug.Log("Weak attack");
        isAttacking = true;
        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
        isOnGlobalCoolDown = true;
        yield return new WaitForSeconds(globalCD);
        isOnGlobalCoolDown = false;
    }

    protected override IEnumerator StrongAttack()
    {
        Debug.Log("Strong attack");
        yield return new WaitForSeconds(0f);
    }

    protected override IEnumerator Skill()
    {
        Debug.Log("Skill");
        yield return new WaitForSeconds(0f);
    }

}
