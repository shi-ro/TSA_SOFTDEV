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
        List<ProblemSet> problemSets = new List<ProblemSet>();
        ProblemSet selectedSet = null;
        bool solverOpen = false;
        public StudentForm()
        {
            InitializeComponent();
            this.Size = new Size(692, 505);
            teacherFormTab.Size = new Size(this.Width - 30, this.Height-10); 
            teacherFormTab.ItemSize = new Size((int)(teacherFormTab.Width / 5) - 1, 41);
            teacherFormTab.SizeMode = TabSizeMode.Fixed;
            this.Controls.Add(teacherFormTab);

            Console.WriteLine("SETUPCHAT");

            this.chat = new Chat();
            // below code commented out because function has gone missing in the sea of git
            //List<User> users = Core.Server.Integration.ExecuteGetUsers();

            //studentLeaderboardText.Text = 
            chatTextBox.Text = "LOADING...";
            /*BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            //bw.DoWork +=
            bw.ProgressChanged += updateChatBox();
            bw.RunWorkerAsync();*/
            var networkThread = new Thread(updateChatBox);
            networkThread.Start();
            
            //Thread.Sleep(10000); //DEAL WITH THIS
            //updateChatBox();
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
                        filteredMessage = filteredMessage.Split(':')[1];
                        chatTextBox.Invoke((MethodInvoker)delegate {
                        // Running on the UI thread
                        chatTextBox.Text += ("\n [Other User]: " + filteredMessage);
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
            LoadProblemSets();
            listBox1.Items.Clear();
            foreach(ProblemSet ps in problemSets)
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
            problemSets.Add(new ProblemSet("Multi",5,"_ _","Basic multiplication",10,0));
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
            if(idx < problemSets.Count && idx > -1)
            {
                selectedSet = problemSets[idx];
                label8.Text = selectedSet.Name;
                label11.Text = selectedSet.Points + "pts";
                richTextBox5.Text = selectedSet.Description;
                richTextBox7.Text = $"{selectedSet.Completed} of {selectedSet.Problems}";
                button1.Enabled = true;
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if(selectedSet!=null)
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
    }
}
