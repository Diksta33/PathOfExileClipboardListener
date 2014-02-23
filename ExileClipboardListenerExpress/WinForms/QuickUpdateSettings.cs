using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExileClipboardListener.Classes;

namespace ExileClipboardListener.WinForms
{
    public partial class QuickUpdateSettings : Form
    {
        public QuickUpdateSettings()
        {
            InitializeComponent();
        }

        private void QuickUpdateSettings_Load(object sender, EventArgs e)
        {
            AllLeagues.Checked = Properties.Settings.Default.QuickLeagues == GlobalMethods.ALL_LEAGUES;
            DefaultLeague.Checked = Properties.Settings.Default.QuickLeagues == GlobalMethods.DEFAULT_LEAGUE;
            CurrencyTab1Name.Text = Properties.Settings.Default.CurrencyTab1;
            CurrencyTab2Name.Text = Properties.Settings.Default.CurrencyTab2;
            CurrencyTab3Name.Text = Properties.Settings.Default.CurrencyTab3;
            MapTab1Name.Text = Properties.Settings.Default.MapTab1;
            MapTab2Name.Text = Properties.Settings.Default.MapTab2;
            MapTab3Name.Text = Properties.Settings.Default.MapTab3;
            GemTab1Name.Text = Properties.Settings.Default.GemTab1;
            GemTab2Name.Text = Properties.Settings.Default.GemTab2;
            GemTab3Name.Text = Properties.Settings.Default.GemTab3;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.QuickLeagues = (AllLeagues.Checked ? GlobalMethods.ALL_LEAGUES : GlobalMethods.DEFAULT_LEAGUE);
            Properties.Settings.Default.CurrencyTab1 = CurrencyTab1Name.Text;
            Properties.Settings.Default.CurrencyTab2 = CurrencyTab2Name.Text;
            Properties.Settings.Default.CurrencyTab3 = CurrencyTab3Name.Text;
            Properties.Settings.Default.MapTab1 = MapTab1Name.Text;
            Properties.Settings.Default.MapTab2 = MapTab2Name.Text;
            Properties.Settings.Default.MapTab3 = MapTab3Name.Text;
            Properties.Settings.Default.GemTab1 = GemTab1Name.Text;
            Properties.Settings.Default.GemTab2 = GemTab2Name.Text;
            Properties.Settings.Default.GemTab3 = GemTab3Name.Text; 
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }
    }
}
