using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DrawMST : MonoBehaviour
{
    /// <summary>
    /// MST kenarlarını çizer.
    /// </summary>
    /// <param name="edges"></param>
    public void DrawEdges(List<Edge> edges)
    {
        foreach (var edge in edges)
        {
            Debug.DrawLine(new Vector3(edge.nodeA.point.x, 0, edge.nodeA.point.z), new Vector3(edge.nodeB.point.x, 0, edge.nodeB.point.z), Color.red, 1000f);
            Debug.Log("Edge : " + edge.nodeA.point.x + " " + edge.nodeA.point.z + " " + edge.nodeB.point.x + " " + edge.nodeB.point.z);
        }
    }
}
