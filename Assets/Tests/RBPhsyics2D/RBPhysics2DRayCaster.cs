using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RBPhysics2DRayCaster : MonoBehaviour {
	
	public CastType Cast;
	public enum CastType
	{
		OverlapCircle,
		OverlapArea,
	}
	
	public bool CastAll;
	public float Radius;
	public Vector2 CornerAOffset;
	public Vector2 CornerBOffset;
	public LayerMask castLayers;
	
	void OnRenderObject ()
	{
		RBPhysics2D.RayCast (transform.position, transform.up, 20);
		//RBPhysics2D.
	}
}