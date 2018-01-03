using NUnit.Framework;
using StringAlgorithms;
using StringAlgorithms.AlignmentAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioStringAlgotihms.Tests
{
    [TestFixture()]
    public class MultipleAlignmentTempTests
    {
        MultipleAlignmentTemp algorithm;
        TextAlignmentParameters parameters;
        int[,] c;
        [OneTimeSetUp]
        public void SetUp()
        {
            
            //seq1 = "ACGT";
            //seq2 = "ATTCT";
            //seq3 = "CTCGA";
            //seq4 = "ACGGT";
            string seq1 = @"ATGTCCTTTGTGTAAGAATGA";
            string seq2 = @"ATGTCCTTTGTGTAAGAACGA";
            string seq3 = @"TCCAGTCTGTTTAGATGTGAT";
            string seq4 = @"CTCAGGAGGCCTTCACCCTCT";
            string seq5 = @"GTGTCCTTTGTGTAAGAATGA";

            parameters = new TextAlignmentParameters();
            c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            //parameters.AddSequence(seq1);
            //parameters.AddSequence(seq2);
            //parameters.AddSequence(seq3);
            //parameters.AddSequence(seq4);
            //parameters.AddSequence(seq5);
            parameters.GapCostFun = ((x) => (x * 5));
            parameters.Comparefunction = ((x, y) => (Math.Min(x, y)));
            parameters.CostArray = new StringCostArray(StringAlgorithms.Constants.DNA, c);
            //algorithm = new MultipleAlignmentTemp(parameters);
        }

        [Test]
        public void MultipleAlignmentTempTest()
        {
            Assert.Fail();
        }

        [Test]
        public void GetOptimalAlignmentScoreTest1()
        {
            string seq1 = @"ATGTCCTTTGTGTAAGAATGA";
            string seq2 = @"ATGTCCTTTGTGTAAGAACGA";
            string seq3 = @"TCCAGTCTGTTTAGATGTGAT";

            bool anyChange = DoesScoreChange(new List<string>() { seq1, seq2, seq3 });

            parameters.AddSequence(seq1);
            parameters.AddSequence(seq2);
            parameters.AddSequence(seq3);
            algorithm = new MultipleAlignmentTemp(parameters);

            MultipleAlignment exactAlgorithm = new MultipleAlignment(parameters);
            List<string> al = exactAlgorithm.GetOptimalAlignment();
            WriteToFile(@"C:\Users\Me\Desktop\Graph\Alignment.txt",al);
            int scoreCheckExact = CountMultipleAlignmentScore(al);

            int exactScore = exactAlgorithm.GetOptimalAlignmentScore();
            double approxScoreDouble = (double)4 / 3 * exactScore;

            al = algorithm.GetOptimalAlignment();
            WriteToFile(@"C:\Users\Me\Desktop\Graph\Alignment2.txt", al);
            int approxScore = algorithm.GetOptimalAlignmentScore();
            int scoreCheckApprox = CountMultipleAlignmentScore(al);
            Assert.That(approxScore, Is.AtMost((int)approxScore));
            Assert.That(anyChange, Is.False);
            Assert.That(exactScore, Is.EqualTo(scoreCheckExact));
            Assert.That(approxScore, Is.EqualTo(scoreCheckApprox));
        }

        [Test]
        public void GetOptimalAlignmentScoreTest2()
        {
            string seq1 = @"ATGGATTTATCTGCGGATCATGTTGAAGAAGTACAAAATGTCCTCAATGCTATGCAGAAAATCTTAGAGTGTCCAATATGTCTGGAGTTGATCAAAGAGCCTGTCTCTACAAAGTGTGACCACATATTTTGCAAATTTTGTATGCTGAAACTTCTCAACCAGAAGAAAGGGCCTTCACAATGTCCTTTGTGTAAGAATGA";
            string seq2 = @"ATGGATTTATCTGCGGATCGTGTTGAAGAAGTACAAAATGTTCTTAATGCTATGCAGAAAATCTTAGAGTGTCCAATATGTCTGGAGTTGATCAAAGAGCCTGTTTCTACAAAGTGTGATCACATATTTTGCAAATTTTGTATGCTGAAACTTCTCAACCAGAGGAAGGGGCCTTCACAGTGTCCTTTGTGTAAGAACGA";
            string seq3 = @"GCGAAATGTAACACGGTAGAGGTGATCGGGGTGCGTTATACGTGCGTGGTGACCTCGGTCGGTGTTGACGGTGCCTGGGGTTCCTCAGAGTGTTTTGGGGTCTGAAGGATGGACTTGTCAGTGATTGCCATTGGAGACGTGCAAAATGTGCTTTCAGCCATGCAGAAGAACTTGGAGTGTCCAGTCTGTTTAGATGTGAT";
            string seq4 = @"GTACCTTGATTTCGTATTCTGAGAGGCTGCTGCTTAGCGGTAGCCCCTTGGTTTCCGTGGCAACGGAAAAGCGCGGGAATTACAGATAAATTAAAACTGCGACTGCGCGGCGTGAGCTCGCTGAGACTTCCTGGACGGGGGACAGGCTGTGGGGTTTCTCAGATAACTGGGCCCCTGCGCTCAGGAGGCCTTCACCCTCT";
            string seq5 = @"ATGGATTTATCTGCTGTTCGCGTTGAAGAAGTACAAAATGTCATTAATGCTATGCAGAAAATCTTAGAGTGTCCAATCTGTCTGGAGTTGATCAAGGAACCTGTCTCCACAAAGTGTGACCACATATTTTGCAGATTTTGCATGCTGAAACTTCTCAACCAGAAGAAAGGGCCTTCACAGTGTCCTTTGTGTAAGAATGA";


            bool anyChange = DoesScoreChange(new List<string>() { seq1, seq2, seq3 });

            parameters.AddSequence(seq1);
            parameters.AddSequence(seq2);
            parameters.AddSequence(seq3);
            algorithm = new MultipleAlignmentTemp(parameters);

            MultipleAlignment exactAlgorithm = new MultipleAlignment(parameters);
            List<string> al = exactAlgorithm.GetOptimalAlignment();
            WriteToFile(@"C:\Users\Me\Desktop\Graph\Alignment.txt", al);
            int scoreCheckExact = CountMultipleAlignmentScore(al);

            int exactScore = exactAlgorithm.GetOptimalAlignmentScore();
            double approxScoreDouble = (double)4 / 3 * exactScore;

            al = algorithm.GetOptimalAlignment();
            WriteToFile(@"C:\Users\Me\Desktop\Graph\Alignment2.txt", al);
            int approxScore = algorithm.GetOptimalAlignmentScore();
            int scoreCheckApprox = CountMultipleAlignmentScore(al);
            Assert.That(approxScore, Is.AtMost((int)approxScore));
            Assert.That(anyChange, Is.False);
            Assert.That(exactScore, Is.EqualTo(scoreCheckExact));
            Assert.That(approxScore, Is.EqualTo(scoreCheckApprox));
        }

        [Test()]
        public void GetOptimalAlignmentTest()
        {

            List<string> alignment = algorithm.GetOptimalAlignment();
            int i = 1;
            Assert.That(i, Is.EqualTo(2));
        }
        [Test]
        public void GetOptimalAlignmentScoreTest3()
        {
            string seq1 = @"ATGGATTTATCTGCGGATCATGTTGAAGAAGTACAAAATGTCCTCAATGCTATGCAGAAAATCTTAGAGTGTCCAATATGTCTGGAGTTGATCAAAGAGCCTGTCTCTACAAAGTGTGACCACATATTTTGCAAATTTTGTATGCTGAAACTTCTCAACCAGAAGAAAGGGCCTTCACAATGTCCTTTGTGTAAGAATGA";
            string seq2 = @"ATGGATTTATCTGCGGATCGTGTTGAAGAAGTACAAAATGTTCTTAATGCTATGCAGAAAATCTTAGAGTGTCCAATATGTCTGGAGTTGATCAAAGAGCCTGTTTCTACAAAGTGTGATCACATATTTTGCAAATTTTGTATGCTGAAACTTCTCAACCAGAGGAAGGGGCCTTCACAGTGTCCTTTGTGTAAGAACGA";
            string seq3 = @"GCGAAATGTAACACGGTAGAGGTGATCGGGGTGCGTTATACGTGCGTGGTGACCTCGGTCGGTGTTGACGGTGCCTGGGGTTCCTCAGAGTGTTTTGGGGTCTGAAGGATGGACTTGTCAGTGATTGCCATTGGAGACGTGCAAAATGTGCTTTCAGCCATGCAGAAGAACTTGGAGTGTCCAGTCTGTTTAGATGTGAT";
            string seq4 = @"GTTCCGAAAGGCTAGCGCTAGGCGCCAAGCGGCCGGTTTCCTTGGCGACGGAGAGCGCGGGAATTTTAGATAGATTGTAATTGCGGCTGCGCGGCCGCTGCCCGTGCAGCCAGAGGATCCAGCACCTCTCTTGGGGCTTCTCCGTCCTCGGCGCTTGGAAGTACGGATCTTTTTTCTCGGAGAAAAGTTCACTGGAACTG";
            string seq5 = @"ATGGATTTATCTGCTCTTCGCGTTGAAGAAGTACAAAATGTCATTAACGCTATGCAGAAAATCTTAGAGTGTCCCATCTGTCTGGAGTTGATCAAGGAACCTGTCTCCACAAAGTGTGACCACATATTTTGCAAATTTTGCATGCTGAAACTTCTCAACCAGAAGAAAGGGCCTTCACAGTGTCCTTTATGTAAGAATGA";
            string seq6 = @"CGCTGGTGCAACTCGAAGACCTATCTCCTTCCCGGGGGGGCTTCTCCGGCATTTAGGCCTCGGCGTTTGGAAGTACGGAGGTTTTTCTCGGAAGAAAGTTCACTGGAAGTGGAAGAAATGGATTTATCTGCTGTTCGAATTCAAGAAGTACAAAATGTCCTTCATGCTATGCAGAAAATCTTGGAGTGTCCAATCTGTTT";
            //string seq1 = @"GTTCCGAAAGGCTAGCGCTAGGCGCC";
            //string seq2 = @"ATGGATTTATCTGCTCTTCG";
            //string seq3 = @"TGCATGCTGAAACTTCTCAACCA";
            //string seq1 = @"AG";
            //string seq2 = @"AT";
            //string seq3 = @"AG";
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };

            TextAlignmentParameters parameters = new TextAlignmentParameters();
            parameters.AddSequence(seq1);
            parameters.AddSequence(seq2);
            parameters.AddSequence(seq3);
            parameters.AddSequence(seq4);
            parameters.AddSequence(seq5);
            parameters.GapCostFun = ((x) => (x * 5));
            parameters.Comparefunction = ((x, y) => (Math.Min(x, y)));
            parameters.CostArray = new StringCostArray(StringAlgorithms.Constants.DNA, c);
            algorithm = new MultipleAlignmentTemp(parameters);
            bool anyChange = DoesScoreChange(parameters.GetSequences());

            List<int> scores = new List<int>();
            //for (int bestId = 0; bestId < 5; ++bestId)
            //{
            //    for (int i = 0; i < 5; ++i)
            //    {
            //        if (i == bestId) continue;
            //        for (int j = 0; j < 5; ++j)
            //        {
            //            if (j == bestId || j == i) continue;
            //            for (int k = 0; k < 5; ++k)
            //            {
            //                if (k == bestId || k == i || k ==j) continue;
            //                for (int l = 0; l < 5; ++l)
            //                {
            //                    if (l == bestId || l == i || l == j || l == k) continue;
            //                    scores.Add(algorithm.GetOptimalAlignmentScore2(bestId,i,j,k,l));
            //                }
            //            }
            //        }
            //    }
            //}

            algorithm = new MultipleAlignmentTemp(parameters);
            int score = algorithm.GetOptimalAlignmentScore();
            Assert.That(score, Is.AtMost(264));
        }


        

        public bool DoesScoreChange(List<string>seq)
        {

            TextAlignmentParameters parameters = new TextAlignmentParameters();
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            parameters.GapCostFun = ((x) => (x * 5));
            parameters.Comparefunction = ((x, y) => (Math.Min(x, y)));
            parameters.CostArray = new StringCostArray(StringAlgorithms.Constants.DNA, c);
            parameters.SetSequences(seq);

            GlobalAlignment globalAlignment = new GlobalAlignment(parameters);
            int bestScore = int.MaxValue - 1000;
            int bestIdx = 0;

            List<string> sequences = parameters.GetSequences();
            for (int i = 0; i < sequences.Count(); ++i)
            {
                int score = 0;
                for (int j = 0; j < sequences.Count(); ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    parameters.SetSequences(new List<string>() { sequences[i], sequences[j] });
                    globalAlignment.Parameters = parameters;
                    score += globalAlignment.GetOptimalAlignmentScore();
                }
                if (score < bestScore)
                {
                    bestScore = score;
                    bestIdx = i;
                }
            }

            List<int> scoresOrg = CountParwiseAlignmentsScores(parameters, globalAlignment, bestIdx, sequences);

            

            parameters.SetSequences(sequences);
            algorithm = new MultipleAlignmentTemp(parameters);
            List<string> alignment = algorithm.GetOptimalAlignment();
            List<int> scoresAft = CountParwiseAlignmentsScores2(parameters, globalAlignment, 0, alignment);
            //for (int j = 1; j < alignment.Count(); ++j)
            //{

            //    parameters.SetSequences(new List<string>() { sequences[0], sequences[j] });
            //    globalAlignment.Parameters = parameters;
            //    scoresAft.Add(globalAlignment.GetOptimalAlignmentScore());
            //}

            for (int i = 0; i < scoresOrg.Count(); ++i)
            {
                if (scoresOrg[i] != scoresAft[i])
                    return true;
            }
            return false;
        }

        private List<int> CountParwiseAlignmentsScores2(TextAlignmentParameters parameters, GlobalAlignment globalAlignment, int bestIdx, List<string> sequences)
        {
            List<int> scoresOrg = new List<int>();
            for (int j = 0; j < sequences.Count(); ++j)
            {
                if (j == bestIdx)
                {
                    continue;
                }

                //parameters.SetSequences(new List<string>() { sequences[bestIdx], sequences[j] });
                //globalAlignment.Parameters = parameters;
                int score = CountMultipleAlignmentScore(new List<string>() { sequences[bestIdx], sequences[j] });
                scoresOrg.Add(score);
            }
            return scoresOrg;
        }

        private List<int> CountParwiseAlignmentsScores(TextAlignmentParameters parameters, GlobalAlignment globalAlignment, int bestIdx, List<string> sequences)
        {
            List<int> scoresOrg = new List<int>();
            for (int j = 0; j < sequences.Count(); ++j)
            {
                if (j == bestIdx)
                {
                    continue;
                }

                parameters.SetSequences(new List<string>() { sequences[bestIdx], sequences[j] });
                globalAlignment.Parameters = parameters;
                scoresOrg.Add(globalAlignment.GetOptimalAlignmentScore());
            }
            return scoresOrg;
        }

        private int CountMultipleAlignmentScore(List<string> alignment)
        {
            int score = 0;
            for (int i = 0; i < alignment.First().Length; ++i)
            {
                for (int j = 0; j < alignment.Count(); ++j)
                {
                    for (int k = j + 1; k < alignment.Count(); ++k)
                    {
                        if (j == k)
                        {
                            continue;
                        }
                        score += parameters.GetSingleCost($"{alignment[j][i]}{alignment[k][i]}");
                    }
                }
            }
            return score;
        }

        private void WriteToFile(string path, List<string> lines)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path))
            {
                foreach (string line in lines)
                {

                    file.WriteLine(line);

                }
            }
        }
    }
}