using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StringAlgorithms;
using System.IO;

namespace SequenceParser
{
    public class DnaParser : SequenceParser
    {
        public DnaParser()
        {
            chars = new char[] { 'A', 'C', 'G', 'T' };
        }


    }
}
