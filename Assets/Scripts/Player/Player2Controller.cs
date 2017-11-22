using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2Controller : PlayerController
{
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
        //Debug.Log("Awake player2");
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
        //Cette ligne permet au Trigger de l'arme de se réinitialiser meme si le personnage ne bouge pas
        //Il doit y avoir une vrai solution mais je ne trouve pas
        rb2d.AddForce(Vector2.zero);
        dirH = Input.GetAxisRaw("HorizontalP2");
        dirV = Input.GetAxisRaw("VerticalP2");
        bool canMoveH = CanMoveH(dirH, moveHability);
        bool canMoveV = CanMoveV(dirV, moveHability);
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
                newVelocity *= maxSpeed * speedMultiplier;
            }
            rb2d.velocity = newVelocity;

            if (!moveHability)
            {
                if (Mathf.Abs(rb2d.velocity.magnitude) > maxSpeed * speedMultiplier)
                {
                    rb2d.velocity = rb2d.velocity.normalized * maxSpeed * speedMultiplier;
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
        if (CanMoveH(dirH, moveHability))
        {
            if (dirH * rb2d.velocity.x < maxSpeed * speedMultiplier)
            {
                rb2d.AddForce(Vector2.right * dirH * moveForce);
            }

            if (!moveHability)
            {
                if (Mathf.Abs(rb2d.velocity.x) > maxSpeed * speedMultiplier)
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

        if (CanMoveV(dirV, moveHability))
        {
            if (dirV * rb2d.velocity.y < maxSpeed * speedMultiplier)
            {
                rb2d.AddForce(Vector2.up * dirV * moveForce);
            }

            if (!moveHability)
            {
                if (Mathf.Abs(rb2d.velocity.y) > maxSpeed * speedMultiplier)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Sign(rb2d.velocity.y) * maxSpeed * speedMultiplier);
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
        moveHability = true;
        speedMultiplier *= dashAcceleration;
        rb2d.velocity = 10f * rb2d.velocity;
        rb2d.AddForce((faceRight ? 1 : -1) * new Vector2(dirH, dirV), ForceMode2D.Impulse);
        isDashOnCoolDown = true;
        Vector2 x = playerTransform.position;
        yield return new WaitForSeconds(0.1f);
        Vector2 y = playerTransform.position;
        moveHability = false;
        speedMultiplier /= dashAcceleration;
        yield return new WaitForSeconds(dashCoolDown);
        isDashOnCoolDown = false;
        yield return null;
    }


}
