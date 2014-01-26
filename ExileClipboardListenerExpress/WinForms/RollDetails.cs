using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExileClipboardListener.Classes;

namespace ExileClipboardListener.WinForms
{
    public partial class RollDetails : Form
    {
        public int FilterId;
        public string AffixTypeString;
        public string ModClassName;
        public string ModNameString;

        public RollDetails()
        {
            InitializeComponent();
        }

        private void RollDetails_Load(object sender, EventArgs e)
        {
            AffixType.Text = AffixTypeString;
            ModClass.Text = ModClassName;
            ModName.Text = ModNameString;
            RefreshScores();
        }

        private void RefreshScores()
        {
            int modId = ModNameString == "(Any)" ? 0 : GlobalMethods.GetScalarInt("SELECT ModId FROM Mod WHERE ModName = '" + ModNameString + "';");

            //First we need to know which mods the item could have 
            var mods = new List<GlobalMethods.Mod>();

            //If they specified a filter with a mod class AND a specific mod then great, we just use that mod
            if (modId != 0)
                mods.Add(new GlobalMethods.Mod { Id = modId });

            //Otherwise we have to work out which mods the item could have based on the mod class
            else
            {
                List<int> match = new List<int>();
                string sql = "SELECT ModId FROM Mod WHERE ModClass = '" + ModClassName + "'";

                //We filter by the current item type
                if (GlobalMethods.StashItem.ItemTypeName == "Weapons")
                    sql += " AND IFNULL(Weapons, 1) = 1";
                else if (GlobalMethods.StashItem.ItemTypeName == "Armour")
                    sql += " AND IFNULL(Armour, 1) = 1";
                else if (GlobalMethods.StashItem.ItemTypeName == "Jewellery")
                    sql += " AND IFNULL(Jewellery, 1) = 1";

                //For Defense we also need to make the base type of the body armour matches the mod, e.g. we wouldn't get evasion on an AR armour
                if (ModClassName == "Defense")
                {
                    if (GlobalMethods.StashItem.Armour == 0)
                        sql += " AND ModName NOT LIKE '%Armour%'";
                    if (GlobalMethods.StashItem.Evasion == 0)
                        sql += " AND ModName NOT LIKE '%Evasion%'";
                    if (GlobalMethods.StashItem.EnergyShield == 0)
                        sql += " AND ModName NOT LIKE '%Energy Shield%'";
                }
                sql += " AND IFNULL(ModPair, 2) = 2;";
                GlobalMethods.StuffIntList(sql, match);
                foreach (int id in match)
                    mods.Add(new GlobalMethods.Mod { Id = id });
            }

            //Now we have a list it's simply a matter of tallying up the scores for each level
            //First the BIS roll
            foreach (GlobalMethods.Mod m in mods)
            {
                var row = new object[6];

                //First get the best in slot scores
                int maxSlotPrimary = GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + ";");
                int maxSlotSecondary = GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM " + AffixTypeString + " WHERE Mod2Id = " + m.Id + ";");

                //Now backfill the best in slot data
                if (maxSlotPrimary != 0)
                {
                    row[0] = GlobalMethods.GetScalarString("SELECT Name FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxSlotPrimary + ";");
                    row[1] = "Primary";
                    row[2] = GlobalMethods.GetScalarString("SELECT Level FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxSlotPrimary + ";");
                    row[3] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                    row[4] = GlobalMethods.GetScalarString("SELECT Mod1ValueMin FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxSlotPrimary + ";");
                    row[5] = GlobalMethods.GetScalarString("SELECT Mod1ValueMax FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxSlotPrimary + ";");
                    BestInSlotGrid.Rows.Add(row);
                }
                if (maxSlotSecondary != 0)
                {
                    row[0] = GlobalMethods.GetScalarString("SELECT Name FROM " + AffixTypeString + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSlotSecondary + ";");
                    row[1] = "Secondary";
                    row[2] = GlobalMethods.GetScalarString("SELECT Level FROM " + AffixTypeString + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSlotSecondary + ";");
                    row[3] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                    row[4] = GlobalMethods.GetScalarString("SELECT Mod2ValueMin FROM " + AffixTypeString + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSlotSecondary + ";");
                    row[5] = GlobalMethods.GetScalarString("SELECT Mod2ValueMax FROM " + AffixTypeString + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSlotSecondary + ";");
                    BestInSlotGrid.Rows.Add(row);
                }

                //Next we do the same but use item level as well
                maxSlotPrimary = GlobalMethods.GetScalarInt("SELECT MAX(Mod1ValueMax) FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + " AND Level < " + GlobalMethods.StashItem.ItemLevel + ";");
                maxSlotSecondary = GlobalMethods.GetScalarInt("SELECT MAX(Mod2ValueMax) FROM " + AffixTypeString + " WHERE Mod2Id = " + m.Id + " AND Level < " + GlobalMethods.StashItem.ItemLevel + ";");

                //Now backfill the best in slot data
                if (maxSlotPrimary != 0)
                {
                    row[0] = GlobalMethods.GetScalarString("SELECT Name FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxSlotPrimary + ";");
                    row[1] = "Primary";
                    row[2] = GlobalMethods.GetScalarString("SELECT Level FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxSlotPrimary + ";");
                    row[3] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                    row[4] = GlobalMethods.GetScalarString("SELECT Mod1ValueMin FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxSlotPrimary + ";");
                    row[5] = GlobalMethods.GetScalarString("SELECT Mod1ValueMax FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMax = " + maxSlotPrimary + ";");
                    BestForItemGrid.Rows.Add(row);
                }
                if (maxSlotSecondary != 0)
                {
                    row[0] = GlobalMethods.GetScalarString("SELECT Name FROM " + AffixTypeString + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSlotSecondary + ";");
                    row[1] = "Secondary";
                    row[2] = GlobalMethods.GetScalarString("SELECT Level FROM " + AffixTypeString + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSlotSecondary + ";");
                    row[3] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                    row[4] = GlobalMethods.GetScalarString("SELECT Mod2ValueMin FROM " + AffixTypeString + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSlotSecondary + ";");
                    row[5] = GlobalMethods.GetScalarString("SELECT Mod2ValueMax FROM " + AffixTypeString + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMax = " + maxSlotSecondary + ";");
                    BestForItemGrid.Rows.Add(row);
                }

                //Finally we need to do this all one last time for the item rolls
                for (int mod = 0; mod < 10; mod++)
                {
                    if (GlobalMethods.StashItem.Mod[mod].Id == m.Id)
                    {
                        string position = GlobalMethods.GetScalarString("SELECT Name FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + ";") == "" ? "Secondary" : "Primary";
                        var yourRow = new object[7];
                        if (position == "Primary")
                        {
                            yourRow[0] = GlobalMethods.GetScalarString("SELECT Name FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                            yourRow[1] = "Primary";
                            yourRow[2] = GlobalMethods.GetScalarString("SELECT Level FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                            yourRow[3] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                            yourRow[4] = GlobalMethods.GetScalarString("SELECT Mod1ValueMin FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                            yourRow[5] = GlobalMethods.GetScalarString("SELECT Mod1ValueMax FROM " + AffixTypeString + " WHERE Mod1Id = " + m.Id + " AND Mod1ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod1ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                            yourRow[6] = GlobalMethods.StashItem.Mod[mod].Value;
                            YourRollGrid.Rows.Add(yourRow);
                        }
                        if (position == "Secondary")
                        {
                            yourRow[0] = GlobalMethods.GetScalarString("SELECT Name FROM " + AffixTypeString + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod2ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                            yourRow[1] = "Secondary";
                            yourRow[2] = GlobalMethods.GetScalarString("SELECT Level FROM " + AffixTypeString + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod2ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                            yourRow[3] = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + m.Id + ";");
                            yourRow[4] = GlobalMethods.GetScalarString("SELECT Mod2ValueMin FROM " + AffixTypeString + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod2ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                            yourRow[5] = GlobalMethods.GetScalarString("SELECT Mod2ValueMax FROM " + AffixTypeString + " WHERE Mod2Id = " + m.Id + " AND Mod2ValueMin <= " + GlobalMethods.StashItem.Mod[mod].Value + " AND Mod2ValueMax >= " + GlobalMethods.StashItem.Mod[mod].Value + ";");
                            yourRow[6] = GlobalMethods.StashItem.Mod[mod].Value;
                            YourRollGrid.Rows.Add(yourRow);
                        }
                    }
                }
            }
        }
    }
}
