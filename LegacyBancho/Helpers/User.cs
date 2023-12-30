using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyBancho.Helpers
{
    public class User
    {
        public MySqlDataReader FindUserByUsername(MySqlConnection connection, string userName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT userId FROM users WHERE Username = @user";
                command.Parameters.AddWithValue("@user", userName);

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var result = command.ExecuteReader();
                return result;
            }
        }
    }
}
