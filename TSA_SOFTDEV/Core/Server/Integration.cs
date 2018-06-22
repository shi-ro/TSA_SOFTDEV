using MainMenu;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Core.Server
{
    public static class Integration
    {
        private static SqlConnection _connection = new SqlConnection("Server=tcp:softdevserver.database.windows.net,1433;Initial Catalog=SoftDevDB;Persist Security Info=False;User ID=serveradmin;Password=SoftDev!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        public static List<ProblemSet> ExecuteGetTeacherProblemsets(string teacherName) //
        {
            return null;
        }

        public static ProblemSet ExecuteGetProblemsetById(string id) // 
        {
            return null;
        }

        public static void ExecuteAddProblemSet(String name, int points, int usesFormula, String formula, String values, String rr, String description) // additional params for creation here
        {
            SqlCommand cmdNew = new SqlCommand("INSERT INTO[dbo].[ProblemSets] VALUES('" + name + "', " + points + ", " + usesFormula + ", '" + formula + "', '" + values + "', '" + rr + "', '" + description + "')", _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
            cmdNew.ExecuteNonQuery();
            _connection.Close();
        }

        public static Team ExecuteGetStudentTeam(String username)
        {
            int teamid = Core.Server.Integration.ExecuteGetStudent(username).TeamId;
            Team userteam = null;

            SqlCommand scndCmd = new SqlCommand("SELECT Teams.Id, Teams.TeamName, Teams.Users, Teams.Points FROM Teams WHERE  Teams.Id = " + teamid, _connection);
            scndCmd.CommandType = CommandType.Text;

            try
            {
                _connection.Open();
                SqlDataReader reader = scndCmd.ExecuteReader();
                while (reader.Read())
                {
                    userteam = new Team(reader[1] + "", (int)reader[3], reader[2] + "", (int)reader[0]);
                }
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("=========================");
                Console.WriteLine(ex);
            }
            return userteam;
        }
        
        public static List<Student> ExecuteGetStudents() //return a list of all the user objects
        {
            SqlCommand cmdGetCount = new SqlCommand("SELECT count(*) FROM Students", _connection);
            cmdGetCount.CommandType = CommandType.Text;
            _connection.Open();

            var numUsers = cmdGetCount.ExecuteScalar();

            _connection.Close();

            List<Student> userList = new List<Student>((int)numUsers);

            SqlCommand cmdAllUsers = new SqlCommand("SELECT Students.Name, Students.Password, Students.Points, Students.Classrooms, Students.Ranks, Students.TeamId, Students.Id FROM Students", _connection);
            cmdAllUsers.CommandType = CommandType.Text;
            try
            {
                _connection.Open();
                SqlDataReader reader = cmdAllUsers.ExecuteReader();
                while (reader.Read())
                {
                    userList.Add(new Student(reader[0].ToString(), reader[1].ToString(), (int)reader[2], reader[3].ToString(), reader[4].ToString(), (int)reader[5], (int)reader[6]));
                }
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("=========================");
                Console.WriteLine(ex);
            }

            return userList;
        }
        
        
        
        
        public static void ExecuteAddStudent(Student bob) //add a user object to the sql server
        {

            SqlCommand cmdNew = new SqlCommand("INSERT INTO[dbo].[Students] VALUES('" + bob.Name + "', '" + bob.Password + "', " + bob.Points + ", '" + bob.Classrooms + "', '" + bob.Ranks + "', " + bob.TeamId + ")", _connection);
            cmdNew.CommandType = CommandType.Text;
            _connection.Open();
            cmdNew.ExecuteNonQuery();
            _connection.Close();
        }
        
        public static Student ExecuteGetStudent(string name) //using a name, get a user
        {
            SqlCommand cmdNew = new SqlCommand("SELECT Students.Password, Students.Points, Students.Classrooms, Students.Ranks, Students.TeamId, Students.Id FROM Students where Students.[Name] = '" + name + "'", _connection);
            cmdNew.CommandType = CommandType.Text;
            Student userToReturn = null;
            try
            {
                _connection.Open();
                SqlDataReader reader = cmdNew.ExecuteReader();
                while (reader.Read())
                {
                    userToReturn = new Student(name, reader[0] + "", (int)reader[1], reader[2] + "", reader[3] + "", (int)reader[4], (int)reader[5]);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("=========================");
                Console.WriteLine(ex);
            }
            _connection.Close();
            return userToReturn;
        }

        public static object ExecuteQuery(string command)
        {
            SqlCommand cmdNew = new SqlCommand(command, _connection);
            cmdNew.CommandType = CommandType.Text;
            _connection.Open();
            var result = cmdNew.ExecuteNonQuery();
            _connection.Close();
            return result;
        }

        public static SqlDataReader ExecuteRead(string command)
        {
            SqlCommand cmdNew = new SqlCommand(command, _connection);
            cmdNew.CommandType = CommandType.Text;
            _connection.Open();
            SqlDataReader reader = cmdNew.ExecuteReader();
            return reader;
        }
    }
}
