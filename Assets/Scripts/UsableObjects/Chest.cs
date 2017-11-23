using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    private bool isOpen;
    private string weaponName;
    private string trinketName; 

    // Use this for initialization
    void Awake()
    {
        isOpen = false;
        int dice = Random.Range(0, 4);
        if(dice == 0)
        {
            weaponName = "Spear";
        }
        else if(dice == 1)
        {
            weaponName = "Sword";
        }
        else if(dice == 2)
        {
            weaponName = "Daggers";
        }
        else
        {
            weaponName = "Bow";
        }
        dice = Random.Range(1, 5);
        trinketName = "Trinket" + dice;
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
                    float dice = Random.Range(0f, 1f);
                    if(dice <= 0.4f)
                    {
                        Instantiate(Resources.Load("Prefabs/Weapons/" + weaponName), transform.position + new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f), Quaternion.Euler(0, 0, -90));
                    }
                    else if(dice >= 0.6f)
                    {
                        Instantiate(Resources.Load("Prefabs/Weapons/Trinkets/" + trinketName), transform.position + new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f), Quaternion.Euler(0, 0, 0));
                    }
                    else
                    {
                        Instantiate(Resources.Load("Prefabs/Weapons/" + weaponName), transform.position + new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f), Quaternion.Euler(0, 0, -90));
                        Instantiate(Resources.Load("Prefabs/Weapons/Trinkets/" + trinketName), transform.position + new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f), Quaternion.Euler(0, 0, 0));
                    }
                }
            }
        }
    }
}
