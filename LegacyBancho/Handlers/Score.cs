using LegacyBancho.Helpers;
using MySqlConnector;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyBancho.Handlers
{
    public class Score
    {
        static Helpers.User User = new Helpers.User();
        static Helpers.Logging logs = new Helpers.Logging();
        public static string GetScores(MySqlConnection connection, string checksum)
        {
            string prepared = "";

            try
            {
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                    connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT * FROM scores WHERE Checksum = @c;";
                    command.Parameters.AddWithValue("@c", checksum);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            return "";

                        if (Helpers.Beatmaps.GetBeatmapStatus(connection, checksum) != 2)
                            prepared += Helpers.Beatmaps.GetBeatmapStatus(connection,checksum) + "\n";
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
        public static string WriteScores(MySqlConnection connection, ScoreSubmitStruct score)
        {
            try
            {
                var user = User.FindUserByUsername(connection, score.sUsername);
                int user_id = 0;
                if (user != null && user.Read())
                {
                    user_id = Int32.Parse(user["userId"].ToString());
                    connection.Close();
                }
                else
                {
                    return "user_not_found";
                }
                if(connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                    connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO `scores`(`Checksum`, `onlineId`, `playerName`, `totalScore`, `maxCombo`, `count50`, `count100`, `count300`, `countMiss`, `countKatu`, `countGeki`, `perfect`, `enabledMods`, `UserID`, `AvatarFileName`) 
                            VALUES (@cs, @oid, @un, @score, @combo, @count50, @count100, @count300, @countMiss, @countKatu, @countGeki, @perfect, @enabledMods, @uid, @filename)";
                    command.Parameters.AddWithValue("@cs", score.fileChecksum);
                    command.Parameters.AddWithValue("@oid", user_id);
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
                    command.Parameters.AddWithValue("@uid", user_id);
                    command.Parameters.AddWithValue("@filename", $"{user_id}.png");

                    if (score.pass)
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();
                        if(Beatmaps.GetBeatmapStatus(connection,score.fileChecksum) == 2)
                        {
                            command.ExecuteNonQuery();
                            return "success";
                        } else
                        {
                            return "fail";
                        }
                        
                        
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
