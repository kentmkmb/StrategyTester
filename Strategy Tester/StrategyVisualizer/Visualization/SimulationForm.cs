using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using StrategyBuilder;

namespace StrategyVisualizer
{
    public sealed partial class SimulationForm
    {
        private readonly ListView history;
        readonly IStrategy strategy;
        StrategyTesterReport currentReport;
        State currentState;
        List<State> stateHistory;
        List<Move> moveHistory;
        List<StrategyTesterReport> reportHistory;
        readonly World environment;
        bool mouseDown;
        Bitmap map;
        readonly Canvas picturePanel;
        readonly GraphPanel graphPanel;
        readonly SolidBrush objectsColor = new SolidBrush(Color.FromArgb(100, 255, 50, 50));

        public SimulationForm(IStrategy strategy, PointD startingPoint)
        {
            this.strategy = strategy;
            var startReport = new StrategyTesterReport(0, startingPoint, true);
            environment = new World(startReport.Coords, startReport.AngleInRadians);
            currentReport = startReport;
            moveHistory = new List<Move>();
            reportHistory = new List<StrategyTesterReport> { startReport };
            var startState = new Start(startingPoint) { Next = strategy.First };
            currentState = startState;
            stateHistory = new List<State> { startState };
            graphPanel = new GraphPanel(startState);
            graphPanel.SelectNode(startState);
            Width = Config.FieldWidth + graphPanel.Width + 150;
            Height = Config.FieldHeight+68;
            graphPanel.Location = new Point(Config.FieldWidth + 150, 0);
            DoubleBuffered = true;
            var sw = new Timer();
            var loadMap = new Button
            {
                Width = 100,
                Height = 30,
                Text = @"Load Map",
                Location = new Point(400, Height - 68)
            };
            picturePanel = new Canvas
            {
                Height = this.Height - 68,
                Width = this.Width - 150 - graphPanel.Width
            };
            var nextBut = new Button
            {
                Width = 100,
                Height = 30,
                Text = @"Next State",
                Location = new Point(0, Height - 68)
            };
            var fastNextBut = new Button
            {
                Width = 100,
                Height = 30,
                Text = @"Next Fast",
                Location = new Point(100, Height - 68)
            };
            var toEndBut = new Button
            {
                Width = 100,
                Height = 30,
                Text = @"To End Fast",
                Location = new Point(300, Height - 68)
            };
            var prevBut = new Button
            {
                Width = 100,
                Height = 30,
                Text = @"Previous State",
                Location = new Point(200, Height - 68)
            };
            history = new ListView
            {
                Height = this.Height - 40,
                Width = 150,
                Location = new Point(Width - 150 - graphPanel.Width, 0),
                View = View.List,
                MultiSelect = false
            };
            sw.Interval = 100;
            sw.Tick += (sender, e) => picturePanel.Invalidate();
            loadMap.Click += loadMap_Click;
            picturePanel.Paint += picturePanel_Paint;
            nextBut.Click += (sender, args) => MakeMoves(false, true);
            fastNextBut.Click += (sender, args) => MakeMoves(true, true);
            prevBut.Click += PrevBut_Click;
            toEndBut.Click +=toEndBut_Click;
            picturePanel.MouseDown += PicturePanel_MouseDown;
            picturePanel.MouseUp += PicturePanel_MouseUp;
            picturePanel.MouseMove += PicturePanel_MouseMove;
            picturePanel.MouseClick += picturePanel_MouseClick;
            history.ItemActivate += history_ItemActivate;
            history.Items.Add(startState.ToString());
            Controls.Add(picturePanel);
            Controls.Add(history);
            Controls.Add(nextBut);
            Controls.Add(fastNextBut);
            Controls.Add(toEndBut);
            Controls.Add(prevBut);
            Controls.Add(loadMap);
            Controls.Add(graphPanel);
            sw.Start();
        }

        void toEndBut_Click(object sender, EventArgs e)
        {
            while (!(currentState is EndOfStrategy))
                MakeMoves(true, false);
        }

        void picturePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) return;
            for (var i = 0; i < environment.Objects.Count; i++)
                if (environment.Objects[i].IsPointIn(e.Location)) environment.Objects.RemoveAt(i);
        }

        void loadMap_Click(object sender, EventArgs e)
        {
            var fDialog = new OpenFileDialog
            {
                Multiselect = false,
                CheckFileExists = true,
                Filter = @"Image Files(*.bmp; *.jpg; *.png)|*.bmp;*.jpg;*.png"
            };
            fDialog.ShowDialog();
            if (fDialog.FileName == "") return;
            var img = Image.FromFile(fDialog.FileName);
            map = new Bitmap(img, picturePanel.Width, picturePanel.Height);
        }

        void PicturePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown) return;
            var obj = environment.Objects.Last();
            obj.Size.Height = -obj.Coords.Y + e.Location.Y;
            obj.Size.Width = -obj.Coords.X + e.Location.X;
        }

        void PicturePanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) return;
            mouseDown = false;
        }

        void PicturePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) return;
            mouseDown = true;
            environment.Objects.Add(new Polygon(e.Location, new Size(0, 0)));
        }

        void picturePanel_Paint(object sender, PaintEventArgs e)
        {
            graphPanel.Repaint();
            var graphics = e.Graphics;
            graphics.Clear(Color.Beige);
            if (map != null) graphics.DrawImage(map, new Point(0, 0));
            for (var i = 0; i < picturePanel.Width; i += Config.GridSize)
            {
                graphics.DrawLine(new Pen(Brushes.Gray, 1), i, 0, i, picturePanel.Height);
                if (Config.AddNumbersToGrid)
                    graphics.DrawString(i.ToString(), new Font("arial", 8), Brushes.Black, new PointF(i, 0));
            }
            for (var i = 0; i < picturePanel.Height; i += Config.GridSize)
            {
                graphics.DrawLine(new Pen(Brushes.Gray, 1), 0, i, picturePanel.Width, i);
                if (Config.AddNumbersToGrid)
                    graphics.DrawString(i.ToString(), new Font("arial", 8), Brushes.Black, new PointF(0, i));
            } 
            foreach (var move in moveHistory)
            {
                graphics.DrawLine(new Pen(Brushes.Red, 5), (float)move.From.X, (float)move.From.Y, (float)move.To.X, (float)move.To.Y);
                graphics.FillPie(Brushes.Red, (float)move.To.X - 2.5f, (float)move.To.Y - 2.5f, 5, 5, 0, 360);
            }
            var robot = environment.OurRobot;
            graphics.TranslateTransform((float)robot.Coords.X, (float)robot.Coords.Y);
            graphics.RotateTransform((float)(-robot.Angle / Math.PI) * 180);
            graphics.TranslateTransform((float)-robot.Coords.X, (float)-robot.Coords.Y);
            graphics.DrawPolygon(
                new Pen(Brushes.Red, 2),
                new[]
                { 
                    new PointF((float)robot.Coords.X-5, (float)robot.Coords.Y+10),
                    new PointF((float)robot.Coords.X-5, (float)robot.Coords.Y-10),
                    new PointF((float)robot.Coords.X+5, (float)robot.Coords.Y)
                }
            );
            graphics.ResetTransform();
            if (environment.Objects.Count == 0) return;
            for (var i = 0; i < environment.Objects.Count - 1; i++)
                graphics.FillRectangle(objectsColor, new Rectangle(environment.Objects[i].Coords, environment.Objects[i].Size));
            if (mouseDown) graphics.DrawRectangle(new Pen(Brushes.Red, 3), new Rectangle(environment.Objects.Last().Coords, environment.Objects.Last().Size));
            else graphics.FillRectangle(objectsColor, new Rectangle(environment.Objects.Last().Coords, environment.Objects.Last().Size));
        }

        void PrevBut_Click(object sender, EventArgs e)
        {
            if (environment.Moving) return;
            if (history.Items.Count == 1) return;
            graphPanel.UnselectNode(currentState);
            history.Items.RemoveAt(history.Items.Count - 1);
            moveHistory.RemoveAt(moveHistory.Count - 1);
            reportHistory.RemoveAt(reportHistory.Count - 1);
            stateHistory.RemoveAt(stateHistory.Count - 1);
            strategy.GoToPreviousState(1);
            currentReport = reportHistory.Last();
            currentState = stateHistory.Last();
            environment.SetState(reportHistory.Last());
            graphPanel.SelectNode(currentState);
        }

        async void MakeMoves(bool doFast, bool async)
        {
            if (environment.Moving) return;
            var newAction = strategy.GetNextState(currentReport);
            if (currentState == newAction.Item2) return;
            graphPanel.UnselectNode(currentState);
            var movesList = newAction.Item1;
            var actionName = newAction.Item2.ToString();
            history.Items.Add(actionName);
            graphPanel.SelectEdge(currentState, newAction.Item2);
            StrategyTesterReport newState;
            if (async)
            {
                var task = Task<StrategyTesterReport>.Factory.StartNew(() => environment.TryMove(currentReport, movesList, doFast));
                newState = await task;
            }
            else newState = environment.TryMove(currentReport, movesList, doFast);
            graphPanel.UnselectEdge(currentState, newAction.Item2);
            if (newState.Success)
            {
                var lastMove = new Move {From = currentReport.Coords, To = newState.Coords};
                moveHistory.Add(lastMove);
                currentState = newAction.Item2;
                stateHistory.Add(newAction.Item2);
                reportHistory.Add(newState);
                currentReport = newState;
            }
            else
            {
                history.Items.RemoveAt(history.Items.Count - 1);
                currentReport.Success = false;
            }
            graphPanel.SelectNode(currentState);
        }

        void history_ItemActivate(object sender, EventArgs e)
        {
            if (environment.Moving) return;
            graphPanel.UnselectNode(currentState);
            var selectedItemIndex = history.SelectedIndices[0];
            var stepsBack = history.Items.Count - selectedItemIndex - 1;
            for (var i = history.Items.Count - 1; i > selectedItemIndex; i--)
                history.Items.RemoveAt(i);
            moveHistory = moveHistory.Take(selectedItemIndex).ToList();
            reportHistory = reportHistory.Take(selectedItemIndex + 1).ToList();
            stateHistory = stateHistory.Take(selectedItemIndex + 1).ToList();
            strategy.GoToPreviousState(stepsBack);
            currentState = stateHistory.Last();
            currentReport = reportHistory.Last();
            environment.SetState(reportHistory.Last());
            graphPanel.SelectNode(currentState);
        }
    }
}
