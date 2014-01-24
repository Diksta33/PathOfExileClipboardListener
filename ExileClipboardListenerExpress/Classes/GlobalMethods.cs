using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace ExileClipboardListener.Classes
{
    public static class GlobalMethods
    {
        //ReSharper disable InconsistentNaming
        public const int STASH_MODE = 0;
        public const int COLLECTION_MODE = 1;
        //ReSharper restore InconsistentNaming
        public static int Mode = STASH_MODE;
        public static string Connection = "Server=localhost\\EXILE;Database=PathOfExile;Trusted_Connection=yes;";
        public static int CommandTimeout = 600;

        //Current League and Character
        public static Guid CurrentLeagueId = Properties.Settings.Default.DefaultLeagueId;
        public static Guid CurrentCharacterId = Properties.Settings.Default.DefaultCharacterId;

        //Stash class
        public static class StashItem
        {
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
            public static decimal CriticalStrikeChance;
            public static int ImplicitMod1Id;
            public static int ImplicitMod1Value;
            public static int ImplicitMod2Id;
            public static int ImplicitMod2Value;
            public static int Prefix1Id;
            public static int Prefix1Mod1Id;
            public static int Prefix1Mod1Value;
            public static int Prefix1Mod2Id;
            public static int Prefix1Mod2Value;
            public static int Prefix2Id;
            public static int Prefix2Mod1Id;
            public static int Prefix2Mod1Value;
            public static int Prefix2Mod2Id;
            public static int Prefix2Mod2Value;
            public static int Prefix3Id;
            public static int Prefix3Mod1Id;
            public static int Prefix3Mod1Value;
            public static int Prefix3Mod2Id;
            public static int Prefix3Mod2Value;
            public static int Suffix1Id;
            public static int Suffix1Mod1Id;
            public static int Suffix1Mod1Value;
            public static int Suffix1Mod2Id;
            public static int Suffix1Mod2Value;
            public static int Suffix2Id;
            public static int Suffix2Mod1Id;
            public static int Suffix2Mod1Value;
            public static int Suffix2Mod2Id;
            public static int Suffix2Mod2Value;
            public static int Suffix3Id;
            public static int Suffix3Mod1Id;
            public static int Suffix3Mod1Value;
            public static int Suffix3Mod2Id;
            public static int Suffix3Mod2Value;
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
            StashItem.CriticalStrikeChance = 0;
            StashItem.ImplicitMod1Id = 0;
            StashItem.ImplicitMod1Value = 0;
            StashItem.ImplicitMod2Id = 0;
            StashItem.ImplicitMod2Value = 0;
            StashItem.Prefix1Id = 0;
            StashItem.Prefix1Mod1Id = 0;
            StashItem.Prefix1Mod1Value = 0;
            StashItem.Prefix1Mod2Id = 0;
            StashItem.Prefix1Mod2Value = 0;
            StashItem.Prefix2Id = 0;
            StashItem.Prefix2Mod1Id = 0;
            StashItem.Prefix2Mod1Value = 0;
            StashItem.Prefix2Mod2Id = 0;
            StashItem.Prefix2Mod2Value = 0;
            StashItem.Prefix3Id = 0;
            StashItem.Prefix3Mod1Id = 0;
            StashItem.Prefix3Mod1Value = 0;
            StashItem.Prefix3Mod2Id = 0;
            StashItem.Prefix3Mod2Value = 0;
            StashItem.Suffix1Id = 0;
            StashItem.Suffix1Mod1Id = 0;
            StashItem.Suffix1Mod1Value = 0;
            StashItem.Suffix1Mod2Id = 0;
            StashItem.Suffix1Mod2Value = 0;
            StashItem.Suffix2Id = 0;
            StashItem.Suffix2Mod1Id = 0;
            StashItem.Suffix2Mod1Value = 0;
            StashItem.Suffix2Mod2Id = 0;
            StashItem.Suffix2Mod2Value = 0;
            StashItem.Suffix3Id = 0;
            StashItem.Suffix3Mod1Id = 0;
            StashItem.Suffix3Mod1Value = 0;
            StashItem.Suffix3Mod2Id = 0;
            StashItem.Suffix3Mod2Value = 0;
        }

        public static int RunQuery(string sql)
        {
            //This method just runs a SQL query that has no results against the standard connection
            try
            {
                using (var con = new SqlConnection(Connection))
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
                using (var con = new SqlConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;
                        com.CommandText = sql;
                        value = (com.ExecuteScalar() as int?) ?? 0;
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
                using (var con = new SqlConnection(Connection))
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

        public static string GetScalarString(string sql)
        {
            //This method just runs a SQL query that returns a single string
            string value;
            try
            {
                using (var con = new SqlConnection(Connection))
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
                using (var con = new SqlConnection(Connection))
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
                using (var con = new SqlConnection(Connection))
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

        public static List<ClipboardNotification.Fix> StuffList(string sql)
        {
            var results = new List<ClipboardNotification.Fix>();
            try
            {
                using (var con = new SqlConnection(Connection))
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
                                var row = new ClipboardNotification.Fix();
                                row.FixId = Convert.ToInt32(dr["FixId"]);
                                row.Mod1Id = Convert.ToInt32(dr["Mod1Id"]);
                                row.Mod1ValueMin = Convert.ToInt32(dr["Mod1ValueMin"]);
                                row.Mod1ValueMax = Convert.ToInt32(dr["Mod1ValueMax"]);
                                row.Mod2Id = dr["Mod2Id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2Id"]);
                                row.Mod2ValueMin = dr["Mod2ValueMin"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2ValueMin"]);
                                row.Mod2ValueMax = dr["Mod2ValueMax"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2ValueMax"]);
                                results.Add(row);
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
    }
}
