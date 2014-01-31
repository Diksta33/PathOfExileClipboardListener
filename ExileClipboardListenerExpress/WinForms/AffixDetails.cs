using System;
using System.Drawing;
using System.Windows.Forms;
using ExileClipboardListener.Classes;
using si = ExileClipboardListener.Classes.GlobalMethods.StashItem;

namespace ExileClipboardListener.WinForms
{
    public partial class AffixDetails : Form
    {
        public string AffixType;
        public int AffixId;
        public int Mod1Id;
        public int Mod2Id;

        public AffixDetails()
        {
            InitializeComponent();
        }

        private void AffixDetails_Load(object sender, EventArgs e)
        {
            RefreshAffixGrid();
            AffixCategory.Text = GlobalMethods.GetScalarString("SELECT mc.ModCategoryName FROM ModCategory mc INNER JOIN " + AffixType + " a ON a.ModCategoryId = mc.ModCategoryId WHERE a." + AffixType + "Id = " + AffixId + ";");
            Weapons.Checked = GlobalMethods.GetScalarInt("SELECT IFNULL(Weapons, 1) FROM Mod WHERE ModId = " + Mod1Id + ";") == 1;
            Armour.Checked = GlobalMethods.GetScalarInt("SELECT IFNULL(Armour, 1) FROM Mod WHERE ModId = " + Mod1Id + ";") == 1;
            Jewellery.Checked = GlobalMethods.GetScalarInt("SELECT IFNULL(Jewellery, 1) FROM Mod WHERE ModId = " + Mod1Id + ";") == 1;
        }

        private void RefreshAffixGrid()
        {
            GlobalMethods.StuffGrid("SELECT a." + AffixType + "Id, a.Level AS [Required Level], a.Name AS [Affix Name], m1.ModName AS [Primary Mod], a.Mod1ValueMin AS [Min], a.Mod1ValueMax AS [Max], m2.ModName AS [Secondary Mod], a.Mod2ValueMin AS [Min], a.Mod2ValueMax AS [Max] FROM " + AffixType + " a LEFT JOIN Mod m1 ON m1.ModId = a.Mod1Id LEFT JOIN Mod m2 ON m2.ModId = a.Mod2Id WHERE a.Mod1Id = " + Mod1Id + " AND IFNULL(a.Mod2Id, 0) = " + Mod2Id + " ORDER BY a.Level;", AffixGrid, false);
            
            //Colour code the grid
            for (int row = 0; row < AffixGrid.Rows.Count; row++)
            {
                int affixId = Convert.ToInt32(AffixGrid.Rows[row].Cells[AffixIdColumn.Index].Value);
                int level = Convert.ToInt32(AffixGrid.Rows[row].Cells[AffixLevelColumn.Index].Value);
                if (level <= si.ItemLevel)
                    AffixGrid.Rows[row].DefaultCellStyle.BackColor = Color.LightGreen;
                if (affixId == AffixId)
                {
                    AffixGrid.CurrentCell = AffixGrid.Rows[row].Cells[AffixLevelColumn.Index];
                    AffixGrid.Rows[row].Selected = true;
                }
            }
        }
    }
}
