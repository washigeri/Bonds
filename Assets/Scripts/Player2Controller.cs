using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : PlayerController
{

    public bool isDashing = false;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        rb2d.gravityScale = 0f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetButtonDown("Dash") && !isDashing)
        {
            isDashing = true;
        }
    }

    protected override void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float dirH = Input.GetAxisRaw("HorizontalP2");
        float dirV = Input.GetAxisRaw("VerticalP2");
        if (CanMoveH(dirH, isDashing))
        {
            if (dirH * rb2d.velocity.x < maxSpeed)
            {
                rb2d.AddForce(Vector2.right * dirH * moveForce);
            }

            if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            {
                rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
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

            if (Mathf.Abs(rb2d.velocity.y) > maxSpeed)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Sign(rb2d.velocity.y) * maxSpeed);
            }
        }
        else
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
    }


}
