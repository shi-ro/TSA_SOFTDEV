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
        public Solver(ProblemSet problemSet)
        {
            InitializeComponent();
            _problemSet = problemSet;
        }

        private void LoadProblem(Problem problem)
        {
            _tex = new BitmapTex(problem.Question);
            pictureBox1.Image = _tex.Image;
        }

        private string FormatQuestion(string question)
        {
            return question.Replace("_","2");
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
            if (_currentProblem.CompareAnswer(attempt))
            {
                nextProblem = true;   
            } else
            {
                _attempts++;
                if(_attempts >= _allotted)
                {
                    nextProblem = true;
                }
            }
            if(nextProblem)
            {
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
