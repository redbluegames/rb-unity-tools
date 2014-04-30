using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Handles casting between this object and it's position in the previous frame. It then
 * sends a callback for each object hit.
 */
public class AttackCast : MonoBehaviour
{
	// GameObjects that show the two cast positions - start and end
	GameObject debugCurrentPosition;
	GameObject debugLastPosition;

	// Track last position of this game object
	Vector3 lastFramePosition;

	// A list of hit game objects so that we only report one OnHit per object
	List<GameObject> ignoreObjects = new List<GameObject> ();

	// The radius for this attack sphere
	public float radius;

	// The bits this attack should hit
	public LayerMask hitLayer;

	// Flags for showing debug information
	public bool debugShowCasts;
	public bool debugShowHits;

	// Store reference to attack that this cast is associated with
	public Attack attackInfo;

	void OnEnable ()
	{
		// TODO: Could support cast to source position, like I did in Ben10

		// Reinitialize previous position
		lastFramePosition = transform.position;

		// Clear previously hit objects so that we can re-hit them
		ignoreObjects.Clear ();
		ignoreObjects.Add (transform.root.gameObject);
	}

	/*
	 * Begin the attack sweep. Must be associated with attack information.
	 */
	public void Begin (Attack attack)
	{
		gameObject.SetActive (true);
		attackInfo = attack;
	}

	/*
	 * End the attack sweep.
	 */
	public void End ()
	{
		gameObject.SetActive (false);
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		Vector3 direction = (transform.position - lastFramePosition).normalized;
		float distance = Vector3.Distance (lastFramePosition, transform.position);
		RaycastHit[] hits;
		hits = Physics.SphereCastAll (lastFramePosition, radius, direction, distance, hitLayer);
		ReportHits (hits);
		RenderDebugCasts ();

		// Remember this position as the last
		lastFramePosition = transform.position;
	}

	void RenderDebugCasts ()
	{
		//TODO: Could wrap a precompile flag to never show debugs in release
		if (!debugShowCasts) {
			return;
		}

		// Spawn debug objects if they haven't been spawned
		if (debugCurrentPosition == null) {
			SpawnDebugObjects ();
		}

		// Update debug line. Wish this was a cylinder.
		Debug.DrawLine (transform.position, lastFramePosition);

		// Update gizmo, if we have one
		Gizmo gizmo = (Gizmo)GetComponent<Gizmo> ();
		if (gizmo != null) {
			gizmo.gizmoSize = radius;
			gizmo.enabled = debugShowCasts;
		}

		// Make sure spheres represent current cast radius as a diameter
		debugLastPosition.transform.localScale = (Vector3.one * (radius * 2));
		debugCurrentPosition.transform.localScale = (Vector3.one * (radius * 2));

		// Hide spheres if debugs are off
		debugLastPosition.SetActive (debugShowCasts);
		debugCurrentPosition.SetActive (debugShowCasts);

		// Set the new positions of the spheres
		debugLastPosition.transform.position = lastFramePosition;
		debugCurrentPosition.transform.position = transform.position;
	}

	/*
	 * Spawn debug objects used to see where the casts are located
	 */
	void SpawnDebugObjects ()
	{
		Material debugMaterial = new Material (Shader.Find ("Transparent/Diffuse"));
		debugCurrentPosition = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		debugCurrentPosition.transform.position = transform.position;
		debugCurrentPosition.renderer.material = debugMaterial;
		debugCurrentPosition.renderer.material.color = new Color (0, 1.0f, 0, .3f);
		debugCurrentPosition.collider.enabled = false;
		debugCurrentPosition.transform.parent = transform;
		debugCurrentPosition.SetActive (debugShowCasts);

		debugLastPosition = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		debugLastPosition.transform.position = lastFramePosition;
		debugLastPosition.renderer.material = debugMaterial;
		debugLastPosition.renderer.material.color = new Color (1.0f, 1.0f, 0, .3f);
		debugLastPosition.collider.enabled = false;
		debugLastPosition.transform.parent = transform;
		debugLastPosition.SetActive (debugShowCasts);
	}

	/*
	 * Send Hits to hit objects, from a Raycast hit array.
	 */
	void ReportHits (RaycastHit[] hits)
	{
		foreach (RaycastHit hit in hits) {
			OnHit (hit);
		}
	}

	/*
	 * Apply the OnHit function to a specified Raycast hit. This should be made project-agnostic
	 * at some point, so that AttackCasts can be a tool.
	 */
	void OnHit (RaycastHit hit)
	{
		// Throw out iIgnored objects
		if (ignoreObjects.Contains (hit.collider.transform.root.gameObject)) {
			return;
		}

		if (debugShowHits) {
			Debug.DrawRay (hit.point, hit.normal, Color.red, 0.5f);
		}

		GameObject hitGameObject = hit.collider.gameObject;
		ignoreObjects.Add (hitGameObject);

		Fighter hitFighter = (Fighter)hitGameObject.GetComponent<Fighter> ();
		Fighter myFighter = (Fighter)transform.root.gameObject.GetComponent<Fighter> ();

		if (hitFighter != null) {
			if (myFighter != null) {
				// No friendly fire, for now
				if (hitFighter.team != myFighter.team) {
					hitFighter.TakeHit (hit, attackInfo, transform.root);
					myFighter.NotifyAttackHit ();
				}
			}
		}
		Debug.Log ("Hit! Object: " + hitGameObject.name);
	}

}
