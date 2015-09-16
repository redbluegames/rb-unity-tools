using UnityEngine;
using System.Collections;
using UnityEditor;

public class RBPhysics2D
{
	public static Color HitColliderColor = Color.yellow;
	public static Color CastColor = Color.green;
	public static Color HitCastColor = Color.red;
	public static Color HitNormalsColor = Color.magenta;

	#region RayCast Wrapper
	public static RaycastHit2D RayCast (Vector2 origin, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers,
	                                    float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
	{
		RaycastHit2D hit = Physics2D.Raycast (origin, direction, distance, layerMask, minDepth, maxDepth);
		DrawRayAndHits (new RaycastHit2D[] {hit}, origin, direction, distance);
		return hit;
	}
	
	public static RaycastHit2D[] RayCastAll (Vector2 origin, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers,
	                                    float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll (origin, direction, distance, layerMask, minDepth, maxDepth);
		DrawRayAndHits (hits, origin, direction, distance);
		return hits;
	}

	static void DrawRayAndHits (RaycastHit2D[] hits, Vector2 origin, Vector2 direction, float distance = Mathf.Infinity)
	{
		float maxLineDistance = 100000.0f;
		float lineDrawDistance = Mathf.Min (distance, maxLineDistance);
		DrawLineAndHits (hits, origin, origin + (direction * lineDrawDistance));
	}

	static void DrawLineAndHits (RaycastHit2D[] hits, Vector2 origin, Vector2 endpoint)
	{
		Color drawColor = CastColor;
		if (hits != null && hits.Length > 0) {
			for (int i = 0; i < hits.Length; i++) {
				DrawRaycastHit2D (hits [i]);
				if (hits [i].collider != null) {
					drawColor = HitCastColor;
				}
			}
		}

		DebugDrawArrow (origin, endpoint, drawColor);
	}

	static void DrawRaycastHit2D (RaycastHit2D hit)
	{
		// Draw the hit collider
		if (hit.collider != null) {
			DebugDrawCollider (hit.collider);
		}

		// Draw the normal at the hit location, or a circle for hits from rays originating inside collider
		bool isRayOriginatingFromInside = Mathf.Approximately (hit.fraction, 0.0f);
		if (isRayOriginatingFromInside) {
			DebugDrawCircle (hit.point, 0.2f, HitNormalsColor, 20);
		} else {
			Debug.DrawRay (hit.point, hit.normal, HitNormalsColor, 0.01f);
		}
	}
	#endregion

	#region OverlapAreaCast Wrapper
	public static Collider2D OverlapArea (Vector2 cornerA, Vector2 cornerB, int layerMask = Physics2D.DefaultRaycastLayers, 
	                         float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity)
	{
		Collider2D hit = Physics2D.OverlapArea (cornerA, cornerB, layerMask, minDepth, maxDepth);
		DrawBoxAndHits (new Collider2D[] {hit}, cornerA, cornerB);

		return hit;
	}

	public static Collider2D[] OverlapAreaAll (Vector2 cornerA, Vector2 cornerB, int layerMask = Physics2D.DefaultRaycastLayers, 
	                                float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity)
	{
		Collider2D[] hits = Physics2D.OverlapAreaAll (cornerA, cornerB, layerMask, minDepth, maxDepth);
		DrawBoxAndHits (hits, cornerA, cornerB);

		return hits;
	}

	static void DrawBoxAndHits (Collider2D[] hits, Vector2 cornerA, Vector2 cornerB)
	{
		Color drawColor = CastColor;
		if (hits != null && hits.Length > 0) {
			drawColor = HitCastColor;
			
			DebugDrawColliders (hits);
		}

		DebugDrawBox (cornerA, cornerB, drawColor);
	}
	#endregion
	
	#region OverlapCircleCast Wrapper
	public static Collider2D OverlapCircle (Vector2 center, float radius, int layerMask = Physics2D.DefaultRaycastLayers, 
	                                  float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity)
	{
		Collider2D hit = Physics2D.OverlapCircle (center, radius, layerMask, minDepth, maxDepth);
		DrawCircleAndHits (new Collider2D[] {hit}, center, radius);

		return hit;
	}

	public static Collider2D[] OverlapCircleAll (Vector2 center, float radius, int layerMask = Physics2D.DefaultRaycastLayers, 
	                                             float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity)
	{
		Collider2D[] hits = Physics2D.OverlapCircleAll (center, radius, layerMask, minDepth, maxDepth);
		DrawCircleAndHits (hits, center, radius);

		return hits;
	}

	static void DrawCircleAndHits (Collider2D[] hits, Vector2 center, float radius)
	{
		Color drawColor = CastColor;
		if (hits != null && hits.Length > 0) {
			drawColor = HitCastColor;
			
			DebugDrawColliders (hits);
		}
		DebugDrawCircle (center, radius, drawColor);
	}
	#endregion

	#region Debug Drawing for Colliders
	static void DebugDrawColliders (Collider2D[] colliders)
	{
		for (int i = 0; i < colliders.Length; i++) {
			DebugDrawCollider (colliders [i]);
		}
	}

	static void DebugDrawCollider (Collider2D collider)
	{
		if (collider.GetType () == typeof(CircleCollider2D)) {
			DebugDrawCircleCollider2D ((CircleCollider2D)collider);
		} else if (collider.GetType () == typeof(BoxCollider2D)) {
			DebugDrawBoxCollider2D ((BoxCollider2D)collider);
		} else if (collider.GetType () == typeof(PolygonCollider2D)) {
			DebugDrawPolygonCollider2D ((PolygonCollider2D)collider);
		} else {
			throw new System.NotImplementedException ("Tried to DebugDraw a collider of unrecognized type. Type: " + collider.GetType ());
		}
	}

	static void DebugDrawCircleCollider2D (CircleCollider2D circleCollider)
	{
		DebugDrawCircle ((Vector2)circleCollider.transform.position + circleCollider.offset, circleCollider.radius, HitColliderColor);
	}

	static void DebugDrawBoxCollider2D (BoxCollider2D boxCollider)
	{
		// Define the corners about the origin
		Vector2 halfSize = boxCollider.size * 0.5f;
		Vector2 cornerTL = new Vector2 (-halfSize.x, halfSize.y);
		Vector2 cornerBR = new Vector2 (halfSize.x, -halfSize.y);
		
		// Offset corners by collider's offset
		cornerTL += boxCollider.offset;
		cornerBR += boxCollider.offset;
		
		DebugDrawBox (boxCollider.transform.position, cornerTL, cornerBR, boxCollider.transform.rotation, HitColliderColor);
	}

	static void DebugDrawPolygonCollider2D (PolygonCollider2D polyCollider)
	{
		if (polyCollider.pathCount >= 1) {
			Vector2[] path = polyCollider.GetPath (0);
			// Apply transform's rotation to points
			Vector2[] rotatedPath = new Vector2[path.Length];
			Quaternion rotation = polyCollider.transform.rotation;
			for (int i = 0; i < path.Length; i++) {
				// First offset points by polygon collider's offset
				Vector3 offsetPoint = path [i] + polyCollider.offset;
				Vector3 rotatedPoint = rotation * new Vector3 (offsetPoint.x, offsetPoint.y, 0.0f);
				rotatedPath [i] = (Vector2)rotatedPoint;
			}
			// render polygon at transform's position
			DebugDrawPolygon (polyCollider.transform.position, rotatedPath, HitColliderColor);
		}
	}
	#endregion

	#region Primitive Drawing
	static void DebugDrawCircle (Vector2 center, float radius, Color color, float numSegments = 40, float duration = 0.01f)
	{	
		// Precompute values based on segments
		float radiansPerCast = (2 * Mathf.PI) / numSegments;
		float cosTheta = Mathf.Cos (radiansPerCast);
		float sinTheta = Mathf.Sin (radiansPerCast);
		
		// Build rotation matrix about z axis
		Vector2[] rotation = new Vector2[] {
			new Vector2 (cosTheta, -sinTheta),
			new Vector2 (sinTheta, cosTheta)
		};
		float startingRadians = 0.0f;
		Vector2 vertexStart = new Vector2 (Mathf.Cos (startingRadians), Mathf.Sin (startingRadians));
		vertexStart *= radius;
		
		for (int i = 0; i < numSegments; i++) {
			Vector2 rotatedPoint = new Vector2 (Vector2.Dot (vertexStart, rotation [0]), 
			                                    Vector2.Dot (vertexStart, rotation [1]));
			// Draw the segment, shifted by the center
			Debug.DrawLine (center + vertexStart, center + rotatedPoint, color, duration);
			
			vertexStart = rotatedPoint;
		}
	}

	static void DebugDrawBox (Vector2 worldTopLeft, Vector2 worldBottomRight, Color color, float duration = 0.01f)
	{
		// Convert corners to local offsets and position
		Vector2 center = (worldTopLeft + worldBottomRight) / 2.0f;
		Vector2 localTopLeft = worldTopLeft - center;
		Vector2 localBottomRight = worldBottomRight - center;
		DebugDrawBox (center, localTopLeft, localBottomRight, Quaternion.identity, color, duration);
	}

	static void DebugDrawBox (Vector2 center, Vector2 localTopLeft, Vector2 localBottomRight, Quaternion rotation, Color color, float duration = 0.01f)
	{
		// Rotate all corners
		Vector3 rotatedTopLeft = rotation * localTopLeft;
		Vector3 rotatedTopRight = rotation * new Vector2 (localBottomRight.x, localTopLeft.y);
		Vector3 rotatedBottomRight = rotation * localBottomRight;
		Vector3 rotatedBottomLeft = rotation * new Vector2 (localTopLeft.x, localBottomRight.y);

		// Shift the corners to the center position
		rotatedTopLeft += (Vector3)center;
		rotatedTopRight += (Vector3)center;
		rotatedBottomRight += (Vector3)center;
		rotatedBottomLeft += (Vector3)center;

		Debug.DrawLine (rotatedTopLeft, rotatedBottomLeft, color, duration);
		Debug.DrawLine (rotatedBottomLeft, rotatedBottomRight, color, duration);
		Debug.DrawLine (rotatedBottomRight, rotatedTopRight, color, duration);
		Debug.DrawLine (rotatedTopRight, rotatedTopLeft, color, duration);
	}

	public static void DebugDrawPolygon (Vector2 center, Vector2[] points, Color color, float duration = 0.01f)
	{
		// Draw each segment except the last
		for (int i = 0; i < points.Length - 1; i++) {
			Vector3 nextPoint = points [i + 1] + center;
			Vector3 currentPoint = points [i] + center;
			Debug.DrawLine (currentPoint, nextPoint, color, duration);
		}
		// Draw the last segment by connecting it back to the start
		if (points.Length > 1) {
			Debug.DrawLine (points [points.Length - 1] + center, points [0] + center, color, duration);
		}
	}

	public static void DebugDrawArrow (Vector2 origin, Vector2 endpoint, Color color, float duration = 0.01f)
	{
		// Draw the line that makes up the body of the arrow
		Debug.DrawLine (origin, endpoint, color, 0.01f);

		// Draw arrowhead so we can see direction
		// Configure arrow
		float arrowThetaDegrees = 30.0f;
		float arrowheadLength = 0.05f;
		Vector2 arrowheadHandle = (origin - endpoint) * arrowheadLength;

		Quaternion arrowRotationR = Quaternion.AngleAxis (arrowThetaDegrees, Vector3.forward);
		Vector2 arrowheadR = arrowRotationR * arrowheadHandle;
		Debug.DrawLine (endpoint, endpoint + arrowheadR, color, 0.01f);

		Quaternion arrowRotationL = Quaternion.AngleAxis (-arrowThetaDegrees, Vector3.forward);
		Vector2 arrowheadL = arrowRotationL * arrowheadHandle;
		Debug.DrawLine (endpoint, endpoint + arrowheadL, color, 0.01f);
	}
	#endregion
}
