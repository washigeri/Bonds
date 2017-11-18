using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class NPCController : MonoBehaviour {

    public string pnjName;

    private bool isTalking = false;
    private int dialogIndex = Int32.MinValue;
    private bool isPlayerInRange = false;
    private bool isCoroutineRunning = false;
    private bool waitingForChoice = false;
    IEnumerator coroutine;

    private TextMesh textMesh;

    // Use this for initialization
    void Awake () {
        textMesh = gameObject.GetComponentInChildren<TextMesh>();
		
	}

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange)
        {
            if (!isTalking && dialogIndex == Int32.MinValue)
            {
                DisplayInteractionText(true);
                if (Input.GetButtonDown("InteractP1") )
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
            else if(waitingForChoice)
            {
                if(dialogIndex == 1)
                {
                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        StopCoroutine(coroutine);
                        isCoroutineRunning = false;
                        Instantiate(Resources.Load("Prefabs/UsableObjects/Potion"), gameObject.transform.position, Quaternion.identity);
                        waitingForChoice = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.T))
                    {
                        StopCoroutine(coroutine);
                        isCoroutineRunning = false;
                        AddStat(18);
                        waitingForChoice = false;

                    }
                }
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
        if (collision.gameObject.CompareTag("Player1")){
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            isPlayerInRange = false;
            if(isTalking && dialogIndex != Int32.MaxValue)
            {
                dialogIndex = Int32.MinValue;
            }
        }
    }

    private void DisplayInteractionText(bool toggle)
    {
        if (toggle)
            textMesh.text = "Appuyez sur saut pour intéragir";
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
                waitingForChoice = true;
                yield return new WaitForSeconds(60f);
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
        textMesh.text = "Que voulez-vous ? \n\t- Potion\n\t- Point de caractéristique";
    }

    private void SayGoodBye()
    {
        textMesh.text = "Au revoir !";
    }

    private void AddStat(int value, GameObject player = null)
    {
        Debug.Log(String.Format("+ {0} stat", value));
    }
}
