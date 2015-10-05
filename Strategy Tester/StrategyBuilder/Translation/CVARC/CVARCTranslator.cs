using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Translation.CVARC
{
    public class CVARCTranslator : ITranslator
    {
        public List<LowLevelCommand> Translate(Report current, MovementTo action)
        {
            var report = current as CVARCReport;
            if (report == null)
                throw new ArgumentException();
            var result = new List<LowLevelCommand>();
            return null;
        }

        public List<LowLevelCommand> Translate(Report current, StoppingAt action)
        {
            var report = current as CVARCReport;
            if (report == null)
                throw new ArgumentException();
            var result = new List<LowLevelCommand>();
            return null;
        }

        public List<LowLevelCommand> Translate(Report current, EndOfStrategy action)
        {
            return null;
        }
    }
}
