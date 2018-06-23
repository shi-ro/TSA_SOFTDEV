using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenu
{
    public class Team
    {
        public String Name { get; set; }
        public int Id { get; set; }
        public String Students { get; set; }
        
        public Team(String name, String stud, int i)
        {
            Name = name;
            Students = stud;
            Id = i;
        }
    }
}
