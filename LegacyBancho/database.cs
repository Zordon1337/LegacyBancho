using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LegacyBancho
{
    public class database {
        public static string username = "root";
        public static string password = "";
        public static string db = "legacybancho";
        public static string server = "127.0.0.1";
        public static MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
        {
            Server = server,
            UserID = username,
            Password = password,
            Database = db,
        };
        static Helpers.Logging logs = new Helpers.Logging();
        public static bool CheckLogin(MySqlConnection connection, string u, string p)
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                logs.LogError("Attempted to connect to the db, but error occured: " + ex.Message);
                logs.LogInfo("Attempting to reconnect in 5 Sec");
                Thread.Sleep(5000);
                connection.Open();
            }

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE Username = @user AND Password = @pass;";
            command.Parameters.AddWithValue("@user", u);
            command.Parameters.AddWithValue("@pass", p);

            var reader = command.ExecuteReader();
            bool bCorrect = reader.HasRows;

            connection.Close();

            return bCorrect;
        }
        public static string HandleStatoth(MySqlConnection connection, string u)
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                logs.LogError("Attempted to connect to the db, but error occured: " + ex.Message);
                logs.LogInfo("Attempting to reconnect in 5 Sec");
                Thread.Sleep(5000);
                connection.Open();
            }
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE Username = @user;";
            command.Parameters.AddWithValue("@user", u);
            var reader = command.ExecuteReader();
            reader.Read();
            string prepared = "";
            if (reader.HasRows)
            {
                 prepared = $"{reader["Score"].ToString()}|{reader["Accuracy"].ToString()}|unknown1|unknown2|{reader["CurrentRank"].ToString()}|{reader["UserId"].ToString()}.png";
            } else
            {
                prepared = "Fail";
            }
            connection.Close();
            return prepared;

        }
        public static string HandleScores(MySqlConnection connection, string checksum)
        {
            try
            {
                connection.Open();
            } catch (Exception ex)
            {
                logs.LogError("Attempted to connect to the db,\nbut error occured: " + ex.Message);
                logs.LogInfo("Attempting to reconnect in 5 Sec");
                Thread.Sleep(5000);
                connection.Open();
            }
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM scores WHERE Checksum = @c;";
            command.Parameters.AddWithValue("@c", checksum);
            
            string prepared = "";
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (!reader.HasRows)

                        return "NoRows";
                    while (reader.HasRows)
                    {
                        prepared += $"{reader["onlineId"].ToString()}|{reader["playerName"].ToString()}|{reader["totalScore"].ToString()}|{reader["maxCombo"].ToString()}|{reader["count50"].ToString()}|{reader["count100"].ToString()}|{reader["count300"].ToString()}|{reader["countMiss"].ToString()}|{reader["countKatu"].ToString()}|{reader["countGeki"].ToString()}|{reader["perfect"].ToString()}|{reader["enabledMods"].ToString()}|{reader["UserID"].ToString()}|{reader["AvatarFileName"].ToString()}\n";
                    }
                }
            } catch(Exception ex)
            {
                logs.LogError("Attempted to execute command,\nbut error occured: " + ex.Message);
            }
            
            
            connection.Close();
            return prepared;
        }
    }
}
