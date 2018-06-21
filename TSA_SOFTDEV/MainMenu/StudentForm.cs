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
        List<User> sortedUsers = new List<User>();
        public StudentForm()
        {
            InitializeComponent();
            this.Size = new Size(692, 505);
            teacherFormTab.Size = new Size(this.Width - 30, this.Height-10); 
            teacherFormTab.ItemSize = new Size((int)(teacherFormTab.Width / 5) - 1, 41);
            teacherFormTab.SizeMode = TabSizeMode.Fixed;

            List<ProblemSet> problemSets = new List<ProblemSet>();

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



                
            //***THALIAS CODE HERE and yes i actually did code it -_-


            List<User> users = Core.Server.Integration.ExecuteGetUsers();
            //users.Sort();
            //Using lambda to sort
            //User Leaderboard
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
                    if (sortedUsers[i].Points == sortedUsers[sortedUsers.Count()-1].Points)
                    {
                        count += 0;
                    }
                    else
                    {
                        count++;
                    }
                }
            }

            //Core.Server.Integration.ExecuteGetUserTeam();
            //team leaderboard
            for (int i =0; i < users.Count(); i++)
            {

            }


            //names.Add(Core.Server.Integration.ExecuteGetUsers());

            //User bob = Core.Server.Integration.ExecuteGetUser("Bob test");
            //Console.WriteLine("bob's team is " + Core.Server.Integration.ExecuteGetUserTeam(bob.Name));
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
                        chatTextBox.Invoke((MethodInvoker)delegate {
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
            //add method call to get problemsets from server 
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
    }
}
