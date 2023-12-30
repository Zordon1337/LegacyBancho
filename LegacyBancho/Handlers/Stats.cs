using MySqlConnector;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LegacyBancho.Handlers
{
    public class Stats
    {
        static Helpers.Logging logs = new Helpers.Logging();
        static Helpers.User user = new Helpers.User();
        public static string HandleStatoth(MySqlConnection connection, string u)
        {
            int totalplayerscore = Helpers.Calculate.GetPlayerTotalScore(connection, u); // getting it before any action cuz i don't want to trigger error
            int rank = user.GetPlayersRank(connection, u); // same as above
            if(connection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    if(connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                        
                    }
                    Thread.Sleep(1500);

                }
                catch (Exception ex)
                {
                    logs.LogError("Attempted to connect to the db, but error occured: " + ex.Message);
                    logs.LogInfo("Attempting to reconnect in 3 Sec");
                    Thread.Sleep(3000);
                    connection.Open();
                    Thread.Sleep(1500);
                }
            }
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE Username = @user;";
            command.Parameters.AddWithValue("@user", u);
            var reader = command.ExecuteReader();
            reader.Read();
            string prepared = "";
            if (reader.HasRows)
            {
                
                
                prepared = $"{totalplayerscore}|{reader["Accuracy"].ToString()}|unknown1|unknown2|{rank}|{reader["UserId"].ToString()}.png";
            }
            else
            {
                prepared = "Fail";
            }
            connection.Close();
            return prepared;

        }
    }
}
