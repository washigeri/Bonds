using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeadEnemy : EnemyController
{

    private int hasATarget = 0;

    protected override void Awake()
    {
        base.Awake();
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        player = null;
    }

    private int GetTarget()
    {
        float dP1 = Vector2.Distance(GameManager.player1.transform.position, enemyTransform.position);
        float dP2 = Vector2.Distance(GameManager.player2.transform.position, enemyTransform.position);
        if (dP1 <= dP2)
        {
            player = GameManager.player1;
            return 1;
        }
        else
        {
            player = GameManager.player2;
            return 2;
        }
    }

    private bool IsVisible(int playerID)
    {
        RaycastHit2D hit = Physics2D.Raycast(enemyTransform.position, (player.transform.position - enemyTransform.position), (playerID == hasATarget ? Mathf.Infinity : detectionRange), ~layerMask);
        if (hit.collider != null)
        {
            return hit.collider.CompareTag(player.tag);
        }
        else
        {
            return false;
        }
    }

    private void TryToAttackOrMove()
    {
        int dirToLook = (player.transform.position.x > enemyTransform.position.x) ? 1 : -1;
        Vector2 end = new Vector2(enemyTransform.position.x + attackRange * dirToLook, enemyTransform.position.y);
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

    protected override void Action()
    {
        rb2d.AddForce(Vector2.zero);
        int playerID = GetTarget();
        if (IsVisible(playerID))
        {
            hasATarget = playerID;
            player = hasATarget == 1 ? GameManager.player1 : GameManager.player2;
            TryToAttackOrMove();
        }
        else
        {
            if (playerID == 1)
            {
                player = GameManager.player2;
            }
            else
            {
                player = GameManager.player1;
            }
            playerID = playerID == 1 ? 2 : 1;
            if (IsVisible(playerID))
            {
                hasATarget = playerID;
                player = hasATarget == 1 ? GameManager.player1 : GameManager.player2;
                TryToAttackOrMove();
            }
            else
            {
                hasATarget = 0;
            }
        }
    }
}
