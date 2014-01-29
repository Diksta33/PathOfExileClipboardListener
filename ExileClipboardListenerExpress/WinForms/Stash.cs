using System;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.IO;
using ExileClipboardListener.Classes;

namespace ExileClipboardListener.WinForms
{
    public partial class Stash : Form
    {
        private int _itemTypeId;
        private int _itemSubTypeId;
        private Guid _leagueId;
        private int _modId;
        private bool _refresh;

        public Stash()
        {
            InitializeComponent();
        }

        private void Stash_Load(object sender, EventArgs e)
        {
            ////Perform a SQL Query into SQLite
            //using (var conSS = new SqlConnection("Server=localhost\\EXILE;Database=PathOfExile;Trusted_Connection=yes;"))
            //{
            //    conSS.Open();
            //    using (var conSL = new SQLiteConnection(GlobalMethods.Connection))
            //    {
            //        conSL.Open();
            //        using (var comSS = conSS.CreateCommand())
            //        {
            //            using (var comSL = conSL.CreateCommand())
            //            {
            //                comSS.CommandText = "SELECT * FROM [BaseItem];";
            //                using (var drSS = comSS.ExecuteReader())
            //                {
            //                    while (drSS.Read())
            //                    {
            //                        string sql = "INSERT INTO [BaseItem] VALUES(";
            //                        for (int i = 0; i < drSS.FieldCount; i++)
            //                        {
            //                            if (drSS[i] == DBNull.Value)
            //                                sql += "NULL";
            //                            else if (drSS[i].GetType() == typeof(System.String))
            //                                sql += "'" + drSS[i].ToString().Replace("'", "''") + "'";
            //                            else
            //                                sql += drSS[i].ToString();
            //                            if (i < drSS.FieldCount - 1)
            //                                sql += ",";
            //                        }
            //                        sql += ")";
            //                        comSL.CommandText = sql;
            //                        comSL.ExecuteNonQuery();
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            _refresh = false;
            GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT LeagueName FROM League ORDER BY 1;", League);
            League.SelectedIndex = 0;
            GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT ItemTypeName FROM ItemType ORDER BY 1;", ItemType);
            ItemType.SelectedIndex = 0;
            GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT ModName FROM [Mod] ORDER BY 1;", Mod);
            Mod.SelectedIndex = 0;
            _refresh = true;
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            if (!_refresh)
                return;
            string sql = @"
                SELECT 
	                i.ItemTypeName,  
	                i2.ItemSubTypeName, 
	                s.ItemName, 
	                b.ItemName, 
	                r.RarityName, 
	                s.Quality, 
	                s.ItemLevel, 
	                b.ReqLevel, 
	                b.ReqStr, 
	                b.ReqDex, 
	                b.ReqInt,
	                b.Armour AS BaseArmour,
	                b.Evasion AS BaseEvasion,
	                b.EnergyShield AS BaseEnergyShield,
	                s.Armour, 
	                s.Evasion, 
	                s.EnergyShield, 	
	                b.DamagePhysicalMin AS BaseDamagePhysicalMin,
	                b.DamagePhysicalMax AS BaseDamagePhysicalMax,
	                mi1.ModName AS ImplictMod1,
	                s.ImplicitMod1Value,
	                mi2.ModName AS ImplicitMod2,
	                s.ImplicitMod2Value,
	                mp11.ModName AS Prefix1Mod1,
	                s.Prefix1Mod1Value,
	                mp12.ModName AS Prefix1Mod2,
	                s.Prefix1Mod2Value,
	                mp21.ModName AS Prefix2Mod1,
	                s.Prefix2Mod1Value,
	                mp22.ModName AS Prefix2Mod2,
	                s.Prefix2Mod2Value,
	                mp31.ModName AS Prefix3Mod1,
	                s.Prefix3Mod1Value,
	                mp32.ModName AS Prefix3Mod2,
	                s.Prefix3Mod2Value,
	                ms11.ModName AS Suffix1Mod1,
	                s.Suffix1Mod1Value,
	                ms12.ModName AS Suffix1Mod2,
	                s.Suffix1Mod2Value,
	                ms21.ModName AS Suffix2Mod1,
	                s.Suffix2Mod1Value,
	                ms22.ModName AS Suffix2Mod2,
	                s.Suffix2Mod2Value,
	                ms31.ModName AS Suffix3Mod1,
	                s.Suffix3Mod1Value,
	                ms32.ModName AS Suffix3Mod2,
	                s.Suffix3Mod2Value	
                FROM 
	                Stash s 
	                INNER JOIN BaseItem b ON b.BaseItemId = s.BaseItemId 
                    INNER JOIN ItemType i ON i.ItemTypeId = b.ItemTypeId 
	                INNER JOIN ItemSubType i2 ON i2.ItemTypeId = b.ItemTypeId AND i2.ItemSubTypeId = b.ItemSubTypeId 
	                INNER JOIN Rarity r ON r.RarityId = s.RarityId
	                LEFT JOIN [Mod] mi1 ON mi1.ModId = b.Mod1Id
	                LEFT JOIN [Mod] mi2 ON mi2.ModId = b.Mod2Id
	                LEFT JOIN [Mod] mp11 ON mp11.ModId = s.Prefix1Mod1Id
	                LEFT JOIN [Mod] mp12 ON mp12.ModId = s.Prefix1Mod2Id
	                LEFT JOIN [Mod] mp21 ON mp21.ModId = s.Prefix2Mod1Id
	                LEFT JOIN [Mod] mp22 ON mp22.ModId = s.Prefix2Mod2Id
	                LEFT JOIN [Mod] mp31 ON mp31.ModId = s.Prefix3Mod1Id
	                LEFT JOIN [Mod] mp32 ON mp32.ModId = s.Prefix3Mod2Id
	                LEFT JOIN [Mod] ms11 ON ms11.ModId = s.Suffix1Mod1Id
	                LEFT JOIN [Mod] ms12 ON ms12.ModId = s.Suffix1Mod2Id
	                LEFT JOIN [Mod] ms21 ON ms21.ModId = s.Suffix2Mod1Id
	                LEFT JOIN [Mod] ms22 ON ms22.ModId = s.Suffix2Mod2Id
	                LEFT JOIN [Mod] ms31 ON ms31.ModId = s.Suffix3Mod1Id
	                LEFT JOIN [Mod] ms32 ON ms32.ModId = s.Suffix3Mod2Id";
            
            //Add filters
            bool where = false;
            if (_leagueId != Guid.Empty)
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
                sql += (where ? " AND " : " WHERE ") + " (mi1.ModId = " + _modId + " OR mi2.ModId = " + _modId + " OR mp11.ModId = " + _modId + " OR mp12.ModId = " + _modId + " OR mp21.ModId = " + _modId + " OR mp22.ModId = " + _modId + " OR mp31.ModId = " + _modId + " OR mp32.ModId = " + _modId + " OR ms11.ModId = " + _modId + " OR ms12.ModId = " + _modId + " OR ms21.ModId = " + _modId + " OR ms22.ModId = " + _modId + " OR ms31.ModId = " + _modId + " OR ms32.ModId = " + _modId + ")";
            }
            GlobalMethods.StuffGrid(sql, StashGrid);
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
            RefreshGrid();
        }

        private void Mod_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Determine the ModId
            _modId = Mod.Text == "(All)" ? 0 : GlobalMethods.GetScalarInt("SELECT ModId FROM [Mod] WHERE ModName = '" + Mod.Text + "';");
            RefreshGrid();
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
            _leagueId = League.Text == "(All)" ? Guid.Empty : GlobalMethods.GetScalarGuid("SELECT LeagueId FROM League WHERE LeagueName = '" + League.Text + "';");
            RefreshGrid();
        }
    }
}
