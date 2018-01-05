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
        private int[,] P;
        private int[,] Q;
        private AlignmentArray alignmentArray2;
        private AlignmentArray helpArrayP;
        private AlignmentArray helpArrayQ;

        public GlobalAlignmentAffine(TextAlignmentParameters parameters) : base(parameters)
        {

        }

        public override Alignment GetOptimalAlignment()
        {
            ComputeSolutionIfNecessary();
            return GetAligment();
        }

        private void ComputeSolutionIfNecessary()
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

            this.alignmentArray = new int[alignmentArrayRowNumber + 1, alignmentArrayColumnNumber + 1];
            this.P = new int[alignmentArrayRowNumber + 1, alignmentArrayColumnNumber + 1];
            this.Q = new int[alignmentArrayRowNumber + 1, alignmentArrayColumnNumber + 1];

            alignmentArray2 = new AlignmentArray();
            helpArrayP = new AlignmentArray();
            helpArrayQ = new AlignmentArray();

            alignmentArray2.Initialize(alignmentArrayRowNumber, alignmentArrayColumnNumber, (x) => (parameters.CostArray.GapCostFun(x)));
            helpArrayP.Initialize(alignmentArrayRowNumber, alignmentArrayColumnNumber);
            helpArrayQ.Initialize(alignmentArrayRowNumber, alignmentArrayColumnNumber);

            helpArrayP.FillRowWithValue(0, (x) => (int.MaxValue / 2));
            helpArrayQ.FillColumnWithValue(0, (x) => (int.MaxValue / 2));

            alignmentArray.FillColumnWithIntValue(0, (x) => (parameters.CostArray.GapCostFun(x)));
            alignmentArray.FillRowWithIntValue(0, (x) => (parameters.CostArray.GapCostFun(x)));
            P.FillRowWithIntValue(0, (x) => (int.MaxValue / 2));
            Q.FillColumnWithIntValue(0, (x) => (int.MaxValue / 2));
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

        int fromDiagonalNeighborCost;
        int fromUpNeighborCost;
        int fromLeftNeighborCost;

        private void ComputeCell(Cell cell)
        {
            int i = cell.rowIndex;
            int j = cell.columnIndex;


            ComputeGoingFromNeighborsCosts(cell);
            cell.value = GetTheBestComingFromNeigborCost();

            alignmentArray2.SetCell(cell);

            Cell cellP = new Cell(i, j, fromUpNeighborCost);
            Cell cellQ = new Cell(i, j, fromLeftNeighborCost);
            helpArrayP.SetCell(cellP);
            helpArrayQ.SetCell(cellQ);

            alignmentArray[i, j] = cell.value;
            P[i, j] = fromUpNeighborCost;
            Q[i, j] = fromLeftNeighborCost;      
        }

        private void ComputeGoingFromNeighborsCosts(Cell currentCell)
        {
            fromUpNeighborCost = ComputeCostOfMatching(currentCell.GetUpNeighbor(), helpArrayP, AlignmentType.MATCH_WITH_GAP);
            fromLeftNeighborCost = ComputeCostOfMatching(currentCell.GetLeftNeighbor(), helpArrayQ, AlignmentType.MATCH_WITH_GAP);
            fromDiagonalNeighborCost = ComputeCostOfMatching(currentCell.GetUpDiagonalNeighbor(), null, AlignmentType.MATCH_SIGNS);
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
            int i = cell.rowIndex;
            int j = cell.columnIndex;
            int gapStartingCost = alignmentArray2.GetCell(i,j).value + parameters.CostArray.GetGapStartingCost();
            int gapExtensionCost = helpArray.GetCell(i,j).value + parameters.CostArray.GetGapExtensionCost();
            return parameters.Comparefunction(gapStartingCost, gapExtensionCost);
        }

        private int ComputeCostOfMatchingSigns(Cell cell)
        {
            int i = cell.rowIndex;
            int j = cell.columnIndex;

            int cost = alignmentArray[i, j] + parameters.CostArray.GetLettersAlignmentCost(parameters.Sequences[0].Value[i], parameters.Sequences[1].Value[j]);
            return cost;
        }

        private int GetTheBestComingFromNeigborCost()
        {
            int bestCost = parameters.Comparefunction(fromDiagonalNeighborCost, parameters.Comparefunction(fromUpNeighborCost, fromLeftNeighborCost));
            return bestCost;
        }

        public override int GetOptimalAlignmentScore()
        {        
            ComputeSolutionIfNecessary();
            Cell bestScoreCell = alignmentArray2.GetCell(alignmentArray2.rowSize, alignmentArray2.columnSize);
            int optimalScore = bestScoreCell.value;
            return optimalScore;
        }

        private StringBuilder firstSeqenceOfAlignment = null;
        private StringBuilder secondSequenceOfAlignment = null;


        private Alignment GetAligment()
        {
            ArrayIterator iterator = alignmentArray2.GetIterator();
            iterator.SetToCell(new Cell(alignmentArray2.rowSize, alignmentArray2.columnSize));
            Cell currentCell = (Cell)iterator.GetCurrentCell();

     


            //int i = parameters.Sequences[0].Value.Length;
            //int j = parameters.Sequences[1].Value.Length;
            String answerSeq1 = "";
            String answerSeq2 = "";


            while (currentCell.IsTopLeftCell() == false)
            {
                currentCell = (Cell)iterator.GetCurrentCell();
                int i = currentCell.rowIndex;
                int j = currentCell.columnIndex;


                int upCost = ComputeCostOfMatchingSigns(currentCell.GetUpDiagonalNeighbor());
                if (i > 0 && j > 0 && alignmentArray[i, j] == upCost)
                {
                    answerSeq1 = parameters.Sequences[0].Value[i - 1] + answerSeq1;
                    answerSeq2 = parameters.Sequences[1].Value[j - 1] + answerSeq2;

                    currentCell = (Cell)iterator.Diagonal();
                }
                else
                {
                    int k = 1;
                    while (true)
                    {
                        if (i >= k && alignmentArray[i, j] == alignmentArray[i - k, j] + parameters.CostArray.GapCostFun(k))
                        {
                            for (int it = 0; it < k; ++it)
                            {
                                answerSeq1 = parameters.Sequences[0].Value[i - 1 - it] + answerSeq1;
                                answerSeq2 = @"-" + answerSeq2;
                            }
                            currentCell.rowIndex -= k;
                            iterator.SetToCell(currentCell);
                            //currentCell = 
                            //i -= k;
                            break;
                        }
                        else if (j >= k && alignmentArray[i, j] == alignmentArray[i, j - k] + parameters.CostArray.GapCostFun(k))
                        {
                            for (int it = 0; it < k; ++it)
                            {
                                answerSeq2 = parameters.Sequences[1].Value[j - 1 - it] + answerSeq2;
                                answerSeq1 = @"-" + answerSeq1;
                            }
                            currentCell.columnIndex -= k;
                            iterator.SetToCell(currentCell);
                            //j -= k;
                            break;
                        }
                        else
                        {
                            ++k;
                        }
                    }
                }
            }
            Sequence alignmentSeq1 = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[0].Name, answerSeq1);
            Sequence alignmentSeq2 = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[1].Name, answerSeq2);
            return new Alignment(alignmentSeq1, alignmentSeq2);
        }

        public override int GetNumberOfOptimalSolutions()
        {
            ComputeSolutionIfNecessary();
            int i = parameters.Sequences[0].Value.Length;
            int j = parameters.Sequences[1].Value.Length;
            int sum = 0;
            CountNumberOfOptimalSolutions(i, j, ref sum);
            return sum;
        }

        private void CountNumberOfOptimalSolutions(int i, int j, ref int sum)
        {

            if (i == 0 && j == 0)
            {
                ++sum;
                return;
            }
            if (i > 0 && j > 0 && alignmentArray[i, j] == alignmentArray[i - 1, j - 1] + parameters.CostArray.GetLettersAlignmentCost(parameters.Sequences[0].Value[i - 1], parameters.Sequences[1].Value[j - 1]))
            {
                CountNumberOfOptimalSolutions(i - 1, j - 1, ref sum);
            }

            int k = 1;
            while (true)
            {
                if (i >= k && alignmentArray[i, j] == alignmentArray[i - k, j] + parameters.CostArray.GapCostFun(k))
                {
                    CountNumberOfOptimalSolutions(i - k, j, ref sum);
                    //break;
                }
                if (j >= k && alignmentArray[i, j] == alignmentArray[i, j - k] + parameters.CostArray.GapCostFun(k))
                {
                    CountNumberOfOptimalSolutions(i,j - k, ref sum);
                    //break;
                }
                ++k;
                if (k > i && k > j) break;
            }
        }

    }
}
