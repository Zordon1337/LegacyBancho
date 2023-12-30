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
                    if(result.HasRows)
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

                if(connection.State != ConnectionState.Open)
                    connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                } catch (Exception ex)
                {
                    logs.LogError("An error occured in UpdateTotalScore()\n " + ex.Message);
                    return false;
                }

            }
        }
    }
}
