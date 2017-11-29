using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponController : MonoBehaviour
{

    private bool alreadyStartedQuickAttack;
    private bool startQuickAttack;
    private float quickAttackDamage;
    private float quickAttackFullTranslation;
    private float quickAttackTranslationLeft;
    private float quickAttackTranslationDuration;
    private float quickAttackTranslationSpeed;
    private float quickAttackTimeBetweenHits;

    private bool alreadyStartedSlowAttack;
    private bool startSlowAttack;
    private float slowAttackDamage;
    private float slowAttackFullRotation;
    private float slowAttackRotationLeft;
    private float slowAttackRotationDuration;
    private float slowAttackRotationSpeed;
    private int slowAttackDirection;
    private float slowAttackTimeBetweenHits;
    private Vector3 slowDefaultRotation;

    private float[] attacksDamage;
    private int isAttacking;

    private Vector3 defaultLocalPosition;
    private Vector3 defaultRotation;
    private BoxCollider2D bCollider2D;

    private void Awake()
    {
        isAttacking = -1;

        alreadyStartedQuickAttack = false;
        startQuickAttack = false;
        quickAttackDamage = 10f;
        quickAttackFullTranslation = 1f;
        quickAttackTranslationLeft = quickAttackFullTranslation;
        quickAttackTranslationDuration = 0.1f;
        quickAttackTranslationSpeed = quickAttackFullTranslation / quickAttackTranslationDuration;
        quickAttackTimeBetweenHits = 0.25f;

        alreadyStartedSlowAttack = false;
        startSlowAttack = false;
        slowAttackDamage = 25f;
        slowAttackFullRotation = 50f;
        slowAttackRotationLeft = slowAttackFullRotation;
        slowAttackRotationDuration = 0.5f;
        slowAttackRotationSpeed = slowAttackFullRotation / slowAttackRotationSpeed;
        slowAttackDirection = 1;
        slowAttackTimeBetweenHits = 0.5f;
        slowDefaultRotation = new Vector3(0, 0, 45);

        attacksDamage = new float[] { quickAttackDamage, slowAttackDamage, 0f };

        defaultLocalPosition = new Vector3(-0.3f, 0f, 0f);
        defaultRotation = new Vector3(0, 0, 90);
        bCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        bCollider2D.enabled = (isAttacking >= 0);
        Debug.Log("alreadystartedquickattack = " + alreadyStartedQuickAttack);
        if (!alreadyStartedQuickAttack)
        {
            if (startQuickAttack)
            {
                StartCoroutine(QuickAttack());
            }
        }
        if (startSlowAttack)
        {
            if (!alreadyStartedSlowAttack)
            {
                StartCoroutine(SlowAttack());
            }
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
            transform.Rotate(Vector3.forward, rotateAngle * slowAttackDirection);
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
        alreadyStartedQuickAttack = true;
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
    }


    private IEnumerator SlowAttack()
    {
        alreadyStartedSlowAttack = true;
        startSlowAttack = false;
        transform.localEulerAngles = slowDefaultRotation;
        isAttacking = 1;
        slowAttackDirection = 1;
        yield return new WaitUntil(() => slowAttackRotationLeft <= 0f);
        isAttacking = -1;
        slowAttackRotationLeft = slowAttackFullRotation;
        transform.localEulerAngles = slowDefaultRotation;
        yield return new WaitForSeconds(slowAttackTimeBetweenHits);
        isAttacking = 1;
        slowAttackDirection = 1;
        yield return new WaitUntil(() => slowAttackRotationLeft <= 0f);
        isAttacking = -1;
        slowAttackRotationLeft = slowAttackFullRotation;
        transform.localEulerAngles = slowDefaultRotation;
        yield return new WaitForSeconds(slowAttackTimeBetweenHits);
        isAttacking = 1;
        slowAttackDirection = 1;
        yield return new WaitUntil(() => slowAttackRotationLeft <= 0f);
        isAttacking = -1;
        slowAttackRotationLeft = slowAttackFullRotation;
        transform.localEulerAngles = slowDefaultRotation;
        alreadyStartedSlowAttack = false;
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
