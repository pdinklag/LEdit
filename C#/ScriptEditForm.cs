using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LEdit
{
    public partial class ScriptEditForm : Form
    {
        L2BattleScript initial_script;
        L2BattleScript script;

        public ScriptEditForm(L2BattleScript open_script, string title)
        {
            InitializeComponent();

            Text = "Skript-Editor: " + title;

            initial_script = open_script;
            script = open_script;

            ShowStuff();
        }

        private void ShowStuff()
        {
            txtScript.Text = script.Disassemble();
            txtScript.SelectionStart = 0;
            txtScript.SelectionLength = 0;

            txtCode.Text = "";
            for (int i = 0; i < script.Bytes.Count; i++ )
            {
                txtCode.Text += script.Bytes[i].ToString("X2");
                if (i + 1 < script.Bytes.Count)
                    txtCode.Text += " ";
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void ScriptEditForm_Load(object sender, EventArgs e)
        {

        }

        private void codeKompilierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Collection<byte> x = new Collection<byte>();
            
            string code = txtCode.Text.Replace(" ", "");
            code = code.Replace("\r\n", "");

            if(code.Length % 2 == 0)
            {
                for (int i = 0; i < code.Length; i += 2)
                {
                    byte b = 0;
                    if (byte.TryParse(code.Substring(i, 2), System.Globalization.NumberStyles.HexNumber, null, out b))
                        x.Add(b);
                }
            }

            script = new L2BattleScript(x, initial_script.Offset);
            ShowStuff();
        }

        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(initial_script != script)
                initial_script.Bytes = script.Bytes;
        }
    }
}
