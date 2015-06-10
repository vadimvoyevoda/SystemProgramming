using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeWork2_TheadPool
{
    public partial class MainWindow : Window
    {
        List<Thread> threads = new List<Thread>();
        List<Timer> timers = new List<Timer>();
        List<int> count = new List<int>();
        ListBox ic = new ListBox();

        Semaphore s;

        public MainWindow()
        {
            InitializeComponent();

            tbNum.Text = "3";
            s = new Semaphore(3, 3, "MySemaphore");

            btnCreate.Click +=btnCreate_Click;
            lbCreate.MouseDoubleClick += lbCreate_MouseDoubleClick;
            lbRun.MouseDoubleClick += lbRun_MouseDoubleClick;
            btnUp.Click += btnUp_Click;
            btnDown.Click += btnDown_Click;
            tbNum.TextChanged += tbNum_TextChanged;
        }

        void lbRun_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lbRun.SelectedItem != null)
            {
                int idx = Int32.Parse((lbRun.SelectedItem as string).Substring(6, (lbRun.SelectedItem as string).IndexOf(" -->") - 6)) - 1;
                lbRun.Items.Remove(lbRun.SelectedItem);
                timers[idx].Dispose();
                threads[idx].Abort();
                s.Release().ToString();                    
            }
        }

        void tbNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            int res;
            if (!Int32.TryParse(tbNum.Text, out res))
            {
                tbNum.Text = "3";
                s = new Semaphore(3, 3, "MySemaphore");
            }
            else
            {                   
                s.Dispose();
                s = new Semaphore(res, res, "MySemaphore");
                //Semaphore newSem = new Semaphore(res, res, "MyNewSemaphore");

                //foreach (var el in lbRun.Items)
                //{
                //    int idx = Int32.Parse((el as string).Substring(6, (el as string).IndexOf(" -->") - 6)) - 1;
                //    threads[idx].Start(new List<object>() {timers[idx], newSem}
                //}
                
                //MessageBox.Show(s.Release().ToString());
            }
        }

        void btnDown_Click(object sender, RoutedEventArgs e)
        {
            int res = Int32.Parse(tbNum.Text);
            if (res > 1)
                    --res;
            tbNum.Text = res.ToString();
            
        }

        void btnUp_Click(object sender, RoutedEventArgs e)
        {
            int res = Int32.Parse(tbNum.Text);            
                ++res;
            tbNum.Text = res.ToString();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            ParameterizedThreadStart ts = new ParameterizedThreadStart(IncrementFunc);
            Thread thread = new Thread(ts);
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);

            threads.Add(thread);
            count.Add(0);
            TimerCallback tcall = new TimerCallback(timerFunc);
            Timer timer = new Timer(tcall);
            timers.Add(timer);

            string newItem = String.Format("Потік {0} --> створений",threads.Count);
            lbCreate.Items.Add(newItem);
        }
        
        void lbCreate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lbCreate.SelectedItem != null)
            {
                lbWait.Items.Add(
                    (lbCreate.SelectedItem as string).
                    Substring(0, (lbCreate.SelectedItem as string).IndexOf(">") + 2) + "очікує"
                    );
                
                int idx = Int32.Parse((lbCreate.SelectedItem as string).Substring(6, (lbCreate.SelectedItem as string).IndexOf(" -->") - 6)) - 1;                
                threads[idx].Start(new List<object>() {timers[idx], s});

                lbCreate.Items.Remove(lbCreate.SelectedItem);
            }
        }
        
        private void IncrementFunc(object param)
        {
            Semaphore tmp = (param as List<object>)[1] as Semaphore;
       
            if (tmp.WaitOne())
            {
                Dispatcher.Invoke(new Action(
                               delegate()
                               {
                                   ic.Items.Clear();

                                   foreach (var el in lbWait.Items)
                                   {
                                       ic.Items.Add(el);
                                   }
                               }));

                int num = 0;

                for (int i = 0; i < timers.Count; i++)
                {
                    if (((param as List<object>)[0] as Timer).GetHashCode() == timers[i].GetHashCode())
                    {
                        num = i;
                        break;
                    }
                }

                foreach (var el in ic.Items)
                {
                    int idx = Int32.Parse((el as string).Substring(6, (el as string).IndexOf(" -->") - 6)) - 1;

                    if (idx == num)
                    {
                        Dispatcher.Invoke(new Action(
                           delegate()
                           {
                               lbRun.Items.Add((el as string).
                                   Substring(0, (el as string).IndexOf(">") + 2));

                               lbWait.Items.Remove(el);
                           }));
                        break;
                    }
                }

                ((param as List<object>)[0] as Timer).Change(0, 1000);
            }
        }

        private void timerFunc(object a)
        {
            int num = 0;

            for (int i = 0; i < timers.Count; i++)
            {
                if ((a as Timer).GetHashCode() == timers[i].GetHashCode())
                {
                    num = i;
                    break;
                }
            }

                count[num]++;

                for (int i = 0; i < lbRun.Items.Count; i++)
                {
                    if ((lbRun.Items[i] as String).StartsWith(String.Format("Потік {0}", (int)num + 1)))
                    {
                        Dispatcher.Invoke(new Action(
                            delegate()
                            {
                                lbRun.Items[i] = String.Format("{0} {1}",
                                      (lbRun.Items[i] as string).Substring(0, (lbRun.Items[i] as string).IndexOf(">") + 1), count[num]);
                            }
                        ));

                        break;
                    }
                }            
        }
    }
}
