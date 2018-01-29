using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms
{
    public static class StringExtensions
    {
        public static bool IsAnySignInStringMultipleTimes(this string seq)
        {
            int[] numbersOfLatinLetters = new int[26];
            foreach(char sign in seq)
            {
                int letterIdx = (int)sign - (int)'A';
                if (numbersOfLatinLetters[letterIdx] > 0)
                {
                    return true;
                }
                else
                {
                    ++numbersOfLatinLetters[letterIdx];
                }
            }
            return false;
        }

        public static void ExtendSequenceWithDummies(this string sequence, char dummy, int times)
        {
            StringBuilder stringBuilder = new StringBuilder(sequence);
            stringBuilder.Append(dummy, times);
            sequence = stringBuilder.ToString();
        }

        public static void ExtendSequenceLengthAsMultiplicityOfNumber(this string sequence, int multiplicity)
        {
            int extensionSignsNumber = sequence.Length % multiplicity;
            sequence.ExtendSequenceWithDummies('-', extensionSignsNumber);
        }
    }
}
