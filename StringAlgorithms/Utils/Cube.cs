using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms.Utils
{
    public struct Cube
    {
        public int rowIndex, columnIndex, depthIndex;
        public int value;

        public Cube(int rowIndex, int columnIndex, int depthIndex, int value = 0)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.depthIndex = depthIndex;
            this.value = value;

        }

        public Cube GetUpNeighbor()
        {
            return new Cube(rowIndex - 1, columnIndex, 0);
        }

        public Cube GetUpDiagonalNeighbor()
        {
            return new Cube(rowIndex - 1, columnIndex - 1,0);
        }

        public Cube GetLeftNeighbor()
        {
            return new Cube(rowIndex, columnIndex - 1,0);
        }

        public bool IsTopLeftCell()
        {
            if(this.rowIndex == 0 && this.columnIndex == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
