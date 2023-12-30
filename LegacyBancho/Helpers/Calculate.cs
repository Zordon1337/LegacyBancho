using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LegacyBancho.Helpers
{
    public class Calculate
    {
        /* 
         * Not sure if it actually works correctly, because at time of writing this
         * im unable to get SS lol
         * UPDATE: well it's SS is actually X not SS
         */
        static Helpers.Logging logs = new Helpers.Logging();
        public static bool CalculatePerfect(ScoreSubmitStruct score)
        {

            return score.ranking == "X";

        }
        public static int GetPlayerTotalScore(MySqlConnection connection, string username)
        {
            if (connection.State != ConnectionState.Open && connection.State != ConnectionState.Connecting && connection.State != ConnectionState.Executing && connection.State != ConnectionState.Fetching)
            {
                connection.Open();
            }
            if (connection.State == ConnectionState.Connecting && connection.State == ConnectionState.Executing && connection.State == ConnectionState.Fetching)
            {
                Thread.Sleep(2500);
            }
            int _totalScore = 0;
            try
            {
                using(var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT * FROM scores WHERE playerName = @player";
                    command.Parameters.AddWithValue("@player", username);

                    using(var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            return 0;

                        while(reader.Read())
                        {
                            _totalScore += Int32.Parse(reader["totalScore"].ToString());
                        }
                        return _totalScore;
                    }
                }
            } catch (Exception e)
            {
                logs.LogError("An Error occured in Function GetPlayerTotalScore(): \n" + e.Message);
                return 0;
            }
        }
        public static int CalculateScore(MySqlConnection connection, string username)
        {
            int TotalScore = GetPlayerTotalScore(connection, username);
            if(TotalScore > 2) {
                // it needs to be divided by 2 otherwise it would be easy to endup with maximum number of unsigned int
                return (TotalScore) / 2;
            } else
            {
                // we do NOT want to turn our cpu into Galaxy Note 7(jk)
                return TotalScore;
            }
        }
    }
}

