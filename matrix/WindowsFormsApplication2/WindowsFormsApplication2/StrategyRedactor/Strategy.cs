using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurobotStrategyCreator
{
    public class Strategy : IStrategy
    {
        public State First;
        private State current;
        private State last;

        public Strategy()
        {
            First = null;
            current = null;
            last = null;
        }

        public Strategy StopAt(int x, int y, double angle)
        {
            var newItem = new StoppingAt(angle, new Point(x, y));
            Connect(newItem);
            return this;
        }
        public Strategy MoveTo(int x, int y)
        {
            var newItem = new MovementTo(new Point(x, y));
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
        public State GetNextState(Report report)
        {
            State decision = MakeDecision(report);
            return decision;
        }
    }
}
