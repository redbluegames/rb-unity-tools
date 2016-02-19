using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RedBlue;

public class TestAttackCast2DSequence : MonoBehaviour
{
	[Header ("Expected Results")]
	public int
		ExpectedNumHits;
	[Header ("Configurations")]
	public List<Vector3>
		CastPositions;
	// [NotNull]
	public AttackCast2D
		AttackCast;
	[Header ("Output")]
	public List<GameObject>
		HitObjects;
	int currentPoint = 0;

	void Awake ()
	{
		// Subscribe to attacks
		AttackCast.Hit += HandleHit;
	}

	void HandleHit (object sender, AttackCast2D.HitEventArgs e)
	{
		HitObjects.Add (e.Hit.collider.gameObject);
	}

	void OnDrawGizmos ()
	{
		DrawDebugInfo ();
	}
	
	void DrawDebugInfo ()
	{
		if (CastPositions != null && CastPositions.Count > 0) {
			Vector3 currentPoint = transform.position + CastPositions [0];
			for (int i = 1; i < CastPositions.Count; i++) {
				Vector3 nextPoint = transform.position + CastPositions [i];
				DebugUtilities.DrawArrow (currentPoint, nextPoint, Color.cyan);
				currentPoint = nextPoint;
			}
		}
	}

	void Update ()
	{
		// When we reach the last point we are done
		if (currentPoint >= CastPositions.Count) {
			if (AttackCast.IsCasting) {
				AttackCast.End ();
				gameObject.SetActive (false);
				VerifyTest ();
			}
			return;
		}
		
		// Move to the appropriate point
		AttackCast.transform.position = transform.position + CastPositions [currentPoint];
		
		if (!AttackCast.IsCasting) {
			AttackCast.Begin ();
		}

		currentPoint++;
	}

	void VerifyTest ()
	{
		string testName = transform.root.gameObject.name;
		if (HitObjects.Count != ExpectedNumHits) {
			Debug.LogError ("Test Failed: " + testName
				+ "\nExpected " + ExpectedNumHits + " hits but received " + 
				HitObjects.Count, this);
		} else {
			Debug.Log ("Test Passed: " + testName, this);
		}
	}
}
