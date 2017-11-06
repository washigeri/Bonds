using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{

    public bool wasSpoken = false;
    public bool pointGiven = false;
    public bool waitingForChoice = false;

    public TextMesh textMesh;

    // Use this for initialization
    void Start()
    {
        textMesh = GetComponentInChildren<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButton("Interact") && !wasSpoken)
        {
            wasSpoken = true;
            waitingForChoice = true;
            textMesh.text = "Which one do you want ? \n S -> [WeakAttack] \n A -> [Jump] \n C -> [StrongAttack]";
            // change text to display choices
        }
        if (waitingForChoice)
        {
            // switch case on choice
            if (Input.GetButton("Jump"))
            {
                pointGiven = true;
                waitingForChoice = false;
                textMesh.text = "Farewell !";
            }
        }

    }
}
