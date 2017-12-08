using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour
{
    [HideInInspector] public bool faceRight = false;

    public AudioClip damageSound;

    public Transform enemyTransform;
    public LayerMask layerMask;

    protected GameObject player;
    public bool isSpirit;

    protected float attackCD;
    protected bool isOnCD;
    protected float health;
    protected float attackRange;

    protected float speed;
    protected float speedMultiplier;

    private float speedMultiplierDuration;
    protected int damage;
    protected float damageMultiplier;

    protected float detectionRange;
    protected bool canAttack;

    protected bool isStunned;
    private bool stunned;
    private float defaultStunDuration;
    private float stunTimeLeft;

    private bool isSlowDown;
    private bool slowDown;

    private bool startedBleeding;
    private bool isBleeding;
    private float bleedingDamage;
    private float bleedingDuration;

    protected bool canDrop;
    private float potionDropRate;
    private float weaponDropRate;
    private float trinketDropRate;

    protected Rigidbody2D rb2d;

    protected bool canBeKnockedBack;
    protected float knockBackForce;

    protected virtual void Awake()
    {
        damageMultiplier = 1f;
        speedMultiplier = 1f;
        speedMultiplierDuration = 0f;
        canAttack = false;
        isStunned = false;
        stunned = false;
        defaultStunDuration = 0.2f;
        stunTimeLeft = 0f;
        isSlowDown = false;
        slowDown = false;
        isBleeding = false;
        bleedingDamage = 0f;
        bleedingDuration = 0f;
        canDrop = true;
        potionDropRate = 0.3f;
        weaponDropRate = 0.15f;
        trinketDropRate = 0.15f;
        rb2d = GetComponent<Rigidbody2D>();
        canBeKnockedBack = true;
        knockBackForce = 5;
    }

    protected virtual void Update()
    {
        if (this.health <= 0)
        {
            if (canDrop)
            {
                Drop();
            }
            Destroy(gameObject);
        }
        else
        {
            if (stunned)
            {
                if (!isStunned)
                {
                    StartCoroutine(StunnedRoutine());
                }
            }
            if (isBleeding)
            {
                if (!startedBleeding)
                {
                    StartCoroutine(StartBleeding());
                }
            }
            if (bleedingDuration > 0f)
            {
                RemoveHealth(Time.deltaTime * bleedingDamage / bleedingDuration, true);
                bleedingDamage -= Time.deltaTime * bleedingDamage / bleedingDuration;
                bleedingDuration -= Time.deltaTime;
            }
            if (speedMultiplierDuration > 0f)
            {
                speedMultiplierDuration -= Time.deltaTime;
            }
            if (isStunned)
            {
                if (stunTimeLeft > 0f)
                {
                    stunTimeLeft -= Time.deltaTime;
                }
            }
            if (slowDown)
            {
                StartCoroutine(Slow());
            }
            Action();
        }
    }

    protected abstract void Action();


    //protected void MoveTowards(Vector2 target)
    //{
    //    Vector2 nextPosition = Vector2.MoveTowards(enemyTransform.position, target, speedMultiplier * speed * Time.deltaTime);
    //    if (nextPosition.x - enemyTransform.position.x > 0)
    //    {
    //        if (!faceRight)
    //        {
    //            Flip();
    //        }
    //    }
    //    else if (nextPosition.x - enemyTransform.position.x < 0)
    //    {
    //        if (faceRight)
    //        {
    //            Flip();
    //        }
    //    }
    //    enemyTransform.position = nextPosition;
    //}

    protected virtual void MoveToward(Vector2 target)
    {

    }

    protected void Stop()
    {
        rb2d.velocity = Vector2.zero;
    }

    public void RemoveHealth(float loss, bool isBleedingDamage)
    {
        if (!isBleedingDamage)
        {
            if (canBeKnockedBack)
            {
                rb2d.AddForce(new Vector2(faceRight ? -1 : 1, 0.25f) * knockBackForce, ForceMode2D.Impulse);
            }
        }
        health -= loss;
    }

    private IEnumerator StunnedRoutine()
    {
        stunned = false;
        isStunned = true;
        yield return new WaitUntil(() => stunTimeLeft <= 0f);
        isStunned = false;
    }

    public bool GetStunned()
    {
        return stunned;
    }

    public void SetStunned(bool stunned, float stunDuration)
    {
        this.stunned = stunned;
        if (stunDuration > 0f)
        {
            this.stunTimeLeft = Mathf.Max(stunTimeLeft, stunDuration);
        }
        else
        {
            this.stunTimeLeft = Mathf.Max(stunTimeLeft, defaultStunDuration);
        }
    }

    protected void Flip()
    {
        faceRight = !faceRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    protected abstract IEnumerator Attack();

    public void SetDamageMultiplier(float damageMultiplier)
    {
        this.damageMultiplier = damageMultiplier;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetSpeedMultiplierParameters(float speedMultiplier, float duration)
    {
        if (isSlowDown)
        {
            if (duration <= this.speedMultiplierDuration && speedMultiplier >= this.speedMultiplier)
            {
                this.speedMultiplier = (this.speedMultiplier + speedMultiplier) / 2f;
                this.speedMultiplierDuration = (this.speedMultiplierDuration + duration) / 2f;
            }
            else if (duration >= this.speedMultiplierDuration && speedMultiplier <= this.speedMultiplier)
            {
                this.speedMultiplierDuration = (this.speedMultiplierDuration + duration) / 2f;
            }
            else if (duration >= this.speedMultiplierDuration && speedMultiplier >= this.speedMultiplier)
            {
                this.speedMultiplier = speedMultiplier;
                this.speedMultiplierDuration = duration;
            }
        }
        else
        {
            slowDown = true;
            this.speedMultiplier *= speedMultiplier;
            speedMultiplierDuration = duration;
        }
    }

    public void SetBleedingParameters(float bleedingDamage, float duration)
    {
        isBleeding = true;
        this.bleedingDamage += bleedingDamage;
        this.bleedingDuration = Mathf.Max(duration, this.bleedingDuration);
    }

    private IEnumerator StartBleeding()
    {
        startedBleeding = true;
        yield return new WaitUntil(() => bleedingDuration <= 0f);
        bleedingDamage = 0f;
        bleedingDuration = 0f;
        isBleeding = false;
        startedBleeding = false;
    }

    private IEnumerator Slow()
    {
        slowDown = false;
        isSlowDown = true;
        yield return new WaitUntil(() => speedMultiplierDuration <= 0f);
        this.speedMultiplier = 1f;
        isSlowDown = false;
    }

    private void Drop()
    {
        canDrop = false;
        float dice = Random.Range(0f, 1f);
        if (dice <= weaponDropRate)
        {
            int weaponType = Random.Range(0, 4);
            string weaponName;
            if (weaponType == 0)
            {
                weaponName = "Spear";
            }
            else if (weaponType == 1)
            {
                weaponName = "Sword";
            }
            else if (weaponType == 2)
            {
                weaponName = "Daggers";
            }
            else
            {
                weaponName = "Bow";
            }
            Instantiate(Resources.Load("Prefabs/Weapons/" + weaponName), enemyTransform.position + new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f), Quaternion.Euler(0, 0, -90));
        }
        else if (dice <= weaponDropRate + trinketDropRate)
        {
            int trinketNumber = Random.Range(1, 5);
            string trinketName = "Trinket" + trinketNumber;
            Instantiate(Resources.Load("Prefabs/Weapons/Trinkets/" + trinketName), enemyTransform.position + new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f), Quaternion.Euler(0, 0, 0));
        }
        dice = Random.Range(0f, 1f);
        if (dice <= potionDropRate)
        {
            Instantiate(Resources.Load("Prefabs/UsableObjects/Potion"), enemyTransform.position + new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f), Quaternion.Euler(0, 0, 0));
        }
    }
}
