﻿using MainMenu;
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
        
        public static List<User> ExecuteGetUsers() //return a list of all the user objects
        {
            SqlCommand cmdGetCount = new SqlCommand("SELECT count(*) FROM Users", _connection);
            cmdGetCount.CommandType = CommandType.Text;
            _connection.Open();

            var numUsers = cmdGetCount.ExecuteScalar();

            _connection.Close();

            List<User> userList = new List<User>((int)numUsers);

            SqlCommand cmdAllUsers = new SqlCommand("SELECT Users.Name, Users.Password, Users.Points, Users.Classrooms, Users.Ranks, Users.TeamId, Users.Id FROM Users", _connection);
            cmdAllUsers.CommandType = CommandType.Text;
            try
            {
                _connection.Open();
                SqlDataReader reader = cmdAllUsers.ExecuteReader();
                while (reader.Read())
                {
                    userList.Add(new User(reader[0].ToString(), reader[1].ToString(), (int)reader[2], reader[3].ToString(), reader[4].ToString(), (int)reader[5], (int)reader[6]));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("=========================");
                Console.WriteLine(ex);
            }

            return userList;
        }

        public static void ExecuteAddUser(User bob) //add a user object to the sql server
        {

            // ExecuteQuery($"INSERT INTO[dbo].[Users] VALUES('{bob.getName()}', '{bob.getPassword()}', {bob.getPoints()}, '{bob.getClassrooms()}', '{bob.getRanks()}', {bob.getTeamId()})");

            //
            //          ABOVE (Reccomended code) BELOW (Actual code) 
            //

            SqlCommand cmdNew = new SqlCommand("INSERT INTO[dbo].[Users] VALUES('" + bob.Name + "', '" + bob.Password + "', " + bob.Points + ", '" + bob.Classrooms + "', '" + bob.Ranks + "', " + bob.TeamId + ")", _connection);
            cmdNew.CommandType = CommandType.Text;
            _connection.Open();
            cmdNew.ExecuteNonQuery();
            _connection.Close();
        }

        public static User ExecuteGetUser(string name) //using a name, get a user
        {

            //SqlDataReader reader = ExecuteRead($"SELECT Users.Password, Users.Points, Users.Classrooms, Users.Ranks, Users.TeamId FROM Users where Users.Name = '{name}'");
            //User userToReturn = null;
            //while (reader.Read())
            //{
            //   userToReturn = new User(name, $"{reader[0]}", (int)reader[1], $"{reader[2]}", $"{reader[3]}", (int)reader[4]);
            //}
            //reader.Close();
            //_connection.Close();
            //return userToReturn;

            //
            //          ABOVE (Reccomended code) BELOW (Actual code) 
            //

            SqlCommand cmdNew = new SqlCommand("SELECT Users.Password, Users.Points, Users.Classrooms, Users.Ranks, Users.TeamId, Users.Id FROM Users where Users.Name = '" + name + "'", _connection);
            cmdNew.CommandType = CommandType.Text;
            User userToReturn = null;
            try
            {
                _connection.Open();
                SqlDataReader reader = cmdNew.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("well dude the reader has some reading to do");
                    userToReturn = new User(name, (String)reader[0] + "", (int)reader[1], (String)reader[2] + "", (String)reader[3] + "", (int)reader[4], (int)reader[5]);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("=========================");
                Console.WriteLine(ex);
            }
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
