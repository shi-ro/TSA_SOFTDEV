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
    public partial class ClassManagerScreen : Form
    {
        public List<Student> added = new List<Student>();
        public List<Student> all = new List<Student>();
        public List<ProblemSet> sets = new List<ProblemSet>();
        public List<ProblemSet> selected = new List<ProblemSet>();
        public ClassManagerScreen()
        {
            InitializeComponent();
            all = Core.Server.Integration.ExecuteGetStudents();
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ClassManagerScreen_Load(object sender, EventArgs e)
        {
            all = Core.Server.Integration.ExecuteGetStudents();
            listBox2.Items.Clear();
            foreach (Student s in all)
            {
                listBox2.Items.Add(s.Name);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                //Remove student from class
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
            {
                //Add student to class
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Assign Problem Set
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Unassign Problem Set
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Student student = Core.Server.Integration.ExecuteGetStudent(textBox2.Text);
            if(student!=null)
            {
                //Add student to class
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
