using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
        public static bool CheckLogin(MySqlConnection connection, string u, string p)
        {
            connection.Open();

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
            connection.Open();
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
    }
}
