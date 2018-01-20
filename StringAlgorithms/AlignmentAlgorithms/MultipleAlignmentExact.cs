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
        private new int[, ,] alignmentArray;
        private AlignmentCube alignmentCube = null;
        private Alignment computedAlignment = null;
        private StringBuilder firstSeqenceOfAlignment = null;
        private StringBuilder secondSequenceOfAlignment = null;
        private StringBuilder thirdSequenceOfAlignment = null;

        public MultipleAlignmentExact(TextAlignmentParameters parameters) : base(parameters)
        {

        }



        public override Alignment GetOptimalAlignment()
        {
            ComputeAlignmentArrayIfNecessary();
            RetrieveAlignmentIfNecessary();
            return computedAlignment;
        }

        private void ComputeAlignmentArrayIfNecessary()
        {
            if (this.alignmentArray == null)
            {
                InitializeAlignmentArray();
                ComputeAlignmentArray();
            }
        }

        private void RetrieveAlignmentIfNecessary()
        {
            if (computedAlignment == null)//TODO: dodać przy zmanie parametrów, że null itd
            {
                InitializeSequencesOfAlignment();
                RetrieveAlignment();
            }
        }

        private void InitializeSequencesOfAlignment()
        {
            firstSeqenceOfAlignment = new StringBuilder();
            secondSequenceOfAlignment = new StringBuilder();
            thirdSequenceOfAlignment = new StringBuilder();
        }

        private void InitializeAlignmentArray()
        {
            int i = parameters.Sequences[0].Value.Length;
            int j = parameters.Sequences[1].Value.Length;
            int k = parameters.Sequences[2].Value.Length;
            this.alignmentArray = new int[i + 1, j + 1, k + 1];

            alignmentCube = new AlignmentCube();
            alignmentCube.Initialize(i, j, k); 

        }

        protected override void ComputeAlignmentArray()
        {
            List<int>[] directionsInCube = Variancy(3);
            CubeIterator alignmentArrayIterator = alignmentCube.GetIterator();

            while (alignmentArrayIterator.HasNext())
            {
                Cube currentCell = (Cube)alignmentArrayIterator.Next();

                List<int> comingFromNeighborsCosts = new List<int>();
                foreach (List<int> directionVector in directionsInCube)
                {
                    comingFromNeighborsCosts.Add(ComputeCell(currentCell.rowIndex, currentCell.columnIndex, currentCell.depthIndex, directionVector[0], directionVector[1], directionVector[2]));
                }

                alignmentArray[currentCell.rowIndex, currentCell.columnIndex, currentCell.depthIndex] = MinFromList(comingFromNeighborsCosts);
                currentCell.value = MinFromList(comingFromNeighborsCosts);
                alignmentCube.SetCell(currentCell);
            }
        }

        private int ComputeCell(int i, int j, int k, int iOffset, int jOffset, int kOffset)
        {
            if (i - iOffset >= 0 && j - jOffset >= 0 && k - kOffset >= 0)
            {
                string A = parameters.Sequences[0].Value;
                string B = parameters.Sequences[1].Value;
                string C = parameters.Sequences[2].Value;
                char a, b, c;
                a = FetchSign(A, i, iOffset);
                b = FetchSign(B, j, jOffset);
                c = FetchSign(C, k, kOffset);
                return alignmentArray[i - iOffset, j - jOffset, k - kOffset] + ComputeAligningValue(a, b, c);
            }
            return int.MaxValue - 1000;
        }


        private int MinFromList(List<int> list)
        {
            int min = list.First();
            foreach(int value in list)
            {
                if (value < min)
                    min = value;
            }
            return min;
        }

        private int ComputeAligningValue(char a, char b, char c)
        {
            int score = 0;
            score += parameters.CostArray.GetLettersAlignmentCost(a, b);
            score += parameters.CostArray.GetLettersAlignmentCost(b, c);
            score += parameters.CostArray.GetLettersAlignmentCost(a, c);
            return score;
        }

        private char FetchSign(string seq, int i, int iOffset)
        {
            if(iOffset == 0)
            {
                return '-';
            }
            return seq[i - iOffset];
        }


        private Alignment RetrieveAlignment()
        {
            firstSeqenceOfAlignment = new StringBuilder();
            secondSequenceOfAlignment = new StringBuilder();
            thirdSequenceOfAlignment = new StringBuilder();
            computedAlignment = new Alignment();

            Cube currentCell = alignmentCube.GetCell(alignmentCube.rowSize, alignmentCube.columnSize, alignmentCube.depthSize);

            while (currentCell.IsTopLeftCell() == false)
            {
                Cube newCell = GoOneStepBack(currentCell);

                currentCell = newCell;
            }
            Sequence firstAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[0].Name, firstSeqenceOfAlignment.ToString()); ///TODO BRZYDKIE!!!!!
            Sequence secondAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[1].Name, secondSequenceOfAlignment.ToString());
            Sequence thirdAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[2].Name, thirdSequenceOfAlignment.ToString());
            computedAlignment = new Alignment(new List<Sequence>(){ firstAlignmentSeq, secondAlignmentSeq, thirdAlignmentSeq });
            return computedAlignment;
        }

        private Cube GoOneStepBack(Cube from)
        {
            string A = parameters.Sequences[0].Value;
            string B = parameters.Sequences[1].Value;
            string C = parameters.Sequences[2].Value;
            List<int>[] possibleDirectionInArray = Variancy(3);

            Cube newCell = new Cube();

            foreach (List<int> directionVector in possibleDirectionInArray)
            {
                int comingFromNeighborCost = ComputeCell(from.rowIndex, from.columnIndex, from.depthIndex, directionVector[0], directionVector[1], directionVector[2]);
                if (comingFromNeighborCost == from.value)
                {
                    char a, b, c;
                    a = FetchSign(A, from.rowIndex, directionVector[0]);
                    b = FetchSign(B, from.columnIndex, directionVector[1]);
                    c = FetchSign(C, from.depthIndex, directionVector[2]);
                    firstSeqenceOfAlignment.Insert(0, a);
                    secondSequenceOfAlignment.Insert(0, b);
                    thirdSequenceOfAlignment.Insert(0, c);
                    newCell = alignmentCube.GetCell(from.rowIndex - directionVector[0], from.columnIndex - directionVector[1], from.depthIndex - directionVector[2]);
                    break;
                }
            }
            return newCell;
        }




        public override int GetOptimalAlignmentScore()
        {
            ComputeAlignmentArrayIfNecessary();
            Cube bestScoreCell = alignmentCube.GetCell(alignmentCube.rowSize, alignmentCube.columnSize, alignmentCube.depthSize);
            int optimalScore = bestScoreCell.value;
            return optimalScore;
        }

        private int Index(int m)
        {
            int i = 0;
            while (m % 2 == 0)
            {
                i++;
                m = m / 2;
            }
            return i;
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
            List<int>[] ret = new List<int>[(int)Math.Pow(2, k)-1];
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

        public override int GetNumberOfOptimalSolutions()
        {
            ComputeAlignmentArrayIfNecessary();
            int optimalSolutionsNumber = 0;

            Cube startCell = alignmentCube.GetCell(alignmentCube.rowSize, alignmentCube.columnSize, alignmentCube.depthSize);

            optimalSolutionsNumber = CountNumberOfOptimalSolutions(startCell);
            return optimalSolutionsNumber;
        }


        private int CountNumberOfOptimalSolutions(Cube cell)
        {
            if (cell.IsTopLeftCell())
            {
                return 1;
            }

            int optimalSolutionsNumber = 0;
            List<int>[] possibleDirectionInArray = Variancy(3);
            foreach (List<int> directionVector in possibleDirectionInArray)
            {
                int comingFromNeighborCost = ComputeCell(cell.rowIndex, cell.columnIndex, cell.depthIndex, directionVector[0], directionVector[1], directionVector[2]);
                if (comingFromNeighborCost == cell.value)
                {
                    Cube newCell = new Cube();
                    newCell = alignmentCube.GetCell(cell.rowIndex - directionVector[0], cell.columnIndex - directionVector[1], cell.depthIndex - directionVector[2]);
                    optimalSolutionsNumber += CountNumberOfOptimalSolutions(newCell);
                }
            }
            return optimalSolutionsNumber;
        }


    }
}
