using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Note : MonoBehaviour {
    public Transform[] startPos;
    public Transform[] endPos;
    private float totalDistanceToDestination;
    private float startTime;


	// Use this for initialization
	void Start () {
        startTime = Time.deltaTime;

    }
	
	// Update is called once per frame
	void Update () {
        float currentDuration = Time.time - startTime;
        int i = 1;
        Transform startPosTarget = startPos[i];
        Transform endPosTarget = endPos[i];

        float totalDistanceToDestination = Vector3.Distance(startPosTarget.position, endPosTarget.position);
        float journeyFraction = currentDuration / totalDistanceToDestination;
        transform.position = Vector3.Lerp(startPosTarget.position, endPosTarget.position, journeyFraction);
    }
}
