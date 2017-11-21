using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chrono : MonoBehaviour {

    Text text;
    float startTimer;

    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
        startTimer = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        var time = Time.time - startTimer;
        var seconds = time % 60;
        var minutes = time / 60;
        text.text = System.String.Format("{0:00}:{1:00}", minutes, seconds);

    }
}
