using System;
using System.Windows.Forms;
using ExileClipboardListener.WinForms;

namespace ExileClipboardListener.Classes
{
    public class ContextMenu
    {
        private bool _aboutLoaded;
        private bool _characterLoaded;
        private bool _leagueLoaded;
        private bool _stashLoaded;
        private bool _filtersLoaded;
        private bool _settingsLoaded;
        private bool _downloadStashLoaded;

        public ContextMenuStrip Menu = new ContextMenuStrip();

        public ContextMenuStrip Create()
        {
            //Add the default menu options
            //Stash Mode
            var item = new ToolStripMenuItem {Text = "Stash Mode"};
            item.Click += StashModeClick;
            item.Checked = GlobalMethods.Mode == GlobalMethods.STASH_MODE;
            item.Tag = "Stash";
            Menu.Items.Add(item);

            //Collection Mode
            item = new ToolStripMenuItem {Text = "Collection Mode"};
            item.Click += CollectionModeClick;
            item.Checked = GlobalMethods.Mode == GlobalMethods.COLLECTION_MODE;
            item.Tag = "Collection";
            Menu.Items.Add(item);

            //Separator
            var sep = new ToolStripSeparator();
            Menu.Items.Add(sep);

            //Character Manager
            item = new ToolStripMenuItem { Text = "Character Manager" };
            item.Click += CharacterClick;
            Menu.Items.Add(item);

            //League Manager
            item = new ToolStripMenuItem { Text = "League Manager" };
            item.Click += LeagueClick;
            Menu.Items.Add(item);

            //Separator
            sep = new ToolStripSeparator();
            Menu.Items.Add(sep);

            //Download Stash
            item = new ToolStripMenuItem { Text = "Download Stash" };
            item.Click += DownloadStashClick;
            Menu.Items.Add(item);

            //View Stash
            item = new ToolStripMenuItem { Text = "View Stash" };
            item.Click += ViewStashClick;
            Menu.Items.Add(item);

            //Separator
            sep = new ToolStripSeparator();
            Menu.Items.Add(sep);

            //Filters
            item = new ToolStripMenuItem {Text = "Filters"};
            item.Click += FiltersClick;
            Menu.Items.Add(item);

            //Settings
            item = new ToolStripMenuItem { Text = "Settings" };
            item.Click += SettingsClick;
            Menu.Items.Add(item);

            //About
            item = new ToolStripMenuItem {Text = "About"};
            item.Click += AboutClick;
            Menu.Items.Add(item);

            //Exit
            item = new ToolStripMenuItem {Text = "Exit"};
            item.Click += ExitClick;
            Menu.Items.Add(item);

            return Menu;
        }

        private void AboutClick(object sender, EventArgs e)
        {
            if (!_aboutLoaded)
            {
                _aboutLoaded = true;
                new AboutBox().ShowDialog();
                _aboutLoaded = false;
            }
        }

        private void CharacterClick(object sender, EventArgs e)
        {
            if (!_characterLoaded)
            {
                _characterLoaded = true;
                new Character().ShowDialog();
                _characterLoaded = false;
            }
        }

        private void LeagueClick(object sender, EventArgs e)
        {
            if (!_leagueLoaded)
            {
                _leagueLoaded = true;
                new League().ShowDialog();
                _leagueLoaded = false;
            }
        }

        private void ViewStashClick(object sender, EventArgs e)
        {
            if (!_stashLoaded)
            {
                _stashLoaded = true;
                new StashViewer().ShowDialog();
                _stashLoaded = false;
            }
        }

        private void StashModeClick(object sender, EventArgs e)
        {
            GlobalMethods.Mode = GlobalMethods.STASH_MODE;
            ((ToolStripMenuItem)Menu.Items[0]).Checked = true;
            ((ToolStripMenuItem)Menu.Items[1]).Checked = false;
        }

        private void CollectionModeClick(object sender, EventArgs e)
        {
            GlobalMethods.Mode = GlobalMethods.COLLECTION_MODE;
            ((ToolStripMenuItem)Menu.Items[0]).Checked = false;
            ((ToolStripMenuItem)Menu.Items[1]).Checked = true;
        }

        private void FiltersClick(object sender, EventArgs e)
        {
            if (!_filtersLoaded)
            {
                _filtersLoaded = true;
                new Filters().ShowDialog();
                _filtersLoaded = false;
            }
        }

        private void SettingsClick(object sender, EventArgs e)
        {
            if (!_settingsLoaded)
            {
                _settingsLoaded = true;
                new Settings().ShowDialog();
                _settingsLoaded = false;
            }
        }

        private void DownloadStashClick(object sender, EventArgs e)
        {
            if (!_downloadStashLoaded)
            {
                _downloadStashLoaded = true;
                new JSONReader().ShowDialog();
                _downloadStashLoaded = false;
            }
        }

        private static void ExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
