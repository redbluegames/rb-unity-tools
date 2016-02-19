using UnityEngine;
using System.Collections;

namespace RedBlueGames.Tools
{
	public static class DebugUtilities
	{
		public static void DrawCircle (Vector2 center, float radius, Color color, float numSegments = 40, float duration = 0.01f)
		{	
			Quaternion rotQuaternion = Quaternion.AngleAxis (360.0f / numSegments, Vector3.forward);
			Vector2 vertexStart = new Vector2 (radius, 0.0f);
			for (int i = 0; i < numSegments; i++) {
				Vector2 rotatedPoint = rotQuaternion * vertexStart;
				// Draw the segment, shifted by the center
				Debug.DrawLine (center + vertexStart, center + rotatedPoint, color, duration);
			
				vertexStart = rotatedPoint;
			}
		}

		public static void DrawBox (Vector2 worldTopLeft, Vector2 worldBottomRight, Color color, float duration = 0.01f)
		{
			Vector2 worldTopRight = new Vector2 (worldBottomRight.x, worldTopLeft.y);
			Vector2 worldBottomLeft = new Vector2 (worldTopLeft.x, worldBottomRight.y);
		
			Debug.DrawLine (worldTopLeft, worldBottomLeft, color, duration);
			Debug.DrawLine (worldBottomLeft, worldBottomRight, color, duration);
			Debug.DrawLine (worldBottomRight, worldTopRight, color, duration);
			Debug.DrawLine (worldTopRight, worldTopLeft, color, duration);
		}

		public static void DrawEdges (Vector2[] worldPoints, Color color, float duration = 0.01f)
		{
			// Draw each segment except the last
			for (int i = 0; i < worldPoints.Length - 1; i++) {
				Vector3 nextPoint = worldPoints [i + 1];
				Vector3 currentPoint = worldPoints [i];
				Debug.DrawLine (currentPoint, nextPoint, color, duration);
			}
		}

		public static void DrawPolygon (Vector2[] worldPoints, Color color, float duration = 0.01f)
		{
			DrawEdges (worldPoints, color, duration);
			// Polygons are just edges with the first and last points connected
			if (worldPoints.Length > 1) {
				Debug.DrawLine (worldPoints [worldPoints.Length - 1], worldPoints [0], color, duration);
			}
		}

		public static void DrawArrow (Vector2 origin, Vector2 endpoint, Color color, float duration = 0.01f)
		{
			// Draw the line that makes up the body of the arrow
			Debug.DrawLine (origin, endpoint, color, 0.01f);
		
			// Draw arrowhead so we can see direction
			Vector2 arrowDirection = (endpoint - origin);
			DebugDrawArrowhead (endpoint, arrowDirection.normalized, GetArrowSizeForLine (arrowDirection), color, duration);
		}

		static float GetArrowSizeForLine (Vector2 line)
		{
			float defaultArrowPercentage = 0.05f;
			return (line * defaultArrowPercentage).magnitude;
		}

		static void DebugDrawArrowhead (Vector2 origin, Vector2 direction, float size, Color color, float duration = 0.01f, float theta = 30.0f)
		{
			// Theta angle is the acute angle of the arrow, so flip direction or else arrow will be pointing "backwards"
			Vector2 arrowheadHandle = -direction * size;
		
			Quaternion arrowRotationR = Quaternion.AngleAxis (theta, Vector3.forward);
			Vector2 arrowheadR = arrowRotationR * arrowheadHandle;
			Debug.DrawLine (origin, origin + arrowheadR, color, duration);
		
			Quaternion arrowRotationL = Quaternion.AngleAxis (-theta, Vector3.forward);
			Vector2 arrowheadL = arrowRotationL * arrowheadHandle;
			Debug.DrawLine (origin, origin + arrowheadL, color, duration);
		}
	}
}
