using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cloudscribeTemplate
{
    public partial class ProjectOptionsDialog : Form
    {
        public ProjectOptionsDialog()
        {
            InitializeComponent();
            InitControls();
        }

        private void InitControls()
        {
            var cbDataStorage = this.Controls["cbDataStorage"] as ComboBox;
            cbDataStorage.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDataStorage.DataSource = new ComboItem[] {
                new ComboItem{ Key = "MSSQL", Text = "Use Microsoft SqlServer" },
                new ComboItem{ Key = "MySql", Text = "Use MySql" },
                new ComboItem{ Key = "pgsql", Text = "Use PostgreSql" },
                new ComboItem{ Key = "NoDb", Text = "Use NoDb (no database) file system storage" }
            };
            cbDataStorage.ValueMember = "Key";
            cbDataStorage.DisplayMember = "Text";

            var cbMultiTenancy = this.Controls["cbMultiTenancy"] as ComboBox;
            cbMultiTenancy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMultiTenancy.DataSource = new ComboItem[] {
                new ComboItem{ Key = "FolderName", Text = "Use FolderName Multitenancy" },
                new ComboItem{ Key = "HostName", Text = "Use HostName Multitenancy - additional tenants require DNS and web server settings" },
                new ComboItem{ Key = "None", Text = "Use a single tenant/site" }
            };
            cbMultiTenancy.ValueMember = "Key";
            cbMultiTenancy.DisplayMember = "Text";

            chkLogging.Checked = true;

            txtNonRootPagesSegment.CharacterCasing = CharacterCasing.Lower;
            txtNonRootPagesSegment.MaxLength = 30;
            txtNonRootPagesSegment.KeyPress += TxtNonRootPagesSegment_KeyPress;
            txtNonRootPagesSegment.TextChanged += TxtNonRootPagesSegment_TextChanged;

            txtNonRootPagesTitle.MaxLength = 50;
            txtNonRootPagesTitle.KeyPress += TxtNonRootPagesTitle_KeyPress;
            txtNonRootPagesTitle.TextChanged += TxtNonRootPagesTitle_TextChanged;
        }

        private void TxtNonRootPagesTitle_TextChanged(object sender, EventArgs e)
        {
            //in case text is pasted in instead of keyed
            var currentValue = txtNonRootPagesSegment.Text;
            var filtered = new string(currentValue.Where(c => Char.IsLetter(c)).ToArray());
            if (currentValue != filtered)
            {
                txtNonRootPagesSegment.Text = filtered;
            }
        }

        private void TxtNonRootPagesTitle_KeyPress(object sender, KeyPressEventArgs e)
        {
            //only aloow letters
            if (e.KeyChar < 65 || e.KeyChar > 122)
            {
                e.Handled = true;
            }
        }

        private void TxtNonRootPagesSegment_TextChanged(object sender, EventArgs e)
        {
            //in case text is pasted in instead of keyed
            var currentValue = txtNonRootPagesSegment.Text;
            var filtered = new string(currentValue.Where(c => Char.IsLetter(c)).ToArray());
            if(currentValue != filtered)
            {
                txtNonRootPagesSegment.Text = filtered;
            }
            
        }

        private void TxtNonRootPagesSegment_KeyPress(object sender, KeyPressEventArgs e)
        {
            //only aloow letters
            if (e.KeyChar < 65 || e.KeyChar > 122)
            {
                e.Handled = true;
            }
        }

        private void optionB_CheckedChanged(object sender, EventArgs e)
        {
            txtNonRootPagesSegment.Enabled = optionB.Checked;
            txtNonRootPagesTitle.Enabled = optionB.Checked;
        }
    }



    class ComboItem
    {
        public string Key { get; set; }
        public string Text { get; set; }
    }
}
