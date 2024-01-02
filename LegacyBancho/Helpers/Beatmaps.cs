using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyBancho.Helpers
{
    public class Beatmaps
    { 
        public static int GetBeatmapStatus(MySqlConnection connection, string checksum)
        {
            /*
             * 
             * Let me explain what int should this return
             * -1 - map is not submitted(not found in db)
             *  0 - Map is pending 
             *  1 - Update available(not really planned to implement)
             *  2 - Ranked
             */
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM `beatmaps` WHERE checksum = @c";
                command.Parameters.AddWithValue("@c", checksum);
                
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return -1; // map not found in db
                    
                    while (reader.Read())
                    {
                        return Int32.Parse(reader["status"].ToString());
                    }
                    return -1; // there is no such user in db
                }
            }
        }
        public static bool DoesMapExist(MySqlConnection connection, string c)
        {
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting)
                connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM `beatmaps` WHERE checksum = @c";
                command.Parameters.AddWithValue("@c", c);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return false;
                    else
                        return true;

                }
            }
        }
    }
}
