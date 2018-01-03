using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringAlgorithms;
using System.IO;

namespace Testing
{
    class Program
    {


        static void Main(string[] args)
        {/*
            string seq1, seq2;
            Console.WriteLine("Enter seq1");
            seq1 = Console.ReadLine();
            Console.WriteLine("Enter seq2");
            seq2 = Console.ReadLine();

            seq1.ToUpper();
            seq2.ToUpper();
            */
            //string seq1 = @"AATAAT";
            //string seq2 = @"AAGG";
            //seq1 = System.IO.File.ReadAllText(@"C:\Users\Me\Documents\Visual Studio 2015\Projects\Project1Bio\Testing\Seq1.txt");
            //seq1.Replace(@" ", String.Empty);
            //seq2 = System.IO.File.ReadAllText(@"C:\Users\Me\Documents\Visual Studio 2015\Projects\Project1Bio\Testing\Seq2.txt");
            /*
            DNA dna = new DNA(seq1, seq2);
            int[,] costs = new int[,] { { 10, 2, 5, 2 }, { 2, 10, 2, 5 }, { 5, 2, 10, 2 }, { 2, 5, 2, 10 } };
            DNACostArray costArray = new DNACostArray(costs);
            costArray.getCost('c', 't');
            LongestCommonSequenceAlgorithm lcs = new LongestCommonSequenceAlgorithm();

            LongestCommonSubsequenceParameters lcsObject = new LongestCommonSubsequenceParameters();
            lcsObject.Dna = dna;
            lcsObject.CostArray = costArray;
            lcsObject.GapCost = -5;

            lcs.LscParam = lcsObject;
            lcs.Compute();
            

            Console.WriteLine(lcsObject.LcsLength);
            Console.WriteLine(lcsObject.DnaLcs.Seq1);
            Console.WriteLine();
            Console.WriteLine(lcsObject.DnaLcs.Seq2);
            Console.WriteLine();
            //Console.WriteLine(lcs.getNumberOfOptimalSolutions());
            */
            Console.ReadKey();

            
        }
    }
}
