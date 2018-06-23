using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TSA_SOFTDEV;

namespace MainMenu
{
    public partial class LoginScreen : Form
    {
        StudentForm openForm = null;
        public LoginScreen()
        {
            InitializeComponent();
            textBox1.KeyDown += EnterForAccount;
            textBox2.KeyDown += EnterForAccount;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            TryLogin();
        }

        private void LoginScreen_Load(object sender, EventArgs e)
        {

        }

        private void TryLogin()
        {
            Student user = Core.Server.Integration.ExecuteGetStudent(textBox1.Text);
            if(user!=null)
            {
                // student exists
                // check if password is correct
                if(textBox2.Text == user.Password)
                {
                    if(openForm!=null)
                    {
                        openForm.Close();
                    }
                    // password is correct
                    StudentForm form = new StudentForm();
                    // open student form
                    form.Show();
                    // minimize login
                    this.WindowState = FormWindowState.Minimized;
                }
                else
                {
                    errorText.Text = "Incorrect password";
                }
            } else
            {
                errorText.Text = "Please specify a valid username";
            }
        }

        private void EnterForAccount(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TryLogin();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
