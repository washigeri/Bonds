using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform camTransform;

    public GameObject player1;
    public GameObject player2;
    public float zOffset;
    public float cameraSpeed;

    [HideInInspector] public static double cameraWidth;
    [HideInInspector] public static double cameraHeight;
    [HideInInspector] public static bool isGrounded;
    [HideInInspector] public static bool isLanding;

    private double xMaxDist;
    private double yMaxDist;
    private float distToCenterP1X;
    private float distToCenterP2X;
    private float distToCenterP1Y;
    private float distToCenterP2Y;
    private bool isXAligned;
    private bool isYAligned;
    private float yOffset;

    // Use this for initialization
    void Start()
    {
        cameraSpeed = 5;
        Vector2 player1Pos = player1.transform.position;
        camTransform.position = new Vector3(player1Pos.x, player1Pos.y, zOffset);
        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        xMaxDist = cameraWidth / 2 - 0.5;
        yMaxDist = cameraHeight / 2 - 0.5;
        distToCenterP1X = player1Pos.x - camTransform.position.x;
        distToCenterP2X = player2.transform.position.x - camTransform.position.x;
        distToCenterP1Y = player1Pos.y - camTransform.position.y;
        distToCenterP2Y = player2.transform.position.y - camTransform.position.y;
    }

    private bool IsXLocked()
    {
        Vector2 player1Pos = player1.transform.position;
        Vector2 player2Pos = player2.transform.position;
        return Mathf.Abs(player2Pos.x - player1Pos.x) >= xMaxDist;
    }

    private bool IsYLocked()
    {
        Vector2 player1Pos = player1.transform.position;
        Vector2 player2Pos = player2.transform.position;
        return Mathf.Abs(player2Pos.y - player1Pos.y) >= yMaxDist;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newCameraPosition = camTransform.position;
        isXAligned = (Mathf.Abs(player1.transform.position.x - newCameraPosition.x) < 0.5f);
        isYAligned = (Mathf.Abs(player1.transform.position.y - newCameraPosition.y) < 0.5f);
        float distToCenterUpdatedP2X = player2.transform.position.x - camTransform.position.x;
        float distToCenterUpdatedP1X = player1.transform.position.x - camTransform.position.x;
        float distToCenterUpdatedP2Y = player2.transform.position.y - camTransform.position.y;
        float distToCenterUpdatedP1Y = player1.transform.position.y - camTransform.position.y;
        if (!IsXLocked())
        {

            if (isXAligned)
            {
                newCameraPosition.x = player1.transform.position.x;
            }
            else
            {
                if (OnSameSideOfCamera())
                {
                    newCameraPosition = Vector3.MoveTowards(newCameraPosition, new Vector3(player1.transform.position.x, newCameraPosition.y, zOffset), Time.deltaTime * cameraSpeed);
                }
                else
                {
                    if (Mathf.Abs(distToCenterUpdatedP2X) < Mathf.Abs(distToCenterP2X))
                    {
                        newCameraPosition = Vector3.MoveTowards(newCameraPosition, new Vector3(player1.transform.position.x, newCameraPosition.y, zOffset), Time.deltaTime * cameraSpeed);
                    }
                }
            }
        }
        else
        {
            if (Mathf.Abs(distToCenterUpdatedP2X) < Mathf.Abs(distToCenterP2X))
            {
                newCameraPosition = Vector3.MoveTowards(newCameraPosition, new Vector3(player1.transform.position.x, newCameraPosition.y, zOffset), Time.deltaTime * cameraSpeed);
            }
        }

        if (!IsYLocked())
        {
            if (isGrounded)
            {
                float playerY = player1.transform.position.y;
                if(playerY != yOffset)
                {
                    newCameraPosition = Vector3.MoveTowards(newCameraPosition, new Vector3(newCameraPosition.x, playerY, zOffset), Time.deltaTime * cameraSpeed);
                    yOffset = newCameraPosition.y;
                }
            }
            else if (isLanding && newCameraPosition.y > player1.transform.position.y)
            {
                newCameraPosition.y = player1.transform.position.y;
            }
        }
        else
        {
            if (Mathf.Abs(distToCenterUpdatedP2Y) < Mathf.Abs(distToCenterP2Y))
            {
                newCameraPosition = Vector3.MoveTowards(newCameraPosition, new Vector3(newCameraPosition.x, player1.transform.position.y, zOffset), Time.deltaTime * cameraSpeed);
                yOffset = newCameraPosition.y;
            }
        }

        distToCenterP2X = distToCenterUpdatedP2X;
        distToCenterP1X = distToCenterUpdatedP1X;
        distToCenterP2Y = distToCenterUpdatedP2Y;
        distToCenterP1Y = distToCenterUpdatedP1Y;

        camTransform.position = newCameraPosition;
    }

    private bool OnSameSideOfCamera()
    {
        float middle = Camera.main.transform.position.x;
        return ((player1.transform.position.x >= middle && player2.transform.position.x >= middle) || (player1.transform.position.x <= middle && player2.transform.position.x <= middle));
    }
}
