using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
