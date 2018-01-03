using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms
{
    //Multiple Sequence Alignment
    public class MultipleAlignmentExact : TextAlignmentAlgorithm
    {
        private new int[, ,] alignmentArray;
        //public TextAlignmentParameters Parameters
        //{
        //    get
        //    {
        //        return parameters;
        //    }
        //    set
        //    {
        //        parameters = value;
        //        alignmentArray = null;
        //    }
        //}


        public MultipleAlignmentExact(TextAlignmentParameters parameters) : base(parameters)
        {

        }

        protected override void ComputeAlignmentArray()
        {
            int i = parameters.Sequences[0].Value.Length;
            int j = parameters.Sequences[1].Value.Length;
            int k = parameters.Sequences[2].Value.Length;
            this.alignmentArray = new int[i + 1, j + 1, k + 1];

            string A = parameters.Sequences[0].Value;
            string B = parameters.Sequences[1].Value;
            string C = parameters.Sequences[2].Value;

            
            List<int>[] v = variancy(3);
            
            for (int it = 0; it <= i; ++it)
            {
                for (int jt = 0; jt <= j; ++jt)
                {
                    for (int kt = 0; kt <= k; ++kt)
                    {
                        List<int> variables = new List<int>();
                        foreach (List<int> list in v)
                        {
                            variables.Add(ComputeCell(it, jt, kt, list[0], list[1], list[2]));
                        }
                        if (it == 0 && jt == 0 && kt == 0)
                        {
                            variables.Add(0);
                        }
                        alignmentArray[it, jt, kt] = MinFromList(variables);
                    }
                }
            }
        }


        private int MinFromList(List<int> list)
        {
            int min = list.First();
            foreach(int value in list)
            {
                if (value < min)
                    min = value;
            }
            return min;
        }

        private int ComputeAligningValue(char a, char b, char c)
        {
            int score = 0;
            score += parameters.CostArray.GetLettersAlignmentCost(a,b);
            score += parameters.CostArray.GetLettersAlignmentCost(b, c);
            score += parameters.CostArray.GetLettersAlignmentCost(a, c);
            return score;
        }

        private char FetchSign(string seq, int i, int iOffset)
        {
            if(iOffset == 0)
            {
                return '-';
            }
            return seq[i - iOffset];
        }

        private int ComputeCell(int i, int j, int k, int iOffset, int jOffset, int kOffset)
        {
            if (i - iOffset >= 0 && j - jOffset >= 0 && k - kOffset >= 0)
            {
                string A = parameters.Sequences[0].Value;
                string B = parameters.Sequences[1].Value;
                string C = parameters.Sequences[2].Value;
                char a, b, c;
                a = FetchSign(A, i, iOffset);
                b = FetchSign(B, j, jOffset);
                c = FetchSign(C, k, kOffset);
                return alignmentArray[i - iOffset, j - jOffset, k - kOffset] + ComputeAligningValue(a, b, c);
            }
            return int.MaxValue - 1000;
        }

        public override Alignment GetOptimalAlignment()
        {
            ComputeSolutionIfNecessary();
            return GetAligment();
        }

        private Alignment GetAligment()
        {
            int i = parameters.Sequences[0].Value.Length;
            int j = parameters.Sequences[1].Value.Length;
            int k = parameters.Sequences[2].Value.Length;
            string answerSeq1 = "";
            string answerSeq2 = "";
            string answerSeq3 = "";
            string A = parameters.Sequences[0].Value;
            string B = parameters.Sequences[1].Value;
            string C = parameters.Sequences[2].Value;
            while (!(i == 0 && j == 0 && k == 0))
            {
                if (i > 0 && j > 0 && k > 0)
                    if (alignmentArray[i - 1, j - 1, k - 1] + ComputeAligningValue(A[i - 1], B[j - 1], C[k - 1]) == alignmentArray[i, j, k])
                    {
                        answerSeq1 = A[i - 1] + answerSeq1;
                        answerSeq2 = B[j - 1] + answerSeq2;
                        answerSeq3 = C[k - 1] + answerSeq3;
                        i--;
                        j--;
                        k--;

                    }

                if (i > 0 && j > 0 && k >= 0)
                    if (alignmentArray[i - 1, j - 1, k] + ComputeAligningValue(A[i - 1], B[j - 1], '-') == alignmentArray[i, j, k])
                    {
                        answerSeq1 = A[i - 1] + answerSeq1;
                        answerSeq2 = B[j - 1] + answerSeq2;
                        answerSeq3 = "-" + answerSeq3;
                        i--;
                        j--;
                    }

                if (i > 0 && j >= 0 && k > 0)
                    if (alignmentArray[i - 1, j, k - 1] + ComputeAligningValue(A[i - 1], '-', C[k - 1]) == alignmentArray[i, j, k])
                    {
                        answerSeq1 = A[i - 1] + answerSeq1;
                        answerSeq2 = "-" + answerSeq2;
                        answerSeq3 = C[k - 1] + answerSeq3;
                        i--;
                        k--;
                    }

                if (i >= 0 && j > 0 && k > 0)
                    if (alignmentArray[i, j - 1, k - 1] + ComputeAligningValue('-', B[j - 1], C[k - 1]) == alignmentArray[i, j, k])
                    {
                        answerSeq1 = "-" + answerSeq1;
                        answerSeq2 = B[j - 1] + answerSeq2;
                        answerSeq3 = C[k - 1] + answerSeq3;
                        j--;
                        k--;
                    }
                
                
                if (i > 0 && j >= 0 && k >= 0)
                    if (alignmentArray[i - 1, j, k] + ComputeAligningValue(A[i - 1], '-', '-') == alignmentArray[i, j, k])
                    {
                        answerSeq1 = A[i - 1] + answerSeq1;
                        answerSeq2 = "-" + answerSeq2;
                        answerSeq3 = "-" + answerSeq3;
                        i--;

                    }
                
                if (i >= 0 && j > 0 && k >= 0)
                    if (alignmentArray[i, j - 1, k] + ComputeAligningValue('-', B[j - 1], '-') == alignmentArray[i, j, k])
                    {
                        answerSeq1 = "-" + answerSeq1;
                        answerSeq2 = B[j - 1] + answerSeq2;
                        answerSeq3 = "-" + answerSeq3;
                        j--;
                    }

                if (i >= 0 && j >= 0 && k > 0)
                    if (alignmentArray[i, j, k - 1] + ComputeAligningValue('-', '-', C[k - 1]) == alignmentArray[i, j, k])
                    {
                        answerSeq1 = "-" + answerSeq1;
                        answerSeq2 = "-" + answerSeq2;
                        answerSeq3 = C[k - 1] + answerSeq3;
                        k--;
                    }

            }
            //temporary
            Sequence s1 = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[0].Name, answerSeq1);
            Sequence s2 = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[1].Name, answerSeq2);
            Sequence s3 = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[2].Name, answerSeq3);
            return new Alignment(new List<Sequence>() { s1,s2,s3 } );
        }


        public override int GetOptimalAlignmentScore()
        {
            ComputeSolutionIfNecessary();
            int i = parameters.Sequences[0].Value.Length;
            int j = parameters.Sequences[1].Value.Length;
            int k = parameters.Sequences[2].Value.Length;
            return alignmentArray[i,j,k];
        }

        private int index(int m)
        {
            int i = 0;
            while (m % 2 == 0)
            {
                i++;
                m = m / 2;
            }
            return i;
        }

        public List<int>[] variancy(int k)
        {
            int i;
            int[] skok = new int[k + 1];
            for (i = 1; i <= k; ++i)
            {
                skok[i] = 1;
            }

            int m = 0;
            i = 0;
            int[] w = new int[k + 1];
            List<int>[] ret = new List<int>[(int)Math.Pow(2, k)-1];
            for (int it = 0; it < ret.Length; ++it)
            {
                ret[it] = new List<int>();
            }
            int col = 0;
            do
            {
                ++m;
                i = index(m) + 1;
                if (i <= k)
                {
                    w[i] = w[i] + skok[i];
                    if (w[i] == 0)
                    {
                        skok[i] = 1;
                    }
                    if (w[i] == 1)
                    {
                        skok[i] = -1;
                    }
                }
                else
                {
                    return ret;
                    //w[i - 1] = w[i - 1] + skok[i - 1];
                }
                for (int j = 1; j <= k; ++j)
                {
                    ret[col].Add(w[j]);
                }
                ++col;
            } while (i <= k);
            return ret;
        }

        public override int GetNumberOfOptimalSolutions()
        {
            ComputeSolutionIfNecessary();
            int i = parameters.Sequences[0].Value.Length;
            int j = parameters.Sequences[1].Value.Length;
            int k = parameters.Sequences[2].Value.Length;

            int sum = 0;
            CountNumberOfOptimalSolutions(i, j, k, ref sum);
            return sum;
        }

        private void CountNumberOfOptimalSolutions(int i, int j, int k, ref int sum)
        {
            if (i == 0 && j == 0 && k == 0)
            {
                ++sum;
                return;
            }
            List<int>[] possibleDirections = variancy(3);
            foreach (List<int> list in possibleDirections)
            {
                if (alignmentArray[i, j, k] == ComputeCell(i, j, k, list[0], list[1], list[2]))
                {
                    CountNumberOfOptimalSolutions(i - list[0], j - list[1], k - list[2], ref sum);
                }
            }
        }


        private void ComputeSolutionIfNecessary()
        {
            if (this.alignmentArray == null)
            {
                ComputeAlignmentArray();
            }
        }

    }
}
