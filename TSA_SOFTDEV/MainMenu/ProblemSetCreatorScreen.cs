using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainMenu
{
    public partial class ProblemSetCreatorScreen : Form
    {
        public ProblemSetCreatorScreen()
        {
            InitializeComponent();
        }

        private void ProblemSetCreatorScreen_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string combined = "";
            for(int i = 0; i < listBox1.Items.Count; i++)
            {
                combined += listBox1.Items[i];
                if (i < listBox1.Items.Count - 1)
                {
                    combined += ",";
                }
            }
            Core.Server.Integration.ExecuteAddProblemSet(textBox2.Text,(int)numericUpDown1.Value,checkBox1.Checked?1:0,textBox1.Text,combined,$"{numericUpDown2.Value},{numericUpDown3.Value}",richTextBox1.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(textBox3.Text != "")
            {
                listBox1.Items.Add(textBox3.Text);
                textBox3.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex >0)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void ChangeEnabledItems()
        {
            bool C1 = checkBox1.Checked;
            bool C2 = checkBox2.Checked;
            textBox1.Enabled = C1;
            numericUpDown2.Enabled = C1;
            numericUpDown3.Enabled = C1;
            listBox1.Enabled = C2;
            textBox3.Enabled = C2;
            button1.Enabled = C2;
            button6.Enabled = C2;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = !checkBox1.Checked;
            ChangeEnabledItems();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox2.Checked;
            ChangeEnabledItems();
        }
    }
}
