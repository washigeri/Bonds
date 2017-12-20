using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AliveEnemy : EnemyController {

    protected bool hasATarget = false;
    protected bool isFlying;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player1");
        isSpirit = false;
    }

    private void LookForPlayer()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player1");
        }
    }

    protected override void MoveToward(Vector2 target)
    {
        if (!isFlying)
        {
            if (player.transform.position.x < enemyTransform.position.x)
            {
                if (faceRight)
                {
                    Flip();
                }
                rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
            }
            else
            {
                if (!faceRight)
                {
                    Flip();
                }
                rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            }
        }
        else
        {
            MoveTowardFlying(target);
        }
       
    }
    
    protected void MoveTowardFlying(Vector2 target)
    {
        Vector2 velocity = rb2d.velocity;
        Vector2 direction = (new Vector3(target.x, target.y, 0f) - transform.position).normalized;
        if (target.x < enemyTransform.position.x)
        {
            if (faceRight)
            {
                Flip();
            }
            velocity.x = -speed * Mathf.Abs(direction.x);
        }
        else
        {
            if (!faceRight)
            {
                Flip();
            }
            velocity.x = speed * Mathf.Abs(direction.x);
        }
        if (target.y > enemyTransform.position.y)
        {
            velocity.y = speed * Mathf.Abs(direction.y);
        }
        else
        {
            velocity.y = -speed * Mathf.Abs(direction.y);
        }
        rb2d.velocity = velocity;
    }

    protected override void Action()
    {
        rb2d.AddForce(Vector2.zero);
        if (player != null)
        {
            if (isFlying)
            {
                ActionFlying();
            }
            else
            {
                ActionOnGround();
            }
        }
        else
        {
            LookForPlayer();
        }
    }

    private void ActionOnGround()
    {
        int dirToLook = (player.transform.position.x > enemyTransform.position.x) ? 1 : -1;
        Vector2 end;
        if (Mathf.Abs(enemyTransform.position.x - player.transform.position.x) <= detectionRange || hasATarget)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemyTransform.position, Vector2.right * dirToLook, (hasATarget ? Mathf.Infinity : detectionRange), ~layerMask);
            if (hasATarget && !player.GetComponent<Player1Controller>().grounded)
            {
                hasATarget = true;
            }
            else if (hit.collider != null)
            {
                hasATarget = hit.collider.CompareTag(player.tag);
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
            if (!isStunned)
            {
                end = new Vector2(enemyTransform.position.x + attackRange * dirToLook, enemyTransform.position.y);
                canAttack = Physics2D.Linecast(enemyTransform.position, end, 1 << LayerMask.NameToLayer(player.tag));
                if (canAttack)
                {
                    Stop();
                    if (!isOnCD)
                    {
                        StartCoroutine(Attack());
                    }
                }
                else
                {
                    MoveToward(player.transform.position);
                }
            }
        }
        else
        {
            canAttack = false;
        }
    }

    private void ActionFlying()
    {
        Vector2 direction = (player.transform.position - enemyTransform.position);
        Vector2 end;
        if (direction.magnitude <= detectionRange || hasATarget)
        {
            end = new Vector2(enemyTransform.position.x, enemyTransform.position.y) + direction.normalized * detectionRange;
            RaycastHit2D hit = Physics2D.Raycast(enemyTransform.position, direction.normalized, (hasATarget ? Mathf.Infinity : detectionRange), ~layerMask);
            if (hasATarget && !player.GetComponent<Player1Controller>().grounded)
            {
                hasATarget = true;
            }
            else if (hit.collider != null)
            {
                hasATarget = hit.collider.CompareTag(player.tag);
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
            if (!isStunned)
            {
                end = new Vector2(enemyTransform.position.x, enemyTransform.position.y) + direction.normalized * attackRange;
                canAttack = Physics2D.Linecast(enemyTransform.position, end, 1 << LayerMask.NameToLayer(player.tag));
                if (canAttack)
                {
                    Stop();
                    if (!isOnCD)
                    {
                        StartCoroutine(Attack());
                    }
                }
                else
                {
                    MoveToward(player.transform.position);
                }
            }
        }
        else
        {
            canAttack = false;
        }
    }

}
