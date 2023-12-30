using LegacyBancho.Helpers;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LegacyBancho
{
    public class DB
    {
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
        static Helpers.Logging logs = new Helpers.Logging();
        
        
        

        

        

    }
}
