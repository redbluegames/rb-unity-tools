using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RBPhysics2DRayCaster : MonoBehaviour {
	
	public CastType Cast;
	public enum CastType
	{
		Raycast,
	}
	
	public bool CastAll;
	public float Distance;
	public LayerMask castLayers;
	
	void Update ()
	{
		if (CastAll) {
			RBPhysics2D.RayCastAll (transform.position, transform.up, Distance, castLayers);
		} else {
			RBPhysics2D.RayCast (transform.position, transform.up, Distance, castLayers);
		}
	}
}