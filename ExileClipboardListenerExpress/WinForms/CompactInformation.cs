using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using ExileClipboardListener.Classes;
using si = ExileClipboardListener.Classes.GlobalMethods.StashItem;

namespace ExileClipboardListener.WinForms
{
    public partial class CompactInformation : Form
    {
        public bool AllowStash = true;

        public CompactInformation()
        {
            InitializeComponent();
        }

        private void CompactInformationLoad(object sender, EventArgs e)
        {
            timer1.Interval = Properties.Settings.Default.CollectionPopUpSeconds * 1000;
            if (Properties.Settings.Default.CollectionPopUpMode == 1)
                timer1.Start();
            RefreshForm();
            AddStash.Enabled = AllowStash;
        }

        private void RefreshForm()
        {
            //First we put some useful information into the Stuff box
            Stuff.Text = si.ItemName + " [" + GlobalMethods.BaseItem.ItemName + "]" + Environment.NewLine;
            if (si.ItemTypeName == "Weapon")
                Stuff.Text += "eDPS: " + si.eDPS + Environment.NewLine + "pDPS: " + si.pDPS + Environment.NewLine + "tDPS: " + si.tDPS + Environment.NewLine;
            if (si.ItemTypeName == "Armour")
                Stuff.Text += "Armour: " + si.Armour + Environment.NewLine + "Evasion: " + si.Evasion + Environment.NewLine + "EShield: " + si.EnergyShield + Environment.NewLine;
            Stuff.Text += "Item Level: " + si.ItemLevel + Environment.NewLine;

            //Next we list the affixes and rating information
            for (int i = 1; i <= 6; i++)
            {
                //Get the controls for this
                var label = (Label)Controls.Find("Label" + i, true).FirstOrDefault();
                var progressBar = (NewProgressBar)Controls.Find("newProgressBar" + i, true).FirstOrDefault();

                //If there is no affix then hide everything
                if (si.Affix[i].AffixId == 0)
                {
                    label.Visible = false;
                    progressBar.Visible = false;
                    continue;
                }

                //Output the details
                label.Text = si.Affix[i].ModCategoryName + Environment.NewLine + si.Affix[i].Mod1.Name.Replace("Local ", "").Replace("Base ", "").Replace(" Rating", "").Replace(" Per ", "/ ");
                label.Text += " [" + si.Affix[i].Mod1.Value + (si.Affix[i].Mod2.Id == 0 ? "" : "," + si.Affix[i].Mod2.Value) + "]";
                int mod1 = si.Affix[i].Mod1.Id;
                int mod2 = si.Affix[i].Mod2.Id;
                var levels = GlobalMethods.StuffIntList("SELECT Level FROM " + si.Affix[i].AffixType + " WHERE Mod1Id = " + mod1 + " AND IFNULL(Mod2Id, 0) = " + mod2 + " ORDER BY Level;");

                //We need to know the highest and lowest values possible
                int minLevel = levels.Find(l => l > 0);
                int maxLevel = levels.FindLast(l => l > 0);
                int minMod1Value = GlobalMethods.GetScalarInt("SELECT Mod1ValueMin FROM " + si.Affix[i].AffixType + " WHERE Level = " + minLevel + " AND Mod1Id = " + mod1 + " AND IFNULL(Mod2Id, 0) = " + mod2 + ";");
                int maxMod1Value = GlobalMethods.GetScalarInt("SELECT Mod1ValueMax FROM " + si.Affix[i].AffixType + " WHERE Level = " + maxLevel + " AND Mod1Id = " + mod1 + " AND IFNULL(Mod2Id, 0) = " + mod2 + ";");
                int minMod2Value = mod2 == 0 ? 0 : GlobalMethods.GetScalarInt("SELECT Mod2ValueMin FROM " + si.Affix[i].AffixType + " WHERE Level = " + minLevel + " AND Mod1Id = " + mod1 + " AND IFNULL(Mod2Id, 0) = " + mod2 + ";");
                int maxMod2Value = mod2 == 0 ? 0 : GlobalMethods.GetScalarInt("SELECT Mod2ValueMax FROM " + si.Affix[i].AffixType + " WHERE Level = " + maxLevel + " AND Mod1Id = " + mod1 + " AND IFNULL(Mod2Id, 0) = " + mod2 + ";");

                //Next we create thresholds
                foreach (var level in levels)
                {
                    //We need a list of thresholds
                    int rangeMod1Low = GlobalMethods.GetScalarInt("SELECT Mod1ValueMin FROM " + si.Affix[i].AffixType + " WHERE Level = " + level + " AND Mod1Id = " + mod1 + " AND IFNULL(Mod2Id, 0) = " + mod2 + ";");
                    int rangeMod1High = GlobalMethods.GetScalarInt("SELECT Mod1ValueMax FROM " + si.Affix[i].AffixType + " WHERE Level = " + level + " AND Mod1Id = " + mod1 + " AND IFNULL(Mod2Id, 0) = " + mod2 + ";");
                    if (rangeMod1High == rangeMod1Low)
                    {
                        rangeMod1Low = rangeMod1Low > 1 ? rangeMod1Low - 1 : 1;
                        rangeMod1High = rangeMod1High < maxMod1Value ? maxMod1Value : rangeMod1High + 1; ;
                    }
                    int rangeMod2Low = mod2 == 0 ? 0 : GlobalMethods.GetScalarInt("SELECT Mod1ValueMin FROM " + si.Affix[i].AffixType + " WHERE Level = " + level + " AND Mod1Id = " + mod1 + " AND IFNULL(Mod2Id, 0) = " + mod2 + ";");
                    int rangeMod2High = mod2 == 0 ? 0 : GlobalMethods.GetScalarInt("SELECT Mod1ValueMax FROM " + si.Affix[i].AffixType + " WHERE Level = " + level + " AND Mod1Id = " + mod1 + " AND IFNULL(Mod2Id, 0) = " + mod2 + ";");
                    if (rangeMod2High == rangeMod2Low && rangeMod2High != 0)
                    {
                        rangeMod2Low = rangeMod2Low > 1 ? rangeMod2Low - 1 : 1;
                        rangeMod2High = rangeMod2High < maxMod2Value ? maxMod2Value : rangeMod2High + 1; 
                    }
                    int rangeMod1ScoreLow = 100 * (rangeMod1Low - minMod1Value) / (maxMod1Value == minMod1Value ? 1 : maxMod1Value - minMod1Value);
                    int rangeMod1ScoreHigh = 100 * (rangeMod1High - minMod1Value) / (maxMod1Value == minMod1Value ? 1 : maxMod1Value - minMod1Value);
                    int rangeMod2ScoreLow = mod2 == 0 ? 0 : 100 * (rangeMod2Low - minMod2Value) / (maxMod2Value == minMod2Value ? 1 : maxMod2Value - minMod2Value);
                    int rangeMod2ScoreHigh = mod2 == 0 ? 0 : 100 * (rangeMod2High - minMod2Value) / (maxMod2Value == minMod2Value ? 1 : maxMod2Value - minMod2Value);
                    int rangeScoreLow = (rangeMod1ScoreLow + rangeMod2ScoreLow) / (mod2 == 0 ? 1 : 2);
                    int rangeScoreHigh = (rangeMod1ScoreHigh + rangeMod2ScoreHigh) / (mod2 == 0 ? 1 : 2);
                    progressBar.Thresholds.Add(new NewProgressBar.Tuple { RangeLow = rangeScoreLow, RangeHigh = rangeScoreHigh });
                }
                double mod1Score = ((double)si.Affix[i].Mod1.Value - minMod1Value) / (maxMod1Value == minMod1Value ? 1 : maxMod1Value - minMod1Value);
                double mod2Score = mod2 == 0 ? 0 : ((double)si.Affix[i].Mod2.Value - minMod2Value) / (maxMod2Value == minMod2Value ? 1 : maxMod2Value - minMod2Value);
                progressBar.Value = (int)(100 * (mod1Score + mod2Score) / (mod2 == 0 ? 1 : 2));
                label.Text = "[" + progressBar.Value + "%] " + label.Text;
            }

            //Shuffle up the bars to take up as little space as possible
            int shuffled = 0;
            for (int i = 1; i <= 6; i++)
            {
                var label = (Label)Controls.Find("Label" + i, true).FirstOrDefault();
                var progressBar = (NewProgressBar)Controls.Find("newProgressBar" + i, true).FirstOrDefault();
                if (label.Visible == false)
                {
                    for (int j = i + 1; j <= 6; j++)
                    {
                        var newLocation = new Point();
                        newLocation.X = ((Label)Controls.Find("Label" + j, true).FirstOrDefault()).Location.X;
                        newLocation.Y = ((Label)Controls.Find("Label" + j, true).FirstOrDefault()).Location.Y - 49;
                        ((Label)Controls.Find("Label" + j, true).FirstOrDefault()).Location = newLocation;
                        newLocation.X = ((NewProgressBar)Controls.Find("newProgressBar" + j, true).FirstOrDefault()).Location.X;
                        newLocation.Y = ((NewProgressBar)Controls.Find("newProgressBar" + j, true).FirstOrDefault()).Location.Y - 49;
                        ((NewProgressBar)Controls.Find("newProgressBar" + j, true).FirstOrDefault()).Location = newLocation;
                    }
                    shuffled++;
                }
            }

            //If we shuffled we also need to shuffle the buttons and shrink the form
            if (shuffled > 0)
            {
                var newLocation = new Point();
                newLocation.X = Dismiss.Location.X;
                newLocation.Y = Dismiss.Location.Y - 49 * shuffled;
                Dismiss.Location = newLocation;
                newLocation.X = AddStash.Location.X;
                newLocation.Y = AddStash.Location.Y - 49 * shuffled;
                AddStash.Location = newLocation;
                Height = Height - 49 * shuffled;
            }
        }

        private void CompactInformation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space || e.KeyChar == (char)Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }
    }
}
