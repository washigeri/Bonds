using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTextEventHandler : MonoBehaviour {

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            GameManager.gameManager.GoBackToMenu();
        }
    }

}
