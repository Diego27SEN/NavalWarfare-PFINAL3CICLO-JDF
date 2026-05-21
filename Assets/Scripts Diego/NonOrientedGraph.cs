using James.Collections.Graphs;
using System.Collections.Generic;
using UnityEngine;

namespace James.Collections.Graphs
{
    public class NonOrientedGraph<T>
    {
        private List<Node<T>> nodes = new();

        //-> añadir un nodo
        public Node<T> AddNode(T value)
        {
            Node<T> newNode = new Node<T>(value);
            nodes.Add(newNode);

            return newNode;
        }
        //-> remover un nodo
        public void RemoveNode(Node<T> node)
        {
            nodes.Remove(node);

            foreach (var other in nodes)
            {
                other.Disconnect(node);
            }
        }
        public void AddEdges(Node<T> a, Node<T> b)
        {
            a.Connect(b);
        }
        public void AddEdges(int posA, int posB)
        {
            nodes[posA].Connect(nodes[posB]);
        }
        public void DeleteEdges(Node<T> a, Node<T> b)
        {
            a.Disconnect(b);
        }

        public void PrintAdjancencyList()//-> cuadratica
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                string nodeList = "Node Nro : " + nodes[i].Value.ToString() + " =>";

                //-> i => posoicion
                for (var j = 0; j < nodes[i].Neighbors.Count; j++)
                {
                    //-> j cada vecino de i

                    nodeList += nodes[i].Neighbors[j].Value.ToString() + ", ";
                }

                Debug.Log(nodeList);

            }
        }

        public void PrintAdjacencyMatrix()
        {
            Debug.Log("Matriz de adyacencia");
            string context = "    ";

            for (var i = 0; i < nodes.Count; i++)
            {
                context += i + "     ";
            }
            context += "\n";
            for (var i = 0; i < nodes.Count; i++)
            {
                context += i + "    ";//\n
                for (var j = 0; j < nodes.Count; j++)
                {
                    context += nodes[i].Neighbors.Contains(nodes[j]) ? "  1  " : "  0  ";
                }
                context += "\n";
            }
            Debug.Log(context);
        }




        // añadir conexion
        // remover una conexion
    }
}