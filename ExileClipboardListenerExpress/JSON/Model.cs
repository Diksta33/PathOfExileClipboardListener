using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExileClipboardListener.JSON
{
    public class Character
    {
        public string Name { get; set; }
        public string League { get; set; }
        public string Class { get; set; }
        public int Level { get; set; }

        public Character(DataContracts.JSONCharacter character)
        {
            this.Name = character.Name;
            this.League = character.League;
            this.Class = character.Class;
            this.Level = character.Level;
        }
    }
}
