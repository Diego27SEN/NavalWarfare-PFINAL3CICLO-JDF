using James.Collections.Graphs;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public NonOrientedGraph<string> graph = new();

    void Start()
    {
        James.Collections.Graphs.Node<string> a = graph.AddNode("A");
        James.Collections.Graphs.Node<string> b = graph.AddNode("B");
        James.Collections.Graphs.Node<string> c = graph.AddNode("C");
        James.Collections.Graphs.Node<string> d = graph.AddNode("D");

        graph.AddEdges(a, b);
        graph.AddEdges(a, c);
        graph.AddEdges(b, c);
        graph.AddEdges(b, d);
        graph.AddEdges(c, d);

        graph.PrintAdjancencyList();
        graph.PrintAdjacencyMatrix();
    }
}
