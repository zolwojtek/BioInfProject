using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms.Utils
{
    public struct Cell
    {
        public int rowIndex, columnIndex;
        public int value;

        public Cell(int rowIndex, int columnIndex, int value = 0)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.value = value;
            //CheckIfTheCellIsLegal();
        }

        private void CheckIfTheCellIsLegal()
        {
            if(rowIndex < 0 || columnIndex < 0)
            {
                throw new ArgumentException("Cell's coordinates must be >= 0!");
            }
        }

        public Cell GetUpNeighbor()
        {
            return new Cell(rowIndex - 1, columnIndex);
        }

        public Cell GetUpDiagonalNeighbor()
        {
            return new Cell(rowIndex - 1, columnIndex - 1);
        }

        public Cell GetLeftNeighbor()
        {
            return new Cell(rowIndex, columnIndex - 1);
        }
    }
}
