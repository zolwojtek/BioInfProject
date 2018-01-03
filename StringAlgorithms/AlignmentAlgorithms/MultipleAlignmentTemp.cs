using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms
{
    public class MultipleAlignmentTemp : TextAlignmentAlgorithm
    {
        private Graph alignmentGraph;

        public MultipleAlignmentTemp(TextAlignmentParameters parameters) : base(parameters)
        {
        }

        protected override void ComputeAlignmentArray()
        {
            TextAlignmentParameters _parameters = parameters.Clone<TextAlignmentParameters>();
            GlobalAlignment globalAlignment = new GlobalAlignment(_parameters);
            List<Sequence> sequences = _parameters.Sequences;

            int bestIdx = GetBestAligmentBase(_parameters);
            alignmentGraph = InitializeGraph(sequences[bestIdx].Value);
            int addedSequenceCounter = 0;

            for (int i = 0; i < sequences.Count(); ++i)
            {
                if (i == bestIdx)
                {
                    continue;
                }
                _parameters.Sequences = new List<Sequence>() { sequences[bestIdx], sequences[i] };
                globalAlignment.Parameters = _parameters;
                Alignment alignment = globalAlignment.GetOptimalAlignment();
                alignmentGraph.AddSequenceToCurrentAlignment(alignment, addedSequenceCounter);
                WriteGraphToFile(alignmentGraph,$@"C:\Users\Me\Desktop\Graph\iteration{i}.txt");
                ++addedSequenceCounter;
            }
        }

        private void WriteToFile(string path, List<string> lines)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path))
            {
                foreach (string line in lines)
                {

                    file.WriteLine(line);

                }
            }
        }

        private void WriteGraphToFile(Graph graph, string path)
        {
            Node node = graph.Nodes[graph.NeighborhoodList[0][0]];
            List<string> lines = new List<string>();
            while (node.Character != '0')
            {
                string line = string.Empty;
                char myChar = node.Character;
                line += "0: ";
                line += myChar;
                line += " ";
                //write myChar
                int neigborsNum = graph.NeighborhoodList[node.Id].Count();//node.Neighbours.Count();
                if (neigborsNum > 1)
                    for (int i = 1; i < neigborsNum; ++i)
                    {
                        char neighSign = graph.Nodes[graph.NeighborhoodList[node.Id][i]].Character;
                        line += $"{i}: {neighSign} ";
                        //write number and sign
                    }
                lines.Add(line);

                node = graph.Nodes[graph.NeighborhoodList[node.Id][0]];

            }
            WriteToFile(path, lines);
        }

        public override int GetOptimalAlignmentScore()
        {
            Alignment alignment = GetOptimalAlignment();
            int score = 0;
            for (int i = 0; i < alignment.Sequences.First().Value.Length; ++i)
            {
                for (int j = 0; j < alignment.Sequences.Count(); ++j)
                {
                    for (int k = j + 1; k < alignment.Sequences.Count(); ++k)
                    {
                        if (j == k)
                        {
                            continue;
                        }
                        score += parameters.CostArray.GetLettersAlignmentCost(alignment.Sequences[j].Value[i],alignment.Sequences[k].Value[i]);
                    }
                }
            }
            return score;
        }

        public override Alignment GetOptimalAlignment()
        {
            ComputeSolutionIfNecessary();
            return alignmentGraph.GetAlignment(parameters.Sequences.Count());
        }

        private Graph InitializeGraph(string initSequence)
        {
            //At the beginning and the end of path is a sentry, that is why we add 2 to its length.
            Graph graf = new Graph(initSequence.Length);
            
            graf.FitInitSequence(initSequence);
            return graf;
        }


        //Choose the fundamental sequence for the alignment (which maximize the score = sum of pairwise alignments every sequence with choosen base sequence) 
        private int GetBestAligmentBase(TextAlignmentParameters parameters)
        {
            GlobalAlignment globalAlignment = new GlobalAlignment(parameters);
            List<Sequence> sequences = parameters.Sequences;
            return GetSeqenceWithHighestScore(parameters, globalAlignment, sequences);         
        }

        private int GetSeqenceWithHighestScore(TextAlignmentParameters parameters, GlobalAlignment globalAlignment, List<Sequence> sequences)
        {
            int bestScore = int.MaxValue - 1000;
            int bestIdx = 0;
            for (int i = 0; i < sequences.Count(); ++i)
            {
                int score = 0;
                for (int j = 0; j < sequences.Count(); ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    parameters.Sequences = new List<Sequence>() { sequences[i], sequences[j] };
                    globalAlignment.Parameters = parameters;
                    score += globalAlignment.GetOptimalAlignmentScore();
                }
                if (score < bestScore)
                {
                    bestScore = score;
                    bestIdx = i;
                }
            }
            return bestIdx;
        }

        public override int GetNumberOfOptimalSolutions()
        {
            return 1;
        }

        private void ComputeSolutionIfNecessary()
        {
            if (this.alignmentGraph == null)
            {
                ComputeAlignmentArray();
            }
        }
    }
}
