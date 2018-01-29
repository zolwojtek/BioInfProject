using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms
{
    [Serializable]
    public class Sequence
    {
        
        public string Name
        {
            get { return name; }
            set
            {
                if (!value.StartsWith(">"))
                {
                    value = ">" + value;
                }

                name = value;

            }
        }       
        public string Value
        {
            get { return value; }
            set
            {
                if (ValidateString(value))
                {
                    this.value = value;
                }
                else
                {
                    throw new ArgumentException("String contains forbidden signs.");
                }
            }
        }
        public readonly string signSet;

        private string name;
        private string value;

        public Sequence(string signs)
        {
            ValidateInput(signs);
            this.signSet = signs;
            //chars = new char[] { '>','_','1','2','3','4','5','6','7','8','9','0','A','a','B','b','C','c','D','d','E','e','F','f','G','g','H','h','I','i','J','j','K','k','L','l','M','m','N','n','O','o','P','p','Q','q','R','r','S','s','T','t','U','u','V','v','W','w','X','x','Y','y','Z','z','-'};
            this.Name = string.Empty;
            this.Value = string.Empty; 
        }

        public Sequence(string signs, string first, string second): this(signs)
        {
            //ValidateInput(signs);
            //this.signSet = signs;
            //chars = new char[] { '>', '_', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'A', 'a', 'B', 'b', 'C', 'c', 'D', 'd', 'E', 'e', 'F', 'f', 'G', 'g', 'H', 'h', 'I', 'i', 'J', 'j', 'K', 'k', 'L', 'l', 'M', 'm', 'N', 'n', 'O', 'o', 'P', 'p', 'Q', 'q', 'R', 'r', 'S', 's', 'T', 't', 'U', 'u', 'V', 'v', 'W', 'w', 'X', 'x', 'Y', 'y', 'Z', 'z', '-' };
            this.Name = first;
            this.Value = second;
        }


        public int GetNumberOfAllowedSigns()
        {
            return signSet.Length;
        }

        private void ValidateInput(string signs)
        {
            LookForRepeatedLettersInString(signs);
        }

        private void LookForRepeatedLettersInString(string signs)
        {
            int[] numbersOfLatinLetters = new int[256];
            for (int i = 0; i < signs.Length; ++i)
            {
                int letterIdx = GetNumericalValueOdChar(signs[i]);// - (int)'A';
                if (numbersOfLatinLetters[letterIdx] > 0)
                {
                    throw new Exception("In given string some letters are repeated");
                }
                else
                {
                    ++numbersOfLatinLetters[letterIdx];
                }
            }
        }

        private int GetNumericalValueOdChar(char a)
        {
            return System.Convert.ToInt32(a);
        }

        protected bool ValidateString(string a)
        {
            for (int i = 0; i < a.Length; ++i)
            {
                if (!signSet.Contains(a[i]))
                {
                    return false;
                }

            }
            return true;
        }

        
    }
}
