using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour
{
    [HideInInspector] public bool faceRight = false;

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
    private float stunDuration;

    private bool isSlowDown;
    private bool slowDown;

    private bool startedBleeding;
    private bool isBleeding;
    private float bleedingDamage;
    private float bleedingDuration;

    private bool canDrop;
    private float potionDropRate;
    private float weaponDropRate;
    private float trinketDropRate;

    protected Rigidbody2D rb2d;

    protected virtual void Awake()
    {
        
        damageMultiplier = 1f;
        speedMultiplier = 1f;
        speedMultiplierDuration = 0f;
        canAttack = false;
        isStunned = false;
        stunned = false;
        stunDuration = 0.15f;
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
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Debug.Log("Ennemy health " + health);
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
                StartCoroutine(StunnedRoutine());
            }
            if (isBleeding)
            {
                if (!startedBleeding)
                {
                    StartCoroutine(StartBleeding());
                }
            }
            if(bleedingDuration > 0f)
            {
                RemoveHealth(Time.deltaTime * bleedingDamage / bleedingDuration, true);
                bleedingDamage -= Time.deltaTime * bleedingDamage / bleedingDuration;
                bleedingDuration -= Time.deltaTime;
            }
            if (speedMultiplierDuration > 0f)
            {
                speedMultiplierDuration -= Time.deltaTime;
            }
            if (slowDown)
            {
                StartCoroutine(Slow());
            }
            Action();
        }
    }

    protected abstract void Action();

    protected void MoveTowards(Vector2 target)
    {
        Vector2 nextPosition = Vector2.MoveTowards(enemyTransform.position, target, speedMultiplier * speed * Time.deltaTime);
        if (nextPosition.x - enemyTransform.position.x > 0)
        {
            if (!faceRight)
            {
                Flip();
            }
        }
        else if (nextPosition.x - enemyTransform.position.x < 0)
        {
            if (faceRight)
            {
                Flip();
            }
        }
        enemyTransform.position = nextPosition;
    }

    public void RemoveHealth(float loss, bool isBleedingDamage)
    {
        if (isBleedingDamage)
        {
            health -= loss;
        }
        else
        {
            if (!isStunned)
            {
                health -= loss;
            }
        }
    }

    private IEnumerator StunnedRoutine()
    {
        stunned = false;
        Debug.Log("starting stunned routine");
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }

    public bool GetStunned()
    {
        return stunned;
    }

    public void SetStunned(bool stunned)
    {
        this.stunned = stunned;
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
        Debug.Log("starts bleeding");
        startedBleeding = true;
        yield return new WaitUntil(() => bleedingDuration <= 0f);
        bleedingDamage = 0f;
        bleedingDuration = 0f;
        isBleeding = false;
        startedBleeding = false;
    }

    private IEnumerator Slow()
    {
        Debug.Log("starting slow routine");
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
        if(dice <= weaponDropRate)
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
            Instantiate(Resources.Load("Prefabs/Weapons/" + weaponName), enemyTransform.position + new Vector3(Random.Range(0f,1f), Random.Range(0f,1f), 0f), Quaternion.Euler(0,0,-90));
        }
        else if(dice <= weaponDropRate + trinketDropRate)
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
