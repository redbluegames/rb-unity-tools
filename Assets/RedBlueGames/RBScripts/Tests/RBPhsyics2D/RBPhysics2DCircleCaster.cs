using UnityEngine;
using System.Collections;
using RedBlueGames;

namespace RedBlueGames.Tools.Tests
{
	[ExecuteInEditMode]
	public class RBPhysics2DCircleCaster : MonoBehaviour
	{
		public float Radius;
		public CastMethod Method;

		public enum CastMethod
		{
			Single,
			CastAll,
			NonAlloc
		}

		public float Distance;
		public LayerMask castLayers;

		RaycastHit2D[] results = new RaycastHit2D[3];

		void Update ()
		{
			switch (Method) {
			case CastMethod.Single:
				RBPhysics2D.CircleCast (transform.position, Radius, transform.up, Distance, castLayers);
				break;
			case CastMethod.CastAll:
				RBPhysics2D.CircleCastAll (transform.position, Radius, transform.up, Distance, castLayers);
				break;
			case CastMethod.NonAlloc:
				RBPhysics2D.CircleCastNonAlloc (transform.position, Radius, transform.up, results, Distance, castLayers);
				break;
			}
		}
	}
}