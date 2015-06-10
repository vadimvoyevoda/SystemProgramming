using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _3.Game_PushKeyAfterSignal_ThreadTime
{
    class Program
    {
        static void Main(string[] args)
        {
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(Method);
            
            Console.WriteLine("You have 1 minute to press required key (Press any key)");
            Console.ReadKey();
            Console.Clear();

            int timeToSearch = 60000;
            int point = 0;

            while (true)
            {
                Thread myThread = new Thread(threadStart);                
                myThread.Start(new List<int>() {timeToSearch,point});
                while (myThread.IsAlive)
                {
                }
                ++point;
                if (timeToSearch > 2000)
                {
                    timeToSearch -= 2000;
                }
            }                     
        }
        
        public static void Method(object param)
        {
            List<int> Param = (List<int>) param;

            Random rand = new Random();
            int letter = rand.Next(97, 123);
            bool isSuccess = true;

            Console.WriteLine("You have {1} sec, Enter key: {0}", (char)letter, Param[0]/1000);
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            
            Stopwatch time = new Stopwatch();
            time.Start();
            while ((int)key.KeyChar != letter)
            {
                if (time.ElapsedMilliseconds > Param[0])
                {
                    isSuccess = false;
                    break;
                }

                if (Console.KeyAvailable)
                {
                    key = Console.ReadKey();
                }
            }
            time.Stop();
            if (isSuccess)
            {
                Console.WriteLine("\nTime : {0} ms\nPoints: {1}\n\n", time.ElapsedMilliseconds,++Param[1]);
            }
            else
            {
                Console.WriteLine("\nTime out (\nYour Result: {0}",Param[1]);
                Environment.Exit(0);
            }

        }
    }
}
