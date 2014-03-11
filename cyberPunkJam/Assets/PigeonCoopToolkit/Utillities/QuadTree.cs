using UnityEngine;
using System.Linq;

using System.Collections.Generic;

namespace PigeonCoopToolkit.Utillities
{
    [AddComponentMenu("Generic Scripts/Quad Tree")]
    [ExecuteInEditMode]
    public class QuadTree : MonoBehaviour
    {
        [System.Serializable]
        public class PositionObjectPair
        {
            public Vector2 Position;
            public Object Obj;
        }

        public PositionObjectPair[] AllTransformsInTree;
        [SerializeField]
        public QuadTreeNode QuadTreeNode;

        public string ObstructionLayer;

        public void GenerateQuadTree(string obstructionLayer, int numObjectsPerNode)
        {
            ObstructionLayer = obstructionLayer;
            numObjectsPerNode = Mathf.Clamp(numObjectsPerNode,1, int.MaxValue);
            DestroyTree();
            if(AllTransformsInTree.Length <= 1)
            {
                Debug.LogError("QuadTree: Only one object in quad tree. Need more.");
                return;
            }

            float minX = AllTransformsInTree.Min(a => a.Position.x) - 0.5f;
            float minY = AllTransformsInTree.Min(a => a.Position.y) - 0.5f;
            float maxX = AllTransformsInTree.Max(a => a.Position.x) + 0.5f;
            float maxY = AllTransformsInTree.Max(a => a.Position.y) + 0.5f;

            QuadTreeNode = ScriptableObject.CreateInstance<QuadTreeNode>();
            QuadTreeNode.Init(new Rect(minX, minY, Mathf.Abs(minX - maxX), Mathf.Abs(minY - maxY)), AllTransformsInTree, obstructionLayer,numObjectsPerNode);
        
        }

        public void DestroyTree()
        {
            if (QuadTreeNode != null)
                DestroyImmediate(QuadTreeNode);
        }

        public void SetObjects(PositionObjectPair[] transforms)
        {
            AllTransformsInTree = transforms;
        }

        public PositionObjectPair ClosestTo<T>(Vector2 pos) where T : Object
        {
            if (QuadTreeNode == null || Physics2D.OverlapPoint(pos, 1 << LayerMask.NameToLayer(ObstructionLayer)) != null)
                return null;

            return QuadTreeNode.ClosestTo<T>(pos);
        }

        public T ClosestObjectTo<T>(Vector2 pos) where T : Object
        {
            if (QuadTreeNode == null || Physics2D.OverlapPoint(pos, 1 << LayerMask.NameToLayer(ObstructionLayer)) != null)
                return null;

            return QuadTreeNode.ClosestObjectTo<T>(pos);
        }

        public T ActualClosestObjectTo<T>(Vector2 pos) where T : Object
        {
            if (QuadTreeNode == null || Physics2D.OverlapPoint(pos, 1 << LayerMask.NameToLayer(ObstructionLayer)) != null)
                return null;

            return QuadTreeNode.ActualClosestObjectTo<T>(pos);
        }

        public Vector2 ClosestPointTo<T>(Vector2 pos) where T : Object
        {
            if (QuadTreeNode == null || Physics2D.OverlapPoint(pos, 1 << LayerMask.NameToLayer(ObstructionLayer)) != null)
                return pos;

            return QuadTreeNode.ClosestPointTo<T>(pos);
        }

        public Vector2 ActualClosestPointTo<T>(Vector2 pos) where T : Object
        {
            if (QuadTreeNode == null || Physics2D.OverlapPoint(pos, 1 << LayerMask.NameToLayer(ObstructionLayer)) != null)
                return pos;

            return QuadTreeNode.ActualClosestPointTo<T>(pos);
        }

        public PositionObjectPair ClosestTo(Vector2 pos)
        {
            if (QuadTreeNode == null || Physics2D.OverlapPoint(pos, 1 << LayerMask.NameToLayer(ObstructionLayer)) != null)
                return null;

            return QuadTreeNode.ClosestTo(pos);
        }

        public Object ClosestObjectTo(Vector2 pos)
        {
            if (QuadTreeNode == null || Physics2D.OverlapPoint(pos, 1 << LayerMask.NameToLayer(ObstructionLayer)) != null)
                return null;

            return QuadTreeNode.ClosestObjectTo(pos);
        }

        public Vector2 ClosestPointTo(Vector2 pos)
        {
            if (QuadTreeNode == null || Physics2D.OverlapPoint(pos, 1 << LayerMask.NameToLayer(ObstructionLayer)) != null)
                return pos;

            return QuadTreeNode.ClosestPointTo(pos);
        }

        void OnDestroy()
        {
            DestroyTree();
        }

    }

    
    
}
