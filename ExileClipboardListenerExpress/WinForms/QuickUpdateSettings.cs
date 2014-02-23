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
            CurrencyTab1.Checked = CurrencyTab1Name.Text != "";
            CurrencyTab2Name.Text = Properties.Settings.Default.CurrencyTab2;
            CurrencyTab2.Checked = CurrencyTab2Name.Text != "";
            CurrencyTab3Name.Text = Properties.Settings.Default.CurrencyTab3;
            CurrencyTab3.Checked = CurrencyTab3Name.Text != "";
            MapTab1Name.Text = Properties.Settings.Default.MapTab1;
            MapTab1.Checked = MapTab1Name.Text != "";
            MapTab2Name.Text = Properties.Settings.Default.MapTab2;
            MapTab2.Checked = MapTab2Name.Text != "";
            MapTab3Name.Text = Properties.Settings.Default.MapTab3;
            MapTab3.Checked = MapTab3Name.Text != "";
            GemTab1Name.Text = Properties.Settings.Default.GemTab1;
            GemTab1.Checked = GemTab1Name.Text != "";
            GemTab2Name.Text = Properties.Settings.Default.GemTab2;
            GemTab2.Checked = GemTab2Name.Text != "";
            GemTab3Name.Text = Properties.Settings.Default.GemTab3;
            GemTab3.Checked = GemTab3Name.Text != "";
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
