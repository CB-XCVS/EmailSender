using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace EmailSender
{
    public partial class SelectPeopleForm : Form
    {
        int selectedIndex = 0;
        public SelectPeopleForm()
        {
            InitializeComponent();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                listView1.SelectedItems[i].Remove();
            }
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            string[] row = { "editme@example.org" };
            var listViewItem = new ListViewItem(row);
            listView1.Items.Add(listViewItem);
        }

        private void importBtn_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string[] lines = File.ReadAllLines(openFileDialog1.FileName);
            listView1.Items.Clear();
            foreach (string line in lines)
            {
                string[] row = { line };
                var listViewItem = new ListViewItem(row);
                listView1.Items.Add(listViewItem);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                listView1.SelectedItems[0].BeginEdit();
            }
        }

        private void SelectPeopleForm_Load(object sender, EventArgs e)
        {
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public string[] GetText()
        {
            var list = new List<string>();
            foreach (ListViewItem itemRow in this.listView1.Items)
            {
                list.Add(itemRow.Text);
            }
            return list.ToArray();
        }
    }
}
