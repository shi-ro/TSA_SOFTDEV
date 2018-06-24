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
        public List<Classroom> Classrooms = new List<Classroom>();
        public List<ProblemSet> SavedProblemSets = new List<ProblemSet>();
        private string _classrooms = "";
        private string _s = "";
        public int Id { get; set; }

        public Teacher(String name, String password, String classrooms, String s)
        {
            Name = name;
            Password = password;
            _classrooms = classrooms;
            _s = s;
        }

        public void Initialize()
        {
            string[] cls = _classrooms.Split(',');
            for (int i = 0; i < cls.Length; i++)
            {
                Classrooms.Add(Core.Server.Integration.ExecuteGetClassroom(Int32.Parse(cls[i])));
            }
            string[] sve = _s.Split(',');
            for (int i = 0; i < sve.Length; i++)
            {
                SavedProblemSets.Add(Core.Server.Integration.ExecuteGetProblemSetById(sve[i]));
            }
}

        public void setTeacherId()
        {
            Id = Core.Server.Integration.ExecuteGetTeacherId(this);
        }

        public void removeProblemSet(ProblemSet toRemove)
        {
            SavedProblemSets.Remove(toRemove);
        }

        public void saveProblemSet(ProblemSet toSave)
        {
            SavedProblemSets.Add(toSave);
        }
    }
}

