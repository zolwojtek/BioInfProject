using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringAlgorithms;

namespace BioStringAlgorithms.Tests
{
    [TestFixture]
    class StringCostArrayTest
    {
        LetterAlignmentCostManager costArray;
        string signSet;
        int[,] costs;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.signSet = "AGCT";
            this.costs = new int[,] { { 10, 2, 5, 2 }, { 2, 10, 2, 5 }, { 5, 2, 10, 2 }, { 2, 5, 2, 10 } };
        }

        [Test]
        public void GetCost_WhatIsGivenCostForPairAA_10()
        {
            costArray = new LetterAlignmentCostManager(signSet, costs);
            int value = costArray.GetLettersAlignmentCost('A', 'A');
            Assert.That(value, Is.EqualTo(10));
        }
        [Test]
        public void GetCost_WhatIsGivenCostForPairAG_2()
        {
            costArray = new LetterAlignmentCostManager(signSet, costs);
            int value = costArray.GetLettersAlignmentCost('A', 'G');
            Assert.That(value, Is.EqualTo(2));
        }
        [Test]
        public void GetCost_WhatIsGivenCostForPairAC_5()
        {
            costArray = new LetterAlignmentCostManager(signSet, costs);
            int value = costArray.GetLettersAlignmentCost('A', 'C');
            Assert.That(value, Is.EqualTo(5));
        }
        [Test]
        public void GetCost_WhatIsGivenCostForPairAT_2()
        {
            costArray = new LetterAlignmentCostManager(signSet, costs);
            int value = costArray.GetLettersAlignmentCost('A', 'T');
            Assert.That(value, Is.EqualTo(2));
        }
        [Test]
        public void GetCost_WhatIsGivenCostForPairTT_10()
        {
            costArray = new LetterAlignmentCostManager(signSet, costs);
            int value = costArray.GetLettersAlignmentCost('T', 'T');
            Assert.That(value, Is.EqualTo(10));
        }
        [Test]
        public void GetCost_WhatIsGivenCostForPairTC_2()
        {
            costArray = new LetterAlignmentCostManager(signSet, costs);
            int value = costArray.GetLettersAlignmentCost('T', 'C');
            Assert.That(value, Is.EqualTo(2));
        }
        [Test]
        public void GetCost_WhatIsGivenCostForPairCT_2()
        {
            costArray = new LetterAlignmentCostManager(signSet, costs);
            int value = costArray.GetLettersAlignmentCost('C', 'T');
            Assert.That(value, Is.EqualTo(2));
        }

    }
}
