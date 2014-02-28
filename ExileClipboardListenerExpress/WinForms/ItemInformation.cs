using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ExileClipboardListener.Classes;
using ExileClipboardListener.Properties;
using System.Linq;
using si = ExileClipboardListener.Classes.GlobalMethods.StashItem;
using bi = ExileClipboardListener.Classes.GlobalMethods.BaseItem;

namespace ExileClipboardListener.WinForms
{
    public partial class ItemInformation : Form
    {
        public bool AllowStash = true;
        private int _filterId;
        private int _itemTypeId;
        private int _itemSubTypeId;
        private List<int> _filters = new List<int>();

        public ItemInformation()
        {
            InitializeComponent();
        }

        private void FilterResults_Load(object sender, EventArgs e)
        {
            timer1.Interval = Properties.Settings.Default.CollectionPopUpSeconds * 1000;
            if (Properties.Settings.Default.CollectionPopUpMode == 1)
                timer1.Start();
            RefreshForm();

            //Set the default tab on the analysis section
            tabControl1.SelectedIndex = Properties.Settings.Default.DefaultTabId;

            //Load the league drop-down and put in the default league if it exists
            GlobalMethods.StuffCombo("SELECT LeagueName FROM League ORDER BY 1;", League);
            string leagueName = GlobalMethods.GetScalarString("SELECT LeagueName FROM League WHERE LeagueId = " + GlobalMethods.LeagueId + ";");
            if (leagueName != "")
                League.Text = leagueName;
            else if (League.Items.Count > 0)
                League.SelectedIndex = 0;
            else
            {
                MessageBox.Show("You have no leagues, better add some before trying to stash items!");
                Hide();
            }
            AddStash.Enabled = AllowStash;

            //Set the default button (the one that is "clicked" by hitting enter)
            if (AllowStash)
            {
                AcceptButton = AddStash;
                AddStash.Focus();
            }
            else
            {
                AcceptButton = Exit;
                Exit.Focus();
            }
        }

        private void RefreshForm()
        {
            //Load up the current stash item onto the form
            Rarity.Text = GlobalMethods.GetScalarString("SELECT RarityName FROM Rarity WHERE RarityId = " + si.RarityId + ";");
            ItemName.Text = si.ItemName;
            BaseItemName.Text = bi.ItemName; 
            Quality.Text = si.Quality.ToString();
            ItemType.Text = si.ItemTypeName;
            ItemSubType.Text = si.ItemSubTypeName;

            //Defense
            BaseArmour.Text = bi.Armour.ToString();
            BaseEvasion.Text = bi.Evasion.ToString();
            BaseEnergyShield.Text = bi.EnergyShield.ToString();
            Armour.Text = si.Armour.ToString();
            Evasion.Text = si.Evasion.ToString();
            EnergyShield.Text = si.EnergyShield.ToString();

            //Weapons
            PhysicalDamageFrom.Text = si.PhysicalDamageMin.ToString();
            PhysicalDamageTo.Text = si.PhysicalDamageMax.ToString();
            ElementalDamageFrom.Text = si.ElementalDamageMin.ToString();
            ElementalDamageTo.Text = si.ElementalDamageMax.ToString();
            BaseAttackSpeed.Text = si.BaseAttacksPerSecond.ToString("#0.00");
            AttackSpeed.Text = si.AttacksPerSecond.ToString("#0.00");
            BaseDPS.Text = si.DamagePerSecond.ToString("#0.00");
            PhysicalDPS.Text = si.pDPS.ToString("#0.00");
            ElementalDPS.Text = si.eDPS.ToString("#0.00");
            TotalDPS.Text = si.tDPS.ToString("#0.00");

            //Requirements
            ItemLevel.Text = si.ItemLevel.ToString();
            ReqLevel.Text = si.ReqLevel.ToString();
            ReqLevelBase.Text = si.ReqLevelBase.ToString();
            ReqStr.Text = bi.ReqStr.ToString(); 
            ReqDex.Text = bi.ReqDex.ToString(); 
            ReqInt.Text = bi.ReqInt.ToString();

            //Sockets
            Sockets.Text = si.Sockets;

            //Icon
            var size = new Size(32 * bi.IconWidth, 32 * bi.IconHeight);
            ItemIcon.Size = size;
            ItemIcon.Image = bi.Icon;

            //Show all mods
            foreach (GlobalMethods.Mod m in si.Mod)
            {
                if (m.Id != 0)
                {
                    var row = new object[5];
                    row[ModIdColumn.Index] = m.Id;
                    row[ModNameColumn.Index] = GlobalMethods.LookUpMod(m.Id).Name;
                    row[ModItemValueColumn.Index] = m.Value;
                    ModGrid.Rows.Add(row);
                }
            }

            //Show all affixes
            bool seenSecondary = false;
            for (int i = 0; i < 7; i++)
            {
                var row = new object[11];
                int affixId = si.Affix[i].AffixId;
                if (si.Affix[i].Mod1.Value != 0)
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
                        row[AffixLevelColumn.Index] = si.Affix[i].Level;
                        row[AffixCategoryColumn.Index] = si.Affix[i].ModCategoryName;
                        row[AffixNameColumn.Index] = si.Affix[i].Name;
                    }
                    row[AffixPrimaryModNameColumn.Index] = si.Affix[i].Mod1.Name;
                    row[AffixPrimaryModRangeColumn.Index] = si.Affix[i].Mod1.ValueMin + "-" + si.Affix[i].Mod1.ValueMax;
                    row[AffixPrimaryModValueColumn.Index] = si.Affix[i].Mod1.Value;
                    if (si.Affix[i].Mod2.Value == 0)
                    {
                        row[AffixSecondaryModNameColumn.Index] = "";
                        row[AffixSecondaryModRangeColumn.Index] = "";
                        row[AffixSecondaryModValueColumn.Index] = "";
                    }
                    else
                    {
                        seenSecondary = true;
                        row[AffixSecondaryModNameColumn.Index] = si.Affix[i].Mod2.Name;
                        row[AffixSecondaryModRangeColumn.Index] = si.Affix[i].Mod2.ValueMin + "-" + si.Affix[i].Mod2.ValueMax;
                        row[AffixSecondaryModValueColumn.Index] = si.Affix[i].Mod2.Value;
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
            FilterResultsGrid.Rows.Clear();
            _itemTypeId = GlobalMethods.GetScalarInt("SELECT ItemTypeId FROM ItemType WHERE ItemTypeName = '" + si.ItemTypeName + "';");
            _itemSubTypeId = GlobalMethods.GetScalarInt("SELECT ItemSubTypeId FROM ItemSubType WHERE ItemSubTypeName = '" + si.ItemSubTypeName + "';");

            //Make a list of matching filters
            _filters.Clear();
            _filters = GlobalMethods.StuffIntList("SELECT FilterId FROM FilterHeader WHERE (ItemTypeId = 0 OR ItemTypeId = " + _itemTypeId + ") AND (ItemSubTypeId = 0 OR ItemSubTypeId = " + _itemSubTypeId + ");");

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

        public int ScoreFilter(int filterId, bool showDetail = false, int filterAffixSlot = 0)
        {
            //This is where the fun begins, we pull out the various mods we were looking for and mark the item against them
            //We return a score but we also load the specified filter and the results into a static class in case we need to do more work with them
            int runningTotalActual = 0;
            int filterCount = 0;
            //GlobalMethods.BISResults.Clear();
            //GlobalMethods.ILevelResults.Clear();
            //GlobalMethods.CLevelResults.Clear();
            //GlobalMethods.ItemResults.Clear();

            //Run through each filter slot
            for (int affixSlot = (filterAffixSlot == 0 ? 1 : filterAffixSlot); affixSlot <= (filterAffixSlot == 0 ? 6 : filterAffixSlot); affixSlot++)
            {
                int runningTotalILevel = 0;
                int runningTotalCLevel = 0;
                int runningTotalSlot = 0;
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

                //See if any mods on the item match the filter, this requires the affixes being parsed correctly as we need to know the ranges that could have rolled
                //We could have multiple hits if they just specified a mod class
                filterCount++;
                int affixesHit = 0;
                for (int mod = 0; mod < 10; mod++)
                {
                    //First check the mod class
                    if (si.Mod[mod].Class != modClass)
                        continue;

                    //Then check the modId if it exists
                    if (modId != 0 && si.Mod[mod].Id != modId)
                        continue;

                    //Before we continue we need to know which affix this is
                    int hitModId = si.Mod[mod].Id;
                    for (int a = 1; a <= 6; a++)
                    {
                        if (si.Affix[a].Mod1.Id == hitModId || si.Affix[a].Mod2.Id == hitModId)
                        {
                            affixesHit++;
                            var affix = si.Affix[a];

                            //Work out the slot position
                            var slot = si.Affix[a].Mod1.Id == hitModId ? "Primary" : "Secondary";

                            //We parse out the rolls for the various levels
                            int actualRoll = si.Mod[mod].Value;
                            int maxILevel = slot == "Primary" ? si.Affix[a].Mod1.ValueMax : si.Affix[a].Mod2.ValueMax;
                            var best = GlobalMethods.AffixCache.Aggregate((agg, next) => next.Mod1.ValueMax > agg.Mod1.ValueMax && next.ModCategoryId == affix.ModCategoryId && next.Mod1.Id == affix.Mod1.Id && next.Mod2.Id == affix.Mod2.Id ? next : agg);
                            int maxSlot = slot == "Primary" ? best.Mod1.ValueMax : best.Mod2.ValueMax;
                            best = GlobalMethods.AffixCache.Aggregate((agg, next) => next.Mod1.ValueMax > agg.Mod1.ValueMax && next.ModCategoryId == affix.ModCategoryId && next.Mod1.Id == affix.Mod1.Id && next.Mod2.Id == affix.Mod2.Id && next.Level <= CharacterLevel.Value ? next : agg);
                            int maxCLevel = slot == "Primary" ? best.Mod1.ValueMax : best.Mod2.ValueMax;

                            //Now we can rate the mod
                            int scoreILevel = maxILevel == 0 ? 0 : actualRoll * 100 / maxILevel;
                            int scoreCLevel = maxCLevel == 0 ? 0 : actualRoll * 100 / maxCLevel;
                            int scoreSlot = maxSlot == 0 ? 0 : actualRoll * 100 / maxSlot;

                            //Tally the scores
                            runningTotalILevel += scoreILevel;
                            runningTotalCLevel += scoreCLevel;
                            runningTotalSlot += scoreSlot;
                        }
                    }
                }

                //Calculate the average score
                if (affixesHit > 0)
                {
                    runningTotalActual += (Properties.Settings.Default.RatingMode == 0 ? runningTotalSlot : runningTotalILevel) / affixesHit;

                    //Dump the results out to the form if this option is on
                    if (showDetail)
                    {
                        tabControl1.Controls.Find(affixName + "ILevel", true)[0].Text = (runningTotalILevel == 0 ? 0 : runningTotalILevel / affixesHit) + "%";
                        tabControl1.Controls.Find(affixName + "CLevel", true)[0].Text = (runningTotalCLevel == 0 ? 0 : runningTotalCLevel / affixesHit) + "%";
                        tabControl1.Controls.Find(affixName + "Slot", true)[0].Text = (runningTotalSlot == 0 ? 0 : runningTotalSlot / affixesHit) + "%";
                        Image smiley;
                        int score = Properties.Settings.Default.RatingMode == 0 ? (runningTotalSlot == 0 ? 0 : runningTotalSlot / affixesHit) : (runningTotalILevel == 0 ? 0 : runningTotalILevel / affixesHit);
                        if (score / affixesHit <= Properties.Settings.Default.TolerancePoorTo)
                            smiley = Resources.PoorSmall;
                        else if (score / affixesHit <= Properties.Settings.Default.ToleranceAverageTo)
                            smiley = Resources.AverageSmall;
                        else
                            smiley = Resources.GoodSmall;
                        ((PictureBox)Controls.Find(affixName + "Smiley", true)[0]).Image = smiley;
                    }
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
                    Controls.Find(affixName + "ModClass", true)[0].Text = "(Any)";
                if (modId != 0)
                    Controls.Find(affixName + "Mod", true)[0].Text = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + modId + ";");
                else if (modClass == "")
                    Controls.Find(affixName + "Mod", true)[0].Text = "";
                else
                    Controls.Find(affixName + "Mod", true)[0].Text = "(Any)";
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
            int mod1Id = GlobalMethods.FindMod(AffixGrid.CurrentRow.Cells[AffixPrimaryModNameColumn.Index].Value.ToString(), 1, si.ItemTypeName, si.ItemSubTypeName).Id;
            int mod2Id = GlobalMethods.FindMod(AffixGrid.CurrentRow.Cells[AffixSecondaryModNameColumn.Index].Value.ToString(), 1, si.ItemTypeName, si.ItemSubTypeName).Id; 
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
            //ScoreFilter(_filterId, false, 1);
            //new RollDetails { AffixTypeString = "Prefix", ModClassName = Prefix1ModClass.Text, ModNameString = Prefix1Mod.Text }.ShowDialog();
        }

        private void Prefix2Details_Click(object sender, EventArgs e)
        {
            //ScoreFilter(_filterId, false, 2);
            //new RollDetails { AffixTypeString = "Prefix", ModClassName = Prefix2ModClass.Text, ModNameString = Prefix2Mod.Text }.ShowDialog();
        }

        private void Prefix3Details_Click(object sender, EventArgs e)
        {
            //ScoreFilter(_filterId, false, 3);
            //new RollDetails { AffixTypeString = "Prefix", ModClassName = Prefix3ModClass.Text, ModNameString = Prefix3Mod.Text }.ShowDialog();
        }

        private void Suffix1Details_Click(object sender, EventArgs e)
        {
            //ScoreFilter(_filterId, false, 4);
            //new RollDetails { AffixTypeString = "Suffix", ModClassName = Suffix1ModClass.Text, ModNameString = Suffix1Mod.Text }.ShowDialog();
        }

        private void Suffix2Details_Click(object sender, EventArgs e)
        {
            //ScoreFilter(_filterId, false, 5);
            //new RollDetails { AffixTypeString = "Suffix", ModClassName = Suffix2ModClass.Text, ModNameString = Suffix2Mod.Text }.ShowDialog();
        }

        private void Suffix3Details_Click(object sender, EventArgs e)
        {
            //ScoreFilter(_filterId, false, 6);
            //new RollDetails { AffixTypeString = "Suffix", ModClassName = Suffix3ModClass.Text, ModNameString = Suffix3Mod.Text }.ShowDialog();
        }

        private void Timer1Tick(object sender, EventArgs e)
        {
            Hide();
        }

        private void LeagueSelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalMethods.LeagueId = GlobalMethods.GetScalarInt("SELECT LeagueId FROM League WHERE LeagueName = '" + League.Text.Replace("'", "''") + "';");
            Properties.Settings.Default.DefaultLeagueId = GlobalMethods.LeagueId;
            Properties.Settings.Default.Save();
        }

        private void ItemInformation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Hide();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
