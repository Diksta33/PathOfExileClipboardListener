using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExileClipboardListener.Classes;
using ExileClipboardListener.Properties;

namespace ExileClipboardListener.WinForms
{
    public partial class FilterResults : Form
    {
        private int _itemTypeId;
        private int _itemSubTypeId;
        private List<int> _filters = new List<int>();

        public FilterResults()
        {
            InitializeComponent();
        }

        private void FilterResults_Load(object sender, EventArgs e)
        {
            LoadModClasses();
            LoadMods();
            RefreshForm();
        }

        private void LoadModClasses()
        {
            //Load the mod classes
            for (int affix = 0; affix < 6; affix++)
            {
                string AffixType = affix < 3 ? "Prefix" : "Suffix";
                string AffixName = affix < 3 ? "Prefix" + (affix + 1) : "Suffix" + (affix - 2);
                string sql = "SELECT '(Any)' UNION ALL SELECT '' UNION ALL SELECT DISTINCT ModClass FROM Mod m INNER JOIN " + AffixType + " a ON a.Mod1Id = m.ModId OR a.Mod2Id = m.ModId WHERE ModClass IS NOT NULL";
                sql += " ORDER BY 1;";

                //Push this into the combo
                ComboBox AffixModClass = (ComboBox)tabControl1.Controls.Find(AffixName + "ModClass", true)[0];
                GlobalMethods.StuffCombo(sql, AffixModClass);
                if (AffixModClass.Items.Count > 0)
                    AffixModClass.SelectedIndex = 0;
            }
        }

        private void LoadMods()
        {
            //Load the mods
            for (int affix = 0; affix < 6; affix++)
            {
                string AffixType = affix < 3 ? "Prefix" : "Suffix";
                string AffixName = affix < 3 ? "Prefix" + (affix + 1) : "Suffix" + (affix - 2);
                string sql = "SELECT '(Any)' UNION ALL SELECT '' UNION ALL SELECT DISTINCT m.ModName FROM Mod m INNER JOIN " + AffixType + " a ON a.Mod1Id = m.ModId OR a.Mod2Id = m.ModId WHERE ModClass IS NOT NULL"; 
                sql += " ORDER BY 1;";

                //Push this into the combo
                ComboBox AffixMod = (ComboBox)tabControl1.Controls.Find(AffixName + "Mod", true)[0];
                GlobalMethods.StuffCombo(sql, AffixMod);
                if (AffixMod.Items.Count > 0)
                    AffixMod.SelectedIndex = 0;
            }
        }

        private void RefreshForm()
        {
            //Load up the current stash item onto the form
            Rarity.Text = GlobalMethods.GetScalarString("SELECT RarityName FROM Rarity WHERE RarityId = " + GlobalMethods.StashItem.RarityId + ";");
            ItemName.Text = GlobalMethods.StashItem.ItemName;
            BaseItemName.Text = GlobalMethods.GetScalarString("SELECT ItemName FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
            Quality.Text = GlobalMethods.StashItem.Quality.ToString();
            ItemType.Text = GlobalMethods.StashItem.ItemTypeName;
            ItemSubType.Text = GlobalMethods.StashItem.ItemSubTypeName;

            //Defense
            BaseArmour.Text = GlobalMethods.GetScalarInt("SELECT Armour FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";").ToString();
            BaseEvasion.Text = GlobalMethods.GetScalarInt("SELECT Evasion FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";").ToString();
            BaseEnergyShield.Text = GlobalMethods.GetScalarInt("SELECT EnergyShield FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";").ToString();
            Armour.Text = GlobalMethods.StashItem.Armour.ToString();
            Evasion.Text = GlobalMethods.StashItem.Evasion.ToString();
            EnergyShield.Text = GlobalMethods.StashItem.EnergyShield.ToString();

            //Weapons
            PhysicalDamageFrom.Text = GlobalMethods.StashItem.PhysicalDamageMin.ToString();
            PhysicalDamageTo.Text = GlobalMethods.StashItem.PhysicalDamageMax.ToString();
            ElementalDamageFrom.Text = GlobalMethods.StashItem.ElementalDamageMin.ToString();
            ElementalDamageTo.Text = GlobalMethods.StashItem.ElementalDamageMax.ToString();
            AttackSpeed.Text = GlobalMethods.StashItem.AttacksPerSecond.ToString();
            DPS.Text = GlobalMethods.StashItem.DamagePerSecond.ToString();

            //Requirements
            ItemLevel.Text = GlobalMethods.StashItem.ItemLevel.ToString();
            ReqLevel.Text = GlobalMethods.StashItem.ReqLevel.ToString();
            ReqStr.Text = GlobalMethods.GetScalarInt("SELECT ReqStr FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";").ToString();
            ReqDex.Text = GlobalMethods.GetScalarInt("SELECT ReqDex FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";").ToString();
            ReqInt.Text = GlobalMethods.GetScalarInt("SELECT ReqInt FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";").ToString();

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
                    string AffixName = i == 0 ? "Implicit" : (i < 4 ? "Prefix" : "Suffix");
                    row[AffixIdColumn.Index] = affixId;
                    row[AffixTypeColumn.Index] = AffixName;
                    if (i == 0)
                    {
                        row[AffixLevelColumn.Index] = "";
                        row[AffixCategoryColumn.Index] = "";
                        row[AffixNameColumn.Index] = "";
                    }
                    else
                    {
                        row[AffixLevelColumn.Index] = GlobalMethods.StashItem.Affix[i].Level;
                        row[AffixCategoryColumn.Index] = GlobalMethods.GetScalarString("SELECT ModCategoryName FROM ModCategory mc INNER JOIN " + AffixName + " a ON a.ModCategoryId = mc.ModCategoryId WHERE a." + AffixName + "Id = " + affixId + ";");
                        row[AffixPrimaryModValueColumn.Index] = GlobalMethods.GetScalarString("SELECT Name FROM " + AffixName + " WHERE " + AffixName + "Id = " + affixId + ";");
                    }
                    row[AffixPrimaryModValueColumn.Index] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + GlobalMethods.StashItem.Affix[i].Mod1.Id + ";");
                    row[AffixPrimaryModValueColumn.Index] = GlobalMethods.StashItem.Affix[i].Mod1.ValueMin + "-" + GlobalMethods.StashItem.Affix[i].Mod1.ValueMax;
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
            _itemSubTypeId = GlobalMethods.GetScalarInt("SELECT ItemSubTypeId FROM ItemSubType WHERE ItemSubTypeName = '" + GlobalMethods.StashItem.ItemTypeName + "';");

            //Make a list of matching filters
            GlobalMethods.StuffIntList("SELECT FilterId FROM FilterHeader WHERE IFNULL(ItemTypeId, " + _itemTypeId + ") = " + _itemTypeId + " AND IFNULL(ItemSubTypeId, " + _itemSubTypeId + ") = " + _itemSubTypeId + ";", _filters);

            //Now score each filter and add it to the results
            foreach (int filterId in _filters)
            {
                int _filterScore = ScoreFilter(filterId);
                var row = new object[2];
                row[0] = GlobalMethods.GetScalarString("SELECT FilterName FROM FilterHeader WHERE FilterId = " + filterId + ";");
                row[1] = _filterScore + "%";
                FilterResultsGrid.Rows.Add(row);
            }

            //Sort in reverse score order
            FilterResultsGrid.Sort(FilterResultsGrid.Columns[1], ListSortDirection.Descending);
        }

        private int ScoreFilter(int FilterId, bool showDetail = false)
        {
            //This is where the fun begins, we pull out the various mods we were looking for and mark the item against them
            int filterCount = 0;
            int filtersHit = 0;
            int runningTotal = 0;
            for (int affixSlot = 1; affixSlot < 7; affixSlot++)
            {
                string affixType = affixSlot < 4 ? "Prefix" : "Suffix";
                string affixName = affixSlot < 4 ? "Prefix" + affixSlot : "Suffix" + (affixSlot - 3);
                string modClass = GlobalMethods.GetScalarString("SELECT ModClass FROM FilterDetail WHERE FilterId = " + FilterId + " AND AffixSlot = " + affixSlot + ";");
                int modId = GlobalMethods.GetScalarInt("SELECT ModId FROM FilterDetail WHERE FilterId = " + FilterId + " AND AffixSlot = " + affixSlot + ";");
             
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
                    List<int> match = new List<int>();
                    string sql = "SELECT ModId FROM Mod WHERE ModClass = '" + modClass + "'";

                    //We filter by the current item type
                    if (GlobalMethods.StashItem.ItemTypeName == "Weapons")
                        sql += " AND IFNULL(Weapons, 1) = 1";
                    else if (GlobalMethods.StashItem.ItemTypeName == "Armour")
                        sql += " AND IFNULL(Armour, 1) = 1";
                    else if (GlobalMethods.StashItem.ItemTypeName == "Jewellery")
                        sql += " AND IFNULL(Jewellery, 1) = 1";

                    //For Defense we also need to make the base type of the body armour matches the mod, e.g. we wouldn't get evasion on an AR armour
                    if (modClass == "Defense")
                    {
                        if (GlobalMethods.StashItem.Armour == 0)
                            sql += " AND ModName NOT LIKE '%Armour%'";
                        if (GlobalMethods.StashItem.Evasion == 0)
                            sql += " AND ModName NOT LIKE '%Evasion%'";
                        if (GlobalMethods.StashItem.EnergyShield == 0)
                            sql += " AND ModName NOT LIKE '%Energy Shield%'";
                    }
                    sql += ";";
                    GlobalMethods.StuffIntList(sql, match);
                    foreach (int id in match)
                        mods.Add(new GlobalMethods.Mod { Id = id });
                }

                //Now we have a list it's simply a matter of tallying up the scores for each mod
                int maxILevel = 0;
                int maxCLevel = 0;
                int maxSlot = 0;
                foreach (GlobalMethods.Mod m in mods)
                {
                    //We want three values for each mod:
                    //the maximum value it could roll for the item's level
                    //the maximum value it could roll for the character's level
                    //the maximum value it could roll for the slot
                    maxILevel = GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Level < " + GlobalMethods.StashItem.ItemLevel + ";");
                    maxILevel += GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Level < " + GlobalMethods.StashItem.ItemLevel + ";");
                    maxCLevel = GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM " + affixType + " WHERE Mod1Id = " + m.Id + " AND Level < " + CharacterLevel.Value + ";");
                    maxCLevel += GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM " + affixType + " WHERE Mod2Id = " + m.Id + " AND Level < " + CharacterLevel.Value + ";");
                    maxSlot = GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM " + affixType + " WHERE Mod1Id = " + m.Id + ";");
                    maxSlot += GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM " + affixType + " WHERE Mod2Id = " + m.Id + ";");
                }

                //Next we need the values from the item that match the same list of mods
                int itemModScore = 0;
                int score = 0;
                foreach (GlobalMethods.Mod m in mods)
                {
                    for (int mod = 1; mod < 10; mod++)
                    {
                        if (GlobalMethods.StashItem.Mod[mod].Id == m.Id)
                            itemModScore += GlobalMethods.StashItem.Mod[mod].Value;
                    }
                }

                //Now we can rate the filter
                if (itemModScore != 0)
                {
                    filtersHit++;
                    score = Properties.Settings.Default.RatingMode == 0 ? (maxSlot == 0 ? 0 : itemModScore * 100 / maxSlot) : (maxCLevel == 0 ? 0 : itemModScore * 100 / maxCLevel);

                    //Dump the results out to the form if this option is on
                    if (showDetail)
                    {
                        tabControl1.Controls.Find(affixName + "ILevel", true)[0].Text = (maxILevel == 0 ? 0 : itemModScore * 100 / maxILevel) + "%";
                        tabControl1.Controls.Find(affixName + "CLevel", true)[0].Text = (maxCLevel == 0 ? 0 : itemModScore * 100 / maxCLevel) + "%";
                        tabControl1.Controls.Find(affixName + "Slot", true)[0].Text = (maxSlot == 0 ? 0 : itemModScore * 100 / maxSlot) + "%";
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

                //Tally the scores
                runningTotal += score;
            }
            return filterCount == 0 ? 0 : runningTotal / filterCount;
        }

        private void FilterResultsGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (FilterResultsGrid.CurrentRow == null)
                return;

            //Determine the filter
            int FilterId = GlobalMethods.GetScalarInt("SELECT FilterId FROM FilterHeader WHERE FilterName = '" + FilterResultsGrid.CurrentRow.Cells[0].Value + "';");

            //Load up the mods into the controls
            for (int affixSlot = 1; affixSlot < 7; affixSlot++)
            {
                string affixName = affixSlot < 4 ? "Prefix" + affixSlot : "Suffix" + (affixSlot - 3);
                string modClass = GlobalMethods.GetScalarString("SELECT ModClass FROM FilterDetail WHERE FilterId = " + FilterId + " AND AffixSlot = " + affixSlot + ";");
                int modId = GlobalMethods.GetScalarInt("SELECT ModId FROM FilterDetail WHERE FilterId = " + FilterId + " AND AffixSlot = " + affixSlot + ";");
                if (modClass != "")
                    Controls.Find(affixName + "ModClass", true)[0].Text = modClass;
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
            ScoreFilter(FilterId, true);
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
            new AffixDetails(){ AffixId = affixId }.ShowDialog();
        }
    }
}
