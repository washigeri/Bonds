using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class WeaponController : MonoBehaviour
{

    public int damage;
    public int range;
    public int speed;
    protected float globalCD = 0.25f;
    protected bool isAttacking = false;
    protected bool isOnGlobalCoolDown = false;
    protected PolygonCollider2D pCollider2D;
    protected float weakCD;
    protected float strongCD;
    protected float skillCD;
    protected string enemyTag;
    [HideInInspector] public string weakName;
    [HideInInspector] public string strongName;
    [HideInInspector] public string skillName;

    // Use this for initialization
    protected virtual void Awake()
    {
        pCollider2D = GetComponent<PolygonCollider2D>();
        if (transform.root.CompareTag("Player1"))
        {
            enemyTag = "Enemy";
            weakName = "WeakP1";
            strongName = "StrongP1";
            skillName = "SkillP1";
        }
        else
        {
            enemyTag = "Spirit";
            weakName = "WeakP2";
            strongName = "StrongP2";
            skillName = "SkillP2";
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOnGlobalCoolDown)
        {
            if (isAttacking)
            {
                if (collision.gameObject.CompareTag(enemyTag))
                {
                    collision.gameObject.GetComponent<EnemyController>().RemoveHealth(damage);
                }
            }
        }
    }

    protected virtual void Update()
    {
        pCollider2D.enabled = !isOnGlobalCoolDown && isAttacking;

        if (!isOnGlobalCoolDown && !isAttacking)
        {
            if (Input.GetButtonDown(weakName))
            {
                StartCoroutine(WeakAttack());
            }
            else if (Input.GetButtonDown(strongName))
            {
                StartCoroutine(StrongAttack());
            }
            else if (Input.GetButtonDown(skillName))
            {
                StartCoroutine(Skill());
            }
        }
    }

    protected abstract IEnumerator WeakAttack();

    protected abstract IEnumerator StrongAttack();

    protected abstract IEnumerator Skill();

}
