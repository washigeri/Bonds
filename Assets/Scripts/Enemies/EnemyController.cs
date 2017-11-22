﻿using System.Collections;
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
    private bool isSlowDown;
    private bool slowDown;
    //private bool isAlreadySlowDown;
    protected Rigidbody2D rb2d;

    protected virtual void Awake()
    {
        damageMultiplier = 1f;
        speedMultiplier = 1f;
        speedMultiplierDuration = 0f;
        canAttack = false;
        isStunned = false;
        isSlowDown = false;
        //isAlreadySlowDown = false;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Debug.Log("Ennemy health " + health);
        if (this.health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            if (speedMultiplierDuration > 0f)
            {
                speedMultiplierDuration -= Time.deltaTime;
            }
            if (stunned)
            {
                StartCoroutine(StunnedRoutine());
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

    public void RemoveHealth(float loss)
    {
        health -= loss;
    }

    private IEnumerator StunnedRoutine()
    {
        stunned = false;
        Debug.Log("starting stunned routine");
        yield return new WaitForSeconds(0.25f);
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

    private IEnumerator Slow()
    {
        Debug.Log("starting slow routine");
        slowDown = false;
        yield return new WaitUntil(() => speedMultiplierDuration <= 0f);
        this.speedMultiplier = 1f;
        isSlowDown = false;
    }

}
