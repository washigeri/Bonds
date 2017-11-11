using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public Transform myTransform;

    private bool isSet;

    private int damage;
    private string enemyTag;
    private Vector3 direction;
    private Vector3 startPosition;
    private float distance;

    private float range;
    private float moveSpeed;
    private Rigidbody2D rb2d;


    private void Awake()
    {
        isSet = false;
        range = 10f;
        moveSpeed = 10f;
        distance = 0f;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSet)
        {
            if (collision.gameObject.CompareTag(enemyTag))
            {
                collision.gameObject.GetComponent<EnemyController>().RemoveHealth(damage);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("collision stay " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Plateform"))
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (isSet)
        {
            distance = Mathf.Abs(Vector3.Distance(startPosition, myTransform.position));
            if (distance > range)
            {
                rb2d.gravityScale = Mathf.Min(1f, rb2d.gravityScale + Time.deltaTime / 2f);
            }
            myTransform.position = myTransform.position + direction * moveSpeed * Time.deltaTime;
        }
        
    }

    public void SetParameters(int damage, string enemyTag, Vector3 direction, Vector3 startPosition)
    {
        this.damage = damage;
        this.enemyTag = enemyTag;
        this.direction = direction;
        if (direction == Vector3.left)
        {
            myTransform.localScale = new Vector3(-1, 1, 1);
        }
        else if(direction == Vector3.left + Vector3.up)
        {
            myTransform.Rotate(Vector3.forward * 135);
        }
        else if(direction == Vector3.up)
        {
            myTransform.Rotate(Vector3.forward * 90);
        }
        else if (direction == Vector3.right + Vector3.up)
        {
            myTransform.Rotate(Vector3.forward * 45);
        }
        else if(direction == Vector3.right)
        {
            myTransform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction == Vector3.down + Vector3.right)
        {
            myTransform.Rotate(Vector3.forward * -45);
        }
        else if(direction == Vector3.down)
        {
            myTransform.Rotate(Vector3.forward * -90);
        }
        else if(direction == Vector3.down + Vector3.left)
        {
            myTransform.Rotate(Vector3.forward * -135);
        }
        this.startPosition = startPosition;
        isSet = true;
    }

}
