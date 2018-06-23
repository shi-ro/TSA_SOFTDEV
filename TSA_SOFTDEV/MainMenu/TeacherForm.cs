using MainMenu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teacher_form
{
    public partial class TeacherForm : Form
    {
        public Teacher teacher;
        public TeacherForm(Teacher teacher)
        {
            this.teacher = teacher; 
            InitializeComponent();
            teacherFormTab.Size = new Size(this.Width - 30, this.Height);//615, 440);
            teacherFormTab.ItemSize = new Size((int)(teacherFormTab.Width/4) - 1, 41);
            teacherFormTab.SizeMode = TabSizeMode.Fixed;
            this.Controls.Add(teacherFormTab);
        }

        private void classTab_Click(object sender, EventArgs e)
        {

        }

        private void TeacherForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(692, 505); // 484);
        }
    }
}
