using UnityEngine;
using System.Collections;
using UnityEditor;

public class RBPhysics2D
{
	public static bool ShowCasts = true;
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

	static void DrawRayAndHits (RaycastHit2D [] hits, Vector2 origin, Vector2 direction, float distance = Mathf.Infinity)
	{
		float maxLineDistance = 100000.0f;
		float lineDrawDistance = Mathf.Min (distance, maxLineDistance);
		DrawLineAndHits (hits, origin, origin + (direction * lineDrawDistance));
	}

	static void DrawLineAndHits (RaycastHit2D [] hits, Vector2 origin, Vector2 endpoint)
	{
		Color drawColor = CastColor;
		if (hits != null && hits.Length > 0) {
			for (int i = 0; i < hits.Length; i++) {
				DrawRaycastHit2D (hits[i]);
				if (hits[i].collider != null) {
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
			DebugDrawCircle (hit.point, 0.2f, HitNormalsColor);
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
	
	#region OverlapACircleCast Wrapper
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

	#region Debug Drawing
	static void DebugDrawColliders (Collider2D[] colliders)
	{
		for (int i = 0; i < colliders.Length; i++) {
			DebugDrawCollider (colliders [i]);
		}
	}

	static void DebugDrawCollider (Collider2D collider)
	{
		if (collider.GetType () == typeof(CircleCollider2D)) {
			CircleCollider2D circleCollider = collider as CircleCollider2D;
			DebugDrawCircle ((Vector2)circleCollider.transform.position + circleCollider.offset, circleCollider.radius, HitColliderColor);
		} else if (collider.GetType () == typeof(BoxCollider2D)) {
			BoxCollider2D boxCollider = collider as BoxCollider2D;
			Vector2 cornerA = (Vector2)boxCollider.transform.position + boxCollider.offset;
			Vector2 cornerB = cornerA;
			cornerA -= (boxCollider.size * 0.5f);
			cornerB += (boxCollider.size * 0.5f);
			DebugDrawBox (cornerA, cornerB, HitColliderColor);
		} else if (collider.GetType () == typeof(PolygonCollider2D)) {
			PolygonCollider2D polyCollider = collider as PolygonCollider2D;
			if (polyCollider.pathCount >= 1) {
				DebugDrawPolygon ((Vector2)polyCollider.transform.position + polyCollider.offset, polyCollider.GetPath(0), HitColliderColor);
			}
		}
	}

	static void DebugDrawBox (Vector2 cornerA, Vector2 cornerB, Color color, float duration = 0.01f)
	{
		Vector2 cornerAB = new Vector2 (cornerA.x, cornerB.y);
		Vector2 cornerBA = new Vector2 (cornerB.x, cornerA.y);

		Debug.DrawLine (cornerA, cornerAB, color, duration);
		Debug.DrawLine (cornerAB, cornerB, color, duration);
		Debug.DrawLine (cornerB, cornerBA, color, duration);
		Debug.DrawLine (cornerBA, cornerA, color, duration);
	}

	static void DebugDrawCircle (Vector2 center, float radius, Color color, float duration = 0.01f)
	{
		if (!ShowCasts) {
			return;
		}

		float startingRadians = 0.0f;
		float segmentsInCircle = 40;
		float radiansPerCast = (1.0f / segmentsInCircle) * 2 * Mathf.PI;

		Vector2 startPoint = new Vector2 (Mathf.Cos (startingRadians), Mathf.Sin (startingRadians));
		startPoint *= radius;
		startPoint += center;

		float currentDrawingAngle = 0.0f;
		while (currentDrawingAngle < (startingRadians + 2 * Mathf.PI)) {
			// Calculate end point
			float nextAngle = currentDrawingAngle + radiansPerCast;
			float nextX = Mathf.Cos (nextAngle);
			float nextY = Mathf.Sin (nextAngle);
			Vector2 nextPoint = new Vector2 (nextX, nextY);
			nextPoint *= radius; // Scale to radius
			nextPoint += center; // Offset to center

			Debug.DrawLine (startPoint, nextPoint, color, duration);

			startPoint = nextPoint;

			currentDrawingAngle = nextAngle;
		}
	}

	public static void DebugDrawPolygon (Vector2 offset, Vector2[] points, Color color, float duration = 0.01f)
	{
		for (int i = 0; i < points.Length - 1; i++) {
			Vector2 nextPoint = points [i + 1] + offset;
			Vector2 currentPoint = points [i] + offset;
			Debug.DrawLine (currentPoint, nextPoint, color, duration);
		}
		// Connect back to start
		if (points.Length > 1) {
			Debug.DrawLine (points [points.Length - 1] + offset, points [0] + offset, color, duration);
		}
	}

	public static void DebugDrawArrow (Vector2 origin, Vector2 endpoint, Color color, float duration = 0.01f)
	{
		Debug.DrawLine (origin, endpoint, color, 0.01f);

		// Draw arrowhead so we can see direction
		Vector2 arrow = endpoint - origin;
		float arrowheadWidthScale = 0.05f;
		Vector2 arrowheadX = new Vector2 (arrow.y, -arrow.x) * arrowheadWidthScale;
		float arrowheadLengthScale = 2f;
		Vector2 arrowheadY = new Vector2 (arrowheadX.y, -arrowheadX.x) * arrowheadLengthScale;
		Debug.DrawLine (endpoint, endpoint + arrowheadX + arrowheadY, color, 0.01f);
		Debug.DrawLine (endpoint, endpoint - arrowheadX + arrowheadY, color, 0.01f);
	}
	#endregion
}
