using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms.AlignmentAlgorithms 
{
    public class MultipleAlignmentAprox : TextAlignmentAlgorithm
    {
        public MultipleAlignmentAprox(TextAlignmentParameters parameters) : base(parameters)
        {

        }

        private int ComputeCell(int i, int j, AlignmentType aligmentType)
        {
            throw new NotImplementedException();
        }

        protected override void Compute()
        {
            throw new NotImplementedException();
        }

        public override int GetOptimalAlignmentScore()
        {
            List<string> alignment = GetOptimalAlignment2();
            int score = 0;
            for(int i = 0; i < alignment.First().Length; ++i)
            {
                for(int j = 0; j < alignment.Count(); ++j)
                {
                    for(int k = j+1; k < alignment.Count(); ++k)
                    {
                        if(j == k)
                        {
                            continue;
                        }
                        score += parameters.CostArray.GetCost(alignment[j][i],alignment[k][i]);
                    }
                }
            }
            return score;
        }

        public Sequence GetOptimalAlignment()
        {
            throw new NotImplementedException();
        }
        public List<string> GetOptimalAlignment2()
        {
            GlobalAlignment globalAlignment = new GlobalAlignment(parameters);

            int bestScore = int.MaxValue - 1000;
            int bestIdx = 0;

            List<Sequence> sequences = parameters.Sequences;
            for(int  i = 0; i < sequences.Count(); ++i)
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
                if(score < bestScore)
                {
                    bestScore = score;
                    bestIdx = i;
                }
            }

            Sequence best = sequences[bestIdx];

            Alignment alignment;
            for(int i = 0; i < sequences.Count(); ++i)
            {
                if(i == bestIdx)
                {
                    continue;
                }
                parameters.Sequences = new List<Sequence>() { best, sequences[i] };
                globalAlignment.Parameters = parameters;
                alignment = globalAlignment.GetOptimalAlignment();

                List<int> gaps = new List<int>();
                gaps = new List<int>();
                for (int j = 0; j < sequences[bestIdx].Value.Length; ++j)
                {
                   if (String.Equals(sequences[bestIdx].Value[j], '-'))
                    {
                        gaps.Add(j);
                    }
                }

                string tempF = alignment.Sequences[0].Value;
                string tempS = alignment.Sequences[1].Value;

                foreach (int gap in gaps)
                {
                    int num1 = 0;
                    int num2 = 0;
                    for(int j = 0; j < gap; ++j)
                    {
                        if (String.Equals(tempF, '-')) 
                            ++num1;

                        if (String.Equals(tempS, '-'))
                            ++num2;
                    }
                    if (!String.Equals(alignment.Sequences[0].Value[gap+num1], '-'))
                    {
                        alignment.Sequences[1].Value = alignment.Sequences[1].Value.Insert(gap+num1, "-");
                    }
                    if (!String.Equals(alignment.Sequences[1].Value[gap + num2], '-'))
                    {
                        alignment.Sequences[1].Value = alignment.Sequences[1].Value.Insert(gap + num2, "-");
                    }
                }

                List<int> gaps2 = new List<int>();
                for(int j = 0; j < tempF.Length; ++j)
                {
                    if (String.Equals(tempF[j], '-'))
                        gaps2.Add(j);
                }

                for (int j = 0; j < i; ++j)
                {
                    if (j == bestIdx)
                    {
                        continue;
                    }
                    foreach (int gap in gaps2)
                    {
                        sequences[j] = sequences[j].Insert(gap + gaps.Count(), "-");
                    }
                }

                sequences[bestIdx] = alignment.Name;
                sequences[i] = alignment.Value;
            }

            int max = sequences[0].Length;
            foreach(string str in sequences)
            {
                if (str.Length > max)
                    max = str.Length;
            }
            for(int j = 0; j < sequences.Count(); ++j)
            {
                if(sequences[j].Length < max)
                {
                    int rep = sequences[j].Length;
                    for (int i = 0; i < max - rep; ++i)
                    {
                        sequences[j] += "-";
                    }
                }
            }
            return sequences;
        }

        protected override void ComputeBlock(int rowStart, int colStart, int rowNum, int colNum)
        {
            throw new NotImplementedException();
        }



        public override int GetNumberOfOptimalSolutions()
        {
            throw new NotImplementedException();
        }

        public override Alignment GetOptimalAlignment()
        {
            throw new NotImplementedException();
        }
    }
}
