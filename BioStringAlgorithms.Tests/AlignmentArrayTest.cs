﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringAlgorithms.Utils;
namespace BioStringAlgorithms.Tests
{
    [TestFixture]
    class AlignmentArrayTest
    {
        [Test]
        public void GetIterator_GetAllElementsFromArray_ReturnsElementsRowByRow()
        {
            int[,,] fakeArray = new int[,,] { { { 0 }, { 0 }, { 0 }, { 0 } }, { { 0 }, { 1 }, { 2 }, { 3 } }, { { 0 }, { 4 }, { 5 }, { 6 } }, { { 0 }, { 7 }, { 8 }, { 9 } } };
            List<int> returnedValues = new List<int>();
            List<int> correctValues = new List<int>() { 0, 0, 0, 0, 0, 1, 2, 3, 0, 4, 5, 6, 0, 7, 8, 9 };
            AlignmentCube alignmentArray = new AlignmentCube();
            alignmentArray.Initialize(fakeArray);

            CubeIterator arrayIterator = alignmentArray.GetIterator();

            returnedValues.Add(((Cube)arrayIterator.GetCurrentCell()).value);
            while(arrayIterator.HasNext())
            {
                Cube returnedCell = (Cube)arrayIterator.Next();
                returnedValues.Add(returnedCell.value);
            }

            CollectionAssert.AreEqual(returnedValues, correctValues);

        }

        [Test]
        public void GetIterator_GetAllElementsFromCube_ReturnsElementsRowByRow()
        {
            int[,,] fakeArray = new int[,,] { { { 0, 1 }, { 0, 2 }, { 0, 3 }, { 0, 4 } }, { { 0, 5 }, { 1, 6 }, { 2, 7 }, { 3, 8 } }, { { 0, 9 }, { 4, 10 }, { 5, 11 }, { 6, 12 } }, { { 0, 13 }, { 7, 14 }, { 8, 15 }, { 9, 16 } } };
            List<int> returnedValues = new List<int>();
            List<int> correctValues = new List<int>() { 0, 0, 0, 0, 0, 1, 2, 3, 0, 4, 5, 6, 0, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            AlignmentCube alignmentArray = new AlignmentCube();
            alignmentArray.Initialize(fakeArray);

            CubeIterator arrayIterator = alignmentArray.GetIterator();

            returnedValues.Add(((Cube)arrayIterator.GetCurrentCell()).value);
            while (arrayIterator.HasNext())
            {
                Cube returnedCell = (Cube)arrayIterator.Next();
                returnedValues.Add(returnedCell.value);
            }

            CollectionAssert.AreEqual(returnedValues, correctValues);

        }

        [Test]
        public void GetNumberOfCells_2x4matrix_correctNumber()
        {
            int[,,] fakeArray = new int[,,] {  { {0}, { 0 }, { 0 }, { 0 }, { 0 } }, { { 0 }, { 1 }, { 2 }, { 3 }, { 4 } }, { { 0 }, { 5 }, { 6 }, { 7 }, { 8 } } };

            AlignmentCube alignmentArray = new AlignmentCube();
            alignmentArray.Initialize(fakeArray);

            int numberOfCells = alignmentArray.GetNumberOfCells();

            Assert.That(numberOfCells, Is.EqualTo(8));
        }

        [Test]
        public void GetCellNumber_firstCell_correctNumber()
        {
            int[,,] fakeArray = new int[,,] { { { 0 }, { 0 }, { 0 }, { 0 }, { 0 } }, { { 0 }, { 1 }, { 2 }, { 3 }, { 4 } }, { { 0 }, { 5 }, { 6 }, { 7 }, { 8 } } };

            AlignmentCube alignmentArray = new AlignmentCube();
            alignmentArray.Initialize(fakeArray);

            int cellNumber = alignmentArray.GetCellNumber(1, 1);

            Assert.That(cellNumber, Is.EqualTo(1));
        }

        [Test]
        public void GetCellNumber_lastCell_correctNumber()
        {
            int[,,] fakeArray = new int[,,] { { { 0 }, { 0 }, { 0 }, { 0 }, { 0 } }, { { 0 }, { 1 }, { 2 }, { 3 }, { 4 } }, { { 0 }, { 5 }, { 6 }, { 7 }, { 8 } } };

            AlignmentCube alignmentArray = new AlignmentCube();
            alignmentArray.Initialize(fakeArray);

            int cellNumber = alignmentArray.GetCellNumber(2, 4);

            Assert.That(cellNumber, Is.EqualTo(8));
        }



        [Test]
        public void GetIterator_GetNextElementWhenEmpty_Throws()
        {
            int[,,] fakeArray = new int[,,] { { { 0 }, { 0 } }, { { 0 }, { 0 } }  };
            AlignmentCube alignmentArray = new AlignmentCube();
            alignmentArray.Initialize(fakeArray);

            CubeIterator iterator = alignmentArray.GetIterator();
            iterator.SetToCell(new Cube(1, 1, 0));

            var ex = Assert.Throws<InvalidOperationException>(() => iterator.Next());
            StringAssert.Contains("no more elements", ex.Message);
        }

        [Test]
        public void GetIterator_GetIteratorWhenNotInitialized_Throws()
        {
            AlignmentCube alignmentArray = new AlignmentCube();

            var ex = Assert.Throws<InvalidOperationException>(() => alignmentArray.GetIterator());
            StringAssert.Contains("not been inicialized", ex.Message);
        }



        [Test]
        public void GetIterator_GetPreviousElementWhenEmpty_Throws()
        {
            int[,,] fakeArray = new int[,,] { { { 0 }, { 0 } }, { { 0 }, { 0 } } };
            AlignmentCube alignmentArray = new AlignmentCube();
            alignmentArray.Initialize(fakeArray);

            CubeIterator iterator = alignmentArray.GetIterator();
            iterator.Next();

            var ex = Assert.Throws<InvalidOperationException>(() => iterator.Previous());
            StringAssert.Contains("no previous elements", ex.Message);
        }

        [Test]
        public void GetIterator_GetUpElementWhenEmpty_Throws()
        {
            int[,,] fakeArray = new int[,,] { { { 0 }, { 0 } }, { { 0 }, { 0 } } };
            AlignmentCube alignmentArray = new AlignmentCube();
            alignmentArray.Initialize(fakeArray);

            CubeIterator iterator = alignmentArray.GetIterator();
            

            var ex = Assert.Throws<InvalidOperationException>(() => iterator.Up());
            StringAssert.Contains("no more elements", ex.Message);
        }

        [Test]
        public void GetIterator_GetLegalUpElement_correctCell()
        {
            int[,,] fakeArray = new int[,,] { { { 0 }, { 0 }, { 0 }, { 0 }, { 0 } }, { { 0 }, { 1 }, { 2 }, { 3 }, { 4 } }, { { 0 }, { 5 }, { 6 }, { 7 }, { 8 } } };

            AlignmentCube alignmentArray = new AlignmentCube();
            alignmentArray.Initialize(fakeArray);

            CubeIterator iterator = alignmentArray.GetIterator();
            iterator.SetToCell(new Cube(alignmentArray.rowSize, alignmentArray.columnSize,0));
            Cube cell = (Cube)iterator.Up();
            

            Assert.That(cell.rowIndex, Is.EqualTo(1));
            Assert.That(cell.columnIndex, Is.EqualTo(4));
        }
    }
}
