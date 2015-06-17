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
            List<int[]> collection = new List<int[]> { new int[10000], new int[10000] };

            Task task = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    for (int j = 0; j < collection[i].Length; j++)
                    {
                        collection[i][j] = r.Next(1, 100);
                    }
                }

                var watch = Stopwatch.StartNew();
                for (int i = 0; i < collection.Count; i++)
                {
                    BubbleSort(collection[i]);
                }
                Console.WriteLine("Similar {0}", watch.ElapsedMilliseconds);
            });

            task.Wait();

        }

        public static void BubbleSort(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 1; j < arr.Length; j++)
                {
                    if (arr[i] < arr[j])
                    {
                        int temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
        }
    }
}
