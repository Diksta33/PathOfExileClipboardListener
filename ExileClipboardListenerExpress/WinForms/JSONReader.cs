using System;
using System.Drawing;
using System.Windows.Forms;
using ExileClipboardListener.JSON;
using ExileClipboardListener.Classes;
using System.Collections.Generic;

namespace ExileClipboardListener.WinForms
{
    public partial class JSONReader : Form
    {
        private DataContracts.JSONStash _stash;
        private DataContracts.JSONInventory _inventory;
        private List<JSON.Character> _characters;
        private int _leagueId;
        private string _stashType;

        public JSONReader()
        {
            InitializeComponent();
        }

        public void SetStatus(string text)
        {
            toolStripStatusLabel1.Text = text;
            Application.DoEvents();
        }

        private void Logon_Click(object sender, EventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.Username == Properties.Settings.Default.PropertyValues["Username"].Property.DefaultValue.ToString())
                {
                    MessageBox.Show("You need to enter your username/ password in the Settings before you can use this feature!");
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                if (POEWeb.Authenticate())
                {
                    Logon.Enabled = false;
                    ItemList.Items.Clear();
                    _characters = POEWeb.GetCharacters();
                    var checkList = new List<string>();
                    foreach (var c in _characters)
                    {
                        if (!checkList.Contains(c.League))
                        {
                            checkList.Add(c.League);
                            League.Items.Add(c.League);
                        }
                    }

                    //If we have leagues let the stash grabbing commence
                    if (League.Items.Count > 0)
                    {
                        League.Enabled = true;
                        StashLeague.Enabled = true;
                        StashAllInventories.Enabled = true;
                        League.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void GrabStash_Click(object sender, EventArgs e)
        {
            try
            {
                GetCurrentLeague();
                ItemList.Items.Clear();
                ItemScript.Text = "";

                //Determine the tab number
                string tabNumber = StashTab.Text.Split('[')[1].Split(']')[0];
                while (tabNumber.Length > 1 && tabNumber.Substring(0, 1) == "0")
                    tabNumber = tabNumber.Substring(1, tabNumber.Length - 1);
                Cursor.Current = Cursors.WaitCursor;
                _stash = POEWeb.GetStash(League.Text, tabNumber);
                Cursor.Current = Cursors.Default;
                foreach (var i in _stash.Items)
                    ItemList.Items.Add(i.TypeLine);
                StashAll.Enabled = true;
                AddStash.Enabled = true;
                ViewItem.Enabled = true;
                _stashType = "Stash";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return;
            }
        }

        private void JSONReader_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Ready";
        }

        private void ItemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get the text description
            int index = ItemList.SelectedIndex;
            DataContracts.JSONItem i = _stashType == "Stash" ? _stash.Items[index] : _inventory.Items[index];
            string item = Parser.ScriptItem(i);
            ItemScript.Text = item;

            //Get the icon image
            var size = new Size(32 * i.Width, 32 * i.Height);
            ItemIcon.Size = size;
            ItemIcon.Image = Parser.GetImage(i.TypeLine, i.Icon, i.Width, i.Height);
        }

        private void StashAll_Click(object sender, EventArgs e)
        {
            try
            {
                //Parse each item in the list, add them one at a time to the stash for the current league
                for (int i = 0; i < ItemList.Items.Count; i++)
                {
                    toolStripStatusLabel1.Text = "Parsing " + (_stashType == "Stash" ? _stash.Items[i].TypeLine : _inventory.Items[i].TypeLine) + "... " + ((i * 100) / ItemList.Items.Count) + "%";
                    Application.DoEvents();
                    var item = _stashType == "Stash" ? _stash.Items[i] : _inventory.Items[i];
                    string itemText = Parser.ScriptItem(item);
                    if (itemText.Contains("Rarity: Gem"))
                    {
                        if (ParseItem.ParseGem(itemText))
                            GlobalMethods.SaveGem(_leagueId);
                    }
                    else if (itemText.Contains("Rarity: Currency"))
                    {
                        if (ParseItem.ParseCurrency(itemText))
                            GlobalMethods.SaveCurrency(_leagueId);
                    }
                    else if (itemText.Contains(" Map"))
                    {
                        if (ParseItem.ParseMap(itemText))
                            GlobalMethods.SaveMap(_leagueId);
                    }
                    else
                    {
                        if (ParseItem.ParseStash(itemText))
                            GlobalMethods.SaveStash(_leagueId);
                        if (item.SocketedItems != null)
                        {
                            foreach (var si in item.SocketedItems)
                            {
                                toolStripStatusLabel1.Text = "Parsing " + si.TypeLine + "... ";
                                Application.DoEvents();
                                itemText = Parser.ScriptItem(si);
                                if (ParseItem.ParseGem(itemText))
                                    GlobalMethods.SaveGem(_leagueId);
                            }
                            toolStripStatusLabel1.Text = "Ready";
                        }
                    }
                }
                toolStripStatusLabel1.Text = "Ready";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return;
            }
        }

        private void AddStash_Click(object sender, EventArgs e)
        {
            try
            {
                if (ItemList.SelectedIndex == -1)
                    return;
                var i = _stashType == "Stash" ? _stash.Items[ItemList.SelectedIndex] : _inventory.Items[ItemList.SelectedIndex];
                string itemText = Parser.ScriptItem(i);

                //Determine the type
                if (itemText.Contains("Rarity: Gem"))
                {
                    if (ParseItem.ParseGem(itemText))
                    {
                        GlobalMethods.SaveGem(_leagueId);
                        MessageBox.Show("Stashed!");
                        return;
                    }
                }
                else if (itemText.Contains("Rarity: Currency"))
                {
                    if (ParseItem.ParseCurrency(itemText))
                    {
                        GlobalMethods.SaveCurrency(_leagueId);
                        MessageBox.Show("Stashed!");
                        return;
                    }
                }
                else if (itemText.Contains(" Map"))
                {
                    if (ParseItem.ParseMap(itemText))
                    {
                        GlobalMethods.SaveMap(_leagueId);
                        MessageBox.Show("Stashed!");
                        return;
                    }
                }
                else
                {
                    if (ParseItem.ParseStash(itemText))
                    {
                        GlobalMethods.SaveStash(_leagueId);
                        if (i.SocketedItems != null)
                        {
                            foreach (var si in i.SocketedItems)
                            {
                                toolStripStatusLabel1.Text = "Parsing " + si.TypeLine + "... ";
                                Application.DoEvents();
                                itemText = Parser.ScriptItem(si);
                                if (ParseItem.ParseGem(itemText))
                                    GlobalMethods.SaveGem(_leagueId);
                            }
                            toolStripStatusLabel1.Text = "Ready";
                        }
                        MessageBox.Show("Stashed!");
                        return;
                    }
                }
                MessageBox.Show("Failed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return;
            }
        }

        private void GrabStashTabs()
        {
            //Grab the first stash tab to see how many there are and what they are called
            StashTab.Items.Clear();
            Cursor.Current = Cursors.WaitCursor;
            _stash = POEWeb.GetStash(League.Text, "1");
            Cursor.Current = Cursors.Default;
            for (int tab = 0; tab < _stash.NumTabs; tab++)
                StashTab.Items.Add("Tab [" + tab.ToString("000") + "] " + _stash.Tabs[tab].n);
            if (StashTab.Items.Count > 0)
                StashTab.SelectedIndex = 0;
            StashTab.Enabled = true;
            StashTab.SelectedIndex = 0;
            GrabStash.Enabled = true;
        }

        private void GetCurrentLeague()
        {
            _leagueId = 0;
            while (_leagueId == 0)
            {
                _leagueId = GlobalMethods.GetScalarInt("SELECT LeagueId FROM League WHERE LeagueName = '" + League.Text + "';");
                if (_leagueId == 0)
                {
                    if (MessageBox.Show("The league: [" + League.Text + "] doesn't exist, do you want to create it?", "Confirm League Create", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        GrabStash.Enabled = false;
                        return;
                    }
                    GlobalMethods.RunQuery("INSERT INTO League(LeagueName, LeagueParentId) VALUES('" + League.Text + "',NULL);");
                }
            }
        }

        private void GrabInventory_Click(object sender, EventArgs e)
        {
            try
            {
                GetCurrentLeague();
                ItemList.Items.Clear();
                ItemScript.Text = "";
                if (CharacterGrid.CurrentRow == null)
                    return;
                string character = CharacterGrid.CurrentRow.Cells[CharacterGridNameColumn.Index].Value.ToString();
                Cursor.Current = Cursors.WaitCursor;
                _inventory = POEWeb.GetInventory(character);
                Cursor.Current = Cursors.Default;
                foreach (var i in _inventory.Items)
                    ItemList.Items.Add(i.TypeLine);
                StashAll.Enabled = true;
                AddStash.Enabled = true;
                ViewItem.Enabled = true;
                _stashType = "Inventory";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return;
            }
        }

        private void CharacterGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (CharacterGrid.CurrentRow == null)
                return;
            //League.Text = CharacterGrid.CurrentRow.Cells[CharacterGridLeagueColumn.Index].Value.ToString();
        }

        private void ViewItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ItemList.SelectedIndex == -1)
                    return;
                string itemText = Parser.ScriptItem(_stashType == "Stash" ? _stash.Items[ItemList.SelectedIndex] : _inventory.Items[ItemList.SelectedIndex]);
                if (!itemText.Contains("Rarity:"))
                    return;
                if (itemText.Contains("Rarity: Gem"))
                {
                }
                else if (itemText.Contains("Rarity: Currency"))
                {
                }
                else if (itemText.Contains(" Map"))
                {
                }
                else
                {
                    if (ParseItem.ParseStash(itemText))
                    {
                        var dr = GlobalMethods.Mode == GlobalMethods.COLLECTION_MODE ? new ItemInformation().ShowDialog() : new CompactInformation().ShowDialog();

                        //Stash the item if we are said to stash it from the pop up
                        if (dr == DialogResult.OK)
                        {
                            GlobalMethods.SaveStash(GlobalMethods.LeagueId);
                            if (Properties.Settings.Default.StashPopUpMode != 0)
                                new PopUpStashed().ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return;
            }
        }

        private void League_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetCurrentLeague();
                CharacterGrid.Rows.Clear();
                foreach (var c in _characters)
                {
                    if (c.League == League.Text)
                    {
                        var row = new object[4];
                        row[0] = c.Level;
                        row[1] = c.Class;
                        row[2] = c.Name;
                        row[3] = c.League;
                        CharacterGrid.Rows.Add(row);
                    }
                }
                if (CharacterGrid.Rows.Count > 0)
                {
                    CharacterGrid_SelectionChanged(null, null);
                    GrabInventory.Enabled = true;
                    QuickUpdate.Enabled = true;
                    QuickUpdateSettings.Enabled = true;
                }
                GrabStashTabs();
                ItemList.Items.Clear();
                ItemScript.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return;
            }
        }

        private void StashLeague_Click(object sender, EventArgs e)
        {
            try
            {
                ItemList.Items.Clear();
                ItemScript.Text = "";
                Cursor.Current = Cursors.WaitCursor;
                GetCurrentLeague();

                //Stash all character inventories
                for (int row = 0; row < CharacterGrid.Rows.Count; row++)
                {
                    string character = CharacterGrid.Rows[row].Cells[CharacterGridNameColumn.Index].Value.ToString();
                    _inventory = POEWeb.GetInventory(character);
                    foreach (var i in _inventory.Items)
                    {
                        toolStripStatusLabel1.Text = "Parsing " + i.TypeLine + "... ";
                        Application.DoEvents();
                        string itemText = Parser.ScriptItem(i);
                        if (itemText.Contains("Rarity: Gem"))
                        {
                            if (ParseItem.ParseGem(itemText))
                                GlobalMethods.SaveGem(_leagueId);
                        }
                        else if (itemText.Contains("Rarity: Currency"))
                        {
                            if (ParseItem.ParseCurrency(itemText))
                                GlobalMethods.SaveCurrency(_leagueId);
                        }
                        else if (itemText.Contains(" Map"))
                        {
                            if (ParseItem.ParseMap(itemText))
                                GlobalMethods.SaveMap(_leagueId);
                        }
                        else
                        {
                            if (ParseItem.ParseStash(itemText))
                                GlobalMethods.SaveStash(_leagueId);
                        }
                        if (i.SocketedItems != null)
                        {
                            foreach (var si in i.SocketedItems)
                            {
                                toolStripStatusLabel1.Text = "Parsing " + si.TypeLine + "... ";
                                Application.DoEvents();
                                itemText = Parser.ScriptItem(si);
                                if (ParseItem.ParseGem(itemText))
                                    GlobalMethods.SaveGem(_leagueId);
                            }
                        }
                    }
                }

                //Stash all stash tabs
                for (int tab = 0; tab < _stash.NumTabs; tab++)
                {
                    string tabNumber = tab.ToString();
                    _stash = POEWeb.GetStash(League.Text, tabNumber);
                    foreach (var i in _stash.Items)
                    {
                        toolStripStatusLabel1.Text = "Parsing " + i.TypeLine + "... ";
                        Application.DoEvents();
                        string itemText = Parser.ScriptItem(i);
                        if (itemText.Contains("Rarity: Gem"))
                        {
                            if (ParseItem.ParseGem(itemText))
                                GlobalMethods.SaveGem(_leagueId);
                        }
                        else if (itemText.Contains("Rarity: Currency"))
                        {
                            if (ParseItem.ParseCurrency(itemText))
                                GlobalMethods.SaveCurrency(_leagueId);
                        }
                        else if (itemText.Contains(" Map"))
                        {
                            if (ParseItem.ParseMap(itemText))
                                GlobalMethods.SaveMap(_leagueId);
                        }
                        else
                        {
                            if (ParseItem.ParseStash(itemText))
                                GlobalMethods.SaveStash(_leagueId);
                        }
                    }
                }
                toolStripStatusLabel1.Text = "Ready";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void StashAllInventories_Click(object sender, EventArgs e)
        {
            try
            {
                ItemList.Items.Clear();
                ItemScript.Text = "";
                Cursor.Current = Cursors.WaitCursor;
                GetCurrentLeague();

                //Stash all character inventories
                for (int row = 0; row < CharacterGrid.Rows.Count; row++)
                {
                    string character = CharacterGrid.Rows[row].Cells[CharacterGridNameColumn.Index].Value.ToString();
                    _inventory = POEWeb.GetInventory(character);
                    foreach (var i in _inventory.Items)
                    {
                        toolStripStatusLabel1.Text = "Parsing " + i.TypeLine + "... ";
                        Application.DoEvents();
                        string itemText = Parser.ScriptItem(i);
                        if (itemText.Contains("Rarity: Gem"))
                        {
                            if (ParseItem.ParseGem(itemText))
                                GlobalMethods.SaveGem(_leagueId);
                        }
                        else if (itemText.Contains("Rarity: Currency"))
                        {
                            if (ParseItem.ParseCurrency(itemText))
                                GlobalMethods.SaveCurrency(_leagueId);
                        }
                        else if (itemText.Contains(" Map"))
                        {
                            if (ParseItem.ParseMap(itemText))
                                GlobalMethods.SaveMap(_leagueId);
                        }
                        else
                        {
                            if (ParseItem.ParseStash(itemText))
                                GlobalMethods.SaveStash(_leagueId);
                        }
                        if (i.SocketedItems != null)
                        {
                            foreach (var si in i.SocketedItems)
                            {
                                toolStripStatusLabel1.Text = "Parsing " + si.TypeLine + "... ";
                                Application.DoEvents();
                                itemText = Parser.ScriptItem(si);
                                if (ParseItem.ParseGem(itemText))
                                    GlobalMethods.SaveGem(_leagueId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception: " + ex.Message);
                return;
            }
            finally
            {
                toolStripStatusLabel1.Text = "Ready";
                Cursor.Current = Cursors.Default;
            }
        }

        private void QuickUpdateSettings_Click(object sender, EventArgs e)
        {
            new QuickUpdateSettings().ShowDialog();
        }

        private void QuickUpdate_Click(object sender, EventArgs e)
        {
            //Just update currency and maps
            int stackItems = 0;
            foreach (object l in League.Items)
            {
                string leagueName = l.ToString();
                int internalLeagueId = GlobalMethods.GetScalarInt("SELECT LeagueId FROM League WHERE LeagueName = '" + leagueName + "';");
                if (Properties.Settings.Default.QuickLeagues == GlobalMethods.DEFAULT_LEAGUE && Properties.Settings.Default.DefaultLeagueId != internalLeagueId)
                    continue;

                //First clear them down
                GlobalMethods.RunQuery("DELETE FROM CurrencyStash WHERE LeagueId = " + internalLeagueId + ";");
                GlobalMethods.RunQuery("DELETE FROM GemStash WHERE LeagueId = " + internalLeagueId + ";");
                GlobalMethods.RunQuery("DELETE FROM MapStash WHERE LeagueId = " + internalLeagueId + ";");

                //Look in inventories first
                foreach (var c in _characters)
                {
                    if (c.League == leagueName)
                    {
                        _inventory = POEWeb.GetInventory(c.Name);
                        foreach (var i in _inventory.Items)
                        {
                            toolStripStatusLabel1.Text = "Parsing " + i.TypeLine + "... ";
                            Application.DoEvents();
                            string itemText = Parser.ScriptItem(i);
                            if (itemText.Contains("Rarity: Gem"))
                            {
                                if (ParseItem.ParseGem(itemText))
                                    GlobalMethods.SaveGem(_leagueId);
                                stackItems++;
                            }
                            else if (itemText.Contains("Rarity: Currency"))
                            {
                                if (ParseItem.ParseCurrency(itemText))
                                    GlobalMethods.SaveCurrency(_leagueId);
                                stackItems++;
                            }
                            else if (itemText.Contains(" Map"))
                            {
                                if (ParseItem.ParseMap(itemText))
                                    GlobalMethods.SaveMap(_leagueId);
                                stackItems++;
                            }
                            if (i.SocketedItems != null)
                            {
                                foreach (var si in i.SocketedItems)
                                {
                                    toolStripStatusLabel1.Text = "Parsing " + si.TypeLine + "... ";
                                    Application.DoEvents();
                                    itemText = Parser.ScriptItem(si);
                                    if (ParseItem.ParseGem(itemText))
                                        GlobalMethods.SaveGem(_leagueId);
                                    stackItems++;
                                }
                            }
                        }
                    }
                }

                //Then check stash tabs, but only if they match the names on the list
                Cursor.Current = Cursors.WaitCursor;
                _stash = POEWeb.GetStash(leagueName, "1");
                Cursor.Current = Cursors.Default;
                for (int tab = 0; tab < _stash.NumTabs; tab++)
                {
                    if (_stash.Tabs[tab].n == Properties.Settings.Default.CurrencyTab1 || _stash.Tabs[tab].n == Properties.Settings.Default.CurrencyTab2 || _stash.Tabs[tab].n == Properties.Settings.Default.CurrencyTab3
                        || _stash.Tabs[tab].n == Properties.Settings.Default.MapTab1 || _stash.Tabs[tab].n == Properties.Settings.Default.MapTab2 || _stash.Tabs[tab].n == Properties.Settings.Default.MapTab3
                        || _stash.Tabs[tab].n == Properties.Settings.Default.GemTab1 || _stash.Tabs[tab].n == Properties.Settings.Default.GemTab2 || _stash.Tabs[tab].n == Properties.Settings.Default.GemTab3)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        _stash = POEWeb.GetStash(League.Text, _stash.Tabs[tab].i.ToString());
                        Cursor.Current = Cursors.Default;
                        foreach (var i in _stash.Items)
                        {
                            toolStripStatusLabel1.Text = "Parsing " + i.TypeLine + "... ";
                            Application.DoEvents();
                            string itemText = Parser.ScriptItem(i);
                            if (itemText.Contains("Rarity: Gem"))
                            {
                                if (ParseItem.ParseGem(itemText))
                                    GlobalMethods.SaveGem(_leagueId);
                                stackItems++;
                            }
                            else if (itemText.Contains("Rarity: Currency"))
                            {
                                if (ParseItem.ParseCurrency(itemText))
                                    GlobalMethods.SaveCurrency(_leagueId);
                                stackItems++;
                            }
                            else if (itemText.Contains(" Map"))
                            {
                                if (ParseItem.ParseMap(itemText))
                                    GlobalMethods.SaveMap(_leagueId);
                                stackItems++;
                            }
                            if (i.SocketedItems != null)
                            {
                                foreach (var si in i.SocketedItems)
                                {
                                    toolStripStatusLabel1.Text = "Parsing " + si.TypeLine + "... ";
                                    Application.DoEvents();
                                    itemText = Parser.ScriptItem(si);
                                    if (ParseItem.ParseGem(itemText))
                                        GlobalMethods.SaveGem(_leagueId);
                                    stackItems++;
                                }
                            }
                        }
                    }
                }
            }

            //Tell them we finished
            MessageBox.Show("Updated " + stackItems.ToString("#,##0") + " stacks");
        }
    }
}
