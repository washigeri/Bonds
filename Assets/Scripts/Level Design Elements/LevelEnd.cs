using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            FinishLevel();
        }
    }

    private void FinishLevel()
    {
        GameManager.gameManager.CleanSceneOnChange();
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        switch (currentScene)
        {
            case 1:
                SceneManager.LoadScene(2);
                GameManager.gameManager.player1.transform.position = new Vector3(-5f, -5f, 0f);
                GameManager.gameManager.player2.transform.position = new Vector3(-6f, -5f, 0f);
                Camera.main.GetComponent<CameraController>().TargetPlayer1();
                //GameManager.gameManager.currentScene = 2;
                break;
            case 2:
                SceneManager.LoadScene(3);
                break;
            default:
                SceneManager.LoadScene(0);
                break;
        }
    }
}
