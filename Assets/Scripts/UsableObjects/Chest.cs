using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    public bool isOpen;
    public int weaponType;

    // Use this for initialization
    void Awake()
    {
        isOpen = false;
        weaponType = Random.Range(0, 4);
    }

    // Update is called once per frame
    void Update()
    {

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
                    Instantiate(Resources.Load("Prefabs/Weapons/Spear"), collision.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                }
            }
        }
    }
}
