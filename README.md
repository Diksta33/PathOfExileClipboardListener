PathOfExileClipboardListener
============================

Clipboard Listener and Stash Manager for Path of Exile

Written in Visual Studio 10 C#.Net targetting .NET 4.0 Framework.

Uses System.Data.SQLite ADO.NET library for SQLite (the 32-bit version for VS10).

============================
Quick Summary:

- start the application and it will run in the system tray;
- right-click the icon and choose Settings to set options;
- right-click the icon and choose other options to configure Leagues, etc.;
- left-click the icon to launch the Stash Viewer;
- with the app. running hover an item in Path of Exile and hit Ctrl-C to parse the item;
- in Stash Mode the item is stashed;
- in Collection Mode a window pops up with information about the item, you can stash the item from here;
- use League Manager to set up leagues and allocate one as default;
- your stashed items go into the league you have as default (or you can change this in the information screen);
- use League Manager to merge stashes together when leagues end or delete entire stashes;
- use Stash Viewer to browse your stashed items;
- Filters can be created to find specific affixes or combinations of affixes and rate items;
- Filters can be used in Stash Viewer to score your entire stash in one go;
- Your stash can be exported for use in third-party applications (e.g. Microsoft Excel).

Now allows direct download of your stashed items via JSON from the Path of Exile website.  This new feature is experimental at the moment.  Note that Item Level can't be parsed this way (to my knowledge) so I have to assume that every item is iLevel 100 :(

To use the Download Stash feature you will need to enter your login details in the settings box.  They get encrypted (a bit) before being stored.  Note also that the download will stall from time to time, sometimes for quite a long time.  So be patient when downloading large stashes.

There is no uniqueness test yet, so if you stash the same item twice you wil have two copies.  I might add a delete option to the Stash Viewer.


============================
Known Issues:

- Characters don't do anything yet;
- Flasks, Quivers and Unqiues aren't parsed but flasks and uniques can now be stashed;
- IIR might still cause problems but I fixed it for some items.
