using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Semka1GUI
{
    public partial class WindowOut : Form
    {
        public WindowOut(string textOut)
        {
            InitializeComponent();
            richTextBox1.Text = textOut;
        }
    }
}
