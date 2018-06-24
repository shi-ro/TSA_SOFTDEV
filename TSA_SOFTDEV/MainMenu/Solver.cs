using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainMenu
{
    public partial class Solver : Form
    {
        private ProblemSet _problemSet = null;
        private Queue<Problem> _problemQueue = new Queue<Problem>();
        private Problem _currentProblem = null;
        private int _pointsEarned = 0;
        private int _additive = 10;
        private SolverMode _mode = SolverMode.Limited;
        private BitmapTex _tex;
        private int _attempts = 0;
        private int _allotted = 3;
        private Student _student;
        private Random r = new Random();
        public Solver(ProblemSet problemSet, Student student)
        {
            InitializeComponent();
            _student = student;
            _problemSet = problemSet;
        }

        private void LoadProblem(Problem problem)
        {
            _tex = new BitmapTex(problem.Question);
            pictureBox1.Image = _tex.Image;
        }

        private string FormatQuestion(string question)
        {
            return question.Replace("_",r.Next(11)+"");
        }

        private void Solver_Load(object sender, EventArgs e)
        {
            messageText.KeyUp += EnterPress;
            if (_problemQueue.Count <= 0)
            {
                for (int i = 0; i < _additive; i++)
                {
                    _problemQueue.Enqueue(new Problem(FormatQuestion(_problemSet.Formula)));
                }
            }
            _currentProblem = _problemQueue.Dequeue();
            LoadProblem(_currentProblem);
        }



        private void SendAnswer()
        {
            string attempt = messageText.Text;
            messageText.Text = "";
            bool nextProblem = false;

            // 
            Thread thread = new Thread(() => 
            {
                bool ans = _currentProblem.CompareAnswer(attempt);
                string ret = "";
                if (ans)
                {
                    ret = $"\n[ {attempt} ] \t Correct! (+{_problemSet.Points})\n";
                    _student.changePoints(_problemSet.Points);
                } else
                {
                    ret = $"\n[ {attempt} ] \t Incorrect ... \n";
                }
                richTextBox1.Invoke((MethodInvoker)delegate
                {
                    // Running on the UI thread
                    richTextBox1.Text += ret;
                });
            });


            if (_problemQueue.Count <= _additive / 2)
            {
                for (int i = _problemQueue.Count; i < _additive; i++)
                {
                    _problemQueue.Enqueue(new Problem(FormatQuestion(_problemSet.Formula)));
                }
            }
            _currentProblem = _problemQueue.Dequeue();
            LoadProblem(_currentProblem);
        }

        private void EnterPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendAnswer();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendAnswer();
        }
    }
}
