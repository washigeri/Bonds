﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AliveEnemy : EnemyController {

    protected bool hasATarget = false;

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player1");
    }

    protected override void Action()
    {
        int dirToLook = (player.transform.position.x > enemyTransform.position.x) ? 1 : -1;
        Vector2 end;
        if (Mathf.Abs(enemyTransform.position.x - player.transform.position.x) <= detectionRange || hasATarget)
        {
            end = new Vector2(enemyTransform.position.x + detectionRange * dirToLook, enemyTransform.position.y);
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
            end = new Vector2(enemyTransform.position.x + attackRange * dirToLook, enemyTransform.position.y);
            canAttack = Physics2D.Linecast(enemyTransform.position, end, 1 << LayerMask.NameToLayer(player.tag));
            if (canAttack)
            {
                if (!isOnCD)
                {
                    StartCoroutine(Attack());
                }
            }
            else
            {
                MoveTowards(player.transform.position);
            }
        }
        else
        {
            canAttack = false;
        }
    }
}
