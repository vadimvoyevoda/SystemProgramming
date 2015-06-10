using System.Diagnostics;
using System.Windows;
using System.Management;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

namespace _4.ProcessesList_TreeView
{
    public partial class MainWindow : Window
    {
        Process[] processes;

        ParameterizedThreadStart threadStartLoad;        
        ParameterizedThreadStart threadChildLoad;

        Thread thread_loadProc;
        Thread thread_loadChildProc;

        TreeViewItem loadTVI;
        Dictionary<int, object> childrenLoad;

        //TimerCallback timerCallback;
        //Timer timer;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.MouseMove += MainWindow_MouseMove;
            this.twProcesses.SelectedItemChanged += twProcesses_SelectedItemChanged;
            this.btnRenew.Click += btnRenew_Click;

            threadStartLoad = new ParameterizedThreadStart(LoadParentProcesses);
            thread_loadProc = new Thread(threadStartLoad);
            thread_loadProc.SetApartmentState(ApartmentState.STA);
            thread_loadProc.IsBackground = true;

            threadChildLoad = new ParameterizedThreadStart(LoadChildProcesses);
            thread_loadChildProc = new Thread(threadChildLoad);
            thread_loadChildProc.SetApartmentState(ApartmentState.STA);
            thread_loadChildProc.IsBackground = true;

            //timerCallback = new TimerCallback(timerFunc);
            //timer = new Timer(timerCallback);
        }

        //private void timerFunc(object a)
        //{
        //    if (!thread_loadChildProc.IsAlive && childrenLoad != null && loadTVI != null)
        //    {
        //        FuncTree(childrenLoad, null);

        //        //timer.Dispose();

        //        //childrenLoad = null;
        //        //loadTVI = null;
        //    }
        //}

        void btnRenew_Click(object sender, RoutedEventArgs e)
        {
            twProcesses.Items.Clear();
            pnlInfo.Children.Clear();

            StartFunc();
        }

        void twProcesses_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            foreach(Process pr in processes)
            {
                if ((sender as TreeView).SelectedItem != null && pr.Id.ToString() == ((sender as TreeView).SelectedItem as TreeViewItem).Uid)
                {
                    pnlInfo.Children.Clear();

                    TextBlock t1 = new TextBlock();
                    t1.Text = "Інформація про процес:";
                    t1.Text += Environment.NewLine;

                    TextBlock t2 = new TextBlock();
                    t2.Text = "ProcessName: " + pr.ProcessName;
                    TextBlock t3 = new TextBlock();
                    t3.Text = "Process Id: " + pr.Id;
                    t3.Text += Environment.NewLine;

                    TextBlock t4 = new TextBlock();
                    t4.Text = "BasePriority: " + pr.BasePriority;
                    TextBlock t5 = new TextBlock();
                    t5.Text = "PriorityClass: ";
                    try { t5.Text += pr.PriorityClass; } 
                    catch (Exception exc) { }
                    TextBlock t6 = new TextBlock();
                    t6.Text = "ProcessorAffinity: ";
                    try { t6.Text += pr.ProcessorAffinity; }
                    catch (Exception exc) { }
                    t6.Text += Environment.NewLine;

                    TextBlock t7 = new TextBlock();
                    t7.Text = "PagedMemorySize: " + pr.PagedMemorySize;
                    TextBlock t8 = new TextBlock();
                    t8.Text = "NonpagedSystemMemorySize: " + pr.NonpagedSystemMemorySize;
                    TextBlock t9 = new TextBlock();
                    t9.Text = "PagedSystemMemorySize: " + pr.PagedSystemMemorySize;
                    TextBlock t10 = new TextBlock();
                    t10.Text = "PeakPagedMemorySize: " + pr.PeakPagedMemorySize;
                    TextBlock t11 = new TextBlock();
                    t11.Text = "PeakVirtualMemorySize: " + pr.PeakVirtualMemorySize;
                    TextBlock t12 = new TextBlock();
                    t12.Text = "PrivateMemorySize: " + pr.PrivateMemorySize;
                    TextBlock t13 = new TextBlock();
                    t13.Text = "VirtualMemorySize: " + pr.VirtualMemorySize;
                    t13.Text += Environment.NewLine;

                    TextBlock t14 = new TextBlock();
                    t14.Text = "StartTime: ";
                    try { t14.Text += pr.StartTime; }
                    catch (Exception exc) { }

                    pnlInfo.Children.Add(t1);
                    pnlInfo.Children.Add(t2);
                    pnlInfo.Children.Add(t3);
                    pnlInfo.Children.Add(t4);
                    pnlInfo.Children.Add(t5);
                    pnlInfo.Children.Add(t6);
                    pnlInfo.Children.Add(t7);
                    pnlInfo.Children.Add(t8);
                    pnlInfo.Children.Add(t9);
                    pnlInfo.Children.Add(t10);
                    pnlInfo.Children.Add(t11);
                    pnlInfo.Children.Add(t12);
                    pnlInfo.Children.Add(t13);
                    pnlInfo.Children.Add(t14);
                    break;
                }
            }
                
        }

        void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (!thread_loadChildProc.IsAlive && childrenLoad != null && loadTVI != null)
            {
                FuncTree(childrenLoad, null);
                childrenLoad = null;
                loadTVI = null;
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartFunc();
        }

        private void StartFunc()
        {
            twProcesses.IsEnabled = false;
            tbProcesses.Text = "Зачекайте, будується дерево процесів";

            Dictionary<int, object> proc = new Dictionary<int, object>();

            thread_loadProc = new Thread(threadStartLoad);
            thread_loadProc.Start(proc);
            thread_loadProc.Join();

            foreach (var pr in BuildTree(proc))
            {
                twProcesses.Items.Add(pr);
            }

            FuncTree(proc, twProcesses);

            tbProcesses.Text = "Кількість запущених процесів: " + processes.Length;
            twProcesses.IsEnabled = true;
        }

        private void FuncTree(Dictionary<int, object> proc, TreeView root)
        {
            if (root == null)
            {
                foreach (var ch in proc)
                {                    
                    foreach (var el in loadTVI.Items)
                    {
                        //string text = String.Empty;
                        //Dispatcher.BeginInvoke(new ThreadStart(
                        //    delegate()
                        //    {
                        //        text = (el as TreeViewItem).Uid;
                        //    }
                        //));

                        if (ch.Key.ToString() == (el as TreeViewItem).Uid)
                        {
                            foreach (var c in BuildTree(ch.Value as Dictionary<int, object>))
                            {
                                (el as TreeViewItem).Items.Add(c);
                            }
                        }
                    }
                }               
            }
            else
            {
                foreach (var ch in proc)
                {
                    foreach (var el in root.Items)
                    {                        
                        if (ch.Key.ToString() == (el as TreeViewItem).Uid)
                        {
                            foreach (var c in BuildTree(ch.Value as Dictionary<int, object>))
                            {
                                (el as TreeViewItem).Items.Add(c);
                            }
                        }
                    }
                }
            }
        }

        private List<TreeViewItem> BuildTree(Dictionary<int, object> source)
        {
            List<TreeViewItem> items = new List<TreeViewItem>();
            foreach (var pr in source)
            {
                TreeViewItem tvi = new TreeViewItem();
                foreach (Process p in processes)
                {
                    if (p.Id == pr.Key)
                    {
                        tvi.Header = p.ProcessName;
                        break;
                    }
                }
                tvi.Uid = pr.Key.ToString();
                items.Add(tvi);
            }
            return items;
        }

        private void LoadParentProcesses(object proc)
        {
            processes = Process.GetProcesses();

            foreach (Process pr in processes)
            {
                if (GetParentId(pr.Id) == -1)
                {
                    (proc as Dictionary<int, object>).Add(pr.Id, null);
                }
            }

            LoadChildProcesses(proc);
        }

        private void LoadChildProcesses(object proc)
        {
            List<int> procId = new List<int>();

            foreach (var el in (proc as Dictionary<int, object>).Keys)
            {
                procId.Add(el);
            }

            foreach (var t in procId)
            {
                Dictionary<int, object> children = new Dictionary<int, object>();
                foreach (Process child in processes)
                {
                    int parentId = GetParentId(child.Id);

                    if (parentId == t)
                    {
                        children.Add(child.Id, null);
                    }
                }
                (proc as Dictionary<int, object>)[t] = children;
            }
        }

        private int GetParentId(int id)
        {
            int parentId = -1;

            // Пошук батьківського процесу
            using (ManagementObject obj = new ManagementObject("win32_process.handle=" + id.ToString()))
            {
                try
                {
                    obj.Get();
                    parentId = Convert.ToInt32(obj["ParentProcessId"]);
                }
                catch (Exception exc)
                {
                }
            }

            // Перевірка чи запущений батьківський процес            
            int count = 0;

            if (parentId != -1)
            {
                foreach (Process pr in processes)
                {
                    if (pr.Id == parentId)
                    {
                        count++;
                        break;
                    }
                }
                if (count == 0)
                {
                    parentId = -1;
                }
            }
            return parentId;
        }

        private void twProcesses_Expanded_1(object sender, RoutedEventArgs e)
        {
            Dictionary<int, object> children = new Dictionary<int, object>();

            foreach (var el in (e.Source as TreeViewItem).Items)
            {
                children.Add(Int32.Parse((el as TreeViewItem).Uid), null);
            }

            thread_loadChildProc = new Thread(threadChildLoad);
            thread_loadChildProc.SetApartmentState(ApartmentState.STA);
            thread_loadChildProc.IsBackground = true;            

            thread_loadChildProc.Start(children);
                        
            loadTVI = (e.Source as TreeViewItem);
            childrenLoad = children;

            //timer.Change(0, 10000);
        }
    }
}
