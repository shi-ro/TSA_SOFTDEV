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

        public Student(String name, String password, int points, String classrooms, String ranks, int teamId)
        {
            Name = name;
            Password = password;
            Points = points;
            Classrooms = classrooms;
            Ranks = ranks;
            TeamId = teamId;
        }

        public void setStudentId()
        {
            Id = Core.Server.Integration.ExecuteGetStudentId(this);
        }
    }
}
