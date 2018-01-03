using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SequenceParser;
using StringAlgorithms;

using System.Diagnostics;
using System.IO;

namespace TempHelper
{
    class Program
    {
        //        public static int GetScore(List<string> alignment, TextAlignmentParameters parameters)
        //        {         
        //            int score = 0;
        //            for (int i = 0; i < alignment.First().Length; ++i)
        //            {
        //                for (int j = 0; j < alignment.Count(); ++j)
        //                {
        //                    for (int k = j + 1; k < alignment.Count(); ++k)
        //                    {
        //                        if (j == k)
        //                        {
        //                            continue;
        //                        }
        //                        score += parameters.GetSingleCost($"{alignment[j][i]}{alignment[k][i]}");
        //                    }
        //                }
        //            }
        //            return score;
        //        }

        //        public static int My(List<string> sequences, ref bool flag)
        //        {
        //            Stopwatch sw = new Stopwatch();

        //            double totalTime = 0; 


        //            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };

        //            TextAlignmentParameters parameters = new TextAlignmentParameters();
        //            parameters.SetSequences(sequences);
        //            parameters.GapCostFun = ((x) => (x * 5));
        //            parameters.Comparefunction = ((x, y) => (Math.Min(x, y)));
        //            parameters.CostArray = new StringCostArray(StringAlgorithms.Constants.DNA, c);
        //            MultipleAlignmentTemp alignmentAlgorithm = new MultipleAlignmentTemp(parameters);

        //            sw.Reset();
        //            sw.Start();
        //            List<string> al = alignmentAlgorithm.GetOptimalAlignment();
        //            sw.Stop();
        //            totalTime += sw.Elapsed.TotalMilliseconds;
        //            Console.WriteLine(totalTime + "-finding alignmentTimeApp");
        //            //WriteToFile(al, @"C:\Users\Me\Desktop\Presentation\align4App.fasta");

        //            int scoreCheckApprox = CountMultipleAlignmentScore(al,parameters);

        //            sw.Reset();
        //            sw.Start();
        //            int score = alignmentAlgorithm.GetOptimalAlignmentScore();
        //            sw.Stop();
        //            totalTime += sw.Elapsed.TotalMilliseconds;
        //            Console.WriteLine(sw.Elapsed.TotalMilliseconds + "-finding alignmentScoreTimeApp");
        //            Console.WriteLine(totalTime + "-Sum");

        //            flag = (scoreCheckApprox == score) ? true : false;
        //            return score;
        //        }

        //        public static int Opt(List<string> sequences, ref bool flag)
        //        {
        //            Stopwatch sw = new Stopwatch();

        //            double totalTime = 0;

        //            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };

        //            TextAlignmentParameters parameters = new TextAlignmentParameters();
        //            parameters.SetSequences(sequences);
        //            parameters.GapCostFun = ((x) => (x * 5));
        //            parameters.Comparefunction = ((x, y) => (Math.Min(x, y)));
        //            parameters.CostArray = new StringCostArray(StringAlgorithms.Constants.DNA, c);
        //            MultipleAlignment alignmentAlgorithm = new MultipleAlignment(parameters);

        //            sw.Reset();
        //            sw.Start();
        //            List<string> al = alignmentAlgorithm.GetOptimalAlignment();
        //            sw.Stop();
        //            totalTime += sw.Elapsed.TotalMilliseconds;
        //            Console.WriteLine(totalTime + "-finding alignmentTimeOpt");
        //            WriteToFile(al, @"C:\Users\Me\Desktop\Presentation\align2Opt.fasta");

        //            int scoreCheckExact = CountMultipleAlignmentScore(al,parameters);

        //            sw.Reset();
        //            sw.Start();
        //            int score = alignmentAlgorithm.GetOptimalAlignmentScore();
        //            sw.Stop();
        //            totalTime += sw.Elapsed.TotalMilliseconds;
        //            Console.WriteLine(sw.Elapsed.TotalMilliseconds + "-finding alignmentScoreTimeOpt");
        //            Console.WriteLine(totalTime + "-Sum");

        //            flag = (scoreCheckExact == score) ? true : false;
        //            return score;
        //        }

        static private int index(int m)
        {
            int i = 0;
            while (m % 2 == 0)
            {
                i++;
                m = m / 2;
            }
            return i;
        }
        static public List<int>[] variancy(int k)
        {
            int i;
            int[] skok = new int[k + 1];
            for (i = 1; i <= k; ++i)
            {
                skok[i] = 1;
            }

            int m = 0;
            i = 0;
            int[] w = new int[k + 1];
            List<int>[] ret = new List<int>[(int)Math.Pow(2, k) + 1];
            for(int it = 0; it < ret.Length; ++it)
            {
                ret[it] = new List<int>();
            }
            int col = 0;
            do
            {
                ++m;
                i = index(m) + 1;
                if (i <= k)
                {
                    w[i] = w[i] + skok[i];
                    if (w[i] == 0)
                    {
                        skok[i] = 1;
                    }
                    if (w[i] == 1)
                    {
                        skok[i] = -1;
                    }
                }
                else
                {
                    w[i-1] = w[i-1] + skok[i-1];
                }
                for (int j = 1; j <= k; ++j)
                {
                    ret[col].Add(w[j]);
                }
                ++col;
            } while (i <= k);

            //for(int j = 0; j <= k; ++j)
            //{
            //    for(int o = 0; o < (int)Math.Pow(2, k) + 1; ++o)
            //    {
            //        Console.Write(ret[j,o] + " ");
            //    }
            //    Console.WriteLine();
            //}
            return ret;
        }


        static void Main(string[] args)
        {
            List<int>[] v = variancy(2);
            Console.ReadKey();
        }




        //            //string seq1 = @"GCTAGTAAGACCATCGGTAGCTAGGCCTCGCCTATGCGCGTAGTGGCAAGACTCTATTAA";
        //            //string seq3 = @"TACCGGTGGATCGCACTTGTGCATTTACAGGTAGAATCCCCGTTCCTTCATCCGTTGGGT";
        //            //string seq2 = @"TGTTCGACTTAAGACTGCATAATCTGTGAAGATGCTAGGTGCGTAGTGACCGCCTGTCTT";
        //            //List<string> sequences = new List<string>();
        //            //sequences.Add(seq1);
        //            //sequences.Add(seq2);
        //            //sequences.Add(seq3);

        //            //int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };

        //            //TextAlignmentParameters parameters = new TextAlignmentParameters();

        //            //parameters.GapCostFun = ((x) => (x * 5));
        //            //parameters.Comparefunction = ((x, y) => (Math.Min(x, y)));
        //            //parameters.CostArray = new StringCostArray(StringAlgorithms.Utils.Constants.DNA, c);
        //            //int score = GetScore(sequences, parameters);
        //            //Console.WriteLine(score);

        //            /*
        //            List<StringPair> sequences = new List<StringPair>();
        //            SequenceParser.SequenceParser parser = new SequenceParser.SequenceParser();
        //            //sequences = parser.ReadFromFile(@"C:\Users\Me\documents\visual studio 2015\Projects\Project1Bio\TempHelper\Aln-Muscle.fasta");
        //            sequences = parser.ReadFromFile(@"C:\Users\Me\documents\visual studio 2015\Projects\Project1Bio\TempHelper\proteins3OUT1.fasta");

        //            List<string> aligments = new List<string>();

        //            foreach(StringPair pair in sequences)
        //            {
        //                aligments.Add(pair.Second);
        //            }

        //            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };

        //            TextAlignmentParameters parameters = new TextAlignmentParameters();
        //            parameters.GapCostFun = ((x) => (x * 5));
        //            parameters.Comparefunction = ((x, y) => (Math.Min(x, y)));
        //            parameters.CostArray = new StringCostArray(StringAlgorithms.Utils.Constants.DNA, c);

        //            int score = GetScore(aligments, parameters);



        //            //int score = My(aligments);
        //            Console.WriteLine(score);

        //            Console.ReadKey();
        //            */

        //            //--------------------------------------------------

        //            //List<StringPair> proteins = new List<StringPair>();
        //            //SequenceParser.SequenceParser parser = new SequenceParser.SequenceParser();
        //            //for (int i = 10; i <= 200; i += 10)
        //            //{
        //            //    string path = $@"C:\Users\Me\Desktop\Alignments\testseqs\testseqs_{i}_3.fasta";
        //            //    proteins = parser.ReadFromFile(path);

        //            //    List<string> sequences = new List<string>();

        //            //    foreach (StringPair pair in proteins)
        //            //    {
        //            //        sequences.Add(pair.Second);
        //            //    }

        //            //    int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };

        //            //    TextAlignmentParameters parameters = new TextAlignmentParameters();
        //            //    parameters.GapCostFun = ((x) => (x * 5));
        //            //    parameters.Comparefunction = ((x, y) => (Math.Min(x, y)));
        //            //    parameters.CostArray = new StringCostArray(StringAlgorithms.Constants.DNA, c);

        //            //    bool flag1 = true;
        //            //    int scoreOpt = Opt(sequences, ref flag1);

        //            //    double approxScoreDouble = (double)4 / 3 * scoreOpt;

        //            //    bool flag2 = true;
        //            //    int scoreApp = My(sequences, ref flag2);

        //            //    bool anyChange = DoesScoreChange(sequences);


        //            //    Console.WriteLine($@"i: {i} scoreApp: {scoreApp} scoreOpt: {scoreOpt}");
        //            //    Console.WriteLine($@"OptAligExactGivesCountedScore: {flag1}, OptAligAppGivesCountedScore: {flag2}, thereAreNoChaneg: {anyChange}");
        //            //    Console.WriteLine($@"Is approximation <= 4/3*optimal score: {scoreApp <= (int)approxScoreDouble}");
        //            //}

        //            //List<StringPair> proteins = new List<StringPair>();
        //            //SequenceParser.SequenceParser parser = new SequenceParser.SequenceParser();

        //            //string path = $@"C:\Users\Me\Desktop\Presentation\brca1-testseqs.fasta";
        //            //proteins = parser.ReadFromFile(path);

        //            //List<string> sequences = new List<string>();

        //            //for (int i = 0; i < 6; ++i)
        //            //{
        //            //    sequences.Add(proteins[i].Second);
        //            //}

        //            ////foreach (StringPair pair in proteins)
        //            ////{
        //            ////    sequences.Add(pair.Second);
        //            ////}

        //            //int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };

        //            //TextAlignmentParameters parameters = new TextAlignmentParameters();
        //            //parameters.GapCostFun = ((x) => (x * 5));
        //            //parameters.Comparefunction = ((x, y) => (Math.Min(x, y)));
        //            //parameters.CostArray = new StringCostArray(StringAlgorithms.Constants.DNA, c);

        //            ////bool flag1 = true;
        //            ////int scoreOpt = Opt(sequences, ref flag1);

        //            ////double approxScoreDouble = (double)4 / 3 * scoreOpt;

        //            //bool flag2 = true;
        //            //int scoreApp = My(sequences, ref flag2);

        //            //bool anyChange = DoesScoreChange(sequences);


        //            //Console.WriteLine($@"scoreApp: {scoreApp}");
        //            //Console.WriteLine($@"OptAligAppGivesCountedScore: {flag2}, thereAreNoChaneg: {anyChange}");
        //            //Console.WriteLine($@"Is approximation <= 4/3*optimal score: {scoreApp <= (int)approxScoreDouble}");



        //            //FULL DNA LAST ONE!!
        //            List<Sequence> proteins = new List<Sequence>();
        //            SequenceParser.SequenceParser parser = new SequenceParser.SequenceParser();

        //            string path = $@"C:\Users\Me\Desktop\Presentation\brca1-full.fasta";
        //            proteins = parser.ReadFromFile(path);

        //            List<string> sequences = new List<string>();

        //            foreach (Sequence pair in proteins)
        //            {
        //                sequences.Add(pair.Value);
        //            }

        //            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };

        //            TextAlignmentParameters parameters = new TextAlignmentParameters();
        //            parameters.GapCostFun = ((x) => (x * 5));
        //            parameters.Comparefunction = ((x, y) => (Math.Min(x, y)));
        //            parameters.CostArray = new StringCostArray(StringAlgorithms.Constants.DNA, c);

        //            bool flag1 = true;
        //            //int scoreOpt = Opt(sequences, ref flag1);

        //            //double approxScoreDouble = (double)4 / 3 * scoreOpt;


        //            bool flag2 = true;

        //            int scoreApp = My(sequences, ref flag2);

        //            bool anyChange = DoesScoreChange(sequences);


        //            Console.WriteLine($@"scoreApp: {scoreApp}");
        //            Console.WriteLine($@"OptAligAppGivesCountedScore: {flag2}, thereAreNoChaneg: {anyChange}");
        //            //Console.WriteLine($@"Is approximation <= 4/3*optimal score: {scoreApp <= (int)approxScoreDouble}");



        //            Console.ReadKey();

        //        }


        //        private static void WriteToFile(List<String> seq, string filePath)
        //        {
        //            using (var writer = new StreamWriter(filePath))
        //            {
        //                foreach(string s in seq)
        //                {
        //                    writer.WriteLine(s);
        //                }
        //            }
        //        }


        //        private static int CountMultipleAlignmentScore(List<string> alignment, TextAlignmentParameters parameters)
        //        {
        //            int score = 0;
        //            for (int i = 0; i < alignment.First().Length; ++i)
        //            {
        //                for (int j = 0; j < alignment.Count(); ++j)
        //                {
        //                    for (int k = j + 1; k < alignment.Count(); ++k)
        //                    {
        //                        if (j == k)
        //                        {
        //                            continue;
        //                        }
        //                        score += parameters.GetSingleCost($"{alignment[j][i]}{alignment[k][i]}");
        //                    }
        //                }
        //            }
        //            return score;
        //        }

        //        private static List<int> CountParwiseAlignmentsScores(TextAlignmentParameters parameters, GlobalAlignment globalAlignment, int bestIdx, List<string> sequences)
        //        {
        //            List<int> scoresOrg = new List<int>();
        //            for (int j = 0; j < sequences.Count(); ++j)
        //            {
        //                if (j == bestIdx)
        //                {
        //                    continue;
        //                }

        //                parameters.SetSequences(new List<string>() { sequences[bestIdx], sequences[j] });
        //                globalAlignment.Parameters = parameters;
        //                scoresOrg.Add(globalAlignment.GetOptimalAlignmentScore());
        //            }
        //            return scoresOrg;
        //        }

        //        private static List<int> CountParwiseAlignmentsScores2(TextAlignmentParameters parameters, GlobalAlignment globalAlignment, int bestIdx, List<string> sequences)
        //        {
        //            List<int> scoresOrg = new List<int>();
        //            for (int j = 0; j < sequences.Count(); ++j)
        //            {
        //                if (j == bestIdx)
        //                {
        //                    continue;
        //                }

        //                //parameters.SetSequences(new List<string>() { sequences[bestIdx], sequences[j] });
        //                //globalAlignment.Parameters = parameters;
        //                int score = CountMultipleAlignmentScore(new List<string>() { sequences[bestIdx], sequences[j] }, parameters);
        //                scoresOrg.Add(score);
        //            }
        //            return scoresOrg;
        //        }


        //        public static bool DoesScoreChange(List<string> seq)
        //        {

        //            TextAlignmentParameters parameters = new TextAlignmentParameters();
        //            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
        //            parameters.GapCostFun = ((x) => (x * 5));
        //            parameters.Comparefunction = ((x, y) => (Math.Min(x, y)));
        //            parameters.CostArray = new StringCostArray(StringAlgorithms.Constants.DNA, c);
        //            parameters.SetSequences(seq);

        //            GlobalAlignment globalAlignment = new GlobalAlignment(parameters);
        //            int bestScore = int.MaxValue - 1000;
        //            int bestIdx = 0;

        //            List<string> sequences = parameters.GetSequences();
        //            for (int i = 0; i < sequences.Count(); ++i)
        //            {
        //                int score = 0;
        //                for (int j = 0; j < sequences.Count(); ++j)
        //                {
        //                    if (i == j)
        //                    {
        //                        continue;
        //                    }

        //                    parameters.SetSequences(new List<string>() { sequences[i], sequences[j] });
        //                    globalAlignment.Parameters = parameters;
        //                    score += globalAlignment.GetOptimalAlignmentScore();
        //                }
        //                if (score < bestScore)
        //                {
        //                    bestScore = score;
        //                    bestIdx = i;
        //                }
        //            }

        //            List<int> scoresOrg = CountParwiseAlignmentsScores(parameters, globalAlignment, bestIdx, sequences);



        //            parameters.SetSequences(sequences);
        //            MultipleAlignmentTemp algorithm = new MultipleAlignmentTemp(parameters);
        //            List<string> alignment = algorithm.GetOptimalAlignment();
        //            List<int> scoresAft = CountParwiseAlignmentsScores2(parameters, globalAlignment, 0, alignment);

        //            parameters.SetSequences(sequences);
        //            MultipleAlignment ma = new MultipleAlignment(parameters);
        //            List<string> a = algorithm.GetOptimalAlignment();
        //            List<int> scoresExact = CountParwiseAlignmentsScores2(parameters, globalAlignment, 0, a);
        //            //for (int j = 1; j < alignment.Count(); ++j)
        //            //{

        //            //    parameters.SetSequences(new List<string>() { sequences[0], sequences[j] });
        //            //    globalAlignment.Parameters = parameters;
        //            //    scoresAft.Add(globalAlignment.GetOptimalAlignmentScore());
        //            //}

        //            for (int i = 0; i < scoresOrg.Count(); ++i)
        //            {
        //                if (scoresOrg[i] != scoresAft[i] || scoresOrg[i] != scoresExact[i])
        //                    return true;
        //            }
        //            return false;
        //        }
    }
}
