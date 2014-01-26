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
    public partial class RollDetails : Form
    {
        public int FilterId;
        public string AffixTypeString;
        public string ModClassName;
        public string ModNameString;

        public RollDetails()
        {
            InitializeComponent();
        }

        private void RollDetails_Load(object sender, EventArgs e)
        {
            AffixType.Text = AffixTypeString;
            ModClass.Text = ModClassName;
            ModName.Text = ModNameString;
            RefreshScores();
        }

        private void RefreshScores()
        {
            BestInSlotGrid.Rows.Clear();
            foreach (object[] row in GlobalMethods.BISResults)
                BestInSlotGrid.Rows.Add(row);
            BestForItemGrid.Rows.Clear();
            foreach (object[] row in GlobalMethods.ILevelResults)
                BestForItemGrid.Rows.Add(row);
            YourRollGrid.Rows.Clear();
            foreach (object[] row in GlobalMethods.ItemResults)
                YourRollGrid.Rows.Add(row);
        }
    }
}
