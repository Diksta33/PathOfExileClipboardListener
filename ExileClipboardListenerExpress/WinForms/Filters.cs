using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExileClipboardListener.Classes;

namespace ExileClipboardListener.WinForms
{
    public partial class Filters : Form
    {
        private bool _adding;
        private bool _editing;

        //Current Filter
        private int _filterId;
        private int _itemTypeId;
        private int _itemSubTypeId;

        public Filters()
        {
            InitializeComponent();
        }

        private void Filters_Load(object sender, EventArgs e)
        {
            GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT ItemTypeName FROM ItemType ORDER BY 1;", ItemType);
            RefreshFilterGrid();
            RefreshButtons();
        }

        private void RefreshFilterGrid()
        {
            GlobalMethods.StuffGrid("SELECT f.FilterName AS [Filter Name], IFNULL(i.ItemTypeName, '(All)') AS [Item Class], IFNULL(ist.ItemSubTypeName, '(All)') AS [Item Type], COUNT(*) AS [Affix Count] FROM FilterHeader f LEFT JOIN ItemType i ON i.ItemTypeId = f.ItemTypeId LEFT JOIN ItemSubType ist ON ist.ItemTypeId = f.ItemTypeId AND ist.ItemSubTypeId = f.ItemSubTypeId LEFT JOIN FilterDetail fd ON fd.FilterId = f.FilterId GROUP BY f.FilterName, IFNULL(i.ItemTypeName, '(All)'), IFNULL(ist.ItemSubTypeName, '(All)') ORDER BY 1; ", FilterGrid);
        }

        private void RefreshButtons()
        {
            EditFilter.Enabled = !_adding && !_editing;
            NewFilter.Enabled = !_adding && !_editing;
            CancelFilter.Enabled = _adding || _editing;
            DeleteFilter.Enabled = !_adding && !_editing && FilterGrid.CurrentRow != null;
            SaveFilter.Enabled = _adding || _editing;
            FilterName.Enabled = _adding || _editing;
            ItemType.Enabled = _adding || _editing;
            ItemSubType.Enabled = _adding || _editing;
            Prefix1ModClass.Enabled = _adding || _editing;
            Prefix1Mod.Enabled = _adding || _editing;
            Prefix2ModClass.Enabled = _adding || _editing;
            Prefix2Mod.Enabled = _adding || _editing;
            Prefix3ModClass.Enabled = _adding || _editing;
            Prefix3Mod.Enabled = _adding || _editing;
            Suffix1ModClass.Enabled = _adding || _editing;
            Suffix1Mod.Enabled = _adding || _editing;
            Suffix2ModClass.Enabled = _adding || _editing;
            Suffix2Mod.Enabled = _adding || _editing;
            Suffix3ModClass.Enabled = _adding || _editing;
            Suffix3Mod.Enabled = _adding || _editing;
        }

        private void NewFilter_Click(object sender, EventArgs e)
        {
            _adding = true;
            FilterName.Text = "";
            if (ItemType.Items.Count > 0)
                ItemType.SelectedIndex = 0;
            if (Prefix1ModClass.Items.Count > 0)
                Prefix1ModClass.SelectedIndex = 0;
            if (Prefix1Mod.Items.Count > 0)
                Prefix1Mod.SelectedIndex = 0;
            if (Prefix2ModClass.Items.Count > 0)
                Prefix2ModClass.SelectedIndex = 0;
            if (Prefix2Mod.Items.Count > 0)
                Prefix2Mod.SelectedIndex = 0;
            if (Prefix3ModClass.Items.Count > 0)
                Prefix3ModClass.SelectedIndex = 0;
            if (Prefix3Mod.Items.Count > 0)
                Prefix3Mod.SelectedIndex = 0;
            if (Suffix1ModClass.Items.Count > 0)
                Suffix1ModClass.SelectedIndex = 0;
            if (Suffix1Mod.Items.Count > 0)
                Suffix1Mod.SelectedIndex = 0;
            if (Suffix2ModClass.Items.Count > 0)
                Suffix2ModClass.SelectedIndex = 0;
            if (Suffix2Mod.Items.Count > 0)
                Suffix2Mod.SelectedIndex = 0;
            if (Suffix3ModClass.Items.Count > 0)
                Suffix3ModClass.SelectedIndex = 0;
            if (Suffix3Mod.Items.Count > 0)
                Suffix3Mod.SelectedIndex = 0;
            RefreshButtons();
        }

        private void ItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Filter the Item Sub Type by the class selected
            _itemTypeId = GlobalMethods.GetScalarInt("SELECT ItemTypeId FROM ItemType WHERE ItemTypeName = '" + ItemType.Text + "';");
            if (_itemTypeId == 0)
            {
                ItemSubType.Items.Clear();
                ItemSubType.Items.Add("(All)");
            }
            else
                GlobalMethods.StuffCombo("SELECT '(All)' UNION ALL SELECT ItemSubTypeName FROM ItemSubType WHERE ItemTypeId = " + _itemTypeId + " ORDER BY 1;", ItemSubType);
            if (ItemSubType.Items.Count > 0)
                ItemSubType.SelectedIndex = 0;
        }

        private void ItemSubType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _itemSubTypeId = GlobalMethods.GetScalarInt("SELECT ItemSubTypeId FROM ItemSubType WHERE ItemTypeId = " + _itemTypeId + " AND ItemSubTypeName = '" + ItemSubType.Text + "';");

            //Refresh the available prefix and suffix mods depending on the Item Type/ Item Sub Type selected
            LoadModClass(Prefix1ModClass, "Prefix");
            LoadModClass(Prefix2ModClass, "Prefix");
            LoadModClass(Prefix3ModClass, "Prefix");
            LoadModClass(Suffix1ModClass, "Suffix");
            LoadModClass(Suffix2ModClass, "Suffix");
            LoadModClass(Suffix3ModClass, "Suffix");
        }

        private void LoadModClass(ComboBox AffixModClass, string AffixType)
        {
            //Load the mod classes
            string sql = "SELECT '(Any)' UNION ALL SELECT DISTINCT ModClass FROM Mod m INNER JOIN " + AffixType + " a ON a.Mod1Id = m.ModId OR a.Mod2Id = m.ModId WHERE ModClass IS NOT NULL";

            //Filter by Item Type
            if (ItemType.Text == "Armour")
                sql += " AND IFNULL(m.Armour, 1) = 1";
            if (ItemType.Text == "Weapons")
                sql += " AND IFNULL(m.Weapons, 1) = 1";
            if (ItemType.Text == "Jewellery")
                sql += " AND IFNULL(m.Jewellery, 1) = 1";

            //There are also exceptions, boots are the only item that can roll movement speed for example
            if ((ItemType.Text != "Armour" && ItemType.Text != "(All)") || (ItemSubType.Text != "Boots" && ItemSubType.Text != "(All)"))
                sql += " AND m.ModName != 'Base Movement Velocity +%'";
            sql += " ORDER BY 1;";

            //Push this into the combo
            GlobalMethods.StuffCombo(sql, AffixModClass);
            if (AffixModClass.Items.Count > 0)
                AffixModClass.SelectedIndex = 0;
        }

        private void Prefix1ModClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMod(Prefix1Mod, "Prefix", Prefix1ModClass.Text);
        }

        private void Prefix2ModClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMod(Prefix2Mod, "Prefix", Prefix2ModClass.Text);
        }

        private void Prefix3ModClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMod(Prefix3Mod, "Prefix", Prefix3ModClass.Text);
        }

        private void Suffix1ModClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMod(Suffix1Mod, "Suffix", Suffix1ModClass.Text);
        }

        private void Suffix2ModClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMod(Suffix2Mod, "Suffix", Suffix2ModClass.Text);
        }

        private void Suffix3ModClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMod(Suffix3Mod, "Suffix", Suffix3ModClass.Text);
        }

        private void LoadMod(ComboBox AffixMod, string AffixType, string ModClassName)
        {
            //Load the mods 
            string sql = "SELECT '(Any)' UNION ALL SELECT DISTINCT m.ModName FROM Mod m INNER JOIN " + AffixType + " a ON a.Mod1Id = m.ModId OR a.Mod2Id = m.ModId WHERE ModClass IS NOT NULL";

            //Filter by class
            if (ModClassName != "(Any)")
                sql += " AND ModClass = '" + ModClassName + "'";

            //Filter by Item Type
            if (ItemType.Text == "Armour")
                sql += " AND IFNULL(m.Armour, 1) = 1";
            if (ItemType.Text == "Weapons")
                sql += " AND IFNULL(m.Weapons, 1) = 1";
            if (ItemType.Text == "Jewellery")
                sql += " AND IFNULL(m.Jewellery, 1) = 1";

            //There are also exceptions, boots are the only item that can roll movement speed for example
            if ((ItemType.Text != "Armour" && ItemType.Text != "(All)") || (ItemSubType.Text != "Boots" && ItemSubType.Text != "(All)"))
                sql += " AND m.ModName != 'Base Movement Velocity +%'";
            sql += " ORDER BY 1;";

            //Push this into the combo
            GlobalMethods.StuffCombo(sql, AffixMod);
            if (AffixMod.Items.Count > 0)
                AffixMod.SelectedIndex = 0;
        }

        private void CancelFilter_Click(object sender, EventArgs e)
        {
            _adding = false;
            _editing = false;
            FilterGrid_SelectionChanged(null, null);
            RefreshButtons();
        }

        private void FilterGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (FilterGrid.CurrentRow == null)
                return;

            //Load the current row values into the controls
            //First the header
            FilterName.Text = FilterGrid.CurrentRow.Cells[0].Value.ToString();
            _filterId = GlobalMethods.GetScalarInt("SELECT FilterId FROM FilterHeader WHERE FilterName = '" + FilterName.Text.Replace("'", "''") + "';");
            _itemTypeId = GlobalMethods.GetScalarInt("SELECT ItemTypeId FROM FilterHeader WHERE FilterId = " + _filterId + ";");
            int OldIndex = ItemType.SelectedIndex;
            if (_itemTypeId == 0)
                ItemType.SelectedIndex = 0;
            else
                ItemType.Text = GlobalMethods.GetScalarString("SELECT ItemTypeName FROM ItemType WHERE ItemTypeId = " + _itemTypeId + ";");
            if (OldIndex == ItemType.SelectedIndex)
                ItemType_SelectedIndexChanged(null, null);
            OldIndex = ItemSubType.SelectedIndex;
            _itemSubTypeId = GlobalMethods.GetScalarInt("SELECT ItemSubTypeId FROM FilterHeader WHERE FilterId = " + _filterId + ";");
            if (_itemSubTypeId == 0)
                ItemSubType.SelectedIndex = 0;
            else
                ItemSubType.Text = GlobalMethods.GetScalarString("SELECT ItemSubTypeName FROM ItemSubType WHERE ItemTypeId = " + _itemTypeId + " AND ItemSubTypeId = " + _itemSubTypeId + ";");
            if (OldIndex == ItemSubType.SelectedIndex)
                ItemSubType_SelectedIndexChanged(null, null);

            //Then the details
            for (int affixSlot = 1; affixSlot < 7; affixSlot++)
            {
                string affixName = affixSlot < 4 ? "Prefix" + affixSlot : "Suffix" + (affixSlot - 3);
                string modClass = GlobalMethods.GetScalarString("SELECT ModClass FROM FilterDetail WHERE FilterId = " + _filterId + " AND AffixSlot = " + affixSlot + ";");
                int modId = GlobalMethods.GetScalarInt("SELECT ModId FROM FilterDetail WHERE FilterId = " + _filterId + " AND AffixSlot = " + affixSlot + ";");
                if (modClass != "")
                {
                    Controls.Find(affixName + "ModClass", true)[0].Text = modClass;
                }
                if (modId != 0)
                {
                    Controls.Find(affixName + "Mod", true)[0].Text = GlobalMethods.GetScalarString("SELECT ModName FROM Mod WHERE ModId = " + modId + ";");
                }
            }
        }

        private void DeleteFilter_Click(object sender, EventArgs e)
        {
            //Delete the currently selected filter
            if (MessageBox.Show("Are you sure?", "Confirm Delete", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            GlobalMethods.RunQuery("DELETE FROM FilterDetail WHERE FilterId = " + _filterId + ";");
            GlobalMethods.RunQuery("DELETE FROM FilterHeader WHERE FilterId = " + _filterId + ";");
            RefreshFilterGrid();
            RefreshButtons();
        }

        private void SaveFilter_Click(object sender, EventArgs e)
        {
            //Save the filter to the database
            //First check the name is unique if we are adding
            if (_adding)
            {
                _filterId = GlobalMethods.GetScalarInt("SELECT FilterId FROM FilterHeader WHERE FilterName = '" + FilterName.Text.Replace("'", "''") + "';");
                if (_filterId != 0)
                {
                    if (MessageBox.Show("There is already a filter with that name, replace it", "Confirm Replace", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;

                    //Maybe we were editing after all!
                    _editing = true;
                }
            }

            //Delete the current filter
            if (_editing)
            {
                GlobalMethods.RunQuery("DELETE FROM FilterDetail WHERE FilterId = " + _filterId + ";");
                GlobalMethods.RunQuery("DELETE FROM FilterHeader WHERE FilterId = " + _filterId + ";");
            }

            //Save the filter
            GlobalMethods.RunQuery("INSERT INTO FilterHeader(FilterName, ItemTypeId, ItemSubTypeId) VALUES ('" + FilterName.Text.Replace("'", "''") + "'," + _itemTypeId + "," + _itemSubTypeId + ");");
            _filterId = GlobalMethods.GetScalarInt("SELECT FilterId FROM FilterHeader WHERE FilterName = '" + FilterName.Text.Replace("'", "''") + "';");
            for (int affixSlot = 1; affixSlot < 7; affixSlot++)
            {
                string affixName = affixSlot < 4 ? "Prefix" + affixSlot : "Suffix" + (affixSlot - 3);
                string modClass = Controls.Find(affixName + "ModClass", true)[0].Text;
                int modId = GlobalMethods.GetScalarInt("SELECT ModId FROM Mod WHERE ModName = '" + Controls.Find(affixName + "Mod", true)[0].Text + "';");
                if (modClass != "(Any)" || modId != 0)
                    GlobalMethods.RunQuery("INSERT INTO FilterDetail(FilterId, AffixSlot, ModClass, ModId) VALUES (" + _filterId + "," + affixSlot + ",'" + (modClass == "(Any)" ? "NULL" : modClass) + "'," + modId + ");");
            }

            //All done
            _editing = false;
            _adding = false;
            RefreshFilterGrid();
            RefreshButtons();
        }

        private void EditFilter_Click(object sender, EventArgs e)
        {
            _editing = true;
            _filterId = GlobalMethods.GetScalarInt("SELECT FilterId FROM FilterHeader WHERE FilterName = '" + FilterName.Text.Replace("'", "''") + "';");
            RefreshButtons();
        }
    }
}
