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

        public Teacher(String name, String password, String classrooms, String savedProblemSets)
        {
            Name = name;
            Password = password;
            Classrooms = classrooms;
            SavedProblemSets = savedProblemSets;
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

