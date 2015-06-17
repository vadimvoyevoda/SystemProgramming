using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FactorialRes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }
        
        public void FactorialCalculate(string num)
        {
            int number;

            if (Int32.TryParse(num, out number))
            {
                long fact = 1;

                for (int i = 2; i <= number; i++)
                {
                    fact *= i;
                }

                tbFactorial.Text = fact.ToString();
            }
            else 
            {
                tbFactorial.Text = "Error Number";
            }
            
        }
    }
}
