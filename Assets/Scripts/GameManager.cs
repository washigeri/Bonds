using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Physics2D.IgnoreLayerCollision(8, 9, true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
