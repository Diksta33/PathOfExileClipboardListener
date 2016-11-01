Path Of Exile Clipboard Listener and Stash Management System
============================================================

Clipboard Listener and Stash Manager for Path of Exile.

Written in Visual Studio 10 C#.Net targetting .NET 4.0 Framework.

Uses System.Data.SQLite ADO.NET library for SQLite (the 32-bit version for VS10).

============================
Main Features:

- Captures item information from Path of Exile when an item is hovered and Ctrl-C is pressed.  This is without communicating with the game client; simply via intercepting the clipboard event.
- Also downloads your stash and inventory items directly from GGG's servers via JSON files.  (Items downloaded this way are all given an Item Level of 100 as this information is not provided by this method of transport.)
- Captured information is parsed, determining the item statistics, base item, requirements, sockets, modifications and affixes.  
- Affixes are worked out on a "best fit" basis where the information available isn't sufficient to determine the exact values rolled.  In theory all affixes should be parsable this way, including a "double roll" of IIR as a Prefix and a Suffix.
- Items can be stashed locally and organised by league.
- Maps, Currency Items and Gems can be stored and viewed in lists.
- Armour, Weapons and Jewellery can be viewed individually in either a large window with lots of information or in a compact view with just the basic information and a visual guide to any affixes rolled.  Once stored they can be viewed in a Stash Viewer, filtered and sorted to find specific attributes.
- User-defined Filters can be created and applied to find items with specific affixes, e.g. Life and Resistances.

============================
How to get started:

- download the package, inflate it and install it using click-once or InnoSetup (requires Admin rights).
- start the application and it will run in the system tray.
- right-click the icon and choose Settings to set options.
- right-click the icon and choose other options to configure Leagues, etc.
- use League Manager to set up leagues and allocate one as default.
- with the app. running hover an item in Path of Exile and hit Ctrl-C to parse the item.
- depending on your setings eithet the item is stashed or a window pops up with information about the item, you can stash the item from here.
- your stashed items go into the league you have as default (or you can change this in the information screen).
- use League Manager to merge stashes together when leagues end or delete entire stashes.
- left-click the icon to launch the Stash Viewer to browse your stashed items.
- Filters can be created to find specific affixes or combinations of affixes and rate items.
- Filters can be used in Stash Viewer to score your entire stash in one go.
- Your stash can be exported for use in third-party applications (e.g. Microsoft Excel).

Now allows direct download of your stashed items via JSON from the Path of Exile website.  This new feature is experimental at the moment.  Note that Item Level can't be parsed this way (to my knowledge) so I have to assume that every item is iLevel 100 :(

To use the Download Stash feature you will need to enter your login details in the settings box.  They get encrypted (a bit) before being stored.  Note also that the download will stall from time to time, sometimes for quite a long time.  So be patient when downloading large stashes.

If you have indicated that you don't want duplicate items then stashing the same item twice will only create one local copy.  However, if an item is modified it can be stored multiple times as the versions will be different.  For Maps, Currency Items and Gems there is no way to check for copies so all stashed items are seen as additions.

============================
Known Issues:

- Characters don't do anything yet;
- Uniques aren't parsed (too many non-standard mods).
