using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ExileClipboardListener.WinForms;
using si = ExileClipboardListener.Classes.GlobalMethods.StashItem;

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

                //If we get here then something is going to end up being messed up if we already have the pop up form open
                _loaded = true;
                if (item.Contains("Rarity: Gem"))
                {
                    if (ParseItem.ParseGem(item))
                    {
                        //If we are in collection mode pop up a window
                        //DialogResult dr = DialogResult.None;
                        //if (GlobalMethods.Mode == GlobalMethods.COLLECTION_MODE)
                        //    dr = new ItemInformation().ShowDialog();

                        //Stash the item if we are in stash mode or said to stash it from the pop up
                        if (GlobalMethods.Mode == GlobalMethods.STASH_MODE)// || dr == DialogResult.OK)
                        {
                            GlobalMethods.SaveGem(GlobalMethods.LeagueId);
                            if (Properties.Settings.Default.StashPopUpMode != 0)
                                new PopUpStashed().ShowDialog();
                        }
                    }
                }
                else if (ParseItem.ParseStash(item))
                {
                    //If we are in collection mode pop up a window
                    DialogResult dr = DialogResult.None;
                    if (GlobalMethods.Mode == GlobalMethods.COLLECTION_MODE)
                        dr = new ItemInformation().ShowDialog();

                    //Stash the item if we are in stash mode or said to stash it from the pop up
                    if (GlobalMethods.Mode == GlobalMethods.STASH_MODE || dr == DialogResult.OK)
                    {
                        GlobalMethods.SaveStash(GlobalMethods.LeagueId);
                        if (Properties.Settings.Default.StashPopUpMode != 0)
                            new PopUpStashed().ShowDialog();
                    }
                }

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
