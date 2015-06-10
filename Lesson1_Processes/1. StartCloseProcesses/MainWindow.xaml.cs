using System;
using System.Diagnostics;
using System.Windows;

namespace Lesson1_Processes
{
    public partial class MainWindow : Window
    {
        Process newProcess = new Process();        

        public MainWindow()
        {
            InitializeComponent();

            btnStart.Click += btnStart_Click;
            btnClose.Click += btnClose_Click;
            
            newProcess.StartInfo = new ProcessStartInfo("notepad.exe");
            newProcess.Exited += newProcess_Exited;
        }
                
        void btnStart_Click(object sender, RoutedEventArgs e)
        {
            newProcess.Start();            
        }

        void btnClose_Click(object sender, RoutedEventArgs e)
        {           
            newProcess.CloseMainWindow();
        }

        void newProcess_Exited(object sender, System.EventArgs e)
        {
            newProcess.Close();
        }
    }
}
