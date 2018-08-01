using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackoutTimer : MonoBehaviour {

	public Camera main;
	public Camera blackout;

	private float velocity;
	private bool walkingState_normal;
	private bool walkingState_blackout;
	private bool walkingState_undoBlackout;
	private List<float> timeList = new List<float> ();

	// Use this for initialization
	void Start () 
	{
		walkingState_normal = true;
		walkingState_blackout = false;
		velocity = AccelerometerInput4.velocity;
	}
	
	// Update is called once per frame
	void Update () 
	{
		walkingState ();
	}

	void walkingState()
	{
		if((walkingState_normal) && (velocity != 0f))
		{
			timeList.Add (Time.time);

			if(velocity == 0f)
			{
				timeList.Add(Time.time);
			}

			if((timeList[timeList.Count - 1] - timeList[0]) >= 60f && gateCollider.isInGate == "1")
			{
				System.Threading.Thread.Sleep (1000);
				walkingState_normal = false;
				walkingState_blackout = true;
			}
			return;
		}

		if(walkingState_blackout)
		{
			main.enabled = false;
			blackout.enabled = true;

			if(Input.GetMouseButton(0)) //Don't know whether this will need to be a 1 or a 0 ***Just did true for now***
			{
				main.enabled = true;
				blackout.enabled = false;
				walkingState_blackout = false;
				walkingState_normal = true;
				timeList.Clear ();
			}
			return;
		}
	}


	void walkingStateTest()
	{
		if(walkingState_normal)
		{
			timeList.Add (Time.time);

			Debug.Log (timeList[timeList.Count - 1] - timeList[0]);

			if(velocity == 0f)
			{
				timeList.Add(Time.time);
			}

			if((timeList[timeList.Count - 1] - timeList[0]) >= 16f && gateCollider.isInGate == "1")
			{
				System.Threading.Thread.Sleep (1000);
				walkingState_normal = false;
				walkingState_blackout = true;
			}
			return;
		}

		else if(walkingState_blackout)
		{
			main.enabled = false;
			blackout.enabled = true;

			if(Input.GetKey("up")) //Don't know whether this will need to be a 1 or a 0 ***Just did true for now***
			{
				main.enabled = true;
				blackout.enabled = false;
				walkingState_blackout = false;
				walkingState_normal = true;
				timeList.Clear ();
			}
			return;
		}
	}

}
