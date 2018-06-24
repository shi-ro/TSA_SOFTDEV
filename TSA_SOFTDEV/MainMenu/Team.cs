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
        public String Students { get; set; }
        public List<Student> studentsList = new List<Student>();
        public int score;
        public double weightParticipation;

        public Team(String name, String stud)
        {
            Name = name;
            Students = stud;
            weightParticipation = .6;
        }

        public Team(String name, String stud, int i)
        {
            Name = name;
            Students = stud;
            weightParticipation = .6;
            Id = i;
        }

        public void Initialize()
        {
            studentsList = getTeamMembers("" + Id);
            score = calculateScore();
        }

        public void setId()
        {
            Id = Core.Server.Integration.ExecuteGetTeamId(this);
        }

        public void addStudent(Student bob)
        {
            if (Students.Equals(""))
            {
                Students += bob.Name;
            }
            else
            {
                Students += ("," + bob.Name);
            }

            studentsList.Add(bob);
            Core.Server.Integration.ExecuteAddStudentToTeam(this);
        }

        private List<Student> getTeamMembers(string team) //insert teamID
        {
            String[] IDstring = Students.Split(',');
            List<Student> students = new List<Student>();
            for (int i = 0; i < IDstring.Length; i++)
            {
                try
                {
                   
                    students.Add(Core.Server.Integration.ExecuteGetStudentById(Int32.Parse(IDstring[i])));
                }
                catch
                {
                    Console.WriteLine("Error");
                }
                
            }
            return students;
        }
        public int calculateScore()
        {
            List<Student> teammates = studentsList;
            int TeamScore = 0;
            int totalPoints = 0;
            for (int i = 0; i < teammates.Count; i++)
            {
                totalPoints += teammates[i].Points;

            }
            double avgPercent = .5;
            if (teammates.Count > 0)
                avgPercent = 1 / teammates.Count;
        double totalDisplacement = 0;
            for (int i = 0; i < teammates.Count; i++)
            {
                totalDisplacement = Math.Abs(avgPercent - teammates[i].Points / totalPoints);
                //if (teammates[i].Points/totalPoints > (avgPercent + buffer) || teammates[i].Points / totalPoints < (avgPercent - buffer))
            }
            return TeamScore = (int)(totalPoints * (1 - weightParticipation) + totalDisplacement * totalPoints * weightParticipation);
        }
    }
}
