using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : WeaponController
{

    private BoxCollider2D bCollider2D;

    private float weakTranslationLeft;
    private float weakTranslationDuration;
    private float weakTranslationSpeed;

    private float chargeAcceleration;
    private float chargeDuration;
    private float strongFullRotation;
    private float strongRotationLeft;
    private float strongRotationSpeed;
    private int strongFaceRight;

    private float stunEnemyDuration;

    protected override void Awake()
    {
        base.Awake();
        bCollider2D = GetComponent<BoxCollider2D>();
        defaultLocalRotation = new Vector3(0f, 0f, -90f);
        damage = 7f;
        range = 7f;
        speed = 1.5f;
        localGlobalCD = speed * globalCD;

        range = 0.75f;
        weakTranslationLeft = range;
        weakTranslationDuration = globalCD / 3f;
        weakTranslationSpeed = range / weakTranslationDuration;

        strongCD = 3f;
        skillCD = 8f;
        chargeAcceleration = 5f;
        chargeDuration = 0.3f;

        strongFullRotation = 200f;
        strongRotationLeft = strongFullRotation;
        strongRotationSpeed = strongFullRotation / 0.2f;
        stunEnemyDuration = 2f;
        isStrongOnCD = false;

        isSkillOnCD = false;
        weaponName = "Spear";
        weaponID = 0;

        attacksDamage[0] = 25f;
        attacksDamage[1] = 40f;
        attacksDamage[2] = 50f;
    }

    protected override void Update()
    {
        bCollider2D.enabled = (!isOnGlobalCoolDown && (isAttacking >= 0)) || !hasOwner;
        if(isAttacking == 0)
        {
            float translationNorm = weakTranslationSpeed * Time.deltaTime;
            transform.Translate(Vector3.up * translationNorm);
            weakTranslationLeft -= translationNorm;
        }
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
        PlayWeakSound();
        isAttacking = 0;
        yield return new WaitUntil(() => weakTranslationLeft <= 0f);
        isAttacking = -1;
        isOnGlobalCoolDown = true;
        weakTranslationLeft = range;
        transform.localPosition = defaultLocalPosition;
        yield return new WaitForSeconds(localGlobalCD * player.GetAttackSpeedMultipler() - weakTranslationDuration);
        isOnGlobalCoolDown = false;
    }

    protected override IEnumerator StrongAttack()
    {
        PlayStrongSound();
        isAttacking = 1;
        strongFaceRight = (player.faceRight ? 1 : -1);
        yield return new WaitUntil(() => strongRotationLeft <= 0f);
        isAttacking = -1;
        transform.parent.eulerAngles = Vector3.zero;
        strongRotationLeft = strongFullRotation;
        isOnGlobalCoolDown = true;
        float currentGCD = localGlobalCD * player.GetAttackSpeedMultipler();
        yield return new WaitForSeconds(currentGCD);
        isOnGlobalCoolDown = false;
        isStrongOnCD = true;
        yield return new WaitForSeconds(strongCD - currentGCD);
        isStrongOnCD = false;
    }

    protected override IEnumerator SkillP1()
    {
        PlaySkillSound();
        isAttacking = 2;
        player.SetSpeedMultiplier(chargeAcceleration * player.GetSpeedMutiplier());
        player.rb2d.AddForce(365f * (player.faceRight ? Vector2.right : Vector2.left), ForceMode2D.Impulse);
        isSkillOnCD = true;
        yield return new WaitForSeconds(chargeDuration);
        isAttacking = -1;
        player.SetSpeedMultiplier(player.GetSpeedMutiplier() / chargeAcceleration);
        yield return new WaitForSeconds(skillCD - chargeDuration);
        isSkillOnCD = false;
    }

    protected override IEnumerator SkillP2()
    {
        if(GameManager.gameManager.player1 != null)
        {
            PlaySkillSound();
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
