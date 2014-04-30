using UnityEngine;
using System.Collections;

/// <summary>
/// Tool to add a rotation effect to an object. Rotates linearly over time.
/// </summary>
public class TweenAutoRotate : MonoBehaviour
{
	// Rotation speed & axis
	public Vector3 rotation;
	public bool rotateOn;
	
	// Rotation space
	public Space space = Space.Self;
	
	void Update ()
	{
		if (rotateOn) {
			transform.Rotate (rotation * Time.deltaTime, space);
		}
	}
}
