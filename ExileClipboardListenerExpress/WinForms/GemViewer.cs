using System;
using System.Text;
using System.Windows.Forms;
using ExileClipboardListener.Classes;

namespace ExileClipboardListener.WinForms
{
    public partial class GemViewer : Form
    {
        private int _leagueId;

        public GemViewer()
        {
            InitializeComponent();
        }

        private void GemViewer_Load(object sender, EventArgs e)
        {
            GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT LeagueName FROM League ORDER BY 1;", League);
            League.SelectedIndex = 0;

            //Make a list of all the gem types
            GlobalMethods.GetGemTypes(GemType);
            GemType.SelectedIndex = 0;
        }

        private void Filter_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            string sql = "SELECT g.GemStashId,";
            if (League.Text == "(All)")
                sql += "l.LeagueName AS [League],";
            sql += @"
                    g.Name AS [Gem Name],
                    CAST(g.Level AS INTEGER) AS Level,
                    CAST(g.Quality AS INTEGER) AS Quality,
                    CAST(g.ReqLevel AS INTEGER) AS [Req Level],
                    CAST(g.ReqStr AS INTEGER) AS [Req Str],
                    CAST(g.ReqDex AS INTEGER) AS [Req Dex],
                    CAST(g.ReqInt AS INTEGER) AS [Req Int],
                    CAST(g.ManaCost AS INTEGER) AS [Mana Cost],
                    CAST(g.ManaMultiplier AS INTEGER) AS [Mana Multiplier],
                    CAST(g.ManaReserved AS INTEGER) AS [Mana Reserved],
                    g.Type
                FROM
                    GemStash g
                    INNER JOIN League l ON l.LeagueId = g.LeagueId";
            bool where = false;
            if (_leagueId != 0)
            {
                sql += " WHERE l.LeagueId = '" + _leagueId + "' ";
                where = true;
            }
            if (GemType.Text != "(All)")
            {
                sql += (where ? " AND " : " WHERE ") + "g.Type LIKE '%" + GemType.Text.Replace("'", "''") + "%';"; 
            }
            GemCount.Text = GlobalMethods.StuffGrid(sql, GemGrid, true, true).ToString("#,##0") + " gems";
        
            //Hide the StashId, it's just a number
            GemGrid.Columns[0].Visible = false;
        }

        private void League_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Determine the LeagueId
            _leagueId = League.Text == "(All)" ? 0 : GlobalMethods.GetScalarInt("SELECT LeagueId FROM League WHERE LeagueName = '" + League.Text + "';");
        }

        private void ViewScript_Click(object sender, EventArgs e)
        {
            if (GemGrid.CurrentRow == null)
                return;
            string item = GlobalMethods.GetScalarString("SELECT OriginalText FROM GemStash WHERE GemStashId = " + GemGrid.CurrentRow.Cells[0].Value + ";");
            var sv = new ScriptViewer();
            sv.ItemScript.Text = item;
            sv.ShowDialog();
        }
    }
}
