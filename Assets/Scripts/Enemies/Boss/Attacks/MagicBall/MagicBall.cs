using UnityEngine;
using System.Collections;

public class MagicBall : MonoBehaviour
{

    BossController boss;
    public bool isSet;

    private Transform target;
    private float hitDamage;
    private string enemyTag;
    private float moveSpeed;
    private bool hitSomething;
    private float timeBeforeExplosion;
    private float explosionRange;
    private float explosionDamage;
    private bool hasExplosionTimerStarted;
    private bool hasExploded;

    private float timeBeforeActivation;
    private bool isActivated;
    private bool startedActivation;

    private void Awake()
    {
        boss = null;
        isSet = false;
        hitDamage = 30f;
        enemyTag = "Player1";
        target = null;
        moveSpeed = 2.5f;
        hitSomething = false;
        timeBeforeExplosion = 2f;
        explosionRange = 3f;
        explosionDamage = 30f;
        hasExplosionTimerStarted = false;
        hasExploded = false;
        isActivated = false;
        startedActivation = false;
        timeBeforeActivation = 2f;
    }

    private IEnumerator OnActivation()
    {
        startedActivation = true;
        yield return new WaitForSeconds(timeBeforeActivation);
        startedActivation = false;
        isActivated = true;
    }

    private IEnumerator OnExplosion()
    {
        hasExplosionTimerStarted = true;
        yield return new WaitForSeconds(timeBeforeExplosion);
        hasExploded = true;

    }

    private void Move()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * moveSpeed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, GameManager.gameManager.player1.transform.position, Time.deltaTime * moveSpeed);
        }
    }

    private void Update()
    {
        if (isSet)
        {
            if (isActivated)
            {
                if (hitSomething)
                {
                    if (!hasExplosionTimerStarted)
                    {
                        StartCoroutine(OnExplosion());
                    }
                }
                else
                {
                    Move();
                }

                if (hasExploded)
                {
                    if (Vector2.Distance(GameManager.gameManager.player1.transform.position, transform.position) <= explosionRange)
                    {
                        GameManager.gameManager.player1.GetComponent<PlayerController>().RemoveHealth(explosionDamage);
                    }
                    if (Vector2.Distance(GameManager.gameManager.player2.transform.position, transform.position) <= explosionRange)
                    {
                        GameManager.gameManager.player2.GetComponent<PlayerController>().RemoveHealth(explosionDamage);
                    }
                    Destroy(gameObject);
                }
            }
            else
            {
                if (!startedActivation)
                {
                    StartCoroutine(OnActivation());
                }
            }
            
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSet)
        {
            if (isActivated)
            {
                if (!hitSomething)
                {
                    if (collision.gameObject.CompareTag("Ground"))
                    {
                        hitSomething = true;
                    }
                    else if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
                    {
                        collision.gameObject.GetComponent<PlayerController>().RemoveHealth(hitDamage);
                        hitSomething = true;
                    }
                }
            } 
        }
    }

    public void SetMagicBall(int target, BossController boss)
    {
        this.target = (target == 1) ? GameManager.gameManager.player1.transform : GameManager.gameManager.player2.transform;
        this.boss = boss;
        isSet = true;
    }

    private void OnDestroy()
    {
        boss.UpdateBallsLeft(-1);
    }

}
