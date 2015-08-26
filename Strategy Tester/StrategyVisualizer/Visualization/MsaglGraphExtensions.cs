using System.Linq;
using Microsoft.Msagl.Drawing;
using StrategyBuilder;

namespace StrategyVisualizer
{
    public static class MsaglGraphExtensions
    {
        public static void AddNodes(this Graph graph, State currentState)
        {
            while (currentState.Next != null)
            {
                if (graph.FindEdge(currentState, currentState.Next) != null)
                {
                    currentState = currentState.Next;
                    continue;
                }
                graph.AddEdge(currentState.GetHashCode().ToString(), currentState.Next.GetHashCode().ToString());
                var currentNode = graph.FindNode(currentState.GetHashCode().ToString());
                currentNode.LabelText = currentState.ToString();
                if (currentState.Alternative != null)
                {
                    graph.AddEdge(currentState.GetHashCode().ToString(), currentState.Alternative.First.GetHashCode().ToString());
                    graph.AddNodes(currentState.Alternative.First);
                    currentNode.Attr.Shape = Shape.Diamond;
                }
                else currentNode.Attr.Shape = Shape.Box;
                currentState = currentState.Next;
            }
            var lastNode = graph.FindNode(currentState.GetHashCode().ToString());
            lastNode.LabelText = currentState.ToString();
        }

        public static Edge FindEdge(this Graph graph, State source, State target)
        {
            var nodeFrom = graph.FindNode(source.GetHashCode().ToString());
            var nodeTo = graph.FindNode(target.GetHashCode().ToString());
            return graph.Edges.FirstOrDefault(x => x.SourceNode == nodeFrom && x.TargetNode == nodeTo);
        }
    }
}
