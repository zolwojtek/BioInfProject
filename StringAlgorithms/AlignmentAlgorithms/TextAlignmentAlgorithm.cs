using StringAlgorithms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms
{
    public abstract class TextAlignmentAlgorithm
    {
        //protected volatile int[,] alignmentArray;
        protected Alignment computedAlignment;
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
                array = null;
            }
        }
        protected StringBuilder firstSeqenceOfAlignment = null;
        protected StringBuilder secondSequenceOfAlignment = null;
        protected StringBuilder thirdSequenceOfAlignment = null;
        protected AlignmentCube array;

        public TextAlignmentAlgorithm(TextAlignmentParameters parameters)
        {
            this.parameters = parameters;
        }


        public virtual Alignment GetOptimalAlignment()
        {
            ComputeAlignmentArrayIfNecessary();
            RetrieveAlignmentIfNecessary();
            return computedAlignment;
        }

        protected virtual void ComputeAlignmentArrayIfNecessary()
        {
            if (this.array == null)
            {
                InitializeAlignmentArray();
                ComputeAlignmentArray();
            }
        }

        protected abstract void InitializeAlignmentArray();

        protected abstract void ComputeAlignmentArray();

        private void RetrieveAlignmentIfNecessary()
        {
            if (computedAlignment == null)//TODO: dodać przy zmanie parametrów, że null itd
            {
                InitializeSequencesOfAlignment();
                RetrieveAlignment();
            }
        }

        protected virtual void InitializeSequencesOfAlignment()
        {
            firstSeqenceOfAlignment = new StringBuilder("");
            secondSequenceOfAlignment = new StringBuilder("");
        }

        protected abstract void RetrieveAlignment();

        protected virtual int ComputeCell(Cube cell, List<int> directionVector)
        {
            int rowIndex = cell.rowIndex;
            int columnIndex = cell.columnIndex;
            int depthIndex = (directionVector.Count() == 2) ? 1 : cell.depthIndex;
            int yOffset = directionVector[0];
            int xOffset = directionVector[1];
            int zOffset = (directionVector.Count() == 2) ? 1 : directionVector[2];

            if (rowIndex - yOffset >= 0 && columnIndex - xOffset >= 0 && depthIndex - zOffset >= 0)
            {
                string A = parameters.Sequences[0].Value;
                string B = parameters.Sequences[1].Value;
                string C = parameters.Sequences?[2]?.Value ?? "@";
                char a, b, c;
                a = FetchSign(A, rowIndex, yOffset);
                b = FetchSign(B, columnIndex, xOffset);
                c = FetchSign(C, depthIndex, zOffset);

                return array.GetCellValue(rowIndex - yOffset, columnIndex - xOffset, depthIndex - zOffset) + ComputeAligningValue(a, b, c);
            }
            return parameters.GetIlligalValue();
        }

        protected virtual int ComputeAligningValue(char a, char b, char c)
        {
            return 0;
        }
        protected abstract char FetchSign(string a, int rowIndex, int yOffset);

        public abstract int GetOptimalAlignmentScore();
        public abstract int GetNumberOfOptimalSolutions();
        
    }
}
