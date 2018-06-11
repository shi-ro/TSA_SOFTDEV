using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSA_SOFTDEV
{
    public partial class StudentForm : Form
    {
        public StudentForm()
        {
            InitializeComponent();
            this.Size = new Size(692, 505);
            teacherFormTab.Size = new Size(this.Width - 30, this.Height-10); 
            teacherFormTab.ItemSize = new Size((int)(teacherFormTab.Width / 5) - 1, 41);
            teacherFormTab.SizeMode = TabSizeMode.Fixed;

            this.Controls.Add(teacherFormTab);
        }

        private void StudentForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(692, 505);
        }
    }
}
