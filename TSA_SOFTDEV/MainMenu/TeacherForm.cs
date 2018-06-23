﻿using MainMenu;
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
        private List<Classroom> _classrooms = new List<Classroom>();
        private List<ProblemSet> _allProblemSets = new List<ProblemSet>();
        private List<ProblemSet> _savedProblemSets = new List<ProblemSet>();
        private List<Team> _allTeams = new List<Team>();
        private ProblemSet _currentlySelectedProblemSet;
        private Classroom _currentlySelectedClassrom;
        private Team _currentlySelectedTeam;
        public bool setCreatorOpened = false;
        public Teacher teacher;
        public TeacherForm(Teacher teacher)
        {
            this.teacher = teacher; 
            InitializeComponent();
            teacherFormTab.Size = new Size(this.Width - 30, this.Height);//615, 440);
            teacherFormTab.ItemSize = new Size((int)(teacherFormTab.Width/4) - 1, 41);
            teacherFormTab.SizeMode = TabSizeMode.Fixed;
            this.Controls.Add(teacherFormTab);
            _savedProblemSets = teacher.SavedProblemSets;
            _classrooms = teacher.Classrooms;
            LoadAllTeams();
            LoadAllProblemSets();
        }

        private void LoadTeacherClassrooms()
        {
            //call server method

            //update listbox
            if (_classrooms.Count > 0)
            {
                foreach (Classroom cs in _classrooms)
                {
                    listBox1.Items.Add(cs.Name);
                }
            }
        }

        private void LoadSavedProblemSets()
        {
            //call server method

            //update listbox
            if (_savedProblemSets.Count > 0)
            {
                foreach (ProblemSet ps in _savedProblemSets)
                {
                    listBox3.Items.Add(ps.Name);
                }
            }
        }

        private void LoadAllTeams()
        {
            //call server method
            _allTeams = Core.Server.Integration.ExecuteGetAllTeams();
            //update listbox
            if (_allTeams.Count>0)
            {
                foreach(Team t in _allTeams)
                {
                    listBox5.Items.Add(t.Name);
                }
            }
        }

        private void LoadAllProblemSets()
        {
            //call server method
            _allProblemSets = Core.Server.Integration.ExecuteGetAllProblemSets();
            //update listbox
            if (_allProblemSets.Count > 0)
            {
                foreach(ProblemSet ps in _allProblemSets)
                {
                    listBox2.Items.Add(ps.Name);
                }
            }
        }

        private void classTab_Click(object sender, EventArgs e)
        {

        }

        private void TeacherForm_Load(object sender, EventArgs e)
        {

            Size = new Size(692, 505); // 484);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!setCreatorOpened)
            {
                ProblemSetCreatorScreen problemSetCreator = new ProblemSetCreatorScreen();
                problemSetCreator.FormClosed += (object se, FormClosedEventArgs ei) =>
                {
                    setCreatorOpened = false;
                };
                setCreatorOpened = true;
                problemSetCreator.Show();
            }
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox5.SelectedIndex>=0)
            {
                _currentlySelectedTeam = _allTeams[listBox5.SelectedIndex];
                //load students in team

                //load team stats
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
            {
                _currentlySelectedProblemSet = _allProblemSets[listBox2.SelectedIndex];
                //load set stats
                string stats = "";
                stats += $"Name : {_currentlySelectedProblemSet.Name}\n";
                stats += $"Point Worth : {_currentlySelectedProblemSet.Points}\n";
                stats += $"Description : \n\t{_currentlySelectedProblemSet.Description}\n";
                richTextBox5.Text = "";
                richTextBox5.Text = stats;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
