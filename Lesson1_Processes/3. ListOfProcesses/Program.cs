using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListOfProcesses
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Список процесів";
            var processes = Process.GetProcesses();

            Console.WriteLine("{0,-28} {1,-10}\n","Process Name", "Id");
            foreach (var el in processes)
            {
                Console.WriteLine("{0,-28} {1,-10}", el.ProcessName, el.Id);
            }
        }
    }
}
