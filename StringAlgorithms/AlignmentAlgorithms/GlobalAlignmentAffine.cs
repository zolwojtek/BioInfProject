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

        private AlignmentCube helpArrayP;
        private AlignmentCube helpArrayQ;

        public GlobalAlignmentAffine(TextAlignmentParameters parameters) : base(parameters)
        {

        }



        protected override void InitializeAlignmentArray()
        {
            base.InitializeAlignmentArray();

            int alignmentArrayRowNumber = parameters.Sequences[0].Value.Length;
            int alignmentArrayColumnNumber = parameters.Sequences[1].Value.Length;

            helpArrayP = new AlignmentCube();
            helpArrayQ = new AlignmentCube();

            helpArrayP.Initialize(alignmentArrayRowNumber, alignmentArrayColumnNumber,0);
            helpArrayQ.Initialize(alignmentArrayRowNumber, alignmentArrayColumnNumber,0);

            helpArrayP.array.FillRowWithIntValue(0, 0, (x) => int.MaxValue / 2);
            helpArrayQ.array.FillColumnWithIntValue(0, 0, (x) => int.MaxValue / 2);
        }





        private int ComputeCell2(Cube cell, List<int> directionVector)
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

                Cube newCell = new Cube(rowIndex - yOffset, columnIndex - xOffset, depthIndex - zOffset);
                int value = ComputeAligningValue(newCell, a, b, c);

                return value;
            }
            return GetIlligalValue();
        }

        protected override int ComputeAligningValue(Cube cell, char a, char b, char c)
        {
            int score = 0;
            if (a == '-')
            {
                score = ComputeCostOfMatchingWithGap(cell, helpArrayQ);
                Cube cellQ = new Cube(cell.rowIndex, cell.columnIndex + 1, 0, score);
                helpArrayQ.SetCell(cellQ);
            }
            else if (b == '-')
            {
                score = ComputeCostOfMatchingWithGap(cell, helpArrayP);
                Cube cellP = new Cube(cell.rowIndex + 1, cell.columnIndex, 0, score);
                helpArrayP.SetCell(cellP);
            }
            else
            {
                score = array.GetCellValue(cell.rowIndex, cell.columnIndex, cell.depthIndex);
                score += parameters.CostArray.GetLettersAlignmentCost(a, b);
            }
            return score;
        }



        private int GetIlligalValue()
        {
            int a = 1;
            int b = 0;
            int c = parameters.Comparefunction(a, b);
            if (c == 0)
            {
                return int.MaxValue / 2;
            }
            else
            {
                return int.MinValue / 2;
            }
        }



        private void ComputeCell(Cube cell)
        {
            int cellRow = cell.rowIndex;
            int celColumn = cell.columnIndex;

            ComputeGoingFromNeighborsCosts(cell);
            cell.value = GetTheBestComingFromNeigborCost();
            array.SetCell(cell);

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
            int gapStartingCost = array.GetCellValue(cellRow, cellColumn, cell.depthIndex) + parameters.CostArray.GetGapStartingCost();
            int gapExtensionCost = helpArray.GetCellValue(cellRow, cellColumn, cell.depthIndex) + parameters.CostArray.GetGapExtensionCost();
            return parameters.Comparefunction(gapStartingCost, gapExtensionCost);
        }

        private int ComputeCostOfMatchingSigns(Cube cell)
        {
            int cellRow = cell.rowIndex;
            int cellColumn = cell.columnIndex;
            int cost = array.GetCellValue(cellRow, cellColumn, cell.depthIndex) + parameters.CostArray.GetLettersAlignmentCost(parameters.Sequences[0].Value[cellRow], parameters.Sequences[1].Value[cellColumn]);
            return cost;
        }

        private int GetTheBestComingFromNeigborCost()
        {
            int bestCost = parameters.Comparefunction(fromDiagonalNeighborCost, parameters.Comparefunction(fromUpNeighborCost, fromLeftNeighborCost));
            return bestCost;
        }



        protected override void RetrieveAlignment()
        {
            CubeIterator iterator = array.GetIterator();
            iterator.SetToCell(new Cube(array.rowSize, array.columnSize,0));
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
                        newCell = array.GetCell(from.rowIndex - gapLength, from.columnIndex,0);
                        break;
                    }
                    else if (costOfCurrentCell.Equals(horizontalGapsCost))
                    {
                        newCell = array.GetCell(from.rowIndex, from.columnIndex - gapLength,0);
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
                horizontalGapsCost = array.GetCellValue(from.rowIndex, from.columnIndex - gapLength, from.depthIndex) + nGapCost;
            }
            else
            {
                horizontalGapsCost = int.MaxValue / 2;
            }
            if (from.rowIndex - gapLength >= 0)
            {
                verticalGapsCost = array.GetCellValue(from.rowIndex - gapLength, from.columnIndex, from.depthIndex) + nGapCost;
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
            Cube bestScoreCell = array.GetCell(array.rowSize, array.columnSize,0);
            int optimalScore = bestScoreCell.value;
            return optimalScore;
        }

        public override int GetNumberOfOptimalSolutions()
        {
            ComputeAlignmentArrayIfNecessary();
            int optimalSolutionsNumber = 0;
            CubeIterator iterator = array.GetIterator();
            iterator.SetToCell(new Cube(array.rowSize, array.columnSize,0));
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
                    CubeIterator diagIterator = array.GetIterator();
                    diagIterator.SetToCell(cell);
                    diagIterator.Diagonal();
                    optimalSolutionsNumber += CountNumberOfOptimalSolutions(diagIterator);
                }
            }

            CubeIterator upIterator = array.GetIterator();
            CubeIterator leftIterator = array.GetIterator();
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

        private Direction GetDirection(List<int> directionVector)
        {
            int y = directionVector[0];
            int x = directionVector[1];
            if(x == 1 && y == 1)
            {
                return Direction.DIAGONAL;
            }
            else if( x == 1 && y == 0)
            {
                return Direction.LEFT;
            }
            else
            {
                return Direction.UP;
            }
        }



        protected override void AddNextSignsOfAlignment(char a, char b, char c)
        {
            throw new NotImplementedException();
        }

        protected override void MakeAlignment()
        {
            throw new NotImplementedException();
        }
    }
}
