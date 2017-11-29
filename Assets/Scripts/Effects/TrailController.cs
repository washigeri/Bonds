using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour {

    private TrailRenderer trailRenderer;
    private Vector3 lastLocalPosition;
    private Vector3 lastLocalRotation;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        trailRenderer.enabled = (lastLocalPosition != transform.localPosition || lastLocalRotation != transform.localEulerAngles);
        lastLocalPosition = transform.localPosition;
        lastLocalRotation = transform.localEulerAngles;
    }
}
