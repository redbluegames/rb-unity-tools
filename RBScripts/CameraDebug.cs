using UnityEngine;
using System.Collections;

public class CameraDebug : MonoBehaviour
{
	public float movespeed = 0.1f;
	bool isDebugPaused;
	Vector3 debugStartPosition;
	bool advanceThisFrame;

	// Stores whether or not each component was originally enabled
	bool[] previousComponentStatus;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		bool isPausePressed = Input.GetKeyDown (KeyCode.Backslash);
		if (isPausePressed) {
			SetDebugPause (!isDebugPaused);
		}
		if (isDebugPaused) {
			UpdateFlight ();
			UpdateFrameAdvance ();
		}
	}

	private void SetDebugPause (bool pause)
	{
		if (pause) {
			TimeManager.Instance.RequestLowLevelPause ();
			isDebugPaused = true;
			debugStartPosition = transform.position;

			// Disable updates on all other components on the camera
			DisableAllOtherBehaviours ();
		} else { 
			TimeManager.Instance.RequestLowLevelUnpause ();
			isDebugPaused = false;
			transform.position = debugStartPosition;

			// Reenable all components
			RestoreAllOtherComponentsToOriginalEnabledStatus ();
		}
	}

	/*
	 * Disables all the script components on this GameObject. Stores off their old
	 * enabled status in order to restore them back to their previous state.
	 */
	void DisableAllOtherBehaviours ()
	{
		MonoBehaviour[] otherBehaviors = GetComponents<MonoBehaviour> ();
		int i = 0;
		previousComponentStatus = new bool[otherBehaviors.Length];
		foreach (MonoBehaviour behavior in otherBehaviors) {
			previousComponentStatus [i] = behavior.enabled;
			behavior.enabled = false;
			i++;
		}
		// Reenable this component
		enabled = true;
	}

	/*
	 * Restores each other script component's original enabled status.
	 */
	void RestoreAllOtherComponentsToOriginalEnabledStatus ()
	{
		int i = 0;
		MonoBehaviour[] otherBehaviors = GetComponents<MonoBehaviour> ();
		foreach (MonoBehaviour behavior in otherBehaviors) {
			behavior.enabled = previousComponentStatus [i];
			i++;
		}
	}

	private void UpdateFlight ()
	{
		//float forwardBack = Input.GetKey (KeyCode.UpArrow) + -Input.GetKey (KeyCode.DownArrow);
		//float leftRight = Input.GetKey (KeyCode.RightArrow) + -Input.GetKey (KeyCode.LeftArrow);
		float forwardBack = Input.GetAxisRaw ("Vertical");
		float leftRight = Input.GetAxisRaw ("Horizontal");
		
		Vector3 direction = new Vector3 (leftRight, 0.0f, forwardBack);
		direction = direction.normalized;
		Vector3 movement = direction * movespeed;
		transform.Translate (movement, Space.Self);
	}

	private void UpdateFrameAdvance ()
	{
		bool wasAdvancing = advanceThisFrame;
		advanceThisFrame = Input.GetKeyDown (KeyCode.RightBracket) || Input.GetKey (KeyCode.LeftBracket);
		if (wasAdvancing && !advanceThisFrame) {
			TimeManager.Instance.RequestLowLevelPause ();
		} else if (!wasAdvancing && advanceThisFrame) {
			TimeManager.Instance.RequestLowLevelUnpause ();
		}
	}
}