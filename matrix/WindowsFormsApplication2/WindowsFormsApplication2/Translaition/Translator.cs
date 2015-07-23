using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurobotStrategyCreator.Translaition
{
    public class Translator : ITranslator
    {
        struct RobotInfo
        {
            double LinearSpeed;
            double AngleSpeed;
        }
        private RobotInfo robotInfo

        public Translator(double linearSpeed, double angleSpeed)
        { 
            
        }

        public List<LowLevelCommand> Translate(Report current, MovementTo action)
        {
            throw new NotImplementedException();
        }
        public List<LowLevelCommand> Translate(Report current, StoppingAt action)
        {
            throw new NotImplementedException();
        }
    }
}
