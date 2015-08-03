using UnityEngine;
using System.Collections;
using UnityEditor;

public class RBPhysics2D
{
	public static bool ShowCasts = true;

	#region Cast Wrappers
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

	static void DrawBoxAndHits (Collider2D[] hits, Vector2 cornerA, Vector2 cornerB) {
		Color drawColor = Color.green;
		if (hits != null && hits.Length > 0) {
			drawColor = Color.red;
			
			DebugDrawColliders (hits);
		}
		
		DebugDrawBox (cornerA, cornerB, drawColor);
	}

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
		Color drawColor = Color.green;
		if (hits != null && hits.Length > 0) {
			drawColor = Color.red;
			
			DebugDrawColliders (hits);
		}
		DebugDrawCircle (center, radius, drawColor);
	}

	#endregion

	#region Debug Drawing
	static void DebugDrawColliders (Collider2D[] colliders)
	{
		for(int i = 0; i < colliders.Length; i++) {
			DebugDrawCollider (colliders[i]);
		}
	}

	static void DebugDrawCollider (Collider2D collider)
	{
		if (collider.GetType () == typeof(CircleCollider2D)) {
			CircleCollider2D circleCollider = collider as CircleCollider2D;
			DebugDrawCircle ((Vector2) circleCollider.transform.position + circleCollider.offset, circleCollider.radius, Color.yellow);
		} else if (collider.GetType () == typeof(BoxCollider2D)) {
			BoxCollider2D boxCollider = collider as BoxCollider2D;
			Vector2 cornerA = (Vector2) boxCollider.transform.position + boxCollider.offset;
			Vector2 cornerB = cornerA;
			cornerA -= (boxCollider.size * 0.5f);
			cornerB += (boxCollider.size * 0.5f);
			DebugDrawBox (cornerA, cornerB, Color.yellow);
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
	#endregion
}
