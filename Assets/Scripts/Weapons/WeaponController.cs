using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class WeaponController : MonoBehaviour
{
    public AudioClip[] weakSounds;
    public AudioClip[] strongSounds;
    public AudioClip livingSkillSound;
    public AudioClip deadSkillSound;

    [HideInInspector] public string weaponName;

    protected Vector3 defaultLocalRotation;
    protected Vector3 defaultLocalPosition;
    protected float damage;
    protected float range;
    protected float speed;
    protected float globalCD;
    protected float localGlobalCD;

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

    protected float[] attacksDamage;

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
        attacksDamage = new float[] { 10 * damage / 3, 10 * damage / 2, 10 * damage, 0f};
        defaultLocalPosition = new Vector3(1.3f, 0f, 0f);
        transform.localPosition = defaultLocalPosition;
        transform.localEulerAngles = defaultLocalRotation;
        SetWeaponInfo();
    }

    public void SetWeaponInfo()
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
                        enemy.RemoveHealth(player.GetDamageDoneMultiplier() * attacksDamage[isAttacking], false);
                        if(player.GetEnemySpeedMultiplierDuration() > 0f)
                        {
                            enemy.SetSpeedMultiplierParameters(player.GetEnemySpeedMultiplier(), player.GetEnemySpeedMultiplierDuration());
                        }
                        if(player.GetEnemyBleedDuration() > 0f)
                        {
                            enemy.SetBleedingParameters(player.GetEnemyBleedPercentage() * attacksDamage[isAttacking] * player.GetDamageDoneMultiplier(), player.GetEnemyBleedDuration());
                        }
                        if (player.GetStunEnemy())
                        {
                            enemy.SetStunned(true, player.GetStunEnemyDuration());
                        }
                        else
                        {
                            enemy.SetStunned(true, -1f);
                        }
                        player.SetIsFighting(true);
                    }
                    else if(collision.gameObject.CompareTag("Boss"))
                    {
                        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                        enemy.RemoveHealth(player.GetDamageDoneMultiplier() * attacksDamage[isAttacking], false);
                        if (player.GetEnemySpeedMultiplierDuration() > 0f)
                        {
                            enemy.SetSpeedMultiplierParameters(player.GetEnemySpeedMultiplier(), player.GetEnemySpeedMultiplierDuration());
                        }
                        if (player.GetEnemyBleedDuration() > 0f)
                        {
                            enemy.SetBleedingParameters(player.GetEnemyBleedPercentage() * attacksDamage[isAttacking] * player.GetDamageDoneMultiplier(), player.GetEnemyBleedDuration());
                        }
                        player.SetIsFighting(true);
                    }
                }
            }
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (!hasOwner)
        {
            if (Input.GetButtonDown("InteractP1"))
            {
                if (collision.gameObject.CompareTag("Player1"))
                {
                    SwapWeapons(collision, 1);
                }
            }
            else if (Input.GetButtonDown("InteractP2"))
            {
                if (collision.gameObject.CompareTag("Player2"))
                {
                    SwapWeapons(collision, 2);
                }
            }
        }
    }

    private void SwapWeapons(Collider2D collision, int owner)
    {
        this.owner = owner;
        player = collision.gameObject.GetComponent<PlayerController>();
        player.DropWeapon();
        gameObject.transform.parent = collision.gameObject.transform.Find("Hand");
        player.SetMyWeapon(this);
        hasOwner = true;
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), Mathf.Abs(this.transform.localScale.y), Mathf.Abs(this.transform.localScale.z));
        transform.localEulerAngles = defaultLocalRotation;
        transform.localPosition = defaultLocalPosition;
        SetWeaponInfo();
        GameManager.gameManager.RemoveObjectToBeCleaned(gameObject.GetInstanceID());
    }

    protected virtual void Update()
    {
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

    public void SetHasOwner(bool hasOwner)
    {
        this.hasOwner = hasOwner;
    }

    public void SetOwner(int owner)
    {
        this.owner = owner;
    }

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
    }

    public void SetIsAttacking(int isAttacking)
    {
        this.isAttacking = isAttacking;
    }

    public Vector3 GetDefaultLocalPosition()
    {
        return defaultLocalPosition;
    }

    public Vector3 GetDefaultLocalRotation()
    {
        return defaultLocalRotation;
    }

    protected abstract IEnumerator WeakAttack();

    protected abstract IEnumerator StrongAttack();

    protected abstract IEnumerator SkillP1();

    protected abstract IEnumerator SkillP2();

    protected void PlayWeakSound()
    {
        SoundManager.instance.PlayRandomSFX(weakSounds);
    }

    protected void PlayStrongSound()
    {
        SoundManager.instance.PlayRandomSFX(strongSounds);
    }

    protected void PlaySkillSound()
    {
        if (owner == 1)
            SoundManager.instance.PlaySFX(livingSkillSound);
        else if (owner == 2)
            SoundManager.instance.PlaySFX(deadSkillSound);
    }

}
