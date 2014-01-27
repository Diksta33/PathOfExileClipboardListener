using System;
using System.Windows.Forms;
using ExileClipboardListener.Properties;
using ExileClipboardListener.WinForms;

namespace ExileClipboardListener.Classes
{
    class ProcessIcon : IDisposable
    {
        readonly NotifyIcon _ni;

        public ProcessIcon()
        {
            //Instantiate the NotifyIcon object
            _ni = new NotifyIcon();
        }

        public void Display()
        {
            try
            {
                //Put the icon in the system tray and allow it react to mouse clicks.			
                _ni.MouseClick += MouseClick;
                _ni.Icon = Resources.PathOfExile;
                _ni.Text = "Path of Exile Clipboard Listener";
                _ni.Visible = true;

                //Load the default user settings
                LoadSettings();

                //Attach a context menu.
                _ni.ContextMenuStrip = new ContextMenu().Create();

                //Put up the test form
                //new Test().ShowDialog();

                //Start listening
                new ClipboardNotification();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void LoadSettings()
        {
            GlobalMethods.Mode = Properties.Settings.Default.StashMode == 0 ? GlobalMethods.STASH_MODE : GlobalMethods.COLLECTION_MODE;
        }

        public void Dispose()
        {
            //When the application closes, this will remove the icon from the system tray immediately
            _ni.Dispose();
        }

        static void MouseClick(object sender, MouseEventArgs e)
        {
            //Handle mouse button clicks
            if (e.Button == MouseButtons.Left)
            {
                //Show the Stash
                new Stash().ShowDialog();
            }
        }
    }
}

