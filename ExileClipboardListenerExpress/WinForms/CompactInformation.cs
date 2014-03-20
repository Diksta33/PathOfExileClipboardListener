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
        private const int BarWidth = 300;

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
                label.Text = si.Affix[i].ModCategoryName + Environment.NewLine + si.Affix[i].Mod1.Name.Replace("Local ", "").Replace("Base ", "").Replace(" Rating", "").Replace("Minimum", "").Replace(" Per ", "/ ");
                label.Text += " [" + si.Affix[i].Mod1.Value + (si.Affix[i].Mod2.Id == 0 ? "" : "," + si.Affix[i].Mod2.Value) + "]";
                int mod1 = si.Affix[i].Mod1.Id;
                int mod2 = si.Affix[i].Mod2.Id;

                //Make a list of affix levels
                //ReSharper disable RedundantAssignment
                List<GlobalMethods.Affix> localCache = null;
                //ReSharper restore RedundantAssignment
                int modCategory = si.Affix[i].ModCategoryId;
                string affixType = si.Affix[i].AffixType;

                //Critical Strike messes this all up as there are different ranges for weapons to jewellery/ quivers
                //This hack won't work 100% but will at least stop this from glitching
                if (mod1 == 73 && mod2 == 0)
                {
                    if (si.ItemTypeName == "Weapons")
                        localCache = GlobalMethods.AffixCache.Where(a => a.AffixType == "Suffix" && (a.AffixId == 115 || a.AffixId == 116 || a.AffixId == 118 || a.AffixId == 120 || a.AffixId == 122 || a.AffixId == 124)).ToList();
                    else
                        localCache = GlobalMethods.AffixCache.Where(a => a.AffixType == "Suffix" && (a.AffixId == 114 || a.AffixId == 117 || a.AffixId == 119 || a.AffixId == 121 || a.AffixId == 123 || a.AffixId == 125)).ToList();
                }
                else
                    localCache = GlobalMethods.AffixCache.Where(a => a.AffixType == affixType && a.ModCategoryId == modCategory && a.Mod1.Id == mod1 && a.Mod2.Id == mod2).ToList();

                //We need to know the highest and lowest values possible
                int minMod1Value = localCache.Aggregate((c1, c2) => c1.Mod1.ValueMin < c2.Mod1.ValueMin ? c1 : c2).Mod1.ValueMin;
                int maxMod1Value = localCache.Aggregate((c1, c2) => c1.Mod1.ValueMax > c2.Mod1.ValueMax ? c1 : c2).Mod1.ValueMax;
                int minMod2Value = localCache.Aggregate((c1, c2) => c1.Mod2.ValueMin < c2.Mod2.ValueMin ? c1 : c2).Mod2.ValueMin;
                int maxMod2Value = localCache.Aggregate((c1, c2) => c1.Mod2.ValueMax > c2.Mod2.ValueMax ? c1 : c2).Mod2.ValueMax;
                int maxMod1ItemValue = localCache.Aggregate((c1, c2) => c1.Level <= si.ItemLevel && c1.Mod1.ValueMax > c2.Mod1.ValueMax ? c1 : c2).Mod1.ValueMax;
                int maxMod2ItemValue = localCache.Aggregate((c1, c2) => c1.Level <= si.ItemLevel && c1.Mod2.ValueMax > c2.Mod2.ValueMax ? c1 : c2).Mod2.ValueMax;

                //Next we create thresholds
                //Some items have no threshold so we drop the bar and score them 100%
                if (minMod1Value == maxMod1Value)
                {
                    progressBar.Visible = false;
                    label.Text = "[100%] " + label.Text;
                }

                //Some items have two ranges, so we score the item based on the roll and give it 50% or 100%
                if (minMod1Value == 1 && maxMod1Value == 2)
                {
                    progressBar.Visible = false;
                    label.Text = (si.Affix[i].Mod1.Value == 1 ? "[50%] " : "[100%] ") + label.Text;
                }
                else
                {
                    var thresholds = new List<int[]>();
                    foreach (var affix in localCache)
                    {
                        //We need a list of thresholds
                        int rangeMod1Low = affix.Mod1.ValueMin;
                        int rangeMod1High = affix.Mod1.ValueMax;
                        int rangeMod2Low = affix.Mod2.ValueMin;
                        int rangeMod2High = affix.Mod2.ValueMax;
                        int rangeMod1ScoreLow = BarWidth * (rangeMod1Low - minMod1Value) / (maxMod1Value == minMod1Value ? 1 : maxMod1Value - minMod1Value);
                        int rangeMod1ScoreHigh = BarWidth * (rangeMod1High - minMod1Value) / (maxMod1Value == minMod1Value ? 1 : maxMod1Value - minMod1Value);
                        int rangeMod2ScoreLow = mod2 == 0 ? 0 : BarWidth * (rangeMod2Low - minMod2Value) / (maxMod2Value == minMod2Value ? 1 : maxMod2Value - minMod2Value);
                        int rangeMod2ScoreHigh = mod2 == 0 ? 0 : BarWidth * (rangeMod2High - minMod2Value) / (maxMod2Value == minMod2Value ? 1 : maxMod2Value - minMod2Value);
                        int rangeScoreLow = (rangeMod1ScoreLow + rangeMod2ScoreLow) / (mod2 == 0 ? 1 : 2);
                        int rangeScoreHigh = (rangeMod1ScoreHigh + rangeMod2ScoreHigh) / (mod2 == 0 ? 1 : 2);

                        //If we end up with a threshold with a thickness of 1 then inflate it
                        if (rangeScoreLow == rangeScoreHigh)
                        {
                            rangeScoreLow = rangeScoreLow > 1 ? rangeScoreLow - 1 : 1;
                            rangeScoreHigh = rangeScoreHigh < BarWidth ? rangeScoreHigh + 1 : BarWidth;
                        }
                        thresholds.Add(new[] { rangeScoreLow, rangeScoreHigh });
                    }

                    //It helps to have the score up front so we can make sure it hits a band
                    double mod1Score = ((double)si.Affix[i].Mod1.Value - minMod1Value) / (maxMod1Value == minMod1Value ? 1 : maxMod1Value - minMod1Value);
                    double mod2Score = mod2 == 0 ? 0 : ((double)si.Affix[i].Mod2.Value - minMod2Value) / (maxMod2Value == minMod2Value ? 1 : maxMod2Value - minMod2Value);
                    var score = (int)(100 * (mod1Score + mod2Score) / (mod2 == 0 ? 1 : 2));
                    var scoreActual = (int)(BarWidth * (mod1Score + mod2Score) / (mod2 == 0 ? 1 : 2));

                    //Now we have all the thresholds we need to go back and fix any overlaps
                    //We need to have a gap between each sequence
                    int bandHit = 0;
                    for (int t = 1; t < thresholds.Count; t++)
                    {
                        //See if this band has been hit
                        if (scoreActual >= thresholds[t][0] && scoreActual <= thresholds[t][1])
                            bandHit = t;

                        //Check the start of the band doesn't overlap with the end of the previous band
                        //We want a space of 1 pixel between the two bands
                        while (thresholds[t][0] - thresholds[t - 1][1] < 2)
                        {
                            thresholds[t - 1][1] -= 1;
                            if (thresholds[t][0] - thresholds[t - 1][1] < 2)
                                thresholds[t][0] += 1;
                        }
                        //int overlap = thresholds[t][0] - thresholds[t - 1][1];
                        //if (overlap >= -2)
                        //{
                        //    if (overlap < 4)
                        //        overlap = 4;
                        //    else
                        //        overlap += 4;
                        //    thresholds[t - 1][1] -= overlap/2;
                        //    thresholds[t][0] += (overlap + 1)/2;
                        //}

                        //Check the previous band still has a width of at least one
                        if (thresholds[t - 1][1] <= thresholds[t - 1][0])
                        {
                            thresholds[t - 1][0] = thresholds[t - 1][1]== 0 ? 0 : thresholds[t - 1][1] - 1;
                        }
                    }

                    //Make sure the band we hit still is hit
                    if (scoreActual < thresholds[bandHit][0])
                        scoreActual = thresholds[bandHit][0];
                    if (scoreActual > thresholds[bandHit][1])
                        scoreActual = thresholds[bandHit][1];
                    
                    //Finally we assign them to the control
                    foreach (var t in thresholds)
                    {
                        progressBar.Thresholds.Add(new NewProgressBar.Tuple { RangeLow = t[0], RangeHigh = t[1] });
                    }

                    //We determine some basic statistics to show next to the bands
                    progressBar.Value = scoreActual;
                    double mod1ItemScore = ((double)si.Affix[i].Mod1.Value - minMod1Value) / (maxMod1ItemValue == minMod1Value ? 1 : maxMod1ItemValue - minMod1Value);
                    double mod2ItemScore = mod2 == 0 ? 0 : ((double)si.Affix[i].Mod2.Value - minMod2Value) / (maxMod2ItemValue == minMod2Value ? 1 : maxMod2ItemValue - minMod2Value);
                    var scoreItem = (int)(100 * (mod1ItemScore + mod2ItemScore) / (mod2 == 0 ? 1 : 2));
                    label.Text = "[" + score + "%] " + (score == scoreItem ? "" : "{" + scoreItem + "%} ") + label.Text;
                }
            }

            //Shuffle up the bars to take up as little space as possible
            int shuffled = 0;
            for (int i = 1; i <= 6; i++)
            {
                var label = (Label)Controls.Find("Label" + i, true).FirstOrDefault();
                var progressBar = (NewProgressBar)Controls.Find("newProgressBar" + i, true).FirstOrDefault();
                if (progressBar.Visible == false)
                {
                    int shift = label.Visible == false ? 59 : 20;
                    for (int j = i + 1; j <= 6; j++)
                    {
                        //ReSharper disable UseObjectOrCollectionInitializer
                        var newLocation = new Point();
                        //ReSharper restore UseObjectOrCollectionInitializer
                        newLocation.X = Controls.Find("Label" + j, true).FirstOrDefault().Location.X;
                        newLocation.Y = Controls.Find("Label" + j, true).FirstOrDefault().Location.Y - shift;
                        Controls.Find("Label" + j, true).FirstOrDefault().Location = newLocation;
                        newLocation.X = Controls.Find("newProgressBar" + j, true).FirstOrDefault().Location.X;
                        newLocation.Y = Controls.Find("newProgressBar" + j, true).FirstOrDefault().Location.Y - shift;
                        Controls.Find("newProgressBar" + j, true).FirstOrDefault().Location = newLocation;
                    }
                    shuffled+=shift;
                }
            }

            //If we shuffled we also need to shuffle the buttons and shrink the form
            if (shuffled > 0)
            {
                var newLocation = new Point {X = Dismiss.Location.X, Y = Dismiss.Location.Y - shuffled};
                Dismiss.Location = newLocation;
                newLocation.X = AddStash.Location.X;
                newLocation.Y = AddStash.Location.Y - shuffled;
                AddStash.Location = newLocation;
                Height = Height - shuffled;
            }
        }

        private void CompactInformation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space || e.KeyChar == (char)Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }

        private void Dismiss_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
