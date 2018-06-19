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
            List<User> allOfThem = Core.Server.Integration.ExecuteGetUsers();
            for(int i = 0; i < allOfThem.Count; i++)
            {
                Console.WriteLine(allOfThem[i].Name + " that is the name of " + i);
            }
        }
    }
}
