using NUnit.Framework;
using StringAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioStringAlgorithms.Tests
{
    [TestFixture]
    public class GlobalAlignmentAffineTest
    {
        TextAlignmentParameters.StrategyFun MAXFUN = (x, y) => Math.Max(x, y);
        TextAlignmentParameters.StrategyFun MINFUN = (x, y) => Math.Min(x, y);
        LetterAlignmentCostManager.CostFun AFFINECOST = x => 5 + x * 5;

        TestUtils testUtils;

        [OneTimeSetUp]
        public void SetUp()
        {
            testUtils = new TestUtils();
        }

        [Test]
        public void GetOptimalAlignmentScore_ReturnsScoreOfOptimalAlignment_24()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"acgtgtcaacgt".ToUpper();
            string str2 = @"acgtcgtagcta".ToUpper();

            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2 };
            GlobalAlignmentAffine globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new GlobalAlignmentAffine(parameters), sequences, MINFUN, AFFINECOST) as GlobalAlignmentAffine;
            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();
            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(scoreTest, Is.EqualTo(score));
            Assert.That(alignment.Sequences[0].Value, Is.EqualTo("acgtgtcaacgt".ToUpper()));
            Assert.That(alignment.Sequences[1].Value, Is.EqualTo("acgtcgtagcta".ToUpper()));
        }

        [Test]
        public void GetOptimalAlignmentScore_ReturnsScoreOfOptimalAlignment_22()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"aataat".ToUpper();
            string str2 = @"aagg".ToUpper();

            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2 };
            GlobalAlignmentAffine globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new GlobalAlignmentAffine(parameters), sequences, MINFUN, AFFINECOST) as GlobalAlignmentAffine;
            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();
            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(scoreTest, Is.EqualTo(score));
            Assert.That(alignment.Sequences[0].Value, Is.EqualTo("aataat".ToUpper()));
            Assert.That(alignment.Sequences[1].Value, Is.EqualTo("a--agg".ToUpper()));
        }

        [Test]
        public void GetOptimalAlignmentScore_ReturnsScoreOfOptimalAlignment_29()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"tccagaga".ToUpper();
            string str2 = @"tcgat".ToUpper();

            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2 };
            GlobalAlignmentAffine globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new GlobalAlignmentAffine(parameters), sequences, MINFUN, AFFINECOST) as GlobalAlignmentAffine;
            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();
            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(score, Is.EqualTo(29));
            Assert.That(scoreTest, Is.EqualTo(score));
            Assert.That(alignment.Sequences[0].Value, Is.EqualTo("tccagaga".ToUpper()));
            Assert.That(alignment.Sequences[1].Value, Is.EqualTo("tc---gat".ToUpper()));
        }

        [Test]
        public void GetOptimalAlignmentScore_ReturnsScoreOfOptimalAlignment_395()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"ggcctaaaggcgccggtctttcgtaccccaaaatctcggcattttaagataagtgagtgttgcgttacactagcgatctaccgcgtcttatacttaagcgtatgcccagatctgactaatcgtgcccccggattagacgggcttgatgggaaagaacagctcgtctgtttacgtataaacagaatcgcctgggttcgc".ToUpper();
            string str2 = @"gggctaaaggttagggtctttcacactaaagagtggtgcgtatcgtggctaatgtaccgcttctggtatcgtggcttacggccagacctacaagtactagacctgagaactaatcttgtcgagccttccattgagggtaatgggagagaacatcgagtcagaagttattcttgtttacgtagaatcgcctgggtccgc".ToUpper();

            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2 };
            GlobalAlignmentAffine globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new GlobalAlignmentAffine(parameters),sequences, MINFUN, AFFINECOST) as GlobalAlignmentAffine;
            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();
            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(score, Is.EqualTo(395));
            Assert.That(scoreTest, Is.EqualTo(score));
            Assert.That(alignment.Sequences[0].Value, Is.EqualTo("ggcctaaaggcgccggtctttcgtaccccaaaatctcggcattttaagataagtgagtgttgcgttacactagcgatctaccgcgtcttatacttaagcgtatgcccagatctgactaatcgtgcccccggattagacgggcttgatgggaaagaacagctcgtc------tgtttacgtataaacagaatcgcctgggttcgc".ToUpper()));
            Assert.That(alignment.Sequences[1].Value, Is.EqualTo("gggctaaaggttagggtctttcacactaaagagtggt-gcgtatcgtggctaatgtaccgcttctggtatc-gtggcttacggc--cagacctacaagtactagacctga--gaactaatcttgtcgagccttccattgagggtaatgggagagaacatcgagtcagaagttattcttgtttacgtagaatcgcctgggtccgc".ToUpper()));
        }

        [Test]
        public void GetNumberOfOptimalSolutions_ReturnsNumberOfOptimalSolutions_3()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"aataat".ToUpper();
            string str2 = @"aagg".ToUpper();
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2};
            GlobalAlignmentAffine globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new GlobalAlignmentAffine(parameters), sequences, MINFUN, AFFINECOST) as GlobalAlignmentAffine;
            int optimalSolutionsNumber = globalAlignment.GetNumberOfOptimalSolutions();
            Assert.That(optimalSolutionsNumber, Is.EqualTo(3));
        }

        [Test]
        public void GetNumberOfOptimalSolutions_ReturnsNumberOfOptimalSolutions_4()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"ggcctaaaggcgccggtctttcgtaccccaaaatctcggcattttaagataagtgagtgttgcgttacactagcgatctaccgcgtcttatacttaagcgtatgcccagatctgactaatcgtgcccccggattagacgggcttgatgggaaagaacagctcgtctgtttacgtataaacagaatcgcctgggttcgc".ToUpper();
            string str2 = @"gggctaaaggttagggtctttcacactaaagagtggtgcgtatcgtggctaatgtaccgcttctggtatcgtggcttacggccagacctacaagtactagacctgagaactaatcttgtcgagccttccattgagggtaatgggagagaacatcgagtcagaagttattcttgtttacgtagaatcgcctgggtccgc".ToUpper();

            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2 };
            GlobalAlignmentAffine globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new GlobalAlignmentAffine(parameters), sequences, MINFUN, AFFINECOST) as GlobalAlignmentAffine;
            int optimalSolutionsNumber = globalAlignment.GetNumberOfOptimalSolutions();
            Assert.That(optimalSolutionsNumber, Is.EqualTo(4));
        }
    }
}
