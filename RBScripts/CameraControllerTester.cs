using UnityEngine;
using System.Collections;

public class CameraControllerTester : MonoBehaviour
{
	public CameraShake cameraShake;

	// Test shake variables
	public float shakeSpeed = 1f;
	public float shakeMagnitude = 2f;
	public float shakeDuration = 1f;
	public float shakeDelay = 0f;
	public bool testShake;                  // Force camera shake using provided properties

	// Test zoom variables
	public float zoomRatio = 1.5f;
	public float zoomDuration = 2f;
	public float resetZoomDuration = 1.0f;
	public bool testZoom;                   // Force camera zoom using provided properties
	public bool testResetZoom;              // Force reset camera zoom

	// Update is called once per frame
	void Update ()
	{
		// Try our test methods
		TestShake ();
	}

	/*
	 * Test method to help check the behavior of various shakes. Set testShake to true
	 * while in Play mode.
	 */
	void TestShake ()
	{
		if (testShake) {
			testShake = false;
			cameraShake.Shake (shakeSpeed, shakeDuration, shakeMagnitude);
		}
	}

}
