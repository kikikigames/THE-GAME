using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Grid grid;
    void Awake()
    {
        grid = FindFirstObjectByType<Grid>();
    }
    /// <summary>
    /// 2 nokta arasındaki yolun bulunması
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="targetPos"></param>
    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        bool pathSuccess = false;
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        //Başlangıç ve bitiş düğümlerinin FloorType'ını değiştirme (Yoksa A* çalışmaz)
        startNode.floor.floorType = FloorType.White;
        targetNode.floor.floorType = FloorType.White;

        if (startNode == null || targetNode == null) //|| !targetNode.walkable
        {
            Debug.LogError("Start or target node is null or target node is unwalkable");
            return;
        }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            // En düşük fCost değerine sahip düğümü seç
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // Bitiş noktasına ulaştık mı?
            if (currentNode == targetNode)
            {
                pathSuccess = true;
                break;
            }

            // Komşuları kontrol et
            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (neighbor.floor.floorType == FloorType.Red || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        if (pathSuccess)
        {
            Debug.Log("Path found!");
            RetracePath(startNode, targetNode);
        }
        else
        {
            Debug.Log("Path not found!");
        }
    }
    /// <summary>
    /// Düğümler arasındaki yolu bulma ve FloorType'ı değiştirme
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        //Debug.Log("Retrace path start: " + startNode.worldPosition);
        //Debug.Log("Retrace path endNode: " + endNode.worldPosition);
        
        startNode.floor.floorType = FloorType.Red;
        endNode.floor.floorType = FloorType.Red;
        grid.CheckPathNeighbors(grid.GetNeighbors(startNode),startNode);
        grid.CheckPathNeighbors(grid.GetNeighbors(endNode),endNode);

        while (currentNode != startNode)
        {
            if (currentNode.parent == null)
            {
                Debug.LogError("RetracePath failed: parent is null!");
                break;
            }
            Debug.Log("Retrace path current: " + currentNode.worldPosition);
            if (currentNode.floor.floorType != FloorType.Red) 
            {
                path.Add(currentNode);
                currentNode.floor.floorType = FloorType.Yellow;
                currentNode.roomId=0;//Path roomId
                FloorManager.AddYellowFloor(currentNode);
            }
            currentNode = currentNode.parent;
        }

        grid.CheckPathNeighbors(grid.GetNeighbors(startNode),startNode);
        grid.CheckPathNeighbors(grid.GetNeighbors(endNode),endNode);
    }
    /// <summary>
    /// Düğümler arasındaki mesafeyi hesaplama (Manhattan)
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>
    /// <returns></returns>
    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        return distX + distY; 
    }

    /// <summary>
    /// Kenardaki düğümler arasında yol bulma
    /// </summary>
    /// <param name="edges"></param>
    public void FindPathBetweenTwoPoint(List<Edge> edges)
    {
        foreach (var edge in edges)
        {
            Debug.Log("Find Edge : " + edge.nodeA.point.x + " " + edge.nodeA.point.z + " " + edge.nodeB.point.x + " " + edge.nodeB.point.z);

            FindPath(new Vector3(edge.nodeA.point.x, 0, edge.nodeA.point.z), new Vector3(edge.nodeB.point.x, 0, edge.nodeB.point.z));
        }
    }

}