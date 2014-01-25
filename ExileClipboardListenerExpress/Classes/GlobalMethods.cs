using System;
using System.Collections.Generic;
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
        public static Guid CurrentLeagueId = Properties.Settings.Default.DefaultLeagueId;
        public static Guid CurrentCharacterId = Properties.Settings.Default.DefaultCharacterId;

        //Modification
        public struct Mod
        {
            public int Id;
            public int Value;
            public int ValueMin;
            public int ValueMax;
        }

        //Affix
        public struct Affix
        {
            public int AffixId;
            public int Level;
            public Mod Mod1;
            public Mod Mod2;
        }

        //Stash class
        public static class StashItem
        {
            public static string OriginalText;
            public static string ItemName;
            public static int BaseItemId;
            public static string ItemTypeName;
            public static string ItemSubTypeName;
            public static int RarityId;
            public static  int Quality;
            public static int ItemLevel;
            public static int ReqLevel;
            public static int Armour;
            public static int Evasion;
            public static int EnergyShield;
            public static int PhysicalDamageMin;
            public static int PhysicalDamageMax;
            public static int ElementalDamageMin;
            public static int ElementalDamageMax;
            public static decimal AttacksPerSecond;
            public static decimal DamagePerSecond;
            public static decimal CriticalStrikeChance;
            public static string Sockets;
            public static Mod[] Mod = new Mod[10];
            public static Affix[] Affix = new Affix[7]; //0 = Implicit, 1-3 - Prefix, 4-6 = Suffix
        }

        public static void ClearStash()
        {
            StashItem.ItemName = "";
            StashItem.BaseItemId = 0;
            StashItem.RarityId = 0;
            StashItem.Quality = 0;
            StashItem.ItemLevel = 0;
            StashItem.ReqLevel = 0;
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
            StashItem.OriginalText = "";
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
                        if (result == DBNull.Value)
                            value = 0;
                        else
                            value = Convert.ToInt32(result);
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

        public static int StuffGrid(string sql, DataGridView gridTarget)
        {
            //This method will run a SQL query and load the results into the specified data grid
            //We try to be clever with dates, if we have a column with a date format then we will convert the SQL results to a date before loading them
            //We return a row count
            int rowCount = 0;
            try
            {
                gridTarget.Rows.Clear();
                gridTarget.Columns.Clear();
                using (var con = new SQLiteConnection (Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;
                        com.CommandText = sql;
                        using (var dr = com.ExecuteReader())
                        {
                            //Add the columns dynamically
                            int columnCount = dr.FieldCount;
                            for (int i = 0; i < columnCount; i++)
                            {
                                var col = new DataGridViewTextBoxColumn {HeaderText = dr.GetName(i)};
                                if (dr.GetName(i).Contains("Date"))
                                    col.DefaultCellStyle.Format = "d";
                                gridTarget.Columns.Add(col);
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

        public static List<GlobalMethods.Affix> StuffList(string sql)
        {
            var results = new List<GlobalMethods.Affix>();
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
                                var affix = new GlobalMethods.Affix();
                                affix.AffixId = Convert.ToInt32(dr["AffixId"]);
                                affix.Level = Convert.ToInt32(dr["Level"]);
                                affix.Mod1.Id = Convert.ToInt32(dr["Mod1Id"]);
                                affix.Mod1.ValueMin = Convert.ToInt32(dr["Mod1ValueMin"]);
                                affix.Mod1.ValueMax = Convert.ToInt32(dr["Mod1ValueMax"]);
                                affix.Mod2.Id = dr["Mod2Id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2Id"]);
                                affix.Mod2.ValueMin = dr["Mod2ValueMin"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2ValueMin"]);
                                affix.Mod2.ValueMax = dr["Mod2ValueMax"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2ValueMax"]);
                                results.Add(affix);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
            }
            return results;
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
