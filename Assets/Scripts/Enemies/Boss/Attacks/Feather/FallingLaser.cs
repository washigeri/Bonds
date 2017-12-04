using UnityEngine;
using System.Collections;

public class FallingLaser : MonoBehaviour
{
    private bool isSet;
    private LineRenderer laserLineRenderer;
    private float laserLength;
    private float laserAddedLength;
    private float laserWidth;
    private float laserMaxLength;
    private float timeToHitGround;
    private float laserSpeed;
    private BoxCollider2D bCollider2D;
    private bool isColliderSet;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 direction;

    private float timeBeforeDestruction;
    private bool isAboutToBeDestroyed;
    private bool canBeDestroyed;

    private void Awake()
    {
        isSet = false;
        laserWidth = 1.5f;
        laserLineRenderer = GetComponent<LineRenderer>();
        laserLineRenderer.startWidth = laserWidth;
        laserLineRenderer.endWidth = laserWidth;
        bCollider2D = null;
        laserMaxLength = (float)CameraController.cameraHeight * 1.1f;
        laserLength = 0f;
        laserAddedLength = 0f;
        timeToHitGround = 0.25f;
        laserSpeed = laserMaxLength / timeToHitGround;
        direction = Vector3.down;
        timeBeforeDestruction = 0.25f;
        isAboutToBeDestroyed = false;
        canBeDestroyed = false;
    }

    private void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition;
        Vector3[] initLaserPositions = new Vector3[2] { startPosition, endPosition };
        laserLineRenderer.SetPositions(initLaserPositions);
        AddColliderToLaser();
        isSet = true;
    }

    private void FixedUpdate()
    {
        if (isSet)
        {
            if (laserLength < laserMaxLength)
            {
                ShootLaserFromTheSky();
                if (!isColliderSet)
                {
                    SetCollider();
                }
                UpdateColliderPosition();
            }
            else
            {
                if (!isAboutToBeDestroyed)
                {
                    StartCoroutine(OnToBeDestroy());
                }
            }
        }
    }

    void Update()
    {
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

    void ShootLaserFromTheSky()
    {
        laserAddedLength = laserSpeed * Time.fixedDeltaTime;
        endPosition += (laserAddedLength * direction);
        laserLength += laserAddedLength;
        laserLineRenderer.SetPosition(1, endPosition);
    }

    private void AddColliderToLaser()
    {
        bCollider2D = gameObject.AddComponent<BoxCollider2D>();
        bCollider2D.isTrigger = true;
    }

    private void SetCollider()
    {
        bCollider2D.size = new Vector2(laserLength, laserWidth);
        bCollider2D.offset = new Vector2(0f, 0f);
        bCollider2D.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
        bCollider2D.transform.position = (startPosition + endPosition) / 2f;
        gameObject.AddComponent<LaserCollider>();
        isColliderSet = true;
    }

    private void UpdateColliderPosition()
    {
        bCollider2D.size = new Vector2(laserLength, laserWidth);
        bCollider2D.transform.position = (startPosition + endPosition) / 2f;
    }

}