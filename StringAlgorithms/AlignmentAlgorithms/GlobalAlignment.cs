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
        private int fromDiagonalNeighborCost = 0;
        private int fromUpNeighborCost = 0;
        private int fromLeftNeighborCost = 0;
        private const int illegalValue = Int32.MinValue;
        

        public GlobalAlignment(TextAlignmentParameters parameters) : base(parameters)
        {

        }

        //later to extract to TextAlignmentAlgorithm
        protected override void InitializeAlignmentArray()
        {
            int alignmentArrayRowNumber = parameters.Sequences[0].Value.Length;
            int alignmentArrayColumnNumber = parameters.Sequences[1].Value.Length;

            array = new AlignmentCube();
            array.Initialize(alignmentArrayRowNumber, alignmentArrayColumnNumber, 0);
            //temporary
            parameters.Sequences.Add(null);
        }



        protected override void ComputeAlignmentArray()
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
                //NOT MIN - COMPAREFUN
                currentCell.value = MinFromList(comingFromNeighborsCosts);
                array.SetCell(currentCell);
            }
        }


        protected override int ComputeAligningValue(char a, char b, char c)
        {
            int score = 0;
            score += parameters.CostArray.GetLettersAlignmentCost(a, b);
            score += parameters.CostArray.GetLettersAlignmentCost(b, c);
            score += parameters.CostArray.GetLettersAlignmentCost(a, c);
            return score;
        }

        protected override char FetchSign(string seq, int i, int iOffset)
        {
            if (iOffset == 0)
            {
                return '-';
            }
            return seq[i - iOffset];
        }

        private void ComputeGoingFromNeighborsCosts(Cube destinationCell)
        {
            fromDiagonalNeighborCost = ComputeCostOfMatching(destinationCell.GetUpDiagonalNeighbor(), AlignmentType.MATCH_SIGNS);
            fromUpNeighborCost = ComputeCostOfMatching(destinationCell.GetUpNeighbor(), AlignmentType.MATCH_WITH_GAP);
            fromLeftNeighborCost = ComputeCostOfMatching(destinationCell.GetLeftNeighbor(), AlignmentType.MATCH_WITH_GAP);
        }

        private int ComputeCostOfMatching(Cube fromCell, AlignmentType alignmentType)
        {
            if (fromCell.rowIndex >= 0 && fromCell.columnIndex >= 0)
            {
                int costOfSignsAlignment = 0;
                if (alignmentType == AlignmentType.MATCH_WITH_GAP)
                    costOfSignsAlignment = parameters.CostArray.GapCostFun(1); //takie srednie
                else if (alignmentType == AlignmentType.MATCH_SIGNS)
                {
                    costOfSignsAlignment = parameters.CostArray.GetLettersAlignmentCost(parameters.Sequences[0].Value[fromCell.rowIndex], parameters.Sequences[1].Value[fromCell.columnIndex]);//slabe
                }
                return array.GetCell(fromCell.rowIndex, fromCell.columnIndex, fromCell.depthIndex).value + costOfSignsAlignment;
            }
            return illegalValue;
        }

        private int GetTheBestComingFromNeigborCost()
        {
            int bestCost = parameters.Comparefunction(fromDiagonalNeighborCost, parameters.Comparefunction(fromUpNeighborCost, fromLeftNeighborCost));
            return bestCost;
        }

        private void RetrieveAlignmentIfNecessary()
        {
            if (computedAlignment == null)//TODO: dodać przy zmanie parametrów, że null itd
            {
                InitializeSequencesOfAlignment();
                RetrieveAlignment();
            }
        }


        protected override void RetrieveAlignment()
        {
            Cube currentCell = array.GetCell(array.rowSize, array.columnSize, 0);

            while (currentCell.IsTopLeftCell() == false)
            {
                Cube newCell = GoOneStepBack(currentCell);
                Direction direction = GetDirection(currentCell, newCell);

                FetchSignsOfAlignment(newCell.rowIndex, newCell.columnIndex, direction);
                currentCell = newCell;
            }
            Sequence firstAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[0].Name, firstSeqenceOfAlignment.ToString()); ///TODO BRZYDKIE!!!!!
            Sequence secondAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[1].Name, secondSequenceOfAlignment.ToString());
            computedAlignment = new Alignment(firstAlignmentSeq, secondAlignmentSeq);
        }

        private Cube GoOneStepBack(Cube from)
        {
            List<int>[] possibleDirectionInArray = Variancy(2);
            //possibleDirectionInArray[0] = new List<int>() { 1, 1 };
            //possibleDirectionInArray[1] = new List<int>() { 1, 0 };
            //possibleDirectionInArray[2] = new List<int>() { 0, 1 };

            Cube newCell = new Cube();

            foreach(List<int> directionVector in possibleDirectionInArray)
            {
                int comingFromNeighborCost = ComputeCell(from, directionVector);
                if(comingFromNeighborCost == from.value)
                {
                    newCell = array.GetCell(from.rowIndex - directionVector[0], from.columnIndex - directionVector[1],0);
                    break;
                }
            }
            return newCell;
        }


        Direction GetDirection(Cube from, Cube to)
        {
            int verticalDiff = from.rowIndex - to.rowIndex;
            int horizontalDiff = from.columnIndex - to.columnIndex;
            Direction direction;
            if (verticalDiff != 0 && horizontalDiff != 0)
            {
                direction = Direction.DIAGONAL;
            }
            else if (verticalDiff != 0)
            {
                direction = Direction.UP;
            }
            else
            {
                direction = Direction.LEFT;
            }
            return direction;
        }

       

        private bool IsTopLeftCellOfAlignmentArray(Cube cell)
        {
            if(cell.rowIndex == 0 && cell.columnIndex == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void FetchSignsOfAlignment(int indexInFirstSequence, int indexInSecondSequence, Direction direction) //TODO oddzialić pobieranie znaków od kolumny i wiersza current Cell...to tylko wskazuje czy mamy wziac znak czy '-'
        {
            char signFirst = '-';
            char signSecond = '-';
            switch (direction)
            {           
                case Direction.DIAGONAL:
                    signFirst = parameters.Sequences[0].Value[indexInFirstSequence];
                    signSecond = parameters.Sequences[1].Value[indexInSecondSequence];
                    break;
                case Direction.LEFT:
                    signFirst = '-';
                    signSecond = parameters.Sequences[1].Value[indexInSecondSequence];
                    break;
                case Direction.UP:
                    signFirst = parameters.Sequences[0].Value[indexInFirstSequence];
                    signSecond = '-';
                    break;
            }
            firstSeqenceOfAlignment.Insert(0, signFirst);
            secondSequenceOfAlignment.Insert(0, signSecond);
        }

        public override int GetOptimalAlignmentScore()
        {
            ComputeAlignmentArrayIfNecessary();
            Cube bestScoreCell = array.GetCell(array.rowSize, array.columnSize,0);
            int optimalScore = bestScoreCell.value;
            return optimalScore;
        }

        public override int GetNumberOfOptimalSolutions()
        {
            ComputeAlignmentArrayIfNecessary();
            int optimalSolutionsNumber = 0;

            Cube startCell = array.GetCell(array.rowSize, array.columnSize,0);

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
            List<int>[] possibleDirectionInArray = Variancy(2);
            foreach (List<int> directionVector in possibleDirectionInArray)
            {
                int comingFromNeighborCost = ComputeCell(cell, directionVector);
                if (comingFromNeighborCost == cell.value)
                {
                    Cube newCell = new Cube();
                    newCell = array.GetCell(cell.rowIndex - directionVector[0], cell.columnIndex - directionVector[1],0);
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

        private int MinFromList(List<int> list)
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
