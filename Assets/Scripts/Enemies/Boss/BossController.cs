using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{

    public float roomMinX;
    public float roomMaxX;
    public float roomMiny;
    public float roomMaxY;

    private bool fightStarted;
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
    private bool startedMagicBallCD;
    private float magicBallsCD;
    private bool isMagicBallOnCD;

    private bool startedMagicRay;
    private int raysCount;
    private int raysLeft;
    private bool canCastRays;
    private bool isMovingTowardRayCastPosition;
    private Vector3 rayCastPosition;
    private float magicRaysCD;
    private bool isMagicRaysOnCD;
    private bool startedMagicRayCD;

    private int numberOfFeathers;
    private float featherCadency;
    private bool isFeatherOnCD;

    private bool startedMeleeCD;
    private float meleeCD;
    private bool isMeleeOnCD;
    private bool startQuickAttack;
    private bool startSlowAttack;
    private bool isDoingMeleeAttack;

    private float distanceToPlayer1;

    private int nextAttack;
    private float globalCD;
    private bool isOnGlobalCD;

    private bool isDead;

    protected override void Awake()
    {
        isDead = false;
        phase = 0;
        fightStarted = false;
        weapon = GetComponentInChildren<BossWeaponController>();
        maxHp = 7500f;
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
        startedMagicBallCD = false;
        magicBallsCD = 10f;
        isMagicBallOnCD = false;
        startedMagicRay = false;
        raysCount = 2;
        raysLeft = raysCount;
        numberOfFeathers = 1;
        featherCadency = 5f;
        isFeatherOnCD = false;
        roomMinX = -(float)CameraController.cameraWidth / 2f;
        roomMaxX = (float)CameraController.cameraWidth / 2f;
        roomMiny = -(float)CameraController.cameraHeight / 2f;
        roomMaxY = (float)CameraController.cameraHeight / 2f;
        defaultPosition = new Vector3((roomMaxX + roomMinX) / 2f, roomMiny + 1f, 0f);
        rb2d = GetComponent<Rigidbody2D>();
        isMovingTowardRayCastPosition = false;
        canCastRays = false;
        rayCastPosition = new Vector3((roomMaxX + roomMinX) / 2f, 2f * roomMaxY / 3f, 0f);
        magicRaysCD = 10f;
        startedMagicRayCD = false;
        isMagicRaysOnCD = false;
        startQuickAttack = false;
        startSlowAttack = false;
        distanceToPlayer1 = 0f;
        isMeleeOnCD = false;
        meleeCD = 6f;
        startedMeleeCD = false;
        nextAttack = 0;
        globalCD = 3f;
        isOnGlobalCD = false;
        isDoingMeleeAttack = false;
    }

    private IEnumerator WaitForInput()
    {
        yield return new WaitUntil(() => phase == 1);
    }

    protected override void Update()
    {
        if (health <= 0f)
        {
            isDead = true;
            Destroy(gameObject);
        }
        else
        {
            GetPhase();
            if (startedMeleeCD)
            {
                if (!isMeleeOnCD)
                {
                    StartCoroutine(WaitForMeleeCD());
                    StartCoroutine(WaitBetweenAttacks());
                }
            }
            if (startedMagicBallCD)
            {
                if (!isMagicBallOnCD)
                {
                    StartCoroutine(WaitForBallsCD());
                    StartCoroutine(WaitBetweenAttacks());
                }
            }
            if (startedMagicRayCD)
            {
                if (!isMagicRaysOnCD)
                {
                    StartCoroutine(WaitForRaysCD());
                }
            }
            if (isMovingTowardDefaultPosition)
            {
                GoTo(defaultPosition, 2f * speed);
                bool isGrounded = Physics2D.Linecast(transform.position, transform.position + 3 * Vector3.down, 1 << LayerMask.NameToLayer("Ground"));
                if (transform.position == defaultPosition || isGrounded)
                {
                    isMovingTowardDefaultPosition = false;
                    rb2d.gravityScale = 1f;
                    isBusy = false;
                }
            }
            if (startSlowAttack)
            {
                if (GameManager.gameManager.player1 != null)
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
            }
            if (GameManager.gameManager.player1 != null && GameManager.gameManager.player2 != null)
            {
                Phase();
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
        if (Mathf.Abs(transform.position.x - target.x) > attackRange / 2f)
        {
            rb2d.velocity = new Vector3(speed * (faceRight ? 1 : -1), rb2d.velocity.y, 0f);
        }
        else
        {
            Stop();
        }
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
        startedMagicRayCD = true;
        raysLeft = raysCount;
        this.isGod = false;
        this.isBlocked = false;
        startedMagicRay = false;
        isMovingTowardDefaultPosition = true;        
    }

    private void PopFeather()
    {
        Vector3 startPos = new Vector3(Random.Range(roomMinX + 1f, roomMaxX - 1f), roomMaxY - 1, 0f);
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

    private IEnumerator WaitForMeleeCD()
    {
        isMeleeOnCD = true;
        yield return new WaitForSeconds(meleeCD);
        isMeleeOnCD = false;
        startedMeleeCD = false;
    }

    private IEnumerator WaitForBallsCD()
    {
        isMagicBallOnCD = true;
        yield return new WaitForSeconds(magicBallsCD);
        isMagicBallOnCD = false;
        startedMagicBallCD = false;
    }

    private IEnumerator WaitForRaysCD()
    {
        isMagicRaysOnCD = true;
        yield return new WaitForSeconds(magicRaysCD);
        isMagicRaysOnCD = false;
        startedMagicRayCD = false;
    }

    private IEnumerator WaitBetweenAttacks()
    {
        isOnGlobalCD = true;
        yield return new WaitForSeconds(globalCD);
        isOnGlobalCD = false;
    }

    private IEnumerator WaitBetweenFeathers()
    {
        yield return new WaitForSeconds(featherCadency);
        isFeatherOnCD = false;
    }

    private void SlowAttack()
    {
        startSlowAttack = true;
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
        startedMagicBallCD = true;
        ballsLeft = ballsCount;
        this.isGod = false;
        this.isBlocked = false;
        startedMagicBalls = false;
    }

    private void Phase1()
    {
        if (!isOnGlobalCD)
        {
            if (!isBusy)
            {
                if (nextAttack == 0)
                {
                    if (!isMeleeOnCD)
                    {
                        SlowAttack();
                        nextAttack = 1;
                        isBusy = true;
                    }
                }
                else if (nextAttack == 1)
                {
                    if (!isMagicBallOnCD)
                    {
                        MoveTo(GameManager.gameManager.player1.transform.position);
                        if (!startedMagicBalls)
                        {
                            StartCoroutine(CastMagicBalls(false, true));
                            nextAttack = 0;
                        }
                    }
                    else
                    {
                        if (!isMeleeOnCD)
                        {
                            SlowAttack();
                            isBusy = true;
                        }
                    }
                }
            }
        }
    }

    private void Phase2()
    {
        if (!isOnGlobalCD)
        {
            if (!isBusy)
            {
                if (nextAttack == 0)
                {
                    if (!isMeleeOnCD)
                    {
                        SlowAttack();
                        nextAttack = 1;
                        isBusy = true;
                    }
                }
                else if (nextAttack == 1)
                {
                    if (!isMagicRaysOnCD && !startedMagicRay)
                    {
                        rb2d.gravityScale = 0f;
                        GoTo(rayCastPosition, speed * 2f);
                        if (transform.position == rayCastPosition)
                        {
                            isBusy = true;
                            StartCoroutine(CastMagicRays(false, true));
                            nextAttack = 2;
                        }
                    }
                    else
                    {
                        if (!isMeleeOnCD)
                        {
                            SlowAttack();
                            isBusy = true;
                        }
                    }
                }
            }
            if(nextAttack == 2 && !startedMagicRay)
            {
                if (!isMagicBallOnCD)
                {
                    if (!startedMagicBalls)
                    {
                        StartCoroutine(CastMagicBalls(false, false));
                        nextAttack = 0;
                    }
                }
            }
        }
    }

    private void Phase3()
    {
        if (health < 0.1f * maxHp)
        {
            featherCadency = 0.5f;
        }
        else if (health < 0.2f * maxHp)
        {
            featherCadency = 1f;
        }
        else if (health < 0.3f * maxHp)
        {
            featherCadency = 2f;
        }
        else if (health < 0.4f * maxHp)
        {
            featherCadency = 4f;
        }
        else
        {
            featherCadency = 5f;
        }
        Phase2();
        if (!isFeatherOnCD)
        {
            DropFeathers();
            isFeatherOnCD = true;
            StartCoroutine(WaitBetweenFeathers());
        }
    }

    private void Phase()
    {
        if (phase == 0)
        {
            if (!fightStarted)
            {
                if (Input.GetButtonDown("WeakP1") || Input.GetButtonDown("WeakP2") || Input.GetButtonDown("StrongP1") || Input.GetButtonDown("StrongP2") || Input.GetButtonDown("SkillP1") || Input.GetButtonDown("SkillP2"))
                {
                    phase = 1;
                    fightStarted = true;
                }
            }
        }
        else if (phase == 1)
        {
            Phase1();
        }
        else if (phase == 2)
        {
            Phase2();
        }
        else if (phase == 3)
        {
            Phase3();
        }
    }

    private void GetPhase()
    {
        if (fightStarted)
        {
            if (health < 0.20f * maxHp)
            {
                phase = 3;
            }
            else if (health < 0.75f * maxHp)
            {
                phase = 2;
            }
            else
            {
                phase = 1;
            }
        }
        else
        {
            phase = 0;
        }
    }

    private void MeleeAttack()
    {

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

    public void SetStartedMeleeCD(bool startedMeleeCD)
    {
        this.startedMeleeCD = startedMeleeCD;
    }

    public void SetIsDoingMeleeAttack(bool isDoingMeleeAttack)
    {
        this.isDoingMeleeAttack = isDoingMeleeAttack;
    }

    private void OnDestroy()
    {
        if (isDead)
        {
            GameManager.gameManager.GoToNextLevel();
        }
    }
}
