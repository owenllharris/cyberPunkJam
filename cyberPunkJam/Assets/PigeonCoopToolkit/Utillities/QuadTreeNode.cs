using System.Linq;
using UnityEngine;

namespace PigeonCoopToolkit.Utillities
{
    [ExecuteInEditMode]
    public class QuadTreeNode : ScriptableObject
    {
        public Rect NodeBounds;
        public string ObstructionLayer;
        public QuadTree.PositionObjectPair[] ObjectsWithinNode;
        public QuadTreeNode[] ChildNodes;

        private void OnEnable()
        {
            hideFlags = HideFlags.HideInHierarchy;
        }

        private void OnDestroy()
        {
            if(Application.isPlaying)
            {
                for (int i = 0; i < ChildNodes.Length; i++)
                {
                    Destroy(ChildNodes[i]);
                }
            }
            else
            {
                for (int i = 0; i < ChildNodes.Length; i++)
                {
                    DestroyImmediate(ChildNodes[i]);
                }
            }
        }


        public void Init(Rect b, QuadTree.PositionObjectPair[] objectsWithinNode, string _ObstructionLayer, int numObjectsPerNode)
        {
            NodeBounds = b;
            ObstructionLayer = _ObstructionLayer;
            ObjectsWithinNode = objectsWithinNode;
            if (ObjectsWithinNode.Length <= numObjectsPerNode)
            {
                ChildNodes = new QuadTreeNode[0];
                return;
            }

            ChildNodes = new QuadTreeNode[4];
            Rect[] childNodeRects = new Rect[4];
            childNodeRects[0] = new Rect(b.xMin, b.yMin, b.width/2f, b.height/2);
            childNodeRects[1] = new Rect(b.xMin + b.width/2f, b.yMin, b.width/2f, b.height/2);
            childNodeRects[2] = new Rect(b.xMin, b.yMin + b.height/2, b.width/2f, b.height/2);
            childNodeRects[3] = new Rect(b.xMin + b.width/2f, b.yMin + b.height/2, b.width/2f, b.height/2);

            ChildNodes[0] = CreateInstance<QuadTreeNode>();
            ChildNodes[0].Init(childNodeRects[0],
                               objectsWithinNode.Where(a => childNodeRects[0].Contains(a.Position)).ToArray(), ObstructionLayer,numObjectsPerNode);
            ChildNodes[1] = CreateInstance<QuadTreeNode>();
            ChildNodes[1].Init(childNodeRects[1],
                               objectsWithinNode.Where(a => childNodeRects[1].Contains(a.Position)).ToArray(), ObstructionLayer, numObjectsPerNode);
            ChildNodes[2] = CreateInstance<QuadTreeNode>();
            ChildNodes[2].Init(childNodeRects[2],
                               objectsWithinNode.Where(a => childNodeRects[2].Contains(a.Position)).ToArray(), ObstructionLayer, numObjectsPerNode);
            ChildNodes[3] = CreateInstance<QuadTreeNode>();
            ChildNodes[3].Init(childNodeRects[3],
                               objectsWithinNode.Where(a => childNodeRects[3].Contains(a.Position)).ToArray(), ObstructionLayer, numObjectsPerNode);
            
        }


        public QuadTree.PositionObjectPair ClosestTo<T>(Vector2 pos) where T : Object
        {
            return ObjectClosestToRecursive<T>(pos);
        }

        public T ClosestObjectTo<T>(Vector2 pos) where T : Object
        {
            QuadTree.PositionObjectPair foundPos = ObjectClosestToRecursive<T>(pos);
            return foundPos != null ? (T)foundPos.Obj : null;
        }

        public Vector2 ClosestPointTo<T>(Vector2 pos) where T : Object
        {
            QuadTree.PositionObjectPair foundPos = ObjectClosestToRecursive<T>(pos);
            return foundPos != null ? foundPos.Position : pos;
        }

        public QuadTree.PositionObjectPair ClosestTo(Vector2 pos)
        {
            return ObjectClosestToRecursive<Object>(pos);
        }

        public Object ClosestObjectTo(Vector2 pos) 
        {
            QuadTree.PositionObjectPair foundPos = ObjectClosestToRecursive<Object>(pos);
            return foundPos != null ? foundPos.Obj : null;
        }

        public Vector2 ClosestPointTo(Vector2 pos)
        {
            QuadTree.PositionObjectPair foundPos = ObjectClosestToRecursive<Object>(pos);
            return foundPos != null ? foundPos.Position : pos;
        }

        private QuadTree.PositionObjectPair ObjectClosestToRecursive<T>(Vector2 pos) where T:Object
        {
            if (ObjectsWithinNode.Length == 0 || ObjectsWithinNode.Any(a => a.Obj is T) == false) 
                return null;

            QuadTree.PositionObjectPair found = null;

            if (NodeBounds.Contains(pos))
            {
                foreach (QuadTreeNode child in ChildNodes)
                {
                    found = child.ObjectClosestToRecursive<T>(pos);
                    if (found != null)
                        break;
                }

                if (found == null)
                {
                    foreach (var t in
                    ObjectsWithinNode.OrderBy(a => Vector2.Distance(a.Position, pos)))
                    {
                        if(t.Obj is T)
                        {
                            if (ObstructionLayer == "" || Physics2D.Raycast(pos, ((Vector2)t.Position - pos).normalized, Vector2.Distance(pos, t.Position), 1 << LayerMask.NameToLayer(ObstructionLayer)) == false)
                            {
                                found = t;
                                break;
                            }
                        }
                    }
                }
            }

            return found;
        }

        public T ActualClosestObjectTo<T>(Vector2 pos) where T : Object
        {
            QuadTree.PositionObjectPair opp =
                ObjectsWithinNode.OrderBy(a => Vector2.Distance(pos, a.Position)).Where(a => a.Obj is T).FirstOrDefault();

            return opp == null ? null : (T)opp.Obj;
        }

        public Vector2 ActualClosestPointTo<T>(Vector2 pos) where T : Object
        {
            QuadTree.PositionObjectPair opp =
               ObjectsWithinNode.OrderBy(a => Vector2.Distance(pos, a.Position)).Where(a => a.Obj is T).FirstOrDefault();

            return opp == null ? Vector2.zero : opp.Position;
        }
    }
}