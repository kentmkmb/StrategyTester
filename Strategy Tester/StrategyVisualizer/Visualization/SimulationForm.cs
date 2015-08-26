﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using StrategyBuilder;

namespace StrategyVisualizer
{
    public sealed partial class SimulationForm
    {
        private readonly ListView history;
        readonly IStrategy strategy;
        Report currentReport;
        State currentState;
        List<State> stateHistory;
        List<Move> moveHistory;
        List<Report> reportHistory;
        readonly World environment;
        bool mouseDown;
        Bitmap map;
        readonly GraphPanel graphPanel;
        readonly SolidBrush objectsColor = new SolidBrush(Color.FromArgb(100, 255, 50, 50));

        public SimulationForm(IStrategy strategy, PointD startingPoint)
        {
            this.strategy = strategy;
            var startReport = new Report(0, startingPoint, true);
            environment = new World(startReport.Coords, startReport.AngleInRadians);
            currentReport = startReport;
            moveHistory = new List<Move>();
            reportHistory = new List<Report> { startReport };
            var startState = new Start(startingPoint) { Next = strategy.First };
            currentState = startState;
            stateHistory = new List<State> { startState };
            graphPanel = new GraphPanel(startState);
            graphPanel.SelectNode(startState);
            Width = 800 + graphPanel.Width;
            Height = 600;
            graphPanel.Location = new Point(800, 0);
            DoubleBuffered = true;
            var sw = new Timer();
            var loadMap = new Button
            {
                Width = 100,
                Height = 30,
                Text = @"Load Map",
                Location = new Point(200, Height - 68)
            };
            var picturePanel = new Canvas
            {
                Height = 530,
                Width = Width - 150 - graphPanel.Width
            };
            var nextBut = new Button
            {
                Width = 100,
                Height = 30,
                Text = @"Next State",
                Location = new Point(0, Height - 68)
            };
            var prevBut = new Button
            {
                Width = 100,
                Height = 30,
                Text = @"Previous State",
                Location = new Point(100, Height - 68)
            };
            history = new ListView
            {
                Height = 560,
                Width = 150,
                Location = new Point(Width - 150 - graphPanel.Width, 0),
                View = View.List,
                MultiSelect = false
            };
            sw.Interval = 100;
            sw.Tick += (sender, e) => picturePanel.Invalidate();
            loadMap.Click += loadMap_Click;
            picturePanel.Paint += picturePanel_Paint;
            nextBut.Click += NextBut_Click;
            prevBut.Click += PrevBut_Click;
            picturePanel.MouseDown += PicturePanel_MouseDown;
            picturePanel.MouseUp += PicturePanel_MouseUp;
            picturePanel.MouseMove += PicturePanel_MouseMove;
            picturePanel.MouseClick += picturePanel_MouseClick;
            history.ItemActivate += history_ItemActivate;
            history.Items.Add(startState.ToString());
            Controls.Add(picturePanel);
            Controls.Add(history);
            Controls.Add(nextBut);
            Controls.Add(prevBut);
            Controls.Add(loadMap);
            Controls.Add(graphPanel);
            sw.Start();
        }

        void picturePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) return;
            for (var i = 0; i < environment.Objects.Count; i++)
                if (environment.Objects[i].IsPointIn(e.Location)) environment.Objects.RemoveAt(i);
        }

        void loadMap_Click(object sender, EventArgs e)
        {
            var fDialog = new OpenFileDialog();
            fDialog.Multiselect = false;
            fDialog.CheckFileExists = true;
            fDialog.Filter = @"Image Files(*.bmp; *.jpg; *.png)|*.bmp;*.jpg;*.png";
            fDialog.ShowDialog();
            if (fDialog.FileName == "") return;
            var img = Image.FromFile(fDialog.FileName);
            map = new Bitmap(img, img.Width, img.Height);
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

        async void NextBut_Click(object sender, EventArgs e)
        {
            if (environment.Moving) return;
            var newAction = strategy.GetNextState(currentReport);
            if (currentState == newAction.Item2) return;
            graphPanel.UnselectNode(currentState);
            var movesList = newAction.Item1;
            var actionName = newAction.Item2.ToString();
            history.Items.Add(actionName);
            graphPanel.SelectEdge(currentState, newAction.Item2);
            var newState = await environment.TryMove(currentReport, movesList);
            graphPanel.UnselectEdge(currentState, newAction.Item2);
            if (newState.Success)
            {
                var lastMove = new Move { From = currentReport.Coords, To = newState.Coords };
                moveHistory.Add(lastMove);
                currentState = newAction.Item2;
                stateHistory.Add(newAction.Item2);
                reportHistory.Add(newState);
                currentReport = newState;
            }
            else currentReport.Success = false;
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
