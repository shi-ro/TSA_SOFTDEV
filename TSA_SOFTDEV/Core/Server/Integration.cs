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
            _connection.Close();
            return reader;
        }

        public static void ExecuteAddUser(User bob)
        {

            // ExecuteQuery($"INSERT INTO[dbo].[Users] VALUES('{bob.getName()}', '{bob.getPassword()}', {bob.getPoints()}, '{bob.getClassrooms()}', '{bob.getRanks()}', {bob.getTeamId()})");
            // everything below can be replaced with above statement

            SqlCommand cmdNew = new SqlCommand("INSERT INTO[dbo].[Users] VALUES('" + bob.getName() + "', '" + bob.getPassword() + "', " + bob.getPoints() + ", '" + bob.getClassrooms() + "', '" + bob.getRanks() + "', " + bob.getTeamId() + ")", _connection);
            cmdNew.CommandType = CommandType.Text;
            _connection.Open();
            cmdNew.ExecuteNonQuery();
            _connection.Close();
        }

        public static User ExecuteGetUser(string name) //using a name, get a user
        {
            
            SqlCommand cmdNew = new SqlCommand("SELECT Users.Password, Users.Points, Users.Classrooms, Users.Ranks, Users.TeamId FROM Users where Users.Name = " + name, _connection);
            cmdNew.CommandType = CommandType.Text;
            User userToReturn = null;
            _connection.Open();
            SqlDataReader reader = cmdNew.ExecuteReader();

            // above code can be replaced with 
            // SqlDataReader reader = ExecuteRead($"SELECT Users.Password, Users.Points, Users.Classrooms, Users.Ranks, Users.TeamId FROM Users where Users.Name = {name}");

            while (reader.Read())
            {
                userToReturn = new User(name, (String)reader[0], (int)reader[1], (String)reader[2], (String)reader[3], (int)reader[4]);
            }

            //line below probably needs to be _connection.Close() instead of reader.Close()
            reader.Close();
            return userToReturn; 
        }
    }
}
