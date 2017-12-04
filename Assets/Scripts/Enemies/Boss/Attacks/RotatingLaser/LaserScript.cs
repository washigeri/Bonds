using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour
{
    public LayerMask layerMask;

    private BossController boss;
    private bool isSet;
    private LineRenderer laserLineRenderer;
    private float laserWidth;
    private float laserMaxLength;
    private float laserLength;
    private BoxCollider2D bCollider2D;
    private bool isColliderSet;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 currentDirection;
    private int turnRight;

    private float fullRotation;
    private float rotationLeft;
    private float rotationDuration;
    private float rotationSpeed;

    private void Awake()
    {
        isSet = false;
        laserWidth = 1f;
        laserMaxLength = 20f;
        laserLength = 0f;
        laserLineRenderer = GetComponent<LineRenderer>();
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        laserLineRenderer.startWidth = laserWidth;
        laserLineRenderer.endWidth = laserWidth;
        bCollider2D = null;
        currentDirection = new Vector3(-0.25f, 1f, 0f);
        turnRight = -1;

        fullRotation = 165f;
        rotationLeft = fullRotation;
        rotationDuration = 5f;
        rotationSpeed = fullRotation / rotationDuration;

    }


    void Update()
    {
        if (isSet)
        {
            if (rotationLeft > 0f)
            {
                float angle = rotationSpeed * Time.deltaTime;
                ShootLaserToTargetPosition(Quaternion.Euler(0f, 0f, turnRight * angle) * currentDirection);
                rotationLeft -= angle;
                if(!isColliderSet)
                {
                    SetCollider();
                }
                UpdateColliderPosition(angle * turnRight);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void ShootLaserToTargetPosition(Vector3 direction)
    {
        direction = direction.normalized;
        startPosition = boss.transform.position;
        Ray ray = new Ray(startPosition, direction);
        RaycastHit2D hit = Physics2D.Raycast(boss.transform.position, direction, laserMaxLength, ~layerMask);
        if(hit.collider != null)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                endPosition = hit.point;
                laserLength = hit.distance;
            }
            else
            {
                endPosition = startPosition + (laserMaxLength * direction);
                laserLength = laserMaxLength;
            }
        }
        else
        {
            endPosition = startPosition + (laserMaxLength * direction);
            laserLength = laserMaxLength;
        }
        laserLineRenderer.SetPosition(0, startPosition);
        laserLineRenderer.SetPosition(1, endPosition);
        currentDirection = direction;
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
        float angle = (Mathf.Abs(startPosition.y - endPosition.y) / Mathf.Abs(startPosition.x - endPosition.x));
        if ((startPosition.y < endPosition.y && startPosition.x > endPosition.x) || (endPosition.y < startPosition.y && endPosition.x > startPosition.x))
        {
            angle *= -1;
        }
        angle = Mathf.Rad2Deg * Mathf.Atan(angle);
        bCollider2D.transform.Rotate(Vector3.forward, angle);
        bCollider2D.transform.position = (startPosition + endPosition) / 2f;
        gameObject.AddComponent<LaserCollider>();
        isColliderSet = true;
    }

    private void UpdateColliderPosition(float angle)
    {
        bCollider2D.size = new Vector2(laserLength, 1f);
        bCollider2D.transform.Rotate(Vector3.forward, angle);
        bCollider2D.transform.position = (startPosition + endPosition) / 2f;
    }

    public void SetLaser(BossController boss, int direction)
    {
        this.boss = boss;
        startPosition = boss.transform.position;
        turnRight = direction;
        currentDirection = new Vector3(currentDirection.x * direction, currentDirection.y, currentDirection.z);
        AddColliderToLaser();
        isSet = true;
    }

    private void OnDestroy()
    {
        boss.UpdateRaysLeft(-1);
    }
}