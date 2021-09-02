using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding 
{
    public class Pathfinder : MonoBehaviour
    {
        PathfinderGrid grid;
        PathRequestManager requestManager;
        static Pathfinder instance;
        int linearCost = 10;
        int diagonalCost = 14;

        public static Pathfinder GetInstance() => instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            instance = this;

            grid = GetComponent<PathfinderGrid>();
            requestManager = GetComponent<PathRequestManager>();

        }

        public void CalculatePath(Vector3 startPosition, Vector3 endPosition)
        {
            StartCoroutine(FindPath(startPosition, endPosition));
        }


        IEnumerator FindPath(Vector2 startPosition, Vector2 endPosition)
        {
            Vector3[] waypoints = new Vector3[0];
            bool pathFound = false;

            Node start = grid.GetNodeFromWorldPoint(startPosition);
            Node end = grid.GetNodeFromWorldPoint(endPosition);

            if (!start.GetIsWalkable() || !end.GetIsWalkable())
            {
                yield return null;
                Debug.LogWarning("Start and/or end point are unwalkable. Path failed");
                requestManager.FinishedCalculatingPath(waypoints, pathFound);
            }

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].GetFCost() < currentNode.GetFCost() ||
                       (openSet[i].GetFCost() == currentNode.GetFCost() && openSet[i].GetHCost() < currentNode.GetHCost()))
                        currentNode = openSet[i];
                }
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == end)
                {
                    pathFound = true;
                    break;
                }

                SetNeighbours(end, openSet, closedSet, currentNode);
            }

            yield return null;
            if (pathFound)
                waypoints = BuildPath(start, end);

            requestManager.FinishedCalculatingPath(waypoints, pathFound);
        }

        private void SetNeighbours(Node end, List<Node> openSet, HashSet<Node> closedSet, Node currentNode)
        {
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (neighbour.GetIsWalkable() == false || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.GetGCost() + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.GetGCost() || !openSet.Contains(neighbour))
                {
                    neighbour.SetGCost(newMovementCostToNeighbour);
                    neighbour.SetHCost(GetDistance(neighbour, end));

                    neighbour.SetExploredFrom(currentNode);

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        Vector3[] BuildPath(Node start, Node end)
        {
            List<Node> path = new List<Node>();
            Node currentNode = end;

            while (currentNode != start)
            {
                path.Add(currentNode);
                currentNode = currentNode.GetExploredFrom();
            }
            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);

            return waypoints;
        }

        Vector3[] SimplifyPath(List<Node> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionNew = new Vector2(path[i - 1].GetXIndex() - path[i].GetXIndex(), path[i - 1].GetYIndex() - path[i].GetYIndex());
                if (directionNew != directionOld)
                {
                    waypoints.Add(new Vector3(path[i].GetWorldPosition().x, path[i].GetWorldPosition().y));
                }
                directionOld = directionNew;
            }
            return waypoints.ToArray();
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int horizontalDistance = Mathf.Abs(nodeA.GetXIndex() - nodeB.GetXIndex());
            int verticalDistance = Mathf.Abs(nodeA.GetYIndex() - nodeB.GetYIndex());

            if (horizontalDistance > verticalDistance)
            {
                return verticalDistance * diagonalCost + linearCost * (horizontalDistance - verticalDistance);
            }

            return horizontalDistance * diagonalCost + linearCost * (verticalDistance - horizontalDistance);

        }
    }
}

