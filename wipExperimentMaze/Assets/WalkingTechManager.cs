using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingTechManager : MonoBehaviour {

	public int subjectNumber;
	public int trialNumber; //-1 means for training

	public static int statSubject;
	public static int statTrial;

	// Use this for initialization
	void Start () {
		statSubject = subjectNumber;
		statTrial = trialNumber;
		System.Type[] conditionOrder = new System.Type[6];

		this.transform.position = this.transform.position + new Vector3 (0f, GlobalVariables.height - 2.74f + 1.26f, 0f);

		if (trialNumber < 0) {
			switch (trialNumber) {
			case -4:
				this.GetComponent<ThresholdGear> ().enabled = true;
				break;
			case -3:
				this.GetComponent<ThresholdGo> ().enabled = true;
				break;
			case -2:
				this.GetComponent<FreqGear> ().enabled = true;
				break;
			case -1:
				this.GetComponent<FreqGo> ().enabled = true;
				break;
			}
			return;
		}

		switch (subjectNumber % 12) {
		case 0:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInput4Old),
				typeof(AccelerometerInputCNN),
				typeof(AccelerometerInputRate),
				typeof(AccelerometerInput4)
			};
			break;
		case 1:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputCNN),
				typeof(AccelerometerInputRate),
				typeof(AccelerometerInput4),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInput4Old)
			};
			break;
		case 2:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInput4Old),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInputCNN),
				typeof(AccelerometerInput4),
				typeof(AccelerometerInputRate)
			};
			break;
		case 3:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputCNN),
				typeof(AccelerometerInput4),
				typeof(AccelerometerInputRate),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInput4Old),
				typeof(AccelerometerInputRateGear)
			};
			break;
		case 4:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInput4Old),
				typeof(AccelerometerInputRate),
				typeof(AccelerometerInputCNN),
				typeof(AccelerometerInput4)
			};
			break;
		case 5:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputRate),
				typeof(AccelerometerInputCNN),
				typeof(AccelerometerInput4),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInput4Old)
			};
			break;
		case 6:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInput4Old),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInputRate),
				typeof(AccelerometerInput4),
				typeof(AccelerometerInputCNN)
			};
			break;
		case 7:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInputRate),
				typeof(AccelerometerInput4),
				typeof(AccelerometerInputCNN),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInput4Old),
				typeof(AccelerometerInputCNNGear)
			};
			break;
		case 8:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInput4Old),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInput4),
				typeof(AccelerometerInputCNN),
				typeof(AccelerometerInputRate)
			};
			break;
		case 9:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInput4),
				typeof(AccelerometerInputCNN),
				typeof(AccelerometerInputRate),
				typeof(AccelerometerInput4Old),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInputRateGear)
			};
			break;
		case 10:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInput4Old),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInputCNNGear),
				typeof(AccelerometerInput4),
				typeof(AccelerometerInputRate),
				typeof(AccelerometerInputCNN)
			};
			break;
		case 11:
			conditionOrder = new System.Type[] {
				typeof(AccelerometerInput4),
				typeof(AccelerometerInputRate),
				typeof(AccelerometerInputCNN),
				typeof(AccelerometerInput4Old),
				typeof(AccelerometerInputRateGear),
				typeof(AccelerometerInputCNNGear)
			};
			break;
		}

		if (conditionOrder[trialNumber] == typeof(AccelerometerInput4))
			this.GetComponent<AccelerometerInput4> ().enabled = true;
		if (conditionOrder[trialNumber] == typeof(AccelerometerInputRate))
			this.GetComponent<AccelerometerInputRate> ().enabled = true;
		if (conditionOrder[trialNumber] == typeof(AccelerometerInputCNN))
			this.GetComponent<AccelerometerInputCNN> ().enabled = true;
		if (conditionOrder[trialNumber] == typeof(AccelerometerInput4Old))
			this.GetComponent<AccelerometerInput4Old> ().enabled = true;
		if (conditionOrder[trialNumber] == typeof(AccelerometerInputRateGear))
			this.GetComponent<AccelerometerInputRateGear> ().enabled = true;
		if (conditionOrder[trialNumber] == typeof(AccelerometerInputCNNGear))
			this.GetComponent<AccelerometerInputCNNGear> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
