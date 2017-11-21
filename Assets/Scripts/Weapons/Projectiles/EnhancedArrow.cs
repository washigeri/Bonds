using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancedArrow : MonoBehaviour
{

    public Transform myTransform;

    private bool isSet;

    private float damageMultiplier;
    private Vector3 direction;
    private Vector3 startPosition;

    private float time;
    private float distance;
    private float maxRange;
    private float moveSpeed;
    private Rigidbody2D rb2d;
    private bool isGrounded;

    private void Awake()
    {
        maxRange = 20f;
        moveSpeed = 20f;
        distance = 0f;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0f;
        isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSet)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                StartCoroutine(collision.gameObject.GetComponentInChildren<Ground>().ModifyGround(time, damageMultiplier, damageMultiplier));
                StartCoroutine(OnDestruction());
            }
        }
    }

    private IEnumerator OnDestruction()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (isSet)
        {
            distance = Mathf.Abs(Vector3.Distance(startPosition, myTransform.position));
            if (distance > maxRange)
            {
                if (!isGrounded)
                {
                    Destroy(gameObject);
                }
            }
            if (!isGrounded)
            {
                myTransform.position = myTransform.position + direction * moveSpeed * Time.deltaTime;
            }
        }

    }

    public void SetParameters(float damageMultiplier, float time, Vector3 direction, Vector3 startPosition)
    {
        this.damageMultiplier = damageMultiplier;
        this.direction = direction;
        this.time = time;
        if (direction == Vector3.up)
        {
            myTransform.Rotate(Vector3.forward * 90);
        }
        else
        {
            myTransform.Rotate(Vector3.forward * -90);
        }
        this.startPosition = startPosition;
        isSet = true;
    }
}
