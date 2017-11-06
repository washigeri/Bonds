using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{

    [HideInInspector] public bool faceRight = true;
    [HideInInspector] public bool isDead = false;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public Transform playerTransform;

    protected int hp;
    protected int agility;
    protected int strengh;
    protected int stamina;

    protected float dirH;
    protected float dirV;

    public Rigidbody2D rb2d;

    // Use this for initialization
    protected virtual void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        hp = 10;
        agility = 1;
        strengh = 1;
        stamina = 1;
    }

    protected virtual void Update()
    {
        isDead = (hp <= 0);
    }

    protected abstract void FixedUpdate();

    protected float GetAxisRaw(string axis, float dir)
    {

        if (Input.GetButtonDown(axis))
        {
            if (dir == 0f)
            {
                return 0f;
            }
            else if (dir < 0)
            {
                return -1f;
            }
            else
            {
                return 1f;
            }
        }
        else
        {
            return 0f;
        }
    }

    protected virtual bool CanMoveH(float dirH, bool isJumping)
    {
        float xDiff = playerTransform.position.x - Camera.main.transform.position.x;
        if (Mathf.Abs(xDiff) <= 0.01)
        {
            return true;
        }
        else
        {
            if (dirH < 0)
            {
                if (xDiff < 0)
                {
                    return (CameraController.cameraWidth / 2 + xDiff > 1);
                }
                else
                {
                    return true;
                }
            }
            else if (dirH > 0)
            {
                if (xDiff > 0)
                {
                    return (CameraController.cameraWidth / 2 - xDiff > 1);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (isJumping)
                {
                    return (CameraController.cameraWidth / 2 - Mathf.Abs(xDiff) > 1);
                }
                else
                {
                    return false;
                }
            }
        }
    }

    protected bool CanMoveV(float dirV, bool isJumping)
    {
        float yDiff = playerTransform.position.y - Camera.main.transform.position.y;
        if (Mathf.Abs(yDiff) <= 0.01)
        {
            return true;
        }
        else
        {
            if (dirV < 0)
            {
                if (yDiff < 0)
                {
                    return (CameraController.cameraHeight / 2 + yDiff > 1);
                }
                else
                {
                    return true;
                }
            }
            else if (dirV > 0)
            {
                if (yDiff > 0)
                {
                    return (CameraController.cameraHeight / 2 - yDiff > 1);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (isJumping)
                {
                    return (CameraController.cameraHeight / 2 - Mathf.Abs(yDiff) > 1);
                }
                else
                {
                    return false;
                }
            }
        }
    }

    public void RemoveHealth(int health)
    {
        hp -= health;
    }

    public int getHealth()
    {
        return hp;
    }

    protected void Flip()
    {
        faceRight = !faceRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
