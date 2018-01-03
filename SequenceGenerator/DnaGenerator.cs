using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StringAlgorithms;
using System.Threading;

namespace SequenceGenerator
{
    public class DnaGenerator
    {
        public string GenerateSequence(int length)
        {
            string chars = StringAlgorithms.Constants.DNA;
            string sequence = string.Empty;
            Random rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            for (int i = 0; i < length; ++i)
            {
                Thread.Sleep(1);
                int id = (rand.Next() + (int)DateTime.Now.Millisecond) % 4;
                if(id < 0)
                {
                    id *= -1;
                    id %= 4;
                }
                sequence += chars[id];
            }
            return sequence;
        }
    }
}
