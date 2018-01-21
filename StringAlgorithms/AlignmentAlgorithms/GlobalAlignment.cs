using StringAlgorithms.Enums;
using StringAlgorithms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace StringAlgorithms
{
    public class GlobalAlignment : TextAlignmentAlgorithm
    {
        private const int illegalValue = Int32.MinValue;
        

        public GlobalAlignment(TextAlignmentParameters parameters) : base(parameters)
        {
           
        }

        protected override int ComputeAligningValue(char a, char b, char c)
        {
            int score = 0;
            score += parameters.CostArray.GetLettersAlignmentCost(a, b);
            score += parameters.CostArray.GetLettersAlignmentCost(b, c);
            score += parameters.CostArray.GetLettersAlignmentCost(a, c);
            return score;
        }

        protected override void MakeAlignment()
        {
            Sequence firstAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[0].Name, firstSeqenceOfAlignment.ToString()); ///TODO BRZYDKIE!!!!!
            Sequence secondAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[1].Name, secondSequenceOfAlignment.ToString());
            computedAlignment = new Alignment(firstAlignmentSeq, secondAlignmentSeq);
        }

        protected override void AddNextSignsOfAlignment(char a, char b, char c)
        {
            firstSeqenceOfAlignment.Insert(0, a);
            secondSequenceOfAlignment.Insert(0, b);
        }
    }
}
