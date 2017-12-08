using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingPlatform : MonoBehaviour
{

    public float fallDelay = 2f;


    private Rigidbody2D rb2d;
    private Vector2 startingPosition;

    private bool isFalling = false;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player1") && isFalling == false)
        {
            isFalling = true;
            StartCoroutine("PlatformFall");
        }
    }

    void Fall()
    {
        rb2d.isKinematic = false;
    }

    private IEnumerator PlatformFall()
    {
        yield return new WaitForSeconds(1);
        rb2d.isKinematic = false;
        yield return new WaitForSeconds(5);
        rb2d.velocity = new Vector2(0f, 0f);
        transform.position = startingPosition;
        rb2d.isKinematic = true;
        isFalling = false;
    }
}
