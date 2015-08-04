using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StrategyBuilder;

namespace StrategyVisualizer
{
    public partial class StraregyVisualizer : Form
    {
        Microsoft.Msagl.GraphViewerGdi.GViewer viewer;
        Microsoft.Msagl.Drawing.Graph graph;
        Strategy strategy;

        public StraregyVisualizer(Strategy strategy)
        {
            viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            graph = new Microsoft.Msagl.Drawing.Graph("graph");
            var currentState = strategy.First;
            graph.AddNodes(currentState);
            viewer.ToolBarIsVisible = false;
            viewer.Graph = graph;
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            Controls.Add(viewer);
        }

        public void SelectNode(State state)
        {

        }

        public void UnselectNode(State state)
        {

        }
    }

    public static class MsaglGraphExtensions
    {
        public static void AddNodes(this Microsoft.Msagl.Drawing.Graph graph, State currentState)
        {
            while (currentState.Next != null)
            {
                graph.AddEdge(currentState.GetHashCode().ToString(), currentState.Next.GetHashCode().ToString());
                var currentNode = graph.FindNode(currentState.GetHashCode().ToString());
                currentNode.LabelText = currentState.ToString();
                if (currentState.Alternative != null)
                {
                    graph.AddEdge(currentState.GetHashCode().ToString(), currentState.Alternative.First.GetHashCode().ToString());
                    graph.AddNodes(currentState.Alternative.First);
                    currentNode.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
                }
                else currentNode.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Box;
                currentState = currentState.Next;
            }
            var lastNode = graph.FindNode(currentState.GetHashCode().ToString());
            lastNode.LabelText = currentState.ToString();
        }
    }
}
