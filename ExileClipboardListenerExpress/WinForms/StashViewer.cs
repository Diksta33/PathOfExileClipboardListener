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
                sql += "ss.Score AS [Filter Score],";
            if (League.Text == "(All)")
                sql += "l.LeagueName AS [League],";
            sql += @"
	                i.ItemTypeName AS [Item Type],
	                i2.ItemSubTypeName AS [Item Sub Type],
	                s.ItemName AS [Item Name],
	                b.ItemName AS [Base Item], 
	                r.RarityName AS [Rarity], 
	                CAST(s.Quality AS INTEGER) AS [Quality], 
	                CAST(s.ItemLevel AS INTEGER) AS [Item Level], 
	                CAST(s.ReqLevel AS INTEGER) AS [Req Level],";
            if (!CompactView.Checked)
                sql += @",
	                CAST(b.ReqLevel AS INTEGER) AS [Base Req Level],";
            if (ItemType.Text == "(All)" || ItemType.Text == "Weapons")
                sql+=@"
                    CAST(s.AttackSpeed AS NUMERIC(18,2)) AS [APS],
                    CAST(s.PhysicalDPS AS NUMERIC(18,2)) AS [pDPS],
                    CAST(s.ElementalDPS AS NUMERIC(18,2)) AS [eDPS],
                    CAST(s.TotalDPS AS NUMERIC(18,2)) AS [tDPS],";
            if (ItemType.Text == "(All)" || ItemType.Text == "Armour")
                sql += @"
                    CAST(s.Armour AS INTEGER) AS [Armour], 
	                CAST(s.Evasion AS INTEGER) AS [Evasion], 
	                CAST(s.EnergyShield AS INTEGER) AS [Energy Shield],";
            if (ItemType.Text == "(All)" || ItemType.Text == "Weapon" || ItemType.Text == "Armour")
                sql += @"            
	                CAST(s.SocketCount AS INTEGER) AS [Sockets],
	                CASE WHEN s.SocketMaxLink = 0 THEN NULL ELSE CAST(s.SocketMaxLink AS VARCHAR(1)) || 'L' END AS [Max Links],";
            sql += @"
	                CAST(s.Life AS INTEGER) AS [Life],
	                CAST(s.Mana AS INTEGER) AS [Mana],
	                CAST(s.FireRes AS INTEGER) AS [Fire Res],
	                CAST(s.ColdRes AS INTEGER) AS [Cold Res],
	                CAST(s.LightningRes AS INTEGER) AS [Lig Res],
	                CAST(s.FireRes + s.ColdRes + s.LightningRes AS INTEGER) AS [Elem Res],
	                CAST(s.ChaosRes AS INTEGER) AS [Chaos Res],
	                CAST(s.FireRes + s.ColdRes + s.LightningRes + s.ChaosRes AS INTEGER) AS [Total Res],";
            if (!CompactView.Checked)
                sql += @",
	                CAST(IFNULL(b.DamagePhysicalMin, 0) AS INTEGER) AS [Base Damage Physical Min],
	                CAST(IFNULL(b.DamagePhysicalMax, 0) AS INTEGER) AS [Base Damage Physical Max],
	                CAST(b.Armour AS INTEGER) AS [Base Armour],
	                CAST(b.Evasion AS INTEGER) AS [Base Evasion],
	                CAST(b.EnergyShield AS INTEGER) AS [Base Energy Shield],
	                CAST(b.ReqStr AS INTEGER) AS [Req Str], 
	                CAST(b.ReqDex AS INTEGER) AS [Req Dex], 
	                CAST(b.ReqInt AS INTEGER) AS [Req Int],
	                mi1.ModName AS [Implict Mod 1],
	                CAST(IFNULL(s.ImplicitMod1Value, 0) AS INTEGER) AS [Implict Mod 1 Value],
	                mi2.ModName AS [Implicit Mod 2],
	                CAST(IFNULL(s.ImplicitMod2Value, 0) AS INTEGER) AS [Implict Mod 2 Value],
                    m01.ModName AS [Mod 1 Name],
                    CAST(CASE WHEN sm.StashModId = 1 THEN sm.ModValue ELSE 0 END AS INTEGER) AS [Mod 1 Value],
                    m02.ModName AS [Mod 2 Name],
                    CAST(CASE WHEN sm.StashModId = 2 THEN sm.ModValue ELSE 0 END AS INTEGER) AS [Mod 2 Value],
                    m03.ModName AS [Mod 3 Name],
                    CAST(CASE WHEN sm.StashModId = 3 THEN sm.ModValue ELSE 0 END AS INTEGER) AS [Mod 3 Value],
                    m04.ModName AS [Mod 4 Name],
                    CAST(CASE WHEN sm.StashModId = 4 THEN sm.ModValue ELSE 0 END AS INTEGER) AS [Mod 4 Value],
                    m05.ModName AS [Mod 5 Name],
                    CAST(CASE WHEN sm.StashModId = 5 THEN sm.ModValue ELSE 0 END AS INTEGER) AS [Mod 5 Value],
                    m06.ModName AS [Mod 6 Name],
                    CAST(CASE WHEN sm.StashModId = 6 THEN sm.ModValue ELSE 0 END AS INTEGER) AS [Mod 6 Value],
                    m07.ModName AS [Mod 7 Name],
                    CAST(CASE WHEN sm.StashModId = 7 THEN sm.ModValue ELSE 0 END AS INTEGER) AS [Mod 7 Value],
                    m08.ModName AS [Mod 8 Name],
                    CAST(CASE WHEN sm.StashModId = 8 THEN sm.ModValue ELSE 0 END AS INTEGER) AS [Mod 8 Value],
                    m09.ModName AS [Mod 9 Name],
                    CAST(CASE WHEN sm.StashModId = 9 THEN sm.ModValue ELSE 0 END AS INTEGER) AS [Mod 9 Value],
                    m10.ModName AS [Mod 10 Name],
                    CAST(CASE WHEN sm.StashModId = 10 THEN sm.ModValue ELSE 0 END AS INTEGER) AS [Mod 10 Value],
                    m11.ModName AS [Mod 11 Name],
                    CAST(CASE WHEN sm.StashModId = 11 THEN sm.ModValue ELSE 0 END AS INTEGER) AS [Mod 11 Value],
                    m12.ModName AS [Mod 12 Name],
                    CAST(CASE WHEN sm.StashModId = 12 THEN sm.ModValue ELSE 0 END AS INTEGER) AS [Mod 12 Value],";
            sql += @"
                    CAST(s.FirstSeen AS INTEGER) AS [First Seen],
                    CAST(s.LastSeen AS INTEGER) AS [Last Seen],
                    s.Location";
            sql += @"
                FROM 
	                Stash s
                    INNER JOIN League l ON l.LeagueId = s.LeagueId
	                INNER JOIN BaseItem b ON b.BaseItemId = s.BaseItemId 
                    INNER JOIN ItemType i ON i.ItemTypeId = b.ItemTypeId 
	                INNER JOIN ItemSubType i2 ON i2.ItemTypeId = b.ItemTypeId AND i2.ItemSubTypeId = b.ItemSubTypeId 
	                INNER JOIN Rarity r ON r.RarityId = s.RarityId
                    LEFT JOIN [Mod] mi1 ON mi1.ModId = b.Mod1Id 
	                LEFT JOIN [Mod] mi2 ON mi2.ModId = b.Mod2Id
                    LEFT JOIN StashMod sm1 ON sm1.StashId = s.StashId AND sm1.StashModId = 1
                    LEFT JOIN [Mod] m01 ON m01.ModId = sm1.ModId
                    LEFT JOIN StashMod sm2 ON sm2.StashId = s.StashId AND sm2.StashModId = 2
                    LEFT JOIN [Mod] m02 ON m02.ModId = sm2.ModId
                    LEFT JOIN StashMod sm3 ON sm3.StashId = s.StashId AND sm3.StashModId = 3
                    LEFT JOIN [Mod] m03 ON m03.ModId = sm3.ModId
                    LEFT JOIN StashMod sm4 ON sm4.StashId = s.StashId AND sm4.StashModId = 4
                    LEFT JOIN [Mod] m04 ON m04.ModId = sm4.ModId
                    LEFT JOIN StashMod sm5 ON sm5.StashId = s.StashId AND sm5.StashModId = 5
                    LEFT JOIN [Mod] m05 ON m05.ModId = sm5.ModId
                    LEFT JOIN StashMod sm6 ON sm6.StashId = s.StashId AND sm6.StashModId = 6
                    LEFT JOIN [Mod] m06 ON m06.ModId = sm6.ModId
                    LEFT JOIN StashMod sm7 ON sm7.StashId = s.StashId AND sm7.StashModId = 7
                    LEFT JOIN [Mod] m07 ON m07.ModId = sm7.ModId
                    LEFT JOIN StashMod sm8 ON sm8.StashId = s.StashId AND sm8.StashModId = 8
                    LEFT JOIN [Mod] m08 ON m08.ModId = sm8.ModId
                    LEFT JOIN StashMod sm9 ON sm9.StashId = s.StashId AND sm9.StashModId = 9
                    LEFT JOIN [Mod] m09 ON m09.ModId = sm9.ModId
                    LEFT JOIN StashMod sm10 ON sm10.StashId = s.StashId AND sm10.StashModId = 10
                    LEFT JOIN [Mod] m10 ON m10.ModId = sm10.ModId
                    LEFT JOIN StashMod sm11 ON sm11.StashId = s.StashId AND sm11.StashModId = 11
                    LEFT JOIN [Mod] m11 ON m11.ModId = sm11.ModId
                    LEFT JOIN StashMod sm12 ON sm12.StashId = s.StashId AND sm12.StashModId = 12
                    LEFT JOIN [Mod] m12 ON m12.ModId = sm12.ModId";
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
                where = true;
            }

            //Some item types have category filters so we apply these here
            if (ItemCategory.Text == "Armour")
                sql += (where ? " AND " : " WHERE ") + " s.Armour != 0 AND s.Evasion = 0 AND s.EnergyShield = 0";
            if (ItemCategory.Text == "Evasion")
                sql += (where ? " AND " : " WHERE ") + " s.Armour = 0 AND s.Evasion != 0 AND s.EnergyShield = 0";
            if (ItemCategory.Text == "Energy Shield")
                sql += (where ? " AND " : " WHERE ") + " s.Armour = 0 AND s.Evasion = 0 AND s.EnergyShield != 0";
            if (ItemCategory.Text == "Armour/ Evasion")
                sql += (where ? " AND " : " WHERE ") + " s.Armour != 0 AND s.Evasion != 0 AND s.EnergyShield = 0";
            if (ItemCategory.Text == "Armour/ Energy Shield")
                sql += (where ? " AND " : " WHERE ") + " s.Armour != 0 AND s.Evasion = 0 AND s.EnergyShield != 0";
            if (ItemCategory.Text == "Evasion/ Energy Shield")
                sql += (where ? " AND " : " WHERE ") + " s.Armour = 0 AND s.Evasion != 0 AND s.EnergyShield != 0";
            if (ItemCategory.Text == "Rings & Amulets")
                sql += (where ? " AND " : " WHERE ") + " i2.ItemSubTypeName IN ('Ring', 'Amulet')";
            if (ItemCategory.Text == "1-Handed")
                sql += (where ? " AND " : " WHERE ") + " i2.ItemSubTypeName IN ('Claw', 'Bow', 'Dagger', 'One Hand Axe', 'One Hand Sword', 'One Hand Mace', 'Sceptre', 'Wand', 'Thrusting One Hand Sword')";
            if (ItemCategory.Text == "2-Handed")
                sql += (where ? " AND " : " WHERE ") + " i2.ItemSubTypeName IN ('Staff', 'Two Hand Axe', 'Two Hand Mace', 'Two Hand Sword')";

            //Run the query and count the items
            ItemCount.Text = GlobalMethods.StuffGrid(sql, StashGrid, true, true).ToString("#,##0") + " items";

            //Hide the StashId, it's just a number
            StashGrid.Columns[0].Visible = false;

            //Select the first row
            if (StashGrid.Rows.Count > 0)
                StashGrid.Rows[0].Cells[1].Selected = true;
            Cursor.Current = Cursors.Default;
        }

         private void ItemType_SelectedIndexChanged(object sender, EventArgs e)
         {
             //Determine the ItemType
             _itemTypeId = ItemType.Text == "(All)" ? 0 : GlobalMethods.GetScalarInt("SELECT ItemTypeId FROM ItemType WHERE ItemTypeName = '" + ItemType.Text + "';");

             //Store the current subtype
             string subType = ItemSubType.Items.Count == 0 ? "XXX" : ItemSubType.Text;

             GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT ItemSubTypeName FROM ItemSubType" + (_itemTypeId != 0 ? " WHERE ItemTypeId = " + _itemTypeId : "") + ";", ItemSubType);

             //We add some categories to make life easier
             ItemCategory.Items.Clear();
             if (ItemType.Text == "Armour")
             {
                 ItemCategory.Items.Add("(All)");
                 ItemCategory.Items.Add("Armour");
                 ItemCategory.Items.Add("Armour/ Energy Shield");
                 ItemCategory.Items.Add("Armour/ Evasion");
                 ItemCategory.Items.Add("Evasion");
                 ItemCategory.Items.Add("Evasion/ Energy Shield");
                 ItemCategory.Items.Add("Energy Shield");
                 ItemCategory.Enabled = true;
             }
             else if (ItemType.Text == "Weapons")
             {
                 ItemCategory.Items.Add("(All)");
                 ItemCategory.Items.Add("1-Handed");
                 ItemCategory.Items.Add("2-Handed");
                 ItemCategory.Enabled = true;
             }
             else if (ItemType.Text == "Jewellery")
             {
                 ItemCategory.Items.Add("(All)");
                 ItemCategory.Items.Add("Rings & Amulets");
                 ItemCategory.Enabled = true;
             }
             else
                 ItemCategory.Enabled = false;
             if (ItemCategory.Items.Count > 0)
                 ItemCategory.SelectedIndex = 0;
             if (ItemSubType.Items.Contains(subType))
                 ItemSubType.Text = subType;
             else
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
            {
                if (GlobalMethods.Mode == GlobalMethods.COLLECTION_MODE)
                    new ItemInformation { AllowStash = false }.ShowDialog();
                else
                    new CompactInformation { AllowStash = false }.ShowDialog();
            }
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
            var sv = new ScriptViewer {ItemScript = {Text = item}};
            sv.ShowDialog();
        }

        private void ReparseStash_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirm Re-Parse ENTIRE Stash", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            var items = GlobalMethods.StuffStringList("SELECT OriginalText FROM Stash;");
            int c = 0;
            foreach (var item in items)
            {
                ReparseLabel.Text = "Parsing item " + ++c + "...";
                Application.DoEvents();
                ParseItem.ParseStash(item);
            }
            MessageBox.Show("Reparse Complete");
            ReparseLabel.Text = "Ready";
        }
    }
}
