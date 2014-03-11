using PigeonCoopToolkit.Utillities;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PigeonCoopToolkit.NavigationSystem
{
    public class Navigator : MonoBehaviour {

        public enum MethodOfPathing
        {
            Standard = 0,
            MajorAndMinorGrid = 1,
            MajorAndMinorGridThreaded = 2,
        }

        public MethodOfPathing PathingMethod = MethodOfPathing.Standard;
        [SerializeField]
        public NavNodeMinor[]  AllNavNodes;
        [SerializeField]
        public QuadTree NodesQuadTree;

        public string ObstructionLayer;


        public void SetNavNodes(NavNodeMinor[] allNavNodes)
        {
            AllNavNodes = allNavNodes;
        }

        internal class AStarData
        {
            public NodeDataPair cameFrom = null;
            public float fScore = 0;
            public float gScore = 0;
            public float hScore = 0;
        }

        internal class NodeDataPair
        {
            public NavNodeData n;
            public AStarData d;
        }

        internal class ThreadData
        {
            public NavNodeData start, end;
            public ManualResetEvent finishEvent, startEvent;
            public int threadNumber;
        }

        List<List<NavNodeData>> _threadedNodeSegments = new List<List<NavNodeData>>();

        public List<NavNodeData> GetPath(NavNodeMinor start, NavNodeMinor end)
        {
            if(PathingMethod == MethodOfPathing.Standard)
                return GetPathWorker(start.data, end.data);
            else if (PathingMethod == MethodOfPathing.MajorAndMinorGrid)
            {

                List<NavNodeMajor> majorPath = GetPathWorker(start.navNodeMajor.data, end.navNodeMajor.data).Select(a => a.node).Cast<NavNodeMajor>().ToList();

                List<NavNodeData> finalPath = new List<NavNodeData>();

                for (int i = 0; i < majorPath.Count; i++)
                {
                    NavNodeMinor _s = null;
                    NavNodeMinor _e = null;

                    if (i == 0)
                    {
                        _s = start;
                    }
                    else
                    {
                        _s = majorPath[i].NavNodeMinorForMajor(majorPath[i - 1]);
                    }


                    if (i == majorPath.Count - 1)
                    {
                        _e = end;
                    }
                    else
                    {
                        _e = majorPath[i].NavNodeMinorForMajor(majorPath[i + 1]);
                    }

                    if (_s == null || _e == null)
                        break;
                    List<NavNodeData> subPath = GetPathWorker(_s.data, _e.data);
                    if (subPath == null)
                        break;

                    finalPath.AddRange(subPath);
                }

                return finalPath;
            }
            else if (PathingMethod == MethodOfPathing.MajorAndMinorGridThreaded)
            {
                List<NavNodeMajor> majorPath = GetPathWorker(start.navNodeMajor.data, end.navNodeMajor.data).Select(a => a.node).Cast<NavNodeMajor>().ToList();
                List<ManualResetEvent> endEvents = new List<ManualResetEvent>();
                _threadedNodeSegments = new List<List<NavNodeData>>();

                for (int i = 0; i < majorPath.Count; i++)
                {
                    NavNodeMinor _s = null;
                    NavNodeMinor _e = null;

                    if (i == 0)
                    {
                        _s = start;
                    }
                    else
                    {
                        _s = majorPath[i].NavNodeMinorForMajor(majorPath[i - 1]);
                    }


                    if (i == majorPath.Count - 1)
                    {
                        _e = end;
                    }
                    else
                    {
                        _e = majorPath[i].NavNodeMinorForMajor(majorPath[i + 1]);
                    }

                    if (_s == null || _e == null)
                        break;

                    _threadedNodeSegments.Add(new List<NavNodeData>());
                    endEvents.Add(new ManualResetEvent(false));

                    ThreadData td = new ThreadData
                                        {
                                            start = _s.data,
                                            end = _e.data,
                                            threadNumber = i,
                                            startEvent = new ManualResetEvent(false),
                                            finishEvent = endEvents[endEvents.Count - 1],
                                        };

                    ThreadPool.QueueUserWorkItem(GetPathThreadWorker, td);
                    td.startEvent.WaitOne();

                }

                WaitHandle.WaitAll(endEvents.ToArray(),1);

                List<NavNodeData> finalPath = new List<NavNodeData>();
                foreach (List<NavNodeData> segment in _threadedNodeSegments)
                {
                    finalPath.AddRange(segment);
                }

                _threadedNodeSegments.Clear();
                return finalPath;
            }

            return null;
        }

        private void GetPathThreadWorker(object threadData)
        {
            ThreadData td = (ThreadData)threadData;
            td.startEvent.Set();
            _threadedNodeSegments[td.threadNumber] = GetPathWorker(td.start, td.end);
            td.finishEvent.Set();
        }
        private List<NavNodeData> GetPathWorker(NavNodeData start, NavNodeData end)
        {
            AStarData startData = new AStarData
                                      {
                                          cameFrom = null,
                                          fScore = 0,
                                          gScore = 0,
                                          hScore = 0
                                      };

            Dictionary<NavNodeData, NodeDataPair> openList = new Dictionary<NavNodeData, NodeDataPair>();
            Dictionary<NavNodeData, NodeDataPair> closedList = new Dictionary<NavNodeData, NodeDataPair>();

            openList.Add(start, new NodeDataPair
                                    {
                                        n = start,
                                        d = startData
                                    });

            while (openList.Count != 0)
            {
                NodeDataPair current = null;

                {
                    float lowestFscore = float.MaxValue;
                    foreach (NodeDataPair n in openList.Values)
                    {
                        if (n.d.fScore < lowestFscore)
                        {
                            lowestFscore = n.d.fScore;
                            current = n;
                        }
                    }
                }

                openList.Remove(current.n);
                closedList.Add(current.n, current);

                if (closedList.ContainsKey(end))
                {
                    //found a path
                    List<NavNodeData> pathFound = new List<NavNodeData>();
                    ReconstructPath(current, ref pathFound);
                    pathFound.Reverse();
                    //Path path = new Path(startNode, endNode);
                    //FindGoingTo(endNode, null);
                    //ReconstructPath(startNode, ref path);
                    //path.Smooth();

                    return pathFound;
                }

                foreach (NavNodeConnection connection in current.n.connections)
                {
                    if (closedList.ContainsKey(connection.connectedNode.data))
                        continue;

                    NodeDataPair pairAlreadyInOpenlist = null;
                    if (openList.ContainsKey(connection.connectedNode.data))
                        pairAlreadyInOpenlist = openList[connection.connectedNode.data];

                    if (pairAlreadyInOpenlist == null)
                    { //if the neighbor doesn't exist
                        AStarData nextNodeData = new AStarData();

                        nextNodeData.gScore = current.d.gScore + Vector3.Distance(current.n.position, connection.connectedNode.data.position);
                        nextNodeData.hScore = CalculateHeuristic(connection.connectedNode.data.position, end.position);//connection.Key == endNode ? Vector3.Distance(current.position,endNode.position) * 10 : Vector3.Distance(connection.Key.position,endNode.position); //heucistics will be used to find
                        //paths that are less dangerous, etc. danger being the heucistic.
                        nextNodeData.fScore = nextNodeData.gScore + nextNodeData.hScore;
                        nextNodeData.cameFrom = current;
                        openList.Add(connection.connectedNode.data, new NodeDataPair
                                                                        {
                                                                            n = connection.connectedNode.data,
                                                                            d = nextNodeData
                                                                        });
                    }
                    else if (current.d.gScore + Vector3.Distance(current.n.position, connection.connectedNode.data.position) < pairAlreadyInOpenlist.d.gScore)
                    { //if the neighbor exists and has bigger g score
                        pairAlreadyInOpenlist.d.gScore = current.d.gScore + Vector3.Distance(current.n.position, connection.connectedNode.data.position);
                        pairAlreadyInOpenlist.d.hScore = CalculateHeuristic(pairAlreadyInOpenlist.n.position, end.position);//connection.Key == endNode ? Vector3.Distance(current.position,endNode.position) * 10 : Vector3.Distance(connection.Key.position,endNode.position);//Vector3.Distance(connection.Key.position,endNode.position);
                        pairAlreadyInOpenlist.d.fScore = pairAlreadyInOpenlist.d.gScore + pairAlreadyInOpenlist.d.hScore;
                        pairAlreadyInOpenlist.d.cameFrom = current;
                    }
                }

            }

            return null;
        }
	
        private float CalculateHeuristic(Vector3 start, Vector3 end)
        {
            //return Vector3.Distance(start,end);
            //return Mathf.Abs(Mathf.Abs(start.x - end.x) - Mathf.Abs(start.y - end.y));
            return Mathf.Abs(Mathf.Abs(start.x - end.x) - Mathf.Abs(start.y - end.y));
        }

        private void ReconstructPath(NodeDataPair currentPair, ref List<NavNodeData> path)
        {
            path.Add(currentPair.n);

            if (currentPair.d.cameFrom != null)
                ReconstructPath(currentPair.d.cameFrom, ref path);
        }

        public List<Vector2> SmoothedVectorPath2D(float paddingSize, Vector2 s, Vector2 e, List<NavNodeData> path)
        {
            
            List<Vector2> finalPath = new List<Vector2>();
            if (path == null)
                return finalPath;

            finalPath.Add(s);
            finalPath.AddRange(path.Select(a => (Vector2)a.position));
            finalPath.Add(e);

            for (int i = 0; i < finalPath.Count - 1; i++)
            {
                if (finalPath.Count <= 2)
                    break;

                int toRemove = 0;
                for (int lookAhead = i + 2; lookAhead < finalPath.Count; lookAhead++)
                {
                    Vector2 cross = Vector2.zero;
                    bool noHit = false;

                    if (ObstructionLayer == "")
                        noHit = true;
                    else if(paddingSize > Mathf.Epsilon)
                    {
                        cross = Vector3.Cross((finalPath[lookAhead] - finalPath[i]).normalized, Vector3.back);
                        noHit =
                            Physics2D.Raycast(finalPath[i] + cross*paddingSize,
                                              (finalPath[lookAhead] - finalPath[i]).normalized,
                                              Vector2.Distance(finalPath[i], finalPath[lookAhead]),
                                              1 << LayerMask.NameToLayer(ObstructionLayer)) == false
                            &&
                            Physics2D.Raycast(finalPath[i] - cross*paddingSize,
                                              (finalPath[lookAhead] - finalPath[i]).normalized,
                                              Vector2.Distance(finalPath[i], finalPath[lookAhead]),
                                              1 << LayerMask.NameToLayer(ObstructionLayer)) == false;
                    }
                    else
                    {
                        noHit =
                            Physics2D.Raycast(finalPath[i],
                                              (finalPath[lookAhead] - finalPath[i]).normalized,
                                              Vector2.Distance(finalPath[i], finalPath[lookAhead]),
                                              1 << LayerMask.NameToLayer(ObstructionLayer)) == false;
                    }

                    if (noHit)
                    {
                        toRemove++;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                if(toRemove!=0)
                {
                    finalPath.RemoveRange(i + 1, toRemove);
                    if(i == 0)
                        i--;
                    else
                    {
                        i--;
                        i--;
                    }
                }
            }


            return finalPath;
        }
    }
}
