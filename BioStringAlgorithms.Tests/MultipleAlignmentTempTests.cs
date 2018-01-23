using BioStringAlgorithms.Tests;
using NUnit.Framework;
using StringAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;


namespace BioStringAlgotihms.Tests
{
    [TestFixture()]
    public class MultipleAlignmentTempTests
    {
        TestUtils testUtils;
        TextAlignmentParameters.StrategyFun MAXFUN = (x, y) => Math.Max(x, y);
        TextAlignmentParameters.StrategyFun MINFUN = (x, y) => Math.Min(x, y);
        LetterAlignmentCostManager.CostFun AFFINECOST = x => 5 + x * 5;
        LetterAlignmentCostManager.CostFun CONSTCOST = x => x * 5;


        [OneTimeSetUp]
        public void SetUp()
        {
            testUtils = new TestUtils();

        }


        [Test]
        [Ignore("")]
        public void GetOptimalAlignmentScoreTest_94opt()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"ATGTCCTTTGTGTAAGAATGA";
            string str2 = @"ATGTCCTTTGTGTAAGAACGA";
            string str3 = @"TCCAGTCTGTTTAGATGTGAT";

            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            Sequence seq3 = new Sequence(Constants.DNA, "seq2", str3);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2, seq3 };
            MultipleAlignmentTemp globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new MultipleAlignmentTemp(parameters), sequences, MINFUN, CONSTCOST) as MultipleAlignmentTemp;
            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();
            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);

            MultipleAlignmentExact exactAlgorithm = testUtils.InitializeTextAlignmentAlgorithm(parameters, new MultipleAlignmentExact(parameters), sequences, MINFUN, CONSTCOST) as MultipleAlignmentExact;
            Alignment al = exactAlgorithm.GetOptimalAlignment();
            int exactScore = exactAlgorithm.GetOptimalAlignmentScore();
            double approxScoreDouble = (double)4 / 3 * exactScore;

            Assert.That(scoreTest, Is.EqualTo(score));
            Assert.That(score, Is.AtMost((int)approxScoreDouble));
            Assert.That(score, Is.AtLeast(exactScore));
        }

        [Test]
        [Ignore("")]
        public void GetOptimalAlignmentScore_ReturnCountedAlignmentScore_198()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"GTTCCGAAAGGCTAGCGCTAGGCGCC";
            string str2 = @"ATGGATTTATCTGCTCTTCG";
            string str3 = @"TGCATGCTGAAACTTCTCAACCA";
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            Sequence seq3 = new Sequence(Constants.DNA, "seq2", str3);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2, seq3 };
            MultipleAlignmentTemp globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new MultipleAlignmentTemp(parameters), sequences, MINFUN, CONSTCOST) as MultipleAlignmentTemp;
            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();
            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);

            MultipleAlignmentExact exactAlgorithm = testUtils.InitializeTextAlignmentAlgorithm(parameters, new MultipleAlignmentExact(parameters), sequences, MINFUN, CONSTCOST) as MultipleAlignmentExact;
            Alignment al = exactAlgorithm.GetOptimalAlignment();
            int exactScore = exactAlgorithm.GetOptimalAlignmentScore();
            double approxScoreDouble = (double)4 / 3 * exactScore;

            Assert.That(scoreTest, Is.EqualTo(score));
            Assert.That(score, Is.AtMost((int)approxScoreDouble));
            Assert.That(score, Is.AtLeast(exactScore));
        }

        [Test]
        [Ignore("")]
        public void GetOptimalAlignmentScore_ReturnCountedAlignmentScore_1482()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"GTTCCGAAAGGCTAGCGCTAGGCGCCAAGCGGCCGGTTTCCTTGGCGACGGAGAGCGCGGGAATTTTAGATAGATTGTAATTGCGGCTGCGCGGCCGCTGCCCGTGCAGCCAGAGGATCCAGCACCTCTCTTGGGGCTTCTCCGTCCTCGGCGCTTGGAAGTACGGATCTTTTTTCTCGGAGAAAAGTTCACTGGAACTG";
            string str2 = @"ATGGATTTATCTGCTCTTCGCGTTGAAGAAGTACAAAATGTCATTAACGCTATGCAGAAAATCTTAGAGTGTCCCATCTGTCTGGAGTTGATCAAGGAACCTGTCTCCACAAAGTGTGACCACATATTTTGCAAATTTTGCATGCTGAAACTTCTCAACCAGAAGAAAGGGCCTTCACAGTGTCCTTTATGTAAGAATGA";
            string str3 = @"CGCTGGTGCAACTCGAAGACCTATCTCCTTCCCGGGGGGGCTTCTCCGGCATTTAGGCCTCGGCGTTTGGAAGTACGGAGGTTTTTCTCGGAAGAAAGTTCACTGGAAGTGGAAGAAATGGATTTATCTGCTGTTCGAATTCAAGAAGTACAAAATGTCCTTCATGCTATGCAGAAAATCTTGGAGTGTCCAATCTGTTT";
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            Sequence seq3 = new Sequence(Constants.DNA, "seq2", str3);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2, seq3 };
            MultipleAlignmentTemp globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new MultipleAlignmentTemp(parameters), sequences, MINFUN, CONSTCOST) as MultipleAlignmentTemp;
            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();
            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);

            MultipleAlignmentExact exactAlgorithm = testUtils.InitializeTextAlignmentAlgorithm(parameters, new MultipleAlignmentExact(parameters), sequences, MINFUN, CONSTCOST) as MultipleAlignmentExact;
            Alignment al = exactAlgorithm.GetOptimalAlignment();
            int exactScore = exactAlgorithm.GetOptimalAlignmentScore();
            double approxScoreDouble = (double)4 / 3 * exactScore;

            Assert.That(scoreTest, Is.EqualTo(score));
            Assert.That(score, Is.AtMost((int)approxScoreDouble));
            Assert.That(score, Is.AtLeast(exactScore));
        }

        [Test]
        [Ignore("")]
        public void GetOptimalAlignmentScore_ReturnCountedAlignmentScore_790()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"ATGGATTTATCTGCGGATCATGTTGAAGAAGTACAAAATGTCCTCAATGCTATGCAGAAAATCTTAGAGTGTCCAATATGTCTGGAGTTGATCAAAGAGCCTGTCTCTACAAAGTGTGACCACATATTTTGCAAATTTTGTATGCTGAAACTTCTCAACCAGAAGAAAGGGCCTTCACAATGTCCTTTGTGTAAGAATGA";
            string str2 = @"ATGGATTTATCTGCGGATCGTGTTGAAGAAGTACAAAATGTTCTTAATGCTATGCAGAAAATCTTAGAGTGTCCAATATGTCTGGAGTTGATCAAAGAGCCTGTTTCTACAAAGTGTGATCACATATTTTGCAAATTTTGTATGCTGAAACTTCTCAACCAGAGGAAGGGGCCTTCACAGTGTCCTTTGTGTAAGAACGA";
            string str3 = @"GCGAAATGTAACACGGTAGAGGTGATCGGGGTGCGTTATACGTGCGTGGTGACCTCGGTCGGTGTTGACGGTGCCTGGGGTTCCTCAGAGTGTTTTGGGGTCTGAAGGATGGACTTGTCAGTGATTGCCATTGGAGACGTGCAAAATGTGCTTTCAGCCATGCAGAAGAACTTGGAGTGTCCAGTCTGTTTAGATGTGAT";
            Sequence seq1 = new Sequence(Constants.DNA, "brca1_bos_taurus", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "brca1_canis_lupus", str2);
            Sequence seq3 = new Sequence(Constants.DNA, "brca1_gallus_gallus", str3);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2, seq3 };
            MultipleAlignmentTemp globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new MultipleAlignmentTemp(parameters), sequences, MINFUN, CONSTCOST) as MultipleAlignmentTemp;
            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();
            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);

            MultipleAlignmentExact exactAlgorithm = testUtils.InitializeTextAlignmentAlgorithm(parameters, new MultipleAlignmentExact(parameters), sequences, MINFUN, CONSTCOST) as MultipleAlignmentExact;
            Alignment al = exactAlgorithm.GetOptimalAlignment();
            int exactScore = exactAlgorithm.GetOptimalAlignmentScore();
            double approxScoreDouble = (double)4 / 3 * exactScore;

            Assert.That(scoreTest, Is.EqualTo(score));
            Assert.That(score, Is.AtMost((int)approxScoreDouble));
            Assert.That(score, Is.AtLeast(exactScore));
        }

        [Test]
        [Ignore("")]
        public void CountAlignmentScore_AreTheScoresCorrectForExamplesFovideInFiles()
        {
            List<int> countedScores = new List<int>();
            List<int> correctScores = new List<int>()
            {
                70,135,231,318,385,440,516,589,628,687,754,810,895,957,1023,1080,1186,1158,1323,1379
            };
            List<Sequence> proteins = new List<Sequence>();
            SequenceParser.SequenceParser parser = new SequenceParser.SequenceParser();
            for (int i = 10; i <= 200; i += 10)
            {
                byte[] file = (byte[])BioStringAlgorithms.Tests.Properties.Resources.ResourceManager.GetObject($"testseqs_{i}_3");

                proteins = parser.ReadFromFile(file);

                List<Sequence> sequences = new List<Sequence>();

                foreach (Sequence seq in proteins)
                {
                    sequences.Add(seq);
                }
                TextAlignmentParameters parameters = new TextAlignmentParameters();
                MultipleAlignmentTemp globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new MultipleAlignmentTemp(parameters), sequences, MINFUN, CONSTCOST) as MultipleAlignmentTemp;
                Alignment alignment = globalAlignment.GetOptimalAlignment();
                int score = globalAlignment.GetOptimalAlignmentScore();
                int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
                Assert.That(scoreTest, Is.EqualTo(score));
                countedScores.Add(score);
            }
            for (int i = 0; i < countedScores.Count(); ++i)
            {
                double approxScoreDouble = (double)4 / 3 * correctScores[i];
                Assert.That(countedScores[i], Is.AtMost((int)approxScoreDouble));
                Assert.That(countedScores[i], Is.AtLeast(correctScores[i]));
            }

        }



        //public bool DoesScoreChange(List<string>seq)
        //{

        //    TextAlignmentParameters parameters = new TextAlignmentParameters();
        //    int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
        //    parameters.GapCostFun = ((x) => (x * 5));
        //    parameters.Comparefunction = ((x, y) => (Math.Min(x, y)));
        //    parameters.CostArray = new StringCostArray(StringAlgorithms.Constants.DNA, c);
        //    parameters.SetSequences(seq);

        //    GlobalAlignment globalAlignment = new GlobalAlignment(parameters);
        //    int bestScore = int.MaxValue - 1000;
        //    int bestIdx = 0;

        //    List<string> sequences = parameters.GetSequences();
        //    for (int i = 0; i < sequences.Count(); ++i)
        //    {
        //        int score = 0;
        //        for (int j = 0; j < sequences.Count(); ++j)
        //        {
        //            if (i == j)
        //            {
        //                continue;
        //            }

        //            parameters.SetSequences(new List<string>() { sequences[i], sequences[j] });
        //            globalAlignment.Parameters = parameters;
        //            score += globalAlignment.GetOptimalAlignmentScore();
        //        }
        //        if (score < bestScore)
        //        {
        //            bestScore = score;
        //            bestIdx = i;
        //        }
        //    }

        //    List<int> scoresOrg = CountParwiseAlignmentsScores(parameters, globalAlignment, bestIdx, sequences);



        //    parameters.SetSequences(sequences);
        //    algorithm = new MultipleAlignmentTemp(parameters);
        //    List<string> alignment = algorithm.GetOptimalAlignment();
        //    List<int> scoresAft = CountParwiseAlignmentsScores2(parameters, globalAlignment, 0, alignment);
        //    //for (int j = 1; j < alignment.Count(); ++j)
        //    //{

        //    //    parameters.SetSequences(new List<string>() { sequences[0], sequences[j] });
        //    //    globalAlignment.Parameters = parameters;
        //    //    scoresAft.Add(globalAlignment.GetOptimalAlignmentScore());
        //    //}

        //    for (int i = 0; i < scoresOrg.Count(); ++i)
        //    {
        //        if (scoresOrg[i] != scoresAft[i])
        //            return true;
        //    }
        //    return false;
        //}

        //private List<int> CountParwiseAlignmentsScores2(TextAlignmentParameters parameters, GlobalAlignment globalAlignment, int bestIdx, List<string> sequences)
        //{
        //    List<int> scoresOrg = new List<int>();
        //    for (int j = 0; j < sequences.Count(); ++j)
        //    {
        //        if (j == bestIdx)
        //        {
        //            continue;
        //        }

        //        //parameters.SetSequences(new List<string>() { sequences[bestIdx], sequences[j] });
        //        //globalAlignment.Parameters = parameters;
        //        int score = CountMultipleAlignmentScore(new List<string>() { sequences[bestIdx], sequences[j] });
        //        scoresOrg.Add(score);
        //    }
        //    return scoresOrg;
        //}

        //private List<int> CountParwiseAlignmentsScores(TextAlignmentParameters parameters, GlobalAlignment globalAlignment, int bestIdx, List<string> sequences)
        //{
        //    List<int> scoresOrg = new List<int>();
        //    for (int j = 0; j < sequences.Count(); ++j)
        //    {
        //        if (j == bestIdx)
        //        {
        //            continue;
        //        }

        //        parameters.SetSequences(new List<string>() { sequences[bestIdx], sequences[j] });
        //        globalAlignment.Parameters = parameters;
        //        scoresOrg.Add(globalAlignment.GetOptimalAlignmentScore());
        //    }
        //    return scoresOrg;
        //}

        //private int CountMultipleAlignmentScore(List<string> alignment)
        //{
        //    int score = 0;
        //    for (int i = 0; i < alignment.First().Length; ++i)
        //    {
        //        for (int j = 0; j < alignment.Count(); ++j)
        //        {
        //            for (int k = j + 1; k < alignment.Count(); ++k)
        //            {
        //                if (j == k)
        //                {
        //                    continue;
        //                }
        //                score += parameters.GetSingleCost($"{alignment[j][i]}{alignment[k][i]}");
        //            }
        //        }
        //    }
        //    return score;
        //}

        //private void WriteToFile(string path, List<string> lines)
        //{
        //    using (System.IO.StreamWriter file =
        //    new System.IO.StreamWriter(path))
        //    {
        //        foreach (string line in lines)
        //        {

        //            file.WriteLine(line);

        //        }
        //    }
        //}
    }
}