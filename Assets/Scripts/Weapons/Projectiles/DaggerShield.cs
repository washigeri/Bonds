using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerShield : MonoBehaviour {

    private bool isSet;
    private PlayerController owner;

    private float damage;
    private float enemySpeedMultiplier;
    private float enemySpeedMultiplierDuration;
    private string enemyTag;
    private Vector3 startPosition;
    private float timeLeft;
    private float timeBetweenTwoHits;
    private bool isReloading;
    private bool startedReloading;
    private BoxCollider2D bCollider2D;

    private void Awake()
    {
        isSet = false;
        bCollider2D = GetComponent<BoxCollider2D>();
        enemySpeedMultiplier = 0.5f;
        enemySpeedMultiplierDuration = 5f;
        timeLeft = 0f;
        timeBetweenTwoHits = 0.5f;
        isReloading = false;
        startedReloading = false;
        owner = null;
    }

    private void UpdateTime()
    {
        if (timeLeft <= 0f)
        {
            Destroy(gameObject);
        }
        else
        {
            timeLeft -= Time.deltaTime;
        }
    }

    private void UpdateCollider()
    {
        if (isReloading)
        {
            if (bCollider2D.enabled)
            {
                bCollider2D.enabled = false;
            }
        }
        else
        {
            if (!bCollider2D.enabled)
            {
                bCollider2D.enabled = true;
            }
        }
    }

    private void Update()
    {
        if (isSet)
        {
            UpdateTime();
            UpdateCollider();
        }
    }

    private IEnumerator OnReload()
    {
        startedReloading = true;
        yield return new WaitForSeconds(timeBetweenTwoHits);
        isReloading = false;
        startedReloading = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSet)
        {
            if (!isReloading)
            {
                if (collision.gameObject.CompareTag(enemyTag))
                {
                    EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                    enemy.RemoveHealth(damage * owner.GetDamageDoneMultiplier(), false);
                    enemy.SetSpeedMultiplierParameters(enemySpeedMultiplier, enemySpeedMultiplierDuration);
                    if (owner.GetEnemyBleedDuration() > 0f)
                    {
                        enemy.SetBleedingParameters(owner.GetEnemyBleedPercentage() * damage * owner.GetDamageDoneMultiplier(), owner.GetEnemyBleedDuration());
                    }
                    if (owner.GetStunEnemy())
                    {
                        enemy.SetStunned(true, owner.GetStunEnemyDuration());
                    }
                    else
                    {
                        enemy.SetStunned(true, -1f);
                    }
                    isReloading = true;
                    StartCoroutine(OnReload());
                }
                else if (collision.gameObject.CompareTag("Boss"))
                {
                    EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
                    enemy.RemoveHealth(damage * owner.GetDamageDoneMultiplier(), false);
                    enemy.SetSpeedMultiplierParameters(enemySpeedMultiplier, enemySpeedMultiplierDuration);
                    if (owner.GetEnemyBleedDuration() > 0f)
                    {
                        enemy.SetBleedingParameters(owner.GetEnemyBleedPercentage() * damage * owner.GetDamageDoneMultiplier(), owner.GetEnemyBleedDuration());
                    }
                }
            }
            else
            {
                if (!startedReloading)
                {
                    StartCoroutine(OnReload());
                }
            }
        }
    }

    public void SetParameters(int owner, float duration, Vector3 startPosition, Vector3 direction, float damage, string enemyTag)
    {
        if (owner == 1)
        {
            this.owner = GameManager.gameManager.player1.GetComponent<PlayerController>();
        }
        else if (owner == 2)
        {
            this.owner = GameManager.gameManager.player2.GetComponent<PlayerController>();
        }
        if(direction == Vector3.right)
        {
            transform.Rotate(Vector3.forward * -90); 
        }
        else if(direction == Vector3.left)
        {
            transform.Rotate(Vector3.forward * 90);
        }
        else if(direction == Vector3.up)
        {
            transform.localScale = Vector3.one;
        }
        else if(direction == Vector3.up + Vector3.right)
        {
            transform.Rotate(Vector3.forward * -45);
        }
        else if(direction == Vector3.up + Vector3.left)
        {
            transform.Rotate(Vector3.forward * 45);
        }
        else if(direction == Vector3.down)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
        else if(direction == Vector3.down + Vector3.right)
        {
            transform.Rotate(Vector3.forward * -135);
        }
        else if(direction == Vector3.down + Vector3.left)
        {
            transform.Rotate(Vector3.forward * 135);
        }
        this.timeLeft = duration;
        this.startPosition = startPosition;
        this.damage = damage;
        this.enemyTag = enemyTag;
        isSet = true;
    }
}
