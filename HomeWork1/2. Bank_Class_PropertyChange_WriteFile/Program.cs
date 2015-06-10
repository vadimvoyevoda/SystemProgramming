using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2.Bank_Class_PropertyChange_WriteFile
{
    class Bank : INotifyPropertyChanged
    {
        int _money;
        string _name;
        int _percent;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Money
        {
            get { return _money; }
            set 
            {
                if (value != _money)
                {
                    _money = value;
                    OnPropertyChanged("money");
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("name");
                }
            }
        }

        public int Percent
        {
            get { return _percent; }
            set
            {
                if (value != _percent)
                {
                    _percent = value;
                    OnPropertyChanged("percent");
                }
            }
        }

        public Bank(int money, string name, int percent)
        {
            _money = money;
            _name = name;
            _percent = percent;

            SaveFile();
            PropertyChanged += Bank_PropertyChanged;
        }
                
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Bank_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ThreadStart start = new ThreadStart(SaveFile);
            Thread thread = new Thread(start);

            thread.Start();
        }

        private void SaveFile()
        {
            using (StreamWriter sw = new StreamWriter("BankInfo.txt"))
            {
                sw.WriteLine(Name);
                sw.WriteLine(Money);
                sw.WriteLine(Percent);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Bank bank = new Bank(10000, "StarBank", 21);
            bank.Money += 4000;
            bank.Name = "NewName";
            bank.Money += 4000;
        }
    }
}
