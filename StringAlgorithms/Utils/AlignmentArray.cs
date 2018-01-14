using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StringAlgorithms.MatrixExtensions;

namespace StringAlgorithms.Utils
{
    public class AlignmentArray: ArrayIterable
    {
        internal int[,] array;
        public int rowSize;
        public int columnSize;

        public AlignmentArray()
        {
            
        }

        public ArrayIterator GetIterator()
        {
            if (!IsInitialized())
            {
                throw new InvalidOperationException("Object has not been inicialized!");
            }
            return new AlignmentArrayIterator(this);
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

        public void Initialize(int lastRowIndex, int lastColumnIndex, ValueFun rowValueFun, ValueFun columnValueFun)
        {
            //because of index 0
            array = new int[lastRowIndex+1, lastColumnIndex+1];

            array.FillRowWithIntValue(0, rowValueFun);
            array.FillColumnWithIntValue(0, columnValueFun);
            this.rowSize = lastRowIndex;
            this.columnSize = lastColumnIndex;    
        }

        public void Initialize(int[,] matrix)
        {
            array = matrix;          
            this.rowSize = matrix.GetLength(0) - 1;
            this.columnSize = matrix.GetLength(1) - 1;
        }

        public int GetCellNumber(int row, int column)
        {
            if(!IsInitialized())
            {
                throw new InvalidOperationException("Object has not been inicialized!");
            }
            int cellNumber = (row-1)*(columnSize) + column;
            return cellNumber;
        }

        public int GetNumberOfCells()
        {
            if (!IsInitialized())
            {
                throw new InvalidOperationException("Object has not been inicialized!");
            }
            return (rowSize) * (columnSize);
        }

        public void SetCell(Cube cell)
        {
            array[cell.rowIndex, cell.columnIndex] = cell.value;
        }

        public Cube GetCell(int row, int column)
        {
            Cube cell = new Cube(row, column,0, array[row, column]);
            return cell;
        }

        public int GetCellValue(int row, int column)
        {
            return array[row, column];
        }

    }

    internal class AlignmentArrayIterator : ArrayIterator
    {
        AlignmentArray alignmentArray;
        Cube activeCell;

        public AlignmentArrayIterator(AlignmentArray array)
        {
            this.alignmentArray = array;
            activeCell = new Cube(1, 0, 0,0);
        }

        public object GetCurrentCell()
        {
            return activeCell;
        }

        public void SetToCell(Cube cell)
        {
            cell.value = alignmentArray.GetCellValue(cell.rowIndex, cell.columnIndex);
            activeCell = cell;
        }

        public bool HasNext()
        {
            int numberOfCells = alignmentArray.GetNumberOfCells();
            int currentCellNumber = alignmentArray.GetCellNumber(activeCell.rowIndex, activeCell.columnIndex);
            if(currentCellNumber < numberOfCells)
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

                activeColumn++;
                if(activeColumn > alignmentArray.columnSize)
                {
                    activeRow++;
                    activeColumn = 1;
                }
                activeCell.rowIndex = activeRow;
                activeCell.columnIndex = activeColumn;
                activeCell.value = alignmentArray.array[activeRow, activeColumn];
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

                activeColumn--;
                if (activeColumn == 0)
                {
                    activeRow--;
                    activeColumn = 1;
                }
                activeCell.rowIndex = activeRow;
                activeCell.columnIndex = activeColumn;
                activeCell.value = alignmentArray.array[activeRow, activeColumn];
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

                activeRow--;

                activeCell.rowIndex = activeRow;

                activeCell.value = alignmentArray.array[activeRow, activeColumn];
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

                activeColumn--;

                activeCell.columnIndex = activeColumn;

                activeCell.value = alignmentArray.array[activeRow, activeColumn];
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

                activeRow++;

                activeCell.rowIndex = activeRow;

                activeCell.value = alignmentArray.array[activeRow, activeColumn];
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

                activeRow--;
                activeColumn--;

                activeCell.rowIndex = activeRow;
                activeCell.columnIndex = activeColumn;

                activeCell.value = alignmentArray.array[activeRow, activeColumn];
                return activeCell;
            }
            else
            {
                throw new InvalidOperationException("Trying to get diagonal element while there is no more elements!!");
            }
        }
    }
}
