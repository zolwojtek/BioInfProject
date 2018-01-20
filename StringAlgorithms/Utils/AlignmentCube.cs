using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StringAlgorithms.MatrixExtensions;

namespace StringAlgorithms.Utils
{
    public class AlignmentCube: CubeIterable
    {
        internal int[,,] array;
        public int rowSize;
        public int columnSize;
        public int depthSize;

        public AlignmentCube()
        {
            
        }

        public CubeIterator GetIterator()
        {
            if (!IsInitialized())
            {
                throw new InvalidOperationException("Object has not been inicialized!");
            }
            return new AlignmentCubeIterator(this);
        }

        private bool IsInitialized()
        {
            if (array == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Initialize(int lastRowIndex, int lastColumnIndex, int lastDepthIndex)
        {
            //+1 because of index 0
            array = new int[lastRowIndex + 1, lastColumnIndex + 1, lastDepthIndex + 1];

            this.rowSize = lastRowIndex;
            this.columnSize = lastColumnIndex;
            this.depthSize = lastDepthIndex;
        }

        public void Initialize(int[,,] matrix)
        {
            array = matrix;          
            this.rowSize = matrix.GetLength(0) - 1;
            this.columnSize = matrix.GetLength(1) - 1;
            this.depthSize = matrix.GetLength(2) - 1;
        }

        public int GetCellNumber(int row, int column)
        {
            if (!IsInitialized())
            {
                throw new InvalidOperationException("Object has not been inicialized!");
            }
            int cellNumber = (row - 1) * (columnSize) + column;
            return cellNumber;
        }

        public int GetNumberOfCells()
        {
            if (!IsInitialized())
            {
                throw new InvalidOperationException("Object has not been inicialized!");
            }
            
            return (rowSize) * (columnSize) * ((depthSize==0)?1:depthSize);
        }

        public void SetCell(Cube cell)
        {
            array[cell.rowIndex, cell.columnIndex, cell.depthIndex] = cell.value;
        }

        public Cube GetCell(int row, int column, int depth)
        {
            Cube cell = new Cube(row, column, depth, array[row, column, depth]);
            return cell;
        }

        public int GetCellValue(int row, int column, int depth)
        {
            return array[row, column, depth];
        }

    }

    internal class AlignmentCubeIterator : CubeIterator
    {
        AlignmentCube alignmentArray;
        Cube activeCell;

        public AlignmentCubeIterator(AlignmentCube array)
        {
            this.alignmentArray = array;
            activeCell = new Cube(0, 0, 0, 0);
        }

        public object GetCurrentCell()
        {
            return activeCell;
        }

        public void SetToCell(Cube cell)
        {
            cell.value = alignmentArray.GetCellValue(cell.rowIndex, cell.columnIndex, cell.depthIndex);
            activeCell = cell;
        }

        public bool HasNext()
        {
            //int numberOfCells = alignmentArray.GetNumberOfCells();
            //int currentCellNumber = alignmentArray.GetCellNumber(activeCell.rowIndex, activeCell.columnIndex);

            //Może porówać z Cube new Cube(alignmentArray.rowSize, alignmentArray.ColumnSize, alignmentArray.DepthSize)?
            if(activeCell.rowIndex < alignmentArray.rowSize || activeCell.columnIndex < alignmentArray.columnSize || activeCell.depthIndex < alignmentArray.depthSize)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object Next()
        {
            if(HasNext())
            {
                int activeRow = activeCell.rowIndex;
                int activeColumn = activeCell.columnIndex;
                int activeDepth = activeCell.depthIndex;

                activeColumn++;
                if(activeColumn > alignmentArray.columnSize)
                {
                    activeRow++;
                    activeColumn = 0;
                }
                if(activeRow > alignmentArray.rowSize)
                {
                    activeDepth++;
                    activeRow = 0;
                }
                activeCell.rowIndex = activeRow;
                activeCell.columnIndex = activeColumn;
                activeCell.depthIndex = activeDepth;
                activeCell.value = alignmentArray.array[activeRow, activeColumn, activeDepth];
                return activeCell;
            }
            else
            {
                throw new InvalidOperationException("Trying to get next element while there is no more elements!!");
            }
        }

        public bool HasPrevious()
        {
            int currentCellNumber = alignmentArray.GetCellNumber(activeCell.rowIndex, activeCell.columnIndex);
            if (currentCellNumber > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object Previous()
        {
            if (HasPrevious())
            {
                int activeRow = activeCell.rowIndex;
                int activeColumn = activeCell.columnIndex;
                int activeDepth = activeCell.depthIndex;

                activeColumn--;
                if (activeColumn == 0)
                {
                    activeRow--;
                    activeColumn = 1;
                }
                activeCell.rowIndex = activeRow;
                activeCell.columnIndex = activeColumn;
                activeCell.value = alignmentArray.array[activeRow, activeColumn, activeDepth];
                return activeCell;
            }
            else
            {
                throw new InvalidOperationException("Trying to get previous element while there is no previous elements!!");
            }
        }

        public bool HasUp()
        {
            if (activeCell.rowIndex > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object Up()
        {
            if (HasUp())
            {
                int activeRow = activeCell.rowIndex;
                int activeColumn = activeCell.columnIndex;
                int activeDepth = activeCell.depthIndex;

                activeRow--;

                activeCell.rowIndex = activeRow;

                activeCell.value = alignmentArray.array[activeRow, activeColumn,activeDepth];
                return activeCell;
            }
            else
            {
                throw new InvalidOperationException("Trying to go up while there is no more elements!!");
            }
        }

        public bool HasLeft()
        {
            if (activeCell.columnIndex > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object Left()
        {
            if (HasLeft())
            {
                int activeRow = activeCell.rowIndex;
                int activeColumn = activeCell.columnIndex;
                int activeDepth = activeCell.depthIndex;

                activeColumn--;

                activeCell.columnIndex = activeColumn;

                activeCell.value = alignmentArray.array[activeRow, activeColumn,activeDepth];
                return activeCell;
            }
            else
            {
                throw new InvalidOperationException("Trying to get left element while there is no more elements!!");
            }
        }

        public bool HasDown()
        {
            if (activeCell.rowIndex < alignmentArray.rowSize)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object Down()
        {
            if (HasDown())
            {
                int activeRow = activeCell.rowIndex;
                int activeColumn = activeCell.columnIndex;
                int activeDepth = activeCell.depthIndex;

                activeRow++;

                activeCell.rowIndex = activeRow;

                activeCell.value = alignmentArray.array[activeRow, activeColumn,activeDepth];
                return activeCell;
            }
            else
            {
                throw new InvalidOperationException("Trying to go down element while there is no more elements!!");
            }
        }

        public bool HasDiagonal()
        {
            if (activeCell.rowIndex > 0 && activeCell.columnIndex > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object Diagonal()
        {
            if (HasDiagonal())
            {
                int activeRow = activeCell.rowIndex;
                int activeColumn = activeCell.columnIndex;
                int activeDepth = activeCell.depthIndex;

                activeRow--;
                activeColumn--;

                activeCell.rowIndex = activeRow;
                activeCell.columnIndex = activeColumn;

                activeCell.value = alignmentArray.array[activeRow, activeColumn,activeDepth];
                return activeCell;
            }
            else
            {
                throw new InvalidOperationException("Trying to get diagonal element while there is no more elements!!");
            }
        }
    }
}
