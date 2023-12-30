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
    }
}
