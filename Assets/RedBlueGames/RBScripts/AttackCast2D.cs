using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using RedBlue;

/// <summary>
/// Handles casting between this object and its position in the previous frame. 
/// It then sends a callback for each object hit.
/// </summary>
public class AttackCast2D : MonoBehaviour
{
	//=========================================================================
	// Enumerations, Structs, Events
	//=========================================================================

	public class HitEventArgs : EventArgs
	{
		public RaycastHit2D Hit;

		public HitEventArgs (RaycastHit2D hit)
		{
			this.Hit = hit;
		}
	}

	/// <summary>
	/// Occurs when the attack hits a collider
	/// </summary>
	public event EventHandler<HitEventArgs> Hit;

	//=========================================================================
	// Properties, Fields, Consts
	//=========================================================================

	public bool IsCasting { get; private set; }

	[Tooltip( "Radius of the attack circle" )]
	public float
		radius;
	[Tooltip( "Layers the attack casts against" )]
	public LayerMask
		hitLayer;
	[Tooltip( "The \"Originator\" of the cast who will be ignored by the attack." )]
	public GameObject
		Originator;
	private     Vector3             lastFramePosition;                                  // Track last position of this game object
	private     List<GameObject>    ignoreObjects = new List<GameObject> ();             // A list of hit game objects so that we only report one OnHit per object

	//=========================================================================
	// Methods
	//=========================================================================

	#region Attack Casting
	/// <summary>
	/// Begin the attack sweep
	/// </summary>
	public void Begin ()
	{
		if (IsCasting) {
			Debug.LogWarning ("AttackCast: Tried to Begin attack when attack is " +
				"already in progress. Ignoring.");
			return;
		}
		IsCasting = true;

		// Initialize previous position to the current attack position
		lastFramePosition = transform.position;

		// Always ignore Originator
		if (Originator != null) {
			// TODO: Consider ignoring all colliders in Originator's hierarchy
			ignoreObjects.Add (Originator);
		}

		StartCoroutine ("CastForHits");
	}

	/// <summary>
	/// End the attack sweep
	/// </summary>
	public void End ()
	{
		StopCoroutine ("CastForHits");

		IsCasting = false;

		// Cleanup the Attack
		ignoreObjects.Clear ();
	}

	IEnumerator CastForHits ()
	{
		while (true) {
			Vector3 direction = (transform.position - lastFramePosition);
			float distance = direction.magnitude;
			direction.Normalize ();
			RaycastHit2D[] hits;
			hits = RBPhysics2D.CircleCastAll (lastFramePosition, radius, direction, distance, hitLayer);
			ReportHits (hits);

			// Remember this position as the last
			lastFramePosition = transform.position;

			yield return null;
		}
	}
	#endregion

	#region Hit Reporting
	/// <summary>
	/// Send Hits to hit objects, from a Raycast hit array.
	/// </summary>
	/// <param name="hits">Hits.</param>
	void ReportHits (RaycastHit2D[] hits)
	{
		foreach (RaycastHit2D hit in hits) {
			ReportHit (hit);
		}
	}

	/// <summary>
	/// Apply the OnHit function to a specified Raycast hit. This should be made project-agnostic
	/// at some point, so that AttackCasts can be a tool.
	/// </summary>
	/// <param name="hit">Hit.</param>
	void ReportHit (RaycastHit2D hit)
	{
		// Throw out ignored objects
		if (ignoreObjects.Contains (GetHitObject (hit))) {
			return;
		}

		// Ignore before OnHit or else ignoredObjects might be added to list after attack is canceled.
		ignoreObjects.Add (GetHitObject (hit));

		OnHit (new HitEventArgs (hit));
	}

	GameObject GetHitObject (RaycastHit2D hit)
	{
		if (hit.collider == null) {
			return null;
		}

		return hit.collider.gameObject;
	}

	void OnHit (HitEventArgs args)
	{
		if (Hit != null) {
			Hit (this, args);
		}
	}
	#endregion

	#region Debugging
	void OnDrawGizmosSelected ()
	{
		// Let the casting debug draw when casting
		if (IsCasting) {
			return;
		}
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere (transform.position, radius);
	}
	#endregion

	//=========================================================================
	// Message Handlers, States
	//=========================================================================

}