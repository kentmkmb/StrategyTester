﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrategyBuilder;
using System.Windows.Forms;

namespace StrategyVisualizer
{
    public class Start : State
    {
        public PointD Coords;

        public Start(PointD coords)
        {
            Coords = coords;
        }

        public override string ToString()
        {
            return Coords.ToString();
        }

        public override List<LowLevelCommand> GetTranslation(ITranslator translator, Report current)
        {
            throw new NotImplementedException();
        }
    }

    public partial class GraphPanel : UserControl
    {
        Microsoft.Msagl.GraphViewerGdi.GViewer viewer;
        Microsoft.Msagl.Drawing.Graph graph;

        public GraphPanel(State start)
        {
            viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            graph = new Microsoft.Msagl.Drawing.Graph("graph");
            graph.AddNodes(start);
            viewer.ToolBarIsVisible = false;
            viewer.Graph = graph;
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            var magnificationCoeff = 560.0 / viewer.Height;
            Height = 560;
            Width = (int)(viewer.Width * magnificationCoeff);
            Controls.Add(viewer);
        }

        public void SelectNode(State state)
        {

        }

        public void UnselectNode(State state)
        {

        }

        public void SelectEdge(State from, State to)
        {

        }

        public void UnselectEdge(State from, State to)
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
