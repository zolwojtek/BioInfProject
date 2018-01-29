using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringAlgorithms.Utils
{
    public interface CubeIterator
    {

        object GetCurrentCell();
        void SetToCell(Cube cell);

        bool HasNext();
        bool HasPrevious();
        bool HasUp();
        bool HasDown();
        bool HasLeft();
        bool HasDiagonal();

        object Next();
        object Previous();
        object Up();
        object Down();
        object Left();
        object Diagonal();
    }

    public interface CubeIterable
    {
        CubeIterator GetIterator();
    }
}
