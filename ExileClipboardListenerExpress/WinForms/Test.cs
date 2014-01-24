using System;
using System.Windows.Forms;

namespace ExileClipboardListener.WinForms
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            progressBar1.Value = Convert.ToInt32(textBox1.Text);
            newProgressBar1.Value = Convert.ToInt32(textBox1.Text);
        }
    }
}
