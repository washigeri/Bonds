using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : WeaponController
{

    private PolygonCollider2D pCollider2D;
    private float chargeAcceleration;

    protected override void Awake()
    {
        pCollider2D = GetComponent<PolygonCollider2D>();
        damage = 7;
        range = 7;
        speed = 4;
        strongCD = 3f;
        skillCD = 10f;
        chargeAcceleration = 5f;
        isStrongOnCD = false;
        isSkillOnCD = false;
        weaponName = "Spear";
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
        Debug.Log("Strong attack");
        yield return new WaitForSeconds(0f);
    }

    protected override IEnumerator SkillP1()
    {
        Debug.Log("Skill");
        isAttacking = 2;
        player.SetMaxSpeed(chargeAcceleration * player.GetMaxSpeed());
        player.rb2d.AddForce(365f * (player.faceRight ? Vector2.right : Vector2.left), ForceMode2D.Impulse);
        isSkillOnCD = true;
        yield return new WaitForSeconds(0.3f);
        isAttacking = -1;
        player.SetMaxSpeed(player.GetMaxSpeed() / chargeAcceleration);
        isOnGlobalCoolDown = true;
        yield return new WaitForSeconds(globalCD);
        isOnGlobalCoolDown = false;
        yield return new WaitForSeconds(skillCD - globalCD);
        isSkillOnCD = false;
    }

    protected override IEnumerator SkillP2()
    {
        yield return new WaitForSeconds(0f);
    }
}
