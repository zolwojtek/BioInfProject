using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms
{
    public class Node
    {
        public char Character { get; set; }
        public int Id { get; set; }

        public Node(int id)
        {
            this.Id = id;
        }

        public Node(char character, int id)
        {
            this.Character = character;
            this.Id = id;
        }
    }

    public class Graph
    {
        public List<Node> Nodes {get; set;}
        public List<List<int>> NeighborhoodList { get; }
        private List<string> SequencesNames;

        public Graph(int size, string initSequence = null)
        {
            this.NeighborhoodList = new List<List<int>>();
            this.Nodes = new List<Node>();
            

            for(int i = 0; i < size + 2; ++i)
            {
                this.Nodes.Add(new Node(i));
                this.NeighborhoodList.Add(new List<int>());
            }

            if (initSequence != null)
                FitInitSequence(initSequence);
        }

        public void AddEdge(int u, int v)
        {
            this.NeighborhoodList[u].Add(v);
        }

        public int AddNode(char sign)
        {
            int newNodeIndex = Nodes.Count();
            Nodes.Add(new Node(sign, newNodeIndex));
            this.NeighborhoodList.Add(new List<int>());
            return newNodeIndex;
        }

        public int GetNextCoreNodeIndex(int currentNodeIndex)
        {
            return this.NeighborhoodList[currentNodeIndex].First();
        }

        public void SetOrChangeNextCoreNode(int currentNodeIndex, int nextNodeIndex)
        {
            if (NeighborhoodList[currentNodeIndex].Count() == 0)
            {
                this.NeighborhoodList[currentNodeIndex].Add(nextNodeIndex);
            }
            else
            {
                this.NeighborhoodList[currentNodeIndex][0] = nextNodeIndex;
            }
        }

        public void AddNodeAfter(int nodeIndex, char sign)
        {
            int newNodeIndex = AddNode(sign);
            SetOrChangeNextCoreNode(newNodeIndex, GetNextCoreNodeIndex(nodeIndex));
            SetOrChangeNextCoreNode(nodeIndex, newNodeIndex);
        }

        public void FitInitSequence(string initSequence)
        {
            //Creating edge between front sentry and the beginning of the sequence
            AddEdge(0, 1);
            Nodes[0].Character = '0';
            //Creating edge between every next node (represensting signs in the string)
            for (int i = 1; i <= initSequence.Length; ++i)
            {
                Nodes[i].Character = initSequence[i - 1];
                if (i < initSequence.Length)
                    AddEdge(i, i + 1);
            }
            //Creating edge between back sentry and the end of the sequence
            AddEdge(initSequence.Length, initSequence.Length + 1);
            //The back entry is recognizable by sign '0' 
            Nodes[initSequence.Length + 1].Character = '0';
        }

        private bool IsSentury(Node node)
        {
            if(node.Character == '0')
            {
                return true;
            }
            return false;
        }

        private  void GetNextLetttersOfAlignment(ref List<StringBuilder> aligments, Node node)
        {
            int seqNum = aligments.Count();
            aligments[0].Append(node.Character);
            for (int k = 1; k < seqNum; ++k)
            {
                aligments[k].Append(Nodes[this.NeighborhoodList[node.Id][k]].Character);
            }
        }

        private List<Sequence> PairSequencesWithNames(ref List<StringBuilder> aligments)
        {
            int seqNum = aligments.Count();
            List<Sequence> seqWithNames = new List<Sequence>();
            for (int i = 0; i < seqNum; ++i)
            {
                seqWithNames.Add(new Sequence(Constants.ALIGNMENT_DNA, SequencesNames[i], aligments[i].ToString()));
            }
            return seqWithNames;
        }

        public Alignment GetAlignment(int seqNum)
        {
            List<StringBuilder> aligments = new List<StringBuilder>();
            for (int i = 0; i < seqNum; ++i)
            {
                aligments.Add(new StringBuilder());
            }

            Node node = Nodes[GetNextCoreNodeIndex(0)];
            while (!IsSentury(node))
            {
                GetNextLetttersOfAlignment(ref aligments, node);
                node = Nodes[GetNextCoreNodeIndex(node.Id)];
            }
            
            return new Alignment(PairSequencesWithNames(ref aligments));
        }

        private void AddSignToAlignment(Node currentNode, char signToAdd)
        {
            int newNodeIndex = AddNode(signToAdd);
            AddEdge(currentNode.Id, newNodeIndex);
        }

        private void FetchNewSignToAlign(int alignmentIterator, ref Alignment alignment, ref char baseSeqSign, ref char newSeqSign)
        {
            if (alignmentIterator < alignment.Length)
            {
                baseSeqSign = alignment.GetSign(0, alignmentIterator);
                newSeqSign = alignment.GetSign(1, alignmentIterator);
            }
            else//jesli dopasowana sekwencja byla krotsza od dopasowania w grafie
            {
                baseSeqSign = newSeqSign = '-';
            }
        }

        public void AddSequenceToCurrentAlignment(Alignment alignment, int it)
        {
            if(SequencesNames == null)
            {
                this.SequencesNames = new List<string>();
                SequencesNames.Add(alignment.Sequences[0].Name);
            }
            SequencesNames.Add(alignment.Sequences[1].Name);
            int alignmentIterator = 0;
            int prevNodeId = 0;
            //alignment.Sequences[0] = base sequence, alignment.Sequences[1] = new seqence that we need to add
            char baseSeqSign = alignment.GetSign(0, alignmentIterator);
                //alignment.Sequences[0].Value[alignmentIterator];
            char newSeqSign = alignment.GetSign(1, alignmentIterator);
            //alignment.Sequences[1].Value[alignmentIterator];
            //znowu wbieramy pierwszy wezeł dopasowania
            Node node = Nodes[GetNextCoreNodeIndex(0)];
            //dopóki nie napotkamy na wartownika lub nowe dopasowanie jest dluzsze niz poprzednie i trzeba bedzie wydluzyc graf
            while (!IsSentury(node) || alignmentIterator < alignment.Length)
            {
                char nodeSign = node.Character;

                if (nodeSign == baseSeqSign)
                {
                    AddSignToAlignment(node, newSeqSign);

                    ++alignmentIterator;
                    FetchNewSignToAlign(alignmentIterator,ref alignment,ref baseSeqSign,ref newSeqSign);
                }
                else
                {
                    if (nodeSign == '-')
                    {//there is a gap in previous alignment that it does not occur in fitting alignment
                        int idx = AddNode('-');
                        AddEdge(node.Id, idx);
                    }
                    else
                    {//there is a gap in fitting alignment that does not occur in the graph
                     //(we need to add it - also for all previous fitted sequences)
                     //we need to go one step(node) back
                        node = Nodes[prevNodeId];
                        AddNodeAfter(node.Id, '-');
                        //int seqNum = NeighborhoodList[node.Id].Count() - 2;
                        //if (seqNum != it) Console.WriteLine("Zjebałeś");

                        for (int k = 0; k < it; ++k)
                        {
                            int idx = AddNode('-');
                            AddEdge(GetNextCoreNodeIndex(node.Id), idx);
                        }
                    }
                }
                prevNodeId = node.Id;
                node = Nodes[GetNextCoreNodeIndex(node.Id)];
            }
        }

    }
}
