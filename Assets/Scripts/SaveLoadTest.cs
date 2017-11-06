using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadTest : MonoBehaviour {

	void OnGUI() {
        if (GUI.Button(new Rect(10, 100, 100, 30), "Save"))
        {
            Gamecontrol.control.PreSave();
        }
        if (GUI.Button(new Rect(10, 140, 100, 30), "Load"))
        {
            Gamecontrol.control.Load();
        }
    }
}
