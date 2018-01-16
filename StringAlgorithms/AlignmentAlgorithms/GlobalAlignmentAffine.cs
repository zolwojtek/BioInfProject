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
        private AlignmentCube alignmentArray2;
        private AlignmentCube helpArrayP;
        private AlignmentCube helpArrayQ;
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

            alignmentArray2 = new AlignmentCube();
            helpArrayP = new AlignmentCube();
            helpArrayQ = new AlignmentCube();

            alignmentArray2.Initialize(alignmentArrayRowNumber, alignmentArrayColumnNumber,0, (x) => 0, (x) => 0);
            helpArrayP.Initialize(alignmentArrayRowNumber, alignmentArrayColumnNumber,0, (x) => 0,(x) => 0);
            helpArrayQ.Initialize(alignmentArrayRowNumber, alignmentArrayColumnNumber,0,(x) => 0, (x) => 0);
        }

        protected override void ComputeAlignmentArray()
        {
            CubeIterator alignmentArrayIterator = alignmentArray2.GetIterator();
            while (alignmentArrayIterator.HasNext())
            {
                Cube currentCell = (Cube)alignmentArrayIterator.Next();
                if (IsBorderCell(currentCell) == false)
                {
                    ComputeCell(currentCell);
                }
                else
                {
                    ComputeBorderCell(currentCell);
                }
            }
        }

        private void ComputeBorderCell(Cube cell)
        {
            int gapCost = 0;
            Cube cellP = new Cube();
            Cube cellQ = new Cube();

            if (cell.rowIndex == 0)
            {
                gapCost = parameters.CostArray.GapCostFun(cell.columnIndex);
                cellP = new Cube(cell.rowIndex, cell.columnIndex, 0, int.MaxValue / 2);
                cellQ = new Cube(cell.rowIndex, cell.columnIndex, 0, 0);
            }
            else if (cell.columnIndex == 0)
            {
                gapCost = parameters.CostArray.GapCostFun(cell.rowIndex);
                cellP = new Cube(cell.rowIndex, cell.columnIndex, 0, 0);
                cellQ = new Cube(cell.rowIndex, cell.columnIndex, 0, int.MaxValue / 2);
            }
            cell.value = gapCost;
            alignmentArray2.SetCell(cell);
            helpArrayP.SetCell(cellP);
            helpArrayQ.SetCell(cellQ);
        }

        private bool IsBorderCell(Cube cell)
        {
            if (cell.rowIndex == 0 || cell.columnIndex == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ComputeCell(Cube cell)
        {
            int cellRow = cell.rowIndex;
            int celColumn = cell.columnIndex;

            ComputeGoingFromNeighborsCosts(cell);
            cell.value = GetTheBestComingFromNeigborCost();
            alignmentArray2.SetCell(cell);

            Cube cellP = new Cube(cellRow, celColumn,0, fromUpNeighborCost);
            Cube cellQ = new Cube(cellRow, celColumn,0, fromLeftNeighborCost);
            helpArrayP.SetCell(cellP);
            helpArrayQ.SetCell(cellQ);
        }

        private void ComputeGoingFromNeighborsCosts(Cube destinationCell)
        {
            fromUpNeighborCost = ComputeCostOfMatching(destinationCell.GetUpNeighbor(), helpArrayP, AlignmentType.MATCH_WITH_GAP);
            fromLeftNeighborCost = ComputeCostOfMatching(destinationCell.GetLeftNeighbor(), helpArrayQ, AlignmentType.MATCH_WITH_GAP);
            fromDiagonalNeighborCost = ComputeCostOfMatching(destinationCell.GetUpDiagonalNeighbor(), null, AlignmentType.MATCH_SIGNS);
        }

        private int ComputeCostOfMatching(Cube cell, AlignmentCube helpArray, AlignmentType alignmentType)
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

        private int ComputeCostOfMatchingWithGap(Cube cell, AlignmentCube helpArray)
        {
            int cellRow = cell.rowIndex;
            int cellColumn = cell.columnIndex;
            int gapStartingCost = alignmentArray2.GetCellValue(cellRow, cellColumn) + parameters.CostArray.GetGapStartingCost();
            int gapExtensionCost = helpArray.GetCellValue(cellRow, cellColumn) + parameters.CostArray.GetGapExtensionCost();
            return parameters.Comparefunction(gapStartingCost, gapExtensionCost);
        }

        private int ComputeCostOfMatchingSigns(Cube cell)
        {
            int cellRow = cell.rowIndex;
            int cellColumn = cell.columnIndex;
            int cost = alignmentArray2.GetCellValue(cellRow, cellColumn) + parameters.CostArray.GetLettersAlignmentCost(parameters.Sequences[0].Value[cellRow], parameters.Sequences[1].Value[cellColumn]);
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
            CubeIterator iterator = alignmentArray2.GetIterator();
            iterator.SetToCell(new Cube(alignmentArray2.rowSize, alignmentArray2.columnSize,0));
            Cube currentCell = (Cube)iterator.GetCurrentCell();

            while (currentCell.IsTopLeftCell() == false)
            {
                Cube newCell = GoOneStepBack(currentCell, iterator);
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

        private Cube GoOneStepBack(Cube from, CubeIterator iterator)
        {
            Cube newCell;
            int diagonalCost = ComputeCostOfMatchingSigns(from.GetUpDiagonalNeighbor());
            int costOfCurrentCell = from.value;

            if (costOfCurrentCell.Equals(diagonalCost))
            {
                newCell = (Cube)iterator.Diagonal();
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

        private void ComputeCostOfHavingNGaps(int gapLength, Cube from)
        {
            int nGapCost = parameters.CostArray.GapCostFun(gapLength);
            if (from.columnIndex - gapLength >= 0)
            {
                horizontalGapsCost = alignmentArray2.GetCellValue(from.rowIndex, from.columnIndex - gapLength) + nGapCost;
            }
            else
            {
                horizontalGapsCost = int.MaxValue / 2;
            }
            if (from.rowIndex - gapLength >= 0)
            {
                verticalGapsCost = alignmentArray2.GetCellValue(from.rowIndex - gapLength, from.columnIndex) + nGapCost;
            }
            else
            {
                verticalGapsCost = int.MaxValue / 2;
            }
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

        private void FetchSeqeunceOfGapsInAlignment(int gapLength, Cube startingCell, Direction direction)
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
            Cube bestScoreCell = alignmentArray2.GetCell(alignmentArray2.rowSize, alignmentArray2.columnSize);
            int optimalScore = bestScoreCell.value;
            return optimalScore;
        }

        public override int GetNumberOfOptimalSolutions()
        {
            ComputeAlignmentArrayIfNecessary();
            int optimalSolutionsNumber = 0;
            CubeIterator iterator = alignmentArray2.GetIterator();
            iterator.SetToCell(new Cube(alignmentArray2.rowSize, alignmentArray2.columnSize,0));
            optimalSolutionsNumber = CountNumberOfOptimalSolutions(iterator);
            return optimalSolutionsNumber;
        }

        private int CountNumberOfOptimalSolutions(CubeIterator iterator)
        {
            int optimalSolutionsNumber = 0;
            Cube cell = (Cube)iterator.GetCurrentCell();
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
                    CubeIterator diagIterator = alignmentArray2.GetIterator();
                    diagIterator.SetToCell(cell);
                    diagIterator.Diagonal();
                    optimalSolutionsNumber += CountNumberOfOptimalSolutions(diagIterator);
                }
            }

            CubeIterator upIterator = alignmentArray2.GetIterator();
            CubeIterator leftIterator = alignmentArray2.GetIterator();
            upIterator.SetToCell(cell);
            leftIterator.SetToCell(cell);
            int k = 1;
            while (true)
            {
  
                ComputeCostOfHavingNGaps(k, cell);
                if (upIterator.HasUp())
                {
                    upIterator.Up();
                    Cube currentCell = (Cube)upIterator.GetCurrentCell();

                    if (costOfCurrentCell.Equals(verticalGapsCost))
                    {      
                        optimalSolutionsNumber += CountNumberOfOptimalSolutions(upIterator);
                    }
                }

                if (leftIterator.HasLeft())
                {
                    leftIterator.Left();
                    Cube currentCell = (Cube)leftIterator.GetCurrentCell();

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
