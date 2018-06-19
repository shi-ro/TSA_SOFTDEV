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
    public partial class LoginScreen : Form
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User bob = Core.Server.Integration.ExecuteGetUser("Bob test");
            Console.WriteLine("bob's team is " + Core.Server.Integration.ExecuteGetUserTeam(bob.Name));
        }
    }
}
