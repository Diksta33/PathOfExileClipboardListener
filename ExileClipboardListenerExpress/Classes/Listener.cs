using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ExileClipboardListener.WinForms;
using si = ExileClipboardListener.Classes.GlobalMethods.StashItem;
using bi = ExileClipboardListener.Classes.GlobalMethods.BaseItem;

namespace ExileClipboardListener.Classes
{
    //Provides notification when the contents of the clipboard is updated
    public sealed class ClipboardNotification
    {
        //Occurs when the contents of the clipboard is updated
        public static event EventHandler ClipboardUpdate;
        private static bool _loaded;

        #pragma warning disable 169
        private static NotificationForm _form = new NotificationForm();
        #pragma warning restore 169

        private static void OnClipboardUpdate(EventArgs e)
        {
            try
            {
                string item = Clipboard.GetText();

                //If we are already loaded then do nothing
                if (_loaded)
                    return;

                //First we need to confirm that this is a POE item
                if (!CheckItem(item))
                    return;

                //We also need to make sure it has been identified
                if (item.Contains("Unidentified"))
                {
                    new PopUpError().Show("You must identify items first!");
                    return;
                }

                //Remove any superfluous text but store the original text first
                GlobalMethods.ClearStash();
                si.OriginalText = item;
                item = item.Replace(" (augmented)", "");
                item = item.Replace("Adds ", "");
                item = item.Replace("Reflects ", "");
                item = item.Replace("Recharges ", "");
                item = item.Replace("Grants ", "");
                item = item.Replace("Removes ", "");

                //Parse the details
                var entity = item.Split(new[] { "\n" }, StringSplitOptions.None);
                for (int i = 0; i < entity.Count(); i++)
                {
                    entity[i] = entity[i].Replace("\n", "");
                    entity[i] = entity[i].Replace("\r", "");
                }

                //If we get here then something is going to end up being messed up if we already have the pop up form open
                _loaded = true;
                ParseStash(entity);

                //Now handle the event
                var handler = ClipboardUpdate;
                if (handler != null)
                    handler(null, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //We are no longer loaded
                _loaded = false;
            }
        }

        private static bool CheckItem(string item)
        {
            if (item.Length < 20)
                return false;
            if (item.Substring(0, 6) != "Rarity")
                return false;
            return true;
        }

        private static void ParseStash(string[] entity)
        {
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
            try
            {
                //The first section is always the basic details
                if (entity.Count() < 2)
                {
                    MessageBox.Show("Couldn't split item did you lose the carriage-returns somehow?");
                    return;
                }

                si.ItemName = entity[1];
                si.Quality = FindAnyValue<int>(entity, "Quality");
                si.RarityId = GlobalMethods.GetScalarInt("SELECT RarityId FROM Rarity WHERE RarityName = '" + entity[0].Split(':')[1].Trim() + "';");

                //Parse out the base item name
                if (si.RarityId == 0)
                {
                    MessageBox.Show("Catastrophic Failure!");
                    _loaded = false;
                    return;
                }

                //For normal items there is no item name other than the base item name
                if (si.RarityId == 1)
                    si.BaseItemId = GlobalMethods.GetScalarInt("SELECT BaseItemId FROM BaseItem WHERE ItemName = '" + si.ItemName.Replace("'", "''") + "';");

                //Magic items have the base item name embedded in the magic name
                if (si.RarityId == 2)
                {
                    string name = si.ItemName;

                    //Remove any suffix
                    name = name.Split(new[] { " of " }, StringSplitOptions.None)[0];
                    si.BaseItemId = GlobalMethods.GetScalarInt("SELECT BaseItemId FROM BaseItem WHERE ItemName = '" + name.Replace("'", "''") + "';");

                    //We might also need to remove the prefix
                    if (si.BaseItemId == 0)
                    {
                        string prefix = name.Split(' ')[0];
                        name = name.Substring(prefix.Length + 1, name.Length - prefix.Length - 1);
                        si.BaseItemId = GlobalMethods.GetScalarInt("SELECT BaseItemId FROM BaseItem WHERE ItemName = '" + name.Replace("'", "''") + "';");
                    }
                }

                //Rare items have a rare name and then the base item name, as do uniques
                if (si.RarityId == 3 || si.RarityId == 4)
                    si.BaseItemId = GlobalMethods.GetScalarInt("SELECT BaseItemId FROM BaseItem WHERE ItemName = '" + entity[2].Replace("'", "''") + "';");
                RemoveSection(ref entity);

                //And we stop here for Uniques for now anyway
                if (si.RarityId == 4 || si.BaseItemId == 0)
                {
                    _loaded = false;
                    return;
                }

                //If we have a base item then load it once
                GlobalMethods.LoadBaseItem(si.BaseItemId);

                //The second section is the item data
                //Jewellery doesn't have a section, so we need to determine the item type first from the Base Item
                string itemTypeName = bi.ItemTypeName;
                string itemSubTypeName = bi.ItemSubTypeName;
                si.ItemTypeName = itemTypeName;
                si.ItemSubTypeName = itemSubTypeName;
                if (itemTypeName != "Jewellery")
                {
                    //Armour
                    si.Armour = FindAnyValue<int>(entity, "Armour");
                    si.Evasion = FindAnyValue<int>(entity, "Evasion Rating");
                    si.EnergyShield = FindAnyValue<int>(entity, "Energy Shield");

                    //Weapons
                    si.PhysicalDamageMin = FindAnyValue<int>(entity, "Physical Damage", 0);
                    si.PhysicalDamageMax = FindAnyValue<int>(entity, "Physical Damage", 1);
                    si.ElementalDamageMin = FindAnyValue<int>(entity, "Elemental Damage", 0);
                    si.ElementalDamageMax = FindAnyValue<int>(entity, "Elemental Damage", 1);
                    si.CriticalStrikeChance = FindAnyValue<decimal>(entity, "Critical Strike Chance");
                    si.AttacksPerSecond = FindAnyValue<decimal>(entity, "Attacks per Second");
                    si.BaseAttacksPerSecond = bi.AttackSpeed;

                    //We only get the base DPS for now
                    si.DamagePerSecond = bi.DPS;
                    RemoveSection(ref entity);
                }

                //Requirements
                //(but these are on the base item already so we ignore them)
                //var reqStr = Math.Max(FindAnyValue<int>(entity, "Str"), FindValue(entity, "Str (gem)"));
                //var reqInt = Math.Max(FindAnyValue<int>(entity, "Int"), FindValue(entity, "Int (gem)"));
                //var reqDex = Math.Max(FindAnyValue<int>(entity, "Dex"), FindValue(entity, "Dex (gem)"));
                si.ReqLevel = FindAnyValue<int>(entity, "Level");
                si.ReqLevelBase = bi.ReqLevel;
                RemoveSection(ref entity);

                //Sockets
                //For now just store them
                if (itemTypeName != "Jewellery")
                {
                    si.Sockets = FindAnyValue<string>(entity, "Sockets");
                    RemoveSection(ref entity);
                }

                //Item Level
                si.ItemLevel = FindAnyValue<int>(entity, "Itemlevel");
                RemoveSection(ref entity);

                //Implict modifiers, there may not be any so we are a little careful
                bool seenImplicit = false;

                //Primary
                si.Affix[0].Mod1.Id = bi.Mod1.Id; 
                if (si.Affix[0].Mod1.Id != 0)
                {
                    var implicitMod = GlobalMethods.LookUpMod(si.Affix[0].Mod1.Id);

                    //For primary implicit mods there might be a roll in the item script
                    si.Affix[0].Mod1.Value = FindMod(entity, implicitMod.Name);
                    if (si.Affix[0].Mod1.Value == 0)
                        si.Affix[0].Mod1.Value = bi.Mod1.ValueMin;
                    seenImplicit = true;

                    //We also want the minimum and maximum values from the base item
                    si.Affix[0].Mod1.ValueMin = bi.Mod1.ValueMin;
                    si.Affix[0].Mod1.ValueMax = bi.Mod1.ValueMax;
                }

                //Secondary
                //If there is a secondary implicit mod then it's just a case of looking up the values and storing them (as there is no roll - yet!)
                si.Affix[0].Mod2.Id = bi.Mod2.Id; 
                if (si.Affix[0].Mod2.Id != 0)
                {
                    //It won't always be this easy
                    si.Affix[0].Mod2.Value = bi.Mod2.ValueMin;

                    //We also want the minimum and maximum values from the base item
                    si.Affix[0].Mod2.ValueMin = bi.Mod2.ValueMin;
                    si.Affix[0].Mod2.ValueMax = bi.Mod2.ValueMax;
                }

                //There may not have been any implict mods so we take care removing this section
                if (seenImplicit)
                    RemoveSection(ref entity);

                //We need to pull out all the individual mods and then see if they map to a prefix or a suffix
                var mods = new List<GlobalMethods.Mod>();
                foreach (string s in entity)
                {
                    if (s.Contains(" "))
                    {
                        string modValue = s.Split(' ')[0];
                        string modName = s.Substring(modValue.Length + 1, s.Length - modValue.Length - 1).Trim();

                        //We need to be a little careful, this is the first mod so only pick mods that are 1st in a sequence, e.g. phyiscal damage min/ max
                        //We also need to check if the mod is allowed on this item type
                        //We sometimes have implict mods that have the same name as affix mods so we try to pick the correct one
                        var match = GlobalMethods.FindMod(modName, 1, itemTypeName);
                        if (match.Id == 0)
                        {
                            MessageBox.Show("Failed to find a mod with a name of " + modName + "!");
                        }
                        else
                        {
                            //We found a mod, so cache it
                            var mod = new GlobalMethods.Mod { Id = match.Id };
                            modValue = modValue.Replace("%", "");
                            modValue = modValue.Replace("+", "");

                            //Life Regen is a pain because it needs multiplying up to get a value we can use
                            if (modName == "Life Regenerated per second")
                                mod.Value = Convert.ToInt32(Convert.ToDecimal(modValue) * 60);
                            else
                                mod.Value = modValue.Contains("-") ? Convert.ToInt32(modValue.Split(new[] { "-" }, StringSplitOptions.None)[0]) : Convert.ToInt32(modValue);
                            mod.Implicit = match.Class == "<implicit>";
                            mods.Add(mod);

                            //Check to see if this is a mod pair, if it is then match the secondary mod
                            //Mod Pairs always have range values, e.g. 1-5, the minimum value is recorded against the first mod and the maximum value is recorded against the second mod
                            match = GlobalMethods.FindMod(modName, 2, itemTypeName);
                            if (match.Id != 0)
                            {
                                var mod2 = new GlobalMethods.Mod
                                {
                                    Id = match.Id,
                                    Value = modValue.Contains("-") ? Convert.ToInt32(modValue.Split(new[] { "-" }, StringSplitOptions.None)[1]) : 0
                                };
                                mods.Add(mod2);
                            }
                        }
                    }
                }

                //Now we store the mods in the stash as they are useful and we can't guarantee the affixes
                for (int i = 0; i < mods.Count; i++)
                {
                    si.Mod[i].Id = mods[i].Id;
                    si.Mod[i].Value = mods[i].Value;
                }

                //Because some prefixes/ suffixes have two mods we need to find these first and then scoop up whatever is left as single mod "-ixes"
                //We also have the situation of double mods, e.g. Item Quantity is a prefix and a suffix, in these cases we are limited to how much we can determine
                //For Item Quantity there are three ranges, 8-12, 13-18 and 19-24.  As an example, if an item has an Item Quantity roll of 21 then this could be:
                // - Prefix = 8, Suffix = 13 (prefix is rank #3, suffix is rank #2)
                // - Prefix = 11, Suffix = 10 (both are rank #2)
                // - Prefix = 21 (rank #1)
                // - Suffix = 21 (rank #1)
                var prefixes = new List<GlobalMethods.Affix>();
                var suffixes = new List<GlobalMethods.Affix>();

                //Find affixes with two mods
                foreach (var f in GlobalMethods.AffixCache)
                {
                    int matched = 0;
                    var mpMod1 = new GlobalMethods.Mod();
                    var mpMod2 = new GlobalMethods.Mod();
                    foreach (var mp in mods)
                    {
                        if (mp.Id == f.Mod1.Id && mp.Value >= f.Mod1.ValueMin && mp.Value <= f.Mod1.ValueMax && f.Level <= si.ItemLevel)
                        {
                            mpMod1 = mp;
                            matched++;
                        }
                        if (mp.Id == f.Mod2.Id && mp.Value >= f.Mod2.ValueMin && mp.Value <= f.Mod2.ValueMax && f.Level <= si.ItemLevel)
                        {
                            mpMod2 = mp;
                            matched++;
                        }
                    }
                    if (matched == 2)
                    {
                        //We got a hit
                        var affix = f;
                        affix.Mod1.Value = mpMod1.Value;
                        affix.Mod2.Value = mpMod2.Value;
                        if (f.AffixType == "Prefix")
                            prefixes.Add(affix);
                        else
                            suffixes.Add(affix);
                        mods.Remove(mpMod1);
                        mods.Remove(mpMod2);
                    }
                }

                //Find affixes with one mod
                foreach (var f in GlobalMethods.AffixCache)
                {
                    int matched = 0;
                    var mpMod = new GlobalMethods.Mod();
                    foreach (var mp in mods)
                    {
                        if (mp.Id == f.Mod1.Id && mp.Value >= f.Mod1.ValueMin && mp.Value <= f.Mod1.ValueMax && f.Level <= si.ItemLevel)
                        {
                            //We got a hit
                            mpMod = mp;
                            matched++;
                        }
                    }
                    if (matched == 1)
                    {
                        var affix = f;
                        affix.Mod1.Value = mpMod.Value;
                        if (f.AffixType == "Prefix")
                            prefixes.Add(affix);
                        else
                            suffixes.Add(affix);
                        mods.Remove(mpMod);
                    }
                }

                //Is there anything left over?
                //If we only have implict mods left over then this is fine
                bool allAssigned = true;
                if (mods.Count() != 0)
                {
                    for (int i = 0; i < mods.Count; i++)
                    {
                        if (!mods[i].Implicit)
                        {
                            MessageBox.Show("Not all affixes were parsed!");
                            allAssigned = false;
                            break;
                        }
                    }
                }

                //If we have any unassigned mods then the chances are that we have a double-mod affix combined with a single-mod affix
                //For example, we could have the affix for +Accuracy combined with the affix for +Accuracy/ +Evasion
                if (!allAssigned)
                {
                }

                //Pop off the prefixes and suffixes into the StashItem class
                for (int prefix = 0; prefix < Math.Min(prefixes.Count(), 3); prefix++)
                {
                    si.Affix[prefix + 1] = prefixes[prefix];
                }
                for (int suffix = 0; suffix < Math.Min(suffixes.Count(), 3); suffix++)
                {
                    si.Affix[suffix + 4] = suffixes[suffix];
                }

                //If we are in collection mode pop up a window
                DialogResult dr = DialogResult.None;
                if (GlobalMethods.Mode == GlobalMethods.COLLECTION_MODE)
                    //dr = new ItemValue().ShowDialog();
                    dr = new FilterResults().ShowDialog();

                //Stash the item if we are in stash mode or said to stash it from the pop up
                if (GlobalMethods.Mode == GlobalMethods.STASH_MODE || dr == DialogResult.OK)
                {
                    SaveStash();
                    if (Properties.Settings.Default.StashPopUpMode != 0)
                        new PopUpStashed().ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
            }
        }

        private static void SaveStash()
        {
            //Save this item to the database
            string sql = "INSERT INTO Stash(ItemName, BaseItemId, RarityId, Quality, ItemLevel, ReqLevel,";
            sql += " Armour, Evasion, EnergyShield, DamagePhysicalMin, DamagePhysicalMax, DamageElementalMin, DamageElementalMax,";
            sql += " ImplicitMod1Id, ImplicitMod1Value, ImplicitMod2Id, ImplicitMod2Value,";
            sql += " Prefix1Id, Prefix1Mod1Id, Prefix1Mod1Value, Prefix1Mod2Id, Prefix1Mod2Value,";
            sql += " Prefix2Id, Prefix2Mod1Id, Prefix2Mod1Value, Prefix2Mod2Id, Prefix2Mod2Value,";
            sql += " Prefix3Id, Prefix3Mod1Id, Prefix3Mod1Value, Prefix3Mod2Id, Prefix3Mod2Value,";
            sql += " Suffix1Id, Suffix1Mod1Id, Suffix1Mod1Value, Suffix1Mod2Id, Suffix1Mod2Value,";
            sql += " Suffix2Id, Suffix2Mod1Id, Suffix2Mod1Value, Suffix2Mod2Id, Suffix2Mod2Value,";
            sql += " Suffix3Id, Suffix3Mod1Id, Suffix3Mod1Value, Suffix3Mod2Id, Suffix3Mod2Value, OriginalText)";
            sql += " VALUES(";

            //Basic Details
            sql += "'" + si.ItemName + "',";
            sql += (si.BaseItemId == 0 ? "NULL" : si.BaseItemId.ToString()) + ",";
            sql += si.RarityId + ",";
            sql += si.Quality + ",";
            sql += si.ItemLevel + ",";
            sql += si.ReqLevelBase + ",";

            //Armour
            sql += si.Armour + ",";
            sql += si.Evasion + ",";
            sql += si.EnergyShield + ",";

            //Weapons
            sql += "NULL, NULL, NULL, NULL,";

            //Affixes
            for (int key = 0; key < 7; key++)
            {
                //Implict Mods don't have an affix
                if (key != 0)
                    sql += (si.Affix[key].AffixId == 0 ? "NULL" : si.Affix[key].AffixId.ToString()) + ",";
                sql += (si.Affix[key].Mod1.Id == 0 ? "NULL" : si.Affix[key].Mod1.Id.ToString()) + ",";
                sql += (si.Affix[key].Mod1.Value == 0 ? "NULL" : si.Affix[key].Mod1.Value.ToString()) + ",";
                sql += (si.Affix[key].Mod2.Id == 0 ? "NULL" : si.Affix[key].Mod2.Id.ToString()) + ",";
                sql += (si.Affix[key].Mod2.Value == 0 ? "NULL" : si.Affix[key].Mod2.Value.ToString()) + ",";
            }
            sql += "'" + si.OriginalText.Replace("'", "''") + "')";

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
        private static T FindAnyValue<T>(IEnumerable<string> entity, string tag, int pair = -1)
        {
            try
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
                            valueString = valueString.Replace(" (augmented)", "");
                            valueString = valueString.Replace(" (gem)", "");
                            valueString = valueString.Replace(" (unmet)", "");
                            valueString = valueString.Trim();
                            if (valueString.Contains("-") && pair != -1)
                            {
                                //We need to cope with two sorts of ranges
                                //Physical Damage: 20-52
                                //Elemental Damage: 10-32, 11-12, 5-10
                                var valuePair = valueString.Split(',');
                                for (int i = 0; i < valuePair.Count(); i++)
                                {
                                    if (i == 0)
                                        valueString = valuePair[i].Split('-')[pair];
                                    else
                                    {
                                        int value;
                                        if (int.TryParse(valueString, out value))
                                        {
                                            int nextValue;
                                            if (int.TryParse(valuePair[i].Split('-')[pair], out nextValue))
                                            {
                                                valueString = (value + nextValue).ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            return (T)Convert.ChangeType(valueString, typeof(T));
                        }
                    }
                }
                return (T)Convert.ChangeType(0, typeof(T));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return (T)Convert.ChangeType(0, typeof(T));
            }
        }

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
                try
                {
                    NativeMethods.SetParent(Handle, NativeMethods.HWND_MESSAGE);
                    NativeMethods.AddClipboardFormatListener(Handle);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            protected override void WndProc(ref Message m)
            {
                try
                {
                    if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE)
                        OnClipboardUpdate(null);
                    base.WndProc(ref m);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
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
