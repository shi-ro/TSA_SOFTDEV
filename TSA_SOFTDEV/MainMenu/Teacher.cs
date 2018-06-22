using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenu
{
    public class Teacher
    {
        public String Name { get; set; }
        public String Password { get; set; }
        public String Classrooms { get; set; }
        public String SavedProblemSets { get; set; }
        public int Id { get; set; }

        public Teacher(String n, String p, String c, String s)
        {
            Name = n;
            Password = p;
            Classrooms = c;
            SavedProblemSets = s;
        }

        public void setTeacherId()
        {
            Id = Core.Server.Integration.ExecuteGetTeacherId(this);
        }

        public void saveProblemSet(String name)
        {

        }
    }
}

