using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {

    void OnGUI()
    {
        if(GUI.Button(new Rect(10, 100, 100, 30),"Load")){
            GameManager2.gameManager.Load();
        }
    }
}
