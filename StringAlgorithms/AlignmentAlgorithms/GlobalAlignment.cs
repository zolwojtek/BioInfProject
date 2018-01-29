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
        

        public GlobalAlignment(TextAlignmentParameters parameters) : base(parameters)
        {
            illegalValue = Int32.MinValue;
        }




    }
}
