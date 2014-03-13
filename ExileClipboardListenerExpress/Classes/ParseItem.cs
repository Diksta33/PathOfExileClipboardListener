using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using bi = ExileClipboardListener.Classes.GlobalMethods.BaseItem;
using si = ExileClipboardListener.Classes.GlobalMethods.StashItem;
using gi = ExileClipboardListener.Classes.GlobalMethods.StashGem;
using ci = ExileClipboardListener.Classes.GlobalMethods.StashCurrency;
using mi = ExileClipboardListener.Classes.GlobalMethods.StashMap;

namespace ExileClipboardListener.Classes
{
    public static class ParseItem
    {
        public static List<GlobalMethods.Affix> Prefixes = new List<GlobalMethods.Affix>();
        public static List<GlobalMethods.Affix> Suffixes = new List<GlobalMethods.Affix>();

        
        public static bool ParseMap(string map)
        {
            GlobalMethods.ClearMap();
            GlobalMethods.StashMap.OriginalText = map;

            //Parse the details
            var entity = map.Split(new[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < entity.Count(); i++)
            {
                entity[i] = entity[i].Replace("\n", "");
                entity[i] = entity[i].Replace("\r", "");
            }

            //The first section is always the basic details
            if (entity.Count() < 2)
            {
                MessageBox.Show("Couldn't split item did you lose the carriage-returns somehow?");
                return false;
            }
            mi.Name = entity[1];
            mi.RarityId = GlobalMethods.GetScalarInt("SELECT RarityId FROM Rarity WHERE RarityName = '" + entity[0].Split(':')[1].Trim() + "';");
            RemoveSection(ref entity);
            mi.MapLevel = FindAnyValue<int>(entity, "Map Level");
            mi.Quality = FindAnyValue<int>(entity, "Quality");
            mi.ItemQuantity = FindAnyValue<int>(entity, "Item Quantity");
            RemoveSection(ref entity);

            //Item Level
            mi.ItemLevel = FindAnyValue<int>(entity, "Itemlevel");
            return true;
        }

        public static bool ParseCurrency(string currency)
        {
            //Parse the details
            var entity = currency.Split(new[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < entity.Count(); i++)
            {
                entity[i] = entity[i].Replace("\n", "");
                entity[i] = entity[i].Replace("\r", "");
            }

            //The first section is always the basic details
            if (entity.Count() < 2)
            {
                MessageBox.Show("Couldn't split item did you lose the carriage-returns somehow?");
                return false;
            }

            ci.Name = entity[1];
            RemoveSection(ref entity);
            ci.StackSize = FindAnyValue<int>(entity, "Stack Size");
            ci.CurrencyItemId = GlobalMethods.CurrencyCache.FirstOrDefault(c => c.Name == ci.Name).CurrencyItemId;
            return true;     
        }


        public static bool ParseGem(string gem)
        {
            GlobalMethods.ClearGem();
            GlobalMethods.StashGem.OriginalText = gem;

            //Parse the details
            var entity = gem.Split(new[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < entity.Count(); i++)
            {
                entity[i] = entity[i].Replace("\n", "");
                entity[i] = entity[i].Replace("\r", "");
            }

            //The first section is always the basic details
            if (entity.Count() < 2)
            {
                MessageBox.Show("Couldn't split item did you lose the carriage-returns somehow?");
                return false;
            }
            gi.GemName = entity[1];
            RemoveSection(ref entity);

            //Next comes the types as a list
            gi.GemType = entity[3];

            //And other properties
            gi.Quality = FindAnyValue<int>(entity, "Quality");
            gi.Level = FindAnyValue<int>(entity, "Level");
            gi.ManaCost = FindAnyValue<int>(entity, "Mana Cost");
            gi.ManaMultiplier = FindAnyValue<int>(entity, "Mana Multiplier");
            gi.ManaReserved = FindAnyValue<int>(entity, "Mana Reserved");
            if (gem.Contains("Experience:"))
                gi.Experience = FindAnyValue<string>(entity, "Experience");
            RemoveSection(ref entity);

            //Then the requirements
            gi.ReqLevel = FindAnyValue<int>(entity, "Level");
            gi.ReqStr = FindAnyValue<int>(entity, "Str");
            gi.ReqDex = FindAnyValue<int>(entity, "Dex");
            gi.ReqInt = FindAnyValue<int>(entity, "Int");

            //Some items have no requirements so be careful
            if (gi.ReqLevel != 0 || gem.Contains("Requirements:"))
                RemoveSection(ref entity);

            //Finally the modifiers
            int mi = 0;
            foreach (var mod in entity)
            {
                if (mod == "--------")
                    break;
                if (mod != "")
                    gi.ExplicitMod[mi++] = mod;
            }
            return true;
        }

        public static bool ParseStash(string item)
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
                //Store the original text then remove junk words
                GlobalMethods.ClearStash();
                si.OriginalText = item;
                item = item.Replace(" (augmented)", "");
                item = item.Replace("Adds ", "");
                item = item.Replace("Has ", "");
                item = item.Replace("Reflects ", "");
                item = item.Replace("Recharges ", "");
                item = item.Replace("Grants ", "");
                item = item.Replace("Removes ", "");

                //Parse the details
                var entity = item.Split(new[] {"\n"}, StringSplitOptions.None);
                for (int i = 0; i < entity.Count(); i++)
                {
                    entity[i] = entity[i].Replace("\n", "");
                    entity[i] = entity[i].Replace("\r", "");
                }

                //The first section is always the basic details
                if (entity.Count() < 2)
                {
                    MessageBox.Show("Couldn't split item did you lose the carriage-returns somehow?");
                    return false;
                }

                si.ItemName = entity[1];
                si.Quality = FindAnyValue<int>(entity, "Quality");
                si.RarityId = GlobalMethods.GetScalarInt("SELECT RarityId FROM Rarity WHERE RarityName = '" + entity[0].Split(':')[1].Trim() + "';");

                //Parse out the base item name
                if (si.RarityId == 0)
                {
                    MessageBox.Show("Catastrophic Failure!");
                    return false;
                }
                string baseName = "";

                //For normal items there is no item name other than the base item name
                if (si.RarityId == 1)
                {
                    baseName = si.ItemName;
                    si.BaseItemId = GlobalMethods.GetScalarInt("SELECT BaseItemId FROM BaseItem WHERE ItemName = '" + si.ItemName.Replace("'", "''") + "';");
                }

                //Magic items have the base item name embedded in the magic name
                if (si.RarityId == 2)
                {
                    string name = si.ItemName;

                    //Remove any suffix
                    name = name.Split(new[] {" of "}, StringSplitOptions.None)[0];
                    si.BaseItemId = GlobalMethods.GetScalarInt("SELECT BaseItemId FROM BaseItem WHERE ItemName = '" + name.Replace("'", "''") + "';");

                    //We might also need to remove the prefix
                    if (si.BaseItemId == 0)
                    {
                        string prefix = name.Split(' ')[0];
                        name = name.Substring(prefix.Length + 1, name.Length - prefix.Length - 1);
                        si.BaseItemId = GlobalMethods.GetScalarInt("SELECT BaseItemId FROM BaseItem WHERE ItemName = '" + name.Replace("'", "''") + "';");
                    }
                    baseName = name;
                }

                //Rare items have a rare name and then the base item name, as do uniques
                if (si.RarityId == 3 || si.RarityId == 4)
                {
                    baseName = entity[2];
                    si.BaseItemId = GlobalMethods.GetScalarInt("SELECT BaseItemId FROM BaseItem WHERE ItemName = '" + entity[2].Replace("'", "''") + "';");
                }
                RemoveSection(ref entity);

                //And we stop here if we didn't get a base item
                if (si.BaseItemId == 0)
                    return false;

                //Sometimes quality comes after the first seperator
                if (si.Quality == 0)
                    si.Quality = FindAnyValue<int>(entity, "Quality");

                //Two-stone rings cause headaches as there are three base items with the same name 
                //and so we need to know the implicit mod before we can be sure we have the right base item
                //TODO: this needs improving
                if (baseName == "Two-Stone Ring")
                {
                    //This is pretty poor, but works for now
                    if (item.Contains("to Cold and Lightning Resistances"))
                        si.BaseItemId = 24;
                    else if (item.Contains("to Fire and Cold Resistances"))
                        si.BaseItemId = 25;
                    else if (item.Contains("to Fire and Lightning Resistances"))
                        si.BaseItemId = 26;
                }

                //If we have a base item then load it once
                GlobalMethods.LoadBaseItem(si.BaseItemId);

                //The second section is the item data
                //Jewellery/ Flasks don't have a section, so we need to determine the item type first from the Base Item
                string itemTypeName = bi.ItemTypeName;
                string itemSubTypeName = bi.ItemSubTypeName;
                si.ItemTypeName = itemTypeName;
                si.ItemSubTypeName = itemSubTypeName;
                if (itemTypeName != "Jewellery" && itemTypeName != "Flask" && itemTypeName != "Quiver")
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

                //If this is a flask give up for now as they have some weird properties
                if (itemTypeName == "Flask")
                    return true;

                //Requirements
                //(but these are on the base item already so we ignore them)
                //var reqStr = Math.Max(FindAnyValue<int>(entity, "Str"), FindValue(entity, "Str (gem)"));
                //var reqInt = Math.Max(FindAnyValue<int>(entity, "Int"), FindValue(entity, "Int (gem)"));
                //var reqDex = Math.Max(FindAnyValue<int>(entity, "Dex"), FindValue(entity, "Dex (gem)"));
                si.ReqLevel = FindAnyValue<int>(entity, "Level");
                si.ReqLevelBase = bi.ReqLevel;

                //Some items have no requirements so be careful
                if (si.ReqLevel != 0 || item.Contains("Requirements:"))
                    RemoveSection(ref entity);

                //Sockets
                //For now just store them
                if ((itemTypeName != "Jewellery" || bi.ItemName == "Unset Ring") && itemTypeName != "Flask" && itemTypeName != "Quiver")
                {
                    si.Sockets = FindAnyValue<string>(entity, "Sockets");
                    RemoveSection(ref entity);
                }

                //Item Level
                si.ItemLevel = FindAnyValue<int>(entity, "Itemlevel");
                RemoveSection(ref entity);

                //Work out the pDPS and eDPS
                if (si.AttacksPerSecond != 0)
                {
                    si.pDPS = Convert.ToDecimal((si.PhysicalDamageMax + si.PhysicalDamageMin)/2)*si.AttacksPerSecond;
                    si.eDPS = Convert.ToDecimal((si.ElementalDamageMax + si.ElementalDamageMin)/2)*si.AttacksPerSecond;
                    si.tDPS = si.pDPS + si.eDPS;
                }

                //Temporarily stop uniques here as they are just too problematic
                if (si.RarityId == 4)
                    return true;

                //Implict modifiers, there may not be any so we are a little careful
                bool seenImplicit = false;

                //Primary
                if (bi.Mod1.Id != 0)
                {
                    var implicitMod = GlobalMethods.LookUpMod(bi.Mod1.Id);

                    //For primary implicit mods there might be a roll in the item script
                    implicitMod.Value = FindMod(entity, implicitMod.RealName, 0);
                    if (implicitMod.Value == 0)
                        implicitMod.Value = bi.Mod1.ValueMin;
                    else
                        seenImplicit = true;

                    //We also want the minimum and maximum values from the base item
                    implicitMod.ValueMin = bi.Mod1.ValueMin;
                    implicitMod.ValueMax = bi.Mod1.ValueMax;
                    si.Affix[0].Mod1 = implicitMod;
                }

                //Secondary
                //If there is a secondary implicit mod then we check this the same way
                if (bi.Mod2.Id != 0)
                {
                    var implicitMod = GlobalMethods.LookUpMod(bi.Mod2.Id);
                    implicitMod.Value = FindMod(entity, implicitMod.RealName, 1);
                    if (implicitMod.Value == 0)
                        implicitMod.Value = bi.Mod2.ValueMin;
                    else
                        seenImplicit = true;

                    //We also want the minimum and maximum values from the base item
                    implicitMod.ValueMin = bi.Mod2.ValueMin;
                    implicitMod.ValueMax = bi.Mod2.ValueMax;
                    si.Affix[0].Mod2 = implicitMod;
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

                        //Two mods causes headaches as they just aren't consistent so we just translate them
                        if (modName == "increased maximum Energy Shield")
                            modName = "increased Energy Shield";
                        if (modName == "increased Global Critical Strike Chance")
                            modName = "increased Critical Strike Chance";

                        //We need to be a little careful, this is the first mod so only pick mods that are 1st in a sequence, e.g. physical damage min/ max
                        //We also need to check if the mod is allowed on this item type
                        //We sometimes have implict mods that have the same name as affix mods so we try to pick the correct one
                        var match = GlobalMethods.FindMod(modName, 1, itemTypeName, itemSubTypeName);

                        //One flask mod needs a kick to put it in the right place
                        if (modName == "reduced Amount Recovered" && si.ItemSubTypeName == "Health Flask")
                        {
                            //Nothing, it already gets put in the right place
                        }
                        if (modName == "reduced Amount Recovered" && si.ItemSubTypeName == "Mana Flask")
                        {
                            match = GlobalMethods.ModCache.FirstOrDefault(m => m.Id == 105);
                        }

                        if (match.Id == 0)
                        {
                            if (si.RarityId != 4)
                                MessageBox.Show("Failed to find a mod with a name of " + modName + "!");
                        }
                        else
                        {
                            //We found a mod, so cache it
                            //var mod = new GlobalMethods.Mod { Id = match.Id };
                            modValue = modValue.Replace("%", "");
                            modValue = modValue.Replace("+", "");

                            //Life Regen is a pain because it needs multiplying up to get a value we can use
                            if (modName == "Life Regenerated per second")
                                match.Value = Convert.ToInt32(Convert.ToDecimal(modValue)*60);
                            else
                            {
                                if (modValue == "No")
                                    match.Value = 0;
                                else
                                    match.Value = modValue.Contains("-") ? Convert.ToInt32(modValue.Split(new[] {"-"}, StringSplitOptions.None)[0]) : Convert.ToInt32(modValue);
                            }
                            mods.Add(match);

                            //Check to see if this is a mod pair, if it is then match the secondary mod
                            //Mod Pairs always have range values, e.g. 1-5, the minimum value is recorded against the first mod and the maximum value is recorded against the second mod
                            match = GlobalMethods.FindMod(modName, 2, itemTypeName, itemSubTypeName);
                            if (match.Id != 0)
                            {
                                match.Value = modValue.Contains("-") ? Convert.ToInt32(modValue.Split(new[] {"-"}, StringSplitOptions.None)[1]) : 0;
                                mods.Add(match);
                            }
                        }
                    }
                }

                //Now we store the mods in the stash as they are useful and we can't guarantee the affixes
                for (int i = 0; i < mods.Count; i++)
                {
                    si.Mod[i] = mods[i];
                }

                //If this item is a legendary or a flask then don't parse the affixes
                if (si.RarityId == 4 || itemTypeName == "Flask")
                    return true;

                //Try to fit the mods to affixes
                FitAffixes(item, ref mods, itemTypeName, itemSubTypeName);

                //Finally, now everything has been parsed we need to pull out some key statistics
                //We ignore % increases for life/ mana at the moment as they are a pain
                si.Life = GetModValues("to maximum Life");
                si.Mana = GetModValues("to maximum Mana");
                si.FireRes = GetModValues("to Fire Resistance");
                si.ColdRes = GetModValues("to Cold Resistance");
                si.LightningRes = GetModValues("to Lightning Resistance");
                si.AllRes = GetModValues("to all Elemental Resistances");
                si.ChaosRes = GetModValues("to Chaos Resistance");
                if (bi.ItemName == "Two-Stone Ring")
                {
                    //Two-Stone Rings are a pain
                    if (item.Contains("to Cold and Lightning Resistances"))
                    {
                        si.ColdRes += si.Affix[0].Mod1.Value;
                        si.LightningRes += si.Affix[0].Mod1.Value;
                    }
                    else if (item.Contains("to Fire and Cold Resistances"))
                    {
                        si.FireRes += si.Affix[0].Mod1.Value;
                        si.ColdRes += si.Affix[0].Mod1.Value;
                    }
                    else if (item.Contains("to Fire and Lightning Resistances"))
                    {
                        si.FireRes += si.Affix[0].Mod1.Value;
                        si.LightningRes += si.Affix[0].Mod1.Value;
                    }
                }

                //Get the subtotal and total
                si.EleRes = si.LightningRes + si.FireRes + si.ColdRes + si.AllRes * 3;
                si.TotalRes = si.EleRes + si.ChaosRes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
            }

            return true;
        }

        private static void FitAffixes(string item, ref List<GlobalMethods.Mod> mods, string itemTypeName, string itemSubTypeName)
        {
            //Because some prefixes/ suffixes have two mods we need to find these first and then scoop up whatever is left as single mod affixes
            //We also have the situation of double mods, e.g. Item Quantity is a prefix and a suffix, in these cases we are limited to how much we can determine
            //For Item Quantity there are three ranges, 8-12, 13-18 and 19-24.  As an example, if an item has an Item Quantity roll of 21 then this could be:
            // - Prefix = 8, Suffix = 13 (prefix is rank #3, suffix is rank #2)
            // - Prefix = 11, Suffix = 10 (both are rank #2)
            // - Prefix = 21 (rank #1)
            // - Suffix = 21 (rank #1)
            //We also have the situation of two mods from three affixes; I think this only happens with Evasion and Stun Recovery?
            Prefixes.Clear();
            Suffixes.Clear();

            //Find affixes with two mods
            MatchDoubleModAffixes(ref mods, itemSubTypeName, true);
            
            //For 2-h weapons we need to run this again, relaxing the restraint on the affix matching the item type
            //This is because 1-h affixes CAN appear on 2-h items
            if (itemSubTypeName.Contains("Two") || itemSubTypeName.Contains("Staff"))
                MatchDoubleModAffixes(ref mods, itemSubTypeName, false);

            //Find affixes with one mod
            MatchSingleModAffixes(ref mods, itemSubTypeName, true);

            //For 2h weapons we need to scan again but allow 1h affixes
            if (itemSubTypeName.Contains("Two") || itemSubTypeName.Contains("Staff"))
                MatchSingleModAffixes(ref mods, itemSubTypeName, false);

            //Now we need to handle IIR, we will be in one of two situations now
            //One is that IIR has been assigned to a prefix but might turn out to be a suffix, we deal with this later
            //The other is that IIR hasn't been assigned at all as the value is too large for the item level as it is BOTH a prefix and a suffix
            //We deal with this here
            var iirMod = mods.FirstOrDefault(m => m.Name == "Base Item Found Rarity +%");
            if (iirMod.Name != null)
            {
                var iir = iirMod.Value;
                mods.Remove(iirMod);
                iirMod.Value = iirMod.Value / 2;
                mods.Add(iirMod);
                var newMod = iirMod;
                newMod.Value = iir - iirMod.Value;
                mods.Add(newMod);

                //See if we can match the two mods now
                MatchSingleModAffixes(ref mods, itemSubTypeName, true, "Base Item Found Rarity +%");
            }

            //Is there anything left over?
            //If we only have implict mods left over then this is fine
            bool allAssigned = true;
            if (mods.Count() != 0)
            {
                foreach (var mod in mods)
                {
                    if (!mod.Implicit)
                    {
                        //Some mods can be implicit and also appear on items
                        //TODO: this needs improving!
                        if ((mod.Id == 17 || mod.Id == 18) && si.BaseItemId == 17)
                            continue;
                        allAssigned = false;
                        break;
                    }
                }
            }

            //This next piece is a total mess (thanks GGG) - priority to refactor this, or to at least relocate it
            //If we have any unassigned mods then the chances are that we have a double-mod affix combined with a single-mod affix
            //For example, we could have the affix for +Accuracy combined with the affix for +Accuracy/ +Evasion
            //We need to guess the "shared" amount for the mod that is common to both
            if (!allAssigned)
            {
                int maxTries = mods.Count * 2 + 1;
                for (int tries = 0; mods.Count > 0 && tries < maxTries; tries++)
                {
                    var mod = mods[0];
                    if (mod.Implicit)
                        mods.Remove(mod);
                    else
                    {
                        //The basic way this works is to pick a mod that wasn't assigned, see if it appears as both a double-mod affix and a single-mod affx
                        //Then see if we have the "other half" of the double-mod affix
                        //Then see if we can "fit" the mods to the affixes in any way that is legal, picking arbitary values for this
                        //This is futher complicated by armour/ hybrid defenses (such a mare)
                        //We need to exclude any armour affixes that are of the wrong type, thankfully these are always singleton affixes
                        GlobalMethods.Affix affix;
                        if (itemTypeName == "Armour")
                        {
                            //Armour only
                            if (si.Armour != 0 && si.Evasion == 0 && si.EnergyShield == 0)
                                affix = GlobalMethods.AffixCache.Where(a => a.Mod1.Class != "Defense" || (a.Mod1.RealName.Contains("Armour") && !a.Mod1.RealName.Contains("and"))).FirstOrDefault(a => (a.Mod1.Id == mod.Id && a.Mod2.Id != 0) || a.Mod2.Id == mod.Id);
                            //Evasion only
                            else if (si.Armour == 0 && si.Evasion != 0 && si.EnergyShield == 0)
                                affix = GlobalMethods.AffixCache.Where(a => a.Mod1.Class != "Defense" || (a.Mod1.RealName.Contains("Evasion") && !a.Mod1.RealName.Contains("and"))).FirstOrDefault(a => (a.Mod1.Id == mod.Id && a.Mod2.Id != 0) || a.Mod2.Id == mod.Id);
                            //Energy Shield only
                            else if (si.Armour == 0 && si.Evasion == 0 && si.EnergyShield != 0)
                                affix = GlobalMethods.AffixCache.Where(a => a.Mod1.Class != "Defense" || (a.Mod1.RealName.Contains("Energy Shield") && !a.Mod1.RealName.Contains("and"))).FirstOrDefault(a => (a.Mod1.Id == mod.Id && a.Mod2.Id != 0) || a.Mod2.Id == mod.Id);
                            //Armour/ Evasion only
                            else if (si.Armour != 0 && si.Evasion != 0 && si.EnergyShield == 0)
                                affix = GlobalMethods.AffixCache.Where(a => a.Mod1.Class != "Defense" || (a.Mod1.RealName.Contains("Armour and Evasion"))).FirstOrDefault(a => (a.Mod1.Id == mod.Id && a.Mod2.Id != 0) || a.Mod2.Id == mod.Id);
                            //Armour/ Energy Shield only
                            else if (si.Armour != 0 && si.Evasion == 0 && si.EnergyShield != 0)
                                affix = GlobalMethods.AffixCache.Where(a => a.Mod1.Class != "Defense" || (a.Mod1.RealName.Contains("Armour and Energy Shield"))).FirstOrDefault(a => (a.Mod1.Id == mod.Id && a.Mod2.Id != 0) || a.Mod2.Id == mod.Id);
                            //Evasion/ Energy Shield only
                            else if (si.Armour == 0 && si.Evasion != 0 && si.EnergyShield != 0)
                                affix = GlobalMethods.AffixCache.Where(a => a.Mod1.Class != "Defense" || (a.Mod1.RealName.Contains("Evasion and Energy Shield"))).FirstOrDefault(a => (a.Mod1.Id == mod.Id && a.Mod2.Id != 0) || a.Mod2.Id == mod.Id);
                            else
                                affix = GlobalMethods.AffixCache.FirstOrDefault(a => (a.Mod1.Id == mod.Id && a.Mod2.Id != 0) || (a.Mod1.Id != 0 && a.Mod2.Id == mod.Id));
                        }
                        else
                            affix = GlobalMethods.AffixCache.FirstOrDefault(a => (a.Mod1.Id == mod.Id && a.Mod2.Id != 0) || (a.Mod1.Id != 0 && a.Mod2.Id == mod.Id));

                        //There is a messy case here for light radius where we get the wrong affix because there are two choices, so we tip the balance back in our favour
                        if (affix.ModCategoryName == "Accuracy")
                        {
                            //Find out what sort of accuracy we have, if it's the "wrong sort" swap it
                            if (si.Mod.FirstOrDefault(m => m.Id == 54).Id == 54)
                            {
                                if (affix.AffixId == 10)
                                    affix = GlobalMethods.AffixCache.Find(a => a.AffixType == "Suffix" && a.AffixId == 221);
                                if (affix.AffixId == 11)
                                    affix = GlobalMethods.AffixCache.Find(a => a.AffixType == "Suffix" && a.AffixId == 222);
                            }
                        }

                        //Now work out the "other" mod that appears on the affix
                        GlobalMethods.Mod modPartner;
                        modPartner.Id = 0;
                        modPartner.Value = 0;
                        string position;
                        if (affix.Mod1.Id == mod.Id)
                        {
                            modPartner = affix.Mod2;
                            position = "Primary";
                        }
                        else if (affix.Mod2.Id == mod.Id)
                        {
                            modPartner = affix.Mod1;
                            position = "Secondary";
                        }
                        else
                        {
                            //MessageBox.Show("Retrofit process went wrong!");
                            continue;
                        }

                        //Do we have the mod partner on this item?
                        int modIndex = -1;
                        for (int i = 0; i < 10; i++)
                        {
                            if (si.Mod[i].Id == modPartner.Id)
                            {
                                modIndex = i;
                                modPartner.Value = si.Mod[i].Value;
                                break;
                            }
                        }

                        //Better check this is all going to plan so far!
                        if (modIndex == -1)
                        {
                            //MessageBox.Show("Retrofit process went badly wrong!");
                            break;
                        }

                        //Now we have two mods, one which was unassigned and one that was possibly assigned incorrectly to a higher level affix than was rolled or was left unassigned
                        //as it was just far too high for any level the item could generate
                        //At this point we take stock and get the two mods in the right order, make sure they are both popped off the list of affixes and then retrofit them
                        GlobalMethods.Mod primaryMod;
                        GlobalMethods.Mod secondaryMod;
                        if (position == "Primary")
                        {
                            primaryMod = mod;
                            secondaryMod = modPartner;
                        }
                        else
                        {
                            primaryMod = modPartner;
                            secondaryMod = mod;
                        }

                        //The next step is to work out the double-mod affix
                        //We look for either mod value to fit and we will adjust the one that doesn't
                        var affixDoubleMod = new GlobalMethods.Affix();
                        var affixFirstSingleMod = new GlobalMethods.Affix();
                        var affixSecondSingleMod = new GlobalMethods.Affix();
                        affixDoubleMod.AffixId = 0;
                        affixFirstSingleMod.AffixId = 0;
                        affixSecondSingleMod.AffixId = 0;

                        //There are multiple levels for each affix, we start with the highest possible for the item and work out way back down until we match or run out of options
                        for (int level = si.ItemLevel; level > 0; level--)
                        {
                            int levelInternal = level;

                            //If we are dealing with spell damage/ mana we have an extra problem, we need to know if this is 1-handed or 2-handed
                            //If this is Defense/ Base Stun Recovery then we could end up with 2 mods producing 3 affixes!  In this case we need to be even more forgiving
                            //For now I will hardcode these values for "simplicty"
                            if (primaryMod.Name == "Spell Damage +%" && si.ItemSubTypeName == "Staff")
                                affixDoubleMod = GlobalMethods.AffixCache.Aggregate((agg, next) => next.ModCategoryName == "Staff Spell Damage and Mana" && next.Mod1.ValueMax > agg.Mod1.ValueMax && next.Mod1.Id == primaryMod.Id && next.Mod2.Id == secondaryMod.Id && ((next.Mod1.ValueMin <= primaryMod.Value && next.Mod1.ValueMax >= primaryMod.Value) || (next.Mod2.ValueMin <= secondaryMod.Value && next.Mod2.ValueMax >= secondaryMod.Value)) && next.Level <= levelInternal ? next : agg);
                            else if (primaryMod.Name == "Spell Damage +%")
                                affixDoubleMod = GlobalMethods.AffixCache.Aggregate((agg, next) => next.ModCategoryName == "One Hand Spell Damage and Mana" && next.Mod1.ValueMax > agg.Mod1.ValueMax && next.Mod1.Id == primaryMod.Id && next.Mod2.Id == secondaryMod.Id && ((next.Mod1.ValueMin <= primaryMod.Value && next.Mod1.ValueMax >= primaryMod.Value) || (next.Mod2.ValueMin <= secondaryMod.Value && next.Mod2.ValueMax >= secondaryMod.Value)) && next.Level <= levelInternal ? next : agg);
                            else if ((primaryMod.Name == "Local Evasion Rating +%" || primaryMod.Name == "Local Physical Damage Reduction Rating +%" || primaryMod.Name == "Local Energy Shield +%" || primaryMod.Name == "Local Armour And Evasion +%" || primaryMod.Name == "Local Evasion And Energy Shield +%" || primaryMod.Name == "Local Armour And Energy Shield +%") && secondaryMod.Name == "Base Stun Recovery +%")
                                affixDoubleMod = GlobalMethods.AffixCache.Aggregate((agg, next) => next.Mod1.ValueMax > agg.Mod1.ValueMax && next.Mod1.Id == primaryMod.Id && next.Mod2.Id == secondaryMod.Id && (next.Mod1.ValueMin <= primaryMod.Value || next.Mod2.ValueMin <= secondaryMod.Value) && next.Level <= levelInternal ? next : agg);
                            else
                                affixDoubleMod = GlobalMethods.AffixCache.Aggregate((agg, next) => next.Mod1.ValueMax > agg.Mod1.ValueMax && next.Mod1.Id == primaryMod.Id && next.Mod2.Id == secondaryMod.Id && ((next.Mod1.ValueMin <= primaryMod.Value && next.Mod1.ValueMax >= primaryMod.Value) || (next.Mod2.ValueMin <= secondaryMod.Value && next.Mod2.ValueMax >= secondaryMod.Value)) && next.Level <= levelInternal ? next : agg);

                            //If we still don't get a match give up
                            if (affixDoubleMod.AffixId == 0)
                            {
                                //MessageBox.Show("Time to call it a day!");
                                break;
                            }

                            //This will leave a bit left over to map to a single-mod affix, maybe even to two at once!
                            var singlePrimaryMod = new GlobalMethods.Mod();
                            var singleSecondaryMod = new GlobalMethods.Mod();
                            int singlePrimaryModValueMin = 0;
                            int singlePrimaryModValueMax = 0;
                            int singleSecondaryModValueMin = 0;
                            int singleSecondaryModValueMax = 0;
                            if (affixDoubleMod.Mod1.ValueMax < primaryMod.Value)
                            {
                                singlePrimaryMod = primaryMod;
                                singlePrimaryModValueMin = primaryMod.Value - affixDoubleMod.Mod1.ValueMax;
                                singlePrimaryModValueMax = primaryMod.Value - affixDoubleMod.Mod1.ValueMin;
                            }
                            else
                                affixDoubleMod.Mod1.Value = primaryMod.Value;
                            if (affixDoubleMod.Mod2.ValueMax < secondaryMod.Value)
                            {
                                singleSecondaryMod = secondaryMod;
                                singleSecondaryModValueMin = secondaryMod.Value - affixDoubleMod.Mod2.ValueMax;
                                singleSecondaryModValueMax = secondaryMod.Value - affixDoubleMod.Mod2.ValueMin;
                            }
                            else
                                affixDoubleMod.Mod2.Value = secondaryMod.Value;

                            //Find the primary mod affix
                            if (singlePrimaryModValueMin != 0)
                            {
                                foreach (var f in GlobalMethods.AffixCache)
                                {
                                    if (f.Mod1.Id == singlePrimaryMod.Id && f.Mod2.Id == 0 && f.Mod1.ValueMin <= singlePrimaryModValueMax && f.Mod1.ValueMax >= singlePrimaryModValueMin && f.Level <= si.ItemLevel)
                                    {
                                        affixFirstSingleMod = f;
                                        break;
                                    }
                                }
                            }

                            //Find the secondary mod affix
                            if (singleSecondaryModValueMin != 0)
                            {
                                foreach (var f in GlobalMethods.AffixCache)
                                {
                                    if (f.Mod1.Id == singleSecondaryMod.Id && f.Mod2.Id == 0 && f.Mod1.ValueMin <= singleSecondaryModValueMax && f.Mod1.ValueMax >= singleSecondaryModValueMin && f.Level <= si.ItemLevel)
                                    {
                                        affixSecondSingleMod = f;
                                        break;
                                    }
                                }
                            }

                            //If either mod wasn't placed go round again
                            if ((singlePrimaryModValueMin != 0 && affixFirstSingleMod.AffixId == 0) || (singleSecondaryModValueMin != 0 && affixSecondSingleMod.AffixId == 0))
                                continue;

                            //If we got a hit then we can exit the loop
                            break;
                        }

                        //We should get at least a double affix out of this
                        if (affixDoubleMod.AffixId == 0)
                        {
                            //MessageBox.Show("Retrofit process went badly wrong!");
                            break;
                        }

                        //Now we have both mods we need to remove any affix that we already collected that reference them
                        for (int i = Prefixes.Count - 1; i >= 0; i--)
                        {
                            if (Prefixes[i].Mod1.Id == primaryMod.Id || Prefixes[i].Mod2.Id == secondaryMod.Id || Prefixes[i].Mod1.Id == secondaryMod.Id)
                                Prefixes.Remove(Prefixes[i]);
                        }
                        for (int i = Suffixes.Count - 1; i >= 0; i--)
                        {
                            if (Suffixes[i].Mod1.Id == primaryMod.Id || Suffixes[i].Mod2.Id == secondaryMod.Id || Suffixes[i].Mod1.Id == secondaryMod.Id)
                                Suffixes.Remove(Suffixes[i]);
                        }

                        //Finally we just need to set the values for the new affixes and add them onto the list
                        if (affixDoubleMod.Mod1.ValueMax < primaryMod.Value)
                        {
                            for (int newRoll = affixDoubleMod.Mod1.ValueMin; newRoll <= affixDoubleMod.Mod1.ValueMax; newRoll++)
                            {
                                int newShare = primaryMod.Value - newRoll;
                                if (newShare >= affixFirstSingleMod.Mod1.ValueMin && newShare <= affixFirstSingleMod.Mod1.ValueMax)
                                {
                                    //Got a match
                                    affixDoubleMod.Mod1.Value = newRoll;
                                    affixFirstSingleMod.Mod1.Value = newShare;
                                    break;
                                }
                            }
                        }
                        if (affixDoubleMod.Mod2.ValueMax < secondaryMod.Value)
                        {
                            for (int newRoll = affixDoubleMod.Mod2.ValueMin; newRoll <= affixDoubleMod.Mod2.ValueMax; newRoll++)
                            {
                                int newShare = secondaryMod.Value - newRoll;
                                if (newShare >= affixSecondSingleMod.Mod1.ValueMin && newShare <= affixSecondSingleMod.Mod1.ValueMax)
                                {
                                    //Got a match
                                    affixDoubleMod.Mod2.Value = newRoll;
                                    affixSecondSingleMod.Mod1.Value = newShare;
                                    break;
                                }
                            }
                        }

                        //Save the double-mod
                        if (affixDoubleMod.AffixType == "Prefix")
                            Prefixes.Add(affixDoubleMod);
                        else
                            Suffixes.Add(affixDoubleMod);

                        //Save the first single mod
                        if (affixFirstSingleMod.AffixType == "Prefix")
                            Prefixes.Add(affixFirstSingleMod);
                        else
                            Suffixes.Add(affixFirstSingleMod);

                        //Save the second single mod
                        if (affixSecondSingleMod.AffixId != 0)
                        {
                            if (affixSecondSingleMod.AffixType == "Prefix")
                                Prefixes.Add(affixSecondSingleMod);
                            else
                                Suffixes.Add(affixSecondSingleMod);
                        }

                        //Now remove any mods we touched from our list
                        for (int i = mods.Count - 1; i >= 0; i--)
                        {
                            if (mods[i].Id == primaryMod.Id || mods[i].Id == secondaryMod.Id)
                                mods.RemoveAt(i);
                        }
                    }
                }
            }

            //Now we need to handle the second case of IIR messing us up
            //This time we might have everything assigned but have ended up with 4 prefixes and 2 suffixes (for example)
            //This will be because IIR will get pushed into a prefix before a suffix and prefix ranges are more generous
            //So we need to check to see if this is the case and move it over
            if (Prefixes.Count > 3)
            {
                var unwantedMod = new GlobalMethods.Mod();
                foreach (var p in Prefixes)
                {
                    if (p.Mod1.Name == "Base Item Found Rarity +%")
                    {
                        unwantedMod = p.Mod1;
                        Prefixes.Remove(p);
                        break;
                    }
                }

                //If we found the IIR mod then kick it into a suffix if possible
                if (unwantedMod.Name != null)
                {
                    //Find affixes with one mod
                    MatchSingleModAffixes(ref mods, itemSubTypeName, true, "Base Item Found Rarity +%", "Suffix");
                }
            }

            //Check one last time to see if there are any mods unparsed
            allAssigned = true;
            if (mods.Count() != 0)
            {
                foreach (var mod in mods)
                {
                    if (!mod.Implicit)
                    {
                        //TODO: this needs improving!
                        if ((mod.Id == 17 || mod.Id == 18) && si.BaseItemId == 17)
                            continue;
                        allAssigned = false;
                        break;
                    }
                }
            }
            if (!allAssigned)
            {
                MessageBox.Show("Not all affixes were parsed, trying to retrofit them failed :(  Here is the item that failed, copy and paste the results as an issue to https://github.com/Diksta33/PathOfExileClipboardListener/issues?state=open.");
                var sv = new WinForms.ScriptViewer { ItemScript = { Text = item } };
                sv.ShowDialog();
            }

            //Pop off the prefixes and suffixes into the StashItem class
            //TODO: check we haven't got too many prefixes/ suffixes
            for (int prefix = 0; prefix < Math.Min(Prefixes.Count(), 3); prefix++)
            {
                si.Affix[prefix + 1] = Prefixes[prefix];
            }
            for (int suffix = 0; suffix < Math.Min(Suffixes.Count(), 3); suffix++)
            {
                si.Affix[suffix + 4] = Suffixes[suffix];
            }
        }

        private static int GetModValues(string modText)
        {
            int retVal = 0;
            foreach (var mod in si.Mod)
                if (mod.Name != null)
                if (mod.RealName.Contains(modText))
                    retVal += mod.Value;
            if(si.Affix[0].Mod1.Name != null)
                    if (si.Affix[0].Mod1.RealName.Contains(modText))
                        retVal += si.Affix[0].Mod1.Value;
            return retVal;
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
                            valueString = valueString.Replace(" (Max)", "");
                            valueString = valueString.Trim();
                            if (valueString.Contains("/"))
                                valueString = valueString.Split('/')[0];
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

        private static int FindMod(IEnumerable<string> entity, string tag, int pair)
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
                    if (valueString.Contains("-"))
                        valueString = valueString.Split('-')[pair];
                    int value;
                    if (int.TryParse(valueString, out value))
                        return value;
                }
            }
            return 0;
        }

        private static void MatchDoubleModAffixes(ref List<GlobalMethods.Mod> mods, string itemSubTypeName, bool checkType)
        {
            foreach (var f in GlobalMethods.AffixCache)
            {
                if (f.AffixId == 0 || f.Mod2.Id == 0)
                    continue;
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
                if (matched == 2 && checkType)
                {
                    //We also need to check that the affix is legal for the item
                    if (itemSubTypeName.Contains("One") && f.ModCategoryName.Contains("Two"))
                        matched = 0;
                    if ((itemSubTypeName.Contains("Two") || itemSubTypeName.Contains("Staff")) && f.ModCategoryName.Contains("One"))
                        matched = 0;
                }
                if (matched == 2)
                {
                    //We got a hit
                    var affix = f;
                    affix.Mod1 = mpMod1;
                    affix.Mod1.ValueMin = f.Mod1.ValueMin;
                    affix.Mod1.ValueMax = f.Mod1.ValueMax;
                    affix.Mod2 = mpMod2;
                    affix.Mod2.ValueMin = f.Mod2.ValueMin;
                    affix.Mod2.ValueMax = f.Mod2.ValueMax;
                    if (f.AffixType == "Prefix")
                        Prefixes.Add(affix);
                    else
                        Suffixes.Add(affix);
                    mods.Remove(mpMod1);
                    mods.Remove(mpMod2);
                }
            }
        }

        private static void MatchSingleModAffixes(ref List<GlobalMethods.Mod> mods, string itemSubTypeName, bool checkType, string modName = null, string affixType = null)
        {
            foreach (var f in GlobalMethods.AffixCache)
            {
                if (f.AffixId == 0 || f.Mod2.Id != 0)
                    continue;
                if (affixType != null && f.AffixType != affixType)
                    continue;

                //If we specified a mod name then make sure we don't already have an affix from this category
                if (modName != null)
                {
                    GlobalMethods.Affix f1 = f;
                    if (f.AffixType == "Prefix" && Prefixes.FirstOrDefault(p => p.ModCategoryId == f1.ModCategoryId).AffixId != 0)
                        continue;
                    if (f.AffixType == "Suffix" && Suffixes.FirstOrDefault(p => p.ModCategoryId == f1.ModCategoryId).AffixId != 0)
                        continue;
                }

                int matched = 0;
                var mpMod = new GlobalMethods.Mod();
                foreach (var mp in mods)
                {
                    if (modName != null && mp.Name != modName)
                        continue;
                    if (mp.Id == f.Mod1.Id && mp.Value >= f.Mod1.ValueMin && mp.Value <= f.Mod1.ValueMax && f.Level <= si.ItemLevel)
                    {
                        //We got a hit
                        mpMod = mp;
                        matched++;
                        break;
                    }
                }
                if (matched == 1 && checkType)
                {
                    //We also need to check that the affix is legal for the item
                    if (itemSubTypeName.Contains("One") && f.ModCategoryName.Contains("Two"))
                        matched = 0;
                    if ((itemSubTypeName.Contains("Two") || itemSubTypeName.Contains("Staff")) && f.ModCategoryName.Contains("One"))
                        matched = 0;
                }
                if (matched == 1)
                {
                    var affix = f;
                    affix.Mod1 = mpMod;
                    affix.Mod1.ValueMin = f.Mod1.ValueMin;
                    affix.Mod1.ValueMax = f.Mod1.ValueMax;
                    if (f.AffixType == "Prefix")
                        Prefixes.Add(affix);
                    else
                        Suffixes.Add(affix);
                    mods.Remove(mpMod);
                }
            }
        }
    }
}
