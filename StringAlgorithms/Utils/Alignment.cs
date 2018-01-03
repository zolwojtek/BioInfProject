using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms
{
    public class Alignment
    {
        public List<Sequence> Sequences { get; set; } = new List<Sequence>();
        public int Length { get; set; }

        public Alignment()
        {

        }

        public Alignment(Sequence seq1, Sequence seq2)
        {
            Sequences.Add(seq1);
            Sequences.Add(seq2);
            this.Length = seq1.Value.Length;
        }

        public Alignment(List<Sequence> sequences)
        {
            this.Sequences = sequences;
            this.Length = Sequences.First().Value.Length;
        }

        public char GetSign(int sequenceIndex, int charIndex)
        {
                return this.Sequences[sequenceIndex].Value[charIndex];
        }

   
    }
}
