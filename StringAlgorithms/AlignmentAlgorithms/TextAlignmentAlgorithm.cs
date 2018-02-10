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
                AddZDimensionIfNecessary(directionVector);
                int comingFriomNeighborScore = ComputeScore(cell, directionVector);
                possibleScores.Add(comingFriomNeighborScore);
            }
           cell.value = GetTheBestValue(possibleScores);
           array.SetCell(cell);
        }

        private void AddZDimensionIfNecessary(List<int> directionVector)
        {
            if(directionVector.Count() == 2)
            {
                directionVector.Add(0);
            }
        }

        private Cube GetCellPointedByVector(Cube origin, List<int> directionVector)
        {
            Cube newCell = new Cube
            {
                rowIndex = origin.rowIndex - directionVector[0],
                columnIndex = origin.columnIndex - directionVector[1],
                depthIndex = origin.depthIndex - directionVector[2]
            };
            //newCell.value = array.GetCellValue(newCell.rowIndex, newCell.columnIndex, newCell.depthIndex);
            return newCell;
        }

        protected virtual int ComputeScore(Cube cell, List<int> directionVector)
        { 
            Cube newCell = GetCellPointedByVector(cell, directionVector);

            if (newCell.IsCellCorrect())
            {
                List<char> signsToAlign = new List<char>();
                signsToAlign = GetSignsToAlign(cell, directionVector);

                return ComputeAligningValue(newCell, signsToAlign);
            }
            return parameters.GetIlligalValue();
        }

        private List<char> GetSignsToAlign(Cube cell, List<int> directionVector)
        {
            List<char> signsToAlign = new List<char>
            {
                FetchSign(parameters.GetSequenceValue(0), cell.rowIndex, directionVector[0]),
                FetchSign(parameters.GetSequenceValue(1), cell.columnIndex, directionVector[1])
            };
            if (parameters.GetNumberOfSequences() > 2)
            {
                signsToAlign.Add(FetchSign(parameters.GetSequenceValue(2), cell.depthIndex, directionVector[2]));
            }
            return signsToAlign;
        }

        protected virtual char FetchSign(string seq, int i, int iOffset)
        {
            if(seq == string.Empty)
            {
                return '\0';
            }
            if (iOffset == 0)
            {
                return '-';
            }
            return seq[i - iOffset];
        }

        protected virtual int ComputeAligningValue(Cube cell, List<char> signsToAlign)
        {
            int score = array.GetCellValue(cell.rowIndex, cell.columnIndex, cell.depthIndex);
            for(int  i = 0; i < signsToAlign.Count() - 1; ++i)
            {
                for(int j = i + 1; j < signsToAlign.Count(); ++j)
                {
                    score += parameters.CostArray.GetLettersAlignmentCost(signsToAlign[i], signsToAlign[j]);
                }
            }
            //score += parameters.CostArray.GetLettersAlignmentCost(a, b);
            //score += parameters.CostArray.GetLettersAlignmentCost(b, c);
            //score += parameters.CostArray.GetLettersAlignmentCost(a, c);
            return score;
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
        }

        protected virtual void RetrieveAlignment()
        {
            //we need to start from bottm right cell
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
            List<int>[] possibleDirectionInArray = Utils.Utils.Variancy(parameters.GetNumberOfSequences());
            //possibleDirectionInArray[0] = new List<int>() { 1, 1 };
            //possibleDirectionInArray[1] = new List<int>() { 1, 0 };
            //possibleDirectionInArray[2] = new List<int>() { 0, 1 };
            Cube newCell = new Cube();

            foreach (List<int> directionVector in possibleDirectionInArray)
            {
                AddZDimensionIfNecessary(directionVector);
                int comingFromNeighborCost = ComputeScore(from, directionVector);
                if (comingFromNeighborCost == from.value)
                {
                    List<char> signsToAlign = new List<char>();
                    signsToAlign = GetSignsToAlign(from, directionVector);
                    AddNextSignsOfAlignment(signsToAlign);
                    newCell = array.GetCell(GetCellPointedByVector(from, directionVector));
                    break;
                }
            }
            return newCell;
        }

        private void AddNextSignsOfAlignment(List<char> signsToAlign)
        {
            for(int i = 0; i < signsToAlign.Count(); ++i)
            {
                alignmentStringsBuilders[i].Insert(0, signsToAlign[i]);
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
