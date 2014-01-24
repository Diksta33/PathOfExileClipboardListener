using System;
using System.Windows.Forms;
using ExileClipboardListener.Classes;

namespace ExileClipboardListener.WinForms
{
    public partial class Character : Form
    {
        public Character()
        {
            InitializeComponent();
        }

        private void Character_Load(object sender, EventArgs e)
        {
            RefreshCharacterGrid();
        }

        private void RefreshCharacterGrid()
        {
            GlobalMethods.StuffGrid("SELECT cl.CharacterClassName AS [Class], c.CharacterName AS [Character], c.CharacterLevel AS [Level], l.LeagueName AS [League Name] FROM Character c INNER JOIN CharacterClass cl ON cl.CharacterClassId = c.CharacterClassId LEFT JOIN League l ON l.LeagueId = c.LeagueId;", LeagueGrid);
        }
    }
}
