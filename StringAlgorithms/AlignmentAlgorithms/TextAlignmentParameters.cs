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
            return Sequences.Count();
        }
        //public void RemoveSequence(int idx)
        //{
        //    --idx;
        //    try
        //    {
        //        sequences.RemoveAt(idx);
        //    }
        //    catch
        //    {
        //        throw new Exception(String.Format("There is not any sequence under given index!"));
        //    }
        //}
        //public void AddSequence(string seq)
        //{
        //    sequences.Add(seq);
        //}

       
    }
}
