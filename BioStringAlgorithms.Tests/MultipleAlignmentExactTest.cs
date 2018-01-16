using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringAlgorithms;

using System.Resources;
using System.Collections;

namespace BioStringAlgorithms.Tests
{
    [TestFixture]
    class MultipleAlignmentExactTest
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
            MultipleAlignmentExact globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new MultipleAlignmentExact(parameters), sequences, MINFUN, CONSTCOST) as MultipleAlignmentExact;

            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();
            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(scoreTest, Is.EqualTo(score));
            Assert.That(score, Is.EqualTo(198));
        }

        [Test]
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
            MultipleAlignmentExact globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new MultipleAlignmentExact(parameters), sequences, MINFUN, CONSTCOST) as MultipleAlignmentExact;

            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();
            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(scoreTest, Is.EqualTo(score));
            Assert.That(score, Is.EqualTo(1482));
        }

        [Test]
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
            MultipleAlignmentExact globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new MultipleAlignmentExact(parameters), sequences, MINFUN, CONSTCOST) as MultipleAlignmentExact;

            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();
            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(scoreTest, Is.EqualTo(score));
            Assert.That(score, Is.EqualTo(790));
        }

        
        [Test]
       // [Ignore("")]
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
                byte[] file = (byte[])Properties.Resources.ResourceManager.GetObject($"testseqs_{i}_3");

                proteins = parser.ReadFromFile(file);

                List<Sequence> sequences = new List<Sequence>();

                foreach (Sequence seq in proteins)
                {
                    sequences.Add(seq);
                }
                TextAlignmentParameters parameters = new TextAlignmentParameters();
                MultipleAlignmentExact globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new MultipleAlignmentExact(parameters), sequences, MINFUN, CONSTCOST) as MultipleAlignmentExact;
                Alignment alignment = globalAlignment.GetOptimalAlignment();
                int score = globalAlignment.GetOptimalAlignmentScore();
                int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
                Assert.That(scoreTest, Is.EqualTo(score));
                countedScores.Add(score);
            }
            for(int i = 0; i < countedScores.Count();++i)
            {
                Assert.That(countedScores[i], Is.EqualTo(correctScores[i]));
            }

        }

        [Test]
        public void GetNumberOfOptimalSolutions_3()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"AAA";
            string str2 = @"AA";
            string str3 = @"AAA";
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            Sequence seq3 = new Sequence(Constants.DNA, "seq2", str3);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2, seq3 };
            MultipleAlignmentExact globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new MultipleAlignmentExact(parameters), sequences, MINFUN, CONSTCOST) as MultipleAlignmentExact;

            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();
            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
            int solutionsNumber = globalAlignment.GetNumberOfOptimalSolutions();

            Assert.That(scoreTest, Is.EqualTo(score));
            Assert.That(score, Is.EqualTo(10));
            Assert.That(solutionsNumber, Is.EqualTo(3));
        }


    }
}
