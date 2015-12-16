﻿using UnityEngine;
using System.Collections;

namespace RedBlueTools
{
	public static class Vector2Extensions
	{
		public static bool IsNormalized (this Vector2 vector)
		{
			return Mathf.Approximately (vector.sqrMagnitude, 1.0f);
		}

		/// <summary>
		/// Determines if the vector is within the arc specified by a direction bisecting an angle.
		/// </summary>
		/// <returns><c>true</c> if the vector is within the arc the arc; otherwise, <c>false</c>.</returns>
		/// <param name="vector">Source Vector</param>
		/// <param name="arcBisector">Arc bisector</param>
		/// <param name="arcAngle">Arc angle</param>
		public static bool IsWithinArc(this Vector2 vector, Vector2 arcBisector, float arcAngle)
		{
			// Can't compare if source vector has no direction
			if (vector == Vector2.zero) {
				return false;
			}

			// Can't compare to an arc without a direction
			if (arcBisector == Vector2.zero) {
				return false;
			}

			// Can't be within a negative angle
			if (arcAngle < 0.0f) {
				return false;
			}

			float angle = Vector2.Angle (vector, arcBisector);
			return angle <= (arcAngle * 0.5f);
		}

		/// <summary>
		/// Rounds to nearest arc (with 4 arcs, all vectors that point from -45 to 45 return (1.0, 0.0))
		/// </summary>
		/// <returns>The bisecting vector for the arc</returns>
		/// <param name="vector">The vector to convert.</param>
		/// <param name="numArcs">The number of arcs to divide the circle into.</param>
		/// <param name="rotationDegrees">Optional rotation of arcs</param>
		public static Vector2 RoundToNearestArc(this Vector2 vector, int numArcs, float rotationDegrees = 0.0f)
		{
			int arc = vector.GetNearestArc(numArcs, rotationDegrees);
			Vector2 arcBisector = vector.GetBisectorForArc(arc, numArcs, rotationDegrees);

			// Reapply the original vector's magnitude
			return arcBisector * vector.magnitude;
		}

		static int GetNearestArc(this Vector2 vector, int numArcs, float rotationDegrees = 0.0f)
		{
			float angleRotated = Mathf.Atan2 (vector.y, vector.x) - (rotationDegrees * Mathf.Deg2Rad);
			float angleAsFractionOfCircle = angleRotated / (2 * Mathf.PI);
			float angleInArcs = (numArcs * angleAsFractionOfCircle);
			// Add a full circle and mod to handle -180 degree / 180 degree wraparound
			int arc = Mathf.RoundToInt((angleInArcs + numArcs) % numArcs);

			return arc;
		}
		
		static Vector2 GetBisectorForArc(this Vector2 vector, int arc, int numArcs, float rotationDegrees = 0.0f)
		{
			float arcsToRadians = (2 * Mathf.PI) / numArcs;
			float arcAngleRotated = (arc * arcsToRadians) + (rotationDegrees * Mathf.Deg2Rad);
			Vector2 arcBisector = new Vector2(Mathf.Cos (arcAngleRotated), Mathf.Sin (arcAngleRotated));
			
			// Workaround - for some reason, Mathf.Sin and Cos return values too big to be considered
			// zero by Mathf.Epsilon for cardinal angles.
			float customEpsilon = 0.000001f;
			if(arcBisector.x > -customEpsilon && arcBisector.x < customEpsilon) {
				arcBisector.x = 0.0f;
			}
			if(arcBisector.y > -customEpsilon && arcBisector.y < customEpsilon) {
				arcBisector.y = 0.0f;
			}

			return arcBisector;
		}

		/// <summary>
		/// Round the input vector to the closest cardinal direction and returns the rounded result.
		/// </summary>
		/// <returns>The nearest cardinal direction</returns>
		/// <param name="vector">Vector to round.</param>
		public static Vector2 RoundToCardinals(this Vector2 vectorToRound)
		{
			return vectorToRound.RoundToNearestArc(4);
		}

		/// <summary>
		/// If the supplied vector is within the supplied angle of a cardinal direction, this will
		/// return the cardinal direction. Otherwise it will just return the same angle.
		/// </summary>
		/// <returns>The cardinal direction within the specified bias.</returns>
		/// <param name="vector">Vector to bias.</param>
		/// <param name="biasAngle">Bias angle.</param>
		public static Vector2 BiasToCardinals (this Vector2 vectorToBias, float biasAngle)
		{
			if (biasAngle > 90.0f) {
				// When biasing by greater than quadrant, bias to nearest 90, not by first vector we test against.
				biasAngle = 90.0f;
			}
			// Can't bias by negative degrees
			if (biasAngle < 0.0f) {
				return vectorToBias;
			}
			Vector2 biasedVector = vectorToBias;
			float assistAngle = biasAngle;
			Vector2[] angles = new Vector2[]
			{
				Vector2.right,
				Vector2.up,
				-Vector2.right,
				-Vector2.up
			};
			foreach(Vector2 angle in angles) {
				if(vectorToBias.IsWithinArc(angle, assistAngle)) {
					biasedVector = angle;
					// Restore magnitude of original vector
					biasedVector *= vectorToBias.magnitude;
					break;
				}
			}

			return biasedVector;
		}

		/// <summary>
		/// Rotates the vector clockwise by an angle in radius
		/// </summary>
		/// <returns>The rotated vector.</returns>
		/// <param name="vector">Vector to rotate.</param>
		/// <param name="angle">Angle in radians</param>
		public static Vector2 RotateByRadians (this Vector2 vector, float angle)
		{
			float vectorAsAngle = Mathf.Atan2 (vector.y, vector.x);
			float combinedAngle = vectorAsAngle - angle;
			Vector2 rotatedVector = new Vector2 (Mathf.Cos (combinedAngle), Mathf.Sin (combinedAngle));
			rotatedVector *= vector.magnitude;
			return rotatedVector;
		}
	}
}
