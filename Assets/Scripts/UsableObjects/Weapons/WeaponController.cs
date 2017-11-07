using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class WeaponController : MonoBehaviour
{
    public string weaponName;

    public int damage;
    public int range;
    public int speed;
    protected float globalCD;

    protected bool hasOwner;
    protected bool isAttacking;
    protected bool isOnGlobalCoolDown;

    protected PolygonCollider2D pCollider2D;

    [HideInInspector] public string weakName;
    [HideInInspector] public string strongName;
    [HideInInspector] public string skillName;

    protected float weakCD;
    protected float strongCD;
    protected float skillCD;

    protected string enemyTag;

    // Use this for initialization
    protected virtual void Awake()
    {
        pCollider2D = GetComponent<PolygonCollider2D>();
        globalCD = 0.25f;
        hasOwner = transform.root.name.Equals("Player1") || transform.root.name.Equals("Player2");
        isAttacking = false;
        isOnGlobalCoolDown = false;
        SetPlayerInfo();
    }

    public void SetPlayerInfo()
    {
        if (transform.root.CompareTag("Player1"))
        {
            enemyTag = "Enemy";
            weakName = "WeakP1";
            strongName = "StrongP1";
            skillName = "SkillP1";
        }
        else if(transform.root.CompareTag("Player2"))
        {
            enemyTag = "Spirit";
            weakName = "WeakP2";
            strongName = "StrongP2";
            skillName = "SkillP2";
        }
        else
        {
            enemyTag = null;
            weakName = null;
            strongName = null;
            skillName = null;
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasOwner)
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
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButtonDown("InteractP1") || Input.GetButtonDown("InteractP2"))
        {
            if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                playerController.DropWeapon();
                gameObject.transform.parent = collision.gameObject.transform.Find("Hand");
                this.transform.localPosition = Vector3.zero;
                this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), Mathf.Abs(this.transform.localScale.y), Mathf.Abs(this.transform.localScale.z));
                hasOwner = true;
                SetPlayerInfo();
            }
        }
    }

    protected virtual void Update()
    {
        pCollider2D.enabled = (!isOnGlobalCoolDown && isAttacking) || !hasOwner;
        if (hasOwner && !isOnGlobalCoolDown && !isAttacking)
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

    public void SetOwner(bool hasOwner)
    {
        this.hasOwner = hasOwner;
    }

    protected abstract IEnumerator WeakAttack();

    protected abstract IEnumerator StrongAttack();

    protected abstract IEnumerator Skill();

}
