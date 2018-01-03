using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StringAlgorithms;

namespace SequenceParser
{
    public class SequenceParser
    {
        protected char[] chars = new char[] { 'A', 'a', 'B', 'b', 'C', 'c', 'D', 'd', 'E', 'e', 'F', 'f', 'G', 'g', 'H', 'h', 'I', 'i', 'J', 'j', 'K', 'k', 'L', 'l', 'M', 'm', 'N', 'n', 'O', 'o', 'P', 'p', 'Q', 'q', 'R', 'r', 'S', 's', 'T', 't', 'U', 'u', 'V', 'v', 'W', 'w', 'X', 'x', 'Y', 'y', 'Z', 'z', '-' };

        //private List<StringPair> sequenceStorage;

        public List<Sequence> ReadFromFile(string filePath)
        {
            List<Sequence> sequenceStorage = new List<Sequence>();

            var reader = new StreamReader(filePath);
            string sequenceName = string.Empty;
            //bool isSeq = false;
            string seq = string.Empty;
            while (true)
            {


                var line = reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    StoreSequence(sequenceStorage, sequenceName, seq);
                    break;
                }
                if (line.StartsWith(">"))
                {
                    if(!String.Equals(seq,string.Empty))
                    {
                        StoreSequence(sequenceStorage, sequenceName, seq);
                        seq = string.Empty;
                    }
                    sequenceName = line;
                    //isSeq = true;
                }
                else
                {       
                    line = line.ToUpper();
                    seq += line;
                    //Na czas obliczania kosztu dopasowania
                    //if(!ValidateString(line))
                    //throw new ArgumentException("Incorrect letter was given. It should be one from that set {'a/A','c/C','g/G','t/T'}");
                    //StoreSequence(sequenceStorage, sequenceName, line);
                }
            }
            reader.Dispose();
            return sequenceStorage;
        }

        public List<Sequence> ReadFromFile(byte[] file)
        {
            List<Sequence> sequenceStorage = new List<Sequence>();
            var str = System.Text.Encoding.Default.GetString(file);      
                
            var reader = new StringReader(str); 
            string sequenceName = string.Empty;

            string seq = string.Empty;
            while (true)
            {


                var line = reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    StoreSequence(sequenceStorage, sequenceName, seq);
                    break;
                }
                if (line.StartsWith(">"))
                {
                    if (!String.Equals(seq, string.Empty))
                    {
                        StoreSequence(sequenceStorage, sequenceName, seq);
                        seq = string.Empty;
                    }
                    sequenceName = line;
                    //isSeq = true;
                }
                else
                {
                    line = line.ToUpper();
                    seq += line;
                    //Na czas obliczania kosztu dopasowania
                    //if(!ValidateString(line))
                    //throw new ArgumentException("Incorrect letter was given. It should be one from that set {'a/A','c/C','g/G','t/T'}");
                    //StoreSequence(sequenceStorage, sequenceName, line);
                }
            }
            reader.Dispose();
            return sequenceStorage;
        }

        public Sequence ReadFromConsole()
        {
            string sequenceName = string.Empty;
            sequenceName = Console.ReadLine();
            string sequence = string.Empty;
            sequence = Console.ReadLine();
            sequence = sequence.ToUpper();
            if (!ValidateString(sequence))
                throw new ArgumentException("Incorrect letter was given. It should be one from that set {'a/A','c/C','g/G','t/T'}");
            return new Sequence(StringAlgorithms.Constants.DNA, sequenceName, sequence);
        }

        //WriteToConsole
        //WriteToFile
    
        protected void StoreSequence(List<Sequence> sequenceStorage, string name, string line)
        {
            sequenceStorage.Add(new Sequence(StringAlgorithms.Constants.DNA, name, line));
        }

        //Check if extension methon not needed
        protected bool ValidateString(string a)
        {
            for (int i = 0; i < a.Length; ++i)
            {
                if (!chars.Contains(a[i]))
                {
                    return false;
                }

            }
            return true;
        }


    }
}
