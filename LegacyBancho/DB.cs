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
        
        public static void Init(MySqlConnection connection)
        {
           
            
                
                CreateBeatmapsTable(connection);
                CreateScoresTable(connection);
                CreateUsersTable(connection);
                


            
        }
        private static void CreateUsersTable(MySqlConnection connection)
        {
            if ((connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting))
            {
                Thread.Sleep(500);
                connection.Open();
            }
            var command = connection.CreateCommand();
            command.CommandText = (@"
                CREATE TABLE IF NOT EXISTS `users` (
                `UserId` int(11) NOT NULL,
                `Accuracy` double NOT NULL,
                `CurrentRank` int(11) NOT NULL,
                `Score` int(11) NOT NULL,
                `Password` text NOT NULL,
                `Username` text NOT NULL
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
                COMMIT;");
            command.ExecuteNonQuery();
            connection.Close();
        }
        private static void CreateBeatmapsTable(MySqlConnection connection)
        {
            if ((connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting))
            {
                Thread.Sleep(500);
                connection.Open();
            }
            var command = connection.CreateCommand();
            command.CommandText = (@"
                CREATE TABLE IF NOT EXISTS `beatmaps` (
                `id` int(11) NOT NULL,
                `checksum` text NOT NULL,
                `status` int(11) NOT NULL,
                `submit_date` text NOT NULL,
                `ranked_data` text NOT NULL,
                `creator` text NOT NULL
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
                REPLACE INTO `beatmaps`  (`id`, `checksum`, `status`, `submit_date`, `ranked_data`, `creator`) VALUES
                (1, 'cf8bd375f2708f152562a919247ba09a', 2, '2007-XX-XX', '2007-XX-XX', 'peppy'),
                (2, '974b72f33a25bd5ef297bd8682d7fa79', 2, '', '', ''),
                (3, 'ea0df9f890e7e9e7ad4d3862a7823359', 2, '', '', ''),
                (4, '774861583d38346a3876ade4116ebbc0', 2, '', '', '');
                COMMIT;");
            command.ExecuteNonQuery();
            connection.Close();
        }
        private static void CreateScoresTable(MySqlConnection connection)
        {
            if ((connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting))
            {
                Thread.Sleep(500);
                connection.Open();
            }
            var command = connection.CreateCommand();
            command.CommandText = (@"
                CREATE TABLE IF NOT EXISTS `scores` (
                `Checksum` text NOT NULL,
                `onlineId` int(11) NOT NULL,
                `playerName` text NOT NULL,
                `totalScore` int(11) NOT NULL,
                `maxCombo` int(11) NOT NULL,
                `count50` int(11) NOT NULL,
                `count100` int(11) NOT NULL,
                `count300` int(11) NOT NULL,
                `countMiss` int(11) NOT NULL,
                `countKatu` int(11) NOT NULL,
                `countGeki` int(11) NOT NULL,
                `perfect` tinyint(1) NOT NULL,
                `enabledMods` int(11) NOT NULL,
                `UserID` int(11) NOT NULL,
                `AvatarFileName` text NOT NULL
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
                COMMIT;");
            command.ExecuteNonQuery();
            connection.Close();

        }

        

        

    }
}
