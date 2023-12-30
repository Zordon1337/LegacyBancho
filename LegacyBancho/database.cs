using LegacyBancho.Helpers;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LegacyBancho
{
    public class database
    {
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
            }
            else
            {
                prepared = "Fail";
            }
            connection.Close();
            return prepared;

        }
        public static string HandleScores(MySqlConnection connection, string checksum)
        {
            string prepared = "";

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT * FROM scores WHERE Checksum = @c;";
                    command.Parameters.AddWithValue("@c", checksum);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            return "";

                        while (reader.Read())
                        {
                            prepared += $"{reader["onlineId"].ToString()}|{reader["playerName"].ToString()}|{reader["totalScore"].ToString()}|{reader["maxCombo"].ToString()}|{reader["count50"].ToString()}|{reader["count100"].ToString()}|{reader["count300"].ToString()}|{reader["countMiss"].ToString()}|{reader["countKatu"].ToString()}|{reader["countGeki"].ToString()}|{reader["perfect"].ToString()}|{reader["enabledMods"].ToString()}|{reader["UserID"].ToString()}|{reader["AvatarFileName"].ToString()}|{DateTime.Now}\n";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logs.LogError("Attempted to execute command,\nbut error occurred: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return prepared;
        }

        private static int FindUserIdByUsername(MySqlConnection connection, string userName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT userId FROM users WHERE Username = @user";
                command.Parameters.AddWithValue("@user", userName);

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        public static string WriteScores(MySqlConnection connection, ScoreSubmitStruct score)
        {
            try
            {
                int username_id = FindUserIdByUsername(connection, score.sUsername);

                if (username_id == -1)
                {
                    // Handle the case where the user is not found
                    return "user_not_found";
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO `scores`(`Checksum`, `onlineId`, `playerName`, `totalScore`, `maxCombo`, `count50`, `count100`, `count300`, `countMiss`, `countKatu`, `countGeki`, `perfect`, `enabledMods`, `UserID`, `AvatarFileName`) 
                            VALUES (@cs, @oid, @un, @score, @combo, @count50, @count100, @count300, @countMiss, @countKatu, @countGeki, @perfect, @enabledMods, @uid, @filename)";
                    command.Parameters.AddWithValue("@cs", score.fileChecksum);
                    command.Parameters.AddWithValue("@oid", username_id);
                    command.Parameters.AddWithValue("@un", score.sUsername);
                    command.Parameters.AddWithValue("@score", score.totalScore);
                    command.Parameters.AddWithValue("@combo", score.maxCombo);
                    command.Parameters.AddWithValue("@count50", score.count50);
                    command.Parameters.AddWithValue("@count100", score.count100);
                    command.Parameters.AddWithValue("@count300", score.count300);
                    command.Parameters.AddWithValue("@countMiss", score.countMiss);
                    command.Parameters.AddWithValue("@countKatu", score.countKatu);
                    command.Parameters.AddWithValue("@countGeki", score.countGeki);
                    command.Parameters.AddWithValue("@perfect", Calculate.CalculatePerfect(score));
                    command.Parameters.AddWithValue("@enabledMods", score.enabledMods);
                    command.Parameters.AddWithValue("@uid", username_id);
                    command.Parameters.AddWithValue("@filename", $"{username_id}.png");

                    if (score.pass)
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        command.ExecuteNonQuery();
                        return "success";
                    }
                    else
                    {
                        return "fail";
                    }
                }
            }
            catch (Exception ex)
            {
                logs.LogError("Error writing scores to the database: " + ex.Message);
                return "error";
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

    }
}
