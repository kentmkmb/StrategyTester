using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StrategyTester
{
    public class Move
    {
        public Point From;
        public Point To;
    }

    public partial class Form1 : Form
    {
        ListView history;
        IStrategy strategy;
        Report currentState;
        Move lastMove;
        List<Move> moveHistory;
        World environment;
        bool mouseDown;
        Bitmap map;

        public Form1(IStrategy strategy, Report start)
        {
            this.strategy = strategy;
            environment = new World(start.Coords, start.AngleInRadians);
            currentState = start;
            moveHistory = new List<Move>();
            lastMove = new Move { From = start.Coords, To = start.Coords };
            Width = 800;
            Height = 600;
            DoubleBuffered = true;
            var sw = new Timer();
            var loadMap = new Button
            {
                Width = 100,
                Height = 30,
                Text = "Load Map",
                Location = new Point(200, Height - 68)
            };
            var picturePanel = new Panel
            {
                Height = 530,
                Width = Width - 150
            };
            var nextBut = new Button
            {
                Width = 100,
                Height = 30,
                Text = "Next State",
                Location = new Point(0, Height - 68)
            };
            var prevBut = new Button
            {
                Width = 100,
                Height = 30,
                Text = "Previous State",
                Location = new Point(100, Height - 68)
            };
            history = new ListView
            {
                Height = 560,
                Width = 150,
                Location = new Point(Width - 150, 0),
                View = View.List,
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
            Controls.Add(picturePanel);
            Controls.Add(history);
            Controls.Add(nextBut);
            Controls.Add(prevBut);
            Controls.Add(loadMap);
            sw.Start();
        }

        void loadMap_Click(object sender, EventArgs e)
        {
            var fDialog = new OpenFileDialog();
            fDialog.Multiselect = false;
            fDialog.CheckFileExists = true;
            fDialog.Filter = "Image Files(*.bmp; *.jpg; *.png)|*.bmp;*.jpg;*.png";
            fDialog.ShowDialog();
            if (fDialog.FileName == "") return;
            var img = Image.FromFile(fDialog.FileName);
            map = new Bitmap(img, 650, 530);
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
            mouseDown = false;
        }

        void PicturePanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            environment.Objects.Add(new MyObject(e.Location, new Size(0, 0)));
        }

        void picturePanel_Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.Clear(Color.Beige);
            if (map != null) graphics.DrawImage(map, new Point(0, 0));
            foreach (var move in moveHistory)
            {
                graphics.DrawLine(new Pen(Brushes.Red, 5), move.From, move.To);
                graphics.FillPie(Brushes.Red, (float)move.To.X - 2.5f, (float)move.To.Y - 2.5f, 5, 5, 0, 360);
            }
            var robot = environment.OurRobot;
            graphics.TranslateTransform(robot.Coords.X, robot.Coords.Y);
            graphics.RotateTransform((float)(robot.Angle / Math.PI) * 180);
            graphics.TranslateTransform(-robot.Coords.X, -robot.Coords.Y);
            graphics.DrawPolygon(
                new Pen(Brushes.Red, 2),
                new Point[] 
                { 
                    new Point(robot.Coords.X-5, robot.Coords.Y+10),
                    new Point(robot.Coords.X-5, robot.Coords.Y-10),
                    new Point(robot.Coords.X+5, robot.Coords.Y)
                }
            );
            graphics.ResetTransform();
            if (environment.Objects.Count == 0) return;
            for (var i = 0; i < environment.Objects.Count - 1; i++)
                graphics.FillRectangle(Brushes.Red, new Rectangle(environment.Objects[i].Coords, environment.Objects[i].Size));
            if (mouseDown) graphics.DrawRectangle(new Pen(Brushes.Red, 3), new Rectangle(environment.Objects.Last().Coords, environment.Objects.Last().Size));
            else graphics.FillRectangle(Brushes.Red, new Rectangle(environment.Objects.Last().Coords, environment.Objects.Last().Size));
        }

        void PrevBut_Click(object sender, EventArgs e)
        {

        }

        async void NextBut_Click(object sender, EventArgs e)
        {
            if (environment.Moving) return;
            var desiredState = strategy.GetNextState(currentState);
            var newState = await environment.TryMove(currentState, desiredState);
            if (newState.Success)
            {
                lastMove = new Move { From = currentState.Coords, To = newState.Coords };
                moveHistory.Add(lastMove);
                currentState = newState;
            }
            else currentState.Success = false;
        }
    }
}
