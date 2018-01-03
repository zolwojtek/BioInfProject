using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringAlgorithms;

using System.Resources;
using System.Collections;

namespace BioStringAlgorithms.Tests
{
    [TestFixture]
    class GraphTest
    {
        Graph graph;
        string sequence;

        [OneTimeSetUp]
        public void SetUp()
        {
            sequence = "AA#C&GTT";
            graph = new Graph(sequence.Length, sequence);
        }

        [Test]
        public void FitInitSequenceTest_CreateCorrectPath()
        {
            graph = new Graph(sequence.Length, sequence);

            if (assertPath(sequence))
                Assert.Pass();
            Assert.Fail();

            
        }

        private bool assertPath(string sequence)
        {
            string path = "0" + sequence + "0";
            for (int i = 0; i < sequence.Length + 1; ++i)
            {
                if (graph.NeighborhoodList[i].Count() != 1)
                    return false;
                if (graph.Nodes[i].Character != path[i])
                    return false;
            }
            if (graph.NeighborhoodList[sequence.Length + 1].Count != 0)
                return false;
            if (graph.Nodes[sequence.Length + 1].Character != path[sequence.Length + 1])
                return false;
            return true;
        }

        [Test]
        public void AddNodeTest_AddingNodeAtTheEndOfGraphNodeList()
        {
            int numberOfNodes = graph.Nodes.Count();
            int newNodeIndex;
            newNodeIndex = graph.AddNode('@');

            Assert.That(newNodeIndex, Is.EqualTo(numberOfNodes));
            Assert.That(graph.Nodes[newNodeIndex].Character == '@');
        }

        [Test]
        public void GetNextCoreNodeIndexTest()
        {
            int nodeIndex = 2; //second 'A'
            int nextNodeIndex = graph.GetNextCoreNodeIndex(nodeIndex);
            Assert.That(graph.Nodes[nextNodeIndex].Character == '#');
        }

        [Test]
        public void SetOrChangeNextCoreNodeTest()
        {
            graph = new Graph(sequence.Length, sequence);
            int nodeIndex = 2; //second 'A'
            int newNodeIndex;
            newNodeIndex = graph.AddNode('@');
            graph.SetOrChangeNextCoreNode(nodeIndex, newNodeIndex);

            //CHANGE
            Assert.That(graph.GetNextCoreNodeIndex(nodeIndex), Is.EqualTo(newNodeIndex));
            Assert.That(graph.Nodes[graph.GetNextCoreNodeIndex(nodeIndex)].Character, Is.EqualTo('@'));

            //SET
            graph.SetOrChangeNextCoreNode(newNodeIndex, nodeIndex);
            Assert.That(graph.GetNextCoreNodeIndex(newNodeIndex), Is.EqualTo(nodeIndex));
            Assert.That(graph.Nodes[graph.GetNextCoreNodeIndex(newNodeIndex)].Character, Is.EqualTo('A'));
        }

        [Test]
        public void AddNodeAfterTest()
        {
            graph = new Graph(sequence.Length, sequence); 
            graph.AddNodeAfter(4, '*');//"0AA#C*&GTT0";
            int asterixIndex = graph.GetNextCoreNodeIndex(4);

            Assert.That(graph.Nodes[asterixIndex].Character == '*');

            int nextNodeIndex = graph.GetNextCoreNodeIndex(asterixIndex);

            Assert.That(graph.Nodes[nextNodeIndex].Character == '&');
        }

        [Test]
        public void FitInitSequenceTest()//"0AA#C&GTT0"
        {
            graph = new Graph(sequence.Length);
            graph.FitInitSequence(sequence);
            int nodeIdx = 0;
            string graphCore = ""; //should == "0" + sequence + "0"
            while(graph.NeighborhoodList[nodeIdx].Count() != 0)
            {
                graphCore += graph.Nodes[nodeIdx].Character;
                nodeIdx = graph.NeighborhoodList[nodeIdx][0];
            }
            graphCore += graph.Nodes[nodeIdx].Character;
            Assert.That(graphCore, Is.EqualTo("0"+sequence+"0"));
        }

        

    }
}
