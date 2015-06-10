using System;
using System.Diagnostics;

namespace WaitForExit
{
    class Program
    {
        static void Main(string[] args)
        {
            Process myProcess = new Process();
            myProcess.StartInfo = new ProcessStartInfo("calc.exe");
            myProcess.Start();

            Console.WriteLine("Запущений процес: {0}", myProcess.ProcessName);

            myProcess.WaitForExit();

            Console.WriteLine("Процес завершився з кодом: {0}", myProcess.ExitCode);
            Console.WriteLine("Поточний процес має ім'я: {0}", Process.GetCurrentProcess().ProcessName);

        }
    }
}
