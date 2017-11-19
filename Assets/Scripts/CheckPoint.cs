using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    private bool used=false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!used)
        {
            used = true;
            Gamecontrol.control.PreSave();
        }
    }
}
