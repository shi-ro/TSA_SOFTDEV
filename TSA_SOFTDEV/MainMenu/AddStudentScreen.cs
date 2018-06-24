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
    public partial class AddStudentScreen : Form
    {
        public List<Student> students = new List<Student>();
        public Student ReturnedStudent;
        public AddStudentScreen()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex >= 0)
            {
                ReturnedStudent = students[listBox1.SelectedIndex];
            }
            Close();
        }

        private void AddStudentScreen_Load(object sender, EventArgs e)
        {
            students = Core.Server.Integration.ExecuteGetStudents();
            if(students.Count >=0 )
            {
                foreach (Student s in students)
                {
                    listBox1.Items.Add(s.Name);
                }
            }
        }
    }
}
