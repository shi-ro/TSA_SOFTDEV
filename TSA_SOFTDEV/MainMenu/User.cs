using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenu
{
    public class User
    {
        private static String name;
        private static String password;
        private static int points;
        private static String classrooms;
        private static String ranks;
        private static int teamId;

        public User(String n, String p, int po, String c, String r, int t)
        {
            name = n;
            password = p;
            points = po;
            classrooms = c;
            ranks = r;
            teamId = t;
        }

        public String getName()
        {
            return name;
        }

        public String getPassword()
        {
            return password;
        }

        public int getPoints()
        {
            return points;
        }

        public String getClassrooms()
        {
            return classrooms;
        }

        public String getRanks()
        {
            return ranks;
        }

        public int getTeamId()
        {
            return teamId;
        }
    }
}
