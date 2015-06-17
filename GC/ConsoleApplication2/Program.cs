using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class MyClass
    {
        int a;

        public MyClass(int a)
        {
            this.a = a;
            Console.WriteLine("Create Element " + a);            
        }

        ~MyClass()
        {
            Console.WriteLine("Delete Element " + a);
        }
    }

    class Program
    {
        
        static void Main(string[] args)
        {
            TimerCallback tc = new TimerCallback(timerFunc);
            Timer timer = new Timer(tc,null,0,300);           

            //MyClass myObj = new MyClass(1);
            //myObj = null;            

           // myObj.ToString();
            Console.ReadKey();
        }

        static void timerFunc(object a)
        {
            Console.WriteLine("Timer run");
            GC.Collect();
        }
    }
}
