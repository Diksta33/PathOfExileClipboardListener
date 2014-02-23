using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Drawing;
using System.IO;

namespace ExileClipboardListener.Classes
{
    public static class GlobalMethods
    {
        //ReSharper disable InconsistentNaming
        public const int STASH_MODE = 0;
        public const int COLLECTION_MODE = 1;
        public const int COMPACT_MODE = 2;
        //ReSharper restore InconsistentNaming
        public static int Mode = STASH_MODE;

        //Quick Update Settings
        public const int ALL_LEAGUES = 0;
        public const int DEFAULT_LEAGUE = 1;

        public static string Connection = "Data Source=ExileStash.s3db;Version=3;";
        public static int CommandTimeout = 600;

        //Current League and Character
        public static int LeagueId = Properties.Settings.Default.DefaultLeagueId;
        public static int CharacterId = Properties.Settings.Default.DefaultCharacterId;

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
            public static int IconWidth;
            public static int IconHeight;
            public static Image Icon;
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
            //ReSharper disable InconsistentNaming
            public static decimal eDPS;
            public static decimal pDPS;
            public static decimal tDPS;
            //ReSharper restore InconsistentNaming
            public static int Life;
            public static int Mana;
            public static int FireRes;
            public static int ColdRes;
            public static int LightningRes;
            public static int EleRes;
            public static int ChaosRes;
            public static int TotalRes;
            public static decimal CriticalStrikeChance;
            public static string Sockets;
            public static Mod[] Mod = new Mod[20];
            public static Affix[] Affix = new Affix[7]; //0 = Implicit, 1-3 - Prefix, 4-6 = Suffix
        }

        //Gem Class
        public static class StashGem
        {
            public static string OriginalText;
            public static string GemName;
            public static string GemType;
            public static string Experience;
            public static int Level;
            public static int Quality;
            public static int ReqLevel;
            public static int ReqStr;
            public static int ReqDex;
            public static int ReqInt;
            public static int ManaCost;
            public static int ManaMultiplier;
            public static int ManaReserved;
            public static string[] ExplicitMod = new string[10];
        }

        //Map Class
        public static class StashMap
        {
            public static string OriginalText;
            public static string Name;
            public static int RarityId;
            public static int MapLevel;
            public static int ItemLevel;
            public static int ItemQuantity;
            public static int Quality;
        }

        //Currency Class, the current item
        public static class StashCurrency
        {
            public static int CurrencyItemId;
            public static string Name;
            public static int StackSize;
        }

        //Currency Stash Class, a list of items and counts
        public struct StashCurrencyBase
        {
            public int CurrencyItemId;
            public string Name;
            public int StackSize;
        }
        public static List<StashCurrencyBase> CurrentStashCurrency = new List<StashCurrencyBase>();

        //Currency Cache
        public struct CurrencyBase
        {
            public int CurrencyItemId;
            public string Name;
            public int StackSize;
            public string Description;
            public Image Icon;
        }
        public static List<CurrencyBase> CurrencyCache = new List<CurrencyBase>();

        //Storage for filter results
        public static List<object[]> BISResults = new List<object[]>();
        //ReSharper disable InconsistentNaming
        public static List<object[]> ILevelResults = new List<object[]>();
        //ReSharper restore InconsistentNaming
        public static List<object[]> CLevelResults = new List<object[]>();
        public static List<object[]> ItemResults = new List<object[]>();

        public static string DecryptCredentials()
        {
            return JSON.StringCipher.Decrypt(Properties.Settings.Default.Password);
        }

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
            StashItem.eDPS = 0;
            StashItem.pDPS = 0;
            StashItem.tDPS = 0;
            StashItem.CriticalStrikeChance = 0;
            StashItem.Sockets = "";
            for (int affix = 0; affix < 7; affix++)
            {
                StashItem.Affix[affix].AffixId = 0;
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

        public static void ClearGem()
        {
            StashGem.GemName = "";
            StashGem.Level = 0;
            StashGem.Quality = 0;
            StashGem.ReqLevel = 0;
            StashGem.ReqStr = 0;
            StashGem.ReqDex = 0;
            StashGem.ReqInt = 0;
            StashGem.ManaCost = 0;
            StashGem.ManaMultiplier = 0;
            StashGem.ManaReserved = 0;
            StashGem.Experience = "";
            for (int im = 0; im < 10; im++)
                StashGem.ExplicitMod[im] = "";
            StashGem.OriginalText = "";
        }

        public static void ClearMap()
        {
            StashMap.Name = "";
            StashMap.RarityId = 0;
            StashMap.MapLevel = 0;
            StashMap.ItemLevel = 0;
            StashMap.ItemQuantity = 0;
            StashMap.Quality = 0;
            StashMap.OriginalText = "";
        }

        public static void LoadCache()
        {
            //Loads data from the system into memory to reduce unnecessary database access
            try
            {
                //Load a null affix
                AffixCache.Clear();
                var affixDummy = new Affix {AffixId = 0};
                AffixCache.Add(affixDummy);

                //Now the real ones
                using (var con = new SQLiteConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;

                        //Prefixes first
                        com.CommandText = "SELECT p.*, mc.ModCategoryName, m1.ModName AS Mod1Name, m1.ModRealName AS Mod1RealName, m1.ModClass AS Mod1Class, m2.ModName AS Mod2Name, m2.ModRealName AS Mod2RealName, m2.ModClass AS Mod2Class FROM Prefix p INNER JOIN ModCategory mc ON mc.ModCategoryId = p.ModCategoryId LEFT JOIN [Mod] m1 ON m1.ModId = p.Mod1Id LEFT JOIN [Mod] m2 ON m2.ModId = p.Mod2Id;";
                        using (var dr = com.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //ReSharper disable UseObjectOrCollectionInitializer
                                var affix = new Affix();
                                //ReSharper restore UseObjectOrCollectionInitializer
                                affix.AffixId = dr["PrefixId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PrefixId"]);
                                affix.AffixType = "Prefix";
                                affix.Name = dr["Name"].ToString();
                                affix.Level = dr["Level"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Level"]);
                                affix.ModCategoryId = dr["ModCategoryId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ModCategoryId"]);
                                affix.ModCategoryName = dr["ModCategoryName"].ToString();
                                affix.Mod1.Id = dr["Mod1Id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1Id"]);
                                affix.Mod1.Name = dr["Mod1Name"].ToString();
                                affix.Mod1.RealName = dr["Mod1RealName"].ToString();
                                affix.Mod1.ValueMin = dr["Mod1ValueMin"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1ValueMin"]);
                                affix.Mod1.ValueMax = dr["Mod1ValueMax"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1ValueMax"]);
                                affix.Mod1.Class = dr["Mod1Class"].ToString();
                                affix.Mod2.Id = dr["Mod2Id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2Id"]);
                                affix.Mod2.Name = dr["Mod2Name"].ToString();
                                affix.Mod2.RealName = dr["Mod2RealName"].ToString();
                                affix.Mod2.ValueMin = dr["Mod2ValueMin"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2ValueMin"]);
                                affix.Mod2.ValueMax = dr["Mod2ValueMax"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2ValueMax"]);
                                affix.Mod2.Class = dr["Mod2Class"].ToString();
                                AffixCache.Add(affix);
                            }
                        }

                        //Suffixes next
                        com.CommandText = "SELECT s.*, mc.ModCategoryName, m1.ModName AS Mod1Name, m1.ModRealName AS Mod1RealName, m1.ModClass AS Mod1Class, m2.ModName AS Mod2Name, m2.ModRealName AS Mod2RealName, m2.ModClass AS Mod2Class FROM Suffix s INNER JOIN ModCategory mc ON mc.ModCategoryId = s.ModCategoryId LEFT JOIN [Mod] m1 ON m1.ModId = s.Mod1Id LEFT JOIN [Mod] m2 ON m2.ModId = s.Mod2Id;";
                        using (var dr = com.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //ReSharper disable UseObjectOrCollectionInitializer
                                var affix = new Affix();
                                //ReSharper restore UseObjectOrCollectionInitializer
                                affix.AffixId = dr["SuffixId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SuffixId"]);
                                affix.AffixType = "Suffix";
                                affix.Name = dr["Name"].ToString();
                                affix.Level = dr["Level"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Level"]);
                                affix.ModCategoryId = dr["ModCategoryId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ModCategoryId"]);
                                affix.ModCategoryName = dr["ModCategoryName"].ToString();
                                affix.Mod1.Id = dr["Mod1Id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1Id"]);
                                affix.Mod1.Name = dr["Mod1Name"].ToString();
                                affix.Mod1.RealName = dr["Mod1RealName"].ToString();
                                affix.Mod1.ValueMin = dr["Mod1ValueMin"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1ValueMin"]);
                                affix.Mod1.ValueMax = dr["Mod1ValueMax"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod1ValueMax"]);
                                affix.Mod1.Class = dr["Mod1Class"].ToString();
                                affix.Mod2.Id = dr["Mod2Id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2Id"]);
                                affix.Mod2.Name = dr["Mod2Name"].ToString();
                                affix.Mod2.RealName = dr["Mod2RealName"].ToString();
                                affix.Mod2.ValueMin = dr["Mod2ValueMin"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2ValueMin"]);
                                affix.Mod2.ValueMax = dr["Mod2ValueMax"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Mod2ValueMax"]);
                                affix.Mod2.Class = dr["Mod2Class"].ToString();
                                AffixCache.Add(affix);
                            }
                        }

                        //Mods 
                        ModCache.Clear();
                        com.CommandText = "SELECT * FROM Mod;";
                        using (var dr = com.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //ReSharper disable UseObjectOrCollectionInitializer
                                var mod = new Mod();
                                //ReSharper restore UseObjectOrCollectionInitializer
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

                        //Currency
                        CurrencyCache.Clear();
                        com.CommandText = "SELECT * FROM CurrencyItem;";
                        using (var dr = com.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //ReSharper disable UseObjectOrCollectionInitializer
                                var currencyItem = new CurrencyBase();
                                //ReSharper restore UseObjectOrCollectionInitializer
                                currencyItem.CurrencyItemId = dr["CurrencyItemId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CurrencyItemId"]);
                                currencyItem.Name = dr["Name"].ToString();
                                currencyItem.StackSize = dr["StackSize"] == DBNull.Value ? 0 : Convert.ToInt32(dr["StackSize"]);
                                currencyItem.Description = dr["Description"].ToString();
                                if (dr["Icon"] != DBNull.Value)
                                {
                                    var imageBytes = (Byte[])dr["Icon"];
                                    currencyItem.Icon = ByteArrayToImage(imageBytes);
                                }
                                else
                                    currencyItem.Icon = null;
                                CurrencyCache.Add(currencyItem);
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
                                BaseItem.IconWidth = dr["IconWidth"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IconWidth"]);
                                BaseItem.IconHeight = dr["IconHeight"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IconHeight"]);
                                if (dr["Icon"] != DBNull.Value)
                                {
                                    var imageBytes = (Byte[])dr["Icon"];
                                    BaseItem.Icon = ByteArrayToImage(imageBytes);
                                }
                                else
                                    BaseItem.Icon = null;
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

        public static void SetBaseItemIcon(int baseItemId, Image icon, int width, int height)
        {
            try
            {
                using (var con = new SQLiteConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandText = "UPDATE BaseItem SET Icon = @Icon, IconWidth = " + width + ", IconHeight = " + height + " WHERE BaseItemId = " + baseItemId + ";";
                        var parameter = new SQLiteParameter("@Icon", System.Data.DbType.Binary) {Value = ImageToByteArray(icon)};
                        com.Parameters.Add(parameter);
                        com.ExecuteNonQuery();
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

        private static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        
        private static byte[] ImageToByteArray(Image imageIn)
        {
            //MemoryStream ms = new MemoryStream();
            //imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            //return ms.ToArray();

            var imageConverter = new ImageConverter();
            var xByte = (byte[])imageConverter.ConvertTo(imageIn, typeof(byte[]));
            return xByte;
        }

        public static Mod FindMod(string modName, int modPair, string itemTypeName, string itemSubTypeName)
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
                case "Quiver":
                    mod = ModCache.FirstOrDefault(m => (m.Name == modName || m.RealName == modName) && m.ModPair == modPair && (m.Armour == 1 || m.Jewellery == 1) && !m.Implicit);
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

        //public static Guid GetScalarGuid(string sql)
        //{
        //    //This method just runs a SQL query that returns a single integer
        //    Guid value;
        //    try
        //    {
        //        using (var con = new SQLiteConnection(Connection))
        //        {
        //            con.Open();
        //            using (var com = con.CreateCommand())
        //            {
        //                com.CommandTimeout = CommandTimeout;
        //                com.CommandText = sql;
        //                value = (com.ExecuteScalar() as Guid?) ?? Guid.Empty;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Unhandled exception: " + ex.Message);
        //        return Guid.Empty;
        //    }
        //    return value;
        //}

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

        public static int StuffGrid(string sql, DataGridView gridTarget, bool reloadColumns = true, bool suppressZeroes = false)
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
                                    if (dr.GetDataTypeName(i).ToUpper().Contains("DATE"))
                                        col.DefaultCellStyle.Format = "d";
                                    else if (dr.GetDataTypeName(i).ToUpper().Contains("INT"))
                                    {
                                        col.DefaultCellStyle.Format = "N0";
                                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    }
                                    else if (dr.GetDataTypeName(i).ToUpper().Contains("DOUBLE"))
                                    {
                                        col.DefaultCellStyle.Format = "N2";
                                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                    }
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
                                else if (gridTarget.Columns[i].DefaultCellStyle.Format.Length >= 1 && gridTarget.Columns[i].DefaultCellStyle.Format.Substring(0, 1) == "N")
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
                                        if (dr[i] == DBNull.Value || dr[i].ToString() == "" || (suppressZeroes && dr[i].ToString() == "0"))
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

        public static List<int> StuffIntList(string sql)
        {
            try
            {
                var target = new List<int>();
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
                return target;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return null;
            }
        }

        //Should make this generic, but it's only two types
        public static List<string> StuffStringList(string sql)
        {
            try
            {
                var target = new List<string>();
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
                                target.Add(dr[0].ToString());
                            }
                        }
                    }
                }
                return target;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return null;
            }
        }

        public static void GetGemTypes(ComboBox GemType)
        {
            try
            {
                GemType.Items.Clear();
                var gemTypes = new List<string>();
                GemType.Items.Add("(All)");
                using (var con = new SQLiteConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;
                        com.CommandText = "SELECT DISTINCT Type FROM GemStash;";
                        using (var dr = com.ExecuteReader())
                        {
                            //Now process the results set
                            while (dr.Read())
                            {
                                var types = dr[0].ToString().Replace(", ", ",").Split(',');
                                foreach (var gemType in types)
                                {
                                    if (!gemTypes.Contains(gemType))
                                        gemTypes.Add(gemType);
                                }
                            }
                        }
                    }
                }

                //Add the gem types to the list
                foreach (var gemType in gemTypes)
                    GemType.Items.Add(gemType);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
            }
        }

        public static void GetCurrencyCount(int leagueId)
        {
            try
            {
                CurrentStashCurrency.Clear();
                using (var con = new SQLiteConnection(Connection))
                {
                    con.Open();
                    using (var com = con.CreateCommand())
                    {
                        com.CommandTimeout = CommandTimeout;
                        com.CommandText = "SELECT cs.CurrencyItemId, ci.Name, cs.Count FROM CurrencyStash cs INNER JOIN CurrencyItem ci ON ci.CurrencyItemId = cs.CurrencyItemId " + (leagueId == 0 ? "" : " WHERE cs.LeagueId = " + leagueId) +  ";";
                        using (var dr = com.ExecuteReader())
                        {
                            //Now process the results set
                            while (dr.Read())
                            {
                                CurrentStashCurrency.Add(new StashCurrencyBase { CurrencyItemId = Convert.ToInt32(dr["CurrencyItemId"]), Name = dr["Name"].ToString(), StackSize = Convert.ToInt32(dr["Count"]) });
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

        public static void SaveMap(int leagueId)
        {
            if (!Properties.Settings.Default.StashDuplicates)
            {
                if (GlobalMethods.GetScalarInt("SELECT COUNT(*) AS Count FROM MapStash WHERE OriginalText = '" + StashMap.OriginalText.Replace("'", "''") + "';") != 0)
                    return;
            }
            string sql = "INSERT INTO MapStash(LeagueId, Name, RarityId, MapLevel, ItemLevel, ItemQuantity, Quality, OriginalText) VALUES(";
            sql += leagueId + ",";
            sql += "'" + StashMap.Name.Replace("'", "''") + "',";
            sql += StashMap.RarityId + ",";
            sql += StashMap.MapLevel + ",";
            sql += StashMap.ItemLevel + ",";
            sql += StashMap.ItemQuantity + ",";
            sql += StashMap.Quality + ",";
            sql += "'" + StashMap.OriginalText.Replace("'", "''") + "')";
            GlobalMethods.RunQuery(sql);
        }

        public static void SaveCurrency(int leagueId)
        {
            string sql = "";

            //Check to see if we have a row for this combination
            if (GlobalMethods.GetScalarInt("SELECT COUNT(*) FROM CurrencyStash WHERE LeagueId = " + leagueId + " AND CurrencyItemId = " + StashCurrency.CurrencyItemId + ";") == 0)
            {
                sql = "INSERT INTO CurrencyStash(LeagueId, CurrencyItemId, Count) VALUES(" + leagueId + ", " + StashCurrency.CurrencyItemId + "," + StashCurrency.StackSize + ");";
            }
            else
                sql = "UPDATE CurrencyStash SET Count = Count + " + StashCurrency.StackSize + " WHERE LeagueId = " + leagueId + " AND CurrencyItemId = " + StashCurrency.CurrencyItemId + ";";

            //Stash this item
            RunQuery(sql);
        }

        public static void SaveGem(int leagueId)
        {
            string sql = "INSERT INTO GemStash(LeagueId, Name, Level, Quality, ReqLevel, ReqStr, ReqDex, ReqInt, Type, ManaCost, ManaMultiplier, ManaReserved, ExplicitMod1, ExplicitMod2, ExplicitMod3, ExplicitMod4, ExplicitMod5, ExplicitMod6, ExplicitMod7, ExplicitMod8, ExplicitMod9, OriginalText) VALUES (";

            //League
            sql += leagueId + ",";

            //Basic Details
            sql += "'" + StashGem.GemName.Replace("'", "''") + "',";
            sql += StashGem.Level + ",";
            sql += StashGem.Quality + ",";
            sql += StashGem.ReqLevel + ",";
            sql += StashGem.ReqStr + ",";
            sql += StashGem.ReqDex + ",";
            sql += StashGem.ReqInt + ",";

            //Other stuff
            sql += "'" + StashGem.GemType.Replace("'", "''") + "',";
            sql += StashGem.ManaCost + ",";
            sql += StashGem.ManaMultiplier + ",";
            sql += StashGem.ManaReserved + ",";

            //Explicit Mods
            sql += "'" + StashGem.ExplicitMod[0].Replace("'", "''") + "',";
            sql += "'" + StashGem.ExplicitMod[1].Replace("'", "''") + "',";
            sql += "'" + StashGem.ExplicitMod[2].Replace("'", "''") + "',";
            sql += "'" + StashGem.ExplicitMod[3].Replace("'", "''") + "',";
            sql += "'" + StashGem.ExplicitMod[4].Replace("'", "''") + "',";
            sql += "'" + StashGem.ExplicitMod[5].Replace("'", "''") + "',";
            sql += "'" + StashGem.ExplicitMod[6].Replace("'", "''") + "',";
            sql += "'" + StashGem.ExplicitMod[7].Replace("'", "''") + "',";
            sql += "'" + StashGem.ExplicitMod[8].Replace("'", "''") + "',";
            if (StashGem.ExplicitMod[9] != "")
                MessageBox.Show("No room at the inn!");

            //Original Text
            sql += "'" + StashGem.OriginalText.Replace("'", "''") + "')";

            //Stash this item
            RunQuery(sql);
        }

        public static void SaveStash(int leagueId)
        {
            //If we don't allow duplicates then check we don't already have this first
            if (!Properties.Settings.Default.StashDuplicates)
            {
                if (GlobalMethods.GetScalarInt("SELECT COUNT(*) AS Count FROM Stash WHERE OriginalText = '" + StashItem.OriginalText.Replace("'", "''") + "';") != 0)
                    return;
            }

            //Save this item to the database
            string sql = "INSERT INTO Stash(LeagueId, ItemName, BaseItemId, RarityId, Quality, ItemLevel, ReqLevel,";
            sql += " Armour, Evasion, EnergyShield, AttackSpeed, DamagePhysicalMin, DamagePhysicalMax, PhysicalDPS, DamageElementalMin, DamageElementalMax, ElementalDPS, TotalDPS,";
            sql += " ImplicitMod1Id, ImplicitMod1Value, ImplicitMod2Id, ImplicitMod2Value, SocketCount, SocketMaxLink, Life, Mana, FireRes, ColdRes, LightningRes, ChaosRes, OriginalText)";
            sql += " VALUES(";

            //League
            sql += leagueId + ",";

            //Basic Details
            sql += "'" + StashItem.ItemName.Replace("'", "''") + "',";
            sql += (StashItem.BaseItemId == 0 ? "NULL" : StashItem.BaseItemId.ToString()) + ",";
            sql += StashItem.RarityId + ",";
            sql += StashItem.Quality + ",";
            sql += StashItem.ItemLevel + ",";
            sql += StashItem.ReqLevelBase + ",";

            //Armour
            sql += StashItem.Armour + ",";
            sql += StashItem.Evasion + ",";
            sql += StashItem.EnergyShield + ",";

            //Weapons
            sql += StashItem.AttacksPerSecond + ",";
            sql += StashItem.PhysicalDamageMin + ",";
            sql += StashItem.PhysicalDamageMax + ",";
            sql += StashItem.pDPS + ",";
            sql += StashItem.ElementalDamageMin + ",";
            sql += StashItem.ElementalDamageMin + ",";
            sql += StashItem.eDPS + ",";
            sql += StashItem.tDPS + ",";

            //Implict Affix
            sql += (StashItem.Affix[0].Mod1.Id == 0 ? "NULL" : StashItem.Affix[0].Mod1.Id.ToString()) + ",";
            sql += (StashItem.Affix[0].Mod1.Value == 0 ? "NULL" : StashItem.Affix[0].Mod1.Value.ToString()) + ",";
            sql += (StashItem.Affix[0].Mod2.Id == 0 ? "NULL" : StashItem.Affix[0].Mod2.Id.ToString()) + ",";
            sql += (StashItem.Affix[0].Mod2.Value == 0 ? "NULL" : StashItem.Affix[0].Mod2.Value.ToString()) + ",";

            //Sockets
            //Socket Count
            sql += StashItem.Sockets.Replace("-", "").Replace(" ", "").Length + ",";

            //Max Socket Link
            var chains = StashItem.Sockets.Split(' ');
            int maxChain = 0;
            foreach (var chain in chains)
            {
                int chainLength = chain.Replace("-", "").Length;
                if (chainLength > maxChain)
                    maxChain = chainLength;
            }
            sql += maxChain + ",";

            //Life, Mana
            sql += StashItem.Life + ",";
            sql += StashItem.Mana + ",";

            //Resistances
            sql += StashItem.FireRes + ",";
            sql += StashItem.ColdRes + ",";
            sql += StashItem.LightningRes + ",";
            sql += StashItem.ChaosRes + ",";

            //Original Text
            sql += "'" + StashItem.OriginalText.Replace("'", "''") + "')";

            //Stash this item
            RunQuery(sql);

            //This is particularly nasty, but I don't know how else to get the StashId for the item we just stashed
            int stashId = GetScalarInt("SELECT MAX(StashId) FROM Stash;");

            //Now stash the affixes
            //Turned off for now as we don't strictly need them
            //for (int key = 1; key < 7; key++)
            //{
            //    sql = "INSERT INTO StashAffix(StashId, AffixType, AffixId, Mod1Id, Mod1Value, Mod2Id, Mod2Value) VALUES (";
            //    sql += stashId + ",";
            //    sql += (key < 4 ? "'Prefix'" : "'Suffix'") + ",";
            //    sql += (StashItem.Affix[key].AffixId == 0 ? "NULL" : StashItem.Affix[key].AffixId.ToString()) + ",";
            //    sql += (StashItem.Affix[key].Mod1.Id == 0 ? "NULL" : StashItem.Affix[key].Mod1.Id.ToString()) + ",";
            //    sql += (StashItem.Affix[key].Mod1.Value == 0 ? "NULL" : StashItem.Affix[key].Mod1.Value.ToString()) + ",";
            //    sql += (StashItem.Affix[key].Mod2.Id == 0 ? "NULL" : StashItem.Affix[key].Mod2.Id.ToString()) + ",";
            //    sql += (StashItem.Affix[key].Mod2.Value == 0 ? "NULL" : StashItem.Affix[key].Mod2.Value.ToString()) + ")";
            //    GlobalMethods.RunQuery(sql);
            //}

            //Finally stash the mods
            for (int mod = 0; mod < 20; mod++)
            {
                if (StashItem.Mod[mod].Id == 0)
                    break;
                sql = "INSERT INTO StashMod(StashId, StashModId, ModId, ModValue) VALUES (";
                sql += stashId + ",";
                sql += (mod + 1) + ",";
                sql += StashItem.Mod[mod].Id + ",";
                sql += StashItem.Mod[mod].Value + ")";
                RunQuery(sql);
            }
        }
    }
}
