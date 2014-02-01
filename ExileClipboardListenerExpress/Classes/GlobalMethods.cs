using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;

namespace ExileClipboardListener.Classes
{
    public static class GlobalMethods
    {
        //ReSharper disable InconsistentNaming
        public const int STASH_MODE = 0;
        public const int COLLECTION_MODE = 1;
        //ReSharper restore InconsistentNaming
        public static int Mode = STASH_MODE;
        public static string Connection = "Data Source=ExileStash.s3db;Version=3;";
        public static int CommandTimeout = 600;

        //Current League and Character
        public static int CurrentLeagueId = Properties.Settings.Default.DefaultLeagueId;
        public static int CurrentCharacterId = Properties.Settings.Default.DefaultCharacterId;

        //Modification
        public struct Mod
        {
            public int Id;
            public string Name;
            public string RealName;
            public int ModPair;
            public int Armour;
            public int Weapons;
            public int Jewellery;
            public string Class;
            public int Value;
            public int ValueMin;
            public int ValueMax;
            public bool Implicit;
        }
        public static List<Mod> ModCache = new List<Mod>();
        public static Mod CurrentMod;

        //Base Item class
        public static class BaseItem
        {
            public static int BaseItemId;
            public static string ItemName;
            public static string ItemTypeName;
            public static string ItemSubTypeName;
            public static int ReqLevel;
            public static int ReqStr;
            public static int ReqDex;
            public static int ReqInt;
            public static int Armour;
            public static int Evasion;
            public static int EnergyShield;
            public static int DamagePhysicalMin;
            public static int DamagePhysicalMax;
            public static decimal AttackSpeed;
            public static decimal DPS;
            public static Mod Mod1;
            public static Mod Mod2;
        }

        //Affix class - this could also be materialised as a view in the database
        //This is used to reduce database access
        public struct Affix
        {
            public int AffixId;
            public string AffixType;
            public string Name;
            public int Level;
            public int ModCategoryId;
            public string ModCategoryName;
            public Mod Mod1;
            public Mod Mod2;
        }
        public static List<Affix> AffixCache = new List<Affix>();

        //Stash class
        public static class StashItem
        {
            public static string OriginalText;
            public static string ItemName;
            public static int BaseItemId;
            public static string ItemTypeName;
            public static string ItemSubTypeName;
            public static int RarityId;
            public static int Quality;
            public static int ItemLevel;
            public static int ReqLevel;
            public static int ReqLevelBase;
            public static int Armour;
            public static int Evasion;
            public static int EnergyShield;
            public static int PhysicalDamageMin;
            public static int PhysicalDamageMax;
            public static int ElementalDamageMin;
            public static int ElementalDamageMax;
            public static decimal BaseAttacksPerSecond;
            public static decimal AttacksPerSecond;
            public static decimal BaseDamagePerSecond;
            public static decimal DamagePerSecond;
            public static decimal CriticalStrikeChance;
            public static string Sockets;
            public static Mod[] Mod = new Mod[10];
            public static Affix[] Affix = new Affix[7]; //0 = Implicit, 1-3 - Prefix, 4-6 = Suffix
        }

        //Storage for filter results
        public static List<object[]> BISResults = new List<object[]>();
        public static List<object[]> ILevelResults = new List<object[]>();
        public static List<object[]> CLevelResults = new List<object[]>();
        public static List<object[]> ItemResults = new List<object[]>();

        public static void ClearStash()
        {
            StashItem.ItemName = "";
            StashItem.BaseItemId = 0;
            StashItem.RarityId = 0;
            StashItem.Quality = 0;
            StashItem.ItemLevel = 0;
            StashItem.ReqLevel = 0;
            StashItem.ReqLevelBase = 0;
            StashItem.Armour = 0;
            StashItem.Evasion = 0;
            StashItem.EnergyShield = 0;
            StashItem.PhysicalDamageMin = 0;
            StashItem.PhysicalDamageMax = 0;
            StashItem.ElementalDamageMin = 0;
            StashItem.ElementalDamageMax = 0;
            StashItem.AttacksPerSecond = 0;
            StashItem.DamagePerSecond = 0;
            StashItem.CriticalStrikeChance = 0;
            StashItem.Sockets = "";
            for (int affix = 0; affix < 7; affix++)
            {
                StashItem.Affix[affix].Mod1.Id = 0;
                StashItem.Affix[affix].Mod1.Value = 0;
                StashItem.Affix[affix].Mod2.Id = 0;
                StashItem.Affix[affix].Mod2.Value = 0;
            }
            for (int mod = 0; mod < StashItem.Mod.GetUpperBound(0); mod++)
            {
                StashItem.Mod[mod].Id = 0;
                StashItem.Mod[mod].Value = 0;
                StashItem.Mod[mod].ValueMax = 0;
                StashItem.Mod[mod].ValueMin = 0;
            }
            StashItem.OriginalText = "";
        }

        public static void LoadCache()
        {
            //Loads data from the system into memory to reduce unnecessary database access
            try
            {
                using (var con = new SQLiteConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;

                        //Prefixes first
                        com.CommandText = "SELECT p.*, mc.ModCategoryName, m1.ModName AS Mod1Name, m2.ModName AS Mod2Name FROM Prefix p INNER JOIN ModCategory mc ON mc.ModCategoryId = p.ModCategoryId LEFT JOIN [Mod] m1 ON m1.ModId = p.Mod1Id LEFT JOIN [Mod] m2 ON m2.ModId = p.Mod2Id;";
                        using (var dr = com.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var affix = new Affix();
                                affix.AffixId = dr["PrefixId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PrefixId"]);
                                affix.AffixType = "Prefix";
                                affix.Name = dr["Name"].ToString();
                                affix.Level = dr["Level"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Level"]);
                                affix.ModCategoryId = dr["ModCategoryId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ModCategoryId"]);
                                affix.ModCategoryName = dr["ModCategoryName"].ToString();
                                affix.Mod1.Id = dr["Mod1Id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1Id"]);
                                affix.Mod1.Name = dr["Mod1Name"].ToString();
                                affix.Mod1.ValueMin = dr["Mod1ValueMin"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1ValueMin"]);
                                affix.Mod1.ValueMax = dr["Mod1ValueMax"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1ValueMax"]);
                                affix.Mod2.Id = dr["Mod2Id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2Id"]);
                                affix.Mod2.Name = dr["Mod2Name"].ToString();
                                affix.Mod2.ValueMin = dr["Mod2ValueMin"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2ValueMin"]);
                                affix.Mod2.ValueMax = dr["Mod2ValueMax"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2ValueMax"]);
                                AffixCache.Add(affix);
                            }
                        }

                        //Suffixes next
                        com.CommandText = "SELECT s.*, mc.ModCategoryName, m1.ModName AS Mod1Name, m2.ModName AS Mod2Name FROM Suffix s INNER JOIN ModCategory mc ON mc.ModCategoryId = s.ModCategoryId LEFT JOIN [Mod] m1 ON m1.ModId = s.Mod1Id LEFT JOIN [Mod] m2 ON m2.ModId = s.Mod2Id;";
                        using (var dr = com.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var affix = new Affix();
                                affix.AffixId = dr["SuffixId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SuffixId"]);
                                affix.AffixType = "Suffix";
                                affix.Name = dr["Name"].ToString();
                                affix.Level = dr["Level"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Level"]);
                                affix.ModCategoryId = dr["ModCategoryId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ModCategoryId"]);
                                affix.ModCategoryName = dr["ModCategoryName"].ToString();
                                affix.Mod1.Id = dr["Mod1Id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1Id"]);
                                affix.Mod1.Name = dr["Mod1Name"].ToString();
                                affix.Mod1.ValueMin = dr["Mod1ValueMin"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1ValueMin"]);
                                affix.Mod1.ValueMax = dr["Mod1ValueMax"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1ValueMax"]);
                                affix.Mod2.Id = dr["Mod2Id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2Id"]);
                                affix.Mod2.Name = dr["Mod2Name"].ToString();
                                affix.Mod2.ValueMin = dr["Mod2ValueMin"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2ValueMin"]);
                                affix.Mod2.ValueMax = dr["Mod2ValueMax"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2ValueMax"]);
                                AffixCache.Add(affix);
                            }
                        }

                        //Mods last
                        com.CommandText = "SELECT * FROM Mod;";
                        using (var dr = com.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var mod = new Mod();
                                mod.Id = dr["ModId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ModId"]);
                                mod.Name = dr["ModName"].ToString();
                                mod.RealName = dr["ModRealName"].ToString();
                                mod.ModPair = dr["ModPair"] == DBNull.Value ? 1 : Convert.ToInt32(dr["ModPair"]);
                                mod.Armour = dr["Armour"] == DBNull.Value ? 1 : Convert.ToInt32(dr["Armour"]);
                                mod.Weapons = dr["Weapons"] == DBNull.Value ? 1 : Convert.ToInt32(dr["Weapons"]);
                                mod.Jewellery = dr["Jewellery"] == DBNull.Value ? 1 : Convert.ToInt32(dr["Jewellery"]);
                                mod.Class = dr["ModClass"].ToString();
                                mod.Implicit = mod.Class == "<implicit>";
                                ModCache.Add(mod);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return;
            }
            return;
        }

        public static void LoadBaseItem(int baseItemId)
        {
            try
            {
                using (var con = new SQLiteConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;
                        com.CommandText = "SELECT * FROM BaseItem b INNER JOIN ItemType i1 ON i1.ItemTypeId = b.ItemTypeId INNER JOIN ItemSubType i2 ON i2.ItemTypeId = b.ItemTypeId AND i2.ItemSubTypeId = b.ItemSubTypeId WHERE b.BaseItemId = " + baseItemId + ";";
                        using (var dr = com.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                BaseItem.BaseItemId = baseItemId;
                                BaseItem.ItemName = dr["ItemName"].ToString();
                                BaseItem.ItemTypeName = dr["ItemTypeName"].ToString();
                                BaseItem.ItemSubTypeName = dr["ItemSubTypeName"].ToString();
                                BaseItem.ReqLevel = dr["ReqLevel"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ReqLevel"]);
                                BaseItem.ReqStr = dr["ReqStr"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ReqStr"]);
                                BaseItem.ReqDex = dr["ReqDex"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ReqDex"]);
                                BaseItem.ReqInt = dr["ReqInt"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ReqInt"]);
                                BaseItem.Armour = dr["Armour"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Armour"]);
                                BaseItem.Evasion = dr["Evasion"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Evasion"]);
                                BaseItem.EnergyShield = dr["EnergyShield"] == DBNull.Value ? 0 : Convert.ToInt32(dr["EnergyShield"]);
                                BaseItem.DamagePhysicalMin = dr["DamagePhysicalMin"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DamagePhysicalMin"]);
                                BaseItem.DamagePhysicalMax = dr["DamagePhysicalMax"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DamagePhysicalMax"]);
                                BaseItem.AttackSpeed = dr["AttackSpeed"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["AttackSpeed"]);
                                BaseItem.DPS = dr["DPS"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["DPS"]);
                                BaseItem.Mod1.Id = dr["Mod1Id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1Id"]);
                                BaseItem.Mod1.ValueMin = dr["Mod1ValueMin"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1ValueMin"]);
                                BaseItem.Mod1.ValueMax = dr["Mod1ValueMax"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1ValueMax"]);
                                BaseItem.Mod2.Id = dr["Mod2Id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2Id"]);
                                BaseItem.Mod2.ValueMin = dr["Mod2ValueMin"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2ValueMin"]);
                                BaseItem.Mod2.ValueMax = dr["Mod2ValueMax"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2ValueMax"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return;
            }
            return;
        }

        public static Mod FindMod(string modName, int modPair, string itemTypeName)
        {
            var mod = new Mod();
            if (modName == "")
            {
                mod.Id = 0;
                return mod;
            }
            switch (itemTypeName)
            {
                case "Weapons":
                    mod = ModCache.FirstOrDefault(m => (m.Name == modName || m.RealName == modName) && m.ModPair == modPair && m.Weapons == 1 && !m.Implicit);
                    break;
                case "Armour":
                    mod = ModCache.FirstOrDefault(m => (m.Name == modName || m.RealName == modName) && m.ModPair == modPair && m.Armour == 1 && !m.Implicit);
                    break;
                case "Jewellery":
                    mod = ModCache.FirstOrDefault(m => (m.Name == modName || m.RealName == modName) && m.ModPair == modPair && m.Jewellery == 1 && !m.Implicit);
                    break;
                default:
                    mod.Id = 0;
                    break;
            }
            return mod;
        }

        public static Mod LookUpMod(int modId)
        {
            return ModCache.FirstOrDefault(m => m.Id == modId);
        }

        public static void LoadMod(int modId)
        {
            CurrentMod.Id = modId;
            for (int i = 0; i < ModCache.Count; i++)
            {
                if (ModCache[i].Id == modId)
                {
                    CurrentMod.Name = ModCache[i].Name;
                    CurrentMod.RealName = ModCache[i].RealName;
                    CurrentMod.ModPair = ModCache[i].ModPair;
                    CurrentMod.Armour = ModCache[i].Armour;
                    CurrentMod.Weapons = ModCache[i].Weapons;
                    CurrentMod.Jewellery = ModCache[i].Jewellery;
                    CurrentMod.Class = ModCache[i].Class;
                }
            }
        }

        public static int RunQuery(string sql)
        {
            //This method just runs a SQL query that has no results against the standard connection
            try
            {
                using (var con = new SQLiteConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;
                        com.CommandText = sql;
                        com.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return 1;
            }
            return 0;
        }

        public static int GetScalarInt(string sql)
        {
            //This method just runs a SQL query that returns a single integer
            int value;
            try
            {
                using (var con = new SQLiteConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;
                        com.CommandText = sql;
                        object result = com.ExecuteScalar();
                        value = result == DBNull.Value ? 0 : Convert.ToInt32(result);
                        //value = (com.ExecuteScalar() as int?) ?? 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return 0;
            }
            return value;
        }

        public static Guid GetScalarGuid(string sql)
        {
            //This method just runs a SQL query that returns a single integer
            Guid value;
            try
            {
                using (var con = new SQLiteConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;
                        com.CommandText = sql;
                        value = (com.ExecuteScalar() as Guid?) ?? Guid.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return Guid.Empty;
            }
            return value;
        }

        public static Decimal GetScalarDecimal(string sql)
        {
            //This method just runs a SQL query that returns a single integer
            Decimal value;
            try
            {
                using (var con = new SQLiteConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;
                        com.CommandText = sql;
                        value = (com.ExecuteScalar() as Decimal?) ?? 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return 0;
            }
            return value;
        }

        public static string GetScalarString(string sql)
        {
            //This method just runs a SQL query that returns a single string
            string value;
            try
            {
                using (var con = new SQLiteConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;
                        com.CommandText = sql;
                        object result = com.ExecuteScalar();

                        //This is a little weird:
                        //If the result is null then we return an empty string
                        //Otherwise we try a straight cast to string
                        //If this is null then the return type musn't be a string, so we use ToString to convert it
                        if (result == null)
                            value = null;
                        else
                            value = result as String ?? result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return "";
            }
            return value ?? "";
        }

        public static int StuffGrid(string sql, DataGridView gridTarget, bool reloadColumns = true)
        {
            //This method will run a SQL query and load the results into the specified data grid
            //We try to be clever with dates, if we have a column with a date format then we will convert the SQL results to a date before loading them
            //We return a row count
            int rowCount = 0;
            try
            {
                gridTarget.Rows.Clear();
                if (reloadColumns)
                    gridTarget.Columns.Clear();
                using (var con = new SQLiteConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;
                        com.CommandText = sql;
                        using (var dr = com.ExecuteReader())
                        {
                            int columnCount = dr.FieldCount;
                            if (reloadColumns)
                            {
                                //Add the columns dynamically
                                for (int i = 0; i < columnCount; i++)
                                {
                                    var col = new DataGridViewTextBoxColumn { HeaderText = dr.GetName(i) };
                                    if (dr.GetName(i).Contains("Date"))
                                        col.DefaultCellStyle.Format = "d";
                                    gridTarget.Columns.Add(col);
                                }
                            }

                            //We change the valuetype for any date or numerical columns
                            //Some of this is redundant as we always add columns dynamically, but I might change this back again?
                            for (int i = 0; i < gridTarget.Columns.Count; i++)
                            {
                                if (gridTarget.Columns[i].DefaultCellStyle.Format == "d")
                                    gridTarget.Columns[i].ValueType = typeof(DateTime);
                                else if (gridTarget.Columns[i].DefaultCellStyle.Format == "N0")
                                    gridTarget.Columns[i].ValueType = typeof(int);
                                else if (gridTarget.Columns[i].DefaultCellStyle.Format.Length > 1 && gridTarget.Columns[i].DefaultCellStyle.Format.Substring(0, 1) == "N")
                                    gridTarget.Columns[i].ValueType = typeof(decimal);
                            }

                            //Now process the results set
                            for (rowCount = 0; dr.Read(); rowCount++)
                            {
                                var row = new object[columnCount];
                                for (int i = 0; i < columnCount; i++)
                                {
                                    //If the column is formatted as a date then we try to load a date into it
                                    //Note we have to use null where there is no date or we will break the sorting
                                    if (gridTarget.Columns[i].DefaultCellStyle.Format == "d")
                                    {
                                        if (dr[i] == DBNull.Value || dr[i].ToString() == "")
                                            row[i] = null;
                                        else
                                            row[i] = Convert.ToDateTime(dr[i]);
                                    }
                                    else if (gridTarget.Columns[i].DefaultCellStyle.Format == "N0")//Number with no decimals
                                    {
                                        if (dr[i] == DBNull.Value || dr[i].ToString() == "")
                                            // ReSharper disable RedundantCast
                                            row[i] = (int?)null;
                                        // ReSharper restore RedundantCast
                                        else
                                            row[i] = Convert.ToInt32(dr[i]);
                                    }
                                    else if (gridTarget.Columns[i].DefaultCellStyle.Format.Length > 1 && gridTarget.Columns[i].DefaultCellStyle.Format.Substring(0, 1) == "N")//Number with decimals
                                    {
                                        if (dr[i] == DBNull.Value || dr[i].ToString() == "")
                                            // ReSharper disable RedundantCast
                                            row[i] = (decimal?)null;
                                        // ReSharper restore RedundantCast
                                        else
                                            row[i] = Convert.ToDecimal(dr[i]);
                                    }
                                    else
                                        row[i] = dr[i].ToString().Trim();
                                }
                                gridTarget.Rows.Add(row);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
            }
            return rowCount;
        }

        public static void StuffCombo(string sql, ComboBox target)
        {
            //This method will run a SQL query and load the results into the specified combo box
            try
            {
                target.Items.Clear();
                using (var con = new SQLiteConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;
                        com.CommandText = sql;
                        using (var dr = com.ExecuteReader())
                        {
                            //Now process the results set
                            while (dr.Read())
                            {
                                target.Items.Add(dr[0].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
            }
            return;
        }

        public static void StuffIntList(string sql, List<int> target)
        {
            try
            {
                using (var con = new SQLiteConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;
                        com.CommandText = sql;
                        using (var dr = com.ExecuteReader())
                        {
                            //Now process the results set
                            while (dr.Read())
                            {
                                target.Add(Convert.ToInt32(dr[0]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
            }
        }
    }
}
