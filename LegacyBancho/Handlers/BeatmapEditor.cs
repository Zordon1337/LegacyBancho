using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyBancho.Handlers
{
    public class BeatmapEditor
    {
        public static string HandleMapUpload(MySqlConnector.MySqlConnection connection, string u, string p, string unknownflag)
        {
            if(Handlers.Auth.CheckLogin(connection,u,p))
            {
                return "ok\n1\n2\ntest\nmap.osz";
            } else
            {
                return "Account verification failed";
            }
        }
    }
}
