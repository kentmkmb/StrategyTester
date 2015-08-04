//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WindowsFormsApplication2
//{

//    class MyStrategy
//    {
//        State first;        
//    }

//    class MyState
//    {
//        public string data;
//        public MyState Next;
//    }
//    class Builder
//    {
//        public MyState current;
//        public MyState first;

//        public Builder()
//        {
//            current = first = new MyState { data = "First state" };
//        }

//        public Builder Go(int x, int y)
//        {
//            var state = new MyState { data = x.ToString() };
//            current.Next = state;
//            current = state;
//            return this;
//        }

//        public Builder Go(int x, int y, out MyState name)
//        {
//            Go(x, y);
//            name = current;
//            return this;
//        }

//        public MyState Build() { return first; }
     

//        void MyMain()
//        {
//            var bld = new Builder { current = new MyState() };
//            MyState intermediate;

//            var strategy = new Builder().Go(10, 10).Go(20, 20, out intermediate).Go(30, 10).Build();

//            intermediate.Alternative = new Builder().Go(20, 30).Go(50, 60).Build();
//        }

//    }
//}
