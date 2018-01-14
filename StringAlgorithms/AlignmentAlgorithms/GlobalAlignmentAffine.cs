using StringAlgorithms.Enums;
using StringAlgorithms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms
{
    public class GlobalAlignmentAffine : TextAlignmentAlgorithm
    {
        private int verticalGapsCost;
        private int horizontalGapsCost;
        private int fromDiagonalNeighborCost = 0;
        private int fromUpNeighborCost = 0;
        private int fromLeftNeighborCost = 0;
        private AlignmentArray alignmentArray2;
        private AlignmentArray helpArrayP;
        private AlignmentArray helpArrayQ;
        private Alignment computedAlignment = null;
        private StringBuilder firstSeqenceOfAlignment = null;
        private StringBuilder secondSequenceOfAlignment = null;

        public GlobalAlignmentAffine(TextAlignmentParameters parameters) : base(parameters)
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

            alignmentArray2 = new AlignmentArray();
            helpArrayP = new AlignmentArray();
            helpArrayQ = new AlignmentArray();

            alignmentArray2.Initialize(alignmentArrayRowNumber, alignmentArrayColumnNumber, (x) => (parameters.CostArray.GapCostFun(x)));
            helpArrayP.Initialize(alignmentArrayRowNumber, alignmentArrayColumnNumber);
            helpArrayQ.Initialize(alignmentArrayRowNumber, alignmentArrayColumnNumber);

            helpArrayP.FillRowWithValue(0, (x) => (int.MaxValue / 2));
            helpArrayQ.FillColumnWithValue(0, (x) => (int.MaxValue / 2));
        }

        protected override void ComputeAlignmentArray()
        {
            ArrayIterator alignmentArrayIterator = alignmentArray2.GetIterator();
            while (alignmentArrayIterator.HasNext())
            {
                Cell currentCell = (Cell)alignmentArrayIterator.Next();
                ComputeCell(currentCell);
            }
        }

        private void ComputeCell(Cell cell)
        {
            int cellRow = cell.rowIndex;
            int celColumn = cell.columnIndex;

            ComputeGoingFromNeighborsCosts(cell);
            cell.value = GetTheBestComingFromNeigborCost();
            alignmentArray2.SetCell(cell);

            Cell cellP = new Cell(cellRow, celColumn, fromUpNeighborCost);
            Cell cellQ = new Cell(cellRow, celColumn, fromLeftNeighborCost);
            helpArrayP.SetCell(cellP);
            helpArrayQ.SetCell(cellQ);
        }

        private void ComputeGoingFromNeighborsCosts(Cell destinationCell)
        {
            fromUpNeighborCost = ComputeCostOfMatching(destinationCell.GetUpNeighbor(), helpArrayP, AlignmentType.MATCH_WITH_GAP);
            fromLeftNeighborCost = ComputeCostOfMatching(destinationCell.GetLeftNeighbor(), helpArrayQ, AlignmentType.MATCH_WITH_GAP);
            fromDiagonalNeighborCost = ComputeCostOfMatching(destinationCell.GetUpDiagonalNeighbor(), null, AlignmentType.MATCH_SIGNS);
        }

        private int ComputeCostOfMatching(Cell cell, AlignmentArray helpArray, AlignmentType alignmentType)
        {
            int cost = 0;
            if (alignmentType == AlignmentType.MATCH_WITH_GAP)
            {
                cost = ComputeCostOfMatchingWithGap(cell, helpArray);
            }
            else if (alignmentType == AlignmentType.MATCH_SIGNS)
            {
                cost = ComputeCostOfMatchingSigns(cell);
            }
            return cost;
        }

        private int ComputeCostOfMatchingWithGap(Cell cell, AlignmentArray helpArray)
        {
            int cellRow = cell.rowIndex;
            int cellColumn = cell.columnIndex;
            int gapStartingCost = alignmentArray2.GetCell(cellRow, cellColumn).value + parameters.CostArray.GetGapStartingCost();
            int gapExtensionCost = helpArray.GetCell(cellRow, cellColumn).value + parameters.CostArray.GetGapExtensionCost();
            return parameters.Comparefunction(gapStartingCost, gapExtensionCost);
        }

        private int ComputeCostOfMatchingSigns(Cell cell)
        {
            int cellRow = cell.rowIndex;
            int cellColumn = cell.columnIndex;
            int cost = alignmentArray2.GetCell(cellRow, cellColumn).value + parameters.CostArray.GetLettersAlignmentCost(parameters.Sequences[0].Value[cellRow], parameters.Sequences[1].Value[cellColumn]);
            return cost;
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
            ArrayIterator iterator = alignmentArray2.GetIterator();
            iterator.SetToCell(new Cell(alignmentArray2.rowSize, alignmentArray2.columnSize));
            Cell currentCell = (Cell)iterator.GetCurrentCell();

            while (currentCell.IsTopLeftCell() == false)
            {
                Cell newCell = GoOneStepBack(currentCell, iterator);
                Direction direction = GetDirection(currentCell, newCell);

                
                if(direction == Direction.DIAGONAL)
                {
                    FetchSignsOfAlignment(newCell.rowIndex, newCell.columnIndex, direction);
                }
                else 
                {
                    int gapLength = 1;
                    if (direction == Direction.LEFT)
                    {
                        gapLength = currentCell.columnIndex - newCell.columnIndex;
                    }
                    else if (direction == Direction.UP)
                    {
                        gapLength = currentCell.rowIndex - newCell.rowIndex;
                    }
                    FetchSeqeunceOfGapsInAlignment(gapLength, newCell, direction);
                }
                currentCell = newCell;
            }
            Sequence firstAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[0].Name, firstSeqenceOfAlignment.ToString());// answerSeq1);
            Sequence secondAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[1].Name, secondSequenceOfAlignment.ToString());// answerSeq2);
            computedAlignment = new Alignment(firstAlignmentSeq, secondAlignmentSeq);
        }

        private Cell GoOneStepBack(Cell from, ArrayIterator iterator)
        {
            Cell newCell;
            int diagonalCost = ComputeCostOfMatchingSigns(from.GetUpDiagonalNeighbor());
            int costOfCurrentCell = from.value;

            if (costOfCurrentCell.Equals(diagonalCost))
            {
                newCell = (Cell)iterator.Diagonal();
            }
            else
            {
                int gapLength = 1;
                while (true)
                {
                    ComputeCostOfHavingNGaps(gapLength, from);
                    if (costOfCurrentCell.Equals(verticalGapsCost))
                    {
                        newCell = alignmentArray2.GetCell(from.rowIndex - gapLength, from.columnIndex);
                        break;
                    }
                    else if (costOfCurrentCell.Equals(horizontalGapsCost))
                    {
                        newCell = alignmentArray2.GetCell(from.rowIndex, from.columnIndex - gapLength);
                        break;
                    }
                    ++gapLength;
                }
                iterator.SetToCell(newCell);
            }
            return newCell;
        }

        private void ComputeCostOfHavingNGaps(int gapLength, Cell from)
        {
            int nGapCost = parameters.CostArray.GapCostFun(gapLength);
            if (from.columnIndex - gapLength >= 0)
            {
                horizontalGapsCost = alignmentArray2.GetCell(from.rowIndex, from.columnIndex - gapLength).value + nGapCost;
            }
            else
            {
                horizontalGapsCost = int.MaxValue / 2;
            }
            if (from.rowIndex - gapLength >= 0)
            {
                verticalGapsCost = alignmentArray2.GetCell(from.rowIndex - gapLength, from.columnIndex).value + nGapCost;
            }
            else
            {
                verticalGapsCost = int.MaxValue / 2;
            }
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

        private void FetchSignsOfAlignment(int indexInFirstSequence, int indexInSecondSequence, Direction direction)
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

        private void FetchSeqeunceOfGapsInAlignment(int gapLength, Cell startingCell, Direction direction)
        {
            int fetchedCellRow = startingCell.rowIndex;
            int fetchedCellcolumn = startingCell.columnIndex;
            for (int dist = gapLength - 1; dist >= 0; --dist)
            {
                if (direction == Direction.LEFT)
                {
                    fetchedCellcolumn = startingCell.columnIndex + dist;
                }
                else if (direction == Direction.UP)
                {
                    fetchedCellRow = startingCell.rowIndex + dist;
                }
                FetchSignsOfAlignment(fetchedCellRow, fetchedCellcolumn, direction);
            }
        }

        public override int GetOptimalAlignmentScore()
        {        
            ComputeAlignmentArrayIfNecessary();
            Cell bestScoreCell = alignmentArray2.GetCell(alignmentArray2.rowSize, alignmentArray2.columnSize);
            int optimalScore = bestScoreCell.value;
            return optimalScore;
        }

        public override int GetNumberOfOptimalSolutions()
        {
            ComputeAlignmentArrayIfNecessary();
            int optimalSolutionsNumber = 0;
            ArrayIterator iterator = alignmentArray2.GetIterator();
            iterator.SetToCell(new Cell(alignmentArray2.rowSize, alignmentArray2.columnSize));
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
            int i = cell.rowIndex;
            int j = cell.columnIndex;
            int costOfCurrentCell = cell.value;

            if (iterator.HasDiagonal())
            {
                int costDiag = ComputeCostOfMatchingSigns(cell.GetUpDiagonalNeighbor());
                if (costOfCurrentCell.Equals(costDiag))
                {
                    ArrayIterator diagIterator = alignmentArray2.GetIterator();
                    diagIterator.SetToCell(cell);
                    diagIterator.Diagonal();
                    optimalSolutionsNumber += CountNumberOfOptimalSolutions(diagIterator);
                }
            }

            ArrayIterator upIterator = alignmentArray2.GetIterator();
            ArrayIterator leftIterator = alignmentArray2.GetIterator();
            upIterator.SetToCell(cell);
            leftIterator.SetToCell(cell);
            int k = 1;
            while (true)
            {
  
                ComputeCostOfHavingNGaps(k, cell);
                if (upIterator.HasUp())
                {
                    upIterator.Up();
                    Cell currentCell = (Cell)upIterator.GetCurrentCell();

                    if (costOfCurrentCell.Equals(verticalGapsCost))
                    {      
                        optimalSolutionsNumber += CountNumberOfOptimalSolutions(upIterator);
                    }
                }

                if (leftIterator.HasLeft())
                {
                    leftIterator.Left();
                    Cell currentCell = (Cell)leftIterator.GetCurrentCell();

                    if (costOfCurrentCell.Equals(horizontalGapsCost))
                    {
                        optimalSolutionsNumber += CountNumberOfOptimalSolutions(leftIterator);
                    }
                }

                ++k;
                if (k > i && k > j) break;
            }
            return optimalSolutionsNumber;
        }

    }
}
