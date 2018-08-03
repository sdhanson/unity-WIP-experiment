using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MazeGen : MonoBehaviour {

	public int sceneNum;

	float[][] angles = new float[][] { 
		new float[] {-75,-63,54,67,82,-45,-63,-70,50,88,-70,48,55,-57,-47,56,-53,59,-65,-85,61,74,77,-76,74,70,-61,80,-82,63,-60,85,-56,-60,59,-61,87,-57,-50,81,-78,70,-49,-64,-46,48,67,-83,65,-46,-64,72,-68,-58,60,-63,71,88,54,-59,80,-80,76,-51,72,-77,57,-67,64,-75}, 

		new float[] {-89,88,-87,74,-87,86,81,50,-89,-85,63,-81,58,-52,-76,58,-72,64,62,-59,52,69,50,-54,-79,49,49,-54,-71,65,-54,-64,51,58,86,-89,66,-60,69,53,-57,-87,65,-56,68,-78,67,-67,58,-55,74,88,-55,-49,46,-63,-70,-61,74,49,-83,62,-82,-58,70,-62,80,-74,60,50},

		new float[] {-68,67,81,-87,-72,68,-79,72,-56,82,-70,73,79,-59,-71,55,72,-84,74,-46,-63,-51,53,87,-45,-66,78,-87,74,-67,52,89,-65,-75,84,-89,78,-82,72,52,-70,86,-75,-58,79,-67,83,-67,82,-51,74,-66,68,-63,-83,48,89,-59,69,-58,47,-60,-52,77,-58,64,-83,55,63,-74}, 

		new float[] {-81,55,-51,-50,71,-82,89,-63,71,68,63,-65,45,66,-50,-87,-62,77,-58,-63,79,53,-80,55,86,46,-68,85,-73,64,-78,50,-52,-74,67,57,-72,-81,50,65,-47,74,-64,-47,-79,80,48,56,-55,89,-77,-55,-57,60,70,-75,-59,-67,84,-78,76,-81,85,-70,81,65,-50,52,-67,-73},

		new float[] {-54,66,55,77,-55,-85,73,69,-53,-49,75,-64,53,-56,-73,84,70,-66,-76,-49,55,74,-79,47,52,-50,-65,72,66,-48,-57,51,51,-82,82,-47,64,-81,88,-49,-69,86,-51,55,-45,-60,73,56,-55,51,-87,71,-72,-66,76,68,-51,62,-79,69,-61,58,-83,-55,-69,62,-89,75,-63,48},

		new float[] {-83,69,47,-74,45,54,-50,-68,83,-45,-54,79,81,-72,55,-64,58,-47,55,-53,-50,45,62,-58,-88,57,-51,81,-69,50,68,-72,-70,88,62,-59,-77,54,69,-53,-52,77,-61,66,-68,65,-74,-45,49,88,-50,85,-77,-45,85,-48,83,-67,51,-75,-75,52,49,-48,45,-77,75,45,-70,-66} 
	};
	
	float[][] gateRatios = new float[][] { new float[] {.25f, .5f, .75f, .25f, .5f, .5f, .5f, .5f, .5f, .4f, .3f, 1f, .2f, .35f, .6f, .75f, .8f, .6f, 1f, .9f, .35f, .45f, .2f, .35f, 1f, 1f, .35f, .45f, .1f, .6f, .85f, .2f, .45f, .6f, .5f, .2f, .1f, .8f, .15f, .3f, .6f, .5f, .4f, .6f, .25f, .65f, .45f, .25f, .9f, 1f, 1f, .25f, .15f, .45f, .85f, .75f, .7f, .4f, .5f, .6f, .15f, .85f, .45f, .5f, .4f, .6f, .5f, .8f, .4f, 1f}, new float[] {0.1f, 0.6f, 0.7f, 0.5f, 0.1f, 0.9f, 0.8f, 0.2f, 0.7f, 0.7f, 0.8f, 0.5f, 0.4f, 0.3f, 0.4f, 0.2f, 0.0f, 0.6f, 0.6f, 0.4f, 0.7f, 0.2f, 0.1f, 0.7f, 0.4f, 0.3f, 0.2f, 0.0f, 0.1f, 0.4f, 0.4f, 0.7f, 0.2f, 1.0f, 0.0f, 0.5f, 0.3f, 0.9f, 0.6f, 0.7f, 0.5f, 0.2f, 0.3f, 0.7f, 0.6f, 0.6f, 0.1f, 0.3f, 0.7f, 0.6f, 0.3f, 0.7f, 0.2f, 0.5f, 0.4f, 0.2f, 0.9f, 0.3f, 0.6f, 0.9f, 0.4f, 0.6f, 0.4f, 0.7f, 0.8f, 0.3f, 0.5f, 0.4f, 0.5f, 1.0f}, new float[] {0.1f, 0.0f, 0.9f, 0.4f, 0.5f, 0.1f, 0.3f, 0.5f, 0.0f, 0.2f, 0.4f, 0.7f, 0.8f, 0.8f, 0.2f, 0.6f, 0.3f, 1.0f, 0.5f, 0.7f, 0.4f, 0.9f, 0.5f, 0.5f, 0.1f, 0.5f, 0.4f, 0.6f, 0.5f, 0.2f, 0.6f, 0.3f, 1.0f, 0.7f, 0.3f, 0.2f, 0.0f, 0.8f, 0.3f, 0.6f, 0.0f, 0.9f, 0.4f, 0.6f, 0.9f, 0.6f, 0.5f, 0.5f, 0.1f, 0.3f, 0.3f, 0.1f, 0.7f, 0.1f, 0.2f, 0.5f, 0.1f, 0.0f, 0.5f, 0.6f, 0.3f, 0.4f, 1.0f, 0.5f, 0.5f, 0.9f, 0.4f, 0.4f, 0.5f, 0.5f}, new float[] {0.4f, 0.5f, 0.1f, 0.2f, 0.5f, 0.7f, 0.6f, 0.4f, 0.6f, 0.7f, 0.2f, 0.3f, 0.6f, 0.0f, 0.3f, 0.1f, 0.1f, 0.2f, 0.4f, 0.8f, 0.6f, 0.1f, 0.8f, 0.6f, 0.7f, 0.4f, 0.1f, 0.7f, 0.5f, 0.4f, 0.3f, 0.7f, 0.4f, 0.0f, 0.2f, 0.9f, 0.5f, 0.1f, 0.7f, 0.5f, 0.4f, 0.9f, 0.4f, 0.0f, 0.8f, 0.1f, 0.3f, 0.8f, 0.3f, 0.5f, 0.5f, 0.7f, 0.9f, 0.5f, 0.6f, 1.0f, 1.0f, 0.1f, 0.8f, 0.5f, 0.3f, 0.2f, 0.4f, 0.9f, 0.8f, 0.1f, 0.2f, 0.6f, 0.7f, 0.9f}, new float[] {0.9f, 0.7f, 0.2f, 0.5f, 0.3f, 0.1f, 0.9f, 0.2f, 0.2f, 0.1f, 1.0f, 0.2f, 0.4f, 0.7f, 0.3f, 0.7f, 1.0f, 0.4f, 0.0f, 0.7f, 0.3f, 0.2f, 0.5f, 0.9f, 0.1f, 0.5f, 0.3f, 0.4f, 0.9f, 0.6f, 0.6f, 0.6f, 0.5f, 0.8f, 0.3f, 0.5f, 0.8f, 0.9f, 0.6f, 0.4f, 0.9f, 0.2f, 0.1f, 0.7f, 0.7f, 0.5f, 0.7f, 0.3f, 0.0f, 0.8f, 0.3f, 0.7f, 0.6f, 0.5f, 0.7f, 0.1f, 0.5f, 0.5f, 0.8f, 0.1f, 0.3f, 0.6f, 0.6f, 0.4f, 0.7f, 0.6f, 0.4f, 0.6f, 0.1f, 0.1f}, new float[] {1.0f, 0.7f, 0.8f, 0.7f, 0.2f, 0.4f, 0.9f, 0.5f, 1.0f, 0.4f, 0.5f, 0.4f, 0.5f, 0.4f, 1.0f, 0.4f, 0.4f, 0.1f, 0.6f, 0.2f, 0.6f, 0.2f, 0.5f, 0.6f, 0.4f, 0.6f, 0.4f, 0.8f, 0.3f, 0.6f, 0.5f, 0.8f, 0.0f, 0.8f, 0.4f, 0.6f, 0.8f, 0.5f, 1.0f, 0.7f, 0.5f, 0.5f, 0.5f, 0.6f, 0.3f, 0.4f, 0.6f, 0.2f, 0.9f, 0.4f, 0.8f, 0.1f, 0.6f, 0.6f, 0.9f, 0.1f, 0.3f, 0.1f, 0.9f, 0.1f, 0.7f, 0.9f, 0.4f, 0.2f, 0.5f, 0.8f, 0.9f, 0.2f, 0.1f, 0.5f} };
	public GameObject gateStart;
	public GameObject gateContPrefabL;
	public GameObject gateContPrefabR;
	public GameObject wallLeftStart;
	public GameObject wallRightStart;
	public GameObject wallLeftEnd;
	public GameObject wallRightEnd;
	private int SceneSelected;

	// Use this for initialization
	void Start () {
		SceneSelected = sceneNum;
		float totalAngle = 0;
		Transform bigWallPrev = gateStart.transform.GetChild (2);
		Transform smallWallPrev = gateStart.transform.GetChild (1);
		//bigWallPrev.localScale = new Vector3 (10f, 1.8f, .4f);
		for (int i = 0; i < angles[SceneSelected].Length; i++) {
			if (angles [SceneSelected][i] < 0) {
				//If the turn is left instantiate a left connected prefab
				GameObject gateCont = Instantiate (gateContPrefabL, this.transform, true);
				GameObject wall1 = Instantiate (wallRightStart, this.transform, true);
				wall1.transform.rotation = Quaternion.Euler (0, totalAngle, 0);
				//wall1.transform.localScale = new Vector3 (4 * Mathf.Abs(Mathf.Sin (Mathf.Deg2Rad * angles [i] / 2)), 1.8f, .4f);
				wall1.transform.position = bigWallPrev.GetChild (0).position;
				//rotate the prefab based on the angle given
				totalAngle += angles [SceneSelected][i];
				gateCont.transform.rotation = Quaternion.Euler (0, totalAngle, 0);
				//change the outside wall
				Transform bigWall = gateCont.transform.GetChild (0).GetChild (1);
				//Get a reference to the connector object, this is the child of one of shorter of the previous walls
				Transform connector = smallWallPrev.transform.GetChild (0);
				gateCont.transform.position = connector.position;
				//create walls (2 out side extended walls)
				GameObject wall2 = Instantiate (wallRightEnd, this.transform, true);
				wall2.transform.rotation = Quaternion.Euler (0, totalAngle, 0);
				wall2.transform.position = bigWall.GetChild (1).position;

				//fix the gates
				gateCont.transform.GetChild(0).GetChild(2).Translate(-gateRatios[SceneSelected][i] * 3, 0, 0);

				//prepare for next iteration
				if (i != angles[SceneSelected].Length - 1) {
					if (angles[SceneSelected][i + 1] < 0) {
						bigWallPrev = bigWall;
						smallWallPrev = gateCont.transform.GetChild (0).GetChild (0);
					} else {
						smallWallPrev = bigWall;
						bigWallPrev = gateCont.transform.GetChild (0).GetChild (0);
					}
				}

			} else {
				//If the turn is left instantiate a left connected prefab
				GameObject gateCont = Instantiate (gateContPrefabR, this.transform, true);
				GameObject wall1 = Instantiate (wallLeftStart, this.transform, true);
				wall1.transform.rotation = Quaternion.Euler (0, totalAngle, 0);
				wall1.transform.position = bigWallPrev.GetChild (0).position;
				//rotate the prefab based on the angle given
				totalAngle += angles [SceneSelected][i];
				gateCont.transform.rotation = Quaternion.Euler (0, totalAngle, 0);
				//change the outside wall
				Transform bigWall = gateCont.transform.GetChild (0).GetChild (0);
				//Get a reference to the connector object, this is the child of one of shorter of the previous walls
				Transform connector = smallWallPrev.transform.GetChild (0);
				gateCont.transform.position = connector.position;
				//create walls (2 out side extended walls)
				GameObject wall2 = Instantiate (wallLeftEnd, this.transform, true);
				wall2.transform.rotation = Quaternion.Euler (0, totalAngle, 0);
				wall2.transform.position = bigWall.GetChild (1).position;

				//fix the gates
				gateCont.transform.GetChild(0).GetChild(2).Translate(-gateRatios[SceneSelected][i] * 3, 0, 0);

				//prepare for next iteration
				if (i != angles[SceneSelected].Length - 1) {
					if (angles [SceneSelected][i + 1] > 0) {
						bigWallPrev = bigWall;
						smallWallPrev = gateCont.transform.GetChild (0).GetChild (1);
					} else {
						smallWallPrev = bigWall;
						bigWallPrev = gateCont.transform.GetChild (0).GetChild (1);
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
