using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semaphore_3App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Semaphore semaphore = new Semaphore(3, 3, "MySemaphore");

            if (semaphore.WaitOne())
            {
                try
                {
                    Application.Run(new Form1());                    
                }
                finally
                {
                    semaphore.Release();
                }
            }
            //else
            //{
            //    MessageBox.Show("Semaphore is Full");
            //}
        }
    }
}
