using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    private bool used=false;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player1"))
        if (!used)
        {
            used = true;
            GameManager.gameManager.Save();
        }
    }
}
