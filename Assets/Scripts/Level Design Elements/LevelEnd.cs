using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            FinishLevel();
        }
    }

    private void FinishLevel()
    {
        //GameManager.gameManager.CleanSceneOnChange();
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if(currentScene == GameManager.gameManager.GetBossSceneBuildIndex())
        {
            GameManager.gameManager.GoBackToMenu();
        }
        else
        {
            GameManager.gameManager.GoToNextLevel();
        }
    }
}
