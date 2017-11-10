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
        range = 20f;
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

    public void SetParameters(int damage, string enemyTag, int direction, Vector3 startPosition)
    {
        this.damage = damage;
        this.enemyTag = enemyTag;
        if (direction == -1)
        {
            this.direction = Vector3.left;
            myTransform.localScale = new Vector3(-1, 1, 1);
        }
        else if(direction == 1)
        {
            this.direction = Vector3.right;
            myTransform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            this.direction = Vector3.up;
            myTransform.Rotate(Vector3.forward * 90);
        }
        this.startPosition = startPosition;
        isSet = true;
        Debug.Log("done");
    }

}
