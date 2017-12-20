using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daggers : WeaponController
{

    private BoxCollider2D bCollider2D;

    private float weakTranslationLeft;
    private float weakTranslationDuration;
    private float weakTranslationSpeed;

    private float strongDuration;
    private float strongRange;

    private float rouladeAcceleration;
    private float rouladeDuration;
    private float rouladeFullRotation;
    private float rouladeRotationLeft;
    private float rouladeRotationSpeed;
    private int rouladeFaceRight;
    private bool startedRoulade;

    private float skillSpeedMultiplier;
    private float skillTimeToActivate;
    private float skillSpeedMultiplierDuration;
    private float skillSpeedMultiplierTimeLeft;
    private float skillAttackSpeedMultiplier;
    private float skillAttackSpeedMultiplierDuration;
    private bool castedBuff;

    protected override void Awake()
    {
        base.Awake();
        bCollider2D = GetComponent<BoxCollider2D>();
        defaultLocalRotation = new Vector3(0f, 0f, -90f);
        damage = 4;
        speed = 1f;
        localGlobalCD = speed * globalCD;

        range = 0.5f;
        weakTranslationLeft = range;
        weakTranslationDuration = globalCD / 3f;
        weakTranslationSpeed = range / weakTranslationDuration;

        strongCD = 5f;
        skillCD = 2f;

        strongDuration = 2f;
        strongRange = 1.25f;

        rouladeAcceleration = 5f;
        rouladeDuration = 0.2f;
        rouladeFullRotation = 360f;
        rouladeFaceRight = -1;
        rouladeRotationLeft = rouladeFullRotation;
        rouladeRotationSpeed = rouladeFullRotation / rouladeDuration;
        startedRoulade = false;

        skillSpeedMultiplier = 1.5f;
        skillTimeToActivate = 5f;
        skillSpeedMultiplierDuration = 5f;
        skillSpeedMultiplierTimeLeft = skillSpeedMultiplierDuration;
        skillAttackSpeedMultiplier = 0.75f;
        skillAttackSpeedMultiplierDuration = 5f;
        castedBuff = false;

        weaponName = "Daggers";
        weaponID = 2;

        attacksDamage[0] = 15f;
        attacksDamage[1] = 5f;
        attacksDamage[2] = 0f;
    }

    protected override void Update()
    {
        bCollider2D.enabled = (!isOnGlobalCoolDown && (isAttacking == 0 || isAttacking == 1)) || !hasOwner;
        if(isAttacking == 0)
        {
            float translationNorm = weakTranslationSpeed * Time.deltaTime;
            transform.Translate(Vector3.up * translationNorm);
            weakTranslationLeft -= translationNorm;
        }
        if (owner == 1 && startedRoulade)
        {
            float rotateAngle = rouladeRotationSpeed * Time.deltaTime;
            player.transform.Rotate(Vector3.forward, rouladeFaceRight * rotateAngle);
            rouladeRotationLeft -= rotateAngle;
        }
        if(owner == 2 && castedBuff)
        {
            skillSpeedMultiplierTimeLeft -= Time.deltaTime;
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

    private void InitDagger(Vector3 direction, float damage)
    {
        GameObject dagger = Instantiate(Resources.Load("Prefabs/Weapons/Projectiles/Dagger"), player.transform.position + direction.normalized * strongRange, Quaternion.Euler(0, 0, 0)) as GameObject;
        dagger.GetComponent<DaggerShield>().SetParameters(owner, strongDuration, Vector3.zero, direction, damage, enemyTag);
        dagger.transform.parent = player.transform;
    }

    protected override IEnumerator StrongAttack()
    {
        PlayStrongSound();
        InitDagger(Vector3.right, 5f);
        InitDagger(Vector3.left, 5f);
        InitDagger(Vector3.up, 5f);
        InitDagger(Vector3.up + Vector3.right, 5f);
        InitDagger(Vector3.up + Vector3.left, 5f);
        InitDagger(Vector3.down, 5f);
        InitDagger(Vector3.down + Vector3.right, 5f);
        InitDagger(Vector3.down + Vector3.left, 5f);
        isStrongOnCD = true;
        yield return new WaitForSeconds(strongCD);
        isStrongOnCD = false;
    }


    protected override IEnumerator SkillP1()
    {
        PlaySkillSound();
        startedRoulade = true;
        player.SetSpeedMultiplier(player.GetSpeedMutiplier() * rouladeAcceleration);
        player.rb2d.AddForce(365f * (player.faceRight ? Vector2.right : Vector2.left), ForceMode2D.Impulse);
        isSkillOnCD = true;
        isOnGlobalCoolDown = true;
        rouladeFaceRight = player.faceRight ? -1 : 1;
        player.SetIsGod(true);
        yield return new WaitUntil(() => rouladeRotationLeft <= 0f);
        startedRoulade = false;
        player.SetIsGod(false);
        player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        rouladeRotationLeft = rouladeFullRotation;
        player.SetSpeedMultiplier(player.GetSpeedMutiplier() / rouladeAcceleration);
        float currentGCD = localGlobalCD * player.GetAttackSpeedMultipler();
        yield return new WaitForSeconds(currentGCD - rouladeDuration);
        isOnGlobalCoolDown = false;
        yield return new WaitForSeconds(skillCD - currentGCD + rouladeDuration);
        isSkillOnCD = false;
    }

    protected override IEnumerator SkillP2()
    {
        PlaySkillSound();
        castedBuff = true;
        PlayerController player1 = GameManager.gameManager.player1.GetComponent<PlayerController>();
        player1.SetSpeedMultiplier(player1.GetSpeedMutiplier() * skillSpeedMultiplier);
        isSkillOnCD = true;
        yield return new WaitUntil(() => (skillSpeedMultiplierTimeLeft <= 0f || player1.GetIsFighting()));
        if (skillSpeedMultiplierTimeLeft > 0f)
        {
            StartCoroutine(SkillP2InnerRoutine(player1));
            yield return new WaitUntil(() => skillSpeedMultiplierTimeLeft <= 0f);
        }
        castedBuff = false;
        player1.SetSpeedMultiplier(player1.GetSpeedMutiplier() / skillSpeedMultiplier);
        skillSpeedMultiplierTimeLeft = skillSpeedMultiplierDuration;
        yield return new WaitForSeconds(skillCD - skillSpeedMultiplierDuration);
        isSkillOnCD = false;
    }

    private IEnumerator SkillP2InnerRoutine(PlayerController player1)
    {
        player1.SetAttackSpeedMultiplier(player1.GetAttackSpeedMultipler() * skillAttackSpeedMultiplier);
        yield return new WaitForSeconds(skillAttackSpeedMultiplierDuration);
        player1.SetAttackSpeedMultiplier(player1.GetAttackSpeedMultipler() / skillAttackSpeedMultiplier);
    }
}
