using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlignmentMarger
{
    public class Node
    {
        public List<int> Neighbours { get; set; }
        public char Character { get; set; }
        public int Id { get; set; }
        public Node(int id)
        {
            this.Neighbours = new List<int>();
            this.Id = id;
        }

        public Node(char character, int id)
        {
            this.Neighbours = new List<int>();
            this.Character = character;
            this.Id = id;
        }
    }

    public class Graph
    {
        public List<Node> Nodes {get; set;}
        public Graph(int size)
        {
            this.Nodes = new List<Node>();
            for(int i = 0; i < size; ++i)
            {
                this.Nodes.Add(new Node(i));
            }
        }

        public void AddEdge(int u, int v)
        {
            Nodes[u].Neighbours.Add(v);
        }

        public int AddNode(char sign)
        {
            int index = Nodes.Count();
            Nodes.Add(new Node(sign, index));
            return index;
        }

        public void AddNodeAfter(int u, char sign)
        {
            int idx = AddNode(sign);
            Nodes[idx].Neighbours.Add(Nodes[u].Neighbours.First());
            Nodes[u].Neighbours[0] = idx;
        }
        
    }
}
