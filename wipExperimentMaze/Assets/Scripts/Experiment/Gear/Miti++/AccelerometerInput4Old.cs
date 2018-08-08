using UnityEngine;
using System.Collections;
using UnityEngine.XR;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AccelerometerInput4Old : MonoBehaviour {
    private float yaw;
    private float rad;
    private float xVal;
    private float zVal;

	public static float velocity = 0f;
	public static float method1StartTimeGrow = 0f;
	public static float method1StartTimeDecay = 0f;
	public static bool wasOne = false; //phase one when above (+/-) 0.105 threshold
	public static bool wasTwo = true; //phase two when b/w -0.105 and 0.105 thresholds

	public static bool advanceState;

	// initial X and Y angles - used to determine if user is looking around
	private float eulerX;
	private float eulerZ;


	void Start () {
        //Enable the gyroscope on the phone
		Input.gyro.enabled = true;
        //If we are on the phone, then setup a client device to read transform data from
        if (Application.platform == RuntimePlatform.Android) SetupClient();

		// user must be looking ahead at the start
		eulerX = InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.x;
		eulerZ = InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.z;
	}

	void FixedUpdate() //was previously FixedUpdate()
	{
		string path = Application.persistentDataPath + "/WIP_GEAR_MITI.txt";

		// This text is always added, making the file longer over time if it is not deleted
		string appendText = "\n" + DateTime.Now.ToString() + ";" + 
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
			gateCollider.isTouchingWall + ";" + velocity;
		
		File.AppendAllText(path, appendText);

        //Do the movement algorithm, more details inside
		move ();
        //Send the current transform data to the server (should probably be wrapped in an if isAndroid but I haven't tested)
		if (myClient != null) {
			myClient.Send (MESSAGE_DATA, new TDMessage (this.transform.localPosition, Camera.main.transform.eulerAngles, advanceState));
			advanceState = false;
		}
	}

	bool look(double start, double curr, double diff)
	{
		//Determines if the user's current angle (curr) is within the window (start +/- diff)
		//Deals with wrap around values (eulerAngles is in range 0 to 360)
		if ((start + diff) > 360f)
		{
			if (((curr >= 0f) && (curr <= (start + diff - 360f))) || ((((start - diff) <= curr) && (curr <= 360f))))
			{
				return false;
			}
		}
		else if ((start - diff) < 0f)
		{
			if (((0f <= curr) && (curr <= (start + diff))) || (((start - diff + 360f) <= curr) && (curr <= 360f)))
			{
				return false;
			}
		}
		else if (((start + diff) <= curr) && (curr <= (start + diff)))
		{
			return false;
		}
		return true;
	}

	void move ()
	{
        //Get the yaw of the subject to allow for movement in the look direction
		yaw = UnityEngine.XR.InputTracking.GetLocalRotation (UnityEngine.XR.XRNode.Head).eulerAngles.y;
        //convert that value into radians because math uses radians
		rad = yaw * Mathf.Deg2Rad;
        //map that value onto the unit circle to faciliate movement in the look direction
		zVal = Mathf.Cos (rad);
		xVal = Mathf.Sin (rad);

		// check if person is looking around in X or Z directions
		bool looking = (look(eulerX, InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.x, 20f) || look(eulerZ, InputTracking.GetLocalRotation(XRNode.Head).eulerAngles.z, 20f));

		if (!looking) {
			
			//If the user is moving their head enough, but not looking up and down (as if they were nodding yes)
			//To be honest, some of this code is a mystery to me. I am commenting it much later than its creation date
			//    I think that the first section is when you are stepping and the second when you are not stepping
			//    The idea is that we have increasing exponential decay (1-e^(-t)) when stepping 
			//    and decreasing exponential decay (e^-t) when not stepping
			if ((Input.gyro.userAcceleration.y >= 0.075f || Input.gyro.userAcceleration.y <= -0.075f)) {
				if (wasTwo) { //we are transitioning from phase 2 to 1
					method1StartTimeGrow = Time.time;
					wasTwo = false;
					wasOne = true;
				}
			} else {
				if (wasOne) {
					method1StartTimeDecay = Time.time;
					wasOne = false;
					wasTwo = true;
				}
			}


			//Why we have the exact same conditions again is really unknown to me. But again, just as the above comment says
			if ((Input.gyro.userAcceleration.y >= 0.075f || Input.gyro.userAcceleration.y <= -0.075f)) { //0.08 is an arbitrary threshold

				velocity = 1.65f - (1.65f - velocity) * Mathf.Exp ((method1StartTimeGrow - Time.time) / 0.2f); //grow
			} else {

				velocity = 0f - (0f - velocity) * Mathf.Exp ((method1StartTimeDecay - Time.time) / 0.4f); //decay
			}
		} else {
			velocity = 0f;
		}

        //Multiply intended speed (called velocity) by delta time to get a distance, then multiply that distamce
        //    by the unit vector in the look direction to get displacement.

		//ADJUSTING THE SPEED
		transform.Translate (xVal * velocity * Time.fixedDeltaTime, 0, zVal * velocity * Time.fixedDeltaTime); 
		//transform.Translate (xVal * velocity * Time.fixedDeltaTime, 0, zVal * 3 * Time.fixedDeltaTime); 
	}

    #region NetworkingCode
    //Declare a client node
    NetworkClient myClient;
    //Define two types of data, one for setup (unused) and one for actual data
    const short MESSAGE_DATA = 880;
    const short MESSAGE_INFO = 881;
    //Server address is Flynn, tracker address is Baines, port is for broadcasting
    const string SERVER_ADDRESS = "192.168.11.11";
    const string TRACKER_ADDRESS = "192.168.1.100";
    const int SERVER_PORT = 5003;

    //Message and message text are now depreciated, were used for debugging
    public string message = "";
    public Text messageText;

    //Connection ID for the client server interaction
    public int _connectionID;
    //transform data that is being read from the clien
    public static Vector3 _pos = new Vector3();
    public static Vector3 _euler = new Vector3();

    // Create a client and connect to the server port
    public void SetupClient()
	{
        myClient = new NetworkClient(); //Instantiate the client
        myClient.RegisterHandler(MESSAGE_DATA, DataReceptionHandler); //Register a handler to handle incoming message data
        myClient.RegisterHandler(MsgType.Connect, OnConnected); //Register a handler to handle a connection to the server (will setup important info
        myClient.Connect(SERVER_ADDRESS, SERVER_PORT); //Attempt to connect, this will send a connect request which is good if the OnConnected fires
    }

    // client function to recognized a connection
    public void OnConnected(NetworkMessage netMsg)
    {
        _connectionID = netMsg.conn.connectionId; //Keep connection id, not really neccesary I don't think
    }

    // Clinet function that fires when a disconnect occurs (probably unnecessary
    public void OnDisconnected(NetworkMessage netMsg)
    {
        _connectionID = -1;
    }

    //I actually don't know for sure if this is useful. I believe that this is erroneously put here and was duplicated in TDServer code. 
    public void DataReceptionHandler(NetworkMessage _transformData)
    {
        TDMessage transformData = _transformData.ReadMessage<TDMessage>();
        _pos = transformData._pos;
        _euler = transformData._euler;
    }
    #endregion

}