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
    public class DictionaryClass
    {
        static List<string> words;

        public static string thisWordStart { get; set; }
        public static string Word { get; set; }

        public static void DictionaryInit()
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

        public static void FindWord()
        {
            Word = String.Empty;
            foreach (var el in words)
            {
                if (el.StartsWith(thisWordStart.ToLower()))
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
                sw.WriteLine(newWord.ToLower());
            }            
        }

        public static bool IsWordInDict(string newWord)
        {
            if (words.Contains(newWord.ToLower()))
            {
                return true;
            }
            
            return false;
        }
    }

    public partial class MainWindow : Window
    {
        bool uset9, isDigit;
        char letter;
        int caretIdx, startSel, LenSel, num;
        TimerCallback callback, tc;
        Timer timer, t;
        Button btn;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;

            this.tbScreen.PreviewKeyDown += tbScreen_PreviewKeyDown;
            this.tbScreen.KeyDown += tbScreen_KeyDown;
            this.tbScreen.TextChanged += tbScreen_TextChanged;
            this.tbScreen.SelectionChanged += tbScreen_SelectionChanged;
            this.tbScreen.LostFocus += tbScreen_LostFocus;
            this.tbScreen.GotFocus += tbScreen_GotFocus;

            this.btnClear.Click += btnClear_Click;
            this.btnAdd.Click += btnAdd_Click;

            this.btnUp.Click += btnUp_Click;
            this.btnDown.Click += btnDown_Click;
            this.btnLeft.Click += btnLeft_Click;
            this.btnRight.Click += btnRight_Click;

            this.btnD.Click += btnD_Click;
            this.btnZ.Click += btnZ_Click;
            this.btn2.Click += btn2_Click;
            this.btn3.Click += btn3_Click;
            this.btn4.Click += btn4_Click;
            this.btn5.Click += btn5_Click;
            this.btn6.Click += btn6_Click;
            this.btn7.Click += btn7_Click;
            this.btn8.Click += btn8_Click;
            this.btn9.Click += btn9_Click;
            this.btn0.Click += btn0_Click;
            this.btn1.Click += btn1_Click;

            this.btn1.PreviewMouseDown += btn_PreviewMouseDown;
            this.btn2.PreviewMouseDown += btn_PreviewMouseDown;
            this.btn3.PreviewMouseDown += btn_PreviewMouseDown;
            this.btn4.PreviewMouseDown += btn_PreviewMouseDown;
            this.btn5.PreviewMouseDown += btn_PreviewMouseDown;
            this.btn6.PreviewMouseDown += btn_PreviewMouseDown;
            this.btn7.PreviewMouseDown += btn_PreviewMouseDown;
            this.btn8.PreviewMouseDown += btn_PreviewMouseDown;
            this.btn9.PreviewMouseDown += btn_PreviewMouseDown;
            this.btn0.PreviewMouseDown += btn_PreviewMouseDown;

            this.btn1.PreviewMouseUp += btn_PreviewMouseUp;
            this.btn2.PreviewMouseUp += btn_PreviewMouseUp;
            this.btn3.PreviewMouseUp += btn_PreviewMouseUp;
            this.btn4.PreviewMouseUp += btn_PreviewMouseUp;
            this.btn5.PreviewMouseUp += btn_PreviewMouseUp;
            this.btn6.PreviewMouseUp += btn_PreviewMouseUp;
            this.btn7.PreviewMouseUp += btn_PreviewMouseUp;
            this.btn8.PreviewMouseUp += btn_PreviewMouseUp;
            this.btn9.PreviewMouseUp += btn_PreviewMouseUp;
            this.btn0.PreviewMouseUp += btn_PreviewMouseUp;
        }

        void btn_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (t != null)
            {
                t.Dispose();
            }
        }
        
        void btn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            isDigit = false;
            tc = new TimerCallback(F);
            t = new Timer(tc);
            t.Change(1000, 1000);
            btn = (sender as Button);
        }

        void F(object a)
        {        
            Dispatcher.BeginInvoke(new Action(delegate
            {
                if (btn.IsPressed)
                {
                    isDigit = true;
                    AddText(btn.Name.Substring(3),caretIdx,true);
                    tbScreen.Focus();
                }
            }));
            if (t != null)
            {
                t.Dispose();
            }
        }

        void btn1_Click(object sender, RoutedEventArgs e)
        {
            if (!isDigit)
            {
                ButtonFunc(new char[] { '.', ',', '?', '!', '-', '_' });
            }
        }

        void btn0_Click(object sender, RoutedEventArgs e)
        {
            if (!isDigit)
            {
                AddText("+", caretIdx, true);
                tbScreen.Focus();
            }
        }

        void btn9_Click(object sender, RoutedEventArgs e)
        {
            if (!isDigit)
            {
                if (System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.IetfLanguageTag == "en-US")
                {
                    ButtonFunc(new char[] { 'w', 'x', 'y', 'z' });
                }
                else
                {
                    ButtonFunc(new char[] { 'ю', 'я' });
                }
            }
        }

        void btn8_Click(object sender, RoutedEventArgs e)
        {
            if (!isDigit)
            {
                if (System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.IetfLanguageTag == "en-US")
                {
                    ButtonFunc(new char[] { 't', 'u', 'v' });
                }
                else
                {
                    ButtonFunc(new char[] { 'ш', 'щ', 'ь' });
                }
            }
        }

        void btn7_Click(object sender, RoutedEventArgs e)
        {
            if (!isDigit)
            {
                if (System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.IetfLanguageTag == "en-US")
                {
                    ButtonFunc(new char[] { 'p', 'q', 'r', 's' });
                }
                else
                {
                    ButtonFunc(new char[] { 'ф', 'х', 'ц', 'ч' });
                }
            }
        }

        void btn6_Click(object sender, RoutedEventArgs e)
        {
            if (!isDigit)
            {
                if (System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.IetfLanguageTag == "en-US")
                {
                    ButtonFunc(new char[] { 'm', 'n', 'o' });
                }
                else
                {
                    ButtonFunc(new char[] { 'р', 'с', 'т', 'у' });
                }
            }
        }

        void btn5_Click(object sender, RoutedEventArgs e)
        {
            if (!isDigit)
            {
                if (System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.IetfLanguageTag == "en-US")
                {
                    ButtonFunc(new char[] { 'j', 'k', 'l' });
                }
                else
                {
                    ButtonFunc(new char[] { 'м', 'н', 'о', 'п' });
                }
            }
        }

        void btn4_Click(object sender, RoutedEventArgs e)
        {
            if (!isDigit)
            {
                if (System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.IetfLanguageTag == "en-US")
                {
                    ButtonFunc(new char[] { 'g', 'h', 'i' });
                }
                else
                {
                    ButtonFunc(new char[] { 'и', 'і', 'ї', 'к', 'л' });
                }
            }
        }

        void btn3_Click(object sender, RoutedEventArgs e)
        {
            if (!isDigit)
            {
                if (System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.IetfLanguageTag == "en-US")
                {
                    ButtonFunc(new char[] { 'd', 'e', 'f' });
                }
                else
                {
                    ButtonFunc(new char[] { 'д', 'е', 'є', 'ж', 'з' });
                }
            }            
        }

        void btn2_Click(object sender, RoutedEventArgs e)
        {
            if (!isDigit)
            {
                if (System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.IetfLanguageTag == "en-US")
                {
                    ButtonFunc(new char[] { 'a', 'b', 'c' });
                }
                else
                {
                    ButtonFunc(new char[] { 'а', 'б', 'в', 'г' });
                }
            }
        }
        
        void ButtonFunc(char[] letters)
        {
            tbScreen.Text = tbScreen.Text.Remove(tbScreen.SelectionStart, tbScreen.SelectionLength);
            uset9 = true;
            startSel = LenSel = 0;

            if (timer != null)
            {
                timer.Dispose();
            }
            callback = new TimerCallback(TimerFunc);
            timer = new Timer(callback);
                        
            if(!letters.Contains(letter) || letter == letters[letters.Length-1])
            {
                if (letter == letters[letters.Length-1])
                {
                    caretIdx--;
                    uset9 = false;
                    tbScreen.Text = tbScreen.Text.Remove(caretIdx, 1);
                    uset9 = true;
                }
                num = 0;
                letter = letters[num];
            }
            else if (letter < letters[letters.Length-1])
            {
                letter=letters[++num];
                caretIdx--;
                uset9 = false;
                tbScreen.Text = tbScreen.Text.Remove(caretIdx, 1);
                uset9 = true;
            }
            AddText(letter.ToString(), caretIdx, true);
            timer.Change(1000, 1000);
        }

        void tbScreen_GotFocus(object sender, RoutedEventArgs e)
        {            
            tbScreen.CaretIndex = caretIdx;
            if (LenSel > 0)
            {
                tbScreen.Select(startSel, LenSel);
            }
        }

        void tbScreen_LostFocus(object sender, RoutedEventArgs e)
        {
            caretIdx = tbScreen.CaretIndex;
        }

        void TimerFunc(object a)
        {            
            letter = (char)0;
            
            Dispatcher.BeginInvoke(new Action(delegate
                {                    
                    tbScreen.Focus();
                }));
            
            timer.Dispose();
        }

        void AddText(string textInto, int caret, bool symb)
        {
            string textBefore = tbScreen.Text.Substring(0, caret);
            string textAfter = tbScreen.Text.Substring(caret, tbScreen.Text.Length - caret);

            if (symb)
            {
                caretIdx++;
            }
            if (textBefore.Length == 0 || textBefore[textBefore.Length - 1] == '.')
            {
                textInto = textInto[0].ToString().ToUpper() + textInto.Substring(1); 
            }
            
            tbScreen.Text = textBefore + textInto + textAfter;
            
            if (!symb)
            {
                startSel = caret;
                LenSel = textInto.Length;
                tbScreen.Focus();    
                tbScreen.Select(caret, textInto.Length);                
            }
        }

        void btnRight_Click(object sender, RoutedEventArgs e)
        {
            if (caretIdx < tbScreen.Text.Length)
            {
                caretIdx++;
            }
            tbScreen.Focus();
        }

        void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            if (caretIdx >= 1)
            {
                caretIdx--;
            }
            tbScreen.Focus();
        }

        void btnDown_Click(object sender, RoutedEventArgs e)
        {
            caretIdx = tbScreen.Text.Length;
            tbScreen.Focus();
        }

        void btnUp_Click(object sender, RoutedEventArgs e)
        {
            caretIdx = 0;
            tbScreen.Focus();
        }       

        void btnZ_Click(object sender, RoutedEventArgs e)
        {
            AddText("*", caretIdx, true);            
            tbScreen.Focus();
        }

        void btnD_Click(object sender, RoutedEventArgs e)
        {
            AddText("#", caretIdx, true);
            tbScreen.Focus();
        }

        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DictionaryClass.AddWordToDictionary(tbScreen.SelectedText);
            DictionaryClass.DictionaryInit();
        }

        void btnClear_Click(object sender, RoutedEventArgs e)
        {
            uset9 = false;
            if (caretIdx > 0)
            {
                caretIdx--;
                tbScreen.Text = tbScreen.Text.Remove(caretIdx, 1);
            }
            tbScreen.Focus();
        }

        void tbScreen_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (tbScreen.SelectionLength > 0)
            {
                int start = SearchTo(tbScreen.SelectionStart) + 1;
                if ((tbScreen.SelectedText.Length > 0) && !tbScreen.SelectedText.Contains(' ')
                    && !DictionaryClass.IsWordInDict(tbScreen.Text.Substring(start, tbScreen.SelectionStart + tbScreen.SelectedText.Length - start)))
                {
                    btnAdd.IsEnabled = true;

                }
                else
                {
                    btnAdd.IsEnabled = false;
                }
            }
        }

        void tbScreen_KeyDown(object sender, KeyEventArgs e)
        {
            uset9 = true;                
            caretIdx = tbScreen.CaretIndex+1;
            if (timer != null)
            {
                timer.Dispose();
            }
        }

        void tbScreen_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Space || e.Key == Key.Delete)
            {
                uset9 = false;
            }
            else
            {
                uset9 = true;
            }
            caretIdx = tbScreen.CaretIndex+1;
            if (timer != null)
            {
                timer.Dispose();
            }
        }

        int SearchTo(int idx)
        {
            for (int i = idx - 1; i >= 0; i--)
            {
                if (tbScreen.Text[i] == '\n' || tbScreen.Text[i] == ' ')
                {
                    return i+1;
                }
            }
            return -1;
        }

        int SearchEnd()
        {
            int wordEnd = tbScreen.Text.IndexOf(" ", caretIdx);
            if (wordEnd == -1)
            {
                wordEnd = tbScreen.Text.IndexOf("\r\n", caretIdx);
                if (wordEnd == -1)
                {
                    wordEnd = tbScreen.Text.Length;
                }
            }
            return wordEnd;
        }
      
        void tbScreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (uset9)
            {
                int tmp = SearchTo(caretIdx-1);
                int wordStart =  tmp == -1 ? 0 : tmp;
                int wordFin = SearchEnd();
                string thisWordStart = tbScreen.Text.Substring(wordStart, wordFin - wordStart);

                if (thisWordStart.Length > 0)
                {
                    ThreadStart start = new ThreadStart(DictionaryClass.FindWord);
                    DictionaryClass.thisWordStart = thisWordStart;
                    Thread thread = new Thread(start);
                    thread.IsBackground = true;
                    thread.Start();
                    thread.Join();

                    string w = DictionaryClass.Word;
                    if (w.Length > 0)
                    {
                        uset9 = false;
                        AddText(w.Substring(thisWordStart.Length),caretIdx,false);
                    }
                }
            }            
        }
        
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            tbScreen.Focus();
            btnAdd.IsEnabled = false;
            DictionaryClass.DictionaryInit();
            letter = (char)0;           
        }        
    }
}
