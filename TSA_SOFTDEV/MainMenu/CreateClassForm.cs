﻿using System;
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
    public partial class CreateClassForm : Form
    {
        public bool Created = false;
        public List<Student> added = new List<Student>();
        public List<Student> all = new List<Student>();
        public string Name;
        public CreateClassForm()
        {
            InitializeComponent();
        }

        private void CreateClassForm_Load(object sender, EventArgs e)
        {
            all = Core.Server.Integration.ExecuteGetStudents();
            listBox2.Items.Clear();
            foreach (Student s in all)
            {
                listBox2.Items.Add(s.Name);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Name = textBox2.Text;
            Created = true;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0 && !added.Contains(all[listBox2.SelectedIndex])) ;
            {
                var cur = all[listBox2.SelectedIndex];
                added.Add(cur);
                listBox1.Items.Add(cur.Name);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex>=0)
            {
                added.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }
    }
}
