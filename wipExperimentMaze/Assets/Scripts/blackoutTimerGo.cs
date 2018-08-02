using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackoutTimerGo : MonoBehaviour {

	public Camera main;
	public Camera blackout;

	private float velocity;
	private static int walkingState_waiting = 0;
	private static int walkingState_normal = 1;
	private static int walkingState_blackout = 2;
	private static int walkingState_waiting2 = 3;
	private static int walkingState_undoBlackout = 4;
	private int walkingState = walkingState_waiting;
	private float minuteTimer = 0;
	private float secondTimer = 0;

	private List<float> timeList = new List<float> ();

	// Use this for initialization
	void Start () 
	{
		velocity = AccelerometerInput4.velocity;
	}
	
	// Update is called once per frame
	void Update () 
	{
		velocity = AccelerometerInput4.velocity;
		OVRInput.Update ();
		walkingStateMachine ();
	}

	void walkingStateMachine()
	{
		if (walkingState == walkingState_waiting) {
			if (OVRInput.Get(OVRInput.Button.One)) {
				walkingState = walkingState_normal;
				Debug.Log ("normal");
				minuteTimer = Time.time;
			}
		} else if (walkingState == walkingState_normal) {
			if (minuteTimer + 60 < Time.time && !(OVRInput.Get(OVRInput.Button.One))) {
				walkingState = walkingState_blackout;
				Debug.Log ("blackout");
				secondTimer = Time.time;
			}
		} else if (walkingState == walkingState_blackout) {
			if (secondTimer + 1 < Time.time) {
				main.gameObject.SetActive (false);
				blackout.gameObject.SetActive (true);
				walkingState = walkingState_waiting2;
			}
		} else if (walkingState == walkingState_waiting2) {
			if (OVRInput.Get(OVRInput.Button.One)) {
				walkingState = walkingState_undoBlackout;
				main.gameObject.SetActive (true);
				blackout.gameObject.SetActive (false);
				secondTimer = Time.time;
			}
		} else if (walkingState == walkingState_undoBlackout) {
			if (secondTimer + 1 < Time.time) {
				walkingState = walkingState_waiting;
			}
		}
			
	}

}
