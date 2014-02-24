using System;
using System.Windows.Forms;
using ExileClipboardListener.Classes;

namespace ExileClipboardListener.WinForms
{
    public partial class MapViewer : Form
    {
        private int _leagueId;

        public MapViewer()
        {
            InitializeComponent();
        }

        private void MapViewer_Load(object sender, EventArgs e)
        {
            GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT LeagueName FROM League ORDER BY 1;", League);
            League.SelectedIndex = 0;
            GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT DISTINCT MapLevel FROM MapStash ORDER BY 1;", MapLevel);
            MapLevel.SelectedIndex = 0;
        }

        private void League_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Determine the LeagueId
            _leagueId = League.Text == "(All)" ? 0 : GlobalMethods.GetScalarInt("SELECT LeagueId FROM League WHERE LeagueName = '" + League.Text + "';");
        }

        private void RefreshGrid()
        {
            string sql = "SELECT m.MapStashId,";
            if (League.Text == "(All)")
                sql += "l.LeagueName AS [League],";
            sql += @"
                    m.Name AS [Map Name],
                    CAST(m.MapLevel AS INTEGER) AS [Map Level],
                    r.RarityName AS [Rarity],
                    CAST(m.Quality AS INTEGER) AS [Quality],
                    CAST(m.ItemQuantity AS INTEGER) AS [Item Quantity],
                    CAST(m.ItemLevel AS INTEGER) AS [Item Level]
                FROM
                    MapStash m
                    INNER JOIN League l ON l.LeagueId = m.LeagueId
                    INNER JOIN Rarity r ON r.RarityId = m.RarityId";
            bool where = false;
            if (_leagueId != 0)
            {
                sql += " WHERE l.LeagueId = '" + _leagueId + "' ";
                where = true;
            }
            if (MapLevel.Text != "(All)")
            {
                sql += (where ? " AND " : " WHERE ") + "m.MapLevel = " + MapLevel.Text + ";"; 
            }

            MapCount.Text = GlobalMethods.StuffGrid(sql, MapGrid, true, true).ToString("#,##0") + " maps";

            //Hide the StashId, it's just a number
            MapGrid.Columns[0].Visible = false;

            //Select the first row
            if (MapGrid.Rows.Count > 0)
                MapGrid.Rows[0].Cells[1].Selected = true;
        }

        private void ViewScript_Click(object sender, EventArgs e)
        {
            if (MapGrid.CurrentRow == null)
                return;
            string item = GlobalMethods.GetScalarString("SELECT OriginalText FROM MapStash WHERE MapStashId = " + MapGrid.CurrentRow.Cells[0].Value + ";");
            var sv = new ScriptViewer {ItemScript = {Text = item}};
            sv.ShowDialog();
        }

        private void MapLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Filter_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }
    }
}
