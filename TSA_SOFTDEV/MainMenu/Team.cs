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
        public List<Student> studentsList;
        public int score;
        public double weightParticipation;

        public Team(String name, String stud, int i)
        {
            Name = name;
            Students = stud;
            Id = i;
            studentsList = getTeamMembers("" + Id);
            weightParticipation = .6;
            score = calculateScore();
        }
        private List<Student> getTeamMembers(string team) //insert teamID
        {
            String[] IDstring = team.Split(',');
            List<Student> students = new List<Student>();
            for (int i = 0; i < IDstring.Length; i++)
            {
                students.Add(Core.Server.Integration.ExecuteGetStudentById(Int32.Parse(IDstring[i])));

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
            int avgPercent = 1 / teammates.Count;
            double buffer = .05;
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
