using StringAlgorithms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms
{
    public static class MatrixExtensions
    {
        public delegate int ValueFun(int x);

        public static int[,,] FillRowWithIntValue(this int[,,] matrix, int rowIdx, int depthIdx, ValueFun fun)
        {
            for (int i = 1; i < matrix.GetLength(1); ++i)
            {
                matrix[rowIdx, i, depthIdx] = fun(i);
            }
            return matrix;
        }

        public static int[,,] FillColumnWithIntValue(this int[,,] matrix, int columnIdx, int depthIdx, ValueFun fun)
        {
            for (int i = 1; i < matrix.GetLength(0); ++i)
            {
                matrix[i, columnIdx, depthIdx] = fun(i);
            }
            return matrix;
        }

        public static int[,] SetCell(this int[,] matrix, Cube cell, int value)
        {
            matrix[cell.rowIndex, cell.columnIndex] = value;
            return matrix;
        }

        //public static int GetCellValue(this int[,] matrix, Cell cell)
        //{
        //    int value = matrix[cell.rowIndex, cell.columnIndex];
        //    return value;
        //}

        //public static Cell GetCell(this int[,] matrix, int row, int column)
        //{
        //    int cellValue = matrix[row, column];
        //    Cell cell = new Cell(row, column,cellValue);
        //    return cell;
        //}
    }
}
