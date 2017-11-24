﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : WeaponController
{
    private BoxCollider2D bCollider2D;
    private float disengageAcceleration;

    protected override void Awake()
    {
        bCollider2D = GetComponent<BoxCollider2D>();
        defaultLocalRotation = Vector3.zero;
        damage = 5;
        range = 10;
        speed = 5;
        strongCD = 6f;
        skillCD = 4f;
        disengageAcceleration = 5f;
        isStrongOnCD = false;
        isSkillOnCD = false;
        weaponName = "Bow";
        base.Awake();
    }

    protected override void Update()
    {
        bCollider2D.enabled = !hasOwner;
        base.Update();
    }

    protected override IEnumerator WeakAttack()
    {
        isAttacking = 0;
        Shoot(GetDirection());
        yield return new WaitForSeconds(0.25f);
        isAttacking = -1;
        isOnGlobalCoolDown = true;
        yield return new WaitForSeconds(globalCD);
        isOnGlobalCoolDown = false;
    }

    private Vector3 GetDirection()
    {
        if (player.GetDirH() == 0f && player.GetDirV() != 0f)
        {
           return Vector3.up;
        }
        else
        {
            return (player.faceRight ? Vector3.right : Vector3.left);
        }
    }

    private void Shoot(Vector3 direction)
    {
        GameObject arrow = Instantiate(Resources.Load("Prefabs/Weapons/Projectiles/Arrow"), gameObject.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        arrow.GetComponent<Arrow>().SetParameters(owner, attacksDamage[isAttacking], enemyTag, direction, gameObject.transform.position);
    }

    protected override IEnumerator StrongAttack()
    {
        isAttacking = 1;
        Vector3 direction = GetDirection();
        if(direction == Vector3.left)
        {
            Shoot(Vector3.left + Vector3.down);
            Shoot(Vector3.left);
            Shoot(Vector3.left + Vector3.up);
        }
        else if(direction == Vector3.right)
        {
            Shoot(Vector3.right + Vector3.down);
            Shoot(Vector3.right);
            Shoot(Vector3.right + Vector3.up);
        }
        else if(direction == Vector3.up)
        {
            Shoot(Vector3.up + Vector3.left);
            Shoot(Vector3.up);
            Shoot(Vector3.up + Vector3.right);
        }
        isAttacking = -1;
        isStrongOnCD = true;
        isOnGlobalCoolDown = true;
        yield return new WaitForSeconds(globalCD);
        isOnGlobalCoolDown = false;
        yield return new WaitForSeconds(strongCD - globalCD);
        isStrongOnCD = false;
    }

    protected override IEnumerator SkillP1()
    {
        player.SetIsBlocked(true);
        player.SetMaxSpeed(disengageAcceleration * player.GetMaxSpeed());
        player.moveHability = true;
        yield return new WaitForSeconds(0.1f);
        player.rb2d.AddForce(365f * (player.faceRight ? Vector2.left : Vector2.right), ForceMode2D.Impulse);
        isSkillOnCD = true;
        yield return new WaitForSeconds(0.2f);
        player.SetIsBlocked(false);
        player.SetMaxSpeed(player.GetMaxSpeed() / disengageAcceleration);
        yield return new WaitForSeconds(skillCD);
        isSkillOnCD = false;
    }

    protected override IEnumerator SkillP2()
    {
        GameObject arrow = Instantiate(Resources.Load("Prefabs/Weapons/Projectiles/EnhancedArrow"), gameObject.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        Vector3 direction = (player.GetDirV() > 0) ? Vector3.up : Vector3.down;
        arrow.GetComponent<EnhancedArrow>().SetParameters(1.5f, 5f, direction, gameObject.transform.position);
        isSkillOnCD = true;
        isOnGlobalCoolDown = true;
        yield return new WaitForSeconds(globalCD);
        isOnGlobalCoolDown = false;
        yield return new WaitForSeconds(12f - globalCD);
        isSkillOnCD = false;
    }
}
