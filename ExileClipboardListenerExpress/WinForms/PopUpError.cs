using System.Windows.Forms;

namespace ExileClipboardListener.WinForms
{
    public partial class PopUpError : Form
    {
        public PopUpError()
        {
            InitializeComponent();
        }

        public void Show(string errorText)
        {
            ErrorMessage.Text = errorText;
        }
    }
}
