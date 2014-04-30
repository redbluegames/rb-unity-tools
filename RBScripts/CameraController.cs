using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public GameObject followTarget;
	public int viewPortIndex;

	// Position offsets from the camera
	public Vector3 defaultOffset;
	public Quaternion defaultRotation { get; private set; }
	public float defaultFieldOfView  { get; private set; }
	Vector3 shakeOffset;
	Vector3 followOffset;
	float fieldOfViewOffset;

	void Start ()
	{
		SetupDefaults ();
	}

	/*
	 * Helper function that wires up all default settings for
	 * the camera.
	 */
	void SetupDefaults ()
	{
		defaultFieldOfView = camera.fieldOfView;

		// Resolve offset so that we can get the angle to the target
		ResolveOffsets ();

		if(followTarget == null) {
			Debug.LogError("Camera is missing a Follow Target.");
			defaultRotation = new Quaternion();
		} else {
			transform.LookAt (followTarget.transform.position);
			defaultRotation = transform.rotation;
		}
	}

	void LateUpdate ()
	{
		FollowTarget ();
		ResolveOffsets ();
	}

	void FollowTarget ()
	{
		// Set the offset this camera uses when following its target
		if (followTarget != null) {
			followOffset = followTarget.transform.position;
		}
	}

	/*
	 * Calculate where the camera will display after having performed other
	 * game updates. Things like shake and camera follow should take effect now.
	 */
	void ResolveOffsets ()
	{
		camera.transform.position = defaultOffset + followOffset + shakeOffset;
		transform.rotation = defaultRotation;
		//camera.fieldOfView = defaultFieldOfView + fieldOfViewOffset;
	}

	/*
	 * Split the cameras up among the players.
	 */
	public void SplitScreenView (int numViewports)
	{
		float border = 0.002f;
		float portion = (1.0f / numViewports) - (border);
		float spacing = viewPortIndex * border;
		camera.rect = new Rect ((viewPortIndex * portion) + spacing, 0, portion, 1);
	}


	#region Zoom Functionality
	/*
	 * Zoom by a given ratio over a given duration in seconds. Increases/decreases zoom
	 * level linearly over time. Example: 1.05 would evenly zoom in 5% over 5 seconds.
	 */
	public void Zoom (float targetZoomRatio, float duration)
	{
		// Convert our "zoom ratio" to a "field of view" ratio which is the inverse
		// i.e. 1.05 zoom-in means 0.95 field of view.
		float fieldOfViewRatio = 1.0f - (targetZoomRatio - 1.0f);
		float newFieldOfView = fieldOfViewRatio * defaultFieldOfView;
		ZoomProperty zoomProperty = new ZoomProperty (newFieldOfView, duration);
		StopCoroutine ("SetFieldOfView");
		StartCoroutine ("SetFieldOfView", zoomProperty);
	}

	/*
	 * Reset the zoom to the starting field of view.
	 */
	public void ResetZoom (float duration)
	{
		StopCoroutine ("SetFieldOfView");
		ZoomProperty zoomProperty = new ZoomProperty (defaultFieldOfView, duration);
		StartCoroutine ("SetFieldOfView", zoomProperty);
	}

	/*
	 * Reset the zoom to the starting field of view over the default amount of time.
	 */
	public void ResetZoom ()
	{
		float defaultResetDuration = 0.2f;
		ResetZoom (defaultResetDuration);
	}

	/*
	 * Sets the camera field of view to a certain value over a provided amount of seconds.
	 */
	IEnumerator SetFieldOfView (ZoomProperty zoomProperty)
	{
		float startingFieldOfView = camera.fieldOfView;
		float elapsed = 0;

		while (elapsed < zoomProperty.duration) {
			elapsed += Time.deltaTime;
			float percentageComplete = elapsed / zoomProperty.duration;
			float newFieldOfView = Mathf.Lerp (startingFieldOfView, zoomProperty.fieldOfView, percentageComplete);
			camera.fieldOfView = newFieldOfView;
			yield return null;
		}
	}

	/*
	 * Custom class to store data about a zoom. Used for coroutines which require
	 * one parameter.
	 */
	class ZoomProperty
	{
		public float fieldOfView { get; private set; }
		public float duration { get; private set; }

		public ZoomProperty (float desiredFieldOfView, float zoomDuration)
		{
			fieldOfView = desiredFieldOfView;
			duration = zoomDuration;
		}
	}

	#endregion

	#region Shake Functionality
	// -------------------------------------------------------------------------
	/*
	 * Shake the camera with a given speed, duration, and magnitude using a Perlin
	 * noised utility.
	 */
	public void Shake (float speed, float duration, float magnitude)
	{
		StartCoroutine (SetShakeOffset (speed, duration, magnitude));
	}

	/*
	 * Shake camera, setting temporary offset variables which will be applied on
	 * LateUpdate.
	 * 
	 * Resource:
	 * http://unitytipsandtricks.blogspot.com/2013/05/camera-shake.html
	 */
	IEnumerator SetShakeOffset (float speed, float duration, float magnitude)
	{
		float randomStart = Random.Range (-1000.0f, 1000.0f);
		float elapsed = 0.0f;
		while (elapsed < duration) {
			
			elapsed += Time.deltaTime;			
			
			float percentComplete = elapsed / duration;			

			// We want to reduce the shake from full power to 0 starting half way through
			float damper = 1.0f - Mathf.Clamp (2.0f * percentComplete - 1.0f, 0.0f, 1.0f);
			
			// Calculate the noise parameter starting randomly and going as fast as speed allows
			float alpha = randomStart + speed * percentComplete;
			
			// map noise to [-1, 1] (noise generated from 0-1)
			float x = Mathf.PerlinNoise (alpha, 0) * 2.0f - 1.0f;
			float y = Mathf.PerlinNoise (0, alpha) * 2.0f - 1.0f;
			
			x *= magnitude * damper;
			y *= magnitude * damper;

			shakeOffset = new Vector3 (x, 0, y);
			yield return null;
		}
	}
	#endregion
}
