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
    protected int damage;
    protected float damageMultiplier;
    protected float detectionRange;
    protected bool canAttack = false;
    protected bool isStunned = false;
    protected Rigidbody2D rb2d;

    protected virtual void Awake()
    {
        damageMultiplier = 1f;
        canAttack = false;
        isStunned = false;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Debug.Log("Ennemy health " + health);
        if (this.health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Action();
        }
    }

    protected abstract void Action();

    protected void MoveTowards(Vector2 target)
    {
        Vector2 nextPosition = Vector2.MoveTowards(enemyTransform.position, target, speed * Time.deltaTime);
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

    public IEnumerator StunnedRoutine()
    {
        isStunned = true;
        yield return new WaitForSeconds(0.25f);
        isStunned = false;
    }

    public bool GetIsStunned()
    {
        return isStunned;
    }

    public void SetIsStunned(bool isStunned)
    {
        this.isStunned = isStunned;
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
}
