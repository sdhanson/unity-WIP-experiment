using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MazeGen : MonoBehaviour {

	float[][] angles = new float[][] { new float[] {-45, -89, 45, 89, 30, -50, 50, -15, 20, 30, 30, -10, -25, -20, 20, -15, 30, 60, -50, 10, -15, 20, -45, 30, 40, -20, -60, 50, 35, -15, 20, -45, 30, -20, 30, -20, 10, 45, -70, 20, 15, -60, 20, 30, -20, -15, 35, -60, -40, 20, 20, 25, -25, 30, 30, -45, -90, 10, 10, 10, -15, 60, -50, 20, 20, 20, -15, -15, -15, 80}, new float[] {-30, 20, -50, 30, 20, 15, -50, 20, -60, 20, 30, -20, -50, 30, 60, -25, -25, 10, -60, 30, 30, -15, -25, 40, 10, 10, 10, -35, -15, 15, -20, -20, 15, 40, 15, -60, 25, 25, 30, -90, 25, 45, -70, 20, -60, 20, 20, 15, 10, 10, -45, -45, 10, 20, 15, 30, 40, -60, -20, -15, 10, 10, 20, 15, 10, -30, -20, 15, 15, 15}, new float[] {
			-30, -20, 15, 10, 20, -20, 60, -10, -10, -10, 20, 15, 40, -30, -20, 10, 20, 30, -15, -15, 20, 20, -60, 25, -45, 20, 30, 10, -25, -25, 20, 30, -20, -15, 20 ,15, 60, -20, -15, 30, -70, 10, 10, 30, -25, -60, 20, 10, 15, 30, -50, -20, 10, 15, 20, -30, 15, 10, -20, 10, -20, -25, 30, 15, 20, -20, -20, -15, 10, 30}, new float[] {-25, -20, 20, -15, 30, 60, -50, 10, -15, 20, -45, 30, 40, -20, -60, 50, -20, 10, 20, 30, -60, 50, -20, 30, -10, 15, 15, -20, 30, 20, -60, 85, -45, 20, 30, 10, -25, -25, 20, 30, -20, -15, 20, 30, 30, -15, -25, 40, 10, 10, 10, -35, -15, 15, -20, -20, 15, 40, 15, -60, 20, -20, 15, 10, 30, -45, 20, 10, -50, 15}, new float[] {-45, 89, 30, -50, 50, -15, 20, 30, 30, -10, -25, -20, 20, 50, -20, 30, -10, 15, 15, -20, 30, 20, -60, 85, -45, 20, 30, 10, -25, 40, 10, 10, 10, -35, -15, 15, -20, -20, 15, 40, 15, -60, 10, -15, 20, 30, 10, 10, 10, 10, -35, -15, 15, -20, -20, 15, 40, 15, -60, 20, -20, 15, 10, 30, -45, 20, 10, -50, 15, -20}, new float[] {-60, -20, 15, 10, 20, -20, 60, -10, -10, -10, 20, 15, 40, -30, -20, 10, 20, 30, -60, 30, -60, 50, -20, 30, -10, 15, 15, -20, 30, 20, -60, 85, -45, 20, 30, 10, -25, -70, 20, -60, 20, 20, 15, 10, 10, -45, -45, 10, 20, 15, 30, 40, -60, -20, -15, 10, 10, -20, -15, 35, 40, -25, 20, -60, 20, 50, 30, -25, 10, 5} };
	float[][] gateRatios = new float[][] { new float[] {.25f, .5f, .75f, .25f, .5f, .5f, .5f, .5f, .5f, .4f, .3f, 1f, .2f, .35f, .6f, .75f, .8f, .6f, 1f, .9f, .35f, .45f, .2f, .35f, 1f, 1f, .35f, .45f, .1f, .6f, .85f, .2f, .45f, .6f, .5f, .2f, .1f, .8f, .15f, .3f, .6f, .5f, .4f, .6f, .25f, .65f, .45f, .25f, .9f, 1f, 1f, .25f, .15f, .45f, .85f, .75f, .7f, .4f, .5f, .6f, .15f, .85f, .45f, .5f, .4f, .6f, .5f, .8f, .4f, 1f}, new float[] {0.1f, 0.6f, 0.7f, 0.5f, 0.1f, 0.9f, 0.8f, 0.2f, 0.7f, 0.7f, 0.8f, 0.5f, 0.4f, 0.3f, 0.4f, 0.2f, 0.0f, 0.6f, 0.6f, 0.4f, 0.7f, 0.2f, 0.1f, 0.7f, 0.4f, 0.3f, 0.2f, 0.0f, 0.1f, 0.4f, 0.4f, 0.7f, 0.2f, 1.0f, 0.0f, 0.5f, 0.3f, 0.9f, 0.6f, 0.7f, 0.5f, 0.2f, 0.3f, 0.7f, 0.6f, 0.6f, 0.1f, 0.3f, 0.7f, 0.6f, 0.3f, 0.7f, 0.2f, 0.5f, 0.4f, 0.2f, 0.9f, 0.3f, 0.6f, 0.9f, 0.4f, 0.6f, 0.4f, 0.7f, 0.8f, 0.3f, 0.5f, 0.4f, 0.5f, 1.0f}, new float[] {0.1f, 0.0f, 0.9f, 0.4f, 0.5f, 0.1f, 0.3f, 0.5f, 0.0f, 0.2f, 0.4f, 0.7f, 0.8f, 0.8f, 0.2f, 0.6f, 0.3f, 1.0f, 0.5f, 0.7f, 0.4f, 0.9f, 0.5f, 0.5f, 0.1f, 0.5f, 0.4f, 0.6f, 0.5f, 0.2f, 0.6f, 0.3f, 1.0f, 0.7f, 0.3f, 0.2f, 0.0f, 0.8f, 0.3f, 0.6f, 0.0f, 0.9f, 0.4f, 0.6f, 0.9f, 0.6f, 0.5f, 0.5f, 0.1f, 0.3f, 0.3f, 0.1f, 0.7f, 0.1f, 0.2f, 0.5f, 0.1f, 0.0f, 0.5f, 0.6f, 0.3f, 0.4f, 1.0f, 0.5f, 0.5f, 0.9f, 0.4f, 0.4f, 0.5f, 0.5f}, new float[] {0.4f, 0.5f, 0.1f, 0.2f, 0.5f, 0.7f, 0.6f, 0.4f, 0.6f, 0.7f, 0.2f, 0.3f, 0.6f, 0.0f, 0.3f, 0.1f, 0.1f, 0.2f, 0.4f, 0.8f, 0.6f, 0.1f, 0.8f, 0.6f, 0.7f, 0.4f, 0.1f, 0.7f, 0.5f, 0.4f, 0.3f, 0.7f, 0.4f, 0.0f, 0.2f, 0.9f, 0.5f, 0.1f, 0.7f, 0.5f, 0.4f, 0.9f, 0.4f, 0.0f, 0.8f, 0.1f, 0.3f, 0.8f, 0.3f, 0.5f, 0.5f, 0.7f, 0.9f, 0.5f, 0.6f, 1.0f, 1.0f, 0.1f, 0.8f, 0.5f, 0.3f, 0.2f, 0.4f, 0.9f, 0.8f, 0.1f, 0.2f, 0.6f, 0.7f, 0.9f}, new float[] {0.9f, 0.7f, 0.2f, 0.5f, 0.3f, 0.1f, 0.9f, 0.2f, 0.2f, 0.1f, 1.0f, 0.2f, 0.4f, 0.7f, 0.3f, 0.7f, 1.0f, 0.4f, 0.0f, 0.7f, 0.3f, 0.2f, 0.5f, 0.9f, 0.1f, 0.5f, 0.3f, 0.4f, 0.9f, 0.6f, 0.6f, 0.6f, 0.5f, 0.8f, 0.3f, 0.5f, 0.8f, 0.9f, 0.6f, 0.4f, 0.9f, 0.2f, 0.1f, 0.7f, 0.7f, 0.5f, 0.7f, 0.3f, 0.0f, 0.8f, 0.3f, 0.7f, 0.6f, 0.5f, 0.7f, 0.1f, 0.5f, 0.5f, 0.8f, 0.1f, 0.3f, 0.6f, 0.6f, 0.4f, 0.7f, 0.6f, 0.4f, 0.6f, 0.1f, 0.1f}, new float[] {1.0f, 0.7f, 0.8f, 0.7f, 0.2f, 0.4f, 0.9f, 0.5f, 1.0f, 0.4f, 0.5f, 0.4f, 0.5f, 0.4f, 1.0f, 0.4f, 0.4f, 0.1f, 0.6f, 0.2f, 0.6f, 0.2f, 0.5f, 0.6f, 0.4f, 0.6f, 0.4f, 0.8f, 0.3f, 0.6f, 0.5f, 0.8f, 0.0f, 0.8f, 0.4f, 0.6f, 0.8f, 0.5f, 1.0f, 0.7f, 0.5f, 0.5f, 0.5f, 0.6f, 0.3f, 0.4f, 0.6f, 0.2f, 0.9f, 0.4f, 0.8f, 0.1f, 0.6f, 0.6f, 0.9f, 0.1f, 0.3f, 0.1f, 0.9f, 0.1f, 0.7f, 0.9f, 0.4f, 0.2f, 0.5f, 0.8f, 0.9f, 0.2f, 0.1f, 0.5f} };
	public GameObject gateStart;
	public GameObject gateContPrefabL;
	public GameObject gateContPrefabR;
	public GameObject wallLeftStart;
	public GameObject wallRightStart;
	public GameObject wallLeftEnd;
	public GameObject wallRightEnd;
	private int SceneSelected;
	private Scene currentScene;

	// Use this for initialization
	void Start () {
		currentScene = SceneManager.GetActiveScene ();
		getSceneName (currentScene);
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

	void getSceneName(Scene other)
	{
		if(other.name == "wipExperimentMaze1")
		{
			SceneSelected = 0;
		}

		else if(other.name == "wipExperimentMaze2")
		{
			SceneSelected = 1;
		}

		else if(other.name == "wipExperimentMaze3")
		{
			SceneSelected = 2;
		}

		else if(other.name == "wipExperimentMaze4")
		{
			SceneSelected = 3;
		}

		else if(other.name == "wipExperimentMaze5")
		{
			SceneSelected = 4;
		}

		else if(other.name == "wipExperimentMaze6")
		{
			SceneSelected = 5;
		}
	}
}
