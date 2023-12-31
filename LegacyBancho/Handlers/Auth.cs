using LegacyBancho.Helpers;
using MySqlConnector;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LegacyBancho.Handlers
{
    public class Auth
    {
        static Helpers.Logging logs = new Helpers.Logging();
        static Helpers.User user = new Helpers.User();
        public static bool CheckLogin(MySqlConnection connection, string u, string p)
        {
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                connection.Open();
            /*try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                logs.LogError("Attempted to connect to the db, but error occured: " + ex.Message);
                logs.LogInfo("Attempting to reconnect in 5 Sec");
                Thread.Sleep(5000);
                connection.Open();
            }*/

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE Username = @user AND Password = @pass;";
            command.Parameters.AddWithValue("@user", u);
            command.Parameters.AddWithValue("@pass", p);

            var reader = command.ExecuteReader();
            bool bCorrect = reader.HasRows;

            connection.Close();

            return bCorrect;
        }
        public static string CreateAccount(MySqlConnection connection, string u, string p)
        {
            try
            {
                if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                    connection.Open();

                int userId = user.GetDBUsersAmount(connection) + 1;

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                INSERT INTO `users`(`UserId`, `Accuracy`, `CurrentRank`, `Score`, `Password`, `Username`)
                VALUES (@uid, '1', '0', '0', @password, @username)";
                    command.Parameters.AddWithValue("@uid", userId);
                    command.Parameters.AddWithValue("@password", Helpers.Calculate.ComputeMD5Hash(p));
                    command.Parameters.AddWithValue("@username", u);

                    command.ExecuteNonQuery();
                    return "Account successfully created. You can now log in with osu! b222";
                }
            }
            catch (Exception ex)
            {
                return "Something went wrong... " + ex.Message;
            }
        }
    }

}
