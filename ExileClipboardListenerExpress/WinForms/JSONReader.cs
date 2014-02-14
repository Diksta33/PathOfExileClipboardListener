using System;
using System.Drawing;
using System.Windows.Forms;
using ExileClipboardListener.JSON;
using ExileClipboardListener.Classes;
using System.Collections.Generic;
using System.Linq;

namespace ExileClipboardListener.WinForms
{
    public partial class JSONReader : Form
    {
        private DataContracts.JSONStash _stash;
        private List<JSON.Character> _characters;
        private int _leagueId;

        public JSONReader()
        {
            InitializeComponent();
        }

        private void Logon_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (POEWeb.Authenticate())
            {
                Logon.Enabled = false;
                GrabCharacters.Enabled = true;
            }
            Cursor.Current = Cursors.Default;
        }

        private void GrabCharacters_Click(object sender, EventArgs e)
        {
            var checkList = new List<string>();
            ItemList.Items.Clear();
            Cursor.Current = Cursors.WaitCursor;
            _characters = POEWeb.GetCharacters();
            Cursor.Current = Cursors.Default;
            foreach (var c in _characters)
            {
                var row = new object[4];
                row[0] = c.Level;
                row[1] = c.Class;
                row[2] = c.Name;
                row[3] = c.League;
                CharacterGrid.Rows.Add(row);
                if (!checkList.Contains(c.League))
                {
                    checkList.Add(c.League);
                    League.Items.Add(c.League);
                }
            }

            //If we have leagues let the stash grabbing commence
            if (League.Items.Count > 0)
            {
                League.SelectedIndex = 0;
                League.Enabled = true;
                GrabStashTabs.Enabled = true;
            }
        }

        private void GrabStash_Click(object sender, EventArgs e)
        {
            ItemList.Items.Clear();
            Cursor.Current = Cursors.WaitCursor;
            _stash = POEWeb.GetStash(League.Text, StashTab.Text);
            Cursor.Current = Cursors.Default;
            foreach (var i in _stash.Items)
                ItemList.Items.Add(i.TypeLine);
            StashAll.Enabled = true;
            AddStash.Enabled = true;
        }

        private void JSONReader_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Ready";
        }

        private void ItemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get the text description
            int index = ItemList.SelectedIndex;
            var i = _stash.Items[index];
            string item = JSON.Parser.ParseItem(i);
            ItemScript.Text = item;

            //Get the icon image
            var size = new Size(32 * i.Width, 32 * i.Height);
            ItemIcon.Size = size;
            ItemIcon.Image = JSON.Parser.GetImage(i.Icon);
        }

        private void StashAll_Click(object sender, EventArgs e)
        {
            //Parse each item in the list, add them one at a time to the stash for the current league
            for (int i = 0; i < ItemList.Items.Count; i++)
            {
                toolStripStatusLabel1.Text = "Parsing " + _stash.Items[i].TypeLine + "... " + ((i * 100) / ItemList.Items.Count).ToString() + "%";
                Application.DoEvents();
                string itemText = JSON.Parser.ParseItem(_stash.Items[i]);
                if (ParseItem.ParseStash(itemText))
                    GlobalMethods.SaveStash(_leagueId);
            }
            toolStripStatusLabel1.Text = "Ready";
        }

        private void AddStash_Click(object sender, EventArgs e)
        {
            if (ItemList.SelectedIndex == -1)
                return;
            string itemText = JSON.Parser.ParseItem(_stash.Items[ItemList.SelectedIndex]);
            if (ParseItem.ParseStash(itemText))
            {
                GlobalMethods.SaveStash(_leagueId);
                MessageBox.Show("Stashed!");
            }
            else
                MessageBox.Show("Failed!");
        }

        private void GrabStashTabs_Click(object sender, EventArgs e)
        {
            //Grab the first stash tab to see how many there are and what they are called
            StashTab.Items.Clear();
            Cursor.Current = Cursors.WaitCursor;
            _stash = POEWeb.GetStash(League.Text, "1");
            Cursor.Current = Cursors.Default;
            for (int tab = 1; tab <= _stash.NumTabs; tab++)
                StashTab.Items.Add(tab.ToString());
            if (StashTab.Items.Count > 0)
                StashTab.SelectedIndex = 0;
            StashTab.Enabled = true;
            StashTab.SelectedIndex = 0;
            GrabStash.Enabled = true;
        }

        private void League_SelectedIndexChanged(object sender, EventArgs e)
        {
            _leagueId = 0;
            while (_leagueId == 0)
            {
                _leagueId = GlobalMethods.GetScalarInt("SELECT LeagueId FROM League WHERE LeagueName = '" + League.Text + "';");
                if (_leagueId == 0)
                {
                    if (MessageBox.Show("That league doesn't exist, do you want to create it?", "Confirm League Create", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        GrabStashTabs.Enabled = false;
                        GrabStash.Enabled = false;
                        return;
                    }
                    GlobalMethods.RunQuery("INSERT INTO League(LeagueName, LeagueParentId) VALUES('" + League.Text + "',NULL);");
                }
            }
        }
    }
}
