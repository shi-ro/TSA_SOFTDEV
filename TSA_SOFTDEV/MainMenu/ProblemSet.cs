﻿using System;
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
        public bool UsesFormula { get; set; }
        public string[] Values { get; set; }
        public int MaxVal { get; set; }
        public int MinVal { get; set; }

        public ProblemSet(string name, int points,string description, string formula, int usesFormula, string values, string randomRange)
        {
            Name = name;
            Points = points;
            Description = description;
            UsesFormula = usesFormula==1?true:false;
            MinVal = Int32.Parse(randomRange.Split(',')[0]);
            MaxVal = Int32.Parse(randomRange.Split(',')[1]);
            if (UsesFormula)
            {
                Formula = formula;
            } else
            {
                Values = values.Split(',');
            }
        }
    }
}
