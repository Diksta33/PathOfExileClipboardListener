using System;
using System.Windows.Forms;
using ExileClipboardListener.Classes;

namespace ExileClipboardListener.WinForms
{
    public partial class PopUpStashed : Form
    {
        public PopUpStashed()
        {
            InitializeComponent();
        }

        private void PopUpStashed_Load(object sender, EventArgs e)
        {
            timer1.Interval = Properties.Settings.Default.StashPopUpSeconds * 1000;
            timer1.Start();
        }

        private void Timer1Tick(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
