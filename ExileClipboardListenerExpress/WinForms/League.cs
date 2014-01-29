using System;
using System.Windows.Forms;
using ExileClipboardListener.Classes;

namespace ExileClipboardListener.WinForms
{
    public partial class League : Form
    {
        public League()
        {
            InitializeComponent();
        }

        private void League_Load(object sender, EventArgs e)
        {
            RefreshLeagueGrid();
        }

        private void RefreshLeagueGrid()
        {
            GlobalMethods.StuffGrid("SELECT l.LeagueName AS [League Name], pl.LeagueName AS [Parent League Name], l.LeagueStartDate AS [Start Date], l.LeagueEndDate AS [End Date] FROM League l LEFT JOIN League pl ON pl.LeagueId = l.LeagueParentId;", LeagueGrid);
        }
    }
}
