using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Knv.Instr.Tool.Controls
{
    public partial class ParamControl : UserControl
    {
        public string ParamValue
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
        public string ParamName
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public ParamControl()
        {
            InitializeComponent();
        }
    }
}
