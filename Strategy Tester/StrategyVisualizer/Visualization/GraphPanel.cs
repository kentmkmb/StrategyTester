using System.Windows.Forms;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using StrategyBuilder;

namespace StrategyVisualizer
{
    public class GraphPanel : UserControl
    {
        public readonly Graph Graph;
        private readonly GViewer viewer;
        private readonly Color selectColor = Color.Yellow;
        private Color memoryColor;

        public GraphPanel(State start)
        {
            viewer = new GViewer();
            Graph = new Graph("graph");
            Graph.AddNodes(start);
            viewer.ToolBarIsVisible = false;
            viewer.Graph = Graph;
            viewer.Dock = DockStyle.Fill;
            var magnificationCoeff = 560.0 / viewer.Height;
            Height = 560;
            Width = (int)(viewer.Width * magnificationCoeff);
            Controls.Add(viewer);
        }

        public void Repaint()
        {
            viewer.Invalidate();
        }

        public void SelectNode(State state)
        {
            var node = Graph.FindNode(state.GetHashCode().ToString());
            node.Attr.FillColor = selectColor;
        }

        public void UnselectNode(State state)
        {
            var node = Graph.FindNode(state.GetHashCode().ToString());
            node.Attr.FillColor = Color.White;
        }

        public void SelectEdge(State from, State to)
        {
            var edge = Graph.FindEdge(from, to);
            memoryColor = edge.Attr.Color;
            edge.Attr.Color = selectColor;
        }

        public void UnselectEdge(State from, State to)
        {
            var edge = Graph.FindEdge(from, to);
            edge.Attr.Color = memoryColor;
        }
    }
}
