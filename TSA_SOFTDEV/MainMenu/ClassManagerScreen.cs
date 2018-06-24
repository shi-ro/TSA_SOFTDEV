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

namespace MainMenu
{
    public partial class ClassManagerScreen : Form
    {
        public List<Student> added = new List<Student>();
        public List<Student> all = new List<Student>();
        public List<ProblemSet> sets = new List<ProblemSet>();
        public List<ProblemSet> selected = new List<ProblemSet>();
        public Classroom classroom;
        public Teacher teacher;
        public ClassManagerScreen(Classroom classroom, Teacher teacher)
        {
            InitializeComponent();
            this.classroom = classroom;
            all = Core.Server.Integration.ExecuteGetStudents();
            foreach(Student s in all)
            {
                if(s!=null)
                {
                    listBox2.Items.Add(s.Name);
                }
            }
            string[] ls = classroom.Students.Split(',');
            for(int i = 0; i < ls.Length; i++)
            {
                try
                {
                    added.Add(Core.Server.Integration.ExecuteGetStudentById(Int32.Parse(ls[i])));
                }
                catch
                {

                }
            }
            foreach (Student s in added)
            {
                if (s != null)
                {
                    listBox1.Items.Add(s.Name);
                }
            }
            sets = teacher.SavedProblemSets;
            foreach(ProblemSet ps in sets)
            {
                if (ps != null)
                {
                    listBox3.Items.Add(ps.Name);
                }
            }
            selected = classroom.AssignedProblemSets;
            foreach(ProblemSet ps in selected)
            {
                if(ps!=null)
                {
                    listBox4.Items.Add(ps.Name);
                }
            }
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
                classroom.addStudent(all[listBox2.SelectedIndex]);
                // add students visually
                listBox1.Items.Add(all[listBox2.SelectedIndex]);
                added.Add(all[listBox2.SelectedIndex]);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(listBox3.SelectedIndex>=0)
            {
                //Assign Problem Set
                classroom.assignProblemSet(sets[listBox3.SelectedIndex]);
                //assign visually
                listBox4.Items.Add(sets[listBox3.SelectedIndex].Name);
                selected.Add(sets[listBox3.SelectedIndex]);
            }
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
