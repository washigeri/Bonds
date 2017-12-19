using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public string pnjName;

    private bool isTalking = false;
    private int dialogIndex = Int32.MinValue;
    private bool isPlayerInRange = false;
    private bool isCoroutineRunning = false;
    private IEnumerator coroutine;

    private TextMesh textMesh;

    // Use this for initialization
    private void Awake()
    {
        textMesh = gameObject.GetComponentInChildren<TextMesh>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isPlayerInRange)
        {
            if (!isTalking && dialogIndex == Int32.MinValue)
            {
                DisplayInteractionText(true);
                if (Input.GetButtonDown("InteractP1"))
                {
                    dialogIndex = -1;
                    isTalking = true;
                }
            }
            if (!isCoroutineRunning)
            {
                coroutine = DoDialog(dialogIndex);
                StartCoroutine(coroutine);
            }
        }
        else
        {
            isTalking = false;
            DisplayInteractionText(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            isPlayerInRange = false;
            if (isTalking && dialogIndex != Int32.MaxValue)
            {
                dialogIndex = Int32.MinValue;
            }
        }
    }

    private void DisplayInteractionText(bool toggle)
    {
        if (toggle)
            textMesh.text = "Intéragir";
        else
            textMesh.text = String.Empty;
    }

    private IEnumerator DoDialog(int Index)
    {
        isCoroutineRunning = true;
        switch (Index)
        {
            case -1:
                WelcomeDialog();
                dialogIndex = 0;
                break;

            case 0:
                GiveChoice();
                dialogIndex = 1;
                break;

            case 1:
                SayGoodBye();
                dialogIndex = Int32.MaxValue;
                isTalking = false;
                break;
        }
        yield return new WaitForSeconds(3f);
        this.isCoroutineRunning = false;
    }

    private void WelcomeDialog()
    {
        textMesh.text = "Bonjour !";
    }

    private void GiveChoice()
    {
        textMesh.text = "Voici deux potions de soin\npour vous aider";
    }

    private void SayGoodBye()
    {
        textMesh.text = "Au revoir !";
        Instantiate(Resources.Load("Prefabs/UsableObjects/Potion"), gameObject.transform.position, Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/UsableObjects/Potion"), gameObject.transform.position, Quaternion.identity);
    }
}