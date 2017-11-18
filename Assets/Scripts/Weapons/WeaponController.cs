using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class WeaponController : MonoBehaviour
{
    [HideInInspector] public string weaponName;

    protected int damage;
    protected int range;
    protected int speed;
    protected float globalCD;

    protected PlayerController player;
    protected bool hasOwner;
    protected int owner;
    protected int isAttacking;
    protected bool isOnGlobalCoolDown;


    [HideInInspector] public string weakName;
    [HideInInspector] public string strongName;
    [HideInInspector] public string skillName;

    protected float weakCD;
    protected float strongCD;
    protected float skillCD;
    
    protected bool isStrongOnCD;
    protected bool isSkillOnCD;

    protected int[] attacksDamage;

    protected string enemyTag;

    // Use this for initialization
    protected virtual void Awake()
    {
        globalCD = 0.25f;
        hasOwner = transform.root.CompareTag("Player1") || transform.root.CompareTag("Player2");
        if (hasOwner)
        {
            owner = transform.root.CompareTag("Player1") ? 1 : 2;
            player = transform.root.GetComponent<PlayerController>();
        }
        else
        {
            owner = 0;
            player = null;
        }
        isAttacking = -1;
        isOnGlobalCoolDown = false;
        attacksDamage = new int[] { 10 * damage / 3, 10 * damage / 2, 10 * damage};
        SetPlayerInfo();
    }

    public void SetPlayerInfo()
    {
        if (owner == 1)
        {
            enemyTag = "Enemy";
            weakName = "WeakP1";
            strongName = "StrongP1";
            skillName = "SkillP1";
        }
        else if (owner == 2)
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
                if (isAttacking >= 0)
                {
                    if (collision.gameObject.CompareTag(enemyTag))
                    {
                        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                        enemy.RemoveHealth(player.GetDamageMultiplier() * attacksDamage[isAttacking]);
                        StartCoroutine(enemy.StunnedRoutine());
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
                transform.localPosition = Vector3.zero;
                transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), Mathf.Abs(this.transform.localScale.y), Mathf.Abs(this.transform.localScale.z));
                hasOwner = true;
                owner = transform.root.CompareTag("Player1") ? 1 : 2;
                player = playerController;
                SetPlayerInfo();
            }
        }
    }

    protected virtual void Update()
    {
        //Debug.Log("damage multiplier read is : " + player.GetDamageMultiplier());
        if (hasOwner && !isOnGlobalCoolDown && (isAttacking == -1))
        {
            if (Input.GetButtonDown(weakName))
            {
                StartCoroutine(WeakAttack());
            }
            else if (Input.GetButtonDown(strongName))
            {
                if (!isStrongOnCD)
                {
                    StartCoroutine(StrongAttack());
                }
            }
            else if (Input.GetButtonDown(skillName))
            {
                if (!isSkillOnCD)
                {
                    if(owner == 1)
                    {
                        StartCoroutine(SkillP1());
                    }
                    else if(owner == 2)
                    {
                        StartCoroutine(SkillP2());
                    }
                }
            }
        }
    }

    public void SetOwner(bool hasOwner)
    {
        this.hasOwner = hasOwner;
    }

    protected abstract IEnumerator WeakAttack();

    protected abstract IEnumerator StrongAttack();

    protected abstract IEnumerator SkillP1();

    protected abstract IEnumerator SkillP2();

}
