using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour
{


    [HideInInspector] public bool faceRight = false;

    public Transform enemyTransform;
    public Transform targetTransform;
    public LayerMask layerMask;


    protected float health;
    protected float attackRange;
    protected float speed;
    protected float damage;
    protected float detectionRange;
    protected bool canAttack = false;
    protected bool hasATarget = false;


    public void RemoveHealth(int loss)
    {
        health -= loss;
    }



    // Update is called once per frame
    protected virtual void Update()
    {

        if (this.health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            int dirToLook = (targetTransform.position.x > enemyTransform.position.x) ? 1 : -1;
            Vector2 end;
            if(Mathf.Abs(enemyTransform.position.x - targetTransform.position.x) <= detectionRange)
            {
                end = new Vector2(enemyTransform.position.x + detectionRange * dirToLook, enemyTransform.position.y);
                RaycastHit2D hit = Physics2D.Raycast(enemyTransform.position, Vector2.right * dirToLook, detectionRange, ~layerMask);
                Debug.DrawRay(enemyTransform.position, Vector2.right * dirToLook * detectionRange, Color.green);
                if (hit.collider != null)
                {
                    hasATarget = hit.collider.CompareTag("Player1");
                }
                else
                {
                    hasATarget = false;
                }
            }
            else
            {
                hasATarget = false;
            }
            if (hasATarget)
            {
                end = new Vector2(enemyTransform.position.x + attackRange * dirToLook, enemyTransform.position.y);
                canAttack = Physics2D.Linecast(enemyTransform.position, end, 1 << LayerMask.NameToLayer("Player1"));
                if (canAttack)
                {
                    StartCoroutine(Attack());
                }
                else
                {
                    MoveTowards(targetTransform.position);
                }
            }
            else
            {
                canAttack = false;
            }
        }
    }

    protected void MoveTowards(Vector2 target)
    {
        Vector2 nextPosition = Vector2.MoveTowards(enemyTransform.position, target, speed * Time.deltaTime);
        if (nextPosition.x - enemyTransform.position.x > 0)
        {
            if (faceRight)
            {
                Flip();
            }
        }
        else if (nextPosition.x - enemyTransform.position.x < 0)
        {
            if (!faceRight)
            {
                Flip();
            }
        }
        enemyTransform.position = nextPosition;
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
