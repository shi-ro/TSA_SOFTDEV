using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using static System.Net.Sockets.Socket;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TSA_SOFTDEV;
using System.Windows.Forms;
//using System.IO.Stream;
//https://stackoverflow.com/questions/8119631/is-there-a-c-sharp-equivalent-way-for-java-inputstream-and-outputstream
//using Windows.Networking.Sockets;
//using System.Collections.Concurrent.ConcurrentQueue;

namespace MainMenu
{
    public class Chat
    {
        private static char[] DELIMITER = { '\r', '\n' };
        private string nick;
        public ConcurrentQueue<string> inQueue;
        public ConcurrentQueue<string> outQueue;
        public List<string> messages;

        private string user;
        private string channel;
        private Boolean run;
        private string server;
        private string port;
        private List<string> messageList;
        private bool _isMessageInQueue = false;
        private string _messageInQueue = "";

        private TcpClient socket;
        private StreamReader reader;
        private StreamWriter writer;

        public Chat(string name, string userID, string TEAMID)
        {

            this.user = userID;//"DANIELA";
            string nickname = name.Replace(" ", "");
            this.nick = nickname + "MATHfgytfgtrftgf";//name + " ?? Mathedonia";   //Get user name
            this.channel = "#Mathedonia_" + TEAMID;// + TEAMID; //replace with actual Team ID
            this.server = "chat.freenode.net";
            this.port = "6667";
            inQueue = new ConcurrentQueue<string>();
            outQueue = new ConcurrentQueue<string>();
            messages = new List<string>();
            var networkThread = new Thread(DoConnect);
            networkThread.Start();
        }

        private void DoConnect()
        {
            TcpClient socket = new TcpClient(server, 6667);
            socket.ReceiveBufferSize = 1024;
            Console.WriteLine("Connected");
            NetworkStream stream = socket.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { NewLine = "\r\n", AutoFlush = true };
            writer.WriteLine("PASS 12345"); //might need to change this
            writer.WriteLine("NICK " + nick);
            writer.WriteLine("USER guest 0 * :" + user);
            writer.WriteLine("JOIN " + channel);
            writer.Flush();
            while (socket.Connected)
            {
                var line = reader.ReadLine();   
                if (line == null) { continue; }
                Console.WriteLine(line);
                inQueue.Enqueue(line);
                messages.Add(line);
                if (line.Contains("PING"))
                {
                    Console.WriteLine("Ping message dealt with");
                    writer.WriteLine("PONG " + channel);
                }
            }
        }

        private void DoMessage(object m)
        {
            string message = $"{m}";
            Console.WriteLine("Message: " + message);
            inQueue.Enqueue(message);
            messages.Add("MYMESSAGE # :" + message);
            writer.WriteLine($"PRIVMSG {channel} :{m}");
            writer.Flush();
        }

        public void SendPress(string m)
        {
            var networkThread = new Thread(DoMessage);
            networkThread.Start(m);
        }
        //Probbably dont neeed
        public void joinChannel()
        {
            string channel = "TEAM1"; //INSERT CHANNEL NAME
            Console.WriteLine("ChannelName:" + channel);
            outQueue.Enqueue("JOIN #" + channel);
        }
       
        private void handlePing()
        {
            outQueue.Enqueue("PONG mercury");
        }

        public void messageDoInBackground()
        {
            //run = true;
            Console.WriteLine("MessageTask: Message task started");
            while (run)
            {
                if (inQueue.Count() >= 0)
                {
                    string message;
                    Boolean m = inQueue.TryDequeue(out message);
                    Console.WriteLine("MessageTask: " + message);
                    //string prefix = "";
                    /*if (message.CharAt(0) == ':')
                    { //indicates presense of a prefix
                        int spaceIndex = message.indexOf(' ');
                        prefix = message.substring(0, spaceIndex);
                        message = message.substring(spaceIndex);
                    }*/
                    int spaceIndex = message.IndexOf(' ');
                    string command = message.Substring(0, spaceIndex).ToUpper();
                    if (command.Equals("PING"))
                    {
                        handlePing();
                        continue;
                    }
                    //publishProgress(prefix, message);
                }
            }
            //return run;
        }
        public void networkDoInBackground()//String... params)
        {
            //Integer.parseInt(params[1]);
            TcpClient irc = new TcpClient(this.server, Int32.Parse(this.port));
            using (NetworkStream stream = irc.GetStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    using (StreamWriter sw = new StreamWriter(stream) { NewLine = "\r\n", AutoFlush = true })
                    {
                        sw.WriteLine("Nick " + this.nick);
                        sw.WriteLine("JOIN #" + this.channel);
                    }
                }
            }
            //Socket socket = new Socket(host, port);
            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//"chat.freenode.net", "6667");
            //StreamSocket socket = new StreamSocket();
            //OutputStream outStream = socket.getOutputStream();
            //InputStream inStream = socket.getInputStream();
            /*Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sck.Bind(new IPEndPoint(0, port)); //IPAddress.Parse("162.213.39.42")
            sck.Listen(0);*/
            /*Console.WriteLine("NetworkTask: Connected to socket");
            while (run) {
                // process input
                if (inStream.available() > 0) 
                {
                    Console.WriteLine("NetworkTask: Detected Message");
                    Boolean foundCR = false;
                    StringBuilder sb = new StringBuilder();
                    while(true) {
                        int next = inStream.read();
                        if (foundCR) {
                            if (next == '\n') {
                                break;
                            }
                        else 
                        {
                            foundCR = false;
                            sb.Append('\n');
                        }
                        }
                        if (next == '\r') {
                            foundCR = true;
                        } else {
                            sb.Append((char) next);
                        }
                    }
                string result = sb.ToString();
                inQueue.Enqueue(result);
                Console.WriteLine("NetworkTask: Processed Message:");
                //Log.i("Message", result);
                }

                // process output
                if (outQueue.Count() == 0) {
                    string message;
                    Boolean m = outQueue.TryDequeue(out message);
                    //outStream.write(message.GetBytes()); //DO I NEED THIS?
                    outStream.write(DELIMITER);
                    Console.WriteLine("NetworkTask: Message Sent:");
                    Console.WriteLine("Message: " + message);
                }
            }
            inStream.close();
            outStream.close();
            socket.close();*/
        }
        
    }
}








/*protected void onProgressUpdate(String...messages) //show messages in box
        {
            TextView message = new TextView(ChatActivity.this);
            message.setText(messages[0] + " " + messages[1]);
            LayoutParams params = new LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.WRAP_CONTENT);
                params.gravity = Gravity.LEFT;
            message.setLayoutParams(params);
            messageList.addView(message);
        }*/

/*public class MessageTask //extends AsyncTask<String, String, Boolean> //might have different params
{
   
    
}

    public class NetworkTask //extends AsyncTask<String, Boolean, Boolean> 
    {
        private Boolean run;

    
    }
}


*/
/*
    public void onBackPressed()
{
    DrawerLayout drawer = findViewById(R.id.drawer_layout);
    if (drawer.isDrawerOpen(GravityCompat.START))
    {
        drawer.closeDrawer(GravityCompat.START);
    }
    else
    {
        super.onBackPressed();
    }
}*/
/*public boolean onCreateOptionsMenu(Menu menu)
{
// Inflate the menu; this adds items to the action bar if it is present.
getMenuInflater().inflate(R.menu.chat, menu);
return true;*/
//}
/* public boolean onOptionsItemSelected(MenuItem item)
{
 // Handle action bar item clicks here. The action bar will
 // automatically handle clicks on the Home/Up button, so long
 // as you specify a parent activity in AndroidManifest.xml.
 int id = item.getItemId();

 //noinspection SimplifiableIfStatement

 return super.onOptionsItemSelected(item);
}
 public boolean onNavigationItemSelected(MenuItem item)
{
 // Handle navigation view item clicks here.
 return true;
}*/

/*private void startTask(object str)
{
if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB)
    asyncTask.executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR, params);
else
    asyncTask.execute(params);
}*/

/*private void Connect(string hostAddress)
   {
       var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
       socket.BeginConnect(hostAddress, Int32.Parse(port), ConnectCompleted, socket);
   }
   private async void ConnectCompleted(IAsyncResult ar)
   {
       var socket = (Socket)ar.AsyncState;
       socket.EndConnect(ar);

       var networkStream = new NetworkStream(socket);
       var reader = new StreamReader(networkStream, Encoding.UTF8);
       //var writer = new StreamWriter(networkStream, Encoding.UTF8);
       do
       {
           var line = await reader.ReadLineAsync();
           Console.WriteLine(line);
       }
       while (!reader.EndOfStream);
       await WriteAsync("NICK " + this.nick, networkStream);
       await WriteAsync("USER " + nick + " 0 " + server + " :TSA_SOFTDEV", networkStream);
       await WriteAsync($"{port}, 6667 : USERID: UNIX : " + nick, networkStream);
       await WriteAsync("JOIN #" + this.channel, networkStream);
       do
       {
           var line = await reader.ReadLineAsync();
           int spaceIndex = line.IndexOf(' ');
           string command = line.Substring(0, spaceIndex).ToUpper();
           if (command.Equals("PING"))
           {
               await WriteAsync("PONG " + this.channel, networkStream);
               continue;
           }
           Console.WriteLine(line);
       }
       while (reader.EndOfStream);



       reader.Close();
       networkStream.Close();
       socket.Close();
   }

   private async Task WriteAsync(string message, NetworkStream stream)
   {
       var command = Encoding.UTF8.GetBytes(string.Format("{0}\r\n", message));

       Console.WriteLine("[S] {0}", message);

       await stream.WriteAsync(command, 0, command.Length);

       await stream.FlushAsync();

       Thread.Sleep(1000);
   }*/
/*public void onCreate() //https://freenode.net/kb/answer/chat
{
    //super.onCreate(savedInstanceState)
    //Intent intent = getIntent();
    //this.nick = intent.getStringExtra("EXTRA_NICK");
    //String server = intent.getStringExtra("EXTRA_SERVER");
    this.server = "chat.freenode.net";
    this.port = "6667";
    inQueue = new ConcurrentQueue<string>();
    outQueue = new ConcurrentQueue<string>();
    //MessageTask mtask = new MessageTask();
    //NetworkTask ntask = new NetworkTask();
    var messageThread = new Thread(messageDoInBackground);
    messageThread.Start();
    var networkThread = new Thread(networkDoInBackground);
    networkThread.Start();
    //startTask(ntask, server, port);
    outQueue.Enqueue("NICK " + nick);
    outQueue.Enqueue("USER " + nick + " 0 " + server + " :TSA_SOFTDEV");
    //outQueue.add("JOIN binaryduo_mercury");
}*/
