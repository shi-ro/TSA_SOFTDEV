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
        
        public static bool Connected()
        {
            return External.Wolfram.Connected();
        }

        public static void ExecuteAddStudentToClassroom(Classroom cls)
        {
            SqlCommand cmdNew = new SqlCommand("UPDATE Classrooms SET Classrooms.Students = '" + cls.Students + "' WHERE Id = " + cls.Id + ";", _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
            cmdNew.ExecuteNonQuery();
            _connection.Close();
        }

        public static int ExecuteGetTeamId(Team t)
        {
            SqlCommand cmdNew = new SqlCommand("SELECT Teams.Id FROM Teams WHERE Teams.[Name] = '" + t.Name + "'", _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
            int id = (int)cmdNew.ExecuteScalar();
            _connection.Close();

            return id;
        }

        public static void ExecuteAddAssignment(Classroom cls, ProblemSet assignment)
        {
            String problems = "";
            for(int i = 0; i < cls.AssignedProblemSets.Count; i++)
            {
                problems += ExecuteGetProblemSetIdByName(cls.AssignedProblemSets[i].Name) + "";
                if(i < cls.AssignedProblemSets.Count-1) { problems += ","; }
            }

            SqlCommand cmdNew = new SqlCommand("UPDATE Classrooms SET Classrooms.AssignedProblemSets = '" + "," + ExecuteGetProblemSetIdByName(assignment.Name) + "' WHERE Id = " + cls.Id + ";", _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
            cmdNew.ExecuteNonQuery();
            _connection.Close();
        }

        public static void ExecuteAddStudentToTeam(Team tm)
        {
            SqlCommand cmdNew = new SqlCommand("UPDATE Teams SET Teams.TeamStudents = '" + tm.Students + "' WHERE Teams.Id = " + tm.Id, _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
            cmdNew.ExecuteNonQuery();
            _connection.Close();
        }

        public static int ExecuteGetClassroomIdByName(String name)
        {
            SqlCommand cmdNew = new SqlCommand("SELECT Classrooms.Id FROM Classrooms WHERE Classrooms.[Name] = '" + name + "'", _connection);
            cmdNew.CommandType = CommandType.Text;

            int id = 0;

            _connection.Open();
            try
            {
                SqlDataReader reader = cmdNew.ExecuteReader();
                while (reader.Read())
                {
                    id = (int)reader[0];
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BB=========================BB");
                Console.WriteLine(ex);
            }
            _connection.Close();

            return id;
        }

        public static void ExecuteAddTeam(Team newTeam)
        {

            SqlCommand cmdNew = new SqlCommand("INSERT INTO Teams (Teams.[Name], Teams.TeamStudents) VALUES ('" + newTeam.Name + "', '" + newTeam.Students + "')", _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
            cmdNew.ExecuteNonQuery();
            _connection.Close();

            if(newTeam != null) { newTeam.setId(); }
        }

        public static void ExecuteAddClassroomToTeacher(Classroom cls, Teacher teach)
        {
            SqlCommand cmdString = new SqlCommand("SELECT Teachers.Classrooms FROM Teachers WHERE Teachers.Id = " + teach.Id, _connection);
            cmdString.CommandType = CommandType.Text;

            String classesStringForm = "";

            _connection.Open();
            try
            {
                SqlDataReader reader = cmdString.ExecuteReader();
                while (reader.Read())
                {
                    classesStringForm = reader[0] + "";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("AA=========================AA");
                Console.WriteLine(ex);
            }
            _connection.Close();

            SqlCommand cmdNew = new SqlCommand("UPDATE Teachers SET Classrooms = '" + classesStringForm + "," + cls.Id + "' WHERE Teachers.Id = " + teach.Id, _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
            cmdNew.ExecuteNonQuery();
            _connection.Close();
        }

        public static void ExecuteSaveProblemSet(Teacher teach, ProblemSet ps)
        {
            SqlCommand cmdString = new SqlCommand("SELECT Teachers.SavedProblemSets FROM Teachers WHERE Teachers.Id = " + teach.Id, _connection);
            cmdString.CommandType = CommandType.Text;

            String problemsStringForm = "";

            _connection.Open();
            try
            {
                SqlDataReader reader = cmdString.ExecuteReader();
                while (reader.Read())
                {
                    problemsStringForm = reader[0] + "";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CC=========================CC");
                Console.WriteLine(ex);
            }
            _connection.Close();

            SqlCommand cmdNew = new SqlCommand("UPDATE Teachers SET SavedProblemSets = '" + problemsStringForm + "," + ExecuteGetProblemSetIdByName(ps.Name) + "' WHERE Teachers.Id = " + teach.Id, _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
            cmdNew.ExecuteNonQuery();
            _connection.Close();
        }

        public static List<Student> ExecuteGetStudentsInTeam(int teamid)
        {
            String students = "";
            List<Student> listToReturn = new List<Student>();
            string ins = "SELECT [TeamStudents] FROM Teams WHERE Teams.[Id] = " + teamid;
            SqlCommand cmdString = new SqlCommand(ins, _connection);
            cmdString.CommandType = CommandType.Text;

            _connection.Open(); 
            try
            {
                SqlDataReader reader = cmdString.ExecuteReader();
                while (reader.Read())
                {
                    students = reader[0] + "";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GG=========================GG");
                Console.WriteLine(ex);
            }
            _connection.Close();

            String[] stringList = students.Split(',');

            for (int i = 0; i < stringList.Length; i++)
            {
                listToReturn.Add(ExecuteGetStudent(stringList[i]));
            }

            return listToReturn;

        }

        public static List<ProblemSet> ExecuteGetAllProblemSets()
        {
            SqlCommand cmdOne = new SqlCommand("SELECT count(*) FROM ProblemSets", _connection);
            cmdOne.CommandType = CommandType.Text;

            _connection.Open();
            int num = (int)cmdOne.ExecuteScalar();
            _connection.Close();

            List<ProblemSet> allSets = new List<ProblemSet>(num);

            SqlCommand cmdTwo = new SqlCommand("SELECT ProblemSets.[Name], ProblemSets.Points, ProblemSets.UsesFormula, ProblemSets.Formula, ProblemSets.[Values], ProblemSets.RandomRange, ProblemSets.Description FROM ProblemSets", _connection);
            cmdTwo.CommandType = CommandType.Text;

            _connection.Open();
            try
            {
                SqlDataReader reader = cmdTwo.ExecuteReader();
                while (reader.Read())
                {
                    allSets.Add(new ProblemSet(reader[0] + "", (int)reader[1], reader[6] + "", reader[3] + "", (int)reader[2], reader[4] + "", reader[5] + ""));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GG=========================GG");
                Console.WriteLine(ex);
            }
            _connection.Close();
            return allSets;
        }

        public static List<Team> ExecuteGetAllTeams()
        {
            SqlCommand cmdOne = new SqlCommand("SELECT count(*) FROM Teams", _connection);
            cmdOne.CommandType = CommandType.Text;

            _connection.Open();
            int num = (int)cmdOne.ExecuteScalar();
            _connection.Close();

            List<Team> allTeams = new List<Team>(num);

            SqlCommand cmdTwo = new SqlCommand("SELECT Teams.Id, Teams.[Name], Teams.TeamStudents FROM Teams", _connection);
            cmdTwo.CommandType = CommandType.Text;

            _connection.Open();
            try
            {
                SqlDataReader reader = cmdTwo.ExecuteReader();
                while (reader.Read())
                {
                    allTeams.Add(new Team(reader[1] + "", reader[2] + "", (int)reader[0]));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("JJ=========================JJ");
                Console.WriteLine(ex);
            }
            _connection.Close();
            return allTeams;
        }
        
        public static void ExecuteAddClassroom(String name, Teacher teach, List<Student> students, List<ProblemSet> problemSets)
        {
            String studentList = "";
            for(int i = 0; i < students.Count; i++)
            {
                students[i].setStudentId();
                studentList += students[i].Id;
                if(i< students.Count-1)
                {
                    studentList += ",";
                }
            }

            String problemList = "";
            for(int a = 0; a < problemSets.Count; a++)
            {
                problemList += ExecuteGetProblemSetIdByName(problemSets[a].Name) + "";
            }

            SqlCommand cmdNew = new SqlCommand("INSERT INTO Classrooms (Classrooms.[Name], Classrooms.Teacher, Classrooms.Students, ClassRooms.AssignedProblemSets) VALUES ('" + name + "', '" + teach.Id + "', '" + studentList + "', '" + problemList + "')", _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
            cmdNew.ExecuteNonQuery();
            _connection.Close();
        }

        public static Classroom ExecuteGetClassroom(int id)
        {
            var e = _connection.State;
            SqlCommand cmdNew = new SqlCommand("SELECT Classrooms.[Name], Classrooms.Teacher, Classrooms.Students, Classrooms.AssignedProblemSets FROM Classrooms WHERE Classrooms.Id = " + id, _connection);
            cmdNew.CommandType = CommandType.Text;

            Classroom toReturn = null;
            _connection.Open();
            try
            {
                SqlDataReader reader = cmdNew.ExecuteReader();
                while (reader.Read())
                {
                    toReturn = new Classroom(reader[0] + "", reader[1] + "", reader[2] + "", id, reader[3] + "");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("=========================");
                Console.WriteLine(ex);
            }
            _connection.Close();

            toReturn.Initialize();
            return toReturn;
        }

        public static Student ExecuteGetStudentById(int id)
        {
            SqlCommand cmdNew = new SqlCommand("SELECT Students.[Name] FROM Students WHERE Students.Id = " + id, _connection);
            cmdNew.CommandType = CommandType.Text;

            String n = "";

            _connection.Open();
            try
            {
                SqlDataReader reader = cmdNew.ExecuteReader();
                while (reader.Read())
                {
                    n += reader[0] + "";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("=========================");
                Console.WriteLine(ex);
            }
            _connection.Close();

            return ExecuteGetStudent(n);
        }

        public static Teacher ExecuteGetTeacherByStudent(Student bob)
        {
            SqlCommand cmdNew = new SqlCommand("SELECT Teacher.[Name] FROM Teachers WHERE Teachers.Classrooms = '" + bob.Classrooms + "'", _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
                   String name = cmdNew.ExecuteNonQuery() + "";
            _connection.Close();

            return Core.Server.Integration.ExecuteGetTeacher(name);
        }

        public static Teacher ExecuteGetTeacher(String name)
        {
            SqlCommand cmdNew = new SqlCommand("SELECT Teachers.[Name], Teachers.Password, Teachers.Classrooms, Teachers.SavedProblemSets FROM Teachers WHERE Teachers.[Name] = '" + name + "'", _connection);
            cmdNew.CommandType = CommandType.Text;

            Teacher toReturn = null;

            _connection.Open();
            try
            {
                SqlDataReader reader = cmdNew.ExecuteReader();
                while (reader.Read())
                {
                    toReturn = new Teacher(reader[0] + "", reader[1] + "", reader[2] + "", reader[3] + "");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DD=========================DD");
                Console.WriteLine(ex);
            }
            _connection.Close();
            if (toReturn == null) { return null; }
            toReturn.Initialize();
            toReturn.setTeacherId();
            return toReturn;
        }

        public static int ExecuteGetTeacherId(Teacher smith)
        {
            SqlCommand cmdNew = new SqlCommand("SELECT Teachers.Id FROM Teachers WHERE Teachers.[Name] = '" + smith.Name + "'", _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
            var id = cmdNew.ExecuteScalar();
            _connection.Close();

            return (int)id;
        }

        public static void ExecuteAddTeacher(Teacher sturt)
        {
            SqlCommand cmdNew = new SqlCommand("INSERT INTO [dbo].[Teachers] VALUES('" + sturt.Name + "', '" + sturt.Password + "', '" + sturt.Classrooms + "', '" + sturt.SavedProblemSets + "')", _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
            cmdNew.ExecuteNonQuery();
            _connection.Close();

            sturt.setTeacherId();
        }

        public static List<ProblemSet> ExecuteGetTeacherProblemSets(int teacherid) //
        {
            SqlCommand cmdString = new SqlCommand("SELECT Teachers.SavedProblemSets FROM Teachers WHERE Teachers.Id = " + teacherid, _connection);
            cmdString.CommandType = CommandType.Text;

            String problemsStringForm = "";

            _connection.Open();
            try
            {
                SqlDataReader reader = cmdString.ExecuteReader();
                while (reader.Read())
                {
                    problemsStringForm = reader[0] + "";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("FF=========================FF");
                Console.WriteLine(ex);
            }
            _connection.Close();

            string[] stringArray = problemsStringForm.Split(',');

            return null;
        }

        public static ProblemSet ExecuteGetProblemSetById(string id) // 
        {
            SqlCommand cmdNew = new SqlCommand("SELECT ProblemSets.[Name], ProblemSets.Points, ProblemSets.UsesFormula, ProblemSets.Formula, ProblemSets.[Values], ProblemSets.RandomRange, ProblemSets.Description FROM ProblemSets WHERE ProblemSets.Id = " + id, _connection);
            cmdNew.CommandType = CommandType.Text;
            ProblemSet setToReturn = null;
            _connection.Open();
            try
            {
                SqlDataReader reader = cmdNew.ExecuteReader();
                while (reader.Read())
                {
                    setToReturn = new ProblemSet(reader[0] + "", (int)reader[1], reader[6] + "", reader[3] + "", (int)reader[2], reader[4] + "", reader[5] + "");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GG=========================GG");
                Console.WriteLine(ex);
            }
            _connection.Close();

            return setToReturn;
        }

        public static int ExecuteGetProblemSetIdByName(String name)
        {
            SqlCommand cmdNew = new SqlCommand("SELECT ProblemSets.Id FROM ProblemSets WHERE ProblemSets.[Name] = '" + name + "'", _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
            var id = (int)cmdNew.ExecuteScalar();
            _connection.Close();

            return id;
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

            SqlCommand scndCmd = new SqlCommand("SELECT Teams.Id, Teams.[Name], Teams.TeamStudents FROM Teams WHERE  Teams.Id = " + teamid, _connection);
            scndCmd.CommandType = CommandType.Text;

            _connection.Open();
            try
            {
                SqlDataReader reader = scndCmd.ExecuteReader();
                while (reader.Read())
                {
                    userteam = new Team(reader[1] + "", reader[2] + "", (int)reader[0]);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("HH=========================HH");
                Console.WriteLine(ex);
            }
            _connection.Close();
            userteam.Initialize();
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

            SqlCommand cmdAllUsers = new SqlCommand("SELECT Students.Name, Students.Password, Students.Points, Students.Classrooms, Students.Ranks, Students.TeamId FROM Students", _connection);
            cmdAllUsers.CommandType = CommandType.Text;
            _connection.Open();
            try
            {
                SqlDataReader reader = cmdAllUsers.ExecuteReader();
                while (reader.Read())
                {
                    userList.Add(new Student(reader[0].ToString(), reader[1].ToString(), (int)reader[2], reader[3].ToString(), reader[4].ToString(), (int)reader[5]));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("JJ=========================JJ");
                Console.WriteLine(ex);
            }
            _connection.Close();
            return userList;
        }
        
        public static void ExecuteAddStudent(Student bob) //add a user object to the sql server
        {

            SqlCommand cmdNew = new SqlCommand("INSERT INTO[dbo].[Students] VALUES('" + bob.Name + "', '" + bob.Password + "', " + bob.Points + ", '" + bob.Classrooms + "', '" + bob.Ranks + "', " + bob.TeamId + ")", _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
            cmdNew.ExecuteNonQuery();
            _connection.Close();

            bob.setStudentId();
        }
        
        public static int ExecuteGetStudentId(Student bob)
        {
            SqlCommand cmdNew = new SqlCommand("SELECT Students.Id FROM Students WHERE Students.[Name] = '" + bob.Name + "'", _connection);
            cmdNew.CommandType = CommandType.Text;

            _connection.Open();
            var id = cmdNew.ExecuteScalar();
            _connection.Close();

            return (int)id;
        }

        public static Student ExecuteGetStudent(string name) //using a name, get a user
        {
            SqlCommand cmdNew = new SqlCommand("SELECT Students.Password, Students.Points, Students.Classrooms, Students.Ranks, Students.TeamId FROM Students where Students.[Name] = '" + name + "'", _connection);
            cmdNew.CommandType = CommandType.Text;
            Student userToReturn = null;
            _connection.Open();
            try
            {
                SqlDataReader reader = cmdNew.ExecuteReader();
                while (reader.Read())
                {
                    userToReturn = new Student(name, reader[0] + "", (int)reader[1], reader[2] + "", reader[3] + "", (int)reader[4]);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("=========================");
                Console.WriteLine(ex);
            }
            _connection.Close();

            if(userToReturn != null) { userToReturn.setStudentId(); }

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

        public static object[] ExecuteGetObject(string fromStr, string paramaters, string by, object byParam, bool isString = true)
        {
            var param = isString ? $"'{byParam}'" : byParam;
            var st = paramaters.Replace(",", $",{fromStr}.");
            st = fromStr + "." + st;
            var command = $"SELECT {st} FROM {fromStr} WHERE {fromStr}.{by} = {param}";
            Console.WriteLine(">>>>> " + command);
            SqlCommand cmdNew = new SqlCommand(command, _connection);
            cmdNew.CommandType = CommandType.Text;
            _connection.Open();
            try
            {
                SqlDataReader reader = cmdNew.ExecuteReader();
                object[] obj = new object[reader.FieldCount];
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        obj[i] = reader[i];
                    }
                }
                reader.Close();
                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SS=========================SS");
                Console.WriteLine(ex);
            }
            _connection.Close();
            return null;
        }
    }
}
