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
using System.Collections.Concurrent;
using System.Threading;


namespace TSA_SOFTDEV
{
    public partial class StudentForm : Form
    {
        Chat chat;
        Student s;
        Teacher t;
        Team team;
        List<Student> teammates;
        List<Student> sortedUsers = new List<Student>();
        List<Team> sortedTeams = new List<Team>();
        List<ProblemSet> problemSets = new List<ProblemSet>();
        ProblemSet selectedSet = null;
        bool solverOpen = false;
        public StudentForm(Student student)
        {
            InitializeComponent();
            s = student;
            string name = s.Name;
            
            team = Core.Server.Integration.ExecuteGetStudentTeam(name);
            //string teammateIDs = team.Students;
            

            //t = Core.Server.Integration.ExecuteGetTeacherByStudent(s);
            
            this.Size = new Size(692, 505);
            teacherFormTab.Size = new Size(this.Width - 30, this.Height - 10);
            teacherFormTab.ItemSize = new Size((int)(teacherFormTab.Width / 5) - 1, 41);
            teacherFormTab.SizeMode = TabSizeMode.Fixed;
            this.Controls.Add(teacherFormTab);

            Console.WriteLine("SETUPCHAT");

            this.chat = new Chat(s.Name, "" + s.Id, "" + team.Id);
            // below code commented out because function has gone missing in the sea of git

            chatTextBox.Text = "LOADING...";
            var networkThread = new Thread(updateChatBox);
            networkThread.Start();

            DoStudentLeaderboard();
            ShowTeamMembers(student);
            DoTeamLeaderboard();

            //names.Add(Core.Server.Integration.ExecuteGetUsers());

            //User bob = Core.Server.Integration.ExecuteGetUser("Bob test");
            //Console.WriteLine("bob's team is " + Core.Server.Integration.ExecuteGetUserTeam(bob.Name));
        }
        private void ShowTeamMembers(Student student)
        {
            Team users = Core.Server.Integration.ExecuteGetStudentTeam(student.Name);
            List<Student> teamList = users.studentsList;
            int count = 1;
            for (int i = 0; i < teamList.Count(); i++)
            {
                ListViewItem lv1 = new ListViewItem(count.ToString());
                lv1.SubItems.Add(teamList[i].Name);
                lv1.SubItems.Add((teamList[i].Points).ToString());
                listView5.Items.Add(lv1);
                //listView3.Items.Add(lv2);

                if (i < sortedUsers.Count() - 1)
                {
                    if (teamList[i].Points == teamList[i + 1].Points)
                    {
                        count += 0;
                    }
                    else
                    {
                        count++;
                    }
                }
                else
                {
                    if (teamList[i].Points == teamList[sortedUsers.Count() - 1].Points)
                        count += 0;
                    else
                    {
                        count++;
                    }
                }
            }



        }
        private void DoTeamLeaderboard()
        {
            List<Team> allTeams = Core.Server.Integration.ExecuteGetAllTeams();
            List<int> TeamScoreList = new List<int>();
            sortedTeams = allTeams.OrderByDescending(x => x.score).ToList();
            int count = 1;
            for (int i = 0; i < allTeams.Count; i++)
            {
                ListViewItem lv1 = new ListViewItem(count.ToString());
                string students = "";
                Console.WriteLine("COUNT: " + sortedTeams[i].studentsList.Count);
                foreach (Student stu in sortedTeams[i].studentsList)
                {
                    students += stu.Name + ", ";
                    Console.WriteLine("SortedTeams: Student: " + stu.Name);
                }
                lv1.SubItems.Add(sortedTeams[i].Name + ": " + students);
                lv1.SubItems.Add((sortedTeams[i].score).ToString());
                listView2.Items.Add(lv1);
                Console.WriteLine("SortedTeams Students: " + students);
                Console.WriteLine("SortedTeams: " + sortedTeams[i].Name);
            }
            
        }
        private void DoStudentLeaderboard()
        {
            List<Student> users = Core.Server.Integration.ExecuteGetStudents();
            sortedUsers = users.OrderByDescending(x => x.Points).ToList();
            int count = 1;
            for (int i = 0; i < sortedUsers.Count(); i++)
            {
                ListViewItem lv1 = new ListViewItem(count.ToString());
                ListViewItem lv2 = new ListViewItem(count.ToString());
                lv1.SubItems.Add(sortedUsers[i].Name);
                lv2.SubItems.Add(sortedUsers[i].Name);
                lv1.SubItems.Add((sortedUsers[i].Points).ToString());
                lv2.SubItems.Add((sortedUsers[i].Points).ToString());
                listView1.Items.Add(lv1);
                //listView3.Items.Add(lv2);

                if (i < sortedUsers.Count() - 1)
                {
                    if (sortedUsers[i].Points == sortedUsers[i + 1].Points)
                    {
                        count += 0;
                    }
                    else
                    {
                        count++;
                    }
                }
                else
                {
                    if (sortedUsers[i].Points == sortedUsers[sortedUsers.Count() - 1].Points)
                        count += 0;
                    else
                    {
                        count++;
                    }
                }
            }

            //Core.Server.Integration.ExecuteGetUserTeam();
            //team leaderboard
        }
        
        private void updateChatBox()
        {
            while (true)
            {
                ConcurrentQueue<string> inQueue = chat.inQueue;
                List<string> listMessages = chat.messages;
                string message;
                Boolean m = inQueue.TryDequeue(out message);
                if (listMessages.Count > 0)
                {
                    message = listMessages.ElementAt(0);
                    listMessages.Remove(message);
                    chat.messages.Remove(message);
                    //FILTER
                    string filteredMessage = message;
                    string otheruser = "";
                    if (message.Contains("End of /NAMES list")) //ONCE Loaded get rid of loaded sign
                    {
                        Console.WriteLine("LOADING SHOULD STOP");
                        messageText.Invoke((MethodInvoker)delegate
                        {
                            // Running on the UI thread
                            messageText.ReadOnly = false;
                        });

                        chatTextBox.Invoke((MethodInvoker)delegate
                        {
                            // Running on the UI thread
                            chatTextBox.Clear();
                        });
                    }
                    if (message.Contains("PRIVMSG #"))
                    {
                        filteredMessage = message.Split('#')[1];
                        otheruser = message.Split('#')[0];
                        filteredMessage = filteredMessage.Split(':')[1];
                        otheruser = otheruser.Split('!')[0];
                        otheruser = otheruser.Substring(1);
                        chatTextBox.Invoke((MethodInvoker)delegate {
                            // Running on the UI thread                           
                            chatTextBox.Text += ("\n [" + otheruser + "]: " + filteredMessage);
                        });

                    }
                    if (message.Contains("MYMESSAGE #"))
                    {
                        filteredMessage = message.Split('#')[1];
                        filteredMessage = filteredMessage.Split(':')[1];
                        chatTextBox.Invoke((MethodInvoker)delegate {
                            // Running on the UI thread
                            chatTextBox.Text += ("\n [Me]: " + filteredMessage);
                        });
                    }
                    //Add to chat box
                    Console.WriteLine("Message added: " + message);
                }
            }
        }

        private void StudentForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(692, 505);
            messageText.KeyUp += EnterForSendMessage;
            chatTextBox.TextChanged += chatTextBox_TextChanged;
            LoadProblemSets();
            listBox1.Items.Clear();
            foreach (ProblemSet ps in problemSets)
            {
                listBox1.Items.Add(ps.Name);
            }
        }

        private void LoadProblemSets()
        {
            // add method call to get problemsets from server
            // and set them to the problem sets list here
            LoadTempProblemSets();
        }

        private void LoadTempProblemSets()
        {
            problemSets.Add(new ProblemSet("Multi", 5, "Basic multiplication", "indefinite integral of sqrt(_)", 1, "", "0,10"));
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
        private void EnterForSendMessage(object sender, KeyEventArgs e)
        {
            if (!messageText.ReadOnly && e.KeyCode == Keys.Enter)
            {
                chat.SendPress(messageText.Text);
                messageText.Text = "";
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        private void sendButton_Click(object sender, EventArgs e)
        {
            chat.SendPress(messageText.Text);
            messageText.Text = "";
        }

        private void messageText_TextChanged(object sender, EventArgs e)
        {

        }

        private void studentAssignments_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = listBox1.SelectedIndex;
            if (idx < problemSets.Count && idx > -1)
            {
                selectedSet = problemSets[idx];
                label8.Text = selectedSet.Name;
                label11.Text = selectedSet.Points + "pts";
                richTextBox5.Text = selectedSet.Description;
                button1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedSet != null)
            {
                Solver solver = new Solver(selectedSet);
                solver.FormClosed += DoSolverClosed;
                button1.Enabled = false;
                solver.Show();
                solverOpen = true;
            }
        }

        private void DoSolverClosed(object sender, FormClosedEventArgs e)
        {
            button1.Enabled = true;
            solverOpen = false;
        }

        private void chatTextBox_TextChanged(object sender, EventArgs e)
        {
            chatTextBox.SelectionStart = chatTextBox.Text.Length;
            chatTextBox.ScrollToCaret();
        }

        private void studentLeaderboardText_TextChanged(object sender, EventArgs e)
        {


        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            int count = 1;
            for (int i = 0; i < sortedUsers.Count(); i++)
            {  
                ListViewItem lv1 = new ListViewItem(count.ToString());
                lv1.SubItems.Add(sortedUsers[i].Name);
                lv1.SubItems.Add((sortedUsers[i].Points).ToString());
                listView1.Items.Add(lv1);
            }
            */
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void memberParticipationChart_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel6_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
