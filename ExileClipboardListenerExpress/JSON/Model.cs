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
            Name = character.Name;
            League = character.League;
            Class = character.Class;
            Level = character.Level;
        }
    }

    public class Tab
    {
        public string Name { get; set; }
        public int I { get; set; }
        public Colour Colour { get; set; }
        public string SrcL { get; set; }
        public string SrcC { get; set; }
        public string SrcR { get; set; }
    }

    public class Colour
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }
}
