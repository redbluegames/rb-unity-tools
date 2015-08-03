using UnityEngine;
using System.Collections;
using UnityEditor;

public class RBPhysics2D
{
	public static bool ShowCasts = true;

	public static Collider2D OverlapCircle (Vector2 center, float radius, int layerMask = Physics2D.DefaultRaycastLayers, 
	                                  float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity)
	{
		Collider2D hit = Physics2D.OverlapCircle (center, radius, layerMask, minDepth, maxDepth);

		Color drawColor = Color.green;
		if (hit != null) {
			drawColor = Color.red;

			DebugDrawCollider (hit);
		}

		DebugDrawCircle (center, radius, drawColor, 1.0f);

		return hit;
	}

	public static Collider2D[] OverlapCircleAll (Vector2 center, float radius, int layerMask = Physics2D.DefaultRaycastLayers, 
	                                             float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity)
	{
		Collider2D[] hits = Physics2D.OverlapCircleAll (center, radius, layerMask, minDepth, maxDepth);

		Color drawColor = Color.green;
		if (hits != null && hits.Length > 0) {
			drawColor = Color.red;

			DebugDrawColliders(hits);
		}
		DebugDrawCircle (center, radius, drawColor, 1.0f);

		return hits;
	}

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
			DebugDrawCircle(circleCollider.transform.position, circleCollider.radius, Color.yellow, 1.0f);
		}
	}

	static void DebugDrawCircle (Vector2 center, float radius, Color color, float duration)
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
}
