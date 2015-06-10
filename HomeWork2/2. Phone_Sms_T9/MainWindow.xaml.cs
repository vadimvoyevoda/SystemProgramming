using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace _2.Phone_Sms_T9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class ThreadData
    {
        static List<string> words;

        public static string thisWordStart { get; set; }
        public static string Word { get; set; }

        public static void ThreadInit()
        {
            words = new List<string>();
            using (StreamReader sr = new StreamReader("dict.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    words.Add(line);
                }
            }
            words.Sort();
        }

        public static void ThreadFunc()
        {
            Word = String.Empty;
            foreach (var el in words)
            {
                if (el.StartsWith(thisWordStart))
                {
                    Word = el;
                    break;
                }
            }
        }

        public static void AddWordToDictionary(string newWord)
        {
            using (StreamWriter sw = new StreamWriter("dict.txt", true))
            {
                sw.WriteLine(newWord);
            }            
        }

        public static bool IsWordInDict(string newWord)
        {
            if (words.Contains(newWord))
            {
                return true;
            }
            
            return false;
        }
    }

    public partial class MainWindow : Window
    {
        bool uset9;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;

            this.tbScreen.PreviewKeyDown += tbScreen_PreviewKeyDown;
            this.tbScreen.KeyDown += tbScreen_KeyDown;
            this.tbScreen.TextChanged += tbScreen_TextChanged;
            this.tbScreen.SelectionChanged += tbScreen_SelectionChanged;
            
            this.btnClear.Click += btnClear_Click;
            this.btnAdd.Click += btnAdd_Click;
            this.btnD.Click += btnD_Click;
            this.btnZ.Click += btnZ_Click;
        }

        void tbScreen_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int start = SearchTo(tbScreen.SelectionStart)+1;
            if ((tbScreen.SelectedText.Length > 0) && !tbScreen.SelectedText.Contains(' ')
                && !ThreadData.IsWordInDict(tbScreen.Text.Substring(start,tbScreen.SelectionStart+tbScreen.SelectedText.Length-start)))
            {
                btnAdd.IsEnabled = true;

            }
            else
            {
                btnAdd.IsEnabled = false;
            }
        }

        int SearchTo(int idx)
        {
            for (int i = idx-1; i >= 0; i--)
            {
                if (tbScreen.Text[i] == ' ')
                {
                    return i;
                }
            }
            return -1;
        }
        void tbScreen_KeyDown(object sender, KeyEventArgs e)
        {
            uset9 = true;
        }

        void tbScreen_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                uset9 = false;
            }
            else
            {
                uset9 = true;
            }
        }

        int StartSel()
        {
            int wordStart = tbScreen.Text.LastIndexOf(" ") + 1;
            if (wordStart == 0)
            {
                wordStart = tbScreen.Text.LastIndexOf("\n") + 1;
            }
            if (wordStart == -1)
            {
                if (wordStart == -1)
                {
                    wordStart = 0;
                }
            }
            return wordStart;
        }

        void tbScreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (uset9)
            {
                int wordStart = StartSel();
                int wordFin = tbScreen.Text.Length;
                string thisWordStart = tbScreen.Text.Substring(wordStart, wordFin - wordStart);

                if (thisWordStart.Length > 0)
                {
                    ThreadStart start = new ThreadStart(ThreadData.ThreadFunc);
                    ThreadData.thisWordStart = thisWordStart;
                    Thread thread = new Thread(start);
                    thread.IsBackground = true;
                    thread.Start();
                    thread.Join();

                    string w = ThreadData.Word;
                    if (w.Length > 0)
                    {
                        tbScreen.Text += w.Substring(thisWordStart.Length);
                        tbScreen.CaretIndex = tbScreen.Text.Length;
                        tbScreen.Select(wordStart + thisWordStart.Length, w.Substring(thisWordStart.Length).Length);
                    }
                }
            }
        }

        void btnZ_Click(object sender, RoutedEventArgs e)
        {
            tbScreen.Text += "*";
        }

        void btnD_Click(object sender, RoutedEventArgs e)
        {
            tbScreen.Text += "#";
        }

        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ThreadData.AddWordToDictionary(tbScreen.SelectedText);
            ThreadData.ThreadInit();
        }
        
        void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbScreen.Clear();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            tbScreen.Focus();
            btnAdd.IsEnabled = false;
            ThreadData.ThreadInit();
        }        
    }
}
