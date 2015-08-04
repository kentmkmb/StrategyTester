//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WindowsFormsApplication2
//{
//    public interface IVisitor
//    {
//        void Visit(A a);
//        //void Visit(B b);
//    }

//    public interface LowLevelCommand
//    {
//        Tuple<Report, string> Process(Report location);
//    }

//    public class Translator : IVisitor
//    {
//        List<LowLevelCommand> list;
//        Report location;
//        public void Visit(A a)
//        {
//            //list.Add(...);
//        }
//    }

//    public interface IVisitableState
//    {
//        void AcceptVisitor(IVisitor visitor);
//    }

//    class A : IVisitableState
//    {
//        public void AcceptVisitor(IVisitor visitor)
//        {
//            visitor.Visit(this);
//        }
//    }
//}
