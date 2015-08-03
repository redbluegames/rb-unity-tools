using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RBPhysics2DCaster : MonoBehaviour {

	public bool CircleCastAll;
	public float Radius;

	void OnGUI ()
	{
		if (CircleCastAll) {
			RBPhysics2D.OverlapCircleAll (transform.position, Radius);
		} else {
			RBPhysics2D.OverlapCircle (transform.position, Radius);
		}
	}
}
