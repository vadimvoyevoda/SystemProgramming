using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsParallel_average
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = new int[short.MaxValue];

            Random r = new Random();
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = r.Next(1, 100);
            }

            var watch = Stopwatch.StartNew();
            int length = 10000;
            for (int i = 0; i < length; i++)
            {
                arr.Average();
            }
            Console.WriteLine("Simple average = {0}",watch.ElapsedMilliseconds);

            //AsParallel
            watch = Stopwatch.StartNew();
            for (int i = 0; i < length; i++)
            {
                arr.AsParallel().Average();
            }
            Console.WriteLine("AsParallel average = {0}", watch.ElapsedMilliseconds);
        }
    }
}
