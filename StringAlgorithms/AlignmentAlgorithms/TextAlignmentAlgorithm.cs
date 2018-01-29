using StringAlgorithms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace StringAlgorithms
{
    public abstract class TextAlignmentAlgorithm
    {
        protected Alignment computedAlignment;
        protected AlignmentCube array;
        protected StringBuilder[] alignmentStringsBuilders;
        protected TextAlignmentParameters parameters;
        public TextAlignmentParameters Parameters
        {
            get
            {
                return parameters;
            }
            set
            {
                parameters = value;
                SetAllComputableElementsToNull();
            }
        }
        protected int illegalValue;

        private void SetAllComputableElementsToNull()
        {
            array = null;
            alignmentStringsBuilders = null;
            computedAlignment = null;
        }
        
        public TextAlignmentAlgorithm(TextAlignmentParameters parameters)
        {
            this.parameters = parameters;
        }

        public virtual int GetOptimalAlignmentScore()
        {
            ComputeAlignmentArrayIfNecessary();
            Cube bestScoreCell = array.GetCell(array.rowSize, array.columnSize, array.depthSize);
            int optimalScore = bestScoreCell.value;
            return optimalScore;
        }

        protected virtual void ComputeAlignmentArrayIfNecessary()
        {
            if (this.array == null)
            {
                InitializeAlignmentArray();
                ComputeAlignmentArray();
            }
        }

        protected virtual void InitializeAlignmentArray()
        {
            List<int> dimensionSizes = parameters.GetDimensionSizes();
            array = new AlignmentCube();
            array.Initialize(dimensionSizes);
        }

        protected virtual void ComputeAlignmentArray()
        {
            
            CubeIterator alignmentArrayIterator = array.GetIterator();

            while (alignmentArrayIterator.HasNext())
            {
                Cube currentCell = (Cube)alignmentArrayIterator.Next();
                ComputeCell(currentCell); 
            }
        }

        protected virtual void ComputeCell(Cube cell)
        {
            List<int>[] directionsInCube = Utils.Utils.Variancy(parameters.GetNumberOfSequences());//clean up the mess with namespaces names TODO
            List<int> possibleScores = new List<int>();
            foreach (List<int> directionVector in directionsInCube)
            {
                int comingFrinNeighborScore = ComputeScore(cell, directionVector);
                possibleScores.Add(comingFrinNeighborScore);
            }
           cell.value = GetTheBestValue(possibleScores);
           array.SetCell(cell);
        }










        protected virtual int ComputeScore(Cube cell, List<int> directionVector)
        {
            int rowIndex = cell.rowIndex;
            int columnIndex = cell.columnIndex;
            int depthIndex = (directionVector.Count() == 2) ? 1 : cell.depthIndex;
            int yOffset = directionVector[0];
            int xOffset = directionVector[1];
            int zOffset = (directionVector.Count() == 2) ? 1 : directionVector[2];

            if (rowIndex - yOffset >= 0 && columnIndex - xOffset >= 0 && depthIndex - zOffset >= 0)
            {
                string A = parameters.Sequences[0].Value;
                string B = parameters.Sequences[1].Value;
                string C = (parameters.GetNumberOfSequences() > 2) ? parameters.Sequences[2].Value : "@";
                char a, b, c;
                a = FetchSign(A, rowIndex, yOffset);
                b = FetchSign(B, columnIndex, xOffset);
                c = FetchSign(C, depthIndex, zOffset);

                Cube newCell = new Cube(rowIndex - yOffset, columnIndex - xOffset, depthIndex - zOffset);
                return ComputeAligningValue(newCell, a, b, c);
            }
            return parameters.GetIlligalValue();
        }

        public virtual Alignment GetOptimalAlignment()
        {
            ComputeAlignmentArrayIfNecessary();
            RetrieveAlignmentIfNecessary();
            return computedAlignment;
        }

        protected void RetrieveAlignmentIfNecessary()
        {
            if (computedAlignment == null)//TODO: dodać przy zmanie parametrów, że null itd
            {
                InitializeSequencesOfAlignment();
                RetrieveAlignment();
            }
        }

        protected virtual void InitializeSequencesOfAlignment()
        {
            int seqNum = parameters.GetNumberOfSequences();
            alignmentStringsBuilders = new StringBuilder[seqNum];
            for(int i = 0; i < seqNum; ++i)
            {
                alignmentStringsBuilders[i] = new StringBuilder();
            }
           // secondSequenceOfAlignment = new StringBuilder("");
        }

        protected virtual void RetrieveAlignment()
        {
            Cube currentCell = array.GetCell(array.rowSize, array.columnSize, array.depthSize);

            while (currentCell.IsTopLeftCell() == false)
            {
                Cube newCell = GoOneStepBack(currentCell);

                currentCell = newCell;
            }
            MakeAlignment();
        }

        protected virtual Cube GoOneStepBack(Cube from)
        {
            string A = parameters.Sequences[0].Value;
            string B = parameters.Sequences[1].Value;
            string C = (parameters.GetNumberOfSequences()>2)?parameters.Sequences[2].Value : "@";
            List<int>[] possibleDirectionInArray = Utils.Utils.Variancy(parameters.GetNumberOfSequences());
            //possibleDirectionInArray[0] = new List<int>() { 1, 1 };
            //possibleDirectionInArray[1] = new List<int>() { 1, 0 };
            //possibleDirectionInArray[2] = new List<int>() { 0, 1 };

            int rowIndex = from.rowIndex;
            int columnIndex = from.columnIndex;
            int depthIndex = (parameters.GetNumberOfSequences() == 2) ? 1 : from.depthIndex;


            Cube newCell = new Cube();

            foreach (List<int> directionVector in possibleDirectionInArray)
            {
                int yOffset = directionVector[0];
                int xOffset = directionVector[1];
                int zOffset = (parameters.GetNumberOfSequences() == 2) ? 1 : directionVector[2];

                int comingFromNeighborCost = ComputeScore(from, directionVector);
                if (comingFromNeighborCost == from.value)
                {
                    char a, b, c;
                    a = FetchSign(A, rowIndex, yOffset);
                    b = FetchSign(B, columnIndex, xOffset);
                    c = FetchSign(C, depthIndex, zOffset);
                    AddNextSignsOfAlignment(a, b, c);
                    newCell = array.GetCell(rowIndex - yOffset, columnIndex - xOffset, depthIndex - zOffset);
                    break;
                }
            }
            return newCell;
        }

        protected virtual void AddNextSignsOfAlignment(char a, char b, char c)
        {
            alignmentStringsBuilders[0].Insert(0, a);
            alignmentStringsBuilders[1].Insert(0, b);
            if (parameters.GetNumberOfSequences() > 2)
            {
                alignmentStringsBuilders[2].Insert(0, c);
            }
        }

        protected virtual void MakeAlignment()
        {
            List<Sequence> sequences = new List<Sequence>();
            for (int i = 0; i < parameters.GetNumberOfSequences(); ++i)
            {
                Sequence firstAlignmentSeq = new Sequence(Constants.ALIGNMENT_DNA, parameters.Sequences[0].Name, alignmentStringsBuilders[i].ToString());
                sequences.Add(firstAlignmentSeq);
            }

            computedAlignment = new Alignment(sequences);
        }



        protected virtual int ComputeAligningValue(Cube cell, char a, char b, char c)
        {
            int score = array.GetCellValue(cell.rowIndex, cell.columnIndex, cell.depthIndex);
            score += parameters.CostArray.GetLettersAlignmentCost(a, b);
            score += parameters.CostArray.GetLettersAlignmentCost(b, c);
            score += parameters.CostArray.GetLettersAlignmentCost(a, c);
            return score;
        }

        protected virtual char FetchSign(string seq, int i, int iOffset)
        {
            if (iOffset == 0)
            {
                return '-';
            }
            return seq[i - iOffset];
        }

        public virtual int GetNumberOfOptimalSolutions()
        {
            ComputeAlignmentArrayIfNecessary();
            int optimalSolutionsNumber = 0;

            Cube startCell = array.GetCell(array.rowSize, array.columnSize, array.depthSize);

            optimalSolutionsNumber = CountNumberOfOptimalSolutions(startCell);
            return optimalSolutionsNumber;
        }


        protected virtual int CountNumberOfOptimalSolutions(Cube cell)
        {
            if (cell.IsTopLeftCell())
            {
                return 1;
            }

            int optimalSolutionsNumber = 0;
            List<int>[] possibleDirectionInArray = Utils.Utils.Variancy(parameters.GetNumberOfSequences());
            foreach (List<int> directionVector in possibleDirectionInArray)
            {
                int comingFromNeighborCost = ComputeScore(cell, directionVector);
                if (comingFromNeighborCost == cell.value)
                {
                    int rowIndex = cell.rowIndex - directionVector[0];
                    int columnIndex = cell.columnIndex - directionVector[1];
                    int depthIndex = (directionVector.Count() == 2) ? 0 : cell.depthIndex - directionVector[2];
                    Cube newCell = new Cube();
                    newCell = array.GetCell(rowIndex, columnIndex, depthIndex);
                    optimalSolutionsNumber += CountNumberOfOptimalSolutions(newCell);
                }
            }
            return optimalSolutionsNumber;
        }


        

        

        protected int GetTheBestValue(List<int> list)
        {
            int min = list.First();
            foreach (int value in list)
            {
                min = parameters.Comparefunction(min, value);
            }
            return min;
        }
    }
}
