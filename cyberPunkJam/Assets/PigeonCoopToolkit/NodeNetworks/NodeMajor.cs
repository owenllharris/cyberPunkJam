using UnityEngine;
using System.Collections.Generic;

namespace PigeonCoopToolkit.NavigationSystem
{
    public class NavNodeMajor : NavNode {

        [System.Serializable]
        public class ListOfNavNodeMinors
        {
            public List<NavNodeMinor> list = new List<NavNodeMinor>();
        }

        [SerializeField]
        public List<NavNodeMajor> m_majorNodeMinorNodeConnectionPointsKeys = new List<NavNodeMajor>();

        [SerializeField]
        public List<ListOfNavNodeMinors> m_majorNodeMinorNodeConnectionPointsValues = new List<ListOfNavNodeMinors>();


        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, Vector3.one * NavHelper.gizmoRadius);
            Gizmos.DrawWireCube(transform.position, Vector3.one * NavHelper.gizmoRadius / 4);

            base.OnDrawGizmos();
        }

        protected override void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, Vector3.one * NavHelper.gizmoRadius / 3.9f);
            Gizmos.DrawWireCube(transform.position, Vector3.one * NavHelper.gizmoRadius * 1.1f);

            base.OnDrawGizmosSelected();
        }

        public NavNodeMinor NavNodeMinorForMajor(NavNodeMajor toMajor)
        {
            int index = m_majorNodeMinorNodeConnectionPointsKeys.IndexOf(toMajor);
            if (index == -1)
                return null;

            return m_majorNodeMinorNodeConnectionPointsValues[index].list[0];
        }

        public void FormMajorToMajorConnection(NavNodeMinor fromMinor, NavNodeMajor toMajor)
        {
            int index = m_majorNodeMinorNodeConnectionPointsKeys.IndexOf(toMajor);

            if (index == -1)
            {
                m_majorNodeMinorNodeConnectionPointsKeys.Add(toMajor);
                m_majorNodeMinorNodeConnectionPointsValues.Add(new ListOfNavNodeMinors());
                index = m_majorNodeMinorNodeConnectionPointsValues.Count - 1;
            }

            m_majorNodeMinorNodeConnectionPointsValues[index].list.Add(fromMinor);

            if (m_majorNodeMinorNodeConnectionPointsValues[index].list.Count == 1)
            {
                data._ConnectTo(toMajor, NavNodeConnection.NavConnectionType.Standard);
            }
        }

        public void RemoveMajorToMajorConnection(NavNodeMinor fromMinor, NavNodeMajor toMajor)
        {
            int index = m_majorNodeMinorNodeConnectionPointsKeys.IndexOf(toMajor);

            if (index == -1)
            {
                Debug.LogError("Can't disconnect from a Major that you have not connected to.");
                return;
            }

            m_majorNodeMinorNodeConnectionPointsValues[index].list.Remove(fromMinor);


            if (m_majorNodeMinorNodeConnectionPointsValues[index].list.Count == 0)
            {
                m_majorNodeMinorNodeConnectionPointsKeys.RemoveAt(index);
                m_majorNodeMinorNodeConnectionPointsValues.RemoveAt(index);
                data._DisconnectFrom(toMajor);
            }
        }

    }
}
