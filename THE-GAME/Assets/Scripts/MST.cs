using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class MST : MonoBehaviour
{
    List<Edge> mstEdges = new List<Edge>();
    HashSet<GraphNode> visited = new HashSet<GraphNode>();
    List<Edge> availableEdges = new List<Edge>();
    /// <summary>
    /// Minimum Spanning Tree (MST) Algoritması
    /// </summary>
    /// <param name="graph"></param>
    /// <returns></returns>
    public List<Edge> GetMST(Graph graph)
    {
        if (graph == null || graph.nodes == null || graph.nodes.Count == 0)
        {
            Debug.LogError("Graph is empty or not initialized!");
            return null;
        }

        // Rastgele bir düğümden başla
        GraphNode startNode = graph.nodes[0];
        visited.Add(startNode);
        availableEdges.AddRange(startNode.edges);

        while (visited.Count < graph.nodes.Count)
        {
            // En küçük ağırlıklı kenarı seç
            Edge minEdge = availableEdges.OrderBy(e => e.weight).FirstOrDefault(e => visited.Contains(e.nodeA) ^ visited.Contains(e.nodeB));

            if (minEdge == null) break;

            // MST'ye ekle
            mstEdges.Add(minEdge);

            // Yeni düğümü işaretle
            GraphNode newNode = visited.Contains(minEdge.nodeA) ? minEdge.nodeB : minEdge.nodeA;
            visited.Add(newNode);

            // Yeni düğümün kenarlarını ekle
            availableEdges.AddRange(newNode.edges);
        }
        return mstEdges;
    }
    /// <summary>
    /// MST'ye count değeri ekstra kenarlar çizme
    /// </summary>
    /// <param name="count"></param>
    public void DrawExtraEdges(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, availableEdges.Count);
            mstEdges.Add(availableEdges[randomIndex]);
        }
    }
}