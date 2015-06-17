using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataForCalculate
{
    public partial class Form1 : Form
    {
        Module factMod { get; set; }
        object Fact;

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(Module fact, object TargetWnd)
        {
            factMod = fact;
            Fact = TargetWnd;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            factMod.GetType("FactorialRes.Form1").GetMethod("FactorialCalculate").Invoke(Fact,
                new object[] { tbNumber.Text });
        }
    }
}
