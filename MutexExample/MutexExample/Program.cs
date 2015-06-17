using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MutexExample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        static bool isCreateNew;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (Mutex mutex = new Mutex(true, "MyMutex", out isCreateNew))
            {
                if (isCreateNew)
                {
                    Application.Run(new Form1());
                }
                else
                {
                    MessageBox.Show("I am already run");
                }
            }
        }
    }
}
