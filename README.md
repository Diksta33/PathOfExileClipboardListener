PathOfExileClipboardListener
============================

Clipboard Listener and Stash Manager for Path of Exile

Written in Visual Studio 10 C#.Net targetting .NET 4.0 Framework.

Uses System.Data.SQLite ADO.NET library for SQLite (the 32-bit version for VS10).

============================
Known Issues:

- some items incorrectly report that not all affixes were parsed when they were.  This is due to implicit mods and will be looked at next;
- Characters and Leagues aren't complete;
- Flasks, Quivers and Unqiues aren't parsed;
- If an item has an affix that gives two bonuses AND it has one of those bonuses on its own on another affix then the parser fails to work this out properly.  I am going to add some code to guess the values and indicate this in the UI as this is the best that can be done in these cases;
- IIR doesn't work properly because it can be a prefix AND a suffix :(
