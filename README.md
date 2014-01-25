PathOfExileClipboardListener
============================

Clipboard Listener and Stash Manager for Path of Exile

Written in Visual Studio 10 C#.Net targetting .NET 4.0 Framework.

Uses System.Data.SQLite ADO.NET library for SQLite (the 32-bit version for VS10).

Note that to go back to the original Item Viewer change the Form that is loaded in the Listener Class.  The original screen is commented out, i.e.:

            if (GlobalMethods.Mode == GlobalMethods.COLLECTION_MODE)
                //dr = new ItemValue().ShowDialog();
                dr = new FilterResults().ShowDialog();
