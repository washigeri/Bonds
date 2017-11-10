using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [HideInInspector] public static GameObject player1;
    [HideInInspector] public static GameObject player2;
    [HideInInspector] public static int potionNumber;
    [HideInInspector] public static int maxPotion;
    [HideInInspector] public static float potionHeal;

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
        potionNumber = 0;
        maxPotion = 5;
        potionHeal = 0.5f;
        Physics2D.IgnoreLayerCollision(8, 9, true);
        Physics2D.IgnoreLayerCollision(9, 10, true);
        Physics2D.IgnoreLayerCollision(4, 12, false);
        Physics2D.IgnoreLayerCollision(4, 10, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (player1.GetComponent<Player1Controller>().GetHealth() <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (player2.GetComponent<Player2Controller>().GetHealth() <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
