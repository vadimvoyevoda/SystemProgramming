using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlPrj
{
    static class Program
    {
        static AppDomain DataForCalculate;
        static AppDomain FactorialRes;

        static Assembly DataForCalculateAsm;
        static Assembly FactorialResAsm;

        static Form DataForCalculateWnd;
        static Form FactorialResWnd;

        [STAThread]
        [LoaderOptimization(LoaderOptimization.MultiDomain)]
        static void Main()
        {
            Application.EnableVisualStyles();

            DataForCalculate = AppDomain.CreateDomain("DataForCalculate");
            FactorialRes = AppDomain.CreateDomain("FactorialRes");

            DataForCalculateAsm = DataForCalculate.Load(AssemblyName.GetAssemblyName("DataForCalculate.exe"));
            FactorialResAsm = FactorialRes.Load(AssemblyName.GetAssemblyName("FactorialRes.exe"));

            FactorialResWnd = Activator.CreateInstance(FactorialResAsm.GetType("FactorialRes.Form1")) as Form;
            DataForCalculateWnd = Activator.CreateInstance(DataForCalculateAsm.GetType("DataForCalculate.Form1"),
                new object[] { FactorialResAsm.GetModule("FactorialRes.exe"), FactorialResWnd}) as Form;
            
            (new Thread(new ThreadStart(RunData))).Start();
            (new Thread(new ThreadStart(RunFact))).Start();

            FactorialRes.DomainUnload += FactorialRes_DomainUnload;
        }

        static void FactorialRes_DomainUnload(object sender, EventArgs e)
        {
            MessageBox.Show("Good Bye");         
        }

        static void RunData()
        {
            DataForCalculateWnd.ShowDialog();
            Application.Exit();           
        }

        static void RunFact()
        {
            FactorialResWnd.ShowDialog();
            AppDomain.Unload(FactorialRes);
        }
    }
}
