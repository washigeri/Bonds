using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {

    private void Awake()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }


}
