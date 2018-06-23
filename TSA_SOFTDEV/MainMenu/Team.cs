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
        public String Users { get; set; }

        public Team(String n, String u, int i)
        {
            Name = n;
            Users = u;
            Id = i;
        }
    }
}
