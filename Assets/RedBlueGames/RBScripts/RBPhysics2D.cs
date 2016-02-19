using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RedBlue
{
	public class RBPhysics2D
	{
		public static Color HitColliderColor = Color.yellow;
		public static Color CastColor = Color.green;
		public static Color HitCastColor = Color.red;
		public static Color HitNormalsColor = Color.magenta;
	
		#region RayCast Wrapper
		public static RaycastHit2D LineCast (Vector2 start, Vector2 end, int layerMask = Physics2D.DefaultRaycastLayers, 
	                                     float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
		{
			RaycastHit2D hit = Physics2D.Linecast (start, end, layerMask, minDepth, maxDepth);
			DrawLineAndHits (new RaycastHit2D[] {hit}, start, end);
			return hit;
		}

		public static RaycastHit2D[] LineCastAll (Vector2 start, Vector2 end, int layerMask = Physics2D.DefaultRaycastLayers, 
	                                     float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
		{
			RaycastHit2D[] hits = Physics2D.LinecastAll (start, end, layerMask, minDepth, maxDepth);
			DrawLineAndHits (hits, start, end);
			return hits;
		}
		
		public static void LineCastNonAlloc (Vector2 start, Vector2 end, RaycastHit2D[] results,
		                 int layerMask = Physics2D.DefaultRaycastLayers, 
		                 float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
		{
			Physics2D.LinecastNonAlloc (start, end, results, layerMask, minDepth, maxDepth);
			DrawLineAndHits (results, start, end);
		}

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
		
		public static void RayCastNonAlloc (Vector2 origin, Vector2 direction, RaycastHit2D[] results,
		                                    float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers,
		                                    float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
		{
			Physics2D.RaycastNonAlloc (origin, direction, results, distance, layerMask, minDepth, maxDepth);
			DrawRayAndHits (results, origin, direction, distance);
		}

		static void DrawRayAndHits (RaycastHit2D[] hits, Vector2 origin, Vector2 direction, float distance = Mathf.Infinity)
		{
			float maxLineDistance = 100000.0f;
			float lineDrawDistance = Mathf.Min (distance, maxLineDistance);
			DrawLineAndHits (hits, origin, origin + (direction * lineDrawDistance));
		}

		static void DrawLineAndHits (RaycastHit2D[] hits, Vector2 origin, Vector2 endpoint)
		{
			DrawHitsForRaycasts (hits);

			Color drawColor = RaycastHitsContainHit (hits) ? HitCastColor : CastColor;
			DebugUtilities.DrawArrow (origin, endpoint, drawColor);
		}

		static bool RaycastHitsContainHit (RaycastHit2D[] hits)
		{
			if (hits != null && hits.Length > 0) {
				for (int i = 0; i < hits.Length; i++) {
					if (hits [i].collider != null) {
						return true;
					}
				}
			}
			return false;
		}
		
		static void DrawHitsForRaycasts (RaycastHit2D[] hits)
		{
			if (hits != null && hits.Length > 0) {
				for (int i = 0; i < hits.Length; i++) {
					// Draw each hit
					DrawRaycastHit2D (hits [i]);
				}
			}
		}
		
		static void DrawRaycastHit2D (RaycastHit2D hit, float size = 1.0f)
		{
			// Draw the hit collider
			if (hit.collider != null) {
				hit.collider.DebugDraw (HitColliderColor);
			}
			
			// Draw the normal at the hit location, or a circle for hits from rays originating inside collider
			bool isRayOriginatingFromInside = Mathf.Approximately (hit.fraction, 0.0f) && hit.collider != null;
			if (isRayOriginatingFromInside) {
				DebugUtilities.DrawCircle (hit.point, size, HitNormalsColor, 20);
			} else {
				Debug.DrawRay (hit.point, hit.normal * size, HitNormalsColor, 0.01f);
			}
		}
		#endregion
		
		#region CircleCast Wrapper
		public static RaycastHit2D CircleCast (Vector2 origin, float radius, Vector2 direction, 
		                                       float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, 
		                                       float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
		{
			RaycastHit2D hit = Physics2D.CircleCast (origin, radius, direction, distance, layerMask, minDepth, maxDepth);
			DrawCircleCastAndHits (new RaycastHit2D[] {hit}, origin, radius, direction, distance);
			return hit;
		}
		
		public static RaycastHit2D[] CircleCastAll (Vector2 origin, float radius, Vector2 direction, 
		                                            float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, 
		                                            float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
		{
			RaycastHit2D[] hits = Physics2D.CircleCastAll (origin, radius, direction, distance, layerMask, minDepth, maxDepth);
			DrawCircleCastAndHits (hits, origin, radius, direction, distance);
			return hits;
		}
		
		public static void CircleCastNonAlloc (Vector2 origin, float radius, Vector2 direction,
		                                       RaycastHit2D[] results, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, 
		                                       float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
		{
			Physics2D.CircleCastNonAlloc (origin, radius, direction, results, distance, layerMask, minDepth, maxDepth);
			DrawCircleCastAndHits (results, origin, radius, direction, distance);
		}
		
		static void DrawCircleCastAndHits (RaycastHit2D[] hits, Vector2 origin, float radius, Vector2 direction, float distance = Mathf.Infinity)
		{	
			// Nothing to draw with a 0 radius or 0 distance
			if (Mathf.Approximately (radius, 0.0f) || Mathf.Approximately (distance, 0.0f)) {
				return;
			}

			// Just in case direction isn't normalized, do it now
			if (!Mathf.Approximately (direction.sqrMagnitude, 1.0f)) {
				direction.Normalize ();
			}
			
			DrawHitsForCirclecast (hits, radius);
			
			Color drawColor = RaycastHitsContainHit (hits) ? HitCastColor : CastColor;
			
			// Draw origin and end circles
			DebugUtilities.DrawCircle (origin, radius, drawColor);
			Vector2 endCirclePosition = origin + (direction * distance);
			DebugUtilities.DrawCircle (endCirclePosition, radius, drawColor);
			
			// Draw edges
			Vector2 radiusSegment = (direction * radius);
			Vector2 orthoganalRadius = new Vector2 (radiusSegment.y, -radiusSegment.x);
			DebugUtilities.DrawArrow (origin + orthoganalRadius, endCirclePosition + orthoganalRadius, drawColor);
			DebugUtilities.DrawArrow (origin - orthoganalRadius, endCirclePosition - orthoganalRadius, drawColor);
		}
		
		static void DrawHitsForCirclecast (RaycastHit2D[] hits, float radius)
		{
			if (hits != null && hits.Length > 0) {
				for (int i = 0; i < hits.Length; i++) {
					// Draw a ray hit at each hit location
					DrawRaycastHit2D (hits [i], radius * 0.1f);
					// Draw centroid
					DebugUtilities.DrawCircle (hits [i].centroid, radius, HitNormalsColor);
				}
			}
		}
		#endregion

		#region OverlapAreaCast Wrapper
		public static Collider2D OverlapArea (Vector2 cornerA, Vector2 cornerB, int layerMask = Physics2D.DefaultRaycastLayers, 
	                         float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity)
		{
			Collider2D hit = Physics2D.OverlapArea (cornerA, cornerB, layerMask, minDepth, maxDepth);
			DrawBoxAndOverlaps (new Collider2D[] {hit}, cornerA, cornerB);

			return hit;
		}

		public static Collider2D[] OverlapAreaAll (Vector2 cornerA, Vector2 cornerB, int layerMask = Physics2D.DefaultRaycastLayers, 
	                                float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity)
		{
			Collider2D[] hits = Physics2D.OverlapAreaAll (cornerA, cornerB, layerMask, minDepth, maxDepth);
			DrawBoxAndOverlaps (hits, cornerA, cornerB);

			return hits;
		}

		static void DrawBoxAndOverlaps (Collider2D[] overlappingColliders, Vector2 cornerA, Vector2 cornerB)
		{
			bool overlapsExist = overlappingColliders != null && overlappingColliders.Length > 0;
			Color drawColor = overlapsExist ? HitCastColor : CastColor;
			DebugUtilities.DrawBox (cornerA, cornerB, drawColor);

			DebugDrawColliders (overlappingColliders, HitColliderColor);
		}
		#endregion
	
		#region OverlapCircleCast Wrapper
		public static Collider2D OverlapCircle (Vector2 center, float radius, int layerMask = Physics2D.DefaultRaycastLayers, 
	                                  float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity)
		{
			Collider2D hit = Physics2D.OverlapCircle (center, radius, layerMask, minDepth, maxDepth);
			DrawCircleAndOverlaps (new Collider2D[] {hit}, center, radius);

			return hit;
		}

		public static Collider2D[] OverlapCircleAll (Vector2 center, float radius, int layerMask = Physics2D.DefaultRaycastLayers, 
	                                             float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity)
		{
			Collider2D[] hits = Physics2D.OverlapCircleAll (center, radius, layerMask, minDepth, maxDepth);
			DrawCircleAndOverlaps (hits, center, radius);

			return hits;
		}

		static void DrawCircleAndOverlaps (Collider2D[] overlappingColliders, Vector2 center, float radius)
		{
			bool overlapsExist = overlappingColliders != null && overlappingColliders.Length > 0;
			Color drawColor = overlapsExist ? HitCastColor : CastColor;
			DebugUtilities.DrawCircle (center, radius, drawColor);

			DebugDrawColliders (overlappingColliders, HitColliderColor);
		}
		#endregion

		#region Debug Drawing
		static void DebugDrawColliders (Collider2D[] colliders, Color color)
		{
			for (int i = 0; i < colliders.Length; i++) {
				colliders [i].DebugDraw (color);
			}
		}
		#endregion
	}
}