using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GameObject player1;
    private GameObject player2;

	// Use this for initialization
	void Awake () {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
    }
	
	// Update is called once per frame
	void Update () {
        if (player1.GetComponent<Player1Controller>().getHealth() <= 0)
        {
            Debug.Log("You lost, Player 1 died.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if(player2.GetComponent<Player2Controller>().getHealth() <= 0)
        {
            Debug.Log("You Lost, Player 2 died again");
        }
	}

}
