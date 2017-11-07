using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : PlayerController
{

    private bool isDashing = false;
    private bool isDashOnCoolDown = false;
    public float dashAcceleration = 5f;
    private float dashCoolDown = 0.25f;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        isPlayer1 = false;
        rb2d.gravityScale = 0f;
        potionBindName = "HealP2";
        interactBindName = "InteractP2";
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetButtonDown("Dash") && !isDashOnCoolDown)
        {
            StartCoroutine(Dash());
        }
    }

    protected override void FixedUpdate()
    {
        MoveWithVelocity();
    }

    private void MoveWithVelocity()
    {
        dirH = Input.GetAxisRaw("HorizontalP2");
        dirV = Input.GetAxisRaw("VerticalP2");
        bool canMoveH = CanMoveH(dirH, isDashing);
        bool canMoveV = CanMoveV(dirV, isDashing);
        if (!canMoveH)
        {
            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
        }
        if (!canMoveV)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
        }
        if (canMoveH || canMoveV)
        {
            Vector2 newVelocity = new Vector2(canMoveH ? dirH : 0f, canMoveV ? dirV : 0f);
            if (newVelocity.x != 0f || newVelocity.y != 0f)
            {
                newVelocity.Normalize();
                newVelocity *= maxSpeed;
            }
            rb2d.velocity = newVelocity;

            if (!isDashing)
            {
                if (Mathf.Abs(rb2d.velocity.magnitude) > maxSpeed)
                {
                    rb2d.velocity = rb2d.velocity.normalized * maxSpeed;
                }
            }

            if (dirH > 0 && !faceRight)
            {
                Flip();
            }

            else if (dirH < 0 && faceRight)
            {
                Flip();
            }
        }
    }

    private void MoveWithForces()
    {
        dirH = Input.GetAxisRaw("HorizontalP2");
        dirV = Input.GetAxisRaw("VerticalP2");
        if (CanMoveH(dirH, isDashing))
        {
            if (dirH * rb2d.velocity.x < maxSpeed)
            {
                rb2d.AddForce(Vector2.right * dirH * moveForce);
            }

            if (!isDashing)
            {
                if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
                {
                    rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
                }
            }

            if (dirH > 0 && !faceRight)
            {
                Flip();
            }

            else if (dirH < 0 && faceRight)
            {
                Flip();
            }
        }
        else
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        if (CanMoveV(dirV, isDashing))
        {
            if (dirV * rb2d.velocity.y < maxSpeed)
            {
                rb2d.AddForce(Vector2.up * dirV * moveForce);
            }

            if (!isDashing)
            {
                if (Mathf.Abs(rb2d.velocity.y) > maxSpeed)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Sign(rb2d.velocity.y) * maxSpeed);
                }
            }

        }
        else
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        maxSpeed *= dashAcceleration;
        rb2d.velocity = 10f * rb2d.velocity;
        rb2d.AddForce((faceRight ? 1 : -1) * new Vector2(dirH, dirV), ForceMode2D.Impulse);
        isDashOnCoolDown = true;
        Vector2 x = playerTransform.position;
        yield return new WaitForSeconds(0.1f);
        Vector2 y = playerTransform.position;
        isDashing = false;
        maxSpeed /= dashAcceleration;
        yield return new WaitForSeconds(dashCoolDown);
        isDashOnCoolDown = false;
        yield return null;
    }


}
