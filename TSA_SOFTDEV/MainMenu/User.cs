using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenu
{
    public class User
    {
        private static String Name { get; set; }
        private static String Password { get; set; }
        private static int Points { get; set; }
        private static String Classrooms { get; set; }
        private static String Ranks { get; set; }
        private static int TeamId { get; set; }

        public User(String n, String p, int po, String c, String r, int t)
        {
            Name = n;
            Password = p;
            Points = po;
            Classrooms = c;
            Ranks = r;
            TeamId = t;
        }
    }
}
