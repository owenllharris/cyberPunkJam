using PigeonCoopToolkit.Utillities;
using UnityEngine;

namespace PigeonCoopToolkit.NavigationSystem
{
    public class NavNodeMinor : NavNode {

        public NavNodeMajor navNodeMajor;

        protected override void OnDrawGizmos()
        {
            Gizmos.color = NavHelper.NavMeshBaseColor;

            if (navNodeMajor != null)
            {
                Gizmos.DrawLine(transform.position, navNodeMajor.transform.position);
            }

            
            Gizmos.DrawWireSphere(transform.position, NavHelper.gizmoRadius / 4);

            base.OnDrawGizmos();        
        }

        protected override void OnDrawGizmosSelected()
        {
            Gizmos.color = NavHelper.NavMeshBaseSelectedColor;
            Gizmos.DrawWireSphere(transform.position, NavHelper.gizmoRadius / 3.9f);
        
            base.OnDrawGizmosSelected();
        }

        protected override void OnDestroy()
        {
            foreach (NavNodeMinor n in FindObjectsOfType(typeof(NavNodeMinor)))
            {
                n.DisconnectFrom(this);
            }

            base.OnDestroy();
        }

        public void ConnectTo(NavNodeMinor n, NavNodeConnection.NavConnectionType connectionType)
        {
            if (data._ConnectTo(n, connectionType) && n.navNodeMajor != navNodeMajor)
            {
                navNodeMajor.FormMajorToMajorConnection(this, n.navNodeMajor);
            }

        }

        public void DisconnectFrom(NavNodeMinor n)
        {
            if (data._DisconnectFrom(n) && n.navNodeMajor != navNodeMajor)
            {
                navNodeMajor.RemoveMajorToMajorConnection(this, n.navNodeMajor);
            }
        }

        public bool IsConnectedTo(NavNodeMinor n)
        {
            return data.ConnectedTo(n);
        }

        public void FindNavNodeMajorParent()
        {
            Transform parent = transform.parent;

            while (parent != null)
            {
                navNodeMajor = parent.GetComponent<NavNodeMajor>();
                if (navNodeMajor != null)
                {
                    break;
                }

                parent = parent.parent;
            }
    
        }
    }
}
