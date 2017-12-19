using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool faceRight = true;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool moveHability = false;

    protected bool isPlayer1;
    protected bool isBlocked;
    private bool isGod;

    protected float moveForce;
    protected float maxSpeed = 5f;
    public Transform playerTransform;

    [HideInInspector] public float maxHp;
    protected float hp;
    protected int agility;
    protected int strengh;
    protected int stamina;

    private bool isFighting;
    private bool isAlreadyTryingToLeaveFight;
    private float timeToQuitFight;
    private float timeToQuitFightLeft;

    protected float damageDoneMultiplier;
    protected float damageReceivedMultiplier;
    protected float speedMultiplier;
    protected float attackSpeedMultiplier;
    protected float enemySpeedMultiplier;
    protected float enemySpeedMultiplierDuration;
    protected float enemyBleedPercentage;
    protected float enemyBleedDuration;
    protected bool stunEnemy;
    protected float stunEnemyDuration;

    protected float dirH;
    protected float dirV;

    protected string potionBindName;
    protected string interactBindName;

    public Rigidbody2D rb2d;
    private TrinketController myTrinket;
    private WeaponController myWeapon;

    // Use this for initialization
    protected virtual void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        maxHp = 100f;
        hp = maxHp;
        agility = 1;
        strengh = 1;
        stamina = 1;
        moveForce = 365f;
        damageDoneMultiplier = 1f;
        damageReceivedMultiplier = 1f;
        speedMultiplier = 1f;
        attackSpeedMultiplier = 1;
        enemySpeedMultiplier = 1f;
        enemySpeedMultiplierDuration = 0f;
        enemyBleedPercentage = 0f;
        enemyBleedDuration = 0f;
        isFighting = false;
        isAlreadyTryingToLeaveFight = false;
        timeToQuitFight = 5f;
        timeToQuitFightLeft = timeToQuitFight;
        isGod = false;
        isBlocked = false;
        myTrinket = null;
        myWeapon = GetWeaponScript();
    }

    private WeaponController GetWeaponScript()
    {
        return playerTransform.gameObject.GetComponentInChildren<WeaponController>();
    }

    protected virtual void Update()
    {
        isDead = (hp <= 0f);
        CheckForInputs();
        if (isFighting)
        {
            if (!isAlreadyTryingToLeaveFight)
            {
                StartCoroutine(OnQuittingFight());
            }
            else
            {
                timeToQuitFightLeft -= Time.deltaTime;
            }
        }
    }

    protected abstract void FixedUpdate();

    protected float GetAxisRaw(string axis, float dir)
    {
        if (Input.GetButtonDown(axis))
        {
            if (dir == 0f)
            {
                return 0f;
            }
            else if (dir < 0)
            {
                return -1f;
            }
            else
            {
                return 1f;
            }
        }
        else
        {
            return 0f;
        }
    }

    protected virtual bool CanMoveH(float dirH, bool isJumping)
    {
        float xDiff = playerTransform.position.x - Camera.main.transform.position.x;
        if (Mathf.Abs(xDiff) <= 0.01)
        {
            return true;
        }
        else
        {
            if (dirH < 0)
            {
                if (xDiff < 0)
                {
                    return (CameraController.cameraWidth / 2 + xDiff > 1);
                }
                else
                {
                    return true;
                }
            }
            else if (dirH > 0)
            {
                if (xDiff > 0)
                {
                    return (CameraController.cameraWidth / 2 - xDiff > 1);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (isJumping)
                {
                    return (CameraController.cameraWidth / 2 - Mathf.Abs(xDiff) > 1);
                }
                else
                {
                    return false;
                }
            }
        }
    }

    protected bool CanMoveV(float dirV, bool isJumping)
    {
        float yDiff = playerTransform.position.y - Camera.main.transform.position.y;
        if (Mathf.Abs(yDiff) <= 0.01)
        {
            return true;
        }
        else
        {
            if (dirV < 0)
            {
                if (yDiff < 0)
                {
                    return (CameraController.cameraHeight / 2 + yDiff > 1);
                }
                else
                {
                    return true;
                }
            }
            else if (dirV > 0)
            {
                if (yDiff > 0)
                {
                    return (CameraController.cameraHeight / 2 - yDiff > 1);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (isJumping)
                {
                    return (CameraController.cameraHeight / 2 - Mathf.Abs(yDiff) > 1);
                }
                else
                {
                    return false;
                }
            }
        }
    }

    protected void UsePotion()
    {
        if (GameManager.gameManager.potionNumber > 0)
        {
            RestaureHealth((int)(GameManager.gameManager.potionHeal * maxHp));
            GameManager.gameManager.potionNumber--;
        }
    }

    protected void CheckForInputs()
    {
        if (Input.GetButtonDown(potionBindName))
        {
            UsePotion();
        }
    }

    public void DropWeapon()
    {
        //WeaponController myWeapon = gameObject.GetComponentInChildren<WeaponController>();
        if (myWeapon != null)
        {
            myWeapon.SetSortingLayer("Foreground");
            myWeapon.SetHasOwner(false);
            myWeapon.gameObject.transform.parent = null;
            myWeapon.transform.localScale = new Vector3(Mathf.Abs(myWeapon.transform.localScale.x), Mathf.Abs(myWeapon.transform.localScale.y), Mathf.Abs(myWeapon.transform.localScale.z));
            //myWeapon.transform.localRotation = new Quaternion(myWeapon.transform.localRotation.x, myWeapon.transform.localRotation.y, myWeapon.transform.localRotation.z * (faceRight ? 1 : -1), myWeapon.transform.localRotation.w);
            GameManager.gameManager.AddObjectToBeCleaned(myWeapon.gameObject);
            myWeapon = null;
        }
    }

    public void DropTrinket()
    {
        if (myTrinket != null)
        {
            myTrinket.SetOwner(false);
            myTrinket.ToggleSprite();
            myTrinket.gameObject.transform.parent = null;
            myTrinket.ResetPlayer();
            GameManager.gameManager.AddObjectToBeCleaned(myTrinket.gameObject);
            myTrinket = null;
        }
    }

    public void EnterFight()
    {
        if (isFighting)
        {
            timeToQuitFightLeft = timeToQuitFight;
        }
        else
        {
            isFighting = true;
        }
    }

    private IEnumerator OnQuittingFight()
    {
        isAlreadyTryingToLeaveFight = true;
        yield return new WaitUntil(() => timeToQuitFightLeft <= 0f);
        isFighting = false;
        isAlreadyTryingToLeaveFight = false;
        timeToQuitFightLeft = timeToQuitFight;
    }

    private void RestaureHealth(float health)
    {
        hp = Mathf.Min(hp + health, maxHp);
    }

    public void RemoveHealth(float health)
    {
        if (!isGod)
        {
            hp -= health * damageReceivedMultiplier;
        }
        EnterFight();
    }

    public void SetHealth(float health)
    {
        this.hp = health;
    }

    public float GetHealth()
    {
        return hp;
    }

    public float GetDirH()
    {
        return dirH;
    }

    public float GetDirV()
    {
        return dirV;
    }

    protected void Flip()
    {
        faceRight = !faceRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public void SetMaxSpeed(float maxSpeed)
    {
        this.maxSpeed = maxSpeed;
    }

    public float GetDamageDoneMultiplier()
    {
        return damageDoneMultiplier;
    }

    public void SetDamageDoneMultiplier(float damageDoneMultiplier)
    {
        this.damageDoneMultiplier = damageDoneMultiplier;
    }

    public float GetDamageReceivedMultiplier()
    {
        return damageReceivedMultiplier;
    }

    public void SetDamageReceivedMultiplier(float damageReceivedMultiplier)
    {
        this.damageReceivedMultiplier = damageReceivedMultiplier;
    }

    public float GetAttackSpeedMultipler()
    {
        return attackSpeedMultiplier;
    }

    public void SetAttackSpeedMultiplier(float attackSpeedMultiplier)
    {
        this.attackSpeedMultiplier = attackSpeedMultiplier;
    }

    public float GetSpeedMutiplier()
    {
        return speedMultiplier;
    }

    public void SetSpeedMultiplier(float speedMultiplier)
    {
        this.speedMultiplier = speedMultiplier;
    }

    public float GetEnemySpeedMultiplier()
    {
        return enemySpeedMultiplier;
    }

    public void SetEnemySpeedMultiplier(float enemySpeedMultiplier)
    {
        this.enemySpeedMultiplier = enemySpeedMultiplier;
    }

    public float GetEnemySpeedMultiplierDuration()
    {
        return enemySpeedMultiplierDuration;
    }

    public void SetEnemySpeedMultiplierDuration(float enemySpeedMultiplierDuration)
    {
        this.enemySpeedMultiplierDuration = enemySpeedMultiplierDuration;
    }

    public float GetEnemyBleedPercentage()
    {
        return enemyBleedPercentage;
    }

    public void SetEnemyBleedPercentage(float enemyBleedPercentage)
    {
        this.enemyBleedPercentage = enemyBleedPercentage;
    }

    public float GetEnemyBleedDuration()
    {
        return enemyBleedDuration;
    }

    public void SetEnemyBleedDuration(float enemyBleedDuration)
    {
        this.enemyBleedDuration = enemyBleedDuration;
    }

    public bool GetStunEnemy()
    {
        return stunEnemy;
    }

    public void SetStunEnemy(bool stunEnemy)
    {
        this.stunEnemy = stunEnemy;
    }

    public float GetStunEnemyDuration()
    {
        return stunEnemyDuration;
    }

    public void SetStunEnemyDuration(float stunEnemyDuration)
    {
        this.stunEnemyDuration = stunEnemyDuration;
    }

    public TrinketController GetMyTrinket()
    {
        return myTrinket;
    }

    public void SetMyTrinket(TrinketController myTrinket)
    {
        this.myTrinket = myTrinket;
    }

    public WeaponController GetMyWeapon()
    {
        return myWeapon;
    }

    public void SetMyWeapon(WeaponController myWeapon)
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        myWeapon.SetSortingLayer(sr.sortingLayerName, sr.sortingOrder);
        this.myWeapon = myWeapon;
    }

    public bool GetIsGod()
    {
        return isGod;
    }

    public void SetIsGod(bool isGod)
    {
        this.isGod = isGod;
    }

    public bool GetIsBlocked()
    {
        return isBlocked;
    }

    public void SetIsBlocked(bool isBlocked)
    {
        this.isBlocked = isBlocked;
    }

    public bool GetIsFighting()
    {
        return isFighting;
    }

    public void SetIsFighting(bool isFighting)
    {
        this.isFighting = isFighting;
    }
}