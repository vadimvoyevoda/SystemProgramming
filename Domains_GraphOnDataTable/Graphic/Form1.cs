using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphic
{
    public partial class Form1 : Form
    {
        List<string> _names;
        List<float> _inputs;
        List<float> _outputs;
        float scaleX, scaleY;

        public Form1()
        {
            InitializeComponent();

            _names = new List<string>();
            //_names.Add("GraphName");
            //_names.Add("Scale X");
            //_names.Add("Scale Y");

            _inputs = new List<float>();
            //_inputs.Add(0);
            //_inputs.Add(1);
            //_inputs.Add(2);
            //_inputs.Add(3);
            //_inputs.Add(4);
            //_inputs.Add(5);

            _outputs = new List<float>();
            //_outputs.Add(0); 
            //_outputs.Add(1);
            //_outputs.Add(4);
            //_outputs.Add(9);
            //_outputs.Add(16);
            //_outputs.Add(25);

            //btnDraw.Enabled = true;
            btnDraw.Enabled = false;
        }

        public void SetData(List<string> names, List<float> inputs, List<float> outputs)
        {
            _names = names;
            _inputs = inputs;
            _outputs = outputs;

            if (_names.Count == 3 && _inputs.Count > 0 && _outputs.Count > 0)
            {
                btnDraw.Enabled = true;
            }
        }

        private void btnDraw_Click(object sender, EventArgs e)
        {            
            Graphics g = pnlGraph.CreateGraphics();
            Pen pen = new Pen(Color.Blue, 3);

            tbName.Text = _names[0];
            SetScales();
            OsDrawing(g);
            
            GraphicsPath gPath = new GraphicsPath();
            PointF[] points = null;
            int num = _inputs.Count % 3;

            switch (num)
            {
                case 0: points = new PointF[_inputs.Count+1];
                    for (int i = 0; i < _inputs.Count; i++)
                    {
                        points[i] = new PointF(30 + _inputs[i] * scaleX, (pnlGraph.Height - 30) - _outputs[i] * scaleY);
                    }
                    points[_inputs.Count] = new PointF(30 + _inputs[_inputs.Count - 1] * scaleX, (pnlGraph.Height - 30) - _outputs[_inputs.Count - 1] * scaleY);
                    break;
                case 1: points = new PointF[_inputs.Count];
                    for (int i = 0; i < _inputs.Count; i++)
                    {
                        points[i] = new PointF(30 + _inputs[i] * scaleX, (pnlGraph.Height - 30) - _outputs[i] * scaleY);
                    }
                    break;
                case 2: points = new PointF[_inputs.Count+2];
                    for (int i = 0; i < _inputs.Count; i++)
                    {
                        points[i] = new PointF(30 + _inputs[i] * scaleX, (pnlGraph.Height - 30) - _outputs[i] * scaleY);
                    }
                    points[_inputs.Count] = new PointF(30 + _inputs[_inputs.Count - 1] * scaleX, (pnlGraph.Height - 30) - _outputs[_inputs.Count - 1] * scaleY);
                    points[_inputs.Count+1] = new PointF(30 + _inputs[_inputs.Count - 1] * scaleX, (pnlGraph.Height - 30) - _outputs[_inputs.Count - 1] * scaleY);
                   

                    break;
                default: break;
            }
            
            gPath.AddBeziers(points);

            g.DrawPath(pen, gPath);
        }

        void OsDrawing(Graphics g)
        {
            Pen black = new Pen(Color.Black,1);
            g.DrawLine(black, 30, 30, 30, pnlGraph.Height - 30);
            g.DrawLine(black, 30, 30, 25, 40);
            g.DrawLine(black, 30, 30, 35, 40);
            g.DrawLine(black, 30, pnlGraph.Height-30, pnlGraph.Width-20, pnlGraph.Height - 30);
            g.DrawLine(black, pnlGraph.Width - 30, pnlGraph.Height - 35, pnlGraph.Width - 20, pnlGraph.Height - 30);
            g.DrawLine(black, pnlGraph.Width - 30, pnlGraph.Height - 25, pnlGraph.Width - 20, pnlGraph.Height - 30);

            g.DrawString(_names[1],
                new Font("Times New Roman", 14),
                new SolidBrush(Color.Black),
                new PointF(20, 10));

            g.DrawString(_names[2],
                new Font("Times New Roman", 14),
                new SolidBrush(Color.Black),
                new PointF(pnlGraph.Width - 100, pnlGraph.Height-20));

            g.DrawString("0",
                new Font("Times New Roman", 14),
                new SolidBrush(Color.Black),
                new PointF(20, pnlGraph.Height - 20));
        }

        void SetScales()
        {
            scaleX = (pnlGraph.Width - 35 - 30) / _inputs.Max();
            scaleY = (pnlGraph.Height - 30 - 35) / _outputs.Max();
        }
                
    }
}
