using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms
{
    [Serializable]
    public class LetterAlignmentCostManager
    {
        public readonly string allowedSigns;
        private Dictionary<string, int> alignmentCosts;
        public delegate int CostFun(int k);
        public CostFun GapCostFun { get; set; }//TODO: zrobić prywatne i dac do konstruktora

        public LetterAlignmentCostManager(string signs, int[,] costArray)
        {
            ValidateInput(signs, costArray);
            this.allowedSigns = signs;
            ParseLetterAlignmentCosts(costArray);
            //AddSingleLetterGapAlignmentCost();
        }

        private void ValidateInput(string signs, int[,] costArray)
        {
            if (signs.IsAnySignInStringMultipleTimes())
            {
                throw new Exception("In given string some signs are repeated");
            }
            if (!IsMatrixSizeAppropriate(signs, costArray))
            {
                throw new Exception("The size of given matrix is inappropriate in accordance to the number of letters in given string.");
            }
        }

        private bool IsMatrixSizeAppropriate(string signs, int[,] costArray)
        {
            int allowedSignsNumber = signs.Length;
            int rowsInCostArrayNumber = costArray.GetLength(0);
            int columnsInCostArrayNumber = costArray.GetLength(1);
            if (allowedSignsNumber != rowsInCostArrayNumber || rowsInCostArrayNumber != columnsInCostArrayNumber)
            {
                return false;
            }
            return true;
        }

        private void ParseLetterAlignmentCosts(int[,] costArray)
        {
            alignmentCosts = new Dictionary<string, int>();
            for (int i = 0; i < allowedSigns.Length; ++i)
            {
                for (int j = 0; j < allowedSigns.Length; ++j)
                {
                    AddLettersAlignmentCost(allowedSigns[i], allowedSigns[j], costArray[i, j]);
                }
            }
        }

        private void AddLettersAlignmentCost(char firstLetter, char secondLetter, int cost)
        {
            string firstKey = $"{firstLetter}{secondLetter}";
            alignmentCosts.Add(firstKey, cost);
        }

        private void AddSingleLetterGapAlignmentCost()
        {
            try
            {
                AddLettersAlignmentCost(Constants.GAP, Constants.GAP, 0);
                for (int i = 0; i < allowedSigns.Length; ++i)
                {
                    AddLettersAlignmentCost(allowedSigns[i], Constants.GAP, GapCostFun(1));
                    AddLettersAlignmentCost(Constants.GAP, allowedSigns[i], GapCostFun(1));
                }
            }
            catch(Exception e)
            {

            }
        }

        //TODO: KONIECZNIE DO POPRAWY PO WYCZYSZCZENIU INNYCH KLAS I TESTOW
        public int GetLettersAlignmentCost(char firstSign, char secondSign)
        {
            AlignmentType alignmentType;
            if (firstSign == Constants.GAP && secondSign == Constants.GAP)
            {
                alignmentType = AlignmentType.NONE;
                return 0;
            }
            else if (firstSign == Constants.NEUTRAL_SIGN || secondSign == Constants.NEUTRAL_SIGN)
            {
                return 0;
            }
            else if (firstSign == Constants.GAP || secondSign == Constants.GAP)
            {
                alignmentType = AlignmentType.MATCH_WITH_GAP;
                return GapCostFun(1);
            } 
            else
            {
                alignmentType = AlignmentType.MATCH_SIGNS;
            }

            if (!IsLetterAllowed(firstSign) || !IsLetterAllowed(secondSign))
            {
                throw new Exception($"Incorrect letters were given. They should be contained in the following set: '{this.allowedSigns}'");
            }

            int value = 0;
            switch (alignmentType)
            {
                case AlignmentType.NONE:
                    {
                        value = 0;
                        break;
                    }
                case AlignmentType.MATCH_WITH_GAP:
                    {
                        value = GapCostFun(1);
                        break;
                    }
                case AlignmentType.MATCH_SIGNS:
                    {
                        string key = $"{firstSign}{secondSign}";
                        alignmentCosts.TryGetValue(key, out value);
                        break;
                    }
            }
            return value;
        }

        private bool IsLetterAllowed(char letter)
        {
            if (this.allowedSigns.Contains(letter))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetGapExtensionCost()
        {
            return this.GapCostFun(1) - this.GapCostFun(0);
        }

        public int GetGapStartingCost()
        {
            return this.GapCostFun(1);
        }


        //TODO To nie powinno być w tej klasie
        public FunctionTypeEnum GetCostFunctionType()
        {
            int value0 = GapCostFun(0);
            int value1 = GapCostFun(1);
            int value2 = GapCostFun(2);
            int value3 = GapCostFun(3);
            if (value2 - value1 != value3 - value2)
            {
                return FunctionTypeEnum.OTHER;
            }
            else
            {
                if (value0 == 0)
                {
                    return FunctionTypeEnum.LINEAR;
                }
                else
                {
                    return FunctionTypeEnum.AFFINE;
                }
            }

        }




    }


    //private int GetNumericalValueOdChar(char a)
    //{
    //    return System.Convert.ToInt32(a);
    //}

}
