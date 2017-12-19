using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTextEventHandler : MonoBehaviour {

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            GameManager.gameManager.GoToNextLevel();
        }
    }
}
