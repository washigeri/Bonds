using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : WeaponController
{

    private BoxCollider2D bCollider2D;
    private float chargeAcceleration;
    private float chargeDuration;
    private float strongFullRotation;
    private float strongRotationLeft;
    private float strongRotationSpeed;
    private int strongFaceRight;
    private float stunEnemyDuration;
    //private float strongResetRotation;

    protected override void Awake()
    {
        bCollider2D = GetComponent<BoxCollider2D>();
        defaultLocalRotation = new Vector3(0f, 0f, -90f);
        damage = 7f;
        range = 7f;
        speed = 2f;
        strongCD = 3f;
        skillCD = 10f;
        chargeAcceleration = 5f;
        chargeDuration = 0.3f;
        strongFullRotation = 200f;
        strongRotationLeft = strongFullRotation;
        strongRotationSpeed = strongFullRotation / 0.2f;
        stunEnemyDuration = 2f;
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
        //isOnGlobalCoolDown = true;
        yield return new WaitForSeconds(globalCD * speed);
        isAttacking = -1;
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
        yield return new WaitForSeconds(globalCD * speed);
        isOnGlobalCoolDown = false;
        isStrongOnCD = true;
        yield return new WaitForSeconds(strongCD - globalCD * speed);
        isStrongOnCD = false;
    }

    protected override IEnumerator SkillP1()
    {
        isAttacking = 2;
        player.SetSpeedMultiplier(chargeAcceleration * player.GetSpeedMutiplier());
        player.rb2d.AddForce(365f * (player.faceRight ? Vector2.right : Vector2.left), ForceMode2D.Impulse);
        isSkillOnCD = true;
        yield return new WaitForSeconds(chargeDuration);
        isAttacking = -1;
        player.SetSpeedMultiplier(player.GetSpeedMutiplier() / chargeAcceleration);
        //isOnGlobalCoolDown = true;
        //yield return new WaitForSeconds(globalCD);
        //isOnGlobalCoolDown = false;
        //yield return new WaitForSeconds(skillCD - globalCD);
        yield return new WaitForSeconds(skillCD - chargeDuration);
        isSkillOnCD = false;
    }

    protected override IEnumerator SkillP2()
    {
        if(GameManager.gameManager.player1 != null)
        {
            PlayerController player1 = GameManager.gameManager.player1.GetComponent<PlayerController>();
            player1.SetStunEnemy(true);
            player1.SetStunEnemyDuration(stunEnemyDuration);
            player1.SetSpeedMultiplier(chargeAcceleration * player1.GetSpeedMutiplier());
            player1.GetMyWeapon().SetIsAttacking(3);
            player1.rb2d.AddForce(365f * (player1.faceRight ? Vector2.right : Vector2.left), ForceMode2D.Impulse);
            isSkillOnCD = true;
            yield return new WaitForSeconds(chargeDuration);
            player1.GetMyWeapon().SetIsAttacking(-1);
            player1.SetStunEnemy(false);
            player1.SetStunEnemyDuration(0f);
            player1.SetSpeedMultiplier(player1.GetSpeedMutiplier() / chargeAcceleration);
            yield return new WaitForSeconds(skillCD - chargeDuration);
            isSkillOnCD = false;

        }
    }
}
