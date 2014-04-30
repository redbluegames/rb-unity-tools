using UnityEngine;
using System.Collections;

public class Gizmo : MonoBehaviour
{
	public float gizmoSize = 0.5f;
	public Color gizmoColor = Color.yellow;

	void OnDrawGizmos ()
	{
		Gizmos.color = gizmoColor;
		Gizmos.DrawWireSphere (transform.position, gizmoSize);

		// Draw X line
		float lineSize = gizmoSize * 2;
		Vector3 position = transform.position + transform.TransformDirection (Vector3.forward * lineSize);
		Gizmos.DrawLine (transform.position, position);
		// Draw Z line
		position = transform.position + transform.TransformDirection (Vector3.right * lineSize);
		Gizmos.DrawLine (transform.position, position);

	}
}

