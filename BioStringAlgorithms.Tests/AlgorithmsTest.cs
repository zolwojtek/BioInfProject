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
    class AlgorithmsTest
    {
        Algorithms algorithms;
        string a;
        string b;
        int[,] costs;
        TestUtils testUtils;
        TextAlignmentParameters.StrategyFun MAXFUN = (x, y) => Math.Max(x, y);
        TextAlignmentParameters.StrategyFun MINFUN = (x, y) => Math.Min(x, y);
        LetterAlignmentCostManager.CostFun AFFINECOST = x => 5 + x * 5;
        LetterAlignmentCostManager.CostFun CONSTCOST = x => x * 5;

        [OneTimeSetUp]
        public void SetUp()
        {
            testUtils = new TestUtils();
            algorithms = new Algorithms();
            this.a = @"CGTGTCAAGTCT";
            this.b = @"ACGTCGTAGCTAGG";
            this.costs = new int[,] { { 10, 2, 5, 2 }, { 2, 10, 2, 5 }, { 5, 2, 10, 2 }, { 2, 5, 2, 10 } }; 
        }


        [Test]
        public void GetLongestComonSubsequence_ReturnFoundSequence_CGTGTAGCT()
        {
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", a);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", b);
            string lcs = algorithms.GetLongestCommonSubsequence(seq1, seq2);
            Assert.That(lcs, Is.EqualTo(@"CGTGTAGCT"));
        }

        [Test]
        public void GetLongestComonSubsequence_ReturnFoundSequence_GTGGGTTTGTGTGTGGT()
        {
            string str1 = @"AAAAAAAGTGGGTTTGTGTGTGGTAAAAAA";
            string str2 = @"CCCGTGCCGGTTCTGCTGTCGCTCGGTCC";
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            string lcs = algorithms.GetLongestCommonSubsequence(seq1, seq2);
            Assert.That(lcs, Is.EqualTo(@"GTGGGTTTGTGTGTGGT"));
        }

        [Test]
        public void GetEditDistance_2()
        {
            string str1 = @"AAAAAAAAA";
            string str2 = @"AATAAAAGA";
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            int editDistance = algorithms.GetEditDistance(seq1,seq2);
            Assert.That(editDistance, Is.EqualTo(2));
        }

        [Test]
        public void GetEditDistance_10()
        {
            string str1 = @"AAAAAAAAAAAAAAAGTGTGTGTGTGTGTGTCCCCCCCCCCCCCCCC";
            string str2 = @"AAAAAAAAAAAAAGTGTGTGTGTGTGTCCCCCCCCCC";
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            int editDistance = algorithms.GetEditDistance(seq1, seq2);
            Assert.That(editDistance, Is.EqualTo(10));
        }

        [Test]
        public void GetEditDistance_15()
        {
            string str1 = @"AAAAAAAAAAAAAAAGTGTGTGTGTGTGTGTCCCCCCCCCCCCCCCC";
            string str2 = @"CCCAAAATTAAAAGTGTGTGTGTGTGTCCCCCCCCCCGGGGG";
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            int editDistance = algorithms.GetEditDistance(seq1, seq2);
            Assert.That(editDistance, Is.EqualTo(15));
        }

        [Test]
        public void GetNumberOfOptimalAlignments_IsTheNumberCorrect_3()
        {
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", a);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", b);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, costs), (x) => (x * -5));

            int numberOfOptimalAligments = algorithms.GetNumberOfOptimalAlignments();
            Assert.That(numberOfOptimalAligments, Is.EqualTo(3));
        }

        [Test]
        public void GetNumberOfOptimalAlignments_IsTheNumberCorrect_4()
        {
            string str1 = @"TCCAGAGA";
            string str2 = @"TCGAT";
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, costs), (x) => (x * -5));

            int numberOfOptimalAligments = algorithms.GetNumberOfOptimalAlignments();
            Assert.That(numberOfOptimalAligments, Is.EqualTo(4));
        }

        [Test]
        //[Ignore("")]
        public void GetNumberOfOptimalAlignments_IsTheNumberCorrect_256()
        {
            string str1 = System.IO.File.ReadAllText(@"E:\DOCUMENTS\Visual Studio 2017\Projects\Project1Bio\Testing\Seq1.txt");
            string str2 = System.IO.File.ReadAllText(@"E:\DOCUMENTS\Visual Studio 2017\Projects\Project1Bio\Testing\Seq2.txt");

            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, costs), (x) => (x * -5));

            int numberOfOptimalAligments = algorithms.GetNumberOfOptimalAlignments();
            Assert.That(numberOfOptimalAligments, Is.EqualTo(256));
        }

        [Test]
        //[Ignore("")]
        public void CountAlignmentScore_AreTheScoresCorrectForExamplesFovideInFilesApprox()
        {
            algorithms.SetLimitsForExactMultipleAlignment(1, 1); //to be sure approx wil be used

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
                int semiCost = 0;
                int[,] c = new int[,] { { semiCost, 5, 2, 5 }, { 5, semiCost, 5, 2 }, { 2, 5, semiCost, 5 }, { 5, 2, 5, semiCost } };


                parameters.CostArray = new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, c);
                parameters.CostArray.GapCostFun = CONSTCOST;
                parameters.Sequences = sequences;
                parameters.Comparefunction = MINFUN;

                algorithms.SetParameters(sequences, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, c), CONSTCOST);
                Alignment alignment = algorithms.GetOptimalAlignment();
                int score = algorithms.GetOptimalAlignmentScore();
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

        [Test]
        [Ignore("")]
        public void CountAlignmentScore_AreTheScoresCorrectForExamplesFovideInFilesExact()
        {
            algorithms.SetLimitsForExactMultipleAlignment(5, 500); //standard params

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
                int semiCost = 0;
                int [,] c = new int[,] { { semiCost, 5, 2, 5 }, { 5, semiCost, 5, 2 }, { 2, 5, semiCost, 5 }, { 5, 2, 5, semiCost } };


                parameters.CostArray = new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, c);
                parameters.CostArray.GapCostFun = CONSTCOST;
                parameters.Sequences = sequences;
                parameters.Comparefunction = MINFUN;


                algorithms.SetParameters(sequences, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, c), CONSTCOST);
                Alignment alignment = algorithms.GetOptimalAlignment();
                int score = algorithms.GetOptimalAlignmentScore();
                int scoreTest = testUtils.CountAlignmentScore(alignment, parameters);
                Assert.That(scoreTest, Is.EqualTo(score));
                countedScores.Add(score);
            }
            for (int i = 0; i < countedScores.Count(); ++i)
            {
                Assert.That(countedScores[i], Is.EqualTo(correctScores[i]));
            }

        }

        [Test]
        public void GetOptimalAlignmentScore_ReturnCountedAlignmentScore_61()
        {
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", a);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", b);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, costs),(x)=>(x*-5));
            int score = algorithms.GetOptimalAlignmentScore();
            Assert.That(score, Is.EqualTo(61));
        }

        [Test]
        public void GetOptimalAlignmentScore_ReturnCountedAlignmentScore_14()
        {
            string str1 = @"aataat".ToUpper();
            string str2 = @"aagg".ToUpper();
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, c), (x) => (x * 5));
            int score = algorithms.GetOptimalAlignmentScore();
            Assert.That(score, Is.EqualTo(14));
        }

        [Test]
        public void GetOptimalAlignmentScore_ReturnCountedAlignmentScore_395()
        {
            string str1 = @"ggcctaaaggcgccggtctttcgtaccccaaaatctcggcattttaagataagtgagtgttgcgttacactagcgatctaccgcgtcttatacttaagcgtatgcccagatctgactaatcgtgcccccggattagacgggcttgatgggaaagaacagctcgtctgtttacgtataaacagaatcgcctgggttcgc".ToUpper();
            string str2 = @"gggctaaaggttagggtctttcacactaaagagtggtgcgtatcgtggctaatgtaccgcttctggtatcgtggcttacggccagacctacaagtactagacctgagaactaatcttgtcgagccttccattgagggtaatgggagagaacatcgagtcagaagttattcttgtttacgtagaatcgcctgggtccgc".ToUpper();
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, c), (x) => (5 + x * 5));
            int score = algorithms.GetOptimalAlignmentScore();
            Assert.That(score, Is.EqualTo(395));
        }

        [Test]
        public void GetOptimalAlignmentScore_ReturnCountedAlignmentScore_22()
        {
            string str1 = @"acgtgtcaacgt".ToUpper();
            string str2 = @"acgtcgtagcta".ToUpper(); 
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);

            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, c), (x) => (x * 5));
            int score = algorithms.GetOptimalAlignmentScore();
            Assert.That(score, Is.EqualTo(22));
        }

        [Test]
        public void GetOptimalAlignmentScore_ReturnCountedAlignmentScore_7()
        {
            string str1 = @"CC";
            string str2 = @"ACCT";   
            int[,] c = new int[,] { { 0, 1, 1, 1 }, { 1, 0, 1, 1 }, { 1, 1, 0, 1 }, { 1, 1, 1, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, c), (x) => (4 + x));
            int score = algorithms.GetOptimalAlignmentScore();
            Assert.That(score, Is.EqualTo(7));
        }

        [Test]
        public void GetOptimalAlignmentScore_ReturnCountedAlignmentScore_24()
        {
            string str1 = @"acgtgtcaacgt".ToUpper();
            string str2 = @"acgtcgtagcta".ToUpper();
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, c), (x) => (5 + 5 * x));
            int score = algorithms.GetOptimalAlignmentScore();
            Assert.That(score, Is.EqualTo(24));
        }

        [Test]
        public void GetOptimalAlignmentScore_ReturnCountedAlignmentScore_325()
        {
            string str1 = @"ggcctaaaggcgccggtctttcgtaccccaaaatctcggcattttaagataagtgagtgttgcgttacactagcgatctaccgcgtcttatacttaagcgtatgcccagatctgactaatcgtgcccccggattagacgggcttgatgggaaagaacagctcgtctgtttacgtataaacagaatcgcctgggttcgc".ToUpper();
            string str2 = @"gggctaaaggttagggtctttcacactaaagagtggtgcgtatcgtggctaatgtaccgcttctggtatcgtggcttacggccagacctacaagtactagacctgagaactaatcttgtcgagccttccattgagggtaatgggagagaacatcgagtcagaagttattcttgtttacgtagaatcgcctgggtccgc".ToUpper();
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, c), (x) => (5 * x));
            int score = algorithms.GetOptimalAlignmentScore();
            Assert.That(score, Is.EqualTo(325));
        }

        [Test]
        public void GetOptimalAlignmentScore_ReturnCountedAlignmentScore_226()
        {
            string str1 = @"tatggagagaataaaagaactgagagatctaatgtcgcagtcccgcactcgcgagatactcactaagaccactgtggaccatatggccataatcaaaaag".ToUpper();
            string str2 = @"atggatgtcaatccgactctacttttcctaaaaattccagcgcaaaatgccataagcaccacattcccttatactggagatcctccatacagccatggaa".ToUpper();
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, c), (x) => (5 * x));
            int score = algorithms.GetOptimalAlignmentScore();
            Assert.That(score, Is.EqualTo(226));
        }

        [Test]
        public void GetOptimalAlignmentScoreWithAffineGapCost_266()
        {
            string str1 = @"tatggagagaataaaagaactgagagatctaatgtcgcagtcccgcactcgcgagatactcactaagaccactgtggaccatatggccataatcaaaaag".ToUpper();
            string str2 = @"atggatgtcaatccgactctacttttcctaaaaattccagcgcaaaatgccataagcaccacattcccttatactggagatcctccatacagccatggaa".ToUpper();
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, c), (x) => (5 + 5 * x));
            int score = algorithms.GetOptimalAlignmentScore();
            Assert.That(score, Is.EqualTo(266));
        }


        [Test]
        public void GetOptimalAlignment_ReturnFoundAlignment_isOneofOptimal()
        {
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", a);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", b);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(StringAlgorithms.Constants.DNA, costs), (x) => (x * -5));
            Alignment alignment = algorithms.GetOptimalAlignment();
            Assert.That(alignment.Sequences[0].Value, Is.EqualTo(@"-CGT-GTCAAGT-CT"));
            Assert.That(alignment.Sequences[1].Value, Is.EqualTo(@"ACGTCGT-AGCTAGG"));
        }

        [Test]
        public void GetOptimalAlignment_ReturnFoundAlignment_isOneofOptimal2()
        {
            string str1 = @"CC";
            string str2 = @"ACCT";
            int[,] c = new int[,] { { 0, 1, 1, 1 }, { 1, 0, 1, 1 }, { 1, 1, 0, 1 }, { 1, 1, 1, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(Constants.DNA, c), (x) => (5 * x));
            Alignment alignment = algorithms.GetOptimalAlignment();
            Assert.That(alignment.Sequences[0].Value, Is.EqualTo(@"-CC-"));
            Assert.That(alignment.Sequences[1].Value, Is.EqualTo(@"ACCT"));
        }

        [Test]
        public void GetOptimalAlignment_ReturnFoundAlignment_isOneofOptimal3()
        {
            string str1 = @"acgtgtcaacgt".ToUpper();
            string str2 = @"acgtcgtagcta".ToUpper();
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(Constants.DNA, c), (x) => (5 * x));
            Alignment alignment = algorithms.GetOptimalAlignment();
            Assert.That(alignment.Sequences[0].Value, Is.EqualTo(@"ACGT-GTCAACGT"));
            Assert.That(alignment.Sequences[1].Value, Is.EqualTo(@"ACGTCGT-AGCTA"));
        }

        [Test]
        public void GetOptimalAlignment_ReturnFoundAlignment_isOneofOptimal4()
        {
            string str1 = @"acgtgtcaacgt".ToUpper();
            string str2 = @"acgtcgtagcta".ToUpper(); 
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(Constants.DNA, c), (x) => (5 * x));
            Alignment alignment = algorithms.GetOptimalAlignment();
            Assert.That(alignment.Sequences[0].Value, Is.EqualTo(@"acgt-gtcaacgt".ToUpper()));
            Assert.That(alignment.Sequences[1].Value, Is.EqualTo(@"acgtcgt-agcta".ToUpper()));
        }

        [Test]
        public void GetOptimalAlignment_ReturnFoundAlignment_isOneofOptimal5()
        {
            string str1 = @"aataat".ToUpper();
            string str2 = @"aagg".ToUpper();
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(Constants.DNA, c), (x) => (5 * x));
            Alignment alignment = algorithms.GetOptimalAlignment();
            Assert.That(alignment.Sequences[0].Value, Is.EqualTo(@"AATAAT"));
            Assert.That(alignment.Sequences[1].Value, Is.EqualTo(@"AA-GG-"));
        }

        [Test]
        [Ignore("")]
        public void GetOptimalAlignment_ReturnFoundAlignment_isOneofOptimal6()
        {
            string str1 = @"ggcctaaaggcgccggtctttcgtaccccaaaatctcggcattttaagataagtgagtgttgcgttacactagcgatctaccgcgtcttatacttaagcgtatgcccagatctgactaatcgtgcccccggattagacgggcttgatgggaaagaacagctcgtctgtttacgtataaacagaatcgcctgggttcgc".ToUpper();
            string str2 = @"gggctaaaggttagggtctttcacactaaagagtggtgcgtatcgtggctaatgtaccgcttctggtatcgtggcttacggccagacctacaagtactagacctgagaactaatcttgtcgagccttccattgagggtaatgggagagaacatcgagtcagaagttattcttgtttacgtagaatcgcctgggtccgc".ToUpper();
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(Constants.DNA, c), (x) => (5 * x));
            Alignment alignment = algorithms.GetOptimalAlignment();
            Assert.That(alignment.Sequences[0].Value, Is.EqualTo(@"ggcctaaaggcgccggtctttcgtaccccaaaatctcg-gcattttaagataa-gtgagtgttgcgttacactagcgatctaccgcgtcttatact-taagcg-tatgccc-agatctga-ctaatcgtgcccccggattagacgggcttgatgggaaagaaca--g-ctc-g--tctgtttacgtataaacagaatcgcctgggttcgc".ToUpper()));
            Assert.That(alignment.Sequences[1].Value, Is.EqualTo(@"gggctaaaggttagggtctttcacactaaagagtggtgcgtatcgt-ggctaatgt-accgcttc-tggtatc-gtggctta-cg-gccagac-ctacaagtactagacctgagaactaatcttgtcgagccttc-catt-ga-ggg--taatgggagagaacatcgagtcagaagttattcttgtttacgtagaatcgcctgggtccgc".ToUpper()));
        }

        [Test]
        [Ignore("")]
        public void GetOptimalAlignment_ReturnFoundAlignment_isOneofOptimal7()
        {
            string str1 = @"ggcctaaaggcgccggtctttcgtaccccaaaatctcggcattttaagataagtgagtgttgcgttacactagcgatctaccgcgtcttatacttaagcgtatgcccagatctgactaatcgtgcccccggattagacgggcttgatgggaaagaacagctcgtctgtttacgtataaacagaatcgcctgggttcgc".ToUpper();
            string str2 = @"gggctaaaggttagggtctttcacactaaagagtggtgcgtatcgtggctaatgtaccgcttctggtatcgtggcttacggccagacctacaagtactagacctgagaactaatcttgtcgagccttccattgagggtaatgggagagaacatcgagtcagaagttattcttgtttacgtagaatcgcctgggtccgc".ToUpper();
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(Constants.DNA, c), (x) => (5 * x));
            Alignment alignment = algorithms.GetOptimalAlignment();
            Assert.That(alignment.Sequences[0].Value, Is.EqualTo(@"ggcctaaaggcgccggtctttcgtaccccaaaatctcg-gcattttaagataa-gtgagtgttgcgttacactagcgatctaccgcgtcttatact-taagcg-tatgccc-agatctga-ctaatcgtgcccccggattagacgggcttgatgggaaagaaca--g-ctc-g--tctgtttacgtataaacagaatcgcctgggttcgc".ToUpper()));
            Assert.That(alignment.Sequences[1].Value, Is.EqualTo(@"gggctaaaggttagggtctttcacactaaagagtggtgcgtatcgt-ggctaatgt-accgcttc-tggtatc-gtggctta-cg-gccagac-ctacaagtactagacctgagaactaatcttgtcgagccttc-catt-ga-ggg--taatgggagagaacatcgagtcagaagttattcttgtttacgtagaatcgcctgggtccgc".ToUpper()));
        }

        [Test]
        [Ignore("")]
        public void GetOptimalAlignment_ReturnFoundAlignment_isOneofOptimal8()
        {
            string str1 = @"tatggagagaataaaagaactgagagatctaatgtcgcagtcccgcactcgcgagatactcactaagaccactgtggaccatatggccataatcaaaaag".ToUpper();
            string str2 = @"atggatgtcaatccgactctacttttcctaaaaattccagcgcaaaatgccataagcaccacattcccttatactggagatcctccatacagccatggaa".ToUpper();
            int[,] c = new int[,] { { 0, 5, 2, 5 }, { 5, 0, 5, 2 }, { 2, 5, 0, 5 }, { 5, 2, 5, 0 } };
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            algorithms.SetParameters(new List<Sequence>() { seq1, seq2 }, new LetterAlignmentCostManager(Constants.DNA, c), (x) => (5 * x));
            Alignment alignment = algorithms.GetOptimalAlignment();
            Assert.That(alignment.Sequences[0].Value, Is.EqualTo(@"TATGGA-GAGAATAAAAGAACTGAGAGATCT-AATGTCGCAGTCCCGCAC-TCGCGAGATACT-CACTAAGAC-CACTGTGGACCATATGGCCATAATCAAAAAG".ToUpper()));
            Assert.That(alignment.Sequences[1].Value, Is.EqualTo(@"-ATGGATGTCAATCCGA-CTCTACTTTTCCTAAAAATTCCAGCGCAAAATGCCATAAG-CACCACATTCCCTTATACTGGAGATCCT-CCA-TACAGCCATGGAA".ToUpper()));
        }







    }
}
