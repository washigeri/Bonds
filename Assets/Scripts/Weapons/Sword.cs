﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponController {

    private BoxCollider2D bCollider2D;

    private float weakTranslationLeft;
    private float weakTranslationDuration;
    private float weakTranslationSpeed;

    private float strongFullRotation;
    private float strongRotationLeft;
    private float strongRotationSpeed;
    private float strongDuration;

    private float parryDuration;

    private float shieldDuration;

    protected override void Awake()
    {
        base.Awake();
        bCollider2D = GetComponent<BoxCollider2D>();
        defaultLocalRotation = new Vector3(0f, 0f, -90f);
        damage = 10;
        range = 0.75f;
        weakTranslationLeft = range;
        weakTranslationDuration = globalCD / 3f;
        weakTranslationSpeed = range / weakTranslationDuration;
        speed = 2f;
        localGlobalCD = globalCD * speed;
        strongCD = 5f;
        skillCD = 2f;
        isStrongOnCD = false;
        isSkillOnCD = false;
        parryDuration = 0.25f;
        strongFullRotation = 1080f;
        strongRotationLeft = strongFullRotation;
        strongDuration = 2f;
        strongRotationSpeed = strongFullRotation / strongDuration;
        shieldDuration = 0.25f;
        weaponName = "Sword";
        weaponID = 1;
        attacksDamage[0] = 34f;
        attacksDamage[1] = 15f;
        attacksDamage[2] = 0f;
        
    }

    protected override void Update()
    {
        bCollider2D.enabled = (!isOnGlobalCoolDown && (isAttacking >= 0)) || !hasOwner;
        if (isAttacking == 0)
        {
            float translationNorm = weakTranslationSpeed * Time.deltaTime;
            transform.Translate(Vector3.up * translationNorm);
            weakTranslationLeft -= translationNorm;
        }
        else if (isAttacking == 1)
        {
            float rotateAngle = strongRotationSpeed * Time.deltaTime;
            player.transform.Rotate(Vector3.up, rotateAngle);
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
        yield return new WaitUntil(() => strongRotationLeft <= 0f);
        isAttacking = -1;
        player.transform.rotation = Quaternion.Euler(0f,0f,0f);
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
        player.SetIsGod(true);
        yield return new WaitForSeconds(parryDuration);
        player.SetIsGod(false);
        isSkillOnCD = true;
        yield return new WaitForSeconds(skillCD - parryDuration);
        isSkillOnCD = false;
    }

    protected override IEnumerator SkillP2()
    {
        if(GameManager.gameManager.player1 != null)
        {
            PlaySkillSound();
            GameManager.gameManager.player1.GetComponent<PlayerController>().SetIsGod(true);
            yield return new WaitForSeconds(shieldDuration);
            GameManager.gameManager.player1.GetComponent<PlayerController>().SetIsGod(false);
            isSkillOnCD = true;
            yield return new WaitForSeconds(skillCD);
            isSkillOnCD = false;
        }
    }
}
