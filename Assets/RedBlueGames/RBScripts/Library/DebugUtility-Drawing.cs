namespace RedBlueGames.Tools
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Contains utilities and "Extension methods" to the Debug class, specifically for Drawing
    /// </summary>
    public static partial class DebugUtility
    {
        /// <summary>
        /// Draws a Circle using Debug.Draw functions
        /// </summary>
        /// <param name="center">Center point.</param>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="color">Color for Debug.Draw.</param>
        /// <param name="numSegments">Number of segments for the circle, used for precision of the draw.</param>
        /// <param name="duration">Duration to show the circle.</param>
        public static void DrawCircle(Vector2 center, float radius, Color color, float numSegments = 40, float duration = 0.01f)
        {
            Quaternion rotQuaternion = Quaternion.AngleAxis(360.0f / numSegments, Vector3.forward);
            Vector2 vertexStart = new Vector2(radius, 0.0f);
            for (int i = 0; i < numSegments; i++)
            {
                Vector2 rotatedPoint = rotQuaternion * vertexStart;

                // Draw the segment, shifted by the center
                Debug.DrawLine(center + vertexStart, center + rotatedPoint, color, duration);

                vertexStart = rotatedPoint;
            }
        }

        /// <summary>
        /// Draws a box using Debug.Draw functions
        /// </summary>
        /// <param name="worldTopLeft">World top left corner.</param>
        /// <param name="worldBottomRight">World bottom right corner.</param>
        /// <param name="color">Color for Debug.Draw.</param>
        /// <param name="duration">Duration to show the box.</param>
        public static void DrawBox(Vector2 worldTopLeft, Vector2 worldBottomRight, Color color, float duration = 0.01f)
        {
            Vector2 worldTopRight = new Vector2(worldBottomRight.x, worldTopLeft.y);
            Vector2 worldBottomLeft = new Vector2(worldTopLeft.x, worldBottomRight.y);

            Debug.DrawLine(worldTopLeft, worldBottomLeft, color, duration);
            Debug.DrawLine(worldBottomLeft, worldBottomRight, color, duration);
            Debug.DrawLine(worldBottomRight, worldTopRight, color, duration);
            Debug.DrawLine(worldTopRight, worldTopLeft, color, duration);
        }

        /// <summary>
        /// Draws an array of edges, where an edge is defined by two Vector2 points, using Debug.Draw
        /// </summary>
        /// <param name="worldPoints">World points, defining each vertex of an edge in world space.</param>
        /// <param name="color">Color for Debug.Draw.</param>
        /// <param name="duration">Duration to show the edges.</param>
        public static void DrawEdges(Vector2[] worldPoints, Color color, float duration = 0.01f)
        {
            // Draw each segment except the last
            for (int i = 0; i < worldPoints.Length - 1; i++)
            {
                Vector3 nextPoint = worldPoints[i + 1];
                Vector3 currentPoint = worldPoints[i];
                Debug.DrawLine(currentPoint, nextPoint, color, duration);
            }
        }

        /// <summary>
        /// Draws a polygon, defined by all verteces of the polygon, using Debug.Draw
        /// </summary>
        /// <param name="worldPoints">World points, defining each vertex of the polygon in world space.</param>
        /// <param name="color">Color for Debug.Draw.</param>
        /// <param name="duration">Duration to show the polygon.</param>
        public static void DrawPolygon(Vector2[] worldPoints, Color color, float duration = 0.01f)
        {
            DrawEdges(worldPoints, color, duration);

            // Polygons are just edges with the first and last points connected
            if (worldPoints.Length > 1)
            {
                Debug.DrawLine(worldPoints[worldPoints.Length - 1], worldPoints[0], color, duration);
            }
        }

        /// <summary>
        /// Draws an arrow using Debug.Draw
        /// </summary>
        /// <param name="origin">Origin point in world space.</param>
        /// <param name="endpoint">Endpoint in world space.</param>
        /// <param name="color">Color for Debug.Draw.</param>
        /// <param name="duration">Duration to show the arrow.</param>
        public static void DrawArrow(Vector2 origin, Vector2 endpoint, Color color, float duration = 0.01f)
        {
            // Draw the line that makes up the body of the arrow
            Debug.DrawLine(origin, endpoint, color, 0.01f);

            // Draw arrowhead so we can see direction
            Vector2 arrowDirection = endpoint - origin;
            DebugDrawArrowhead(endpoint, arrowDirection.normalized, GetArrowSizeForLine(arrowDirection), color, duration);
        }

        private static float GetArrowSizeForLine(Vector2 line)
        {
            float defaultArrowPercentage = 0.05f;
            return (line * defaultArrowPercentage).magnitude;
        }

        private static void DebugDrawArrowhead(Vector2 origin, Vector2 direction, float size, Color color, float duration = 0.01f, float theta = 30.0f)
        {
            // Theta angle is the acute angle of the arrow, so flip direction or else arrow will be pointing "backwards"
            Vector2 arrowheadHandle = -direction * size;

            Quaternion arrowRotationR = Quaternion.AngleAxis(theta, Vector3.forward);
            Vector2 arrowheadR = arrowRotationR * arrowheadHandle;
            Debug.DrawLine(origin, origin + arrowheadR, color, duration);

            Quaternion arrowRotationL = Quaternion.AngleAxis(-theta, Vector3.forward);
            Vector2 arrowheadL = arrowRotationL * arrowheadHandle;
            Debug.DrawLine(origin, origin + arrowheadL, color, duration);
        }
    }
}
