using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [HideInInspector] public static GameObject player1;
    [HideInInspector] public static GameObject player2;

    public static GameManager gameManager;

    // Use this for initialization
    void Awake()
    {
        if (gameManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        Physics2D.IgnoreLayerCollision(8, 9, true);
        Physics2D.IgnoreLayerCollision(9, 10, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (player1.GetComponent<Player1Controller>().getHealth() <= 0)
        {
            Debug.Log("You lost, Player 1 died.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (player2.GetComponent<Player2Controller>().getHealth() <= 0)
        {
            Debug.Log("You Lost, Player 2 died again");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
