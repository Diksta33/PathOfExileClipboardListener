using System;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.IO;
using ExileClipboardListener.Classes;
using si = ExileClipboardListener.Classes.GlobalMethods.StashItem;

namespace ExileClipboardListener.WinForms
{
    public partial class StashViewer : Form
    {
        private int _itemTypeId;
        private int _itemSubTypeId;
        private int _leagueId;
        private int _modId;
        private int _filterId;

        public StashViewer()
        {
            InitializeComponent();
        }

        private void Stash_Load(object sender, EventArgs e)
        {
            GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT LeagueName FROM League ORDER BY 1;", League);
            League.SelectedIndex = 0;
            GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT ItemTypeName FROM ItemType ORDER BY 1;", ItemType);
            ItemType.SelectedIndex = 0;
            GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT ModName FROM [Mod] ORDER BY 1;", Mod);
            Mod.SelectedIndex = 0;
            GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT FilterName FROM FilterHeader ORDER BY 1;", FilterList);
            FilterList.SelectedIndex = 0;
            //RefreshGrid();
        }

        private void RefreshGrid()
        {
            //See if we need to include scores
            bool scored = false;
            Cursor.Current = Cursors.WaitCursor;
            if (FilterList.SelectedIndex != 0)
            {
                //Load each item into the current stash one at a time
                GlobalMethods.RunQuery("DELETE FROM StashScore;");
                int stashId = 0;
                while (stashId != -1)
                {
                    string sqlFilter = "SELECT MIN(s.StashId) FROM Stash s INNER JOIN BaseItem b ON b.BaseItemId = s.BaseItemId WHERE s.StashId > " + stashId;

                    //Don't score things that are hidden as that's a wasted effort
                    if (_leagueId != 0)
                    {
                        sqlFilter += " AND s.LeagueId = '" + _leagueId + "' ";
                    }
                    if (_itemTypeId != 0)
                    {
                        sqlFilter += " AND b.ItemTypeId = " + _itemTypeId + " ";
                    }
                    if (_itemSubTypeId != 0)
                    {
                        sqlFilter += " AND b.ItemSubTypeId = " + _itemSubTypeId + " ";
                    }
                    if (_modId != 0)
                    {
                        sqlFilter += " AND EXISTS (SELECT * FROM StashMod sm2 WHERE sm2.StashId = s.StashId AND sm2.ModId = " + _modId + ")";
                    }
                    stashId = GlobalMethods.GetScalarInt(sqlFilter);
                    if (stashId == 0)
                        break;
                    string item = GlobalMethods.GetScalarString("SELECT OriginalText FROM [Stash] WHERE StashId = " + stashId + ";");
                    ParseItem.ParseStash(item);
                    int score = new ItemInformation().ScoreFilter(_filterId);
                    GlobalMethods.RunQuery("INSERT INTO StashScore VALUES(" + stashId + "," + score + ");");
                }
                scored = true;
            }

            //Get a list of items based on the current filters
            string sql = "SELECT s.StashId AS [Stash Id],";
            if (scored)
                sql += "MAX(ss.Score) AS [Filter Score],";
            sql += @"
	                MAX(i.ItemTypeName) AS [Item Type],
	                MAX(i2.ItemSubTypeName) AS [Item Sub Type],
	                s.ItemName AS [Item Name],
	                MAX(b.ItemName) AS [Base Item], 
	                MAX(r.RarityName) AS [Rarity], 
	                CAST(MAX(s.Quality) AS INTEGER) AS [Quality], 
	                CAST(MAX(s.ItemLevel) AS INTEGER) AS [Item Level], 
	                CAST(MAX(b.ReqLevel) AS INTEGER) AS [Req Level],
                    CAST(MAX(s.AttackSpeed) AS NUMERIC(18,2)) AS [APS],
                    CAST(MAX(s.PhysicalDPS) AS NUMERIC(18,2)) AS [pDPS],
                    CAST(MAX(s.ElementalDPS) AS NUMERIC(18,2)) AS [eDPS],
                    CAST(MAX(s.TotalDPS) AS NUMERIC(18,2)) AS [tDPS],
	                CAST(MAX(s.Armour) AS INTEGER) AS [Armour], 
	                CAST(MAX(s.Evasion) AS INTEGER) AS [Evasion], 
	                CAST(MAX(s.EnergyShield) AS INTEGER) AS [Energy Shield]";
            if (!CompactView.Checked)
                sql += @",
	                CAST(MAX(IFNULL(b.DamagePhysicalMin, 0)) AS INTEGER) AS [Base Damage Physical Min],
	                CAST(MAX(IFNULL(b.DamagePhysicalMax, 0)) AS INTEGER) AS [Base Damage Physical Max],
	                CAST(MAX(b.Armour) AS INTEGER) AS [Base Armour],
	                CAST(MAX(b.Evasion) AS INTEGER) AS [Base Evasion],
	                CAST(MAX(b.EnergyShield) AS INTEGER) AS [Base Energy Shield],
	                CAST(MAX(b.ReqStr) AS INTEGER) AS [Req Str], 
	                CAST(MAX(b.ReqDex) AS INTEGER) AS [Req Dex], 
	                CAST(MAX(b.ReqInt) AS INTEGER) AS [Req Int],
	                MAX(mi1.ModName) AS [Implict Mod 1],
	                CAST(MAX(IFNULL(s.ImplicitMod1Value, 0)) AS INTEGER) AS [Implict Mod 1 Value],
	                MAX(mi2.ModName) AS [Implicit Mod 2],
	                CAST(MAX(IFNULL(s.ImplicitMod2Value, 0)) AS INTEGER) AS [Implict Mod 2 Value],
                    MAX(m01.ModName) AS [Mod 1 Name],
                    CAST(MAX(CASE WHEN sm.StashModId = 1 THEN sm.ModValue ELSE 0 END) AS INTEGER) AS [Mod 1 Value],
                    MAX(m02.ModName) AS [Mod 2 Name],
                    CAST(MAX(CASE WHEN sm.StashModId = 2 THEN sm.ModValue ELSE 0 END) AS INTEGER) AS [Mod 2 Value],
                    MAX(m03.ModName) AS [Mod 3 Name],
                    CAST(MAX(CASE WHEN sm.StashModId = 3 THEN sm.ModValue ELSE 0 END) AS INTEGER) AS [Mod 3 Value],
                    MAX(m04.ModName) AS [Mod 4 Name],
                    CAST(MAX(CASE WHEN sm.StashModId = 4 THEN sm.ModValue ELSE 0 END) AS INTEGER) AS [Mod 4 Value],
                    MAX(m05.ModName) AS [Mod 5 Name],
                    CAST(MAX(CASE WHEN sm.StashModId = 5 THEN sm.ModValue ELSE 0 END) AS INTEGER) AS [Mod 5 Value],
                    MAX(m06.ModName) AS [Mod 6 Name],
                    CAST(MAX(CASE WHEN sm.StashModId = 6 THEN sm.ModValue ELSE 0 END) AS INTEGER) AS [Mod 6 Value],
                    MAX(m07.ModName) AS [Mod 7 Name],
                    CAST(MAX(CASE WHEN sm.StashModId = 7 THEN sm.ModValue ELSE 0 END) AS INTEGER) AS [Mod 7 Value],
                    MAX(m08.ModName) AS [Mod 8 Name],
                    CAST(MAX(CASE WHEN sm.StashModId = 8 THEN sm.ModValue ELSE 0 END) AS INTEGER) AS [Mod 8 Value],
                    MAX(m09.ModName) AS [Mod 9 Name],
                    CAST(MAX(CASE WHEN sm.StashModId = 9 THEN sm.ModValue ELSE 0 END) AS INTEGER) AS [Mod 9 Value],
                    MAX(m10.ModName) AS [Mod 10 Name],
                    CAST(MAX(CASE WHEN sm.StashModId = 10 THEN sm.ModValue ELSE 0 END) AS INTEGER) AS [Mod 10 Value],
                    MAX(m11.ModName) AS [Mod 11 Name],
                    CAST(MAX(CASE WHEN sm.StashModId = 11 THEN sm.ModValue ELSE 0 END) AS INTEGER) AS [Mod 11 Value],
                    MAX(m12.ModName) AS [Mod 12 Name],
                    CAST(MAX(CASE WHEN sm.StashModId = 12 THEN sm.ModValue ELSE 0 END) AS INTEGER) AS [Mod 12 Value]";
            sql += @"
                FROM 
	                Stash s 
	                INNER JOIN BaseItem b ON b.BaseItemId = s.BaseItemId 
                    INNER JOIN ItemType i ON i.ItemTypeId = b.ItemTypeId 
	                INNER JOIN ItemSubType i2 ON i2.ItemTypeId = b.ItemTypeId AND i2.ItemSubTypeId = b.ItemSubTypeId 
	                INNER JOIN Rarity r ON r.RarityId = s.RarityId
                    LEFT JOIN StashMod sm ON sm.StashId = s.StashId
                    LEFT JOIN [Mod] mi1 ON mi1.ModId = b.Mod1Id 
	                LEFT JOIN [Mod] mi2 ON mi2.ModId = b.Mod2Id
                    LEFT JOIN [Mod] m01 ON m02.ModId = sm.ModId AND sm.StashModId = 1
                    LEFT JOIN [Mod] m02 ON m01.ModId = sm.ModId AND sm.StashModId = 2
                    LEFT JOIN [Mod] m03 ON m03.ModId = sm.ModId AND sm.StashModId = 3
                    LEFT JOIN [Mod] m04 ON m04.ModId = sm.ModId AND sm.StashModId = 4
                    LEFT JOIN [Mod] m05 ON m05.ModId = sm.ModId AND sm.StashModId = 5
                    LEFT JOIN [Mod] m06 ON m06.ModId = sm.ModId AND sm.StashModId = 6
                    LEFT JOIN [Mod] m07 ON m07.ModId = sm.ModId AND sm.StashModId = 7
                    LEFT JOIN [Mod] m08 ON m08.ModId = sm.ModId AND sm.StashModId = 8
                    LEFT JOIN [Mod] m09 ON m09.ModId = sm.ModId AND sm.StashModId = 9
                    LEFT JOIN [Mod] m10 ON m10.ModId = sm.ModId AND sm.StashModId = 10
                    LEFT JOIN [Mod] m11 ON m11.ModId = sm.ModId AND sm.StashModId = 11
                    LEFT JOIN [Mod] m12 ON m12.ModId = sm.ModId AND sm.StashModId = 12";
            if (scored)
                sql += " LEFT JOIN StashScore ss ON ss.StashId = s.StashId";

            //Add filters
            bool where = false;
            if (_leagueId != 0)
            {
                sql += " WHERE s.LeagueId = '" + _leagueId + "' ";
                where = true;
            }
            if (_itemTypeId != 0)
            {
                sql += (where ? " AND " : " WHERE ") + " i.ItemTypeId = " + _itemTypeId + " ";
                where = true;
            }
            if (_itemSubTypeId != 0)
            {
                sql += (where ? " AND " : " WHERE ") + " i2.ItemSubTypeId = " + _itemSubTypeId + " ";
                where = true;
            }
            if (_modId != 0)
            {
                sql += (where ? " AND " : " WHERE ") + " EXISTS (SELECT * FROM StashMod sm2 WHERE sm2.StashId = s.StashId AND sm2.ModId = " + _modId+ ")";
                where = true;
            }
            if (MinItemLevel.Value > 1)
            {
                sql += (where ? " AND " : " WHERE ") + " s.ItemLevel >= " + MinItemLevel.Value + " ";
                where = true;
            }
            if (MaxItemLevel.Value > 1)
            {
                sql += (where ? " AND " : " WHERE ") + " s.ItemLevel <= " + MaxItemLevel.Value + " ";
                where = true;
            }
            if (MinReqLevel.Value > 1)
            {
                sql += (where ? " AND " : " WHERE ") + " s.ReqLevel >= " + MinReqLevel.Value + " ";
                where = true;
            }
            if (MaxReqLevel.Value > 1)
            {
                sql += (where ? " AND " : " WHERE ") + " s.ReqLevel <= " + MaxReqLevel.Value + " ";
                where = true;
            }
            if (scored && HideZeroScores.Checked)
            {
                sql += (where ? " AND " : " WHERE ") + " IFNULL(ss.Score, 0) > 0";
            }
            sql += " GROUP BY s.StashId, s.ItemName;";
            ItemCount.Text = GlobalMethods.StuffGrid(sql, StashGrid, true, true).ToString("#,##0") + " items";

            //Hide the StashId, it's just a number
            StashGrid.Columns[0].Visible = false;
            Cursor.Current = Cursors.Default;
        }

        private void ItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Determine the ItemType
            _itemTypeId = ItemType.Text == "(All)" ? 0 : GlobalMethods.GetScalarInt("SELECT ItemTypeId FROM ItemType WHERE ItemTypeName = '" + ItemType.Text + "';");
            GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT ItemSubTypeName FROM ItemSubType" + (_itemTypeId != 0 ? " WHERE ItemTypeId = " + _itemTypeId : "") + ";", ItemSubType);
            ItemSubType.SelectedIndex = 0;
        }

        private void ItemSubType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Determine the ItemSubType
            _itemSubTypeId = ItemSubType.Text == "(All)" ? 0 : GlobalMethods.GetScalarInt("SELECT ItemSubTypeId FROM ItemSubType WHERE ItemSubTypeName = '" + ItemSubType.Text + "';");
        }

        private void Mod_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Determine the ModId
            _modId = Mod.Text == "(All)" ? 0 : GlobalMethods.GetScalarInt("SELECT ModId FROM [Mod] WHERE ModName = '" + Mod.Text + "';");
        }

        private void Export_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            DialogResult dr = saveFileDialog1.ShowDialog();
            if (dr == DialogResult.Cancel)
                return;
            string path = saveFileDialog1.FileName;

            //Get the headers
            var headers = StashGrid.Columns.Cast<DataGridViewColumn>();
            sb.AppendLine(string.Join(",", headers.Select(column => "\"" + column.HeaderText + "\"")));

            //Get the rows
            foreach (DataGridViewRow row in StashGrid.Rows)
            {
                var cells = row.Cells.Cast<DataGridViewCell>();
                sb.AppendLine(string.Join(",", cells.Select(cell => "\"" + cell.Value + "\"")));
            }

            //Write the file
            var file = new StreamWriter(path);
            file.WriteLine(sb);
            file.Close();
        }

        private void League_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Determine the LeagueId
            _leagueId = League.Text == "(All)" ? 0 : GlobalMethods.GetScalarInt("SELECT LeagueId FROM League WHERE LeagueName = '" + League.Text + "';");
        }

        private void FilterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Determine the filter
            _filterId = GlobalMethods.GetScalarInt("SELECT FilterId FROM FilterHeader WHERE FilterName = '" + FilterList.Text + "';");
        }

        private void Filter_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void ViewItem_Click(object sender, EventArgs e)
        {
            if (StashGrid.CurrentRow == null)
                return;
            string item = GlobalMethods.GetScalarString("SELECT OriginalText FROM [Stash] WHERE StashId = " + StashGrid.CurrentRow.Cells[0].Value + ";");
            if (ParseItem.ParseStash(item))
                new ItemInformation { AllowStash = false }.ShowDialog();
        }

        private void MaxItemLevel_ValueChanged(object sender, EventArgs e)
        {
            if (MinItemLevel.Value > MaxItemLevel.Value)
                MinItemLevel.Value = MaxItemLevel.Value;
        }

        private void MinItemLevel_ValueChanged(object sender, EventArgs e)
        {
            if (MinItemLevel.Value > MaxItemLevel.Value)
                MaxItemLevel.Value = MinItemLevel.Value;
        }

        private void DeleteItem_Click(object sender, EventArgs e)
        {
            if (StashGrid.CurrentRow == null)
                return;
            int stashId = Convert.ToInt32(StashGrid.CurrentRow.Cells[0].Value);
            if (MessageBox.Show("Are you sure?", "Confirm Delete", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            GlobalMethods.RunQuery("DELETE FROM Stash WHERE StashId = " + stashId);
            RefreshGrid();
        }

        private void ViewScriptClick(object sender, EventArgs e)
        {
            if (StashGrid.CurrentRow == null)
                return;
            string item = GlobalMethods.GetScalarString("SELECT OriginalText FROM [Stash] WHERE StashId = " + StashGrid.CurrentRow.Cells[0].Value + ";");
            var sv = new ScriptViewer();
            sv.ItemScript.Text = item;
            sv.ShowDialog();
        }
    }
}
