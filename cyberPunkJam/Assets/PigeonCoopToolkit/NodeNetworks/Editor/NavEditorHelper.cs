using UnityEngine;
using UnityEditor;
using System.Linq;

namespace PigeonCoopToolkit.NavigationSystem.Editor
{
    public class NavEditorHelper {
        /*
        [MenuItem("Window/Pigeon Coop Toolkit/Navigation System/Connect Selected %#c")]
        public static void ConnectNodes()
        {
            NavNodeMinor[] allNavNodes = Selection.gameObjects.Select(a => a.GetComponent<NavNodeMinor>()).Where(a => a != null).ToArray();
            Object[] effectMajorNodes = allNavNodes.Select(a => a.navNodeMajor).Distinct().ToArray();
            Object[] allEffectedNodes = effectMajorNodes.Concat(allNavNodes).Where(a => a != null).ToArray();

            if (allEffectedNodes.Length == 0)
                return;


            Undo.RecordObjects(allEffectedNodes, "Node Connections");

            foreach (NavNodeMinor n in allNavNodes)
            {
                foreach (NavNodeMinor nn in allNavNodes)
                {
                    if (nn == n)
                        continue;

                    n.ConnectTo(nn, NavNodeConnection.NavConnectionType.Smoothable);
                }
            }
            SceneView.RepaintAll();
        }

        [MenuItem("Window/Pigeon Coop Toolkit/Navigation System/Connect Unobstructed Selected")]
        public static void ConnectUnobstructedNodes()
        {
            NavNodeMinor[] allNavNodes = Selection.gameObjects.Select(a => a.GetComponent<NavNodeMinor>()).Where(a => a != null).ToArray();
            Object[] effectMajorNodes = allNavNodes.Select(a => a.navNodeMajor).Distinct().ToArray();
            Object[] allEffectedNodes = effectMajorNodes.Concat(allNavNodes).Where(a => a != null).ToArray();

            if (allEffectedNodes.Length == 0)
                return;


            Undo.RecordObjects(allEffectedNodes, "Node Connections");

            foreach (NavNodeMinor n in allNavNodes)
            {
                foreach (NavNodeMinor nn in allNavNodes)
                {
                    if (nn == n)
                        continue;

                    if (!Physics.CapsuleCast(n.transform.position, nn.transform.position, 0.5f, (nn.transform.position - n.transform.position).normalized))
                    {
                        n.ConnectTo(nn, NavNodeConnection.NavConnectionType.Smoothable);
                    }
                }
            }
            SceneView.RepaintAll();
        }


        [MenuItem("Window/Pigeon Coop Toolkit/Navigation System/Disconnect Selected %#d")]
        public static void DisconnectNodes()
        {
            NavNodeMinor[] allNavNodes = Selection.gameObjects.Select(a => a.GetComponent<NavNodeMinor>()).Where(a => a != null).ToArray();
            Object[] effectMajorNodes = allNavNodes.Select(a => a.navNodeMajor).Distinct().ToArray();
            Object[] allEffectedNodes = effectMajorNodes.Concat(allNavNodes).Where(a => a != null).ToArray();

            if (allEffectedNodes.Length == 0)
                return;


            Undo.RecordObjects(allEffectedNodes, "Node Connections");

            foreach (NavNodeMinor n in allNavNodes)
            {
                foreach (NavNodeMinor nn in allNavNodes)
                {
                    if (nn == n)
                        continue;

                    n.DisconnectFrom(nn);
                }
            }
            SceneView.RepaintAll();
        }

        [MenuItem("Window/Pigeon Coop Toolkit/Navigation System/Connect To All %&#c")]
        public static void ConnectAllNodes()
        {
            NavNodeMinor[] allNavNodes = Selection.gameObjects.Select(a => a.GetComponent<NavNodeMinor>()).Where(a => a != null).ToArray();
            Object[] effectMajorNodes = allNavNodes.Select(a => a.navNodeMajor).Distinct().ToArray();
            Object[] allEffectedNodes = effectMajorNodes.Concat(allNavNodes).Where(a => a != null).ToArray();

            if (allEffectedNodes.Length == 0)
                return;


            Undo.RecordObjects(allEffectedNodes, "Node Connections");

            NavNodeMinor[] allSceneNodes = GameObject.FindObjectsOfType(typeof(NavNodeMinor)) as NavNodeMinor[];

            foreach (NavNodeMinor n in allNavNodes)
            {
                foreach (NavNodeMinor nn in allSceneNodes)
                {
                    if (nn == n)
                        continue;

                    n.ConnectTo(nn, NavNodeConnection.NavConnectionType.Smoothable);
                }
            }
            SceneView.RepaintAll();
        }

        [MenuItem("Window/Pigeon Coop Toolkit/Navigation System/Disconnect From All %&#d")]
        public static void DisconnectAllNodes()
        {
            NavNodeMinor[] allNavNodes = Selection.gameObjects.Select(a => a.GetComponent<NavNodeMinor>()).Where(a => a != null).ToArray();
            Object[] effectMajorNodes = allNavNodes.Select(a => a.navNodeMajor).Distinct().ToArray();
            Object[] allEffectedNodes = effectMajorNodes.Concat(allNavNodes).Where(a => a != null).ToArray();

            if (allEffectedNodes.Length == 0)
                return;


            Undo.RecordObjects(allEffectedNodes, "Node Connections");

            NavNodeMinor[] allSceneNodes = GameObject.FindObjectsOfType(typeof(NavNodeMinor)) as NavNodeMinor[];

            foreach (NavNodeMinor n in allNavNodes)
            {
                foreach (NavNodeMinor nn in allSceneNodes)
                {
                    if (nn == n)
                        continue;

                    n.DisconnectFrom(nn);
                }
            }
            SceneView.RepaintAll();
        }

        [MenuItem("Window/Pigeon Coop Toolkit/Navigation System/Connect To All Unobstructed")]
        public static void ConnectAllUnobstructedNodes()
        {
            NavNodeMinor[] allNavNodes = Selection.gameObjects.Select(a => a.GetComponent<NavNodeMinor>()).Where(a => a != null).ToArray();
            Object[] effectMajorNodes = allNavNodes.Select(a => a.navNodeMajor).Distinct().ToArray();
            Object[] allEffectedNodes = effectMajorNodes.Concat(allNavNodes).Where(a => a != null).ToArray();

            if (allEffectedNodes.Length == 0)
                return;


            Undo.RecordObjects(allEffectedNodes, "Node Connections");

            NavNodeMinor[] allSceneNodes = GameObject.FindObjectsOfType(typeof(NavNodeMinor)) as NavNodeMinor[];

            foreach (NavNodeMinor n in allNavNodes)
            {
                foreach (NavNodeMinor nn in allSceneNodes)
                {
                    if (nn == n)
                        continue;

                    if (!Physics.CapsuleCast(n.transform.position, nn.transform.position, 0.35f, (nn.transform.position - n.transform.position).normalized))
                    {
                        n.ConnectTo(nn, NavNodeConnection.NavConnectionType.Smoothable);
                    }
                }
            }
            SceneView.RepaintAll();
        }


        [MenuItem("Window/Pigeon Coop Toolkit/Navigation System/Find Major node for Minors")]
        public static void FindMajorNodeForMinors()
        {
            NavNodeMinor[] allNavNodes = Selection.gameObjects.Select(a => a.GetComponent<NavNodeMinor>()).Where(a => a != null).ToArray();
            Object[] effectMajorNodes = allNavNodes.Select(a => a.navNodeMajor).Distinct().ToArray();
            Object[] allEffectedNodes = effectMajorNodes.Concat(allNavNodes).Where(a => a != null).ToArray();

            if (allEffectedNodes.Length == 0)
                return;


            Undo.RecordObjects(allEffectedNodes, "Node Connections");

            foreach (NavNodeMinor n in allNavNodes)
            {
                n.FindNavNodeMajorParent();
            }
            SceneView.RepaintAll();
        }
         */
    }
}
