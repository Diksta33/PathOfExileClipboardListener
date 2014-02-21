using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ExileClipboardListener.Classes;
using si = ExileClipboardListener.Classes.GlobalMethods.StashItem;

namespace ExileClipboardListener.WinForms
{
    public partial class CompactInformation : Form
    {
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
        }

        private void RefreshForm()
        {
            //First we put some useful information into the Stuff box
            Stuff.Text = "Useful information will appear here...";

            //Next we list the affixes and rating information
            for (int i = 1; i < 7; i++)
            {
                if (si.Affix[i].AffixId == 0)
                    continue;
                
                //Get the controls for this
                var label = (Label)Controls.Find("Label" + i, true).FirstOrDefault();
                label.Text = si.Affix[i].ModCategoryName;
                var progressBar = (NewProgressBar)Controls.Find("newProgressBar" + i, true).FirstOrDefault();
                int mod1 = si.Affix[i].Mod1.Id;
                int mod2 = si.Affix[i].Mod2.Id;
                var levels = new List<int>();
                GlobalMethods.StuffIntList("SELECT Level FROM " + si.Affix[i].AffixType + " WHERE Mod1Id = " + mod1 + (mod2 != 0 ? " AND Mod2Id = " + mod2 : "") + " ORDER BY Level;", levels);
                //We need to know the highest and lowest values possible
                int minLevel = levels.Find(l => l > 0);
                int minValue = GlobalMethods.GetScalarInt("SELECT Mod1ValueMin FROM " + si.Affix[i].AffixType + " WHERE Level = " + minLevel + " AND Mod1Id = " + mod1 + (mod2 != 0 ? " AND Mod2Id = " + mod2 : "") + ";");
                int maxLevel = levels.FindLast(l => l > 0);
                int maxValue = GlobalMethods.GetScalarInt("SELECT Mod1ValueMax FROM " + si.Affix[i].AffixType + " WHERE Level = " + maxLevel + " AND Mod1Id = " + mod1 + (mod2 != 0 ? " AND Mod2Id = " + mod2 : "") + ";");
                foreach (var level in levels)
                {
                    //We need a list of thresholds
                    int rangeLow = GlobalMethods.GetScalarInt("SELECT Mod1ValueMin FROM " + si.Affix[i].AffixType + " WHERE Level = " + level + " AND Mod1Id = " + mod1 + (mod2 != 0 ? " AND Mod2Id = " + mod2 : "") + ";");
                    int rangeHigh = GlobalMethods.GetScalarInt("SELECT Mod1ValueMax FROM " + si.Affix[i].AffixType + " WHERE Level = " + level + " AND Mod1Id = " + mod1 + (mod2 != 0 ? " AND Mod2Id = " + mod2 : "") + ";");
                    progressBar.Thresholds.Add(new NewProgressBar.Tuple { RangeLow = 100 * (rangeLow - minValue) / (maxValue - minValue), RangeHigh = 100 * (rangeHigh - minValue) / (maxValue - minValue) });
                }
                progressBar.Value = 100 * (si.Affix[i].Mod1.Value - minValue) / (maxValue - minValue);
            }
        }

        private void CompactInformation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space || e.KeyChar == (char)Keys.Escape)
                Hide();
        }
    }
}
