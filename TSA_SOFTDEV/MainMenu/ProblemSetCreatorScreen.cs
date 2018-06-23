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

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(textBox3.Text != "")
            {
                listBox1.Items.Add(textBox3.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex >0)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }
    }
}
