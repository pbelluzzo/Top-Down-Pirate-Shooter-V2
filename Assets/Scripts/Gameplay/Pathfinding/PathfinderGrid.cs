using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class PathfinderGrid : MonoBehaviour
    {
        [SerializeField] bool displayGrid;
        [SerializeField] LayerMask unwalkableLayer;
        [SerializeField] Vector2 gridWorldSize;
        [SerializeField] float nodeRadius;

        Node[,] grid;
        float nodeDiameter;
        int gridSizeX;
        int gridSizeY;

        Vector2Int[] directions = {
        Vector2Int.up,
        new Vector2Int (1,1),
        Vector2Int.right,
        new Vector2Int (1,-1),
        Vector2Int.down,
        new Vector2Int (-1,-1),
        Vector2Int.left,
        new Vector2Int (1, -1)
        };

        public Node GetGridNode(int x, int y) => grid[x, y];

        private void Awake()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }

        void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft = (transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2);

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector2 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                    bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableLayer);
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            foreach (Vector2 direction in directions)
            {
                int checkX = node.GetXIndex() + (int)direction.x;
                int checkY = node.GetYIndex() + (int)direction.y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {

                    neighbours.Add(grid[checkX, checkY]);
                }
            }

            return neighbours;
        }

        public Node GetNodeFromWorldPoint(Vector2 worldPos)
        {
            float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float percentY = (worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

            if (grid[x, y] == null)
                return null;

            return grid[x, y];
        }

        public List<Node> path;
        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y));

            if (grid != null && displayGrid == true)
            {
                foreach (Node node in grid)
                {
                    Gizmos.color = node.GetIsWalkable() ? Color.white : Color.red;
                    Vector3 gizmoPos = new Vector3(node.GetWorldPosition().x, node.GetWorldPosition().y);
                    Gizmos.DrawCube(new Vector3(node.GetWorldPosition().x, node.GetWorldPosition().y), Vector3.one * (nodeDiameter - (0.1f * nodeDiameter)));
                }
            }
        }
    }
}

