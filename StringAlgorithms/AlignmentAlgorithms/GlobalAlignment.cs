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
        private Alignment computedAlignment = null;
        private StringBuilder firstSeqenceOfAlignment = null;
        private StringBuilder secondSequenceOfAlignment = null;
        private AlignmentArray array;

        public GlobalAlignment(TextAlignmentParameters parameters): base(parameters)
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

        private void InitializeAlignmentArray()
        {
            int alignmentArrayRowNumber = parameters.Sequences[0].Value.Length;
            int alignmentArrayColumnNumber = parameters.Sequences[1].Value.Length;

            array = new AlignmentArray();
            array.Initialize(alignmentArrayRowNumber, alignmentArrayColumnNumber, (x) => (parameters.CostArray.GapCostFun(x)));
        }

        protected override void ComputeAlignmentArray()
        {
            ArrayIterator alignmentArrayIterator = array.GetIterator();
            while (alignmentArrayIterator.HasNext())
            {
                Cell currentCell = (Cell)alignmentArrayIterator.Next();
                ComputeCell(currentCell);
            }
        }

        private void ComputeCell(Cell cell)
        {
            ComputeGoingFromNeighborsCosts(cell);
            cell.value = GetTheBestComingFromNeigborCost();
            array.SetCell(cell);
        }

        private void ComputeGoingFromNeighborsCosts(Cell destinationCell)
        {
            fromDiagonalNeighborCost = ComputeCostOfMatching(destinationCell.GetUpDiagonalNeighbor(), AlignmentType.MATCH_SIGNS);
            fromUpNeighborCost = ComputeCostOfMatching(destinationCell.GetUpNeighbor(), AlignmentType.MATCH_WITH_GAP);
            fromLeftNeighborCost = ComputeCostOfMatching(destinationCell.GetLeftNeighbor(), AlignmentType.MATCH_WITH_GAP);
        }

        private int ComputeCostOfMatching(Cell fromCell, AlignmentType alignmentType)
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
                return array.GetCell(fromCell.rowIndex, fromCell.columnIndex).value + costOfSignsAlignment;
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

        private void InitializeSequencesOfAlignment()
        {
            firstSeqenceOfAlignment = new StringBuilder("");
            secondSequenceOfAlignment = new StringBuilder("");
        }

        private void RetrieveAlignment()
        {       
            ArrayIterator iterator = array.GetIterator();
            iterator.SetToCell(new Cell(array.rowSize, array.columnSize));
            Cell currentCell = (Cell)iterator.GetCurrentCell();

            while (currentCell.IsTopLeftCell() == false)
            {
                Cell newCell = GoOneStepBack(currentCell, iterator);
                Direction direction = GetDirection(currentCell, newCell);

                FetchSignsOfAlignment(newCell.rowIndex, newCell.columnIndex, direction);
                currentCell = newCell;
            }
            Sequence firstAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[0].Name, firstSeqenceOfAlignment.ToString()); ///TODO BRZYDKIE!!!!!
            Sequence secondAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[1].Name, secondSequenceOfAlignment.ToString());
            computedAlignment = new Alignment(firstAlignmentSeq, secondAlignmentSeq);
        }

        private Cell GoOneStepBack(Cell from, ArrayIterator iterator)
        {
            Cell newCell = new Cell();

            ComputeGoingFromNeighborsCosts(from);
            int costOfCurrentCell = from.value;

            if (costOfCurrentCell.Equals(fromDiagonalNeighborCost))
            {
                newCell = (Cell)iterator.Diagonal();
            }
            else if (costOfCurrentCell.Equals(fromUpNeighborCost))
            {
                newCell = (Cell)iterator.Up();
            }
            else if (costOfCurrentCell.Equals(fromLeftNeighborCost))
            {
                newCell = (Cell)iterator.Left();
            }
            return newCell;
        }

        Direction GetDirection(Cell from, Cell to)
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

        private bool IsTopLeftCellOfAlignmentArray(Cell cell)
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
            Cell bestScoreCell = array.GetCell(array.rowSize, array.columnSize);
            int optimalScore = bestScoreCell.value;
            return optimalScore;
        }

        public override int GetNumberOfOptimalSolutions()
        {
            ComputeAlignmentArrayIfNecessary();
            int optimalSolutionsNumber = 0;
            ArrayIterator iterator = array.GetIterator();
            iterator.SetToCell(new Cell(array.rowSize, array.columnSize));
            optimalSolutionsNumber = CountNumberOfOptimalSolutions(iterator);
            return optimalSolutionsNumber;
        }

        private int CountNumberOfOptimalSolutions(ArrayIterator iterator)
        {
            int optimalSolutionsNumber = 0;
            Cell cell = (Cell)iterator.GetCurrentCell();
            if (cell.IsTopLeftCell())
            {
                return 1;
            }
            
            int costOfCurrentCell = cell.value;

            if (iterator.HasDiagonal())
            {
                ComputeGoingFromNeighborsCosts(cell);
                if (costOfCurrentCell.Equals(fromDiagonalNeighborCost))
                {
                    ArrayIterator diagIterator = array.GetIterator();
                    diagIterator.SetToCell(cell);
                    diagIterator.Diagonal();
                    optimalSolutionsNumber += CountNumberOfOptimalSolutions(diagIterator);
                }
            }
            if (iterator.HasUp())
            {
                ComputeGoingFromNeighborsCosts(cell);
                if (costOfCurrentCell.Equals(fromUpNeighborCost))
                {
                    ArrayIterator upIterator = array.GetIterator();
                    upIterator.SetToCell(cell);
                    upIterator.Up();
                    optimalSolutionsNumber += CountNumberOfOptimalSolutions(upIterator);
                }
            }
            if (iterator.HasLeft())
            {
                ComputeGoingFromNeighborsCosts(cell);
                if (costOfCurrentCell.Equals(fromLeftNeighborCost))
                {
                    ArrayIterator leftIterator = array.GetIterator();
                    leftIterator.SetToCell(cell);
                    leftIterator.Left();
                    optimalSolutionsNumber += CountNumberOfOptimalSolutions(leftIterator);
                }
            }
            return optimalSolutionsNumber;
        }
    }
}
