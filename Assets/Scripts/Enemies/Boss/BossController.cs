﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    public float roomMinX;
    public float roomMaxX;
    public float roomMiny;
    public float roomMaxY;

    private Vector3 defaultPosition;
    private bool isMovingTowardDefaultPosition;

    private BossWeaponController weapon;

    private float speedMutliplier;

    private bool isGod;
    private bool isBlocked;

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



    protected override void Awake()
    {
        weapon = GetComponentInChildren<BossWeaponController>();
        health = 2000f;
        speed = 5;
        speedMutliplier = 1f;
        isGod = false;
        isBlocked = false;
        canDrop = true;
        startedMagicBalls = false;
        timeBetweenMagicBalls = 1f;
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
    }

    protected override void Update()
    {
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
        else
        {
            #region Move To Default Position
            if (isMovingTowardDefaultPosition)
            {
                MoveTo(defaultPosition, speed * 2f);
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
                isMovingTowardRayCastPosition = true;
                rb2d.gravityScale = 0f;
            }
            if (isMovingTowardRayCastPosition)
            {
                MoveTo(rayCastPosition, speed * 2f);
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
                weapon.SetStartQuickAttack(true);
            }
            if (Input.GetButtonDown("HealP1"))
            {
                DropFeathers();
            }
        }
    }


    private IEnumerator QuickAttack()
    {
        yield return null;
    }

    private IEnumerator SlowAttack()
    {
        yield return null;
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

    private void MoveTo(Vector3 target, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
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
}
