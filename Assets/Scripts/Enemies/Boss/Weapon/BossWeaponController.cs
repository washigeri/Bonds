using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponController : MonoBehaviour
{
    private BossController boss;

    private float quickAttackCD;
    private bool isQuickAttackOnCD;
    private bool alreadyStartedQuickAttack;
    private bool startQuickAttack;
    private float quickAttackDamage;
    private float quickAttackFullTranslation;
    private float quickAttackTranslationLeft;
    private float quickAttackTranslationDuration;
    private float quickAttackTranslationSpeed;
    private float quickAttackTimeBetweenHits;
    private float quickAttackWarningDuration;

    private float slowAttackCD;
    private float isSlowAttackOnCD;
    private bool alreadyStartedSlowAttack;
    private bool startSlowAttack;
    private float slowAttackDamage;
    private float slowAttackFullRotation;
    private float slowAttackRotationLeft;
    private float slowAttackRotationDuration;
    private float slowAttackRotationSpeed;
    private float slowAttackTimeBetweenHits;
    private Vector3 slowDefaultRotation;
    private int slowFaceRight;
    private float slowAttackWarningDuration;
    private float slowAttackWarningFullRotation;
    private float slowAttackWarningRotationLeft;
    private float slowAttackWarningRotationSpeed;
    private bool slowAttackWarningStarted;

    private float[] attacksDamage;
    private int isAttacking;

    private Vector3 defaultLocalPosition;
    private Vector3 defaultLocalRotation;
    private BoxCollider2D bCollider2D;

    private float timeBetweenBothAttacks;
    private float timeBetweenBothAttacksLeft;

    private void Awake()
    {
        boss = null;
        isAttacking = -1;

        alreadyStartedQuickAttack = false;
        startQuickAttack = false;
        quickAttackDamage = 10f;
        quickAttackFullTranslation = 1f;
        quickAttackTranslationLeft = quickAttackFullTranslation;
        quickAttackTranslationDuration = 0.1f;
        quickAttackTranslationSpeed = quickAttackFullTranslation / quickAttackTranslationDuration;
        quickAttackTimeBetweenHits = 0.15f;
        quickAttackWarningDuration = 0.5f;

        alreadyStartedSlowAttack = false;
        startSlowAttack = false;
        slowAttackDamage = 15f;
        slowAttackFullRotation = 75f;
        slowAttackRotationLeft = slowAttackFullRotation;
        slowAttackRotationDuration = 0.1f;
        slowAttackRotationSpeed = slowAttackFullRotation / slowAttackRotationDuration;
        slowAttackTimeBetweenHits = 0.5f;
        slowDefaultRotation = new Vector3(0, 0, 0);
        slowFaceRight = 1;
        slowAttackWarningStarted = false;
        slowAttackWarningDuration = 1f;
        slowAttackWarningFullRotation = 45f;
        slowAttackWarningRotationLeft = slowAttackWarningFullRotation;
        slowAttackWarningRotationSpeed = slowAttackWarningFullRotation / slowAttackWarningDuration;

        attacksDamage = new float[] { quickAttackDamage, slowAttackDamage, 0f };

        defaultLocalPosition = new Vector3(-0.33f, 0f, 0f);
        defaultLocalRotation = new Vector3(0f, 0f, 90f);
        bCollider2D = GetComponent<BoxCollider2D>();

        timeBetweenBothAttacks = 2f;
        timeBetweenBothAttacksLeft = timeBetweenBothAttacks;
    }

    private void Start()
    {
        boss = GetComponentInParent<BossController>();
    }

    private void Update()
    {
        bCollider2D.enabled = (isAttacking >= 0);
        if (!alreadyStartedQuickAttack)
        {
            if (startQuickAttack)
            {
                StartCoroutine(QuickAttack());
            }
        }
        else
        {
            if (timeBetweenBothAttacksLeft > timeBetweenBothAttacks / 2f)
            {
                boss.MoveTo(GameManager.gameManager.player1.transform.position);
                timeBetweenBothAttacksLeft -= Time.deltaTime;
            }
            else if(timeBetweenBothAttacksLeft > 0f)
            {
                timeBetweenBothAttacksLeft -= Time.deltaTime;
            }
        }
        if (!alreadyStartedSlowAttack)
        {
            if (startSlowAttack)
            {
                StartCoroutine(SlowAttack());
            }
        }
        if (slowAttackWarningStarted)
        {
            float rotateAngle = slowAttackWarningRotationSpeed * Time.deltaTime;
            transform.parent.Rotate(Vector3.forward, rotateAngle * slowFaceRight);
            slowAttackWarningRotationLeft -= rotateAngle;
        }
        if (isAttacking == 0)
        {
            float translationNorm = quickAttackTranslationSpeed * Time.deltaTime;
            transform.Translate(Vector3.up * translationNorm);
            quickAttackTranslationLeft -= translationNorm;
        }
        if (isAttacking == 1)
        {
            float rotateAngle = slowAttackRotationSpeed * Time.deltaTime;
            transform.parent.Rotate(Vector3.forward, rotateAngle * -slowFaceRight);
            slowAttackRotationLeft -= rotateAngle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttacking >= 0)
        {
            if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
            {
                collision.GetComponent<PlayerController>().RemoveHealth(attacksDamage[isAttacking]);
            }
        }
    }

    private IEnumerator QuickAttack()
    {
        boss.SetStartQuickAttack(false);
        boss.SetIsBusy(true);
        alreadyStartedQuickAttack = true;
        boss.Target();
        yield return new WaitUntil(() => timeBetweenBothAttacksLeft <= 0f);
        isAttacking = 0;
        yield return new WaitUntil(() => quickAttackTranslationLeft <= 0f);
        quickAttackTranslationLeft = quickAttackFullTranslation;
        transform.localPosition = defaultLocalPosition;
        isAttacking = -1;
        yield return new WaitForSeconds(quickAttackTimeBetweenHits);
        isAttacking = 0;
        yield return new WaitUntil(() => quickAttackTranslationLeft <= 0f);
        quickAttackTranslationLeft = quickAttackFullTranslation;
        transform.localPosition = defaultLocalPosition;
        isAttacking = -1;
        yield return new WaitForSeconds(quickAttackTimeBetweenHits);
        isAttacking = 0;
        yield return new WaitUntil(() => quickAttackTranslationLeft <= 0f);
        quickAttackTranslationLeft = quickAttackFullTranslation;
        transform.localPosition = defaultLocalPosition;
        isAttacking = -1;
        alreadyStartedQuickAttack = false;
        startQuickAttack = false;
        boss.SetIsBusy(false);
        boss.SetIsDoingMeleeAttack(false);
        boss.SetStartedMeleeCD(true);
    }

    private IEnumerator SlowAttack()
    {
        timeBetweenBothAttacksLeft = timeBetweenBothAttacks;
        boss.SetStartSlowAttack(false);
        alreadyStartedSlowAttack = true;
        boss.Target();
        slowFaceRight = (boss.faceRight ? 1 : -1);
        slowAttackWarningStarted = true;
        yield return new WaitUntil(() => slowAttackWarningRotationLeft <= 0f);
        slowAttackWarningRotationLeft = slowAttackWarningFullRotation;
        slowAttackWarningStarted = false;
        isAttacking = 1;
        yield return new WaitUntil(() => slowAttackRotationLeft <= 0f);
        isAttacking = -1;
        slowAttackRotationLeft = slowAttackFullRotation;
        transform.parent.eulerAngles = slowFaceRight * defaultLocalRotation / 2f;
        boss.Target();
        yield return new WaitForSeconds(slowAttackTimeBetweenHits);
        slowFaceRight = (boss.faceRight ? 1 : -1);
        isAttacking = 1;
        yield return new WaitUntil(() => slowAttackRotationLeft <= 0f);
        isAttacking = -1;
        slowAttackRotationLeft = slowAttackFullRotation;
        transform.parent.eulerAngles = slowFaceRight * defaultLocalRotation / 2;
        boss.Target();
        yield return new WaitForSeconds(slowAttackTimeBetweenHits);
        slowFaceRight = (boss.faceRight ? 1 : -1);
        isAttacking = 1;
        yield return new WaitUntil(() => slowAttackRotationLeft <= 0f);
        isAttacking = -1;
        slowAttackRotationLeft = slowAttackFullRotation;
        transform.parent.eulerAngles = slowDefaultRotation;
        alreadyStartedSlowAttack = false;
        startSlowAttack = false;

        alreadyStartedQuickAttack = true;
        boss.Target();
        yield return new WaitUntil(() => timeBetweenBothAttacksLeft <= 0f);
        isAttacking = 0;
        yield return new WaitUntil(() => quickAttackTranslationLeft <= 0f);
        quickAttackTranslationLeft = quickAttackFullTranslation;
        transform.localPosition = defaultLocalPosition;
        isAttacking = -1;
        yield return new WaitForSeconds(quickAttackTimeBetweenHits);
        isAttacking = 0;
        yield return new WaitUntil(() => quickAttackTranslationLeft <= 0f);
        quickAttackTranslationLeft = quickAttackFullTranslation;
        transform.localPosition = defaultLocalPosition;
        isAttacking = -1;
        yield return new WaitForSeconds(quickAttackTimeBetweenHits);
        isAttacking = 0;
        yield return new WaitUntil(() => quickAttackTranslationLeft <= 0f);
        quickAttackTranslationLeft = quickAttackFullTranslation;
        transform.localPosition = defaultLocalPosition;
        isAttacking = -1;
        alreadyStartedQuickAttack = false;
        startQuickAttack = false;
        boss.SetStartedMeleeCD(true);
        boss.SetIsBusy(false);
        boss.SetIsDoingMeleeAttack(false);
    }

    public void SetStartQuickAttack(bool startQuickAttack)
    {
        this.startQuickAttack = startQuickAttack;
    }

    public void SetStartSlowAttack(bool startSlowAttack)
    {
        this.startSlowAttack = startSlowAttack;
    }


}
