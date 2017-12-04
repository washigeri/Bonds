using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    public float roomMinX;
    public float roomMaxX;
    public float roomMiny;
    public float roomMaxY;

    private int phase;

    private Vector3 defaultPosition;
    private bool isMovingTowardDefaultPosition;

    private BossWeaponController weapon;

    private float speedMutliplier;

    private float maxHp;
    private bool isGod;
    private bool isBlocked;
    private bool isBusy;

    private Vector3[] ballSpanws;
    private bool canCastBalls;
    private bool startedMagicBalls;
    private float timeBetweenMagicBalls;
    private int ballsCount;
    private int ballsLeft;

    private bool startedMagicRay;
    private int raysCount;
    private int raysLeft;
    private bool canCastRays;
    private bool isMovingTowardRayCastPosition;
    private Vector3 rayCastPosition;

    private int numberOfFeathers;

    private bool startQuickAttack;
    private bool startSlowAttack;

    private float distanceToPlayer1;

    protected override void Awake()
    {
        phase = 0;
        weapon = GetComponentInChildren<BossWeaponController>();
        maxHp = 2000f;
        attackRange = 2.5f;
        health = maxHp;
        speed = 5;
        speedMutliplier = 1f;
        isGod = false;
        isBlocked = false;
        isBusy = false;
        canDrop = true;
        startedMagicBalls = false;
        timeBetweenMagicBalls = 1f;
        ballSpanws = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero };
        ballsCount = 4;
        ballsLeft = ballsCount;
        canCastBalls = false;
        startedMagicRay = false;
        raysCount = 2;
        raysLeft = raysCount;
        numberOfFeathers = 1;
        roomMinX = 0f;
        roomMaxX = (float)CameraController.cameraWidth;
        roomMiny = 0f;
        roomMaxY = (float)CameraController.cameraHeight;
        defaultPosition = new Vector3((roomMaxX - roomMinX) / 2f, roomMiny, 0f);
        rb2d = GetComponent<Rigidbody2D>();
        isMovingTowardRayCastPosition = false;
        canCastRays = false;
        rayCastPosition = new Vector3(2f * roomMaxY / 3f, roomMaxX / 2f, 0f);
        startQuickAttack = false;
        startSlowAttack = false;
        distanceToPlayer1 = 0f;
    }

    protected override void Update()
    {
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
        else
        {
            #region phase
            if (health < 0.15f * maxHp)
            {
                phase = 3;
            }
            else if (health < 0.65f * maxHp)
            {
                phase = 2;
            }
            else
            {
                phase = 1;
            }
            #endregion
            #region Move To Default Position
            if (isMovingTowardDefaultPosition)
            {
                GoTo(defaultPosition, speed * 2f);
                if (transform.position == defaultPosition)
                {
                    isMovingTowardDefaultPosition = false;
                    rb2d.gravityScale = 1f;
                }
            }
            #endregion
            #region Rays
            if (Input.GetButtonDown("SkillP1") && !startedMagicRay)
            {
                isBusy = true;
                isMovingTowardRayCastPosition = true;
                rb2d.gravityScale = 0f;
            }
            if (isMovingTowardRayCastPosition)
            {
                if (startSlowAttack)
                {
                    startSlowAttack = false;
                }
                if (startQuickAttack)
                {
                    startQuickAttack = false;
                }
                GoTo(rayCastPosition, speed * 2f);
                if (transform.position == rayCastPosition)
                {
                    isMovingTowardRayCastPosition = false;
                    canCastRays = true;
                }
            }
            if (canCastRays)
            {
                canCastRays = false;
                StartCoroutine(CastMagicRays(false, true));
            }
            #endregion
            #region Balls
            if (Input.GetButtonDown("StrongP1") && !startedMagicBalls)
            {
                canCastBalls = true;
            }
            if (canCastBalls)
            {
                canCastBalls = false;
                StartCoroutine(CastMagicBalls(true, true));
            }
            #endregion
            if (Input.GetButtonDown("WeakP1"))
            {
                SlowAttack();
            }
            if (startSlowAttack)
            {
                distanceToPlayer1 = Vector3.Distance(transform.position, GameManager.gameManager.player1.transform.position);
                if (distanceToPlayer1 <= attackRange)
                {
                    weapon.SetStartSlowAttack(true);
                }
                else
                {
                    MoveTo(GameManager.gameManager.player1.transform.position);
                }
            }
            if (Input.GetButtonDown("HealP1"))
            {
                DropFeathers();
            }
            if (Input.GetButtonDown("InteractP1"))
            {
                Target();
            }
        }
    }


    private void QuickAttack()
    {
        if (!isBusy)
        {
            startQuickAttack = true;
        }
    }

    private void SlowAttack()
    {
        if (!isBusy)
        {
            startSlowAttack = true;
        }
    }

    private void PopMagicBall()
    {
        int target = Random.Range(1, 3);
        GameObject magicBall = Instantiate(Resources.Load("Prefabs/Enemies/Boss/Attacks/MagicBall"), transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
        magicBall.GetComponent<MagicBall>().SetMagicBall(target, this);
    }

    private IEnumerator CastMagicBalls(bool isGod, bool isBlocked)
    {
        startedMagicBalls = true;
        this.isGod = isGod;
        this.isBlocked = isBlocked;
        PopMagicBall();
        yield return new WaitForSeconds(timeBetweenMagicBalls);
        PopMagicBall();
        yield return new WaitForSeconds(timeBetweenMagicBalls);
        PopMagicBall();
        yield return new WaitForSeconds(timeBetweenMagicBalls);
        PopMagicBall();
        yield return new WaitUntil(() => ballsLeft <= 0);
        ballsLeft = ballsCount;
        this.isGod = false;
        this.isBlocked = false;
        startedMagicBalls = false;
    }

    public void Target()
    {
        Vector3 targetPosition;
        targetPosition = GameManager.gameManager.player1.transform.position;
        if (targetPosition.x - transform.position.x > 0)
        {
            if (!faceRight)
            {
                Flip();
            }
        }
        else
        {
            if (faceRight)
            {
                Flip();
            }
        }
    }

    public void GoTo(Vector3 target, float speed)
    {
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (nextPosition.x - transform.position.x > 0)
        {
            if (!faceRight)
            {
                Flip();
            }
        }
        else if (nextPosition.x - transform.position.x < 0)
        {
            if (faceRight)
            {
                Flip();
            }
        }
        transform.position = nextPosition;
    }

    public void MoveTo(Vector3 target)
    {
        if (target.x - transform.position.x > 0)
        {
            if (!faceRight)
            {
                Flip();
            }
        }
        else
        {
            if (faceRight)
            {
                Flip();
            }
        }
        rb2d.velocity = new Vector3(speed * (faceRight ? 1 : -1), rb2d.velocity.y, 0f);
    }

    private void PopMagicRay(int direction)
    {
        GameObject magicRay = Instantiate(Resources.Load("Prefabs/Enemies/Boss/Attacks/RotatingLaser"), transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
        magicRay.GetComponent<LaserScript>().SetLaser(this, direction);
    }

    private IEnumerator CastMagicRays(bool isGod, bool isBlocked)
    {
        startedMagicRay = true;
        this.isGod = isGod;
        this.isBlocked = isBlocked;
        PopMagicRay(-1);
        PopMagicRay(1);
        yield return new WaitUntil(() => raysLeft <= 0);
        raysLeft = raysCount;
        this.isGod = false;
        this.isBlocked = false;
        startedMagicRay = false;
        isMovingTowardDefaultPosition = true;
    }

    private void PopFeather()
    {
        Vector3 startPos = new Vector3(Random.Range(roomMinX + 1f, roomMaxX - 1f), roomMaxY, 0f);
        Debug.Log("start pos according to boss = " + startPos);
        Instantiate(Resources.Load("Prefabs/Enemies/Boss/Attacks/Feather"), startPos, Quaternion.Euler(0f, 0f, 0f));
    }

    private void DropFeathers()
    {
        for (int i = 0; i < numberOfFeathers; i++)
        {
            PopFeather();
        }
    }

    public void UpdateBallsLeft(int increment)
    {
        ballsLeft += increment;
    }

    public void UpdateRaysLeft(int increment)
    {
        raysLeft += increment;
    }

    public void RemoveHealth(float damage)
    {
        if (!isGod)
        {
            health -= damage;
        }
    }

    protected override void Action()
    {

    }

    protected override IEnumerator Attack()
    {
        yield return null;
    }

    public void SetIsBusy(bool isBusy)
    {
        this.isBusy = isBusy;
    }

    public void SetStartSlowAttack(bool startSlowAttack)
    {
        this.startSlowAttack = startSlowAttack;
    }

    public void SetStartQuickAttack(bool startQuickAttack)
    {
        this.startQuickAttack = startQuickAttack;
    }
}
