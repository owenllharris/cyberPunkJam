using PigeonCoopToolkit.Utillities;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PigeonCoopToolkit.NavigationSystem
{
    [ExecuteInEditMode]
    public class NavNode : MonoBehaviour {

        public NavNodeData data;

        protected virtual void OnDrawGizmos()
        {
            if (data == null)
                return;
            foreach (NavNodeConnection c in data.connections)
            {
                Vector3 dir =  c.connectedNode.transform.position - transform.position;
                dir = dir.normalized * NavHelper.gizmoDistanceFromNode;
                Vector3 cross = Vector3.Cross(Vector3.forward, dir).normalized * NavHelper.gizmoNodeConnectionSeperation;

                GizmosExtra.GizmosDrawArrow(transform.position + dir + cross, c.connectedNode.transform.position - dir + cross,NavHelper.gizmoArrowSize);
            }
        }

        protected virtual void Awake()
        {
            if(data == null)
                data = new NavNodeData();

            data.position = transform.position;
            data.node = this;
        }

        protected virtual void FixedUpdate()
        {
            data.position = transform.position;
            data.node = this;
        }

        protected virtual void OnDrawGizmosSelected()
        {
        
        }

        protected virtual void OnDestroy()
        {
        }


    }

    [System.Serializable]
    public class NavNodeData
    {
        public List<NavNodeConnection> connections = new List<NavNodeConnection>();
        public HashSet<NavNode> connectionsHashed = new HashSet<NavNode>();
        public Vector3 position;
        public NavNode node;

        public bool ConnectedTo(NavNode n)
        {
            return connectionsHashed.Contains(n);
        }

        public virtual bool _ConnectTo(NavNode n, NavNodeConnection.NavConnectionType connectionType)
        {
            if (!ConnectedTo(n))
            {
                connectionsHashed.Add(n);
                NavNodeConnection newConnection = new NavNodeConnection();

                newConnection.connectedNode = n;
                newConnection.connectionType = connectionType;
                connections.Add(newConnection);
                return true;
            }

            return false;
        }

        public virtual bool _DisconnectFrom(NavNode n)
        {
            if (ConnectedTo(n))
            {
                connectionsHashed.Remove(n);

                connections.Remove(connections.First(a => a.connectedNode == n));
                return true;
            }

            return false;
        }
    }
}