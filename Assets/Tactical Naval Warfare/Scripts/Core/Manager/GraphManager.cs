using TacticalNavalWarfare.Collections.Graphs;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public NonOrientedGraph<string> graph = new();

    void Start()
    {
        TacticalNavalWarfare.Collections.Graphs.Node<string> a = graph.AddNode("A");
        TacticalNavalWarfare.Collections.Graphs.Node<string> b = graph.AddNode("B");
        TacticalNavalWarfare.Collections.Graphs.Node<string> c = graph.AddNode("C");
        TacticalNavalWarfare.Collections.Graphs.Node<string> d = graph.AddNode("D");

        graph.AddEdges(a, b);
        graph.AddEdges(a, c);
        graph.AddEdges(b, c);
        graph.AddEdges(b, d);
        graph.AddEdges(c, d);

        graph.PrintAdjancencyList();
        graph.PrintAdjacencyMatrix();
    }
}
