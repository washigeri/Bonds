using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponController {

    private PolygonCollider2D pCollider2D;

    protected override void Awake()
    {
        pCollider2D = GetComponent<PolygonCollider2D>();
        damage = 10;
        range = 5;
        speed = 3;
        weaponName = "Sword";
        base.Awake();
    }

    protected override void Update()
    {
        pCollider2D.enabled = (!isOnGlobalCoolDown && (isAttacking >= 0)) || !hasOwner;
        base.Update();
    }

    protected override IEnumerator WeakAttack()
    {
        Debug.Log("Weak attack");
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

    protected override IEnumerator Skill()
    {
        yield return new WaitForSeconds(0f);
    }

}
