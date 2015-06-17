using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UsingControl
{
    static class Program
    {
        static AppDomain Graphic;
        static AppDomain DataTable;

        static Assembly GraphicAsm;
        static Assembly DataTableAsm;

        static Form GraphicWindow;
        static Form DataTableWindow;

        [STAThread]
        [LoaderOptimization(LoaderOptimization.MultiDomain)]
        static void Main()
        {
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            Graphic = AppDomain.CreateDomain("Graphic");
            DataTable = AppDomain.CreateDomain("DataTable");
            
            GraphicAsm = Graphic.Load(AssemblyName.GetAssemblyName("Graphic.exe"));
            DataTableAsm = DataTable.Load(AssemblyName.GetAssemblyName("DataTable.exe"));

            GraphicWindow = Activator.CreateInstance(GraphicAsm.GetType("Graphic.Form1")) as Form;
            DataTableWindow = Activator.CreateInstance(DataTableAsm.GetType("DataTable.Form1"),
                new object[] {GraphicAsm.GetModule("Graphic.exe"), GraphicWindow}) as Form;
           
            (new Thread(new ThreadStart(RunVisualizer))).Start();
            (new Thread(new ThreadStart(RunGraphic))).Start();

            Graphic.DomainUnload += Graphic_DomainUnload;
        }
       
        static void Graphic_DomainUnload(object sender, EventArgs e)
        {
            MessageBox.Show("Domain with name " + (sender as AppDomain).FriendlyName +
                " has been successfully unloaded!");
        }

        static void RunGraphic()
        {
            GraphicWindow.ShowDialog();
            AppDomain.Unload(Graphic);
        }

        static void RunVisualizer()
        {
            DataTableWindow.ShowDialog();
            Application.Exit();
        }
    }
}
