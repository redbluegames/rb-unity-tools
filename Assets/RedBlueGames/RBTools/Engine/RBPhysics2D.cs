namespace RedBlueGames.Tools
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// RBPhysics2D wraps 2D physics calls with Debug calls for drawing the casts in the Editor
    /// </summary>
    public class RBPhysics2D
    {
        static RBPhysics2D()
        {
            HitColliderColor = Color.yellow;
            CastColor = Color.green;
            HitCastColor = Color.red;
            HitNormalsColor = Color.magenta;
        }

        private static Color HitColliderColor { get; set; }

        private static Color CastColor { get; set; }

        private static Color HitCastColor { get; set; }

        private static Color HitNormalsColor { get; set; }

        /// <summary>
        /// Perform a LineCast, with debug drawing. Gets the first hit.
        /// </summary>
        /// <returns>Information for the first hit along the line.</returns>
        /// <param name="start">Start point, in 2D world space.</param>
        /// <param name="end">End point, in 2D world space.</param>
        /// <param name="layerMask">Layer mask to hit.</param>
        /// <param name="minDepth">Minimum depth - hit items with at least this z.</param>
        /// <param name="maxDepth">Max depth - hit items with at most this z.</param>
        public static RaycastHit2D LineCast(
            Vector2 start,
            Vector2 end,
            int layerMask = Physics2D.DefaultRaycastLayers,
            float minDepth = Mathf.NegativeInfinity,
            float maxDepth = Mathf.Infinity)
        {
            RaycastHit2D hit = Physics2D.Linecast(start, end, layerMask, minDepth, maxDepth);
            DrawLineAndHits(new RaycastHit2D[] { hit }, start, end);
            return hit;
        }

        /// <summary>
        /// Perform a LineCast, with debug drawing. Gets all hits along the line
        /// </summary>
        /// <returns>All the cast hit information.</returns>
        /// <param name="start">Start point, in 2D world space.</param>
        /// <param name="end">End point, in 2D world space.</param>
        /// <param name="layerMask">Layer mask to hit.</param>
        /// <param name="minDepth">Minimum depth - hit items with at least this z.</param>
        /// <param name="maxDepth">Max depth - hit items with at most this z.</param>
        public static RaycastHit2D[] LineCastAll(
            Vector2 start,
            Vector2 end,
            int layerMask = Physics2D.DefaultRaycastLayers,
            float minDepth = Mathf.NegativeInfinity,
            float maxDepth = Mathf.Infinity)
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll(start, end, layerMask, minDepth, maxDepth);
            DrawLineAndHits(hits, start, end);
            return hits;
        }

        /// <summary>
        /// Perform a LineCast, wait debug drawing. Uses a supplied array to prevent allocation.
        /// </summary>
        /// <param name="start">Start point in 2D world space.</param>
        /// <param name="end">End point in 2D world space.</param>
        /// <param name="results">Array to store the results in.</param>
        /// <param name="layerMask">Layer mask to hit.</param>
        /// <param name="minDepth">Minimum depth - hit items with at least this z.</param>
        /// <param name="maxDepth">Max depth - hit items with at most this z.</param>
        public static void LineCastNonAlloc(
            Vector2 start,
            Vector2 end,
            RaycastHit2D[] results,
            int layerMask = Physics2D.DefaultRaycastLayers,
            float minDepth = Mathf.NegativeInfinity,
            float maxDepth = Mathf.Infinity)
        {
            Physics2D.LinecastNonAlloc(start, end, results, layerMask, minDepth, maxDepth);
            DrawLineAndHits(results, start, end);
        }

        /// <summary>
        /// Casts a ray against scene colliders, with debug drawing information. Returns the first hit.
        /// </summary>
        /// <returns>The first hit in the cast.</returns>
        /// <param name="origin">Origin for the ray in 2D world space.</param>
        /// <param name="direction">Direction as a 2D vector.</param>
        /// <param name="distance">Distance for the raycast.</param>
        /// <param name="layerMask">Layer mask to hit.</param>
        /// <param name="minDepth">Minimum depth - hit items with at least this z.</param>
        /// <param name="maxDepth">Max depth - hit items with at most this z.</param>
        public static RaycastHit2D RayCast(
            Vector2 origin,
            Vector2 direction,
            float distance = Mathf.Infinity,
            int layerMask = Physics2D.DefaultRaycastLayers,
            float minDepth = Mathf.NegativeInfinity,
            float maxDepth = Mathf.Infinity)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask, minDepth, maxDepth);
            DrawRayAndHits(new RaycastHit2D[] { hit }, origin, direction, distance);
            return hit;
        }

        /// <summary>
        /// Casts a ray against scene colliders, with debug drawing information. Returns all hit along the cast.
        /// </summary>
        /// <returns>All hits along the cast.</returns>
        /// <param name="origin">Origin for the ray in 2D world space.</param>
        /// <param name="direction">Direction as a 2D vector.</param>
        /// <param name="distance">Distance for the raycast.</param>
        /// <param name="layerMask">Layer mask to hit.</param>
        /// <param name="minDepth">Minimum depth - hit items with at least this z.</param>
        /// <param name="maxDepth">Max depth - hit items with at most this z.</param>
        public static RaycastHit2D[] RayCastAll(
            Vector2 origin,
            Vector2 direction,
            float distance = Mathf.Infinity,
            int layerMask = Physics2D.DefaultRaycastLayers,
            float minDepth = Mathf.NegativeInfinity,
            float maxDepth = Mathf.Infinity)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance, layerMask, minDepth, maxDepth);
            DrawRayAndHits(hits, origin, direction, distance);
            return hits;
        }

        /// <summary>
        /// Casts a ray against scene colliders, with debug drawing information. Passes results into a supplied array in order
        /// to minimize allocations
        /// </summary>
        /// <param name="origin">Origin for the ray in 2D world space.</param>
        /// <param name="direction">Direction as a 2D vector.</param>
        /// <param name="results">Array to store resulting hits in.</param>
        /// <param name="distance">Distance for the raycast.</param>
        /// <param name="layerMask">Layer mask to hit.</param>
        /// <param name="minDepth">Minimum depth - hit items with at least this z.</param>
        /// <param name="maxDepth">Max depth - hit items with at most this z.</param>
        public static void RayCastNonAlloc(
            Vector2 origin,
            Vector2 direction,
            RaycastHit2D[] results,
            float distance = Mathf.Infinity,
            int layerMask = Physics2D.DefaultRaycastLayers,
            float minDepth = Mathf.NegativeInfinity,
            float maxDepth = Mathf.Infinity)
        {
            Physics2D.RaycastNonAlloc(origin, direction, results, distance, layerMask, minDepth, maxDepth);
            DrawRayAndHits(results, origin, direction, distance);
        }

        /// <summary>
        /// Casts a circle against colliders in the scene, with debug drawing information. Returns the first hit information.
        /// </summary>
        /// <returns>Hit information for the first hit along the cast.</returns>
        /// <param name="origin">Origin point in 2D.</param>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="direction">Direction to "drag" the circle.</param>
        /// <param name="distance">Distance for the cast.</param>
        /// <param name="layerMask">Layer mask to hit.</param>
        /// <param name="minDepth">Minimum depth - hit items with at least this z.</param>
        /// <param name="maxDepth">Max depth - hit items with at most this z.</param>
        public static RaycastHit2D CircleCast(
            Vector2 origin,
            float radius,
            Vector2 direction,
            float distance = Mathf.Infinity,
            int layerMask = Physics2D.DefaultRaycastLayers,
            float minDepth = Mathf.NegativeInfinity,
            float maxDepth = Mathf.Infinity)
        {
            RaycastHit2D hit = Physics2D.CircleCast(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
            DrawCircleCastAndHits(new RaycastHit2D[] { hit }, origin, radius, direction, distance);
            return hit;
        }

        /// <summary>
        /// Casts a circle against colliders in the scene, with debug drawing information. Returns all the hits along the cast.
        /// </summary>
        /// <returns>All hits along the cast.</returns>
        /// <param name="origin">Origin point in 2D.</param>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="direction">Direction to "drag" the circle.</param>
        /// <param name="distance">Distance for the cast.</param>
        /// <param name="layerMask">Layer mask to hit.</param>
        /// <param name="minDepth">Minimum depth - hit items with at least this z.</param>
        /// <param name="maxDepth">Max depth - hit items with at most this z.</param>
        public static RaycastHit2D[] CircleCastAll(
            Vector2 origin,
            float radius,
            Vector2 direction,
            float distance = Mathf.Infinity, 
            int layerMask = Physics2D.DefaultRaycastLayers,
            float minDepth = Mathf.NegativeInfinity, 
            float maxDepth = Mathf.Infinity)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
            DrawCircleCastAndHits(hits, origin, radius, direction, distance);
            return hits;
        }

        /// <summary>
        /// Casts a circle against colliders in the scene, with debug drawing information. Passes results into a supplied
        /// array to minimize allocations.
        /// </summary>
        /// <param name="origin">Origin point in 2D.</param>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="direction">Direction to "drag" the circle.</param>
        /// <param name="results">Array to store the resulting hits in.</param>
        /// <param name="distance">Distance for the cast.</param>
        /// <param name="layerMask">Layer mask to hit.</param>
        /// <param name="minDepth">Minimum depth - hit items with at least this z.</param>
        /// <param name="maxDepth">Max depth - hit items with at most this z.</param>
        public static void CircleCastNonAlloc(
            Vector2 origin,
            float radius,
            Vector2 direction,
            RaycastHit2D[] results,
            float distance = Mathf.Infinity,
            int layerMask = Physics2D.DefaultRaycastLayers, 
            float minDepth = Mathf.NegativeInfinity,
            float maxDepth = Mathf.Infinity)
        {
            Physics2D.CircleCastNonAlloc(origin, radius, direction, results, distance, layerMask, minDepth, maxDepth);
            DrawCircleCastAndHits(results, origin, radius, direction, distance);
        }

        /// <summary>
        /// Checks if any collider lies inside a rectangle. Returns the collider with the lowest z value (I think...)
        /// </summary>
        /// <returns>The hit information for the first hit found.</returns>
        /// <param name="cornerA">One corner for the Area</param>
        /// <param name="cornerB">Opposite corner for the Area</param>
        /// <param name="layerMask">Layer mask for colliders to check.</param>
        /// <param name="minDepth">Minimum depth - hit items with at least this z.</param>
        /// <param name="maxDepth">Max depth - hit items with at most this z.</param>
        public static Collider2D OverlapArea(
            Vector2 cornerA,
            Vector2 cornerB,
            int layerMask = Physics2D.DefaultRaycastLayers,
            float minDepth = -Mathf.Infinity,
            float maxDepth = Mathf.Infinity)
        {
            Collider2D hit = Physics2D.OverlapArea(cornerA, cornerB, layerMask, minDepth, maxDepth);
            DrawBoxAndOverlaps(new Collider2D[] { hit }, cornerA, cornerB);

            return hit;
        }

        /// <summary>
        /// Checks if any collider lies inside a rectangle. Returns all colliders found
        /// </summary>
        /// <returns>All hit information for overlapping colliders.</returns>
        /// <param name="cornerA">One corner for the Area</param>
        /// <param name="cornerB">Opposite corner for the Area</param>
        /// <param name="layerMask">Layer mask for colliders to check.</param>
        /// <param name="minDepth">Minimum depth - hit items with at least this z.</param>
        /// <param name="maxDepth">Max depth - hit items with at most this z.</param>
        public static Collider2D[] OverlapAreaAll(
            Vector2 cornerA, 
            Vector2 cornerB, 
            int layerMask = Physics2D.DefaultRaycastLayers, 
            float minDepth = -Mathf.Infinity, 
            float maxDepth = Mathf.Infinity)
        {
            Collider2D[] hits = Physics2D.OverlapAreaAll(cornerA, cornerB, layerMask, minDepth, maxDepth);
            DrawBoxAndOverlaps(hits, cornerA, cornerB);

            return hits;
        }

        /// <summary>
        /// Checks if any collider lies inside a circle. Returns the collider with the lowest z value (I think...)
        /// </summary>
        /// <returns>Hit information for the first found hit.</returns>
        /// <param name="center">Center for the circle in 2D world space.</param>
        /// <param name="radius">Radius for the circle.</param>
        /// <param name="layerMask">Layer mask for colliders to check.</param>
        /// <param name="minDepth">Minimum depth - hit items with at least this z.</param>
        /// <param name="maxDepth">Max depth - hit items with at most this z.</param>
        public static Collider2D OverlapCircle(
            Vector2 center,
            float radius,
            int layerMask = Physics2D.DefaultRaycastLayers, 
            float minDepth = -Mathf.Infinity,
            float maxDepth = Mathf.Infinity)
        {
            Collider2D hit = Physics2D.OverlapCircle(center, radius, layerMask, minDepth, maxDepth);
            DrawCircleAndOverlaps(new Collider2D[] { hit }, center, radius);

            return hit;
        }

        /// <summary>
        /// Checks if any collider lies inside a circle. Returns all overlapping colliders
        /// </summary>
        /// <returns>Hit information for all overlapping colliders</returns>
        /// <param name="center">Center for the circle in 2D world space.</param>
        /// <param name="radius">Radius for the circle.</param>
        /// <param name="layerMask">Layer mask for colliders to check.</param>
        /// <param name="minDepth">Minimum depth - hit items with at least this z.</param>
        /// <param name="maxDepth">Max depth - hit items with at most this z.</param>
        public static Collider2D[] OverlapCircleAll(
            Vector2 center,
            float radius,
            int layerMask = Physics2D.DefaultRaycastLayers, 
            float minDepth = -Mathf.Infinity, 
            float maxDepth = Mathf.Infinity)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, layerMask, minDepth, maxDepth);
            DrawCircleAndOverlaps(hits, center, radius);

            return hits;
        }

        private static void DrawRayAndHits(RaycastHit2D[] hits, Vector2 origin, Vector2 direction, float distance = Mathf.Infinity)
        {
            float maxLineDistance = 100000.0f;
            float lineDrawDistance = Mathf.Min(distance, maxLineDistance);
            DrawLineAndHits(hits, origin, origin + (direction * lineDrawDistance));
        }

        private static void DrawLineAndHits(RaycastHit2D[] hits, Vector2 origin, Vector2 endpoint)
        {
            DrawHitsForRaycasts(hits);

            Color drawColor = RaycastHitsContainHit(hits) ? HitCastColor : CastColor;
            DebugUtility.DrawArrow(origin, endpoint, drawColor);
        }

        private static bool RaycastHitsContainHit(RaycastHit2D[] hits)
        {
            if (hits != null && hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static void DrawHitsForRaycasts(RaycastHit2D[] hits)
        {
            if (hits != null && hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    // Draw each hit
                    DrawRaycastHit2D(hits[i]);
                }
            }
        }

        private static void DrawRaycastHit2D(RaycastHit2D hit, float size = 1.0f)
        {
            // Draw the hit collider
            if (hit.collider != null)
            {
                hit.collider.DebugDraw(HitColliderColor);
            }

            // Draw the normal at the hit location, or a circle for hits from rays originating inside collider
            bool isRayOriginatingFromInside = Mathf.Approximately(hit.fraction, 0.0f) && hit.collider != null;
            if (isRayOriginatingFromInside)
            {
                DebugUtility.DrawCircle(hit.point, size, HitNormalsColor, 20);
            }
            else
            {
                Debug.DrawRay(hit.point, hit.normal * size, HitNormalsColor, 0.01f);
            }
        }

        private static void DrawCircleCastAndHits(
            RaycastHit2D[] hits,
            Vector2 origin,
            float radius,
            Vector2 direction,
            float distance = Mathf.Infinity)
        {
            // Nothing to draw with a 0 radius or 0 distance
            if (Mathf.Approximately(radius, 0.0f) || Mathf.Approximately(distance, 0.0f))
            {
                return;
            }

            // Just in case direction isn't normalized, do it now
            if (!Mathf.Approximately(direction.sqrMagnitude, 1.0f))
            {
                direction.Normalize();
            }

            DrawHitsForCirclecast(hits, radius);

            Color drawColor = RaycastHitsContainHit(hits) ? HitCastColor : CastColor;

            // Draw origin and end circles
            DebugUtility.DrawCircle(origin, radius, drawColor);
            Vector2 endCirclePosition = origin + (direction * distance);
            DebugUtility.DrawCircle(endCirclePosition, radius, drawColor);

            // Draw edges
            Vector2 radiusSegment = direction * radius;
            Vector2 orthoganalRadius = new Vector2(radiusSegment.y, -radiusSegment.x);
            DebugUtility.DrawArrow(origin + orthoganalRadius, endCirclePosition + orthoganalRadius, drawColor);
            DebugUtility.DrawArrow(origin - orthoganalRadius, endCirclePosition - orthoganalRadius, drawColor);
        }

        private static void DrawHitsForCirclecast(RaycastHit2D[] hits, float radius)
        {
            if (hits != null && hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    // Draw a ray hit at each hit location
                    DrawRaycastHit2D(hits[i], radius * 0.1f);

                    // Draw centroid
                    DebugUtility.DrawCircle(hits[i].centroid, radius, HitNormalsColor);
                }
            }
        }

        private static void DrawBoxAndOverlaps(Collider2D[] overlappingColliders, Vector2 cornerA, Vector2 cornerB)
        {
            bool overlapsExist = overlappingColliders != null && overlappingColliders.Length > 0;
            Color drawColor = overlapsExist ? HitCastColor : CastColor;
            DebugUtility.DrawBox(cornerA, cornerB, drawColor);

            DebugDrawColliders(overlappingColliders, HitColliderColor);
        }

        private static void DrawCircleAndOverlaps(Collider2D[] overlappingColliders, Vector2 center, float radius)
        {
            bool overlapsExist = overlappingColliders != null && overlappingColliders.Length > 0;
            Color drawColor = overlapsExist ? HitCastColor : CastColor;
            DebugUtility.DrawCircle(center, radius, drawColor);

            DebugDrawColliders(overlappingColliders, HitColliderColor);
        }

        private static void DebugDrawColliders(Collider2D[] colliders, Color color)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].DebugDraw(color);
            }
        }
    }
}