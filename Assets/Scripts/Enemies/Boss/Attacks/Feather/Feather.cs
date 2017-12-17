using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{

    //private bool isSet;
    private bool isGrounded;
    private Vector3 groundCheck;
    private float timeBeforeExplosion;
    private bool isAboutToExplode;
    private bool hasExploded;
    private float timeBeforeDestruction;
    private bool isAboutToBeDestroyed;
    private bool canBeDestroyed;
    private float roomMaxY;
    private LayerMask layerMask;

    private void Awake()
    {
        //isSet = false;
        isGrounded = false;
        groundCheck = Vector3.down;
        timeBeforeExplosion = 0.5f;
        isAboutToExplode = false;
        hasExploded = false;
        isAboutToBeDestroyed = false;
        canBeDestroyed = false;
        roomMaxY = (float)CameraController.cameraHeight / 2f;
        layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Plateform"));
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!hasExploded)
        {
            isGrounded = Physics2D.Linecast(transform.position, transform.position + groundCheck, layerMask);
            if (isGrounded)
            {
                if (!isAboutToExplode)
                {
                    StartCoroutine(OnExplode());
                }
            }
        }
    }

    private void Update()
    {
        if (hasExploded)
        {
            if (!isAboutToBeDestroyed)
            {
                Instantiate(Resources.Load("Prefabs/Enemies/Boss/Attacks/FallingLaser"), new Vector3(transform.position.x, roomMaxY, 0f), Quaternion.Euler(0f, 0f, 0f));
                StartCoroutine(OnToBeDestroy());
            }
        }
        if (canBeDestroyed)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator OnToBeDestroy()
    {
        isAboutToBeDestroyed = true;
        yield return new WaitForSeconds(timeBeforeDestruction);
        canBeDestroyed = true;
    }

    private IEnumerator OnExplode()
    {
        isAboutToExplode = true;
        yield return new WaitForSeconds(timeBeforeExplosion);
        hasExploded = true;
    }

    //public void SetFeather(float roomMaxY)
    //{
    //    this.roomMaxY = roomMaxY;
    //}
}
