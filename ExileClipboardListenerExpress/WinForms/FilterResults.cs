using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ExileClipboardListener.Classes;
using ExileClipboardListener.Properties;

namespace ExileClipboardListener.WinForms
{
    public partial class FilterResults : Form
    {
        private int _filterId;
        private int _itemTypeId;
        private int _itemSubTypeId;
        private readonly List<int> _filters = new List<int>();

        public FilterResults()
        {
            InitializeComponent();
        }

        private void FilterResults_Load(object sender, EventArgs e)
        {
            LoadModClasses();
            LoadMods();
            RefreshForm();

            //Set the default tab on the analysis section
            tabControl1.SelectedIndex = Properties.Settings.Default.DefaultTabId;
        }

        private void LoadModClasses()
        {
            //Load the mod classes
            for (int affix = 0; affix < 6; affix++)
            {
                string affixType = affix < 3 ? "Prefix" : "Suffix";
                string affixName = affix < 3 ? "Prefix" + (affix + 1) : "Suffix" + (affix - 2);
                string sql = "SELECT '(Any)' UNION ALL SELECT '' UNION ALL SELECT DISTINCT ModClass FROM Mod m INNER JOIN " + affixType + " a ON a.Mod1Id = m.ModId OR a.Mod2Id = m.ModId WHERE ModClass IS NOT NULL";
                sql += " ORDER BY 1;";

                //Push this into the combo
                var affixModClass = (ComboBox)tabControl1.Controls.Find(affixName + "ModClass", true)[0];
                GlobalMethods.StuffCombo(sql, affixModClass);
                if (affixModClass.Items.Count > 0)
                    affixModClass.SelectedIndex = 0;
            }
        }

        private void LoadMods()
        {
            //Load the mods
            for (int affix = 0; affix < 6; affix++)
            {
                string affixType = affix < 3 ? "Prefix" : "Suffix";
                string affixName = affix < 3 ? "Prefix" + (affix + 1) : "Suffix" + (affix - 2);
                string sql = "SELECT '(Any)' UNION ALL SELECT '' UNION ALL SELECT DISTINCT m.ModName FROM Mod m INNER JOIN " + affixType + " a ON a.Mod1Id = m.ModId OR a.Mod2Id = m.ModId WHERE ModClass IS NOT NULL"; 
                sql += " ORDER BY 1;";

                //Push this into the combo
                var affixMod = (ComboBox)tabControl1.Controls.Find(affixName + "Mod", true)[0];
                GlobalMethods.StuffCombo(sql, affixMod);
                if (affixMod.Items.Count > 0)
                    affixMod.SelectedIndex = 0;
            }
        }

        private void RefreshForm()
        {
            //Load up the current stash item onto the form
            Rarity.Text = GlobalMethods.GetScalarString("SELECT RarityName FROM Rarity WHERE RarityId = " + GlobalMethods.StashItem.RarityId + ";");
            ItemName.Text = GlobalMethods.StashItem.ItemName;
            BaseItemName.Text = GlobalMethods.BaseItem.ItemName; //GlobalMethods.GetScalarString("SELECT ItemName FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
            Quality.Text = GlobalMethods.StashItem.Quality.ToString();
            ItemType.Text = GlobalMethods.StashItem.ItemTypeName;
            ItemSubType.Text = GlobalMethods.StashItem.ItemSubTypeName;

            //Defense
            BaseArmour.Text = GlobalMethods.BaseItem.Armour.ToString(); // GlobalMethods.GetScalarInt("SELECT Armour FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";").ToString();
            BaseEvasion.Text = GlobalMethods.BaseItem.Evasion.ToString(); // GlobalMethods.GetScalarInt("SELECT Evasion FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";").ToString();
            BaseEnergyShield.Text = GlobalMethods.BaseItem.EnergyShield.ToString(); // GlobalMethods.GetScalarInt("SELECT EnergyShield FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";").ToString();
            Armour.Text = GlobalMethods.StashItem.Armour.ToString();
            Evasion.Text = GlobalMethods.StashItem.Evasion.ToString();
            EnergyShield.Text = GlobalMethods.StashItem.EnergyShield.ToString();

            //Weapons
            PhysicalDamageFrom.Text = GlobalMethods.StashItem.PhysicalDamageMin.ToString();
            PhysicalDamageTo.Text = GlobalMethods.StashItem.PhysicalDamageMax.ToString();
            ElementalDamageFrom.Text = GlobalMethods.StashItem.ElementalDamageMin.ToString();
            ElementalDamageTo.Text = GlobalMethods.StashItem.ElementalDamageMax.ToString();
            BaseAttackSpeed.Text = GlobalMethods.StashItem.BaseAttacksPerSecond.ToString();
            AttackSpeed.Text = GlobalMethods.StashItem.AttacksPerSecond.ToString();
            BaseDPS.Text = GlobalMethods.StashItem.DamagePerSecond.ToString();

            //Work out the pDPS and eDPS
            decimal attackSpeed;
            if (decimal.TryParse(AttackSpeed.Text, out attackSpeed))
            {
                int damMin;
                int damMax;
                if (int.TryParse(PhysicalDamageFrom.Text, out damMin) && int.TryParse(PhysicalDamageTo.Text, out damMax))
                    PhysicalDPS.Text = (((damMax + damMin) / 2) * attackSpeed).ToString("#0.00");
                if (int.TryParse(ElementalDamageFrom.Text, out damMin) && int.TryParse(ElementalDamageTo.Text, out damMax))
                    ElementalDPS.Text = (((damMax + damMin) / 2) * attackSpeed).ToString("#0.00");
            }

            //Requirements
            ItemLevel.Text = GlobalMethods.StashItem.ItemLevel.ToString();
            ReqLevel.Text = GlobalMethods.StashItem.ReqLevel.ToString();
            ReqLevelBase.Text = GlobalMethods.StashItem.ReqLevelBase.ToString();
            ReqStr.Text = GlobalMethods.BaseItem.ReqStr.ToString(); // GlobalMethods.GetScalarInt("SELECT ReqStr FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";").ToString();
            ReqDex.Text = GlobalMethods.BaseItem.ReqDex.ToString(); // GlobalMethods.GetScalarInt("SELECT ReqDex FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";").ToString();
            ReqInt.Text = GlobalMethods.BaseItem.ReqInt.ToString(); // GlobalMethods.GetScalarInt("SELECT ReqInt FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";").ToString();

            //Sockets
            Sockets.Text = GlobalMethods.StashItem.Sockets;

            //Show all mods
            foreach (GlobalMethods.Mod m in GlobalMethods.StashItem.Mod)
            {
                if (m.Id != 0)
                {
                    var row = new object[5];
                    row[ModIdColumn.Index] = m.Id;
                    row[ModNameColumn.Index] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                    row[ModValueMinColumn.Index] = m.ValueMin;
                    row[ModValueMaxColumn.Index] = m.ValueMax;
                    row[ModItemValueColumn.Index] = m.Value;
                    ModGrid.Rows.Add(row);
                }
            }

            //Show all affixes
            bool seenSecondary = false;
            for (int i = 0; i < 7; i++)
            {
                var row = new object[11];
                int affixId = GlobalMethods.StashItem.Affix[i].AffixId;
                if (GlobalMethods.StashItem.Affix[i].Mod1.Value != 0)
                {
                    string affixName = i == 0 ? "Implicit" : (i < 4 ? "Prefix" : "Suffix");
                    row[AffixIdColumn.Index] = affixId;
                    row[AffixTypeColumn.Index] = affixName;
                    if (i == 0)
                    {
                        row[AffixLevelColumn.Index] = "";
                        row[AffixCategoryColumn.Index] = "";
                        row[AffixNameColumn.Index] = "";
                    }
                    else
                    {
                        row[AffixLevelColumn.Index] = GlobalMethods.StashItem.Affix[i].Level;
                        row[AffixCategoryColumn.Index] = GlobalMethods.GetScalarString("SELECT ModCategoryName FROM ModCategory mc INNER JOIN " + affixName + " a ON a.ModCategoryId = mc.ModCategoryId WHERE a." + affixName + "Id = " + affixId + ";");
                        row[AffixNameColumn.Index] = GlobalMethods.GetScalarString("SELECT Name FROM " + affixName + " WHERE " + affixName + "Id = " + affixId + ";");
                    }
                    row[AffixPrimaryModNameColumn.Index] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + GlobalMethods.StashItem.Affix[i].Mod1.Id + ";");
                    row[AffixPrimaryModRangeColumn.Index] = GlobalMethods.StashItem.Affix[i].Mod1.ValueMin + "-" + GlobalMethods.StashItem.Affix[i].Mod1.ValueMax;
                    row[AffixPrimaryModValueColumn.Index] = GlobalMethods.StashItem.Affix[i].Mod1.Value;
                    if (GlobalMethods.StashItem.Affix[i].Mod2.Value == 0)
                    {
                        row[AffixSecondaryModNameColumn.Index] = "";
                        row[AffixSecondaryModRangeColumn.Index] = "";
                        row[AffixSecondaryModValueColumn.Index] = "";
                    }
                    else
                    {
                        seenSecondary = true;
                        row[AffixSecondaryModNameColumn.Index] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + GlobalMethods.StashItem.Affix[i].Mod2.Id + ";");
                        row[AffixSecondaryModRangeColumn.Index] = GlobalMethods.StashItem.Affix[i].Mod2.ValueMin + "-" + GlobalMethods.StashItem.Affix[i].Mod2.ValueMax;
                        row[AffixSecondaryModValueColumn.Index] = GlobalMethods.StashItem.Affix[i].Mod2.Value;
                    }
                    AffixGrid.Rows.Add(row);
                }

                //Hide the secondary mods if we don't need to show them
                HideSecondary.Checked = !seenSecondary;

                //Turn off the option to turn them back on if there aren't any
                HideSecondary.Enabled = seenSecondary;
            }
            AffixDetails.Enabled = AffixGrid.Rows.Count > 0;

            //Refresh the filters
            RefreshFilters();
        }

        private void RefreshFilters()
        {
            //Show the filters that match the current item and score them
            _itemTypeId = GlobalMethods.GetScalarInt("SELECT ItemTypeId FROM ItemType WHERE ItemTypeName = '" + GlobalMethods.StashItem.ItemTypeName + "';");
            _itemSubTypeId = GlobalMethods.GetScalarInt("SELECT ItemSubTypeId FROM ItemSubType WHERE ItemSubTypeName = '" + GlobalMethods.StashItem.ItemSubTypeName + "';");

            //Make a list of matching filters
            GlobalMethods.StuffIntList("SELECT FilterId FROM FilterHeader WHERE (ItemTypeId = 0 OR ItemTypeId = " + _itemTypeId + ") AND (ItemSubTypeId = 0 OR ItemSubTypeId = " + _itemSubTypeId + ");", _filters);

            //Now score each filter and add it to the results
            AverageScoreColumn.DefaultCellStyle.Format = "0\\%";
            foreach (int filterId in _filters)
            {
                int filterScore = ScoreFilter(filterId);
                var row = new object[2];
                row[0] = GlobalMethods.GetScalarString("SELECT FilterName FROM FilterHeader WHERE FilterId = " + filterId + ";");
                row[1] = filterScore;
                FilterResultsGrid.Rows.Add(row);
            }

            //Sort in reverse score order
            FilterResultsGrid.Sort(FilterResultsGrid.Columns[1], ListSortDirection.Descending);
        }

        private int ScoreFilter(int filterId, bool showDetail = false, int filterAffixSlot = 0)
        {
            //This is where the fun begins, we pull out the various mods we were looking for and mark the item against them
            //We return a score but we also load the specified filter and the results into a static class in case we need to do more work with them
            int runningTotalActual = 0;
            int filterCount = 0;
            for (int affixSlot = (filterAffixSlot == 0 ? 1 : filterAffixSlot); affixSlot <= (filterAffixSlot == 0 ? 6 : filterAffixSlot); affixSlot++)
            {
                string affixType = affixSlot < 4 ? "Prefix" : "Suffix";
                string affixName = affixSlot < 4 ? "Prefix" + affixSlot : "Suffix" + (affixSlot - 3);
                string modClass = GlobalMethods.GetScalarString("SELECT ModClass FROM FilterDetail WHERE FilterId = " + filterId + " AND AffixSlot = " + affixSlot + ";");
                int modId = GlobalMethods.GetScalarInt("SELECT ModId FROM FilterDetail WHERE FilterId = " + filterId + " AND AffixSlot = " + affixSlot + ";");
             
                //Clean down any previous values
                if (showDetail)
                {
                    tabControl1.Controls.Find(affixName + "ILevel", true)[0].Text = "";
                    tabControl1.Controls.Find(affixName + "CLevel", true)[0].Text = "";
                    tabControl1.Controls.Find(affixName + "Slot", true)[0].Text = "";
                    ((PictureBox)Controls.Find(affixName + "Smiley", true)[0]).Image = null;
                }

                //If there is nothing to score for this slot then continue
                if (modClass == "" && modId == 0)
                    continue;

                //First we need to know which mods the item could have 
                filterCount++;
                var mods = new List<GlobalMethods.Mod>();

                //If they specified a filter with a mod class AND a specific mod then great, we just use that mod
                if (modId != 0)
                    mods.Add(new GlobalMethods.Mod { Id = modId });

                //Otherwise we have to work out which mods the item could have based on the mod class
                else
                {
                    var match = new List<int>();
                    string sql = "SELECT m.ModId FROM Mod m WHERE m.ModClass = '" + modClass + "'";

                    //We filter by the current item type
                    if (GlobalMethods.StashItem.ItemTypeName == "Weapons")
                        sql += " AND IFNULL(m.Weapons, 1) = 1";
                    else if (GlobalMethods.StashItem.ItemTypeName == "Armour")
                        sql += " AND IFNULL(m.Armour, 1) = 1";
                    else if (GlobalMethods.StashItem.ItemTypeName == "Jewellery")
                        sql += " AND IFNULL(m.Jewellery, 1) = 1";

                    //For Defense we also need to make the base type of the body armour matches the mod, e.g. we wouldn't get evasion on an AR armour
                    if (modClass == "Defense")
                    {
                        if (GlobalMethods.StashItem.Armour != 0)
                        {
                            if (GlobalMethods.StashItem.Evasion != 0)
                                sql += " AND (m.ModRealName = 'increased Armour and Evasion' OR m.ModRealName = 'to Armour' OR m.ModRealName = 'to Evasion Rating')";
                            else if (GlobalMethods.StashItem.EnergyShield != 0)
                                sql += " AND (m.ModRealName = 'increased Armour and Energy Shield' OR m.ModRealName = 'to Armour' OR m.ModRealName = 'to Energy Shield')";
                            else
                                sql += " AND (m.ModRealName = 'increased Armour' OR m.ModRealName = 'to Armour')";
                        }
                        else if (GlobalMethods.StashItem.Evasion != 0)
                        {
                            if (GlobalMethods.StashItem.EnergyShield != 0)
                                sql += " AND (m.ModRealName = 'increased Evasion and Energy Shield' OR m.ModRealName = 'to Evasion Rating' OR m.ModRealName = 'to Energy Shield')";
                            else
                                sql += " AND  (m.ModRealName = 'increased Evasion' OR m.ModRealName = 'to Evasion Rating')";
                        }
                        else if (GlobalMethods.StashItem.EnergyShield != 0)
                            sql += " AND (m.ModRealName = 'increased Energy Shield' OR m.ModRealName = 'to Energy Shield')";
                    }
                    sql += " AND IFNULL(m.ModPair, 2) = 2";
                    sql += " AND EXISTS (SELECT * FROM " + affixType + " a WHERE a.Mod1Id = m.ModId OR a.Mod2Id = m.ModId);";
                    GlobalMethods.StuffIntList(sql, match);
                    foreach (int id in match)
                        mods.Add(new GlobalMethods.Mod { Id = id });
                }

                //Now we have a list it's simply a matter of tallying up the scores for each mod
                int score = 0;
                int modsHit = 0;
                int runningTotalILevel = 0;
                int runningTotalCLevel = 0;
                int runningTotalSlot = 0;
                GlobalMethods.BISResults.Clear();
                GlobalMethods.ILevelResults.Clear();
                GlobalMethods.CLevelResults.Clear();
                GlobalMethods.ItemResults.Clear();
                foreach (GlobalMethods.Mod m in mods)
                {
                    //First the BIS roll
                    int maxPrimary = GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM " + affixType + " WHERE Mod1Id = " + m.Id + ";");
                    int maxSecondary = GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM " + affixType + " WHERE Mod2Id = " + m.Id + ";");
                    int maxSlot = maxPrimary == 0 ? maxSecondary : maxPrimary;

                    //Now backfill the best in slot data
                    if (maxPrimary != 0)
                    {
                        var row = new object[6];
                        row[0] = GlobalMethods.GetScalarString("SELECT Name FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxPrimary + ";");
                        row[1] = "Primary";
                        row[2] = GlobalMethods.GetScalarString("SELECT Level FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxPrimary + ";");
                        row[3] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                        row[4] = GlobalMethods.GetScalarString("SELECT Mod1ValueMin FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxPrimary + ";");
                        row[5] = GlobalMethods.GetScalarString("SELECT Mod1ValueMax FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxPrimary + ";");
                        GlobalMethods.BISResults.Add(row);
                    }
                    if (maxSecondary != 0)
                    {
                        var row = new object[6];
                        row[0] = GlobalMethods.GetScalarString("SELECT Name FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSecondary + ";");
                        row[1] = "Secondary";
                        row[2] = GlobalMethods.GetScalarString("SELECT Level FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSecondary + ";");
                        row[3] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                        row[4] = GlobalMethods.GetScalarString("SELECT Mod2ValueMin FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSecondary + ";");
                        row[5] = GlobalMethods.GetScalarString("SELECT Mod2ValueMax FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSecondary + ";");
                        GlobalMethods.BISResults.Add(row);
                    }

                    //Next we do the same but use item level as well
                    maxPrimary = GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Level <= " + GlobalMethods.StashItem.ItemLevel + ";");
                    maxSecondary = GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Level <= " + GlobalMethods.StashItem.ItemLevel + ";");
                    int maxILevel = maxPrimary == 0 ? maxSecondary : maxPrimary;

                    //Now backfill the item level data
                    if (maxPrimary != 0)
                    {
                        var row = new object[6];
                        row[0] = GlobalMethods.GetScalarString("SELECT Name FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxPrimary + ";");
                        row[1] = "Primary";
                        row[2] = GlobalMethods.GetScalarString("SELECT Level FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxPrimary + ";");
                        row[3] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                        row[4] = GlobalMethods.GetScalarString("SELECT Mod1ValueMin FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxPrimary + ";");
                        row[5] = GlobalMethods.GetScalarString("SELECT Mod1ValueMax FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxPrimary + ";");
                        GlobalMethods.ILevelResults.Add(row);
                    }
                    if (maxSecondary != 0)
                    {
                        var row = new object[6];
                        row[0] = GlobalMethods.GetScalarString("SELECT Name FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSecondary + ";");
                        row[1] = "Secondary";
                        row[2] = GlobalMethods.GetScalarString("SELECT Level FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSecondary + ";");
                        row[3] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                        row[4] = GlobalMethods.GetScalarString("SELECT Mod2ValueMin FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSecondary + ";");
                        row[5] = GlobalMethods.GetScalarString("SELECT Mod2ValueMax FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSecondary + ";");
                        GlobalMethods.ILevelResults.Add(row);
                    }

                    //Next we do the same but use item level as well
                    maxPrimary = GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Level <= " + CharacterLevel.Value + ";");
                    maxSecondary = GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Level <= " + CharacterLevel.Value + ";");
                    int maxCLevel = maxPrimary == 0 ? maxSecondary : maxPrimary;

                    //Now backfill the item level data
                    if (maxPrimary != 0)
                    {
                        var row = new object[6];
                        row[0] = GlobalMethods.GetScalarString("SELECT Name FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxPrimary + ";");
                        row[1] = "Primary";
                        row[2] = GlobalMethods.GetScalarString("SELECT Level FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxPrimary + ";");
                        row[3] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                        row[4] = GlobalMethods.GetScalarString("SELECT Mod1ValueMin FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxPrimary + ";");
                        row[5] = GlobalMethods.GetScalarString("SELECT Mod1ValueMax FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxPrimary + ";");
                        GlobalMethods.CLevelResults.Add(row);
                    }
                    if (maxSecondary != 0)
                    {
                        var row = new object[6];
                        row[0] = GlobalMethods.GetScalarString("SELECT Name FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSecondary + ";");
                        row[1] = "Secondary";
                        row[2] = GlobalMethods.GetScalarString("SELECT Level FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSecondary + ";");
                        row[3] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                        row[4] = GlobalMethods.GetScalarString("SELECT Mod2ValueMin FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSecondary + ";");
                        row[5] = GlobalMethods.GetScalarString("SELECT Mod2ValueMax FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSecondary + ";");
                        GlobalMethods.CLevelResults.Add(row);
                    }

                    //Finally we need to do this all one last time for the item rolls
                    int itemModScore = 0;
                    for (int mod = 0; mod < 10; mod++)
                    {
                        if (GlobalMethods.StashItem.Mod[mod].Id == m.Id)
                        {
                            itemModScore = GlobalMethods.StashItem.Mod[mod].Value;
                            string position = GlobalMethods.GetScalarString("SELECT Name FROM " + affixType + " WHERE Mod1Id = " + m.Id + ";") == "" ? "Secondary" : "Primary";
                            if (position == "Primary")
                            {
                                var row = new object[7];
                                row[0] = GlobalMethods.GetScalarString("SELECT Name FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                                row[1] = "Primary";
                                row[2] = GlobalMethods.GetScalarString("SELECT Level FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                                row[3] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                                row[4] = GlobalMethods.GetScalarString("SELECT Mod1ValueMin FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                                row[5] = GlobalMethods.GetScalarString("SELECT Mod1ValueMax FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                                row[6] = GlobalMethods.StashItem.Mod[mod].Value;
                                GlobalMethods.ItemResults.Add(row);
                            }
                            if (position == "Secondary")
                            {
                                var row = new object[7];
                                row[0] = GlobalMethods.GetScalarString("SELECT Name FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod2ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                                row[1] = "Secondary";
                                row[2] = GlobalMethods.GetScalarString("SELECT Level FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod2ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                                row[3] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                                row[4] = GlobalMethods.GetScalarString("SELECT Mod2ValueMin FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod2ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                                row[5] = GlobalMethods.GetScalarString("SELECT Mod2ValueMax FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod2ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                                row[6] = GlobalMethods.StashItem.Mod[mod].Value;
                                GlobalMethods.ItemResults.Add(row);
                            }
                        }
                    }

                    //Now we can rate the mod
                    int scoreILevel = maxILevel == 0 ? 0 : itemModScore * 100 / maxILevel;
                    int scoreCLevel = maxCLevel == 0 ? 0 : itemModScore * 100 / maxCLevel;
                    int scoreSlot = maxSlot == 0 ? 0 : itemModScore * 100 / maxSlot;
                    if (itemModScore != 0)
                    {
                        score = Properties.Settings.Default.RatingMode == 0 ? scoreSlot : scoreILevel;
                    }

                    //Tally the scores
                    runningTotalILevel += scoreILevel;
                    runningTotalCLevel += scoreCLevel;
                    runningTotalSlot += scoreSlot;
                    runningTotalActual += score;
                    modsHit++;
                }

                //Dump the results out to the form if this option is on
                if (showDetail)
                {
                    tabControl1.Controls.Find(affixName + "ILevel", true)[0].Text = (runningTotalILevel == 0 ? 0 : runningTotalILevel / modsHit) + "%";
                    tabControl1.Controls.Find(affixName + "CLevel", true)[0].Text = (runningTotalCLevel == 0 ? 0 : runningTotalCLevel/ modsHit) + "%";
                    tabControl1.Controls.Find(affixName + "Slot", true)[0].Text = (runningTotalSlot == 0 ? 0 : runningTotalSlot / modsHit) + "%";
                    Image smiley;
                    if (score <= Properties.Settings.Default.TolerancePoorTo)
                        smiley = Resources.PoorSmall;
                    else if (score <= Properties.Settings.Default.ToleranceAverageTo)
                        smiley = Resources.AverageSmall;
                    else
                        smiley = Resources.GoodSmall;
                    ((PictureBox)Controls.Find(affixName + "Smiley", true)[0]).Image = smiley;
                }
            }

            //Return a single score
            return filterCount == 0 ? 0 : runningTotalActual / filterCount;
        }

        private void FilterResultsGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (FilterResultsGrid.CurrentRow == null)
                return;

            //Determine the filter
            _filterId = GlobalMethods.GetScalarInt("SELECT FilterId FROM FilterHeader WHERE FilterName = '" + FilterResultsGrid.CurrentRow.Cells[0].Value + "';");

            //Load up the mods into the controls
            for (int affixSlot = 1; affixSlot < 7; affixSlot++)
            {
                string affixName = affixSlot < 4 ? "Prefix" + affixSlot : "Suffix" + (affixSlot - 3);
                string modClass = GlobalMethods.GetScalarString("SELECT ModClass FROM FilterDetail WHERE FilterId = " + _filterId + " AND AffixSlot = " + affixSlot + ";");
                int modId = GlobalMethods.GetScalarInt("SELECT ModId FROM FilterDetail WHERE FilterId = " + _filterId + " AND AffixSlot = " + affixSlot + ";");
                if (modClass != "")
                {
                    Controls.Find(affixName + "ModClass", true)[0].Text = modClass;
                    Controls.Find(affixName + "Details", true)[0].Enabled = true;
                }
                else if (modId == 0)
                {
                    Controls.Find(affixName + "ModClass", true)[0].Text = "";
                    Controls.Find(affixName + "Details", true)[0].Enabled = false;
                }
                else
                    ((ComboBox)Controls.Find(affixName + "ModClass", true)[0]).SelectedIndex = 1;
                if (modId != 0)
                    Controls.Find(affixName + "Mod", true)[0].Text = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + modId + ";");
                else if (modClass == "")
                    Controls.Find(affixName + "Mod", true)[0].Text = "";
                else
                    ((ComboBox)Controls.Find(affixName + "Mod", true)[0]).SelectedIndex = 1;
            }

            //Show a breakdown of the score
            ScoreFilter(_filterId, true);
        }

        private void RefreshResults_Click(object sender, EventArgs e)
        {
            RefreshFilters();
        }

        private void HideSecondary_CheckedChanged(object sender, EventArgs e)
        {
            //bool seenSecondary = false;
            //for (int i = 0; i < AffixGrid.Rows.Count; i++)
            //    if (AffixGrid.Rows[i].Cells[7].Value.ToString() != "")
            //        seenSecondary = true;
            AffixGrid.Columns[AffixSecondaryModNameColumn.Index].Visible = !HideSecondary.Checked;
            AffixGrid.Columns[AffixSecondaryModRangeColumn.Index].Visible = !HideSecondary.Checked;
            AffixGrid.Columns[AffixSecondaryModValueColumn.Index].Visible = !HideSecondary.Checked;
        }

        private void AffixDetails_Click(object sender, EventArgs e)
        {
            if (AffixGrid.CurrentRow == null)
                return;
            int affixId = Convert.ToInt32(AffixGrid.CurrentRow.Cells[AffixIdColumn.Index].Value);
            string affixType = AffixGrid.CurrentRow.Cells[AffixTypeColumn.Index].Value.ToString();
            int mod1Id = GlobalMethods.GetScalarInt("SELECT ModId FROM Mod WHERE ModName = '" + AffixGrid.CurrentRow.Cells[AffixPrimaryModNameColumn.Index].Value + "';");
            int mod2Id = GlobalMethods.GetScalarInt("SELECT ModId FROM Mod WHERE ModName = '" + AffixGrid.CurrentRow.Cells[AffixSecondaryModNameColumn.Index].Value + "';");
            new AffixDetails { AffixId = affixId, AffixType = affixType, Mod1Id = mod1Id, Mod2Id = mod2Id }.ShowDialog();
        }

        private void ModDetails_Click(object sender, EventArgs e)
        {
            if (ModGrid.CurrentRow == null)
                return;
            int modId = Convert.ToInt32(ModGrid.CurrentRow.Cells[ModIdColumn.Index].Value);
            new ModDetails { ModId = modId }.ShowDialog();
        }

        private void Prefix1Details_Click(object sender, EventArgs e)
        {
            ScoreFilter(_filterId, false, 1);
            new RollDetails { AffixTypeString = "Prefix", ModClassName = Prefix1ModClass.Text, ModNameString = Prefix1Mod.Text }.ShowDialog();
        }

        private void Prefix2Details_Click(object sender, EventArgs e)
        {
            ScoreFilter(_filterId, false, 2);
            new RollDetails { AffixTypeString = "Prefix", ModClassName = Prefix2ModClass.Text, ModNameString = Prefix2Mod.Text }.ShowDialog();
        }

        private void Prefix3Details_Click(object sender, EventArgs e)
        {
            ScoreFilter(_filterId, false, 3);
            new RollDetails { AffixTypeString = "Prefix", ModClassName = Prefix3ModClass.Text, ModNameString = Prefix3Mod.Text }.ShowDialog();
        }

        private void Suffix1Details_Click(object sender, EventArgs e)
        {
            ScoreFilter(_filterId, false, 4);
            new RollDetails { AffixTypeString = "Suffix", ModClassName = Suffix1ModClass.Text, ModNameString = Suffix1Mod.Text }.ShowDialog();
        }

        private void Suffix2Details_Click(object sender, EventArgs e)
        {
            ScoreFilter(_filterId, false, 5);
            new RollDetails { AffixTypeString = "Suffix", ModClassName = Suffix2ModClass.Text, ModNameString = Suffix2Mod.Text }.ShowDialog();
        }

        private void Suffix3Details_Click(object sender, EventArgs e)
        {
            ScoreFilter(_filterId, false, 6);
            new RollDetails { AffixTypeString = "Suffix", ModClassName = Suffix3ModClass.Text, ModNameString = Suffix3Mod.Text }.ShowDialog();
        }
    }
}
