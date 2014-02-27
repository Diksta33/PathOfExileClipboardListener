using System;
using System.Windows.Forms;

namespace ExileClipboardListener.WinForms
{
    public partial class ScriptViewer : Form
    {
        public ScriptViewer()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, System.EventArgs e)
        {
            Hide();
        }

        private void ScriptViewer_Load(object sender, System.EventArgs e)
        {
            ItemScript.Text = "--------Script Viewer--------" + Environment.NewLine + ItemScript.Text + Environment.NewLine + "--------Script Viewer--------";
        }
    }
}
