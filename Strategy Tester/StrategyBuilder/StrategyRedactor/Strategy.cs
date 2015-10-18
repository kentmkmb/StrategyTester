using System;
using System.Collections.Generic;
using StrategyBuilder.Translation;

namespace StrategyBuilder
{
    public class Strategy : IStrategy
    {
        public State First { get; set; }
        private State current;
        private State last;
        private readonly List<State> history;
        private readonly ITranslator translator;

        public Strategy(ITranslator translator)
        {
            First = null;
            current = null;
            last = null;
            history = new List<State>();
            this.translator = translator;
        }

        public Strategy() : this(null) { }

        public Strategy StopAt(int x, int y, double angle)
        {
            var newItem = new StoppingAt(new PointD(x, y), (angle/180)*Math.PI);
            Connect(newItem);
            return this;
        }

        public Strategy MoveTo(int x, int y)
        {
            var newItem = new MovementTo(new PointD(x, y));
            Connect(newItem);
            return this;
        }

        public Strategy Else(Strategy planB)
        {
            var pointer = last;
            while (pointer.Alternative == null)
            {
                pointer.Alternative = planB;
                planB.First.Previous = pointer;
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
                last.Next = newItem;
                newItem.Previous = last;
                last = newItem;
            }
        }

        private State MakeDecision(Report report)
        {
            if (report.Success)
            {
                if (current is EndOfStrategy)
                    return current;
                current = current.Next;
                return current.Previous;
            }
            GoToPreviousState(1);
            var pointer = current.Previous;
            while (!(current is EndOfStrategy))
                if (pointer.Alternative == null) pointer = pointer.Next;
                else
                {
                    current = pointer.Alternative.First.Next;
                    return current.Previous;
                }
            return new EndOfStrategy();
        }

        public Tuple<List<LowLevelCommand>, State> GetNextState(Report report)
        {
            var decision = MakeDecision(report);
            if ((history.Count == 0) || (!(history[history.Count - 1] is EndOfStrategy)))
                history.Add(decision);
            var translation = decision.GetTranslation(translator, report);
            return new Tuple<List<LowLevelCommand>, State>(translation, decision);
        }

        public void GoToPreviousState(int number)
        {
            for (var i = 0; i < number; i++)
            {
                if (history.Count == 0) return;
                current = history[history.Count - 1];
                history.RemoveAt(history.Count - 1);
            }
        }
    }
}
