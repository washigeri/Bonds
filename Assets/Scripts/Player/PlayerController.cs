using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerController : MonoBehaviour
{

    [HideInInspector] public bool faceRight = true;
    [HideInInspector] public bool isDead = false;
    protected bool isPlayer1;

    protected float moveForce;
    public float maxSpeed = 5f;
    public Transform playerTransform;

    private int maxHp;
    protected int hp;
    protected int agility;
    protected int strengh;
    protected int stamina;

    protected float dirH;
    protected float dirV;

    protected string potionBindName;
    protected string interactBindName;

    public Rigidbody2D rb2d;
    public Slider PlayerHealth;

    // Use this for initialization
    protected virtual void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        maxHp = 100;
        hp = maxHp;
        agility = 1;
        strengh = 1;
        stamina = 1;
        moveForce = 365f;
    }

    protected virtual void Update()
    {
        isDead = (hp <= 0);
        CheckForInputs();
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

    protected void UsePotion()
    {
        if (GameManager.potionNumber > 0)
        {
            RestaureHealth((int)(GameManager.potionHeal * maxHp));
            GameManager.potionNumber--;
        }
    }

    protected void CheckForInputs()
    {
        if (Input.GetButtonDown(potionBindName))
        {
            UsePotion();
        }
    }

    public void DropWeapon()
    {
        WeaponController myWeapon = gameObject.GetComponentInChildren<WeaponController>();
        if(myWeapon != null)
        {
            myWeapon.gameObject.GetComponent<WeaponController>().SetOwner(false);
            myWeapon.gameObject.transform.parent = null;
            myWeapon.transform.localScale = new Vector3(Mathf.Abs(myWeapon.transform.localScale.x), Mathf.Abs(myWeapon.transform.localScale.y), Mathf.Abs(myWeapon.transform.localScale.z));
            myWeapon.transform.localRotation = new Quaternion(myWeapon.transform.localRotation.x, myWeapon.transform.localRotation.y, myWeapon.transform.localRotation.z * (faceRight ? 1 : -1), myWeapon.transform.localRotation.w);
        }
    }

    private void RestaureHealth(int health)
    {
        hp = Mathf.Min(hp + health, maxHp);
        PlayerHealth.value = hp;
    }

    public void RemoveHealth(int health)
    {
        hp -= health;
        PlayerHealth.value = hp;

    }

    public int GetHealth()
    {
        return hp;
    }

    public float GetDirH()
    {
        return dirH;
    }

    public float GetDirV()
    {
        return dirV;
    }

    protected void Flip()
    {
        faceRight = !faceRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
