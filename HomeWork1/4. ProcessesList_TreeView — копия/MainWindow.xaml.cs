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
        #region
    //    ThreadStart threadStart;
    //    ParameterizedThreadStart threadStartChild;

    //    Thread Thread_LoadMainProc;
    //    Thread Thread_LoadChildProc;

    //    Process[] processes;

    //    public MainWindow()
    //    {
    //        InitializeComponent();

    //        threadStart = new ThreadStart(LoadProcesses);
    //        Thread_LoadMainProc = new Thread(threadStart);
    //        Thread_LoadMainProc.Priority = ThreadPriority.Highest;
    //        Thread_LoadMainProc.SetApartmentState(ApartmentState.STA);
            
    //        this.Loaded += MainWindow_Loaded;
    //    }

    //    void MainWindow_Loaded(object sender, RoutedEventArgs e)
    //    {
    //        twProcesses.IsEnabled = false;                        
    //        Thread_LoadMainProc.Start();             
    //    }

    //    private void LoadProcesses()
    //    {
    //        tbProcesses.Dispatcher.BeginInvoke(new Action(
    //            delegate()
    //            {
    //                tbProcesses.Text = "Завантаження процесів ... ";
    //                tbProcesses.Cursor = Cursors.Wait;
    //            }
    //        ));
    //        twProcesses.Dispatcher.BeginInvoke(new Action(
    //           delegate()
    //           {
    //               twProcesses.Cursor = Cursors.Wait;
    //           }
    //       ));  

    //        processes = Process.GetProcesses();
                        
    //        BuildProcessTree();

    //        tbProcesses.Dispatcher.BeginInvoke(new Action(
    //            delegate()
    //            {
    //                tbProcesses.Text = "Кількість запущених процесів: " + processes.Length;
    //                tbProcesses.Cursor = Cursors.Arrow;
    //            }
    //        ));

    //        twProcesses.Dispatcher.BeginInvoke(new Action(
    //            delegate()
    //            {
    //                twProcesses.IsEnabled = true;
    //                twProcesses.Cursor = Cursors.Arrow;
    //            }
    //        ));           
    //    }

    //    private void BuildProcessTree()
    //    {
    //        foreach (Process pr in processes)
    //        {
    //            if (GetParentId(pr.Id) == -1)
    //            {
    //                twProcesses.Dispatcher.BeginInvoke(new Action(
    //                       delegate()
    //                       {
    //                            twProcesses.Items.Add(BuildProcessTree(pr));
    //                       }
    //                ));                     
    //            }
    //        }
    //    }

    //    private TreeViewItem BuildProcessTree(Process pr)
    //    {
    //        TreeViewItem tviProc = new TreeViewItem();
    //        tviProc.Header = pr.ProcessName;
    //        tviProc.Uid = pr.Id.ToString();

    //        if (HaveChildren(pr.Id))
    //        {
    //            tviProc.Items.Add("");
    //        }
    //        return tviProc;
    //    }

    //    private bool HaveChildren(int id)
    //    {
    //        foreach (Process pr in processes)
    //        {
    //            if (GetParentId(pr.Id) == id)
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }

    //    private List<Process> GetChildren(int id)
    //    {
    //        List<Process> children = new List<Process>();

    //        foreach (Process pr in processes)
    //        {
    //            if (GetParentId(pr.Id) == id)
    //            {
    //                children.Add(pr);
    //            }
    //        }
    //        return children;
    //    }


    //    private int GetParentId(int id)
    //    {
    //        int parentId = -1;

    //        // Пошук батьківського процесу
    //        using (ManagementObject obj = new ManagementObject("win32_process.handle=" + id.ToString()))
    //        {
    //            obj.Get();
    //            parentId = Convert.ToInt32(obj["ParentProcessId"]);
    //        }

    //        // Перевірка чи запущений батьківський процес            
    //        int count = 0;
    //        foreach (Process pr in processes)
    //        {
    //            if (pr.Id == parentId)
    //            {
    //                count++;
    //                break;
    //            }
    //        }
    //        if (count == 0)
    //        {
    //            parentId = -1;
    //        }

    //        return parentId;
    //    }

    //    private void twProcesses_Expanded_1(object sender, RoutedEventArgs e)
    //    {
    //        threadStartChild = new ParameterizedThreadStart(LoadChildren);
    //        Thread_LoadChildProc = new Thread(threadStartChild);
    //        Thread_LoadChildProc.IsBackground = true;
    //        Thread_LoadChildProc.SetApartmentState(ApartmentState.STA);

    //        List<TreeViewItem> twl = new List<TreeViewItem>();
    //        List<object> param = new List<object>();
    //        param.Add(e.Source);
    //        param.Add(twl);

    //        Thread_LoadChildProc.Start(param);

    //        MessageBox.Show("1");
    //        while (Thread_LoadChildProc.IsAlive)
    //        {
    //            Thread.Sleep(100);
    //        }
    //        MessageBox.Show("2");
    //        foreach (var el in twl)
    //        {
    //            (e.Source as TreeViewItem).Items.Add(el);
    //        }
    //        MessageBox.Show("3");
    //    }

    //    private void LoadChildren(object param)
    //    {
    //        TreeViewItem select = (TreeViewItem)(param as List<object>)[0];            

    //        select.Dispatcher.BeginInvoke(new Action(
    //                       delegate()
    //                       {
    //                           select.Items.Clear();
    //                       }
    //                ));

    //        MessageBox.Show("4");
    //        foreach (Process pr in processes)
    //        {
    //            var text = String.Empty;
    //            System.Windows.Application.Current.Dispatcher.Invoke(
    //            DispatcherPriority.Normal,
    //            (ThreadStart)delegate { text = select.Uid; });

    //            if (pr.Id.ToString() == text)
    //            {                    
    //                List<Process> child = GetChildren(pr.Id);
    //                for (int i = 0; i < child.Count; i++)
    //                {
    //                    TreeViewItem tviChild = new TreeViewItem();
    //                    tviChild.Header = child[i].ProcessName;
    //                    tviChild.Uid = child[i].Id.ToString();

    //                    MessageBox.Show("5");
    //                    ((param as List<object>)[1] as List<TreeViewItem>).Add(tviChild);
    //                    MessageBox.Show(((param as List<object>)[1] as List<TreeViewItem>).Count.ToString());
    //                    //(select as TreeViewItem).Items.Add(tviChild);

    //                    //Dispatcher.BeginInvoke(new ThreadStart(
    //                    //    delegate { (select as TreeViewItem).Items.Add(tviChild); }));

    //                    //(select as TreeViewItem).Dispatcher.BeginInvoke(new Action(
    //                    //   delegate()
    //                    //   {
    //                    //       (select as TreeViewItem).Items.Add(tviChild);
    //                    //   }
    //                    //));                         
    //                }
    //                break;
    //            }
    //        }

    //        MessageBox.Show("6");
    //        foreach (var el in (select as TreeViewItem).Items)
    //        {
    //            foreach (Process pr in processes)
    //            {
    //                var text = String.Empty;
    //                System.Windows.Application.Current.Dispatcher.Invoke(
    //                DispatcherPriority.Normal,
    //                (ThreadStart)delegate { text = select.Uid; });

    //                if (pr.Id.ToString().Equals(text))
    //                {
    //                    if (HaveChildren(pr.Id))
    //                    {
    //                        (el as TreeViewItem).Dispatcher.BeginInvoke(new Action(
    //                                delegate()
    //                                {
    //                                    (el as TreeViewItem).Items.Add("");
    //                                }
    //                             )); 
    //                    }
    //                    break;
    //                }
    //            }
    //            MessageBox.Show("7");
    //        }               
    //    }
    #endregion

        Process[] processes;
        ParameterizedThreadStart threadStartLoad;
        Thread thread_loadProc;
        ParameterizedThreadStart threadChildLoad;
        Thread thread_loadChildProc;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;

            threadStartLoad = new ParameterizedThreadStart(LoadParentProcesses);
            thread_loadProc = new Thread(threadStartLoad);
            thread_loadProc.SetApartmentState(ApartmentState.STA);
            thread_loadProc.IsBackground = true;

            threadChildLoad = new ParameterizedThreadStart(LoadChildProcesses);
            thread_loadChildProc = new Thread(threadChildLoad);
            thread_loadChildProc.SetApartmentState(ApartmentState.STA);
            thread_loadChildProc.IsBackground = true;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            twProcesses.IsEnabled = false;
            tbProcesses.Text = "Зачекайте, будується дерево процесів";
                        
            Dictionary<int, object> proc = new Dictionary<int, object>();
            
            thread_loadProc.Start(proc);

            while (thread_loadProc.IsAlive)
            {
                Thread.Sleep(50);
            }

            foreach (var pr in BuildTree(proc))
            {
                twProcesses.Items.Add(pr);
            }

            FuncTree(proc, twProcesses.Items);

            tbProcesses.Text = "Кількість запущених процесів: " + processes.Length;
            twProcesses.IsEnabled = true;
        }

        private void FuncTree(Dictionary<int, object> proc, ItemCollection items)
        {
            foreach (var ch in proc)
            {
                foreach (var el in items)
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

        private List<TreeViewItem> BuildTree(Dictionary<int, object> source)
        {
            List<TreeViewItem> items = new List<TreeViewItem>();
            foreach (var pr in source)
            {
                TreeViewItem tvi = new TreeViewItem();
                foreach(Process p in processes)
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

            foreach(var el in (proc as Dictionary<int, object>).Keys)
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

            return parentId;
        }

        private void twProcesses_Expanded_1(object sender, RoutedEventArgs e)
        {
            Dictionary<int, object> children = new Dictionary<int, object>();

            foreach (var el in (e.Source as TreeViewItem).Items)
            {
                children.Add(Int32.Parse((el as TreeViewItem).Uid), null);
            }
            
            thread_loadChildProc.Start(children);

            while (thread_loadChildProc.IsAlive)
            {
            }
            
            FuncTree(children, (e.Source as TreeViewItem).Items);
            
        }
    }
}
