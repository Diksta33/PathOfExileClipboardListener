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
    }
}
