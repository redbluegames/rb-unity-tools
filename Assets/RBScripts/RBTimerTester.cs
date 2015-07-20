using UnityEngine;
using System.Collections;
using RedBlueTools;

public class RBTimerTester : MonoBehaviour {

	public RBTimer myTimer1;
	public RBTimer myTimer2;

	void onTimerExpires1 ()
	{
		Debug.Log ("Done1");
	}
	
	void onTimerExpires2 ()
	{
		Debug.Log ("Done2");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			if(myTimer1.IsRunning) {
				myTimer1.Stop ();
			} else {
				myTimer1.Start (this, onTimerExpires1);
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			if(myTimer2.IsRunning) {
				myTimer2.Stop ();
			} else {
				myTimer2.Start (this, onTimerExpires2);
			}
		}
		
		if(Input.GetKeyDown(KeyCode.Space)) {
			StopAllCoroutines ();
		}
	}
}
