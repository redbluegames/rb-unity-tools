using UnityEngine;
using System.Collections;

namespace RedBlueGames.Tools.Tests
{
    [ExecuteInEditMode]
    public class RBPhysics2DRayCaster : MonoBehaviour
    {
        public CastType Cast;

        public enum CastType
        {
            Raycast,
            Linecast,
        }

        public CastMethod Method;

        public enum CastMethod
        {
            Single,
            All,
            NonAlloc
        }

        public float Distance;
        public LayerMask castLayers;
        RaycastHit2D[] results = new RaycastHit2D[3];

        void Update()
        {
            Vector3 start = transform.position;
            Vector3 direction = transform.up;
            if (Cast == CastType.Linecast)
            {
                Vector3 end = start + (Distance * direction);
                switch (Method)
                {
                    case CastMethod.Single:
                        RBPhysics2D.LineCast(start, end, castLayers);
                        break;
                    case CastMethod.All:
                        RBPhysics2D.LineCastAll(start, end, castLayers);
                        break;
                    case CastMethod.NonAlloc:
                        RBPhysics2D.LineCastNonAlloc(start, end, results, castLayers);
                        break;
                }
            }
            else
            {
                switch (Method)
                {
                    case CastMethod.Single:
                        RBPhysics2D.RayCast(start, direction, Distance, castLayers);
                        break;
                    case CastMethod.All:
                        RBPhysics2D.RayCastAll(start, direction, Distance, castLayers);
                        break;
                    case CastMethod.NonAlloc:
                        RBPhysics2D.RayCastNonAlloc(start, direction, results, Distance, castLayers);
                        break;
                }
            }
        }
    }
}