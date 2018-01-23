using StringAlgorithms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms
{
    public class MultipleAlignmentExact : TextAlignmentAlgorithm
    {

        public MultipleAlignmentExact(TextAlignmentParameters parameters) : base(parameters)
        {

        }

        protected override void InitializeSequencesOfAlignment()
        {
            base.InitializeSequencesOfAlignment();
            thirdSequenceOfAlignment = new StringBuilder();
        }


        protected override void MakeAlignment()
        {
            Sequence firstAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[0].Name, firstSeqenceOfAlignment.ToString()); ///TODO BRZYDKIE!!!!!
            Sequence secondAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[1].Name, secondSequenceOfAlignment.ToString());
            Sequence thirdAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[2].Name, thirdSequenceOfAlignment.ToString());
            computedAlignment = new Alignment(new List<Sequence>() { firstAlignmentSeq, secondAlignmentSeq, thirdAlignmentSeq });
        }


        protected override void AddNextSignsOfAlignment(char a, char b, char c)
        {
            firstSeqenceOfAlignment.Insert(0, a);
            secondSequenceOfAlignment.Insert(0, b);
            thirdSequenceOfAlignment.Insert(0, c);
        }
    }
}
