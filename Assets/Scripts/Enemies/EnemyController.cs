using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour
{
     public bool faceRight = false;

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
    protected float detectionRange;
    protected bool canAttack = false;


    // Update is called once per frame
    protected virtual void Update()
    {
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

    public void RemoveHealth(int loss)
    {
        health -= loss;
    }



    protected void Flip()
    {
        faceRight = !faceRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    protected abstract IEnumerator Attack();
}
