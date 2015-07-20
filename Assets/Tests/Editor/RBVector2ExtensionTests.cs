using NUnit.Framework;
using System;
using RedBlueTools;
using UnityEngine;

[TestFixture]
public class RBVector2ExtensionTests
{

	[Test]
	public void IsNormalized_ZeroVector_ReturnsFalse ()
	{
		// Arrange
		Vector2 zero = Vector2.zero;

		Assert.False (zero.IsNormalized (), "Zero vector should not be considered Normalized.");
	}

	[Test]
	public void IsNormalized_NonNormalized_ReturnsFalse ()
	{
		// Arrange
		Vector2 tooLarge = new Vector2 (1.0f, 1.0f);
		
		Assert.False (tooLarge.IsNormalized (), "Non-Unit Vector should not be considered Normalized.");
	}
	
	[Test]
	public void IsNormalized_UnitVector_ReturnsTrue ()
	{
		// Arrange
		Vector2 unit = Vector2.up;
		
		Assert.True (unit.IsNormalized (), "Unit Vector should be considered Normalized.");
	}

	[Test]
	public void RoundToArc_ZeroVector_RemainsZero ()
	{
		// Arrange
		Vector2 zero = Vector2.zero;

		// Act
		Vector2 roundedZero = zero.RoundToNearestArc (2, 0.0f);

		Assert.True (Mathf.Approximately (roundedZero.magnitude, 0.0f), 
		             "Magnitude of zero vector changed during rounding. Expected " + zero.magnitude + ", result was " + roundedZero.magnitude);
	}
	
	[Test]
	public void RoundToArc_TwoArcs_89DegreeRoundsToRight ()
	{
		// Arrange
		float angle = 89.0f;
		Vector2 oneDegree = new Vector2 (Mathf.Cos (angle * Mathf.Deg2Rad), Mathf.Sin (angle * Mathf.Deg2Rad));
		Vector2 right = Vector2.right;
		
		// Act
		Vector2 rounded = oneDegree.RoundToNearestArc (2);
		
		Assert.True (rounded.Equals (right), "Created: " + rounded + " Expected: " + right);
	}
	
	[Test]
	public void RoundToArc_TwoArcs_91DegreeRoundsToLeft ()
	{
		// Arrange
		float angle = 91.0f;
		Vector2 angleAsVector = new Vector2 (Mathf.Cos (angle * Mathf.Deg2Rad), Mathf.Sin (angle * Mathf.Deg2Rad));
		Vector2 left = -Vector2.right;
		
		// Act
		Vector2 rounded = angleAsVector.RoundToNearestArc (2);

		Assert.True (rounded.Equals (left), "Created: " + rounded + " Expected: " + left);
	}
	
	[Test]
	public void RoundToArc_TwoArcsWith90DegreeRotation_1DegreeRoundsToUp ()
	{
		// Arrange
		float angle = 1.0f;
		Vector2 angleAsVector = new Vector2 (Mathf.Cos (angle * Mathf.Deg2Rad), Mathf.Sin (angle * Mathf.Deg2Rad));
		Vector2 up = Vector2.up;
		
		// Act
		Vector2 rounded = angleAsVector.RoundToNearestArc (2, 90.0f);
		
		Assert.True (rounded.Equals (up), "Created: " + rounded + " Expected: " + up);
	}
	
	[Test]
	public void RoundToArc_FourArcs_Neg179DegreeRoundsToLeft ()
	{
		// Arrange
		float angle = -179.0f;
		Vector2 angleAsVector = new Vector2 (Mathf.Cos (angle * Mathf.Deg2Rad), Mathf.Sin (angle * Mathf.Deg2Rad));
		Vector2 left = -Vector2.right;
		
		// Act
		Vector2 rounded = angleAsVector.RoundToNearestArc (4);
		
		Assert.True (rounded.Equals (left), "Created: " + rounded + " Expected: " + left);
	}
	
	[Test]
	public void RoundToArc_FourArcsRotated45_179DegreesRoundsTopLeft ()
	{
		// Arrange
		float angle = 179.0f;
		Vector2 angleAsVector = new Vector2 (Mathf.Cos (angle * Mathf.Deg2Rad), Mathf.Sin (angle * Mathf.Deg2Rad));
		Vector2 topLeft = new Vector2 (Mathf.Cos (135 * Mathf.Deg2Rad), Mathf.Sin (135 * Mathf.Deg2Rad));
		
		// Act
		Vector2 rounded = angleAsVector.RoundToNearestArc (4, -45.0f);
		
		Assert.True (rounded.Equals (topLeft), "Created: " + rounded + " Expected: " + topLeft);
	}

	[Test]
	public void RotateByAngle_ZeroRotation_RemainsUnchanged ()
	{
		Vector2 upVector = Vector2.up;

		Vector2 upVectorRotatedByZero = upVector.RotateByRadians (0.0f);

		Assert.True (upVector == upVectorRotatedByZero, 
		             string.Format ("Vectors are not equal. Expected {0}, but was {1}", upVector, upVectorRotatedByZero));
	}

	[Test]
	public void RotateByAngle_360Rotation_RemainsUnchanged ()
	{
		Vector2 upVector = Vector2.up;
		Vector2 upVectorRotatedBy360 = upVector.RotateByRadians (2 * Mathf.PI);
		
		Assert.True (upVector == upVectorRotatedBy360, 
		             string.Format ("Vectors are not equal. Expected {0}, but was {1}", upVector, upVectorRotatedBy360));
	}

	[Test]
	public void RotateByAngle_BasicRotation_AsExpected ()
	{
		Vector2 diagonal = new Vector2 (1.0f, 1.0f);
		Vector2 expectedVector = new Vector2 (1.0f, 0.0f) * diagonal.magnitude;

		Vector2 diagonalRotatedBy45 = diagonal.RotateByRadians (45 * Mathf.Deg2Rad);

		Assert.True (diagonalRotatedBy45 == expectedVector, 
		             string.Format ("Vectors are not equal. Expected {0}, but was {1}", expectedVector, diagonalRotatedBy45));
	}
}
