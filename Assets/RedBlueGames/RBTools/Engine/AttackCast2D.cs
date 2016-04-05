namespace RedBlueGames.Tools
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Handles casting between this object and its position in the previous frame. 
    /// It then sends a callback for each object hit.
    /// </summary>
    public class AttackCast2D : MonoBehaviour
    {
        [Tooltip("Radius of the attack circle")]
        [SerializeField]
        private float radius;

        [Tooltip("Layers the attack casts against")]
        [SerializeField]
        private LayerMask hitLayer;

        [Tooltip("The \"Originator\" of the cast who will be ignored by the attack.")]
        private GameObject originator;
        private Vector3 lastFramePosition;

        // Track last position of this game object
        // A list of hit game objects so that we only report one OnHit per object
        private List<GameObject> ignoreObjects = new List<GameObject>();

        /// <summary>
        /// Occurs when the attack hits a collider
        /// </summary>
        public event EventHandler<HitEventArgs> Hit;

        /// <summary>
        /// Gets a value indicating whether this instance is currently casting for hits.
        /// </summary>
        /// <value><c>true</c> if this instance is casting; otherwise, <c>false</c>.</value>
        public bool IsCasting { get; private set; }

        /// <summary>
        /// Begin the attack sweep
        /// </summary>
        public void Begin()
        {
            if (this.IsCasting)
            {
                Debug.LogWarning("AttackCast: Tried to Begin attack when attack is " +
                    "already in progress. Ignoring.");
                return;
            }

            this.IsCasting = true;

            // Initialize previous position to the current attack position
            this.lastFramePosition = transform.position;

            // Always ignore Originator
            if (this.originator != null)
            {
                // TODO: Consider ignoring all colliders in Originator's hierarchy
                this.ignoreObjects.Add(this.originator);
            }

            this.StartCoroutine("CastForHits");
        }

        /// <summary>
        /// End the attack sweep
        /// </summary>
        public void End()
        {
            this.StopCoroutine("CastForHits");

            this.IsCasting = false;

            // Cleanup the Attack
            this.ignoreObjects.Clear();
        }

        private IEnumerator CastForHits()
        {
            while (true)
            {
                Vector3 direction = transform.position - this.lastFramePosition;
                float distance = direction.magnitude;
                direction.Normalize();
                RaycastHit2D[] hits;
                hits = RBPhysics2D.CircleCastAll(this.lastFramePosition, this.radius, direction, distance, this.hitLayer);
                this.ReportHits(hits);

                // Remember this position as the last
                this.lastFramePosition = transform.position;

                yield return null;
            }
        }

        /// <summary>
        /// Send Hits to hit objects, from a Raycast hit array.
        /// </summary>
        /// <param name="hits">Hits to report</param>
        private void ReportHits(RaycastHit2D[] hits)
        {
            foreach (RaycastHit2D hit in hits)
            {
                this.ReportHit(hit);
            }
        }

        /// <summary>
        /// Apply the OnHit function to a specified Raycast hit. This should be made project-agnostic
        /// at some point, so that AttackCasts can be a tool.
        /// </summary>
        /// <param name="hit">Single hit to report</param>
        private void ReportHit(RaycastHit2D hit)
        {
            // Throw out ignored objects
            if (this.ignoreObjects.Contains(this.GetHitObject(hit)))
            {
                return;
            }

            // Ignore before OnHit or else ignoredObjects might be added to list after attack is canceled.
            this.ignoreObjects.Add(this.GetHitObject(hit));

            this.OnHit(new HitEventArgs(hit));
        }

        private GameObject GetHitObject(RaycastHit2D hit)
        {
            if (hit.collider == null)
            {
                return null;
            }

            return hit.collider.gameObject;
        }

        private void OnHit(HitEventArgs args)
        {
            if (this.Hit != null)
            {
                this.Hit(this, args);
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Let the casting debug draw when casting
            if (this.IsCasting)
            {
                return;
            }

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, this.radius);
        }

        /// <summary>
        /// Event Arguments for Hit events
        /// </summary>
        public class HitEventArgs : EventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="HitEventArgs"/> class.
            /// </summary>
            /// <param name="hit">Hit Information.</param>
            public HitEventArgs(RaycastHit2D hit)
            {
                this.Hit = hit;
            }

            /// <summary>
            /// Gets the hit information
            /// </summary>
            /// <value>The hit.</value>
            public RaycastHit2D Hit { get; private set; }
        }
    }
}