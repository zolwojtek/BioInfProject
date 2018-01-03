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
    public class TestUtilsTest
    {
        TextAlignmentParameters parameters;
        TestUtils testUtils;

        [OneTimeSetUp]
        public void SetUp()
        {
            testUtils = new TestUtils();

            parameters = new TextAlignmentParameters();
            int[,] costs = new int[,] { { 0, 2, 5, 2 }, { 2, 0, 2, 5 }, { 5, 2, 0, 2 }, { 2, 5, 2, 0 } };
            LetterAlignmentCostManager costArray = new LetterAlignmentCostManager(Constants.DNA,costs);
            parameters.CostArray = costArray;
            //parameters.CostArray.GapCostFun = x => x * 5;
        }

        [Test]
        public void CountAlignmentScore_DoesItGiveRightLinearScore_0()
        {
            parameters.CostArray.GapCostFun = x => x * 5;
            string str1 = "AA";
            string str2 = "AA";
            Sequence seq1 = new Sequence(Constants.DNA,"seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            Alignment alignment = new Alignment(seq1, seq2);
            int score = testUtils.CountAlignmentScore(alignment,parameters);
            Assert.That(score, Is.EqualTo(0));
        }

        [Test]
        public void CountAlignmentScore_DoesItGiveRightLinearScore_20()
        {
            parameters.CostArray.GapCostFun = x => x * 5;
            string str1 = "A-GT--C";
            string str2 = "--G-ATC";
            Sequence seq1 = new Sequence(Constants.ALIGNMENT_DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.ALIGNMENT_DNA, "seq2", str2);
            Alignment alignment = new Alignment(seq1, seq2);
            int score = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(score, Is.EqualTo(20));
        }

        [Test]
        public void CountAlignmentScore_DoesItGiveRightLinearScore_51()
        {
            parameters.CostArray.GapCostFun = x => x * 5;
            string str1 = "AA-C-GT";
            string str2 = "CA-G-A-";
            string str3 = "TA--TA-";
            Sequence seq1 = new Sequence(Constants.ALIGNMENT_DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.ALIGNMENT_DNA, "seq2", str2);
            Sequence seq3 = new Sequence(Constants.ALIGNMENT_DNA, "seq3", str3);
            Alignment alignment = new Alignment(new List<Sequence>() { seq1, seq2, seq3 });
            int score = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(score, Is.EqualTo(51));
        }

        [Test]
        public void CountAlignmentScore_DoesItGiveRightAffineScore_0()
        {
            parameters.CostArray.GapCostFun = x => 5 + x * 5;
            string str1 = "AA";
            string str2 = "AA";
            Sequence seq1 = new Sequence(Constants.DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.DNA, "seq2", str2);
            Alignment alignment = new Alignment(seq1, seq2);
            int score = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(score, Is.EqualTo(0));
        }

        [Test]
        public void CountAlignmentScore_DoesItGiveRightAffineScore_35()
        {
            parameters.CostArray.GapCostFun = x => 5 + x * 5;
            string str1 = "A-GT--C";
            string str2 = "--G-ATC";
            Sequence seq1 = new Sequence(Constants.ALIGNMENT_DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.ALIGNMENT_DNA, "seq2", str2);
            Alignment alignment = new Alignment(seq1, seq2);
            int score = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(score, Is.EqualTo(35));
        }

        [Test]
        public void CountAlignmentScore_DoesItGiveRightAffineScore_81()
        {
            parameters.CostArray.GapCostFun = x => 5 + x * 5;
            string str1 = "AA-C-GT";
            string str2 = "CA-G-A-";
            string str3 = "TA--TA-";
            Sequence seq1 = new Sequence(Constants.ALIGNMENT_DNA, "seq1", str1);
            Sequence seq2 = new Sequence(Constants.ALIGNMENT_DNA, "seq2", str2);
            Sequence seq3 = new Sequence(Constants.ALIGNMENT_DNA, "seq3", str3);
            Alignment alignment = new Alignment(new List<Sequence>() { seq1, seq2, seq3 });
            int score = testUtils.CountAlignmentScore(alignment, parameters);
            Assert.That(score, Is.EqualTo(81));
        }
    }
}
