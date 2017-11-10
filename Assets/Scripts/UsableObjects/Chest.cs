using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    private bool isOpen;
    private int weaponType;
    private string weaponName;

    // Use this for initialization
    void Awake()
    {
        isOpen = false;
        weaponType = Random.Range(0, 4);
        if(weaponType == 0)
        {
            weaponName = "Spear";
        }
        else if(weaponType == 1)
        {
            weaponName = "Sword";
        }
        else if(weaponType == 2)
        {
            weaponName = "Daggers";
        }
        else
        {
            weaponName = "Bow";
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isOpen)
        {
            if (Input.GetButtonDown("InteractP1") || Input.GetButtonDown("InteractP2"))
            {
                if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
                {
                    isOpen = true;
                    Instantiate(Resources.Load("Prefabs/Weapons/" + weaponName), collision.gameObject.transform.position, Quaternion.Euler(0, 0, -90));
                }
            }
        }
    }
}
