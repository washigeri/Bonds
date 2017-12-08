using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour {

    public Slider slider;
    public int player; 

	private void Update () {
        //if (GameManager.gameManager.isGameInitialized)
        //{
            if (player == 1)
            {
                slider.value = GameManager.gameManager.player1.GetComponent<PlayerController>().GetHealth();
            }
            if (player == 2)
            {
                slider.value = GameManager.gameManager.player2.GetComponent<PlayerController>().GetHealth();
            }
        //}
        
    }
}
