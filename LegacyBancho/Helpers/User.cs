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
        Helpers.Logging logs = new Helpers.Logging();
        public MySqlDataReader FindUserByUsername(MySqlConnection connection, string userName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT userId FROM users WHERE Username = @user";
                command.Parameters.AddWithValue("@user", userName);

                if (connection.State != ConnectionState.Open)
                    connection.Open();
                var result = command.ExecuteReader();
                try
                {
                    if (result.HasRows)
                    {
                        return result;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    logs.LogError("An error occured in FindUserByUsername()\n " + ex.Message);
                    result.Close();
                    return null;
                }

            }
        }
        public bool UpdateTotalScore(MySqlConnection connection, int uid, int NewScore)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"UPDATE `users` SET `score`=@score WHERE UserId = @uid";
                command.Parameters.AddWithValue("@score", NewScore);
                command.Parameters.AddWithValue("@uid", uid);

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    logs.LogError("An error occured in UpdateTotalScore()\n " + ex.Message);
                    return false;
                }

            }
        }
        public int GetPlayersRank(MySqlConnection connection, string username)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM `users` ORDER BY Score";
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return 0; // imposibble situation, but just in case someone manages to attempt to get rank without an account
                    int rank = 1;
                    while (reader.Read())
                    {
                        string userfromdb = reader["Username"].ToString();
                        if (userfromdb == username)
                            return rank;
                        else
                        {
                            rank++;
                        }
                    }
                    return -1; // there is no such user in db
                }
            }
        }
        public int GetDBUsersAmount(MySqlConnection connection)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();

            using (MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM users", connection))
            {
                int rows = Convert.ToInt32(command.ExecuteScalar());
                return rows;
            }
        }
    }
}
