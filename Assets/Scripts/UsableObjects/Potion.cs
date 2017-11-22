using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potion : MonoBehaviour
{
    
    private bool pickedUp = false;
    private bool isLanded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.gameManager.potionNumber < GameManager.gameManager.maxPotion)
        {
            if (collision.CompareTag("Player1"))
            {
                if (!pickedUp)
                {
                    GameManager.gameManager.potionNumber++;
                    pickedUp = true;
                    Destroy(gameObject);
                }
            }

        }

        if (!isLanded)
        {
            if (collision.CompareTag("Ground") || collision.CompareTag("Plateform"))
            {
                isLanded = true;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            }
        }


    }




}
