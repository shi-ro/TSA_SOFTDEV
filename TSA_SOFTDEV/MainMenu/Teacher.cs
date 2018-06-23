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
        public List<Classroom> Classrooms { get; set; }
        public List<ProblemSet> SavedProblemSets { get; set; }
        public int Id { get; set; }

        public Teacher(String name, String password, String classrooms, String s)
        {
            Name = name;
            Password = password;
            string[] cls = classrooms.Split(',');
            for(int i = 0; i < cls.Length; i++)
            {
                Classrooms.Add(Core.Server.Integration.ExecuteGetClassroomById(cls[i])));
            }
            string[] sve = s.Split(',');
            for (int i = 0; i < sve.Length; i++)
            {
                SavedProblemSets.Add(Core.Server.Integration.ExecuteGetProblemSetById(sve[i]));
            }
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

