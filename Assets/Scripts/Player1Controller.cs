using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player1Controller : PlayerController
{

    [HideInInspector] public bool jump = false;

    public float jumpForce = 650f;
    public Transform groundCheck;

    public bool grounded = false;


    protected override void Awake()
    {
        dirV = 0f;
        base.Awake();
    }

    //Update is called once per frame
    protected override void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));


        CameraController.isGrounded = grounded;
        CameraController.isLanding = (rb2d.velocity.y < 0f);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
    }

    protected override void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        dirH = Input.GetAxisRaw("HorizontalP1");
        if (CanMoveH(dirH, !grounded))
        {
            if (grounded && Input.GetButtonUp("HorizontalP1"))
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
            else
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
        }
        else
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
        if (jump)
        {
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
        if (!grounded)
        {
            bool isFalling;
            if ((isFalling = rb2d.velocity.y < 0f))
            {
                if (!CanMoveV(-1f, true))
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
                }
            }
            else
            {
                if (!CanMoveV(1f, true))
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
                }
            }
        }
    }


}
