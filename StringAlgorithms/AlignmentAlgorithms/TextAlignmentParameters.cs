using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms
{
    [Serializable]
    public class TextAlignmentParameters
    {
        
        public delegate int StrategyFun(int a, int b);
        public LetterAlignmentCostManager CostArray { get; set; }     
        
        public StrategyFun Comparefunction;

        public List<Sequence> Sequences { get; set; }

        public TextAlignmentParameters()
        {
            Sequences = new List<Sequence>();
        }

        //public string GetSequence(int idx)
        //{
        //    --idx;
        //    try
        //    {
        //        return sequences[idx];
        //    }
        //    catch 
        //    {
        //        throw new Exception(String.Format("There are {0} sequences stored!",sequences.Count()));
        //    }
        //}
        //public List<string> GetSequences()
        //{
        //    return sequences;
        //}
        //public void SetSequences(List<string> sequences)
        //{
        //    this.sequences = sequences;
        //}

        public int GetNumberOfSequences()
        {
            int seqNumber = 0;
            foreach(Sequence seq in Sequences)
            {
                if(seq != null)
                {
                    ++seqNumber;
                }
            }
            return seqNumber;
        }


        public int GetIlligalValue()
        {
            int a = 1;
            int b = 0;
            int c = Comparefunction(a, b);
            if (c == 0)
            {
                return int.MaxValue / 2;
            }
            else
            {
                return int.MinValue / 2;
            }
        }


    }
}
