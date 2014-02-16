using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Drawing;
using ExileClipboardListener.Classes;
using bi = ExileClipboardListener.Classes.GlobalMethods.BaseItem;

namespace ExileClipboardListener.JSON
{
    public static class Parser
    {
        public static string ParseItem(DataContracts.JSONItem i)
        {

            string rarity = "Normal";
            if (i.FrameType == 1)
                rarity = "Magic";
            else if (i.FrameType == 2)
                rarity = "Rare";
            else if (i.FrameType == 3)
                rarity = "Unique";
            string item = "Rarity: " + rarity + Environment.NewLine;
            if ((i.Name ?? "") != "")
                item += i.Name + Environment.NewLine;
            if (i.TypeLine != null)
                item += i.TypeLine + Environment.NewLine;

            //Properties
            if (i.Properties != null)
            {
                item += "--------" + Environment.NewLine;
                foreach (var p in i.Properties)
                    item += p.Name + (p.Values.Count != 0 && p.Name != "" ? ": " : "") + (p.Values.Count != 0 ? ((object[])p.Values[0])[0] : "") + Environment.NewLine;
            }

            //Requirements
            if (i.Requirements != null)
            {
                item += "--------" + Environment.NewLine;
                item += "Requirements:" + Environment.NewLine;
                foreach (var r in i.Requirements)
                    item += r.Name + ": " + ((object[])r.Value[0])[0] + Environment.NewLine;
            }

            //Sockets
            if (i.Sockets.Count > 0)
            {
                string sockets = "";
                for (int group = 0; group < 6; group++)
                {
                    bool first = true;
                    foreach (DataContracts.JSONSocket t in i.Sockets)
                    {
                        if (t.Group != group)
                            continue;
                        if (t.Attribute == "S")
                            sockets += (first ? (group == 0 ? "" : " ") : "-") + "R";
                        if (t.Attribute == "D")
                            sockets += (first ? (group == 0 ? "" : " ") : "-") + "G";
                        if (t.Attribute == "I")
                            sockets += (first ? (group == 0 ? "" : " ") : "-") + "B";
                        first = false;
                    }
                }
                item += "--------" + Environment.NewLine;
                item += sockets + Environment.NewLine;
            }

            //Socketed Items contains the gems that are currently socketed in this item (if there are any)
            //InventoryId is the name of the stash tab (internal), e.g. "Stash29" (which we already know)
            //Height and Width are the number of squares the item takes up in the stash, e.g. 2x2
            //League is the name of the league the stash belongs to (which we already know)
            //Icon is the URL for the item image
            //Identified is a bool that does what it says
            //Verified is something that I know nothing about, many times it comes up false which is a bit odd?

            //Item Level
            //I don't think we can get the item level from GGG, the best I can do here is to work out the minimum level the item needs to be by looking at:
            // - the requirements, e.g. Req Level = 20 so the item level must be >= 20
            // - the affixes, e.g. if the level 20 item has a level 30 affix then the item level must be >= 30
            item += "--------" + Environment.NewLine;
            item += "Itemlevel: 100" + Environment.NewLine;

            //Implicit Mods/ Flavour
            if (i.FlavourText != null || i.ImplicitMods != null)
            {
                item += "--------" + Environment.NewLine;
                if (i.FlavourText != null)
                    foreach (string flavourText in i.FlavourText)
                        item += flavourText + Environment.NewLine;
                if (i.ImplicitMods != null)
                    foreach (var im in i.ImplicitMods)
                        item += im + Environment.NewLine;
            }

            //Explicit Mods
            if (i.ExplicitMods != null)
            {
                item += "--------" + Environment.NewLine;
                foreach (var em in i.ExplicitMods)
                    item += em + Environment.NewLine;
            }

            return item;
        }

        public static Bitmap GetImage(string baseItem, string iconURL, int width, int height)
        {
            //Remove any suffix
            baseItem = baseItem.Split(new[] { " of " }, StringSplitOptions.None)[0];

            //Determine the base item
            int baseItemId;
            baseItemId = GlobalMethods.GetScalarInt("SELECT BaseItemId FROM BaseItem WHERE ItemName = '" + baseItem.Replace("'", "''") + "';");

            //We might also need to remove the prefix
            if (baseItemId == 0)
            {
                string prefix = baseItem.Split(' ')[0];
                if (baseItem.Length > prefix.Length)
                {
                    baseItem = baseItem.Substring(prefix.Length + 1, baseItem.Length - prefix.Length - 1);
                    baseItemId = GlobalMethods.GetScalarInt("SELECT BaseItemId FROM BaseItem WHERE ItemName = '" + baseItem.Replace("'", "''") + "';");
                }
            }

            //See if we already have the icon
            if (baseItemId != 0)
            {
                GlobalMethods.LoadBaseItem(baseItemId);
                if (bi.Icon != null)
                {
                    return new Bitmap(bi.Icon);
                }
            }

            //We try to load the item image
            var request = WebRequest.Create(iconURL);
            var resp = request.GetResponse();
            var respStream = resp.GetResponseStream();
            var bmp = new Bitmap(respStream);
            respStream.Dispose();

            //If we have identified the base item and it didn't have an image then set it now
            if (baseItemId != 0)
                GlobalMethods.SetBaseItemIcon(baseItemId, bmp, width, height);

            return bmp;
        }
    }
}
