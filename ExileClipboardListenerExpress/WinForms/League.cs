using System;
using System.Windows.Forms;
using ExileClipboardListener.Classes;

namespace ExileClipboardListener.WinForms
{
    public partial class League : Form
    {
        private bool _adding;
        private bool _editing;
        private int _leagueId;

        public League()
        {
            InitializeComponent();
        }

        private void League_Load(object sender, EventArgs e)
        {
            GlobalMethods.StuffCombo("SELECT LeagueName FROM League UNION ALL SELECT '' ORDER BY 1;", LeagueParent);
            RefreshLeagueGrid();
        }

        private void RefreshLeagueGrid()
        {
            int currentRow = 0;
            if (LeagueGrid.CurrentRow != null)
                currentRow = LeagueGrid.CurrentRow.Index;
            GlobalMethods.StuffGrid("SELECT l.LeagueName AS [League Name], pl.LeagueName AS [Parent League Name], sc.StashCount AS [Stashed Items], gc.GemCount AS [Stashed Gems], cc.CurrencyCount AS [Stashed Currency], mc.MapCount AS [Map Count] FROM League l LEFT JOIN League pl ON pl.LeagueId = l.LeagueParentId LEFT JOIN (SELECT LeagueId, COUNT(*) AS StashCount FROM Stash GROUP BY LeagueId) sc ON sc.LeagueId = l.LeagueId LEFT JOIN (SELECT LeagueId, COUNT(*) AS GemCount FROM GemStash GROUP BY LeagueId) gc ON gc.LeagueId = l.LeagueId LEFT JOIN (SELECT LeagueId, COUNT(*) AS CurrencyCount FROM CurrencyStash GROUP BY LeagueId) cc ON cc.LeagueId = l.LeagueId LEFT JOIN (SELECT LeagueId, COUNT(*) AS MapCount FROM MapStash GROUP BY LeagueId) mc ON mc.LeagueId = l.LeagueId;", LeagueGrid);
            if (currentRow != 0)
            {
                if (currentRow >= LeagueGrid.Rows.Count)
                    currentRow = LeagueGrid.Rows.Count - 1;
                LeagueGrid.CurrentCell = LeagueGrid.Rows[currentRow].Cells[1];
                LeagueGrid_SelectionChanged(null, null);
            }
            RefreshButtons();
        }

        private void RefreshButtons()
        {
            LeagueName.Enabled = _adding || _editing;
            LeagueParent.Enabled = _adding || _editing;
            LeagueGrid.Enabled = !_adding && !_editing;
            NewLeague.Enabled = !_adding && !_editing;
            EditLeague.Enabled = !_adding && !_editing && LeagueGrid.Rows.Count > 0; 
            CancelLeague.Enabled = _adding || _editing;
            DeleteLeague.Enabled = !_adding && !_editing && LeagueGrid.Rows.Count > 0; 
            SaveLeague.Enabled = _adding || _editing;
            LeagueDefault.Enabled = !_adding && !_editing && LeagueGrid.Rows.Count > 0;
            MergeStash.Enabled = !_adding && !_editing && LeagueGrid.Rows.Count > 0 && LeagueParent.Text != ""; //Must have a parent
            ClearStash.Enabled = !_adding && !_editing && LeagueGrid.Rows.Count > 0;
        }

        private void EditLeague_Click(object sender, EventArgs e)
        {
            _editing = true;
            RefreshButtons();
        }

        private void LeagueGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (LeagueGrid.CurrentRow == null)
                return;

            //Copy the details down
            LeagueName.Text = LeagueGrid.CurrentRow.Cells[0].Value.ToString();
            LeagueParent.Text = LeagueGrid.CurrentRow.Cells[1].Value.ToString();
            _leagueId = GlobalMethods.GetScalarInt("SELECT LeagueId FROM League WHERE LeagueName = '" + LeagueName.Text + "';");
            RefreshButtons();
        }

        private void NewLeague_Click(object sender, EventArgs e)
        {
            _adding = true;
            RefreshButtons();
        }

        private void CancelLeague_Click(object sender, EventArgs e)
        {
            _adding = false;
            _editing = false;
            RefreshButtons();
        }

        private void DeleteLeague_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirm Delete", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            GlobalMethods.RunQuery("DELETE FROM League WHERE LeagueId = " + _leagueId + ";");
            MessageBox.Show("Success!");
            RefreshLeagueGrid();
        }

        private void SaveLeague_Click(object sender, EventArgs e)
        {
            int parentId = LeagueParent.Text == "" ? 0 : GlobalMethods.GetScalarInt("SELECT LeagueId FROM League WHERE LeagueName = '" + LeagueParent.Text + "';");
            if (_editing)
                GlobalMethods.RunQuery("UPDATE League SET LeagueName = '" + LeagueName.Text + "', LeagueParentId = " + (parentId == 0 ? "NULL" : parentId.ToString()) + " WHERE LeagueId = " + _leagueId + ";");
            else
                GlobalMethods.RunQuery("INSERT INTO League(LeagueName, LeagueParentId) VALUES('" + LeagueName.Text + "'," + (parentId == 0 ? "NULL" : parentId.ToString()) + ");");
            MessageBox.Show("Success!");
            RefreshLeagueGrid();
        }

        private void LeagueDefault_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DefaultLeagueId = _leagueId;
            MessageBox.Show("Success!");
        }

        private void ClearStash_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirm Delete", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            GlobalMethods.RunQuery("DELETE FROM StashMod WHERE EXISTS (SELECT * FROM Stash s WHERE s.LeagueId = " + _leagueId + " AND s.StashId = StashMod.StashId);");
            GlobalMethods.RunQuery("DELETE FROM Stash WHERE LeagueId = " + _leagueId + ";");
            GlobalMethods.RunQuery("DELETE FROM GemStash WHERE LeagueId = " + _leagueId + ";");
            GlobalMethods.RunQuery("DELETE FROM CurrencyStash WHERE LeagueId = " + _leagueId + ";");
            GlobalMethods.RunQuery("DELETE FROM MapStash WHERE LeagueId = " + _leagueId + ";");
            RefreshLeagueGrid();
        }

        private void MergeStash_Click(object sender, EventArgs e)
        {
            int parentId = LeagueParent.Text == "" ? 0 : GlobalMethods.GetScalarInt("SELECT LeagueId FROM League WHERE LeagueName = '" + LeagueParent.Text + "';");
            if (parentId == 0)
            {
                MessageBox.Show("You can't merge a league that doesn't have a parent to merge into");
                return;
            }
            if (MessageBox.Show("Are you sure?", "Confirm Merge", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            GlobalMethods.RunQuery("UPDATE Stash SET LeagueId = " + parentId + " WHERE LeagueId = " + _leagueId + ";");
            GlobalMethods.RunQuery("UPDATE GemStash SET LeagueId = " + parentId + " WHERE LeagueId = " + _leagueId + ";");
            GlobalMethods.RunQuery("UPDATE CurrencyStash SET LeagueId = " + parentId + " WHERE LeagueId = " + _leagueId + ";");
            GlobalMethods.RunQuery("UPDATE MapStash SET LeagueId = " + parentId + " WHERE LeagueId = " + _leagueId + ";");
            RefreshLeagueGrid();
        }
    }
}
