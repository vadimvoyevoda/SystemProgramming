using System.Windows;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;


namespace Lesson1_Processes_Home
{   
    public partial class MainWindow : Window
    {
        const uint WM_SETTEXT = 0x0C;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hwnd, uint Msg, int wParam,
                                       [MarshalAs(UnmanagedType.LPStr)]string lParam);
        
        List<Process> Processes = new List<Process>();

        int count = 0;

        void LoadAvailableAssemblies()
        {
            string except = new FileInfo(Assembly.GetExecutingAssembly().Location).Name;
            except = except.Substring(0, except.IndexOf('.'));
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.exe");

            foreach (var file in files)
            {
                string fileName = new FileInfo(file).Name;

                if (fileName.IndexOf(except) == -1)
                {
                    availableAssembly.Items.Add(fileName);
                }
            }
        }

        void RunProcess(string AssemblyName)
        {
            Process proc = Process.Start(AssemblyName);
            Processes.Add(proc);

            if (Process.GetCurrentProcess().Id == GetParentProcessId(proc.Id))
            {
                MessageBox.Show(proc.ProcessName + "дійсно дочірній процес поточного процесу");
            }
            proc.EnableRaisingEvents = true;
            proc.Exited +=proc_Exited;
            SetChildWindowText(proc.MainWindowHandle, "Child Process # " + (++count));

            if (!runAssembly.Items.Contains(proc.ProcessName))
            {
                runAssembly.Items.Add(proc.ProcessName);
            }
            availableAssembly.Items.Remove(availableAssembly.SelectedItem);
        }

        void SetChildWindowText(IntPtr Handle, string text)
        {
            SendMessage(Handle, WM_SETTEXT, 0, text);
        }

        int GetParentProcessId(int Id)
        {
            int parentId = 0;
            using (ManagementObject obj = new ManagementObject("win32_process.handle=" + Id.ToString()))
            {
                obj.Get();
                parentId = Convert.ToInt32(obj["ParentProcessId"]);
            }
            return parentId;
        }

        void proc_Exited(object sender, EventArgs e)
        {
            Process proc = sender as Process;
            try
            {
                runAssembly.Items.Remove(proc.ProcessName);
                availableAssembly.Items.Add(proc.ProcessName);
            }
            catch (Exception exp)
            { }

            Processes.Remove(proc);
            count--;
            int index = 0;

            foreach (var p in Processes)
            {
                SetChildWindowText(p.MainWindowHandle, "Child process #" + (++index));
            }
        }

        delegate void ProcessDelegate(Process proc);

        void ExecuteOnProcessesByName(string ProcessName, ProcessDelegate funk)
        {
            Process[] processes = Process.GetProcessesByName(ProcessName);
            foreach (var process in processes)
            {
                if (Process.GetCurrentProcess().Id == GetParentProcessId(process.Id))
                {
                    funk(process);
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            LoadAvailableAssemblies();

            btnStart.Click += btnStart_Click;
            btnStop.Click += btnStop_Click;
            btnClose.Click += btnClose_Click;
            btnRefresh.Click += btnRefresh_Click;

            availableAssembly.SelectionChanged += availableAssembly_SelectionChanged;
            runAssembly.SelectionChanged += runAssembly_SelectionChanged;

            this.Closing += MainWindow_Closing;

            btnCalc.Click += btnCalc_Click;
        }

        void btnCalc_Click(object sender, RoutedEventArgs e)
        {
            RunProcess("calc.exe");
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var p in Processes)
            {
                p.Kill();
            }
        }

        void runAssembly_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (runAssembly.SelectedItems.Count == 0)
            {
                btnStop.IsEnabled = false;
                btnClose.IsEnabled = false;
                btnRefresh.IsEnabled = false;
            }
            else
            {
                btnStop.IsEnabled = true;
                btnClose.IsEnabled = true;
                btnRefresh.IsEnabled = true;
            }
        }

        void availableAssembly_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (availableAssembly.SelectedItems.Count == 0)
            {
                btnStart.IsEnabled = false;
            }
            else
            {
                btnStart.IsEnabled = true;
            }
        }
                
        void Refresh(Process proc)
        {
            proc.Refresh();
        }

        void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnProcessesByName(runAssembly.SelectedItem.ToString(), Refresh);
        }

        void CloseMainWindow(Process proc)
        {
            proc.CloseMainWindow();
        }
        
        void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnProcessesByName(runAssembly.SelectedItem.ToString(), CloseMainWindow);
            if (runAssembly.SelectedItem.ToString() != "calc")
            {
                availableAssembly.Items.Add(runAssembly.SelectedItem);
            }
            runAssembly.Items.Remove(runAssembly.SelectedItem);

        }
        
        void Kill(Process proc)
        {
            proc.Kill();
        }

        void btnStop_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnProcessesByName(runAssembly.SelectedItem.ToString(), Kill);
            if (runAssembly.SelectedItem.ToString() != "calc")
            {
                availableAssembly.Items.Add(runAssembly.SelectedItem);
            }
            runAssembly.Items.Remove(runAssembly.SelectedItem);
        }
        
        void btnStart_Click(object sender, RoutedEventArgs e)
        {
            RunProcess(availableAssembly.SelectedItem.ToString());
        }
    }
}
