using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateCollider : MonoBehaviour {

	public Collider player;
	public static string isInGate = "0";
	public static string isTouchingWall = "0";

	void Update()
	{
		//Debug.Log (isTouchingWall);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Gate")
		{
			player.isTrigger = true;
		}
			
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Gate") 
		{
			isInGate = "1";
		} 
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "Gate")
		{
			isInGate = "0";
			player.isTrigger = false;
		}			
	}

	private void OnCollisionEnter(Collision other)
	{
		if ((player.bounds.Intersects (other.collider.bounds)) && (other.collider.isTrigger == false)) 
		{
			isTouchingWall = "1";
		}
	}
	/*
	private void OnCollisionStay(Collision other)
	{
		if ((other.collider.isTrigger == false)) 
		{
			isTouchingWall = "You Are Touching a Wall";
		}
	}
	*/
	private void OnCollisionExit(Collision other)
	{
		isTouchingWall = "0";
	}
		
}