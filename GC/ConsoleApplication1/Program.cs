using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class BigObject
    {
        double[] mas = new double[100000];
        List<double> rrr = new List<double>();
        int a;

        public BigObject(double ar, int a)
        {
            //Console.WriteLine("Create Element " + a);
            this.a = a;

            for (int i = 0; i < a; i++)
            {
                rrr.Add(ar);
            }
        }

        ~BigObject()
        {
            //Console.WriteLine("Delete Element " + a);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            double arr = 1.2;
            List<BigObject> res = new List<BigObject>();

            try
            {
                for (int i = 0; i < 1000000; i++)
                {
                    BigObject bo = new BigObject(arr, i);
                    //res.Add(bo);
                }
            }
            catch (OutOfMemoryException exc)
            {
                Console.WriteLine(exc.Message);
            }
            finally
            {
                Console.WriteLine("0 gen: " + GC.CollectionCount(0));
                Console.WriteLine("1 gen: " + GC.CollectionCount(1));
                Console.WriteLine("2 gen: " + GC.CollectionCount(2));
            }
            
        }
    }
}
