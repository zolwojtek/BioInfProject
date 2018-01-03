using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms
{  
    public abstract class TextAlignmentAlgorithm
    {
        protected volatile int[,] alignmentArray;
        protected TextAlignmentParameters parameters;
        public TextAlignmentParameters Parameters
        {
            get
            {
                return parameters;
            }
            set
            {
                parameters = value;
                alignmentArray = null;
            }
        }

        public TextAlignmentAlgorithm(TextAlignmentParameters parameters)
        {
            this.parameters = parameters;
        }

        //protected abstract int ComputeCell(int i, int j, AlignmentType aligmentType);
        //protected abstract void ComputeBlock(int rowStart, int colStart, int rowNum, int colNum);
        protected abstract void ComputeAlignmentArray();
        public abstract int GetOptimalAlignmentScore();
        public abstract int GetNumberOfOptimalSolutions();
        public abstract Alignment GetOptimalAlignment();

    }
}
