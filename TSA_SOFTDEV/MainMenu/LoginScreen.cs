using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Teacher_form;
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
            if(Core.Server.Integration.Connected())
            {
                Student user = Core.Server.Integration.ExecuteGetStudent(textBox1.Text);
                Teacher user2 = Core.Server.Integration.ExecuteGetTeacher(textBox1.Text);
                if (user!=null)
                {
                    // student exists
                    // check if password is correct
                    if (textBox2.Text == user.Password)
                    {
                        if (openForm != null)
                        {
                            openForm.Close();
                        }
                        // password is correct
                        StudentForm form = new StudentForm(user);
                        // open student form
                        form.Show();
                        // minimize login
                        this.WindowState = FormWindowState.Minimized;
                    }
                    else
                    {
                        errorText.Text = "Incorrect Uassword";
                    }
                } else if(user2 != null)
                {
                    // teacher exists
                    // check if password is correct
                    if (textBox2.Text == user2.Password)
                    {
                        if (openForm != null)
                        {
                            openForm.Close();
                        }
                        // password is correct
                        TeacherForm form = new TeacherForm(user2);
                        // open teacher form
                        form.Show();
                        // minimize login
                        this.WindowState = FormWindowState.Minimized;
                    }
                    else
                    {
                        errorText.Text = "Incorrect Uassword";
                    }
                }
                else
                {
                    errorText.Text = "Invalid Username";
                }
            }
            else
            {
                errorText.Text = "Check Connection";
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
            Enabled = false;
            CreateAccountScreen accountScreen = new CreateAccountScreen(textBox1.Text);
            accountScreen.FormClosed += (object se, FormClosedEventArgs ei) => 
            {
                Enabled = true;
            };
            accountScreen.FormClosing += (object sen, FormClosingEventArgs eis) =>
            {
                if(accountScreen.UserName!=null&&accountScreen.UserPassword!=null)
                {
                    errorText.Text = "";
                    textBox1.Text = accountScreen.UserName;
                    textBox2.Text = accountScreen.UserPassword;
                }
            };
            accountScreen.Show();
        }
    }
}
