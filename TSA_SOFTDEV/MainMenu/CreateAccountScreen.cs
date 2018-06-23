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
    public partial class CreateAccountScreen : Form
    {
        public string UserName = null;
        public string UserPassword = null;
        public CreateAccountScreen(string username)
        {
            InitializeComponent();
            textBox1.Text = username;
            textBox1.KeyDown += EnterForTextbox;
            textBox2.KeyDown += EnterForTextbox;
            textBox3.KeyDown += EnterForTextbox;
        }

        private void EnterForTextbox(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TryCreate();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void TryCreate()
        {
            // check if there is connection
            if (Core.Server.Integration.Connected())
            {
                // connection exists
                // check if username is already in use
                Student user = Core.Server.Integration.ExecuteGetStudent(textBox1.Text);
                if (user == null)
                {
                    // username is not in use
                    // make sure passwords in two boxes match
                    if (textBox2.Text == textBox3.Text)
                    {
                        //connection exists
                        // username is not taken
                        // passwords match
                        // create account
                        label3.Text = "";
                        UserName = textBox1.Text;
                        UserPassword = textBox2.Text;
                        Student student = new Student(UserName, UserPassword, 0, "", "", 1);
                        Core.Server.Integration.ExecuteAddStudent(student);
                        Close();
                    }
                    else
                    {
                        label3.Text = "Passwords don't Match";
                    }
                }
                else
                {
                    label3.Text = "Username In Use";
                }
            }
            else
            {
                label3.Text = "Check Connection";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TryCreate();
        }
    }
}
