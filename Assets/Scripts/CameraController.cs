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

    private double xMaxDist;
    private double yMaxDist;
    private float distToCenterP1X;
    private float distToCenterP2X;
    private bool isAligned;

    // Use this for initialization
    void Start()
    {
        Vector2 player1Pos = player1.transform.position;
        camTransform.position = new Vector3(player1Pos.x, player1Pos.y, zOffset);
        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        xMaxDist = cameraWidth / 2 - 0.5;
        yMaxDist = cameraHeight / 2 - 0.5;
        distToCenterP1X = player1Pos.x - camTransform.position.x;
        distToCenterP2X = player2.transform.position.x - camTransform.position.x;
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
        isAligned = (Mathf.Abs(player1.transform.position.x - newCameraPosition.x) < 0.5);
        float distToCenterUpdatedP2X = player2.transform.position.x - camTransform.position.x;
        float distToCenterUpdatedP1X = player1.transform.position.x - camTransform.position.x;
        if (!IsXLocked())
        {

            if (isAligned)
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
                        newCameraPosition.x -= (distToCenterP2X - distToCenterUpdatedP2X);
                    }
                }
            }
        }
        else
        {
            if (Mathf.Abs(distToCenterUpdatedP2X) < Mathf.Abs(distToCenterP2X))
            {
                newCameraPosition.x -= (distToCenterP2X - distToCenterUpdatedP2X);
            }
        }

        distToCenterP2X = distToCenterUpdatedP2X;
        distToCenterP1X = distToCenterUpdatedP1X;
        //if (!IsYLocked())
        //{
        //    newCameraPosition.y = player1.transform.position.y;
        //}
        camTransform.position = newCameraPosition;
    }

    private bool OnSameSideOfCamera()
    {
        float middle = Camera.main.transform.position.x;
        return ((player1.transform.position.x >= middle && player2.transform.position.x >= middle) || (player1.transform.position.x <= middle && player2.transform.position.x <= middle));
    }
}
