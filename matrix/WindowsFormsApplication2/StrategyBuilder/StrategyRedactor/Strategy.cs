using System;
using System.Collections.Generic;
using System.Drawing;

namespace StrategyTester
{
    public class Strategy : IStrategy
    {
        public State First;
        private State current;
        private State last;
        private List<State> history;
        private ITranslator translator;

        public Strategy(ITranslator translator)
        {
            this.First = null;
            this.current = null;
            this.last = null;
            this.history = new List<State>();
            this.translator = translator;
        }
        public Strategy() : this(null) { }

        public Strategy StopAt(int x, int y, double angle)
        {
            var newItem = new StoppingAt(angle, new MyPoint(x, y));
            Connect(newItem);
            return this;
        }
        public Strategy MoveTo(int x, int y)
        {
            var newItem = new MovementTo(new MyPoint(x, y));
            Connect(newItem);
            return this;
        }
        public Strategy Else(Strategy planB)
        {
            State pointer = this.last;
            while (pointer == null)
            {
                pointer.Alternative = planB;
                if (pointer.Previous != null) pointer = pointer.Previous;
                else break;
            }
            return this;
        }
        public Strategy End()
        {
            Connect(new EndOfStrategy());
            return this;
        }

        private void Connect(State newItem)
        { 
            if (First == null)
            {
                First = current = last = newItem;
            }
            else 
            {
                this.last.Next = newItem;
                newItem.Previous = this.last;
                this.last = newItem;
            }
        }

        private State MakeDecision(Report report)
        {
            if (report.Success)
            {
                if (current is EndOfStrategy)
                {
                    return current;
                }
                current = current.Next;
                return current.Previous;
            }
            else
            {
                State pointer = current.Previous;
                while (!(current is EndOfStrategy))
                {
                    if (pointer.Alternative == null) pointer = pointer.Next;
                    else
                    {
                        return current = pointer.Alternative.First;
                    }
                }
                return new EndOfStrategy();
            }   
        }
        public Tuple<List<LowLevelCommand>, State> GetNextState(Report report)
        {
            State decision = MakeDecision(report);
            history.Add(decision);
            var translation = decision.GetTranslation(translator, report);
            return new Tuple<List<LowLevelCommand>, State>(translation, decision);
        }

        public void GoToPreviousState(int number)
        {
            for (int i = 0; i < number; i++)
            {
                if (history.Count == 0) return;
                current = history[history.Count - 1];
                history.RemoveAt(history.Count - 1);
            }
        }
    }
}
