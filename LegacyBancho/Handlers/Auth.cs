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
    public class Auth
    {
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
    }
}
