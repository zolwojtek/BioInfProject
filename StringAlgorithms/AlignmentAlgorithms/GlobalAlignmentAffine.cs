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

            List<int> dimensionSizes = parameters.GetDimensionSizes();
            helpArrayP.Initialize(dimensionSizes);
            helpArrayQ.Initialize(dimensionSizes);

            helpArrayP.array.FillRowWithIntValue(0, 0, (x) => int.MaxValue / 2); //nie powinniśmy z zewnątrz wykonywać operacji bezpośrednio na wewnętrznej skladowej innej klasy TODO
            helpArrayQ.array.FillColumnWithIntValue(0, 0, (x) => int.MaxValue / 2);
        }

        protected override int ComputeAligningValue(Cube cell, List<char> signsToALign)
        {
            int score = 0;
            if (signsToALign[0] == '-')
            {
                score = ComputeCostOfMatchingWithGap(cell, helpArrayQ);
                Cube cellQ = new Cube(cell.rowIndex, cell.columnIndex + 1, 0, score);
                helpArrayQ.SetCell(cellQ);
            }
            else if (signsToALign[1] == '-')
            {
                score = ComputeCostOfMatchingWithGap(cell, helpArrayP);
                Cube cellP = new Cube(cell.rowIndex + 1, cell.columnIndex, 0, score);
                helpArrayP.SetCell(cellP);
            }
            else
            {
                score = array.GetCellValue(cell.rowIndex, cell.columnIndex, cell.depthIndex);
                score += parameters.CostArray.GetLettersAlignmentCost(signsToALign[0], signsToALign[1]);
            }
            return score;
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

        

        protected override Cube GoOneStepBack(Cube from)
        {
            Cube newCell;
            int diagonalCost = ComputeCostOfMatchingSigns(from.GetUpDiagonalNeighbor());
            int costOfCurrentCell = from.value;

            if (costOfCurrentCell.Equals(diagonalCost))
            {
                newCell = array.GetCell(from.rowIndex - 1, from.columnIndex - 1, from.depthIndex);
                FetchSignsOfAlignment(newCell.rowIndex, newCell.columnIndex, Direction.DIAGONAL);
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
                        FetchSeqeunceOfGapsInAlignment(gapLength, newCell, Direction.UP);
                        break;
                    }
                    else if (costOfCurrentCell.Equals(horizontalGapsCost))
                    {
                        newCell = array.GetCell(from.rowIndex, from.columnIndex - gapLength,0);
                        FetchSeqeunceOfGapsInAlignment(gapLength, newCell, Direction.LEFT);
                        break;
                    }
                    ++gapLength;
                }
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
            alignmentStringsBuilders[0].Insert(0, signFirst);
            alignmentStringsBuilders[1].Insert(0, signSecond);
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

        private void AddNextSignsOfAlignment(char a, char b, char c)
        {
            throw new NotImplementedException();
        }

    }
}
