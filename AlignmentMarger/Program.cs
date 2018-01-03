//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using StringAlgorithms;
//using StringAlgorithms.AlignmentAlgorithms;

//namespace AlignmentMarger
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            MultipleAlignment alignmentAlgorithm = new MultipleAlignment(new TextAlignmentParameters());
//            alignmentAlgorithm.variancy(4);



//            string seq1;
//            string seq2;
//            string seq3;
//            string seq4;
//            seq1 = "ACGT";
//            seq2 = "ATTCT";
//            seq3 = "CTCGA";
//            seq4 = "ACGGT";
            

//            TextAlignmentParameters parameters = new TextAlignmentParameters();
//            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
//            parameters.AddSequence(seq1);
//            parameters.AddSequence(seq2);
//            parameters.AddSequence(seq3);
//            //parameters.AddSequence(seq4);
//            parameters.GapCostFun = ((x) => (x * 1));
//            parameters.Comparefunction = ((x, y) => (Math.Min(x, y)));
//            parameters.CostArray = new StringCostArray(StringAlgorithms.Constants.DNA, c);


//            GlobalAlignment globalAlignment = new GlobalAlignment(parameters);

//            List<string> sequences = parameters.GetSequences();
//            int bestIdx = GetBestAligmentBase(parameters);
//            Graph graf = InitializeGraph(seq1);
//            int addedSequencesCouter = 0;
//            for (int i = 0; i < sequences.Count(); ++i)
//            {
//                if (i == bestIdx)
//                {
//                    continue;
//                }
//                parameters.SetSequences(new List<string>() { sequences[bestIdx], sequences[i] });
//                globalAlignment.Parameters = parameters;
//                Sequence alignment = globalAlignment.GetOptimalAlignment();
//                AddSequenceToCurrentAlignment(graf, alignment, addedSequencesCouter);
//                ++addedSequencesCouter;

//            }

//            List<string> finalAlignments = RetrieveAlignments(graf, addedSequencesCouter);

//        }

//        private static Graph InitializeGraph(string bestSequence)
//        {
//            Graph graf = new Graph(bestSequence.Length + 1);
//            graf.AddEdge(0, 1);
//            for (int i = 1; i <= bestSequence.Length; ++i)
//            {
//                graf.Nodes[i].Character = bestSequence[i - 1];
//                if (i < bestSequence.Length)
//                    graf.AddEdge(i, i + 1);
//            }

//            return graf;
//        }

//        private static int GetBestAligmentBase(TextAlignmentParameters parameters)
//        {
//            GlobalAlignment globalAlignment = new GlobalAlignment(parameters);
//            int bestScore = int.MaxValue - 1000;
//            int bestIdx = 0;

//            List<string> sequences = parameters.GetSequences();
//            for (int i = 0; i < sequences.Count(); ++i)
//            {
//                int score = 0;
//                for (int j = 0; j < sequences.Count(); ++j)
//                {
//                    if (i == j)
//                    {
//                        continue;
//                    }

//                    parameters.SetSequences(new List<string>() { sequences[i], sequences[j] });
//                    globalAlignment.Parameters = parameters;
//                    score += globalAlignment.GetOptimalAlignmentScore();
//                }
//                if (score < bestScore)
//                {
//                    bestScore = score;
//                    bestIdx = i;
//                }
//            }
//            return bestIdx;
//        }

//        private static List<string> RetrieveAlignments(Graph graf, int seqNum)
//        {
     
//            List<string> aligments = new List<string>();
//            for (int i = 0; i <= seqNum; ++i)
//            {
//                aligments.Add("");
//            }

//            Node node = graf.Nodes[graf.Nodes[0].Neighbours.First()];
//            while (true)
//            {
//                aligments[0] += node.Character;
//                for (int k = 1; k <= seqNum; ++k)
//                {
//                    if (node.Neighbours.Count() == seqNum)
//                        aligments[k] += graf.Nodes[node.Neighbours[k - 1]].Character;
//                    else
//                        aligments[k] += graf.Nodes[node.Neighbours[k]].Character;
//                }
//                if (node.Neighbours.Count() == seqNum)
//                    break;
//                node = graf.Nodes[node.Neighbours[0]];
//            }
//            return aligments;
//        }


//        private static void AddSequenceToCurrentAlignment(Graph graf, Sequence alignment, int it)
//        {
//            int alignmentItereator = 0;
//            int prevNodeId = 0;
//            char s0 = alignment.Name[alignmentItereator];
//            char s1 = alignment.Value[alignmentItereator];
//            Node node = graf.Nodes[1];
//            while (true)
//            {
//                char nodeSign = node.Character;
//                if (nodeSign == s0)
//                {
//                    int idx = graf.AddNode(s1);
//                    graf.AddEdge(node.Id, idx);
//                    ++alignmentItereator;
//                    if (alignmentItereator < alignment.Name.Length)
//                    {
//                        s0 = alignment.Name[alignmentItereator];
//                        s1 = alignment.Value[alignmentItereator];
//                    }
//                    else
//                    {
//                        s0 = s1 = '-';
//                    }
//                }
//                else
//                {
//                    if (nodeSign == '-')
//                    {

//                        int idx = graf.AddNode('-');
//                        graf.AddEdge(node.Id, idx);

//                    }
//                    else
//                    {

//                        graf.AddNodeAfter(prevNodeId,'-');
//                        for (int k = 0; k < it; ++k)
//                        {
//                            int idx = graf.AddNode('-');
//                            graf.AddEdge(graf.Nodes[prevNodeId].Neighbours.First(), idx);
//                        }

//                        node = graf.Nodes[prevNodeId];
//                    }
//                }
//                prevNodeId = node.Id;
//                if (alignmentItereator < alignment.Name.Length && node.Neighbours.Count() == it+1)
//                {

//                    int idx = graf.AddNode('-');
//                    graf.AddEdge(node.Id, idx);

//                }
//                if (alignmentItereator >= alignment.Name.Length && node.Neighbours.Count() == it+1)
//                {
//                    break;
//                }
//                node = graf.Nodes[node.Neighbours.First()];
//            }

//        }
//    }
//}
