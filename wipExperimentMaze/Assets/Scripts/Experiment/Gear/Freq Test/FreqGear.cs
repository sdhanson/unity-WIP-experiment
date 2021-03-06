﻿using UnityEngine;
using System.Collections;
using UnityEngine.XR;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FreqGear : MonoBehaviour
{
	// set per person - NEED TO GET HIGH AND LOW THRESHOLDS 
	private float height = GlobalVariables.height;
	private float ht = GlobalVariables.gearHT;
	private float lt = GlobalVariables.gearLT;

	// used to determine direction to walk
	private float yaw;
	private float rad;
	private float xVal;
	private float zVal;

	// determine if person is picking up speed or slowing down
	public static float velocity = 0f;
	public static float method1StartTimeGrow = 0f;
	public static float method1StartTimeDecay = 0f;
	public static bool wasOne = false;
	//phase one when above (+/-) 0.10 threshold
	public static bool wasTwo = true;
	//phase two when b/w -0.10 and 0.10 thresholds

	//Initial X and Y angles (used to determine if user is looking around)
	private float eulerX;
	private float eulerZ;

	private bool walking = false;
	private float iteration = 0f;

	//Queue to keep track of past X userAcceleration.Y values
	private Queue<float> accelY;
	private float sumY = 0f;
	private float thresholdAccelY = 0.008f;

	//Queue to keep track of diff between X pairs of current and previous
	//userAcceleration.Y values
	private Queue<float> changeY;
	private float sumChangeY = 0f;
	private float thresholdChangeY = 0.008f;
	private float prev = 0f;

	private float decayRate = 0.2f;
	private float velocityMax = 0.0f;

	// variables for determining the step frequency
	// time of the last step
	private float prevTime = 0f;
	// time between recently detected step and last step
	private float stepTime = 0f;
	// y value when the peak of the current step occurred
	private float maxy = -100f;
	// time the peak of the current step occurred + 0.25 to create the time window
	private float maxt = -1f;
	// alert indicates we MIGHT BE currently stepping and should be on the look out for second peak
	private bool alert = false;
	// low = true means we hit the lower peak threshold
	private bool low = false;
	// high = true means we hit the lower peak threshold
	private bool high = false;
	// unset = true means the velocityMax has not yet been set for the detected step
	private bool unset = true;
	// if user hasn't stepped in a while, firstStep is set to true so there is no lag in the starting step
	private bool firstStep = true;

	// variable for debugging to see if we are counting the right number of steps
	private float stepCount = 0f;

	private float test = 0f;


	void Start ()
	{
		//Enable the gyroscope on the phone
		Input.gyro.enabled = true;
		//If we are on the phone, then setup a client device to read transform data from
		if (Application.platform == RuntimePlatform.Android)
			SetupClient ();

		//User must be looking ahead at the start
		eulerX = InputTracking.GetLocalRotation (XRNode.Head).eulerAngles.x;
		eulerZ = InputTracking.GetLocalRotation (XRNode.Head).eulerAngles.z;

		//Initialize the queues
		accelY = new Queue<float> ();
		changeY = new Queue<float> ();

		string path = Application.persistentDataPath + "/inGearFreq.txt";

		File.Delete (path);
	}

	void FixedUpdate () //was previously FixedUpdate()
	{
		string path = Application.persistentDataPath + "/inGearFreq.txt";

		// This text is always added, making the file longer over time if it is not deleted
		string appendText =
			Time.time + ";" + 

			Input.GetMouseButton(0) + ";" +

			Input.gyro.userAcceleration.x + ";" + 
			Input.gyro.userAcceleration.y + ";" + 
			Input.gyro.userAcceleration.z + ";" + 

			gameObject.transform.position.x + ";" + 
			gameObject.transform.position.y + ";" + 
			gameObject.transform.position.z + ";" +

			UnityEngine.XR.InputTracking.GetLocalRotation (UnityEngine.XR.XRNode.Head).eulerAngles.x + ";" +
			UnityEngine.XR.InputTracking.GetLocalRotation (UnityEngine.XR.XRNode.Head).eulerAngles.y + ";" +
			UnityEngine.XR.InputTracking.GetLocalRotation (UnityEngine.XR.XRNode.Head).eulerAngles.z + ";" +

			gateCollider.isInGate + ";" + 
			gateCollider.isTouchingWall + ";" + test  + "\r\n";

		File.AppendAllText (path, appendText);

		//Determine if the user is walking, more details inside
		manageWalking ();
		//Do the movement algorithm, more details inside
		move ();
		//Send the current transform data to the server (should probably be wrapped in an if isAndroid but I haven't tested)
		if (myClient != null)
			myClient.Send (MESSAGE_DATA, new TDMessage (this.transform.localPosition, Camera.main.transform.eulerAngles, false));
	}

	// sets the velocity max given the step frequency from frequency()
	void setMax ()
	{
		// if this is the first step, prevTime can't be used or very low velocityMax, so set to 1.0f
		// if not the first step, use equation to set velocityMax
		if (!firstStep)
		{
			// get freq
			stepTime = Time.time - prevTime;
			float frequency = 1.0f / stepTime;

			test = frequency;
			// set velocity max with biomedical equation
			velocityMax = Mathf.Pow (((frequency / 1.57f) * (height / 1.72f)), 2);
		}
		else
		{
			velocityMax = 0.75f;
			firstStep = false;
		}
		// set time of last step to current time
		prevTime = Time.time;
	}

	// checks to see if we are currently stepping and then calls setMax() to calculate velocity from discovered step frequency
	void frequency ()
	{
		// if we aren't in the shadow of a previous step (aka if we aren't a secondary peak)
		if (!alert && (Time.time > maxt))
		{
			// if we aren't on alert (aka if we are currently just in noise territory and haven't hit some peak recently)
			// checking to see if the signal is beyond the allowed window - INDIVIDUALIZED BOUNDARIES
			if ((Input.gyro.userAcceleration.y < lt) || (Input.gyro.userAcceleration.y > ht))
			{
				alert = true;
				// distingiush if the signal was high or low
				if (Input.gyro.userAcceleration.y < lt)
				{
					low = true;
					high = false;
				}
				else
				{
					low = false;
					high = true;
				}
				// set the max to be the initial value
				maxy = Input.gyro.userAcceleration.y;
				maxt = Time.time + 0.25f;
			}
		}
		else if (alert && (Time.time < maxt))
		{
			// if we are in the alert zone and hit the outside of the other threshold,
			// then this is a valid peak, call the set max function to determine new max velocity
			if (unset && ((high && (Input.gyro.userAcceleration.y < lt)) || (low && (Input.gyro.userAcceleration.y > ht))))
			{
				stepCount++;
				unset = false;
				maxt = Time.time + 0.25f;
				setMax ();
			}
		}
		else if (alert && (Time.time >= maxt))
		{
			// if we have left the max time zone, then reset necessary variables to false
			maxy = -100;
			alert = false;
			low = false;
			high = false;
			unset = true;
		}
	}

	//Algorithm to determine if the user is looking around. Looking and walking generate similar gyro.accelerations, so we
	//want to ignore movements that could be spawned from looking around. Makes sure user's head orientation is in certain window
	bool look (double start, double curr, double diff)
	{
		//Determines if the user's current angle (curr) is within the window (start +/- diff)
		//Deals with wrap around values (eulerAngles is in range 0 to 360)
		if ((start + diff) > 360f) {
			if (((curr >= 0f) && (curr <= (start + diff - 360f))) || ((((start - diff) <= curr) && (curr <= 360f)))) {
				return false;
			}
		} else if ((start - diff) < 0f) {
			if (((0f <= curr) && (curr <= (start + diff))) || (((start - diff + 360f) <= curr) && (curr <= 360f))) {
				return false;
			}
		} else if (((start + diff) <= curr) && (curr <= (start + diff))) {
			return false;
		}
		return true;
	}

	//Determines if user has met conditions for STARTING to walk (enough acceleration in Y and Z and not looking around)
	//More lenient than usual conditions because we want the user to start moving IMMEDIATELY when they start walking
	bool inWindow ()
	{
		bool moving = false;
		double xDif = 20f;
		double zDif = 15f;

		//Checks if the user is moving enough to be considered walking. Thresholds determined by analyzing typical walking averages.
		if ((Input.gyro.userAcceleration.y >= 0.045f || Input.gyro.userAcceleration.y <= -0.045f)
			&& ((Input.gyro.userAcceleration.z < 0.08f) && (Input.gyro.userAcceleration.z > -0.08f))) {
			moving = true;
		}
		//Checks that the user is not looking around
		if (moving && !look (eulerX, InputTracking.GetLocalRotation (XRNode.Head).eulerAngles.x, xDif)
			&& !look (eulerZ, InputTracking.GetLocalRotation (XRNode.Head).eulerAngles.z, zDif)) {
			return true;
		}
		return false;
	}

	//If the user is walking, moves them in correct direction with varying velocities
	//Also sets velocity to 0 if it is determined that the user is no longer walking
	void move ()
	{
		//Get the yaw of the subject to allow for movement in the look direction
		yaw = InputTracking.GetLocalRotation (XRNode.Head).eulerAngles.y;
		//convert that value into radians because math uses radians
		rad = yaw * Mathf.Deg2Rad;
		//map that value onto the unit circle to faciliate movement in the look direction
		zVal = Mathf.Cos (rad);
		xVal = Mathf.Sin (rad);

		// set velocity max by using biomedical equation with freq
		frequency ();

	}

	//Determines if the user is walking by looking at the average value of userAcceleration.Y and the average change in
	//userAcceleration.Y over the past ~10 time steps
	void manageWalking ()
	{

		//iteration is 0 when the user is stopped. iteration is 10 when the queue is full.
		if (iteration < 9f) {
			//We only start keeping track of values when the user is determined to be walking (inWindow)
			//and then we fill the queue with data for the next 9 time steps
			if (inWindow () || (iteration > 0f)) {
				//Filling the changeY queue (can't add on first round bc don't have prev value)
				if (iteration != 0f) {
					float change = prev - Math.Abs (Input.gyro.userAcceleration.y);
					changeY.Enqueue (Math.Abs (change));
					sumChangeY += Math.Abs (change);
				}
				//Filling the accelY queue
				walking = true;
				accelY.Enqueue (Math.Abs (Input.gyro.userAcceleration.y));
				iteration++;
				sumY += Math.Abs (Input.gyro.userAcceleration.y);
			}
			//Setting prev
			prev = Math.Abs (Input.gyro.userAcceleration.y);
		} else {
			//Adding current value to changeY queue
			float change = prev - Math.Abs (Input.gyro.userAcceleration.y);
			changeY.Enqueue (Math.Abs (change));
			sumChangeY += Math.Abs (change);

			//Adding current value to accelY queue
			accelY.Enqueue (Math.Abs (Input.gyro.userAcceleration.y));
			sumY += Math.Abs (Input.gyro.userAcceleration.y);

			//If the average over the past ten values for accelY or changeY is below the threshold or the user
			//is looking around, the user is not walking anymore. Reset everything and walking=false (so velocity is set to 0)
			//If we are walking, need to keep queue at 10 values, so we remove the oldest value. || ((sumChangeY / 9) < thresholdChangeY) || ((sumY / 10) > 0.8)
			if (((sumY / 10) < thresholdAccelY) || ((sumChangeY / 9) < thresholdChangeY) || look (eulerX, InputTracking.GetLocalRotation (XRNode.Head).eulerAngles.x, 15f) || look (eulerZ, InputTracking.GetLocalRotation (XRNode.Head).eulerAngles.z, 10f)) {
				walking = false;
				accelY.Clear ();
				changeY.Clear ();
				iteration = 0f;
			} else {
				//Removing from changeY
				float temp2 = changeY.Peek ();
				changeY.Dequeue ();
				sumChangeY -= temp2;
				//Removing from accelY
				float temp = accelY.Peek ();
				accelY.Dequeue ();
				sumY -= temp;
			}
		} 

	}

	#region NetworkingCode

	//Declare a client node
	NetworkClient myClient;
	//Define two types of data, one for setup (unused) and one for actual data
	const short MESSAGE_DATA = 880;
	const short MESSAGE_INFO = 881;
	//Server address is Flynn, tracker address is Baines, port is for broadcasting
	const string SERVER_ADDRESS = "192.168.1.2";
	const string TRACKER_ADDRESS = "192.168.1.100";
	const int SERVER_PORT = 5000;

	//Message and message text are now depreciated, were used for debugging
	public string message = "";
	public Text messageText;

	//Connection ID for the client server interaction
	public int _connectionID;
	//transform data that is being read from the clien
	public static Vector3 _pos = new Vector3 ();
	public static Vector3 _euler = new Vector3 ();

	// Create a client and connect to the server port
	public void SetupClient ()
	{
		myClient = new NetworkClient (); //Instantiate the client
		myClient.RegisterHandler (MESSAGE_DATA, DataReceptionHandler); //Register a handler to handle incoming message data
		myClient.RegisterHandler (MsgType.Connect, OnConnected); //Register a handler to handle a connection to the server (will setup important info
		myClient.Connect (SERVER_ADDRESS, SERVER_PORT); //Attempt to connect, this will send a connect request which is good if the OnConnected fires
	}

	// client function to recognized a connection
	public void OnConnected (NetworkMessage netMsg)
	{
		_connectionID = netMsg.conn.connectionId; //Keep connection id, not really neccesary I don't think
	}

	// Clinet function that fires when a disconnect occurs (probably unnecessary
	public void OnDisconnected (NetworkMessage netMsg)
	{
		_connectionID = -1;
	}

	//I actually don't know for sure if this is useful. I believe that this is erroneously put here and was duplicated in TDServer code.
	public void DataReceptionHandler (NetworkMessage _transformData)
	{
		TDMessage transformData = _transformData.ReadMessage<TDMessage> ();
		_pos = transformData._pos;
		_euler = transformData._euler;
	}

	#endregion

}
