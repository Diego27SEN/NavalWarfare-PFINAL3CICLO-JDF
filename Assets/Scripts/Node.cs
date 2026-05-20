using System.Collections.Generic;
using UnityEngine;


namespace James.Collections.Graphs
{
    public class Node<T>//->vertices
    {
        private T value;
        private List<Node<T>> neighbors = new();

        public Node(T value)
        {
            this.value = value;
        }

        public void Connect(Node<T> node)//->bidireccional
        {
            if (!neighbors.Contains(node))
            {
                neighbors.Add(node);
            }
        
            node.neighbors.Add(this);
       
        }
        public void Disconnect(Node<T> node)//> bidireccional
        {
            if (neighbors.Contains(node))
                neighbors.Remove(node);

            node.Disconnect(this);
        }

        public T Value => value;
        public List<Node<T>> Neighbors => neighbors;
    }
}