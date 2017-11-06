using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potion : MonoBehaviour {

    [HideInInspector] public int hpRegen;
    private bool pickedUp = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GameManager.potionNumber < GameManager.maxPotion)
        {
            Debug.Log(collision.tag);
            if (collision.CompareTag("Player1"))
            {
                if (!pickedUp)
                {
                    GameManager.potionNumber++;
                    pickedUp = true;
                    Destroy(gameObject);
                }
            }
            
        }
    }

}
