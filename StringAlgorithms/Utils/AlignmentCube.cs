using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms.Utils
{
    public class AlignmentCube : ArrayIterable
    {

        internal int[,,] array;

        public ArrayIterator GetIterator()
        {
            if(!IsInitialized())
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
    }

    internal class AlignmentCubeIterator : ArrayIterator
    {
        AlignmentCube alignmentCube;
        Cube activeCell;

        public AlignmentCubeIterator(AlignmentCube cube)
        {
            this.alignmentCube = cube;
            activeCell = new Cube(1, 0, 0,0);
        }


        public object Diagonal()
        {
            throw new NotImplementedException();
        }

        public object Down()
        {
            throw new NotImplementedException();
        }

        public object GetCurrentCell()
        {
            throw new NotImplementedException();
        }

        public bool HasDiagonal()
        {
            throw new NotImplementedException();
        }

        public bool HasDown()
        {
            throw new NotImplementedException();
        }

        public bool HasLeft()
        {
            throw new NotImplementedException();
        }

        public bool HasNext()
        {
            throw new NotImplementedException();
        }

        public bool HasPrevious()
        {
            throw new NotImplementedException();
        }

        public bool HasUp()
        {
            throw new NotImplementedException();
        }

        public object Left()
        {
            throw new NotImplementedException();
        }

        public object Next()
        {
            throw new NotImplementedException();
        }

        public object Previous()
        {
            throw new NotImplementedException();
        }

        public void SetToCell(Cube cell)
        {
            throw new NotImplementedException();
        }

        public object Up()
        {
            throw new NotImplementedException();
        }
    }
}
