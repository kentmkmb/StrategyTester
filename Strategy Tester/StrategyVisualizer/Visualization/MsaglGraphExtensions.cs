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
                var edge = graph.AddEdge(currentState.GetHashCode().ToString(), currentState.Next.GetHashCode().ToString());
                edge.Attr.LineWidth = 2;
                var currentNode = graph.FindNode(currentState.GetHashCode().ToString());
                currentNode.LabelText = currentState.ToString();
                if (currentState.Alternative != null)
                {
                    var altEdge = graph.AddEdge(currentState.GetHashCode().ToString(), currentState.Alternative.First.GetHashCode().ToString());
                    altEdge.Attr.Color = Color.Gray;
                    altEdge.Attr.LineWidth = 2;
                    graph.AddNodes(currentState.Alternative.First);
                }
                currentNode.Attr.Shape = Shape.Box;
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
