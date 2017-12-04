using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform camTransform;

    public float zOffset;
    public float yOffset;
    public float cameraSpeed;

    [HideInInspector] public static double cameraWidth;
    [HideInInspector] public static double cameraHeight;
    [HideInInspector] public static bool isGrounded;
    [HideInInspector] public static bool isLanding;

    private double xMaxDist;
    private double yMaxDist;
    private float distToCenterP2X;
    private float distToCenterP2Y;
    private bool isXAligned;
    private float yCamera;

    private bool isCameraReadyForGame;
    private bool isCameraSetForBoss;

    public void SetCameraForBoss()
    {
        cameraSpeed = 0;
        yOffset = 0;
        camTransform.position = new Vector3(0, 0, zOffset);
        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        xMaxDist = cameraWidth / 2 - 0.5;
        yMaxDist = cameraHeight / 2 - 0.5;
        distToCenterP2X = GameManager.gameManager.player2.transform.position.x - camTransform.position.x;
        distToCenterP2Y = GameManager.gameManager.player2.transform.position.y - camTransform.position.y;
        isCameraSetForBoss = true;
    }

    public void SetCameraForGame()
    {
        cameraSpeed = 5;
        Vector2 player1Pos = GameManager.gameManager.player1.transform.position;
        camTransform.position = new Vector3(player1Pos.x, player1Pos.y + yOffset, zOffset);
        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        xMaxDist = cameraWidth / 2 - 0.5;
        yMaxDist = cameraHeight / 2 - 0.5;
        distToCenterP2X = GameManager.gameManager.player2.transform.position.x - camTransform.position.x;
        distToCenterP2Y = GameManager.gameManager.player2.transform.position.y - camTransform.position.y;
    }

    private void SetCameraForMenu()
    {
        isCameraReadyForGame = false;
    }

    void Start()
    {
        SetCameraForMenu();
    }

    private bool IsXLocked()
    {
        Vector2 player1Pos = GameManager.gameManager.player1.transform.position;
        Vector2 player2Pos = GameManager.gameManager.player2.transform.position;
        return Mathf.Abs(player2Pos.x - player1Pos.x) >= xMaxDist;
    }

    private bool IsYLocked()
    {
        Vector2 player1Pos = GameManager.gameManager.player1.transform.position;
        Vector2 player2Pos = GameManager.gameManager.player2.transform.position;
        return Mathf.Abs(player2Pos.y - player1Pos.y) >= yMaxDist;
    }

    private void UpdateForLevel()
    {
        if ((GameManager.gameManager.startedGame > -1) && GameManager.gameManager.isGameInitialized)
        {
            Vector3 newCameraPosition = camTransform.position;
            isXAligned = (Mathf.Abs(GameManager.gameManager.player1.transform.position.x - newCameraPosition.x) < 0.5f);
            float distToCenterUpdatedP2X = GameManager.gameManager.player2.transform.position.x - camTransform.position.x;
            float distToCenterUpdatedP2Y = GameManager.gameManager.player2.transform.position.y - camTransform.position.y;
            if (!IsXLocked())
            {

                if (isXAligned)
                {
                    newCameraPosition.x = GameManager.gameManager.player1.transform.position.x;
                }
                else
                {
                    if (OnSameSideOfCamera())
                    {
                        newCameraPosition = Vector3.MoveTowards(newCameraPosition, new Vector3(GameManager.gameManager.player1.transform.position.x, newCameraPosition.y + yOffset, zOffset), Time.deltaTime * cameraSpeed);
                    }
                    else
                    {
                        if (Mathf.Abs(distToCenterUpdatedP2X) < Mathf.Abs(distToCenterP2X))
                        {
                            newCameraPosition = Vector3.MoveTowards(newCameraPosition, new Vector3(GameManager.gameManager.player1.transform.position.x, newCameraPosition.y + yOffset, zOffset), Time.deltaTime * cameraSpeed);
                        }
                    }
                }
            }
            else
            {
                if (Mathf.Abs(distToCenterUpdatedP2X) < Mathf.Abs(distToCenterP2X))
                {
                    newCameraPosition = Vector3.MoveTowards(newCameraPosition, new Vector3(GameManager.gameManager.player1.transform.position.x, newCameraPosition.y + yOffset, zOffset), Time.deltaTime * cameraSpeed);
                }
            }

            if (!IsYLocked())
            {
                if (isGrounded)
                {
                    float playerY = GameManager.gameManager.player1.transform.position.y;
                    if (playerY != yCamera - yOffset)
                    {
                        newCameraPosition = Vector3.MoveTowards(newCameraPosition, new Vector3(newCameraPosition.x, playerY + yOffset, zOffset), Time.deltaTime * cameraSpeed);
                        yCamera = newCameraPosition.y;
                    }
                }
                else if (isLanding && newCameraPosition.y > GameManager.gameManager.player1.transform.position.y + yOffset)
                {
                    newCameraPosition.y = GameManager.gameManager.player1.transform.position.y + yOffset;
                }
            }
            else
            {
                if (Mathf.Abs(distToCenterUpdatedP2Y) < Mathf.Abs(distToCenterP2Y))
                {
                    newCameraPosition = Vector3.MoveTowards(newCameraPosition, new Vector3(newCameraPosition.x, GameManager.gameManager.player1.transform.position.y + yOffset, zOffset), Time.deltaTime * cameraSpeed);
                    yCamera = newCameraPosition.y;
                }
            }

            distToCenterP2X = distToCenterUpdatedP2X;
            distToCenterP2Y = distToCenterUpdatedP2Y;
            camTransform.position = newCameraPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCameraSetForBoss)
        {
            UpdateForLevel();
        }
    }

    private bool OnSameSideOfCamera()
    {
        float middle = Camera.main.transform.position.x;
        return ((GameManager.gameManager.player1.transform.position.x >= middle && GameManager.gameManager.player2.transform.position.x >= middle) || (GameManager.gameManager.player1.transform.position.x <= middle && GameManager.gameManager.player2.transform.position.x <= middle));
    }

    public void TargetPlayer1()
    {
        camTransform.position = new Vector3(GameManager.gameManager.player1.transform.position.x, GameManager.gameManager.player1.transform.position.y + yOffset, zOffset);
    }
}
