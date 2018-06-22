using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenu
{
    public class Student
    {
        public String Name { get; set; }
        public String Password { get; set; }
        public int Points { get; set; }
        public String Classrooms { get; set; }
        public String Ranks { get; set; }
        public int TeamId { get; set; }
        public int Id { get; set; }

        public Student(String n, String p, int po, String c, String r, int t, int i)
        {
            Name = n;
            Password = p;
            Points = po;
            Classrooms = c;
            Ranks = r;
            TeamId = t;
            Id = i;
        }
    }
}
