using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using PigeonCoopToolkit.Generic;
using PigeonCoopToolkit.NavigationSystem;
using PigeonCoopToolkit.Utillities;
using UnityEngine;



[System.Serializable]
public class NavMesh2D : MonoBehaviour 
{
    [System.Serializable]
    public class MeshContour
    {
        public Vector2[] Points;
    }

    [System.Serializable]
    public class NavmeshGenerationInformation
    {
        public string WalkableColliderLayer;
        public string ObstructionColliderLayer;
        public float CircleColliderSubdivisionFactor;
        public float CalculationScaleFactor;
        public float ColliderPadding;
        public bool UseGrid;
        public Vector2 GridSize;
        public JoinType JoinType;
    }

    private static NavMesh2D _instanceRef;

    private static NavMesh2D instance
    {
        get
        {
            if(_instanceRef == null)
            {
                _instanceRef = FindObjectOfType<NavMesh2D>();
            }

            return _instanceRef;
        }
    }

    public MeshContour[] Contours;
    public Mesh NavMesh;
    public NavNodeMinor[] NavNodes;
    public QuadTree NodeQuadTree;
    public Navigator Navigator;
    public NavmeshGenerationInformation GenerationInformation;
    public VersionInformation Version;

    /// <summary>
    /// Returns true if the scene has a NavMesh baked into it.
    /// </summary>
    public static bool SceneHasNavmesh()
    {
        return instance != null;
    }

    /// <summary>
    /// A smoothed path from start to end.
    /// </summary>
    /// <param name="startPosition">The Start position</param>
    /// <param name="endPosition">The End position</param>
    /// <returns>A list of points, order from start to end</returns>
    public static List<Vector2> GetSmoothedPath(Vector2 startPosition, Vector2 endPosition)
    {
        if (instance == null)
        {
            Debug.LogError("NavMesh2D: Scene does not contain a 2D NavMesh");
            return new List<Vector2>();
        }

           

        List<Vector2> resultingPath = new List<Vector2>(0);

        NavNodeMinor startNode = instance.NodeQuadTree.ClosestObjectTo<NavNodeMinor>(startPosition);
        NavNodeMinor endNode = instance.NodeQuadTree.ClosestObjectTo<NavNodeMinor>(endPosition);
        if (startNode != null && endNode != null)
        {
            resultingPath = instance.Navigator.SmoothedVectorPath2D(
                instance.GenerationInformation.ColliderPadding, startPosition, endPosition,
                instance.Navigator.GetPath(startNode, endNode)
                );
        }


        return resultingPath;
    }

    /// <summary>
    /// A path from start to end.
    /// </summary>
    /// <param name="startPosition">The Start position</param>
    /// <param name="endPosition">The End position</param>
    /// <returns>A list of points, order from start to end</returns>
    public static List<Vector2> GetPath(Vector2 startPosition, Vector2 endPosition)
    {
        if (instance == null)
        {
            Debug.LogError("NavMesh2D: Scene does not contain a 2D NavMesh");
            return new List<Vector2>();
        }

        List<Vector2> resultingPath = new List<Vector2>();
        resultingPath.Add(startPosition);
        NavNodeMinor startNode = instance.NodeQuadTree.ClosestObjectTo<NavNodeMinor>(startPosition);
        NavNodeMinor endNode = instance.NodeQuadTree.ClosestObjectTo<NavNodeMinor>(endPosition);
        if (startNode != null && endNode != null)
        {
            resultingPath.AddRange(instance.Navigator.GetPath(
                startNode,
                endNode).Select(a => (Vector2) a.position));
        }

        resultingPath.Add(endPosition);

        return resultingPath;
    }

    /// <summary>
    /// Returns the layer on which the walls for this navmesh reside
    /// </summary>
    public static LayerMask GetObstructionLayer()
    {
        if (instance == null)
        {
            Debug.LogError("NavMesh2D: Scene does not contain a 2D NavMesh");
            return new LayerMask();
        }

        return 1 << LayerMask.NameToLayer(instance.GenerationInformation.ObstructionColliderLayer);
    }

    /// <summary>
    /// Returns the layer on which the floors for this navmesh reside
    /// </summary>
    public static LayerMask GetWalkableLayer()
    {
        if (instance == null)
        {
            Debug.LogError("NavMesh2D: Scene does not contain a 2D NavMesh");
            return new LayerMask();
        }

        return 1 << LayerMask.NameToLayer(instance.GenerationInformation.WalkableColliderLayer);
    }

    /// <summary>
    /// Gets the estimated closest node to a given position (Uses the quadtree, faster but may not be accurate)
    /// </summary>
    /// <param name="pos">The position to test against</param>
    public static NavNodeMinor ClosestNodeTo(Vector2 pos)
    {
        if (instance == null)
        {
            Debug.LogError("NavMesh2D: Scene does not contain a 2D NavMesh");
            return null;
        }

        return instance.NodeQuadTree.ClosestObjectTo<NavNodeMinor>(pos);
    }

    /// <summary>
    /// Gets the closest node to a given position (Slower but guaranteed to be accurate)
    /// </summary>
    /// <param name="pos">The position to test against</param>
    public static NavNodeMinor ActualClosestNodeTo(Vector2 pos)
    {
        if (instance == null)
        {
            Debug.LogError("NavMesh2D: Scene does not contain a 2D NavMesh");
            return null;
        }

        return instance.NodeQuadTree.ActualClosestObjectTo<NavNodeMinor>(pos);
    }
}

    
