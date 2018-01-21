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
        protected Alignment computedAlignment;
        protected AlignmentCube array;
        protected StringBuilder firstSeqenceOfAlignment;
        protected StringBuilder secondSequenceOfAlignment;
        protected StringBuilder thirdSequenceOfAlignment;     
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
                SetAllComputableElementsToNull();
            }
        }

        private void SetAllComputableElementsToNull()
        {
            array = null;
            firstSeqenceOfAlignment = null;
            secondSequenceOfAlignment = null;
            thirdSequenceOfAlignment = null;
            computedAlignment = null;
        }
        

        public TextAlignmentAlgorithm(TextAlignmentParameters parameters)
        {
            this.parameters = parameters;
        }

        public virtual int GetOptimalAlignmentScore()
        {
            ComputeAlignmentArrayIfNecessary();
            Cube bestScoreCell = array.GetCell(array.rowSize, array.columnSize, array.depthSize);
            int optimalScore = bestScoreCell.value;
            return optimalScore;
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

        protected virtual void InitializeAlignmentArray()
        {
            //temporary
            parameters.Sequences.Add(null);
            int alignmentArrayRowNumber = parameters.Sequences[0].Value.Length;
            int alignmentArrayColumnNumber = parameters.Sequences[1].Value.Length;
            int alignmentArrayDepthNumber = parameters.Sequences?[2]?.Value.Length ?? 0;
            array = new AlignmentCube();
            array.Initialize(alignmentArrayRowNumber, alignmentArrayColumnNumber, alignmentArrayDepthNumber);

        }

        protected virtual void ComputeAlignmentArray()
        {
            List<int>[] directionsInCube = Variancy(parameters.GetNumberOfSequences());
            CubeIterator alignmentArrayIterator = array.GetIterator();

            while (alignmentArrayIterator.HasNext())
            {
                Cube currentCell = (Cube)alignmentArrayIterator.Next();

                List<int> comingFromNeighborsCosts = new List<int>();
                foreach (List<int> directionVector in directionsInCube)
                {
                    comingFromNeighborsCosts.Add(ComputeCell(currentCell, directionVector));
                }
                currentCell.value = MinFromList(comingFromNeighborsCosts);

                array.SetCell(currentCell);
            }
        }

        protected void RetrieveAlignmentIfNecessary()
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

        protected virtual void RetrieveAlignment()
        {
            Cube currentCell = array.GetCell(array.rowSize, array.columnSize, array.depthSize);

            while (currentCell.IsTopLeftCell() == false)
            {
                Cube newCell = GoOneStepBack(currentCell);

                currentCell = newCell;
            }
            MakeAlignment();
        }

        protected virtual Cube GoOneStepBack(Cube from)
        {
            string A = parameters.Sequences[0].Value;
            string B = parameters.Sequences[1].Value;
            string C = parameters.Sequences?[2]?.Value ?? "@";
            List<int>[] possibleDirectionInArray = Variancy(parameters.GetNumberOfSequences());
            //possibleDirectionInArray[0] = new List<int>() { 1, 1 };
            //possibleDirectionInArray[1] = new List<int>() { 1, 0 };
            //possibleDirectionInArray[2] = new List<int>() { 0, 1 };

            int rowIndex = from.rowIndex;
            int columnIndex = from.columnIndex;
            int depthIndex = (parameters.GetNumberOfSequences() == 2) ? 1 : from.depthIndex;


            Cube newCell = new Cube();

            foreach (List<int> directionVector in possibleDirectionInArray)
            {
                int yOffset = directionVector[0];
                int xOffset = directionVector[1];
                int zOffset = (parameters.GetNumberOfSequences() == 2) ? 1 : directionVector[2];

                int comingFromNeighborCost = ComputeCell(from, directionVector);
                if (comingFromNeighborCost == from.value)
                {
                    char a, b, c;
                    a = FetchSign(A, rowIndex, yOffset);
                    b = FetchSign(B, columnIndex, xOffset);
                    c = FetchSign(C, depthIndex, zOffset);
                    AddNextSignsOfAlignment(a, b, c);
                    newCell = array.GetCell(rowIndex - yOffset, columnIndex - xOffset, depthIndex - zOffset);
                    break;
                }
            }
            return newCell;
        }

        protected abstract void AddNextSignsOfAlignment(char a, char b, char c);

        protected abstract void MakeAlignment();

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

        protected virtual char FetchSign(string seq, int i, int iOffset)
        {
            if (iOffset == 0)
            {
                return '-';
            }
            return seq[i - iOffset];
        }

        public virtual int GetNumberOfOptimalSolutions()
        {
            ComputeAlignmentArrayIfNecessary();
            int optimalSolutionsNumber = 0;

            Cube startCell = array.GetCell(array.rowSize, array.columnSize, array.depthSize);

            optimalSolutionsNumber = CountNumberOfOptimalSolutions(startCell);
            return optimalSolutionsNumber;
        }


        protected virtual int CountNumberOfOptimalSolutions(Cube cell)
        {
            if (cell.IsTopLeftCell())
            {
                return 1;
            }

            int optimalSolutionsNumber = 0;
            List<int>[] possibleDirectionInArray = Variancy(parameters.GetNumberOfSequences());
            foreach (List<int> directionVector in possibleDirectionInArray)
            {
                int comingFromNeighborCost = ComputeCell(cell, directionVector);
                if (comingFromNeighborCost == cell.value)
                {
                    int rowIndex = cell.rowIndex - directionVector[0];
                    int columnIndex = cell.columnIndex - directionVector[1];
                    int depthIndex = (directionVector.Count() == 2) ? 0 : cell.depthIndex - directionVector[2];
                    Cube newCell = new Cube();
                    newCell = array.GetCell(rowIndex, columnIndex, depthIndex);
                    optimalSolutionsNumber += CountNumberOfOptimalSolutions(newCell);
                }
            }
            return optimalSolutionsNumber;
        }


        public List<int>[] Variancy(int k)
        {
            int i;
            int[] skok = new int[k + 1];
            for (i = 1; i <= k; ++i)
            {
                skok[i] = 1;
            }

            int m = 0;
            i = 0;
            int[] w = new int[k + 1];
            List<int>[] ret = new List<int>[(int)Math.Pow(2, k) - 1];
            for (int it = 0; it < ret.Length; ++it)
            {
                ret[it] = new List<int>();
            }
            int col = 0;
            do
            {
                ++m;
                i = Index(m) + 1;
                if (i <= k)
                {
                    w[i] = w[i] + skok[i];
                    if (w[i] == 0)
                    {
                        skok[i] = 1;
                    }
                    if (w[i] == 1)
                    {
                        skok[i] = -1;
                    }
                }
                else
                {
                    return ret;
                    //w[i - 1] = w[i - 1] + skok[i - 1];
                }
                for (int j = 1; j <= k; ++j)
                {
                    ret[col].Add(w[j]);
                }
                ++col;
            } while (i <= k);
            return ret;
        }

        protected int Index(int m)
        {
            int i = 0;
            while (m % 2 == 0)
            {
                i++;
                m = m / 2;
            }
            return i;
        }

        protected int MinFromList(List<int> list)
        {
            int min = list.First();
            foreach (int value in list)
            {
                min = parameters.Comparefunction(min, value);
            }
            return min;
        }
    }
}
