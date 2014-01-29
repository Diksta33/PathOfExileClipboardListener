using System;
using System.Drawing;
using System.Windows.Forms;
using ExileClipboardListener.Classes;

namespace ExileClipboardListener.WinForms
{
    public partial class ModDetails : Form
    {
        public int ModId;

        public ModDetails()
        {
            InitializeComponent();
        }

        private void ModDetails_Load(object sender, EventArgs e)
        {
            RefreshAffixGrid();
            ModName.Text = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + ModId + ";");
            ModClass.Text = GlobalMethods.GetScalarString("SELECT ModClass FROM Mod WHERE ModId = " + ModId + ";");
            Weapons.Checked = GlobalMethods.GetScalarInt("SELECT IFNULL(Weapons, 1) FROM Mod WHERE ModId = " + ModId + ";") == 1;
            Armour.Checked = GlobalMethods.GetScalarInt("SELECT IFNULL(Armour, 1) FROM Mod WHERE ModId = " + ModId + ";") == 1;
            Jewellery.Checked = GlobalMethods.GetScalarInt("SELECT IFNULL(Jewellery, 1) FROM Mod WHERE ModId = " + ModId + ";") == 1;
        }

        private void RefreshAffixGrid()
        {
            string sql = "SELECT PrefixId, Level AS [Required Level], Name AS [Affix Name], 'Prefix' AS [Affix Type], 'Primary' AS [Mod Position], Mod1ValueMin AS [Min], Mod1ValueMax AS [Max] FROM Prefix WHERE Mod1Id = " + ModId;
            sql += " UNION ALL SELECT PrefixId, Level AS [Required Level], Name AS [Affix Name], 'Prefix' AS [Affix Type], 'Secondary' AS [Mod Position], Mod2ValueMin AS [Min], Mod2ValueMax AS [Max] FROM Prefix WHERE Mod2Id = " + ModId;
            sql += " UNION ALL SELECT SuffixId, Level AS [Required Level], Name AS [Affix Name], 'Suffix' AS [Affix Type], 'Primary' AS [Mod Position], Mod1ValueMin AS [Min], Mod1ValueMax AS [Max] FROM Suffix WHERE Mod1Id = " + ModId;
            sql += " UNION ALL SELECT SuffixId, Level AS [Required Level], Name AS [Affix Name], 'Suffix' AS [Affix Type], 'Secondary' AS [Mod Position], Mod2ValueMin AS [Min], Mod2ValueMax AS [Max] FROM Suffix WHERE Mod2Id = " + ModId;
            sql += " ORDER BY Level;";
            GlobalMethods.StuffGrid(sql, AffixGrid, false);
            
            //Colour code the grid
            for (int row = 0; row < AffixGrid.Rows.Count; row++)
            {
                int level = Convert.ToInt32(AffixGrid.Rows[row].Cells[AffixLevelColumn.Index].Value);
                if (level <= GlobalMethods.StashItem.ItemLevel)
                    AffixGrid.Rows[row].DefaultCellStyle.BackColor = Color.LightGreen;
            }
        }
    }
}
