using UnityEngine;

namespace Pathfinding
{
    public class Node
    {
        bool isWalkable;
        Node exploredFrom;
        Vector2Int worldPosition;

        int gridX;
        int gridY;

        int gCost;
        int hCost;

        public int GetXIndex() => gridX;
        public int GetYIndex() => gridY;
        public int GetGCost() => gCost;
        public int GetHCost() => hCost;
        public int GetFCost() => gCost + hCost;
        public void SetGCost(int value) => gCost = value;
        public void SetHCost(int value) => hCost = value;
        public Vector2Int GetWorldPosition() => worldPosition;
        public bool GetIsWalkable() => isWalkable;
        public Node GetExploredFrom() => exploredFrom;
        public void SetExploredFrom(Node node) => exploredFrom = node;

        public Node(bool walkable, Vector2 worldPos, int gridIndexX, int gridIndexY)
        {
            isWalkable = walkable;
            worldPosition = new Vector2Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y));
            gridX = gridIndexX;
            gridY = gridIndexY;
        }

    }

}
