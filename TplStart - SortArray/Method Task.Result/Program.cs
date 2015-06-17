using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TplStart___SortArray
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            int[] collection = new int[30];

            for (int j = 0; j < collection.Length; j++)
            {
                collection[j] = r.Next(1, 100);
            }
           
            Task<int[]> task = new Task<int[]>(() =>
            {
                return BubbleSort(collection); 
            });

            task.Start();
            var res = task.Result;

            Console.WriteLine("SortedArray: ");
            foreach (var el in res)
            {
                Console.Write("{0} ", el);
            }
            //Parallel.ForEach(res, c =>
            //    {
            //        Console.Write("{0} ", c);
            //    });

        }

        public static int[] BubbleSort(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i+1; j < arr.Length; j++)
                {
                    if (arr[i] > arr[j])
                    {
                        int temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
            return arr;
        }
    }
}
