using System;
using System.Linq;
using System.Windows.Forms;
using ExileClipboardListener.Classes;

namespace ExileClipboardListener.WinForms
{
    public partial class CurrencyViewer : Form
    {
        private int _leagueId;

        public CurrencyViewer()
        {
            InitializeComponent();
        }

        private void CurrencyViewer_Load(object sender, EventArgs e)
        {
            //Get the leagues
            GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT LeagueName FROM League ORDER BY 1;", League);
            League.SelectedIndex = 0;

            //Set dynamic tooltips
            var toolTip = new ToolTip {IsBalloon = false, ShowAlways = true};
            foreach (var currency in GlobalMethods.CurrencyCache)
            {
                var icon = (PictureBox)Controls.Find("Icon" + currency.CurrencyItemId, true).FirstOrDefault();
                if (icon != null) 
                    toolTip.SetToolTip(icon, currency.Description);
            }
        }

        private void League_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Determine the LeagueId
            _leagueId = League.Text == "(All)" ? 0 : GlobalMethods.GetScalarInt("SELECT LeagueId FROM League WHERE LeagueName = '" + League.Text + "';");
            RefreshForm();
        }

        private void RefreshForm()
        {
            for (int i = 1; i <= 24; i++)
            {
                var textBox = (TextBox)Controls.Find("Item" + i, true).FirstOrDefault();
                textBox.Text = "";
            }
            GlobalMethods.GetCurrencyCount(_leagueId);
            foreach (var currency in GlobalMethods.CurrentStashCurrency)
            {
                var textBox = (TextBox)Controls.Find("Item" + currency.CurrencyItemId, true).FirstOrDefault();
                textBox.Text = currency.StackSize.ToString("#,##0");
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
