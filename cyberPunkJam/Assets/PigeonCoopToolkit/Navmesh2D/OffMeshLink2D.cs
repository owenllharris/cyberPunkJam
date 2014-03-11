using PigeonCoopToolkit.NavigationSystem;
using PigeonCoopToolkit.Utillities;
using UnityEngine;

namespace PigeonCoopToolkit.Navmesh2D
{
    [ExecuteInEditMode]
    public class OffMeshLink2D : MonoBehaviour
    {
        [HideInInspector]
        public NavNodeMinor PointA, PointB;
        [HideInInspector]
        public Vector2 PointAPos = Vector2.right, PointBPos = -Vector2.right;
        public bool LinkActive = true;
        public bool Bidirectional = true;

        private Vector2 _lastPointAPos, _lastPointBPos;
        private bool _lastBidirectional;
        private bool LinkEstablished
        {
            get { return PointA != null && PointB != null; }
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 0.05f);
            if (LinkEstablished == false)
            {
                return;
            }

            Gizmos.color = Color.white;
            GizmosExtra.GizmosDrawArrow((Vector3)PointA.transform.position + Vector3.back * 0.1f, (Vector3)PointB.transform.position + Vector3.back * 0.1f, 0.2f);
            if(Bidirectional)
                GizmosExtra.GizmosDrawArrow((Vector3)PointB.transform.position + Vector3.back * 0.1f, (Vector3)PointA.transform.position + Vector3.back * 0.1f, 0.2f);

           
        }

        void OnDrawGizmosSelected()
        {
            if (LinkEstablished == false)
            {
                return;
            }

            GizmosExtra.GizmosDrawCircle(transform.TransformPoint(PointAPos), Vector3.back, 0.1f, 10);
            GizmosExtra.GizmosDrawCircle(transform.TransformPoint(PointBPos), Vector3.back, 0.1f, 10);
        }

        void OnDestroy()
        {
            if(LinkEstablished)
                BreakLink();
        }

        void Update()
        {
            if(NavMesh2D.SceneHasNavmesh() == false)
            {
                PointA = null;
                PointB = null;
                return;
            }


            EnforceConnection();

            if(LinkEstablished && !LinkActive)
            {
                BreakLink();
            }
            else if(LinkEstablished == false && LinkActive)
            {
                EstablishLink();
            }
            else if (((Vector3)_lastPointAPos != transform.TransformPoint(PointAPos) || (Vector3)_lastPointBPos != transform.TransformPoint(PointBPos)) || (_lastBidirectional != Bidirectional))
            {
                BreakLink();
                EstablishLink();
            }
        }

        private void EnforceConnection()
        {
            if(LinkEstablished && LinkActive)
            {
                PointA.ConnectTo(PointB, NavNodeConnection.NavConnectionType.Standard);
                if (Bidirectional)
                    PointB.ConnectTo(PointA, NavNodeConnection.NavConnectionType.Standard);
            }
        }

        private void BreakLink()
        {
            PointA.DisconnectFrom(PointB);
            PointB.DisconnectFrom(PointA);
            PointA = null;
            PointB = null;
        }

        void EstablishLink()
        {
            _lastPointAPos = transform.TransformPoint(PointAPos);
            _lastPointBPos = transform.TransformPoint(PointBPos);
            _lastBidirectional = Bidirectional;

            PointA = NavMesh2D.ClosestNodeTo(transform.TransformPoint(PointAPos));
            PointB = NavMesh2D.ClosestNodeTo(transform.TransformPoint(PointBPos));

            if (PointA == null || PointB == null)
            {
                PointA = NavMesh2D.ActualClosestNodeTo(transform.TransformPoint(PointAPos));
                PointB = NavMesh2D.ActualClosestNodeTo(transform.TransformPoint(PointBPos));
                if (PointA == null || PointB == null)
                {
                    return;
                }
            }

            if(Bidirectional && PointA.IsConnectedTo(PointB) && PointB.IsConnectedTo(PointA))
            {
                PointA = null;
                PointB = null;
                return;
            }

            if (!Bidirectional && PointA.IsConnectedTo(PointB))
            {
                PointA = null;
                PointB = null;
                return;
            }

            PointA.ConnectTo(PointB, NavNodeConnection.NavConnectionType.Standard);
            if(Bidirectional)
                PointB.ConnectTo(PointA, NavNodeConnection.NavConnectionType.Standard);

            
        }
    }
}
