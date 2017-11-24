using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : WeaponController
{

    private BoxCollider2D bCollider2D;
    private float chargeAcceleration;
    private float strongFullRotation;
    private float strongRotationLeft;
    private float strongRotationSpeed;
    private int strongFaceRight;
    //private float strongResetRotation;

    protected override void Awake()
    {
        bCollider2D = GetComponent<BoxCollider2D>();
        defaultLocalRotation = new Vector3(0f, 0f, -90f);
        damage = 7;
        range = 7;
        speed = 4;
        strongCD = 3f;
        skillCD = 10f;
        chargeAcceleration = 5f;
        strongFullRotation = 200f;
        strongRotationLeft = strongFullRotation;
        strongRotationSpeed = strongFullRotation / 0.2f;
        isStrongOnCD = false;
        isSkillOnCD = false;
        weaponName = "Spear";
        base.Awake();
    }

    protected override void Update()
    {
        bCollider2D.enabled = (!isOnGlobalCoolDown && (isAttacking >= 0)) || !hasOwner;
        if (isAttacking == 1)
        {
            float rotateAngle = strongRotationSpeed * Time.deltaTime;
            transform.parent.Rotate(Vector3.forward, strongFaceRight * rotateAngle);
            strongRotationLeft -= rotateAngle;
        }
        base.Update();
    }

    protected override IEnumerator WeakAttack()
    {
        isAttacking = 0;
        yield return new WaitForSeconds(0.25f);
        isAttacking = -1;
        isOnGlobalCoolDown = true;
        yield return new WaitForSeconds(globalCD);
        isOnGlobalCoolDown = false;
    }

    protected override IEnumerator StrongAttack()
    {
        isAttacking = 1;
        strongFaceRight = (player.faceRight ? 1 : -1);
        yield return new WaitUntil(() => strongRotationLeft <= 0f);
        isAttacking = -1;
        transform.parent.eulerAngles = Vector3.zero;
        strongRotationLeft = strongFullRotation;
        isOnGlobalCoolDown = true;
        yield return new WaitForSeconds(globalCD);
        isOnGlobalCoolDown = false;
        isStrongOnCD = true;
        yield return new WaitForSeconds(strongCD - globalCD);
        isStrongOnCD = false;
    }

    protected override IEnumerator SkillP1()
    {
        isAttacking = 2;
        player.SetSpeedMultiplier(chargeAcceleration * player.GetSpeedMutiplier());
        player.rb2d.AddForce(365f * (player.faceRight ? Vector2.right : Vector2.left), ForceMode2D.Impulse);
        isSkillOnCD = true;
        yield return new WaitForSeconds(0.3f);
        isAttacking = -1;
        player.SetSpeedMultiplier(player.GetSpeedMutiplier() / chargeAcceleration);
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
