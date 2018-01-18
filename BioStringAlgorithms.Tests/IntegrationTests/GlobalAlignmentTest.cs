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
    public class GlobalAlignmentTest
    {
        TestUtils testUtils;
        TextAlignmentParameters.StrategyFun MAXFUN = (x, y) => Math.Max(x, y);
        TextAlignmentParameters.StrategyFun MINFUN = (x, y) => Math.Min(x, y);

        [OneTimeSetUp]
        public void SetUp()
        {
            testUtils = new TestUtils();
        }


        [Test]
        public void GetOptimalAlignment_CheckingIfComputedAligmentGivesComputedScore_27()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"TCCAGAGA";
            string str2 = @"TCGAT";

            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2 };
            GlobalAlignment globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new GlobalAlignment(parameters), sequences, MAXFUN , x => x * -5) as GlobalAlignment;
            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();
            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(scoreTest, Is.EqualTo(score));
        }

        [Test]
        public void GetOptimalAlignment_CheckingIfComputedAligmentGivesComputedScore_61()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"CGTGTCAAGTCT";
            string str2 = @"ACGTCGTAGCTAGG";


            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2 };
            GlobalAlignment globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new GlobalAlignment(parameters), sequences, MAXFUN, x => x * -5) as GlobalAlignment;
            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();

            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(scoreTest, Is.EqualTo(score));
        }

        [Test]
        public void GetOptimalAlignment_CheckingIfComputedAligmentGivesComputedScore_1346()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = System.IO.File.ReadAllText(@"E:\DOCUMENTS\Visual Studio 2017\Projects\Project1Bio\Testing\Seq1.txt");
            string str2 = System.IO.File.ReadAllText(@"E:\DOCUMENTS\Visual Studio 2017\Projects\Project1Bio\Testing\Seq2.txt");

            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2 };
            GlobalAlignment globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new GlobalAlignment(parameters), sequences, MAXFUN, x => x * -5) as GlobalAlignment;
            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();

            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(scoreTest, Is.EqualTo(score));
        }

        [Test]
        public void GetOptimalAlignment_CheckingIfComputedAligmentGivesComputedScore_60()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"AAAAAA";
            string str2 = @"AAAAAA";

            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2 };
            GlobalAlignment globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new GlobalAlignment(parameters), sequences, MAXFUN, x => x * -5) as GlobalAlignment;
            Alignment alignment = globalAlignment.GetOptimalAlignment();
            int score = globalAlignment.GetOptimalAlignmentScore();

            int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(scoreTest, Is.EqualTo(score));
        }

        [Test]
        public void GetNumberOfOptimalSolutions_CheckingComputedNumberOfSolutions_4()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"TCCAGAGA";
            string str2 = @"TCGAT";

            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2 };
            GlobalAlignment globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new GlobalAlignment(parameters), sequences, MAXFUN, x => x * -5) as GlobalAlignment;
            Alignment alignment = globalAlignment.GetOptimalAlignment();

            int solutionsNum = globalAlignment.GetNumberOfOptimalSolutions();
            Assert.That(solutionsNum, Is.EqualTo(4));
        }

        [Test]
        public void GetNumberOfOptimalSolutions_CheckingComputedNumberOfSolutions_3()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = @"CGTGTCAAGTCT";
            string str2 = @"ACGTCGTAGCTAGG";

            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2 };
            int[,] c = new int[,] { { 10, 2, 5, 2 }, { 2, 10, 2, 5 }, { 5, 2, 10, 2 }, { 2, 5, 2, 10 } };
            GlobalAlignment globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new GlobalAlignment(parameters), sequences, MAXFUN, x => x * -5,c) as GlobalAlignment;
    
            Alignment alignment = globalAlignment.GetOptimalAlignment();
            

            int solutionsNum = globalAlignment.GetNumberOfOptimalSolutions();
            Assert.That(solutionsNum, Is.EqualTo(3));
        }

        [Test]
        public void GetNumberOfOptimalSolutions_CheckingComputedNumberOfSolutions_256()
        {
            TextAlignmentParameters parameters = new TextAlignmentParameters();
            string str1 = System.IO.File.ReadAllText(@"E:\DOCUMENTS\Visual Studio 2017\Projects\Project1Bio\Testing\Seq1.txt");
            string str2 = System.IO.File.ReadAllText(@"E:\DOCUMENTS\Visual Studio 2017\Projects\Project1Bio\Testing\Seq2.txt");

            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            List<Sequence> sequences = new List<Sequence>() { seq1, seq2 };
            int[,] c = new int[,] { { 10, 2, 5, 2 }, { 2, 10, 2, 5 }, { 5, 2, 10, 2 }, { 2, 5, 2, 10 } };
            GlobalAlignment globalAlignment = testUtils.InitializeTextAlignmentAlgorithm(parameters, new GlobalAlignment(parameters), sequences, MAXFUN, x => x * -5, c) as GlobalAlignment;
            Alignment alignment = globalAlignment.GetOptimalAlignment();

            int solutionsNum = globalAlignment.GetNumberOfOptimalSolutions();
            Assert.That(solutionsNum, Is.EqualTo(256));
        }


    }
}
