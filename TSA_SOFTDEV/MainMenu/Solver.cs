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
    public partial class Solver : Form
    {
        private ProblemSet _problemSet = null;
        public Solver(ProblemSet problemSet)
        {
            InitializeComponent();
            _problemSet = problemSet;
        }

        private void Solver_Load(object sender, EventArgs e)
        { 

        }
    }
}
