using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1.Thread_ElementCollection_ToString
{
    class Program
    {
        static void Main(string[] args)
        {
            ParameterizedThreadStart threadstart = new ParameterizedThreadStart(ThreadFunc);
            Thread myThread = new Thread(threadstart);

            List<object> collection = new List<object>() {1, "Name", 12.5, "Bubble"};
            myThread.Start(collection);
        }

        static void ThreadFunc(object a)
        {
            foreach (var el in (a as List<object>))
            {
                Console.WriteLine(el.ToString());
            }
        }
    }
}
