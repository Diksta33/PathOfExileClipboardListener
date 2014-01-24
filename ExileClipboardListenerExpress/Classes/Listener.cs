using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ExileClipboardListener.WinForms;

namespace ExileClipboardListener.Classes
{
    //Provides notification when the contents of the clipboard is updated
    public sealed class ClipboardNotification
    {
        //private class BaseItem
        //{
        //    public string ItemName;
        //    public int ItemTypeId;
        //    public int ItemSubTypeId;
        //    public int ReqLevel;
        //    public int ReqStr;
        //    public int ReqDex;
        //    public int ReqInt;
        //    //and more
        //}

        //Occurs when the contents of the clipboard is updated
        public static event EventHandler ClipboardUpdate;
        private static bool _loaded;

        #pragma warning disable 169
        private static NotificationForm _form = new NotificationForm();
        #pragma warning restore 169

        public class ModPair
        {
            public int ModId;
            public int ModValue;
        }

        public class Fix
        {
            public int FixId;
            public int Mod1Id;
            public int Mod1ValueMin;
            public int Mod1ValueMax;
            public int Mod1Value;
            public int Mod2Id;
            public int Mod2ValueMin;
            public int Mod2ValueMax;
            public int Mod2Value;
        }

        private static void OnClipboardUpdate(EventArgs e)
        {
            string item = Clipboard.GetText();

            //If we are already loaded then do nothing
            if (_loaded)
                return;

            //First we need to confirm that this is a POE item
            if (item.Length < 20)
                return;
            if (item.Substring(0, 6) != "Rarity")
                return;

            //We also need to make sure it has been identified
            if (item.Contains("Unidentified"))
            {
                new PopUpError().Show("You must identify items first!");
                return;
            }

            //Remove the seperators
            //item.Replace("--------", "");

            //Remove any (augmented) text as we don't really care
            item = item.Replace(" (augmented)", "");

            //Same goes for junk at the start
            item = item.Replace("Adds ", "");
            item = item.Replace("Reflects ", "");

            //Parse the details
            var entity = item.Split(new[] { "\r\n" }, StringSplitOptions.None);

            //There are a number of sections in the entity that vary depending on rarity and item type
            //
            //                      Jewellery       Armour/ Weapons
            //Basics                X               X
            //Item Stats                            X
            //Requirements          X               X
            //Sockets                               X
            //Item Level            X               X
            //Implicit Mods         *               * (optional)
            //Prefixes/ Suffixes    *               * (optional)
            //
            //Implicit Mods only appear on items where the Base Item entry has at least one implict mod
            //Prefixes/ Suffixes only appear on Magic Items or better
            //Unique Items might have mods that aren't on our list so we just ignore them
            //
            //First we need to see how many sections there are
            //int sectionCount = entity.Count(s => s == "--------");

            //If we get here then something is going to end up being messed up if we already have the pop up form open
            _loaded = true;
            GlobalMethods.ClearStash();

            //The first section is always the basic details
            GlobalMethods.StashItem.ItemName = entity[1];
            GlobalMethods.StashItem.Quality = FindAnyValue<int>(entity, "Quality");
            GlobalMethods.StashItem.RarityId = GlobalMethods.GetScalarInt("SELECT RarityId FROM Rarity WHERE RarityName = '" + entity[0].Split(':')[1].Trim() + "';");

            //Parse out the base item name
            //For normal items there is no item name other than the base item name
            if (GlobalMethods.StashItem.RarityId == 1)
                GlobalMethods.StashItem.BaseItemId = GlobalMethods.GetScalarInt("SELECT BaseItemId FROM BaseItem WHERE ItemName = '" + GlobalMethods.StashItem.ItemName.Replace("'", "''") + "';");

            //Magic items have the base item name embedded in the magic name
            if (GlobalMethods.StashItem.RarityId == 2)
            {
                string name = GlobalMethods.StashItem.ItemName;

                //Remove any suffix
                name = name.Split(new[] { " of " }, StringSplitOptions.None)[0];
                GlobalMethods.StashItem.BaseItemId = GlobalMethods.GetScalarInt("SELECT BaseItemId FROM BaseItem WHERE ItemName = '" + name.Replace("'", "''") + "';");
                
                //We might also need to remove the prefix
                if (GlobalMethods.StashItem.BaseItemId == 0)
                {
                    string prefix = name.Split(' ')[0];
                    name = name.Substring(prefix.Length + 1, name.Length - prefix.Length - 1);
                    GlobalMethods.StashItem.BaseItemId = GlobalMethods.GetScalarInt("SELECT BaseItemId FROM BaseItem WHERE ItemName = '" + name.Replace("'", "''") + "';");
                }
            }
            
            //Rare items have a rare name and then the base item name, as do uniques
            if (GlobalMethods.StashItem.RarityId == 3 || GlobalMethods.StashItem.RarityId == 4)
                GlobalMethods.StashItem.BaseItemId = GlobalMethods.GetScalarInt("SELECT BaseItemId FROM BaseItem WHERE ItemName = '" + entity[2].Replace("'", "''") + "';");
            RemoveSection(ref entity);

            //And we stop here for Uniques for now anyway
            if (GlobalMethods.StashItem.RarityId == 4)
                return;

            //The second section is the item data, we can technically ignore this as we should be able to recalculate this
            //But we will eventually pull it out anyway
            //Jewellery doesn't have a section here, so we need to determine the item type
            string itemTypeName = GlobalMethods.GetScalarString("SELECT it.ItemTypeName FROM BaseItem bi INNER JOIN ItemType it ON it.ItemTypeId = bi.ItemTypeId WHERE bi.BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
            string itemSubTypeName = GlobalMethods.GetScalarString("SELECT it.ItemSubTypeName FROM BaseItem bi INNER JOIN ItemSubType it ON it.ItemTypeId = bi.ItemTypeId AND it.ItemSubTypeId = bi.ItemSubTypeId WHERE bi.BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
            GlobalMethods.StashItem.ItemTypeName = itemTypeName;
            GlobalMethods.StashItem.ItemSubTypeName = itemSubTypeName;
            if (itemTypeName != "Jewellery")
            {
                //Armour
                GlobalMethods.StashItem.Armour = FindAnyValue<int>(entity, "Armour");
                GlobalMethods.StashItem.Evasion = FindAnyValue<int>(entity, "Evasion");
                GlobalMethods.StashItem.EnergyShield = FindAnyValue<int>(entity, "Energy Shield");

                //Weapons
                GlobalMethods.StashItem.PhysicalDamageMin = FindAnyValue<int>(entity, "Physical Damage");
                GlobalMethods.StashItem.PhysicalDamageMax = FindAnyValue<int>(entity, "Physical Damage", 1);
                GlobalMethods.StashItem.ElementalDamageMin = FindAnyValue<int>(entity, "Elemental Damage");
                GlobalMethods.StashItem.ElementalDamageMax = FindAnyValue<int>(entity, "Elemental Damage", 1);
                GlobalMethods.StashItem.CriticalStrikeChance = FindAnyValue<decimal>(entity, "Critical Strike Chance");
                GlobalMethods.StashItem.AttacksPerSecond = FindAnyValue<decimal>(entity, "Attacks per Second");
                RemoveSection(ref entity);
            }

            //Requirements
            //(but these are on the base item already so we ignore them)
            //var reqStr = Math.Max(FindAnyValue<int>(entity, "Str"), FindValue(entity, "Str (gem)"));
            //var reqInt = Math.Max(FindAnyValue<int>(entity, "Int"), FindValue(entity, "Int (gem)"));
            //var reqDex = Math.Max(FindAnyValue<int>(entity, "Dex"), FindValue(entity, "Dex (gem)"));
            GlobalMethods.StashItem.ReqLevel = FindAnyValue<int>(entity, "Level");
            RemoveSection(ref entity);

            //Sockets
            //For now just lose them
            if (itemTypeName != "Jewellery")
            {
                RemoveSection(ref entity);
            }

            //Item Level
            GlobalMethods.StashItem.ItemLevel = FindAnyValue<int>(entity, "Itemlevel");
            RemoveSection(ref entity);

            //Implict modifiers
            bool SeenImplicit = false;

            //Primary
            //If there is a primary implicit mod then it's just a case of looking up the Min Value as there is never a max (for now!)
            GlobalMethods.StashItem.ImplicitMod1Id = GlobalMethods.GetScalarInt("SELECT Mod1Id FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
            if (GlobalMethods.StashItem.ImplicitMod1Id != 0)
            {
                //If the mod has an "<implicit" at the start of the name then the value isn't rolled, it's just taken directly from the base item and won't appear in the item text anyway
                if (GlobalMethods.GetScalarString("SELECT ModRealName FROM [Mod] WHERE ModId = " + GlobalMethods.StashItem.ImplicitMod1Id + ";").Contains("<implicit"))
                {
                    GlobalMethods.StashItem.ImplicitMod1Value = GlobalMethods.GetScalarInt("SELECT Mod1ValueMin FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
                }
                else
                {
                    //It won't always be this easy
                    GlobalMethods.StashItem.ImplicitMod1Value = FindMod(entity, GlobalMethods.GetScalarString("SELECT ISNULL(ModRealName, ModName) FROM [Mod] WHERE ModId = " + GlobalMethods.StashItem.ImplicitMod1Id + ";"));
                    SeenImplicit = true;
                }
            }

            //Secondary
            //If there is a secondary implicit mod then it's just a case of looking up the values and storing them (as there is no roll - yet!)
            GlobalMethods.StashItem.ImplicitMod2Id = GlobalMethods.GetScalarInt("SELECT Mod2Id FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
            if (GlobalMethods.StashItem.ImplicitMod2Id != 0)
            {
                //It won't always be this easy
                GlobalMethods.StashItem.ImplicitMod2Value = GlobalMethods.GetScalarInt("SELECT Mod2ValueMin FROM BaseItem WHERE BaseItemId = " + GlobalMethods.StashItem.BaseItemId + ";");
            }

            //There may not have been any implict mods so we take care removing this section
            if (SeenImplicit)
                RemoveSection(ref entity);
            
            //Prefixes & Suffixes
            //These are lumped together
            //We need to pull out all the individual mods and then see if they map to a prefix or a suffix
            var mods = new List<ModPair>();
            foreach (string s in entity)
            {
                if (s.Contains(" "))
                {
                    string modValue = s.Split(' ')[0];
                    string modName = s.Substring(modValue.Length + 1, s.Length - modValue.Length - 1).Trim();

                    //We need to be a little careful, this is the first mod so only pick mods that are 1st in a sequence, e.g. phyiscal damage min/ max
                    //We also need to check if the mod is allowed on this item type
                    int modId = GlobalMethods.GetScalarInt("SELECT ModId FROM [Mod] WHERE ISNULL(ModRealName, ModName) = '" + modName + "' AND ISNULL(ModPair, 1) = 1 AND ISNULL(" + itemTypeName + ", 1) = 1;");
                    if (modId == 0)
                    {
                        MessageBox.Show("Failed to find a mod with a name of " + modName + "!");
                    }
                    else
                    {
                        //We found a mod, so cache it
                        var mod = new ModPair {ModId = modId};
                        modValue = modValue.Replace("%", "");
                        modValue = modValue.Replace("+", "");

                        //Life Regen is a pain because it needs multiplying up to get a value we can use
                        if (modName == "Life Regenerated per second")
                        {
                            mod.ModValue = Convert.ToInt32(Convert.ToDecimal(modValue) * 60);
                        }
                        else
                        {
                            mod.ModValue = modValue.Contains("-") ? Convert.ToInt32(modValue.Split(new[] { "-" }, StringSplitOptions.None)[0]) : Convert.ToInt32(modValue);
                        }
                        mods.Add(mod);

                        //Check to see if this is a mod pair, if it is then match the secondary mod
                        //Mod Pairs always have range values, e.g. 1-5, the minimum value is recorded against the first mod and the maximum value is recorded against the second mod
                        modId = GlobalMethods.GetScalarInt("SELECT ModId FROM [Mod] WHERE ISNULL(ModRealName, ModName) = '" + modName + "' AND ISNULL(ModPair, 1) = 2 AND ISNULL(" + itemTypeName + ", 1) = 1;");
                        if (modId != 0)
                        {
                            var mod2 = new ModPair { ModId = modId };
                            mod2.ModValue = modValue.Contains("-") ? Convert.ToInt32(modValue.Split(new[] { "-" }, StringSplitOptions.None)[1]) : 0;
                            mods.Add(mod2);
                        }
                    }
                }
            }

            //Because some prefixes/ suffixes have two mods we need to find these first and then scoop up whatever is left as single mod "-ixes"
            //We also have the situation of double mods, e.g. Item Quantity is a prefix and a suffix, in these cases we are limited to how much we can determine
            //For Item Quantity there are three ranges, 8-12, 13-18 and 19-24.  As an example, if an item has an Item Quantity roll of 21 then this could be:
            // - Prefix = 8, Suffix = 13 (prefix is rank #3, suffix is rank #2)
            // - Prefix = 11, Suffix = 10 (both are rank #2)
            // - Prefix = 21 (rank #1)
            // - Suffix = 21 (rank #1)
            var prefixes = new List<Fix>();
            var suffixes = new List<Fix>();

            //Find prefixes with two mods
            List<Fix> test = GlobalMethods.StuffList("SELECT DISTINCT PrefixId AS FixId, Mod1Id, Mod1ValueMin, Mod1ValueMax, NULL, Mod2Id, Mod2ValueMin, Mod2ValueMax, NULL FROM Prefix WHERE Mod2Id IS NOT NULL;");
            foreach (var f in test)
            {
                int matched = 0;
                var mpMod1 = new ModPair();
                var mpMod2 = new ModPair();
                foreach (var mp in mods)
                {
                    if (mp.ModId == f.Mod1Id)
                    {
                        f.Mod1Value = mp.ModValue;
                        mpMod1 = mp;
                        matched++;
                    }
                    if (mp.ModId == f.Mod2Id)
                    {
                        f.Mod2Value = mp.ModValue;
                        mpMod2 = mp;
                        matched++;
                    }
                }
                if (matched == 2)
                {
                    //We got a hit
                    prefixes.Add(f);
                    mods.Remove(mpMod1);
                    mods.Remove(mpMod2);
                }
            }

            //Find suffixes with two mods
            test = GlobalMethods.StuffList("SELECT DISTINCT SuffixId AS FixId, Mod1Id, Mod1ValueMin, Mod1ValueMax, Mod2Id, Mod2ValueMin, Mod2ValueMax FROM Suffix WHERE Mod2Id IS NOT NULL;");
            foreach (var f in test)
            {
                int matched = 0;
                var mpMod1 = new ModPair();
                var mpMod2 = new ModPair();
                foreach (var mp in mods)
                {
                    if (mp.ModId == f.Mod1Id)
                    {
                        f.Mod1Value = mp.ModValue;
                        mpMod1 = mp;
                        matched++;
                    }
                    if (mp.ModId == f.Mod2Id)
                    {
                        f.Mod2Value = mp.ModValue;
                        mpMod2 = mp;
                        matched++;
                    }
                }
                if (matched == 2)
                {
                    //We got a hit
                    suffixes.Add(f);
                    mods.Remove(mpMod1);
                    mods.Remove(mpMod2);
                }
            }

            //Find prefixes with one mod
            test = GlobalMethods.StuffList("SELECT DISTINCT PrefixId AS FixId, Mod1Id, Mod1ValueMin, Mod1ValueMax, Mod2Id, Mod2ValueMin, Mod2ValueMax FROM Prefix WHERE Mod2Id IS NULL;");
            foreach (var f in test)
            {
                foreach (var mp in mods)
                {
                    if (mp.ModId == f.Mod1Id)
                    {
                        //We got a hit
                        f.Mod1Value = mp.ModValue;
                        prefixes.Add(f);
                        mods.Remove(mp);
                        break;
                    }
                }
            }

            //Find suffixes with one mod
            test = GlobalMethods.StuffList("SELECT DISTINCT SuffixId AS FixId, Mod1Id, Mod1ValueMin, Mod1ValueMax, Mod2Id, Mod2ValueMin, Mod2ValueMax FROM Suffix WHERE Mod2Id IS NULL;");
            foreach (var f in test)
            {
                foreach (var mp in mods)
                {
                    if (mp.ModId == f.Mod1Id)
                    {
                        //We got a hit
                        f.Mod1Value = mp.ModValue;
                        suffixes.Add(f);
                        mods.Remove(mp);
                        break;
                    }
                }
            }

            //Pop off the prefixes and suffixes into the StashItem class
            for (int prefix = 0; prefix < prefixes.Count(); prefix++)
            {
                if (prefix == 0)
                {
                    GlobalMethods.StashItem.Prefix1Id = prefixes[prefix].FixId;
                    GlobalMethods.StashItem.Prefix1Mod1Id = prefixes[prefix].Mod1Id;
                    GlobalMethods.StashItem.Prefix1Mod1Value = prefixes[prefix].Mod1Value;
                    GlobalMethods.StashItem.Prefix1Mod2Id = prefixes[prefix].Mod2Id;
                    GlobalMethods.StashItem.Prefix1Mod2Value = prefixes[prefix].Mod2Value;
                }
                if (prefix == 1)
                {
                    GlobalMethods.StashItem.Prefix2Id = prefixes[prefix].FixId;
                    GlobalMethods.StashItem.Prefix2Mod1Id = prefixes[prefix].Mod1Id;
                    GlobalMethods.StashItem.Prefix2Mod1Value = prefixes[prefix].Mod1Value;
                    GlobalMethods.StashItem.Prefix2Mod2Id = prefixes[prefix].Mod2Id;
                    GlobalMethods.StashItem.Prefix2Mod2Value = prefixes[prefix].Mod2Value;
                }
                if (prefix == 2)
                {
                    GlobalMethods.StashItem.Prefix3Id = prefixes[prefix].FixId;
                    GlobalMethods.StashItem.Prefix3Mod1Id = prefixes[prefix].Mod1Id;
                    GlobalMethods.StashItem.Prefix3Mod1Value = prefixes[prefix].Mod1Value;
                    GlobalMethods.StashItem.Prefix3Mod2Id = prefixes[prefix].Mod2Id;
                    GlobalMethods.StashItem.Prefix3Mod2Value = prefixes[prefix].Mod2Value;
                }
            }
            for (int suffix = 0; suffix < suffixes.Count(); suffix++)
            {
                if (suffix == 0)
                {
                    GlobalMethods.StashItem.Suffix1Id = suffixes[suffix].FixId;
                    GlobalMethods.StashItem.Suffix1Mod1Id = suffixes[suffix].Mod1Id;
                    GlobalMethods.StashItem.Suffix1Mod1Value = suffixes[suffix].Mod1Value;
                    GlobalMethods.StashItem.Suffix1Mod2Id = suffixes[suffix].Mod2Id;
                    GlobalMethods.StashItem.Suffix1Mod2Value = suffixes[suffix].Mod2Value;
                }
                if (suffix == 1)
                {
                    GlobalMethods.StashItem.Suffix2Id = suffixes[suffix].FixId;
                    GlobalMethods.StashItem.Suffix2Mod1Id = suffixes[suffix].Mod1Id;
                    GlobalMethods.StashItem.Suffix2Mod1Value = suffixes[suffix].Mod1Value;
                    GlobalMethods.StashItem.Suffix2Mod2Id = suffixes[suffix].Mod2Id;
                    GlobalMethods.StashItem.Suffix2Mod2Value = suffixes[suffix].Mod2Value;
                }
                if (suffix == 2)
                {
                    GlobalMethods.StashItem.Suffix3Id = suffixes[suffix].FixId;
                    GlobalMethods.StashItem.Suffix3Mod1Id = suffixes[suffix].Mod1Id;
                    GlobalMethods.StashItem.Suffix3Mod1Value = suffixes[suffix].Mod1Value;
                    GlobalMethods.StashItem.Suffix3Mod2Id = suffixes[suffix].Mod2Id;
                    GlobalMethods.StashItem.Suffix3Mod2Value = suffixes[suffix].Mod2Value;
                }
            }

            //Is there anything left over?
            if (mods.Count() != 0)
                MessageBox.Show("Not all -ixes were parsed!");

            //If we are in collection mode pop up a window
            DialogResult dr = DialogResult.None;
            if (GlobalMethods.Mode == GlobalMethods.COLLECTION_MODE)
                dr = new ItemValue().ShowDialog();

            //Stash the item if we are in stash mode or said to stash it from the pop up
            if (GlobalMethods.Mode == GlobalMethods.STASH_MODE || dr == DialogResult.OK)
            {
                SaveStash();
                if (Properties.Settings.Default.StashPopUpMode != 0)
                    new PopUpStashed().ShowDialog();
            }

            //Now handle the event
            var handler = ClipboardUpdate;
            if (handler != null)
                handler(null, e);

            //We are no longer loaded
            _loaded = false;
        }

        private static void SaveStash()
        {
            //Save this item to the database
            string sql = "INSERT INTO Stash(StashId, ItemName, BaseItemId, RarityId, Quality, ItemLevel, ReqLevel,";
            sql += " Armour, Evasion, EnergyShield, DamagePhysicalMin, DamagePhysicalMax, DamageElementalMin, DamageElementalMax,";
            sql += " ImplicitMod1Id, ImplicitMod1Value, ImplicitMod2Id, ImplicitMod2Value,";
            sql += " Prefix1Id, Prefix1Mod1Id, Prefix1Mod1Value, Prefix1Mod2Id, Prefix1Mod2Value,";
            sql += " Prefix2Id, Prefix2Mod1Id, Prefix2Mod1Value, Prefix2Mod2Id, Prefix2Mod2Value,";
            sql += " Prefix3Id, Prefix3Mod1Id, Prefix3Mod1Value, Prefix3Mod2Id, Prefix3Mod2Value,";
            sql += " Suffix1Id, Suffix1Mod1Id, Suffix1Mod1Value, Suffix1Mod2Id, Suffix1Mod2Value,";
            sql += " Suffix2Id, Suffix2Mod1Id, Suffix2Mod1Value, Suffix2Mod2Id, Suffix2Mod2Value,";
            sql += " Suffix3Id, Suffix3Mod1Id, Suffix3Mod1Value, Suffix3Mod2Id, Suffix3Mod2Value)";
            sql += " VALUES(";
            sql += "NEWID(),";

            //Basic Details
            sql += "'" + GlobalMethods.StashItem.ItemName + "',";
            sql += (GlobalMethods.StashItem.BaseItemId == 0 ? "NULL" : GlobalMethods.StashItem.BaseItemId.ToString()) + ",";
            sql += GlobalMethods.StashItem.RarityId + ",";
            sql += GlobalMethods.StashItem.Quality + ",";
            sql += GlobalMethods.StashItem.ItemLevel + ",";
            sql += GlobalMethods.StashItem.ReqLevel + ",";

            //Armour
            sql += GlobalMethods.StashItem.Armour + ",";
            sql += GlobalMethods.StashItem.Evasion + ",";
            sql += GlobalMethods.StashItem.EnergyShield + ",";

            //Weapons
            sql += "NULL, NULL, NULL, NULL,";

            //Implicit Mods
            sql += (GlobalMethods.StashItem.ImplicitMod1Id == 0 ? "NULL" : GlobalMethods.StashItem.ImplicitMod1Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.ImplicitMod1Value == 0 ? "NULL" : GlobalMethods.StashItem.ImplicitMod1Value.ToString()) + ",";
            sql += (GlobalMethods.StashItem.ImplicitMod2Id == 0 ? "NULL" : GlobalMethods.StashItem.ImplicitMod2Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.ImplicitMod2Value == 0 ? "NULL" : GlobalMethods.StashItem.ImplicitMod2Value.ToString()) + ",";

            //Prefixes
            sql += (GlobalMethods.StashItem.Prefix1Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix1Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Prefix1Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix1Mod1Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Prefix1Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix1Mod1Value.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Prefix1Id == 0 || GlobalMethods.StashItem.Prefix1Mod2Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix1Mod2Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Prefix1Id == 0 || GlobalMethods.StashItem.Prefix1Mod2Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix1Mod2Value.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Prefix2Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix2Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Prefix2Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix2Mod1Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Prefix2Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix2Mod1Value.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Prefix2Id == 0 || GlobalMethods.StashItem.Prefix2Mod2Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix2Mod2Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Prefix2Id == 0 || GlobalMethods.StashItem.Prefix2Mod2Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix2Mod2Value.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Prefix3Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix3Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Prefix3Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix3Mod1Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Prefix3Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix3Mod1Value.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Prefix3Id == 0 || GlobalMethods.StashItem.Prefix3Mod2Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix3Mod2Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Prefix3Id == 0 || GlobalMethods.StashItem.Prefix3Mod2Id == 0 ? "NULL" : GlobalMethods.StashItem.Prefix3Mod2Value.ToString()) + ",";

            //Suffixes
            sql += (GlobalMethods.StashItem.Suffix1Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix1Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Suffix1Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix1Mod1Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Suffix1Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix1Mod1Value.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Suffix1Id == 0 || GlobalMethods.StashItem.Suffix1Mod2Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix1Mod2Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Suffix1Id == 0 || GlobalMethods.StashItem.Suffix1Mod2Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix1Mod2Value.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Suffix2Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix2Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Suffix2Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix2Mod1Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Suffix2Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix2Mod1Value.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Suffix2Id == 0 || GlobalMethods.StashItem.Suffix2Mod2Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix2Mod2Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Suffix2Id == 0 || GlobalMethods.StashItem.Suffix2Mod2Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix2Mod2Value.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Suffix3Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix3Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Suffix3Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix3Mod1Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Suffix3Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix3Mod1Value.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Suffix3Id == 0 || GlobalMethods.StashItem.Suffix3Mod2Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix3Mod2Id.ToString()) + ",";
            sql += (GlobalMethods.StashItem.Suffix3Id == 0 || GlobalMethods.StashItem.Suffix3Mod2Id == 0 ? "NULL" : GlobalMethods.StashItem.Suffix3Mod2Value.ToString()) + ")";

            //Stash this item
            GlobalMethods.RunQuery(sql);
        }

        private static void RemoveSection(ref string[] entity)
        {
            //Scans the item, removing anything up to and including the first seperator or the end of the item
            for (int i = 0; i < entity.Count(); i++)
            {
                if (entity[i] == "--------")
                {
                    entity[i] = "";
                    break;
                }
                entity[i] = "";
            }
        }

        //Generic method
        private static T FindAnyValue<T>(IEnumerable<string> entity, string tag, int pair = 0)
        {
            //Look for a tag and if found return the value associated with it as an integer
            foreach (var item in entity)
            {
                //If we hit a seperator then stop
                if (item == "--------")
                    break;
                if (item.Contains(":"))
                {
                    var tagName = item.Split(':')[0];
                    if (tagName == tag)
                    {
                        var valueString = item.Split(':')[1];
                        valueString = valueString.Replace("+", "");
                        valueString = valueString.Replace("%", "");
                        valueString = valueString.Replace("(augmented)", "");
                        valueString = valueString.Replace("(gem)", "");
                        valueString = valueString.Replace("(unmet)", "");
                        valueString = valueString.Trim();
                        if (valueString.Contains("-"))
                            valueString = valueString.Split('-')[pair];
                        //int value;
                        //if (int.TryParse(valueString, out value))
                        return (T) Convert.ChangeType(valueString, typeof (T));
                    }
                }
            }
            return (T) Convert.ChangeType(0, typeof (T));
        }


        //private static int FindValue(IEnumerable<string> entity, string tag)
        //{
        //    //Look for a tag and if found return the value associated with it as an integer
        //    foreach (var item in entity)
        //    {
        //        //If we hit a seperator then stop
        //        if (item == "--------")
        //            break;
        //        if (item.Contains(":"))
        //        {
        //            var tagName = item.Split(':')[0];
        //            if (tagName == tag)
        //            {
        //                var valueString = item.Split(':')[1];
        //                valueString = valueString.Replace("+", "");
        //                valueString = valueString.Replace("%", "");
        //                valueString = valueString.Replace("(augmented)", "");
        //                valueString = valueString.Trim();
        //                int value;
        //                if (int.TryParse(valueString, out value))
        //                    return value;
        //            }
        //        }
        //    }
        //    return 0;
        //}

        private static int FindMod(IEnumerable<string> entity, string tag)
        {
            //Look for a tag and if found return the value associated with it as an integer
            foreach (var item in entity)
            {
                //If we hit a seperator then stop
                if (item == "--------")
                    break;
                if (item.Contains(tag))
                {
                    var valueString = item.Split(' ')[0];
                    valueString = valueString.Replace("+", "");
                    valueString = valueString.Replace("%", "");
                    valueString = valueString.Replace("(augmented)", "");
                    valueString = valueString.Trim();
                    int value;
                    if (int.TryParse(valueString, out value))
                        return value;
                }
            }
            return 0;
        }

        //Hidden form to recieve the WM_CLIPBOARDUPDATE message
        private class NotificationForm : Form
        {
            public NotificationForm()
            {
                NativeMethods.SetParent(Handle, NativeMethods.HWND_MESSAGE);
                NativeMethods.AddClipboardFormatListener(Handle);
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE)
                {
                    OnClipboardUpdate(null);
                }
                base.WndProc(ref m);
            }
        }
    }

    internal static class NativeMethods
    {
        //ReSharper disable InconsistentNaming
        public const int WM_CLIPBOARDUPDATE = 0x031D;
        public static IntPtr HWND_MESSAGE = new IntPtr(-3);
        //ReSharper restore InconsistentNaming

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
    }
}
