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

namespace DataTable
{
    public partial class Form1 : Form
    {
        Module DrawerModule { get; set; }
        object Drawer;
        System.Data.DataTable dt;
        List<float> inputs;
        List<float> outputs;

        public Form1(Module drawer, object TargetWindow)
        {
            DrawerModule = drawer;
            Drawer = TargetWindow;

            InitializeComponent();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            inputs = new List<float>();
            outputs = new List<float>();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            DrawerModule.GetType("Graphic.Form1").GetMethod("SetData").Invoke(Drawer,
               new object[] { new List<string>() { tbName.Text, tbInput.Text, tbOutput.Text }, inputs, outputs });
        }

        private void tbName_Click(object sender, EventArgs e)
        {
            tbName.Text = "";
        }

        private void tbInput_Click(object sender, EventArgs e)
        {
            tbInput.Text = "";
        }

        private void tbOutput_Click(object sender, EventArgs e)
        {
            tbOutput.Text = "";
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            dt = new System.Data.DataTable();

            dt.Columns.Add(tbInput.Text, typeof(double));
            dt.Columns.Add(tbOutput.Text, typeof(double));

            dt.NewRow();
            dgData.DataSource = dt;
                        
            dt.RowChanged += dt_RowChanged;
        }
               
        void dt_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            inputs.Clear();
            outputs.Clear();

            DataRowCollection rows = dt.Rows;
            foreach (DataRow el in rows)
            {
                inputs.Add(Convert.ToSingle(el.ItemArray[0]));
                outputs.Add(Convert.ToSingle(el.ItemArray[1]));
            }

            btnSend.Enabled = true;
        }
    }
}
