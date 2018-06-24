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
        public List<ProblemSet> AssignedProblemSets = new List<ProblemSet>();
        public int Id { get; set; }
        private string _problemList;

        public Classroom(String name, String tName, String students, int id, String problemList)
        {
            Name = name;
            TeacherName = tName;
            Students = students;
            Id = id;
            _problemList = problemList;
        }

        public void Initialize()
        {
            String[] problemSetArray = _problemList.Split(',');
            if(_problemList.Length<=0)
            {
                return;
            }
            for (int i = 0; i < problemSetArray.Length; i++)
            {
                AssignedProblemSets.Add(Core.Server.Integration.ExecuteGetProblemSetById(problemSetArray[i]));
            }
        }

        public void addStudent(Student stu)
        {
            Students += "," + stu.Id;
            Core.Server.Integration.ExecuteAddStudentToClassroom(this);
        }

        public void assignProblemSet(ProblemSet ps)
        {
            AssignedProblemSets.Add(ps);
            Core.Server.Integration.ExecuteAddAssignment(this, ps);
        }

        public void setClassroomId()
        {
            Id = Core.Server.Integration.ExecuteGetClassroomIdByName(Name);
        }
    }
}
