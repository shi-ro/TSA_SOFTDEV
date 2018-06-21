using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenu
{
    public class ProblemSet
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public string Formula { get; set; }
        public string Description { get; set; }
        public int Problems { get; set; }
        public int Completed { get; set; }

        public ProblemSet(string name, int points, string formula, string description, int problems, int completed)
        {
            Name = name;
            Points = points;
            Formula = formula;
            Description = description;
            Problems = problems;
            Completed = completed;
        }
    }
}
