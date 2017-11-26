using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : WeaponController
{
    private BoxCollider2D bCollider2D;
    private float disengageAcceleration;
    private float disengageDuration;

    protected override void Awake()
    {
        bCollider2D = GetComponent<BoxCollider2D>();
        defaultLocalRotation = Vector3.zero;
        damage = 5;
        range = 10;
        speed = 1.5f;
        localGlobalCD = globalCD * speed;
        strongCD = 6f;
        skillCD = 4f;
        disengageAcceleration = 5f;
        disengageDuration = 0.2f;
        isStrongOnCD = false;
        isSkillOnCD = false;
        weaponName = "Bow";
        base.Awake();
    }

    protected override void Update()
    {
        bCollider2D.enabled = !hasOwner || isAttacking == 3;
        base.Update();
    }

    protected override IEnumerator WeakAttack()
    {
        PlayWeakSound();
        isAttacking = 0;
        Shoot(GetDirection());
        yield return new WaitForSeconds(0.25f);
        isAttacking = -1;
        isOnGlobalCoolDown = true;
        yield return new WaitForSeconds(localGlobalCD * player.GetAttackSpeedMultipler());
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
        GameObject arrow = Instantiate(Resources.Load("Prefabs/Weapons/Projectiles/Arrow"), transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        arrow.GetComponent<Arrow>().SetParameters(owner, attacksDamage[isAttacking], enemyTag, direction, gameObject.transform.position);
    }

    protected override IEnumerator StrongAttack()
    {
        PlayStrongSound();
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
        float currentGCD = localGlobalCD * player.GetAttackSpeedMultipler();
        yield return new WaitForSeconds(currentGCD);
        isOnGlobalCoolDown = false;
        yield return new WaitForSeconds(strongCD - currentGCD);
        isStrongOnCD = false;
    }

    protected override IEnumerator SkillP1()
    {
        PlaySkillSound();
        player.SetIsBlocked(true);
        player.SetMaxSpeed(disengageAcceleration * player.GetMaxSpeed());
        Physics2D.IgnoreLayerCollision(8, 10, true);
        player.moveHability = true;
        yield return new WaitForSeconds(0.1f);
        player.rb2d.AddForce(365f * (player.faceRight ? Vector2.left : Vector2.right), ForceMode2D.Impulse);
        isSkillOnCD = true;
        yield return new WaitForSeconds(disengageDuration);
        player.SetIsBlocked(false);
        player.SetMaxSpeed(player.GetMaxSpeed() / disengageAcceleration);
        Physics2D.IgnoreLayerCollision(8, 10, false);
        yield return new WaitForSeconds(skillCD - disengageDuration);
        isSkillOnCD = false;
    }

    protected override IEnumerator SkillP2()
    {
        PlaySkillSound();
        GameObject arrow = Instantiate(Resources.Load("Prefabs/Weapons/Projectiles/EnhancedArrow"), gameObject.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        Vector3 direction = (player.GetDirV() > 0) ? Vector3.up : Vector3.down;
        arrow.GetComponent<EnhancedArrow>().SetParameters(1.5f, 5f, direction, gameObject.transform.position);
        isSkillOnCD = true;
        isOnGlobalCoolDown = true;
        float currentGCD = localGlobalCD * player.GetAttackSpeedMultipler();
        yield return new WaitForSeconds(currentGCD);
        isOnGlobalCoolDown = false;
        yield return new WaitForSeconds(skillCD - currentGCD);
        isSkillOnCD = false;
    }
}
