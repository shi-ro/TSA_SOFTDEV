using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenu
{
    public class Classroom
    {
        public String Name { get; set; }
        public String TeacherName { get; set; }
        public String Students { get; set; }
        public int Id { get; set; }

        public Classroom(String name, String tName, String students, int id)
        {
            Name = name;
            TeacherName = tName;
            Students = students;
            Id = id;
        }

        public void setClassroomId()
        {
            Id = 0; //Core.Server.Integration.ExecuteGetStudentId(this);
        }
    }
}
