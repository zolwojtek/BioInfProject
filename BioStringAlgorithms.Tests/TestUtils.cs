using StringAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioStringAlgorithms.Tests
{
    class TestUtils
    {
        public int CountAlignmentScore(Alignment alignment, TextAlignmentParameters parameters)
        {
            FunctionTypeEnum functionType = parameters.CostArray.GetCostFunctionType();
            bool gapStartedA = false;
            bool gapStartedB = false;
            int score = 0;
            int n = alignment.Sequences.Count();
            int m = alignment.Sequences.First().Value.Length;
            for (int i = 0; i < n; ++i)
            {
                for (int j = i+1; j < n; ++j)
                {
                    for (int k = 0; k < m; ++k)
                    {
                        char a = alignment.Sequences[i].Value[k];
                        char b = alignment.Sequences[j].Value[k];
                        if (functionType == FunctionTypeEnum.LINEAR)
                        {
                            score += parameters.CostArray.GetLettersAlignmentCost(a, b);
                        }
                        else if(functionType == FunctionTypeEnum.AFFINE)
                        {
                            if ((a == '-' || b == '-') && !(a == '-' && b == '-'))
                            {
                                if(a == '-' && !gapStartedA)
                                {
                                    score += parameters.CostArray.GetGapStartingCost();
                                    gapStartedA = true;
                                    gapStartedB = false;
                                }
                                else if (b == '-' && !gapStartedB)
                                {
                                    score += parameters.CostArray.GetGapStartingCost();
                                    gapStartedB = true;
                                    gapStartedA = false;
                                }
                                else
                                {
                                    score += parameters.CostArray.GetGapExtensionCost();
                                }
                            }
                            else
                            {
                                score += parameters.CostArray.GetLettersAlignmentCost(a, b);
                                if(!(a == '-' && b == '-'))
                                {
                                    gapStartedA = false;
                                    gapStartedB = false;
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("The cost function is not constant or linear!");
                        }
                    }
                }
            }
            return score;
        }

        public TextAlignmentAlgorithm InitializeTextAlignmentAlgorithm(TextAlignmentParameters parameters,TextAlignmentAlgorithm algorithm, List<Sequence> sequences, TextAlignmentParameters.StrategyFun compareFunction, LetterAlignmentCostManager.CostFun costFun, int[,] costs = null)
        {
            //Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            //Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            parameters.Sequences = sequences;
            //int[,] costs;
            if (costs == null)
            {
                int semiCost = 0;
                if (compareFunction(1, 2) == 2)
                {
                    costs = new int[,] { { semiCost, 2, 5, 2 }, { 2, semiCost, 2, 5 }, { 5, 2, semiCost, 2 }, { 2, 5, 2, semiCost } };
                }
                else
                {
                    costs = new int[,] { { semiCost, 5, 2, 5 }, { 5, semiCost, 5, 2 }, { 2, 5, semiCost, 5 }, { 5, 2, 5, semiCost } };

                }
            }
            LetterAlignmentCostManager costArray = new LetterAlignmentCostManager(Constants.DNA, costs);
            parameters.CostArray = costArray;
            parameters.CostArray.GapCostFun = costFun;//x => x * -5;
            parameters.Comparefunction = compareFunction;

            algorithm.Parameters = parameters;
            return algorithm;
        }

    }
}
