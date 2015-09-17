using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RBPhysics2DRayCaster : MonoBehaviour
{
	public CastType Cast;
	public enum CastType
	{
		Raycast,
		Linecast,
	}
	
	public bool CastAll;
	public float Distance;
	public LayerMask castLayers;
	
	void Update ()
	{
		switch (Cast) {
		case CastType.Raycast:
			if (CastAll) {
				RBPhysics2D.RayCastAll (transform.position, transform.up, Distance, castLayers);
			} else {
				RBPhysics2D.RayCast (transform.position, transform.up, Distance, castLayers);
			}
			break;
		case CastType.Linecast:
			if (CastAll) {
				RBPhysics2D.LineCastAll (transform.position, transform.position + (Distance * transform.up), castLayers);
			} else {
				RBPhysics2D.LineCast (transform.position, transform.position + (Distance * transform.up), castLayers);
			}
			break;
		}
	}
}