using System;
using System.Drawing;
using System.Windows.Forms;
using ExileClipboardListener.Classes;
using ExileClipboardListener.Properties;

namespace ExileClipboardListener.WinForms
{
    public partial class ItemValue : Form
    {
        public ItemValue()
        {
            InitializeComponent();
        }

        private void ItemValue_Load(object sender, EventArgs e)
        {
            //Tally up an average score
            int averageModScore = 0;
            int averageILevelScore = 0;
            int averageCLevelScore = 0;
            int averageSlotScore = 0;
            int keyCount = 0;
            int index = 0;

            //At some point we will allow character profiles, but for now we hard-code this as being 50
            CharLevel.Text = "50";
            int charLevel = Convert.ToInt32(CharLevel.Text);

            //We can get the item level out though
            int itemLevel = GlobalMethods.StashItem.ItemLevel;
            ItemLevel.Text = itemLevel.ToString();

            //Load the item type/ subtype and name
            ItemName.Text = GlobalMethods.StashItem.ItemName;
            ItemType.Text = GlobalMethods.StashItem.ItemTypeName;
            ItemSubType.Text = GlobalMethods.StashItem.ItemSubTypeName;

            //Rate each prefix and suffix
            for (int key = 0; key < 7; key++)
            {
                //We are adding rows to a grid for each key
                //Determine the mods and value
                int mod1Id = 0;
                int mod2Id = 0;
                int itemMod1Value = 0;
                int itemMod2Value = 0;
                string modType = "";
                if (key == 0)
                {
                    index = 1;
                    modType = "Implicit";
                    mod1Id = GlobalMethods.StashItem.ImplicitMod1Id;
                    mod2Id = GlobalMethods.StashItem.ImplicitMod2Id;
                    itemMod1Value = GlobalMethods.StashItem.ImplicitMod1Value;
                    itemMod2Value = GlobalMethods.StashItem.ImplicitMod2Value;
                }
                if (key == 1)
                {
                    index = 1;
                    modType = "Prefix";
                    mod1Id = GlobalMethods.StashItem.Prefix1Mod1Id;
                    mod2Id = GlobalMethods.StashItem.Prefix1Mod2Id;
                    itemMod1Value = GlobalMethods.StashItem.Prefix1Mod1Value;
                    itemMod2Value = GlobalMethods.StashItem.Prefix1Mod2Value;
                }
                if (key == 2)
                {
                    index = 2;
                    modType = "Prefix";
                    mod1Id = GlobalMethods.StashItem.Prefix2Mod1Id;
                    mod2Id = GlobalMethods.StashItem.Prefix2Mod2Id;
                    itemMod1Value = GlobalMethods.StashItem.Prefix2Mod1Value;
                    itemMod2Value = GlobalMethods.StashItem.Prefix2Mod2Value;
                }
                if (key == 3)
                {
                    index = 3;
                    modType = "Prefix";
                    mod1Id = GlobalMethods.StashItem.Prefix3Mod1Id;
                    mod2Id = GlobalMethods.StashItem.Prefix3Mod2Id;
                    itemMod1Value = GlobalMethods.StashItem.Prefix3Mod1Value;
                    itemMod2Value = GlobalMethods.StashItem.Prefix3Mod2Value;
                }
                if (key == 4)
                {
                    index = 1;
                    modType = "Suffix";
                    mod1Id = GlobalMethods.StashItem.Suffix1Mod1Id;
                    mod2Id = GlobalMethods.StashItem.Suffix1Mod2Id;
                    itemMod1Value = GlobalMethods.StashItem.Suffix1Mod1Value;
                    itemMod2Value = GlobalMethods.StashItem.Suffix1Mod2Value;
                }
                if (key == 5)
                {
                    index = 2;
                    modType = "Suffix";
                    mod1Id = GlobalMethods.StashItem.Suffix2Mod1Id;
                    mod2Id = GlobalMethods.StashItem.Suffix2Mod2Id;
                    itemMod1Value = GlobalMethods.StashItem.Suffix2Mod1Value;
                    itemMod2Value = GlobalMethods.StashItem.Suffix2Mod2Value;
                }
                if (key == 6)
                {
                    index = 3;
                    modType = "Suffix";
                    mod1Id = GlobalMethods.StashItem.Suffix3Mod1Id;
                    mod2Id = GlobalMethods.StashItem.Suffix3Mod2Id;
                    itemMod1Value = GlobalMethods.StashItem.Suffix3Mod1Value;
                    itemMod2Value = GlobalMethods.StashItem.Suffix3Mod2Value;
                }

                //If there isn't one then skip this
                if (mod1Id == 0 && mod2Id == 0)
                    continue;

                //Set the mod name
                Controls.Find(modType + index + "Mod1Name", true)[0].Text = GlobalMethods.GetScalarString("SELECT ModName FROM [Mod] WHERE ModId = " + mod1Id + ";");
                Controls.Find(modType + index + "Mod2Name", true)[0].Text = GlobalMethods.GetScalarString("SELECT ModName FROM [Mod] WHERE ModId = " + mod2Id + ";");
                
                //Now we need to lookup each mod to get the maximum and minimum values
                int mod1ValueMin = 0;
                int mod1ValueMax = 0;
                int mod2ValueMin = 0;
                int mod2ValueMax = 0;
                int result;

                //First check the item
                //Implicit Mods have a set range that doesn't alter by level
                if (key == 0)
                {
                    mod1ValueMin = GlobalMethods.GetScalarInt("SELECT Mod1ValueMin FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
                    mod1ValueMax = GlobalMethods.GetScalarInt("SELECT Mod1ValueMax FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
                    mod2ValueMin = GlobalMethods.GetScalarInt("SELECT Mod2ValueMin FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
                    mod2ValueMax = GlobalMethods.GetScalarInt("SELECT Mod2ValueMax FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
                }

                //If we have a prefix or a suffix we will get a Mod Category
                int modCategoryId = 0;

                //Prefix Mods are trickier as we have to guess the level of the mod first
                //This should really be moved to the Listener class as a lot of this is duplicate work
                //We should really be stashing the actual prefixes/ suffixes but instead we stash the first from a group that matches
                //So if we had 3 prefixes called "Added Widget" with ids of #1 = 10-20, #2 = 21-30, #3 = 31-40 and out value was 22 we would stash #1
                int prefixId = 0;
                if (key == 1 || key == 2 || key == 3)
                {
                    //This needs more work, it will spot false-positives where a prefix with two mods happens to collide with the item
                    if (key == 1)
                        prefixId = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT PrefixId FROM Prefix WHERE Mod1Id = " + mod1Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Prefix1Mod1Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Prefix1Mod1Value + ";") : GlobalMethods.GetScalarInt("SELECT PrefixId FROM Prefix WHERE Mod1Id = " + mod1Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Prefix1Mod1Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Prefix1Mod1Value + "AND Mod2Id = " + mod2Id + " AND Mod2ValueMin <= " + GlobalMethods.StashItem.Prefix1Mod2Value + " AND Mod2ValueMax >= " + GlobalMethods.StashItem.Prefix1Mod2Value + ";");
                    if (key == 2)
                        prefixId = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT PrefixId FROM Prefix WHERE Mod1Id = " + mod1Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Prefix2Mod1Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Prefix2Mod1Value + ";") : GlobalMethods.GetScalarInt("SELECT PrefixId FROM Prefix WHERE Mod1Id = " + mod1Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Prefix2Mod1Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Prefix2Mod1Value + "AND Mod2Id = " + mod2Id + " AND Mod2ValueMin <= " + GlobalMethods.StashItem.Prefix2Mod2Value + " AND Mod2ValueMax >= " + GlobalMethods.StashItem.Prefix2Mod2Value + ";");
                    if (key == 3)
                        prefixId = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT PrefixId FROM Prefix WHERE Mod1Id = " + mod1Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Prefix3Mod1Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Prefix3Mod1Value + ";") : GlobalMethods.GetScalarInt("SELECT PrefixId FROM Prefix WHERE Mod1Id = " + mod1Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Prefix3Mod1Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Prefix3Mod1Value + "AND Mod2Id = " + mod2Id + " AND Mod2ValueMin <= " + GlobalMethods.StashItem.Prefix3Mod2Value + " AND Mod2ValueMax >= " + GlobalMethods.StashItem.Prefix3Mod2Value + ";");
                    modCategoryId = GlobalMethods.GetScalarInt("SELECT ModCategoryId FROM Prefix WHERE PrefixId = " + prefixId + ";");
                    Controls.Find(modType + index + "Category", true)[0].Text = GlobalMethods.GetScalarString("SELECT ModCategoryName FROM ModCategory WHERE ModCategoryId = " + modCategoryId + ";");
                    mod1ValueMin = GlobalMethods.GetScalarInt("SELECT Mod1ValueMin FROM Prefix WHERE PrefixId = " + prefixId + ";");
                    mod1ValueMax = GlobalMethods.GetScalarInt("SELECT Mod1ValueMax FROM Prefix WHERE PrefixId = " + prefixId + ";");
                    mod2ValueMin = GlobalMethods.GetScalarInt("SELECT Mod2ValueMin FROM Prefix WHERE PrefixId = " + prefixId + ";");
                    mod2ValueMax = GlobalMethods.GetScalarInt("SELECT Mod2ValueMax FROM Prefix WHERE PrefixId = " + prefixId + ";");
                }

                //Suffix Mods are trickier as we have to guess the level of the mod first
                int suffixId = 0;
                if (key == 4 || key == 5 || key == 6)
                {
                    if (key == 4)
                        suffixId = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT SuffixId FROM Suffix WHERE Mod1Id = " + mod1Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Suffix1Mod1Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Suffix1Mod1Value + ";") : GlobalMethods.GetScalarInt("SELECT SuffixId FROM Suffix WHERE Mod1Id = " + mod1Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Suffix1Mod1Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Suffix1Mod1Value + "AND Mod2Id = " + mod2Id + " AND Mod2ValueMin <= " + GlobalMethods.StashItem.Suffix1Mod2Value + " AND Mod2ValueMax >= " + GlobalMethods.StashItem.Suffix1Mod2Value + ";");
                    if (key == 5)
                        suffixId = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT SuffixId FROM Suffix WHERE Mod1Id = " + mod1Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Suffix2Mod1Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Suffix2Mod1Value + ";") : GlobalMethods.GetScalarInt("SELECT SuffixId FROM Suffix WHERE Mod1Id = " + mod1Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Suffix2Mod1Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Suffix2Mod1Value + "AND Mod2Id = " + mod2Id + " AND Mod2ValueMin <= " + GlobalMethods.StashItem.Suffix2Mod2Value + " AND Mod2ValueMax >= " + GlobalMethods.StashItem.Suffix2Mod2Value + ";");
                    if (key == 6)
                        suffixId = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT SuffixId FROM Suffix WHERE Mod1Id = " + mod1Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Suffix3Mod1Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Suffix3Mod1Value + ";") : GlobalMethods.GetScalarInt("SELECT SuffixId FROM Suffix WHERE Mod1Id = " + mod1Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Suffix3Mod1Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Suffix3Mod1Value + "AND Mod2Id = " + mod2Id + " AND Mod2ValueMin <= " + GlobalMethods.StashItem.Suffix3Mod2Value + " AND Mod2ValueMax >= " + GlobalMethods.StashItem.Suffix3Mod2Value + ";");
                    modCategoryId = GlobalMethods.GetScalarInt("SELECT ModCategoryId FROM Suffix WHERE SuffixId = " + suffixId + ";");
                    Controls.Find(modType + index + "Category", true)[0].Text = GlobalMethods.GetScalarString("SELECT ModCategoryName FROM ModCategory WHERE ModCategoryId = " + modCategoryId + ";");
                    mod1ValueMin = GlobalMethods.GetScalarInt("SELECT Mod1ValueMin FROM Suffix WHERE SuffixId = " + suffixId + ";");
                    mod1ValueMax = GlobalMethods.GetScalarInt("SELECT Mod1ValueMax FROM Suffix WHERE SuffixId = " + suffixId + ";");
                    mod2ValueMin = GlobalMethods.GetScalarInt("SELECT Mod2ValueMin FROM Suffix WHERE SuffixId = " + suffixId + ";");
                    mod2ValueMax = GlobalMethods.GetScalarInt("SELECT Mod2ValueMax FROM Suffix WHERE SuffixId = " + suffixId + ";");
                }

                //Load the values onto the form
                Controls.Find(modType + index + "Mod1Min", true)[0].Text = mod1ValueMin.ToString();
                Controls.Find(modType + index + "Mod1Max", true)[0].Text = mod1ValueMax.ToString();
                Controls.Find(modType + index + "Mod1Item", true)[0].Text = itemMod1Value.ToString();
                Controls.Find(modType + index + "Mod2Min", true)[0].Text = mod2ValueMin == 0 ? "" : mod2ValueMin.ToString();
                Controls.Find(modType + index + "Mod2Max", true)[0].Text = mod2ValueMax == 0 ? "" : mod2ValueMax.ToString();
                Controls.Find(modType + index + "Mod2Item", true)[0].Text = itemMod2Value == 0 ? "" : itemMod2Value.ToString();

                //Determine the performance of the mod value by mod
                if (itemMod1Value == 0)
                    result = 0;
                else if (mod1ValueMax <= itemMod1Value)
                    result = 100;
                else if (mod2Id == 0)
                    //result = ((itemMod1Value - mod1ValueMin) * 100) / (mod1ValueMax - mod1ValueMin);
                    result = (itemMod1Value * 100) / mod1ValueMax;
                else
                    //result = (((itemMod1Value - mod1ValueMin) * 100) / (mod1ValueMax - mod1ValueMin) + ((itemMod2Value - mod2ValueMin) * 100) / (mod2ValueMax - mod2ValueMin)) / 2;
                    result = ((itemMod1Value * 100) / mod1ValueMax + (itemMod2Value * 100) / mod2ValueMax) / 2;
                averageModScore += result;
                Controls.Find(modType + index + "Mod", true)[0].Text = result + "%";

                //Determine the maximum mod value by level
                 if (prefixId != 0)
                {
                    mod1ValueMin = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMin) FROM Prefix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMin) FROM Prefix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod1ValueMax = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM Prefix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM Prefix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod2ValueMin = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMin) FROM Prefix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMin) FROM Prefix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod2ValueMax = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM Prefix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM Prefix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                }
                if (suffixId != 0)
                {
                    mod1ValueMin = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMin) FROM Suffix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMin) FROM Suffix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod1ValueMax = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM Suffix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM Suffix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod2ValueMin = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMin) FROM Suffix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMin) FROM Suffix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod2ValueMax = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM Suffix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM Suffix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                }
                if (itemMod1Value == 0)
                    result = 0;
                else if (mod1ValueMax <= itemMod1Value)
                    result = 100;
                else if (mod2Id == 0)
                    //result = ((itemMod1Value - mod1ValueMin) * 100) / (mod1ValueMax - mod1ValueMin);
                    result = (itemMod1Value * 100) / mod1ValueMax;
                else
                    //result = (((itemMod1Value - mod1ValueMin) * 100) / (mod1ValueMax - mod1ValueMin) + ((itemMod2Value - mod2ValueMin) * 100) / (mod2ValueMax - mod2ValueMin)) / 2;
                    result = ((itemMod1Value * 100) / mod1ValueMax + (itemMod2Value * 100) / mod2ValueMax) / 2;
                averageILevelScore += result;
                Controls.Find(modType + index + "ILevel", true)[0].Text = result + "%";

                //Determine the maximum mod value by character level
                if (prefixId != 0)
                {
                    mod1ValueMin = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMin) FROM Prefix WHERE [Level] <= " + charLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMin) FROM Prefix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod1ValueMax = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM Prefix WHERE [Level] <= " + charLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM Prefix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod2ValueMin = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMin) FROM Prefix WHERE [Level] <= " + charLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMin) FROM Prefix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod2ValueMax = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM Prefix WHERE [Level] <= " + charLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM Prefix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                }
                if (suffixId != 0)
                {
                    mod1ValueMin = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMin) FROM Suffix WHERE [Level] <= " + charLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMin) FROM Suffix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod1ValueMax = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM Suffix WHERE [Level] <= " + charLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM Suffix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod2ValueMin = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMin) FROM Suffix WHERE [Level] <= " + charLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMin) FROM Suffix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod2ValueMax = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM Suffix WHERE [Level] <= " + charLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM Suffix WHERE [Level] <= " + itemLevel + " AND Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                }
                if (itemMod1Value == 0)
                    result = 0;
                else if (mod1ValueMax <= itemMod1Value)
                    result = 100;
                else if (mod2Id == 0)
                    //result = ((itemMod1Value - mod1ValueMin) * 100) / (mod1ValueMax - mod1ValueMin);
                    result = (itemMod1Value * 100) / mod1ValueMax;
                else
                    //result = (((itemMod1Value - mod1ValueMin) * 100) / (mod1ValueMax - mod1ValueMin) + ((itemMod2Value - mod2ValueMin) * 100) / (mod2ValueMax - mod2ValueMin)) / 2;
                    result = ((itemMod1Value * 100) / mod1ValueMax + (itemMod2Value * 100) / mod2ValueMax) / 2;
                averageCLevelScore += result;
                Controls.Find(modType + index + "CLevel", true)[0].Text = result + "%";

                ////Determine the maximum mod value
                if (prefixId != 0)
                {
                    mod1ValueMin = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMin) FROM Prefix WHERE Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMin) FROM Prefix WHERE Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod1ValueMax = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM Prefix WHERE Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM Prefix WHERE Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod2ValueMin = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMin) FROM Prefix WHERE Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMin) FROM Prefix WHERE Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod2ValueMax = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM Prefix WHERE Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM Prefix WHERE Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                }
                if (suffixId != 0)
                {
                    mod1ValueMin = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMin) FROM Suffix WHERE Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMin) FROM Suffix WHERE Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod1ValueMax = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM Suffix WHERE Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM Suffix WHERE Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod2ValueMin = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMin) FROM Suffix WHERE Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMin) FROM Suffix WHERE Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                    mod2ValueMax = mod2Id == 0 ? GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM Suffix WHERE Mod1Id = " + mod1Id + " AND Mod2Id IS NULL AND ModCategoryId = " + modCategoryId + ";") : GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM Suffix WHERE Mod1Id = " + mod1Id + " AND Mod2Id = " + mod2Id + " AND ModCategoryId = " + modCategoryId + ";");
                }
                if (itemMod1Value == 0)
                    result = 0;
                else if (mod1ValueMax <= itemMod1Value)
                    result = 100;
                else if (mod2Id == 0)
                    //result = ((itemMod1Value - mod1ValueMin) * 100) / (mod1ValueMax - mod1ValueMin);
                    result = (itemMod1Value * 100) / mod1ValueMax;
                else
                    //result = (((itemMod1Value - mod1ValueMin) * 100) / (mod1ValueMax - mod1ValueMin) + ((itemMod2Value - mod2ValueMin) * 100) / (mod2ValueMax - mod2ValueMin)) / 2;
                    result = ((itemMod1Value * 100) / mod1ValueMax + (itemMod2Value * 100) / mod2ValueMax) / 2;
                averageSlotScore += result;
                Controls.Find(modType + index + "Slot", true)[0].Text = result + "%";

                //Give a rating
                Image smiley;
                Color progressColour;
                if (result <= Properties.Settings.Default.TolerancePoorTo)
                {
                    progressColour = Color.DarkRed;
                    smiley = Resources.PoorSmall;
                }
                else if (result <= Properties.Settings.Default.ToleranceAverageTo)
                {
                    progressColour = Color.Orange;
                    smiley = Resources.AverageSmall;
                }
                else
                {
                    progressColour = Color.Green;
                    smiley = Resources.GoodSmall;
                }
                ((PictureBox)Controls.Find(modType + index + "Smiley", true)[0]).Image = smiley;
                ((NewProgressBar)Controls.Find(modType + index + "Rating", true)[0]).Value = result;
                
                //This is to prevent this code from being used but keeps Resharper happy
                if (progressColour == Color.Red)
                    Controls.Find(modType + index + "Rating", true)[0].ForeColor = progressColour;

                //Count this key
                keyCount++;
            }

            //Get the overall verdict
            if (keyCount != 0)
            {
                Image smiley;

                //Mod
                averageModScore /= keyCount;
                OverallMod.Text = averageModScore + "%";
                if (averageModScore <= Properties.Settings.Default.TolerancePoorTo)
                    smiley = Resources.PoorSmall;
                else if (averageModScore <= Properties.Settings.Default.ToleranceAverageTo)
                    smiley = Resources.AverageSmall;
                else
                    smiley = Resources.GoodSmall;
                ((PictureBox)Controls.Find("OverallModSmiley", true)[0]).Image = smiley;

                //ILevel
                averageILevelScore /= keyCount;
                OverallILevel.Text = averageILevelScore + "%";
                if (averageILevelScore <= Properties.Settings.Default.TolerancePoorTo)
                    smiley = Resources.PoorSmall;
                else if (averageILevelScore <= Properties.Settings.Default.ToleranceAverageTo)
                    smiley = Resources.AverageSmall;
                else
                    smiley = Resources.GoodSmall;
                ((PictureBox)Controls.Find("OverallILevelSmiley", true)[0]).Image = smiley;

                //CLevel
                averageCLevelScore /= keyCount;
                OverallCLevel.Text = averageCLevelScore + "%";
                if (averageCLevelScore <= Properties.Settings.Default.TolerancePoorTo)
                    smiley = Resources.PoorSmall;
                else if (averageCLevelScore <= Properties.Settings.Default.ToleranceAverageTo)
                    smiley = Resources.AverageSmall;
                else
                    smiley = Resources.GoodSmall;
                ((PictureBox)Controls.Find("OverallCLevelSmiley", true)[0]).Image = smiley;

                //Slot
                averageSlotScore /= keyCount;
                OverallSlot.Text = averageSlotScore + "%";
                if (averageSlotScore <= Properties.Settings.Default.TolerancePoorTo)
                    smiley = Resources.PoorSmall;
                else if (averageSlotScore <= Properties.Settings.Default.ToleranceAverageTo)
                    smiley = Resources.AverageSmall;
                else
                    smiley = Resources.GoodSmall;
                ((PictureBox)Controls.Find("OverallSlotSmiley", true)[0]).Image = smiley;
            }

            //We need to dynamically highlight the important statistics and hide the ones that are less important
            //var cell = StatisticsGrid.Rows[0].Cells[0];
            //cell.Style = new DataGridViewCellStyle()
            //{
            //    BackColor = Color.White,
            //    Font = new Font("Tahoma", 8F),
            //    ForeColor = SystemColors.WindowText,
            //    SelectionBackColor = Color.Red,
            //    SelectionForeColor = SystemColors.HighlightText
            //};

            //Start the timer
            timer1.Interval = Properties.Settings.Default.CollectionPopUpSeconds * 1000;
            timer1.Start();
        }

        //private void RedundantComparisonKeyRoutineThatCouldBeUseful()
        //{
        //    //Pull out the comparison keys so we can iterate through them
        //    var keyId = new int[6];
        //    keyId[0] = Properties.Settings.Default.KeyMod1Value;
        //    keyId[1] = Properties.Settings.Default.KeyMod2Value;
        //    keyId[2] = Properties.Settings.Default.KeyMod3Value;
        //    keyId[3] = Properties.Settings.Default.KeyMod4Value;
        //    keyId[4] = Properties.Settings.Default.KeyMod5Value;

        //    //Tally up an average score
        //    int averageScore = 0;

        //    //Rate each key
        //    for (int key = 0; key < 5; key++)
        //    {
        //        //We are adding rows to a grid for each key
        //        var row = new object[9];

        //        //Determine the mod
        //        var modId = keyId[key];
        //        row[0] = GlobalMethods.GetScalarString("SELECT ModName FROM [Mod] WHERE ModId = " + modId + ";");

        //        //We have to mess around a bit here, we are ideally looking for a mod that our item already has, there is a chance it might have it twice if it has it as an implicit and prefix/ suffix.
        //        //There is also the situation where there are multiple possible mods, e.g. accuracy can come as raw accuracy or associated with light radius.  On our mod list we might just look for accuracy + and not care
        //        //where it comes from, but we need to be very careful here.  For example, if we had an accuracy/ light radius mod on our item as the only source of accuracy
        //        //we would want to compare the accuracy roll to the maximum accuracy/ light radius mod but we would want to compare the items (low) accuracy roll to the best possible accuracy roll for the level/ slot.
        //        int itemModValue = 0;
        //        int minModValue = 0;
        //        int maxModValue = 0;
        //        int result;

        //        //First check the item
        //        //Implicit Mods have a set range that doesn't alter by level
        //        if (GlobalMethods.StashItem.ImplicitMod1Id == modId)
        //        {
        //            itemModValue = GlobalMethods.StashItem.ImplicitMod1ValueMax;
        //            minModValue = GlobalMethods.GetScalarInt("SELECT Mod1ValueMin FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
        //            maxModValue = GlobalMethods.GetScalarInt("SELECT Mod1ValueMax FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
        //        }
        //        if (GlobalMethods.StashItem.ImplicitMod2Id == modId)
        //        {
        //            itemModValue = GlobalMethods.StashItem.ImplicitMod2ValueMax;
        //            minModValue = GlobalMethods.GetScalarInt("SELECT Mod2ValueMin FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
        //            maxModValue = GlobalMethods.GetScalarInt("SELECT Mod2ValueMax FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
        //        }

        //        //Determine the performance of the mod value by mod
        //        if (itemModValue == 0)
        //            row[4] = "0%";
        //        else if (maxModValue <= itemModValue)
        //            row[4] = "100%";
        //        else
        //            row[4] = (itemModValue * 100 / maxModValue) + "%";

        //        //Determine the maximum mod value by level
        //        //As yet we have only got implicit mods so this is just the same
        //        if (itemModValue == 0)
        //        {
        //            row[5] = "0%";
        //            result = 0;
        //        }
        //        else if (maxModValue <= itemModValue)
        //        {
        //            row[5] = "100%";
        //            result = 100;
        //        }
        //        else
        //        {
        //            row[5] = (itemModValue * 100 / maxModValue) + "%";
        //            result = itemModValue * 100 / maxModValue;
        //        }

        //        //Determine the maximum mod value
        //        //Pull out the itemlevel
        //        int itemLevel = GlobalMethods.StashItem.ItemLevel;
        //        ItemLevel.Text = itemLevel.ToString();

        //        //As yet we have only got implicit mods so this is just the same
        //        if (itemModValue == 0)
        //        {
        //            row[6] = "0%";
        //            if (Properties.Settings.Default.RatingMode == 0)
        //                result = 0;
        //        }
        //        else if (maxModValue <= itemModValue)
        //        {
        //            row[6] = "100%";
        //            if (Properties.Settings.Default.RatingMode == 0)
        //                result = 100;
        //        }
        //        else
        //        {
        //            row[6] = (itemModValue * 100 / maxModValue) + "%";
        //            if (Properties.Settings.Default.RatingMode == 0)
        //                result = itemModValue * 100 / maxModValue;
        //        }

        //        //Now show the min/ max rolls and the item roll
        //        row[1] = minModValue;
        //        row[2] = maxModValue;
        //        row[3] = itemModValue;

        //        //Give a rating
        //        if (result <= Properties.Settings.Default.TolerancePoorTo)
        //            row[7] = Resources.PoorSmall;
        //        else if (result <= Properties.Settings.Default.ToleranceAverageTo)
        //            row[7] = Resources.AverageSmall;
        //        else
        //            row[7] = Resources.GoodSmall;

        //        //Copy the results out to the form
        //        Controls.Find("Mod" + (key + 1) + "Name", true)[0].Text = row[0].ToString();
        //        Controls.Find("Mod" + (key + 1) + "Min", true)[0].Text = row[1].ToString();
        //        Controls.Find("Mod" + (key + 1) + "Max", true)[0].Text = row[2].ToString();
        //        Controls.Find("Mod" + (key + 1) + "Item", true)[0].Text = row[3].ToString();
        //        Controls.Find("Mod" + (key + 1) + "Mod", true)[0].Text = row[4].ToString();
        //        Controls.Find("Mod" + (key + 1) + "Level", true)[0].Text = row[5].ToString();
        //        Controls.Find("Mod" + (key + 1) + "Slot", true)[0].Text = row[6].ToString();
        //        ((ProgressBar)Controls.Find("Mod" + (key + 1) + "Rating", true)[0]).Value = maxModValue == 0 ? 0 : itemModValue * 100 / maxModValue;
        //        ((PictureBox)Controls.Find("Mod" + (key + 1) + "Smiley", true)[0]).Image = (Image)row[7];

        //        //Tally the average score
        //        averageScore += result;
        //    }

        //    //Get the overall verdict
        //    averageScore /= 5;
        //    if (averageScore <= Properties.Settings.Default.TolerancePoorTo)
        //        OverallResult.Image = Resources.Poor;
        //    else if (averageScore <= Properties.Settings.Default.ToleranceAverageTo)
        //        OverallResult.Image = Resources.Average;
        //    else
        //        OverallResult.Image = Resources.Good;
        //}

        private void Timer1Tick(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.CollectionPopUpMode == 1)
                Hide();
        }
    }
}
