using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms
{
    public class Algorithms
    {
        private bool DEBUG = true;
        private int MAX_SEQUENCES_FOR_EXACT_AlGORITHM = 5;
        private int MAX_SEQUENCE_LENGTH_FOR_EXACT_AlGORITHM = 500;

        public void SetLimitsForExactMultipleAlignment(int maxSeq, int maxSeqLegth)
        {
            if (DEBUG)
            {
                MAX_SEQUENCES_FOR_EXACT_AlGORITHM = maxSeq;
                MAX_SEQUENCE_LENGTH_FOR_EXACT_AlGORITHM = maxSeqLegth;
            }
            else
                throw new Exception("Need to be in debug mode!");
        }

        //private GlobalAlignment globalAlignment;
        //private GlobalAlignmentAffineGapCost globalAlignmentAffine;
        private TextAlignmentAlgorithm algorithm;


        public void SetParameters(List<Sequence> sequences, LetterAlignmentCostManager costArray, LetterAlignmentCostManager.CostFun costFun)
        {

            TextAlignmentParameters parameters = new TextAlignmentParameters();
            parameters.CostArray = costArray;
            //a może już powinno być podane w costArray?
            parameters.CostArray.GapCostFun = costFun;
            parameters.Sequences = sequences;
            parameters.Comparefunction = GetStrategyFunction(costArray);
            //Zdecyduj instacje której klasy stworzyć (algorithm)
            algorithm = ChooseAlgorithmType(parameters);
            //this.globalAlignment.Parameters = parameters;
            //this.globalAlignmentAffine.Parameters = parameters;
        }

        private TextAlignmentAlgorithm ChooseAlgorithmType(TextAlignmentParameters parameters)
        {
            FunctionTypeEnum functionType = parameters.CostArray.GetCostFunctionType();

            if (parameters.Sequences.Count() == 2)
            {
                switch (functionType)
                {
                    case StringAlgorithms.FunctionTypeEnum.LINEAR:
                        //optimalAlignmentScore = globalAlignment.GetOptimalAlignmentScore();r
                        return new GlobalAlignment(parameters);
                    case StringAlgorithms.FunctionTypeEnum.AFFINE:
                        //optimalAlignmentScore = globalAlignmentAffine.GetOptimalAlignmentScore();
                        return new GlobalAlignmentAffine(parameters);
                    default:
                        throw new Exception("The cost function is not constant or linear!");
                        //ZMIENIC W ENUM NA CONSTANT I LINEAR !!
                }
            }
            else
            {
                if(functionType != StringAlgorithms.FunctionTypeEnum.LINEAR)
                    throw new Exception("For multiple alignment the cost function is not constant!");
                int maxSequenceLength = 0;
                for(int i = 0; i < parameters.Sequences.Count(); ++i)
                {
                    if(parameters.Sequences[i].Value.Length > maxSequenceLength)
                    {
                        maxSequenceLength = parameters.Sequences[i].Value.Length;
                    }
                }
                if (parameters.Sequences.Count() < MAX_SEQUENCES_FOR_EXACT_AlGORITHM && maxSequenceLength < MAX_SEQUENCE_LENGTH_FOR_EXACT_AlGORITHM)
                    return new MultipleAlignmentExact(parameters);
                else return new MultipleAlignmentTemp(parameters);

            }
        }

        public Algorithms()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            int[,] costMatrix = CreateIdentityMatrix(StringAlgorithms.Constants.ALPHABET.Length);
            parameters.CostArray = new LetterAlignmentCostManager(StringAlgorithms.Constants.ALPHABET, costMatrix);
            parameters.CostArray.GapCostFun = (x) => x;
            parameters.Comparefunction = (x, y) => (Math.Max(x, y));
            //this.globalAlignmentAffine = new GlobalAlignmentAffineGapCost(parameters);
            //this.globalAlignment = new GlobalAlignment(parameters); 
            algorithm = ChooseAlgorithmType(parameters);
        }

        //Za każdym azem bedzie sprawdzac typ funkcji/ moze lepiej by było przy zmianie parametrów sprawidzić raz i zapisac?
        public int GetOptimalAlignmentScore()
        {
            //int optimalAlignmentScore = -1;
            //StringAlgorithms.FunctionTypeEnum functionType = GetCostFunctionType(globalAlignment.Parameters.CostArray.GapCostFun);

            //switch(functionType)
            //{
            //    case StringAlgorithms.FunctionTypeEnum.LINEAR:
            //        optimalAlignmentScore = globalAlignment.GetOptimalAlignmentScore();
            //        break;
            //    case StringAlgorithms.FunctionTypeEnum.AFFINE:
            //        optimalAlignmentScore = globalAlignmentAffine.GetOptimalAlignmentScore();
            //        break;
            //    default:
            //        throw new Exception("The cost function is not constant or linear!");
            //        //ZMIENIC W ENUM NA CONSTANT I LINEAR !!
            //}
            //return optimalAlignmentScore;
            return algorithm.GetOptimalAlignmentScore();
        }

        public Alignment GetOptimalAlignment()
        {
            //Alignment optimalAlignment;
            //StringAlgorithms.FunctionTypeEnum functionType = GetCostFunctionType(globalAlignment.Parameters.CostArray.GapCostFun);

            //switch (functionType)
            //{
            //    case StringAlgorithms.FunctionTypeEnum.LINEAR:
            //        optimalAlignment = globalAlignment.GetOptimalAlignment();
            //        break;
            //    case StringAlgorithms.FunctionTypeEnum.AFFINE:
            //        optimalAlignment = globalAlignmentAffine.GetOptimalAlignment();
            //        break;
            //    default:
            //        throw new Exception("The cost function is not constant or linear!");
            //        //ZMIENIC W ENUM NA CONSTANT I LINEAR !!
            //}
            //return optimalAlignment;
            return algorithm.GetOptimalAlignment();
        }

        public int GetNumberOfOptimalAlignments()
        {
            return algorithm.GetNumberOfOptimalSolutions();
        }

        public string GetLongestCommonSubsequence(Sequence seq1, Sequence seq2)
        {
            int[,] c = CreateIdentityMatrix(4);
            LetterAlignmentCostManager costArray = new LetterAlignmentCostManager(Constants.DNA, c);
            this.SetParameters(new List<Sequence>() { seq1, seq2 }, costArray, (x) => (0));
            Alignment alignment = algorithm.GetOptimalAlignment();
            return ExtractLongestCommonSubsequence(alignment.Sequences[0].Value, alignment.Sequences[1].Value);
        }

        public int GetEditDistance(Sequence seq1, Sequence seq2)
        {
            int[,] c = CreateIdentityMatrix(4, 1, 0);
            LetterAlignmentCostManager costArray = new LetterAlignmentCostManager(Constants.DNA, c);
            this.SetParameters(new List<Sequence>() { seq1, seq2 },costArray,(x)=>(x));
            return algorithm.GetOptimalAlignmentScore();
        }

        private string ExtractLongestCommonSubsequence(string str1, string str2)
        {
            string lcs = string.Empty;
            for (int i = 0; i < str1.Length; ++i)
            {
                if (str1[i] == str2[i])
                {
                    lcs += str1[i];
                }
            }
            return lcs;
        }

        private int[,] CreateIdentityMatrix(int size,int neutral = 0, int diagonal = 1)
        {
            int[,] array = new int[size, size];
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    if (i == j)
                    {
                        array[i, j] = diagonal;
                    }
                    else array[i, j] = neutral;
                }
            }
            return array;
        }

        //private FunctionTypeEnum GetCostFunctionType(StringCostArray.CostFun costFun)
        //{
        //    int value0 = costFun(0);
        //    int value1 = costFun(1);
        //    int value2 = costFun(2);
        //    int value3 = costFun(3);
        //    if(value2-value1 != value3-value2)
        //    {
        //        return StringAlgorithms.FunctionTypeEnum.OTHER;
        //    }
        //    else
        //    {
        //        if(value0 == 0)
        //        {
        //            return StringAlgorithms.FunctionTypeEnum.LINEAR;
        //        }
        //        else
        //        {
        //            return StringAlgorithms.FunctionTypeEnum.AFFINE;
        //        }
        //    }
                
        //}

        private TextAlignmentParameters.StrategyFun GetStrategyFunction(LetterAlignmentCostManager array)
        {
            string signSet = array.allowedSigns;
            int aa = array.GetLettersAlignmentCost(signSet[0], signSet[0]);
            int ab = array.GetLettersAlignmentCost(signSet[0], signSet[1]);
            if (aa < ab)
            {
                return (x, y) => (Math.Min(x, y));
            }
            return (x, y) => (Math.Max(x, y));
        }
    }
}
