using LibHTTP;
using MySqlConnector;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace LegacyBancho
{
    
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initalizing HTTP Server
            HTTP http = new HTTP();
            // Creating Thread so it will continue executing rest of the script(including http.GET)
            Thread ServerThread = new Thread(() => http.ListenMA(new string[] { "http://127.0.0.1:80/", "http://localhost:80/","http://osu.ppy.sh:80/" }));
            // starting the thread
            ServerThread.Start();
            // initalizing db connection
            MySqlConnection connection = new MySqlConnection(database.builder.ConnectionString);
            // initalizing logs
            Helpers.Logging log = new Helpers.Logging();
            http.Get("/web/osu-login.php", "text/html", queryparams =>
            {
                try
                {
                    string u = null; // user(username)
                    string p = null; // user password
                    if (queryparams.TryGetValue("username", out u) && queryparams.TryGetValue("password", out p))
                    {
                        
                        if (database.CheckLogin(connection, u,p))
                        {
                            return "1";
                        }
                        else
                        {
                            return "0";
                        }



                    }
                    else
                    {
                        return "wtf";
                    }
                } catch(Exception e) { return e.Message; }
            });
            http.Get("/web/osu-statoth.php", "text/html", queryparams =>
            {
                string u = null; // username
                string c = null; // it is some sort of md5 encrypted string which is combined AcutalName(idk peppy meant with this) and string "prettyplease!!!"
                //int Score = 1337;
                //float Accuracy = 1f; // accuracy is multiplied by osu client by 100, so for example 100% accuracy is 1f and 1% accuracy is 0.01f etc.
                //string unknown1 = " "; // osu requires it but doesn't use it so i don't really know for what purpose is that
                //string unknown2 = " "; // same as above
                //int CurrentRank = 1; // idk how it works yet
                //int UserID = 1; // used to fetch avatar but i don't know how to implement it yet
                if (queryparams.TryGetValue("u", out u) && queryparams.TryGetValue("c", out c))
                {
                    string PreparedString = database.HandleStatoth(connection, u);
                    return PreparedString;
                }
                else
                {
                    return "Fail";
                }
            });
            http.Get("/rating/ingame-rate.php", "text/html", queryparams =>
            {
                string u = null; // user(username)
                string p = null; // user password
                string c = null; // beatmap checksum
                if(queryparams.TryGetValue("u", out u)&&queryparams.TryGetValue("p", out p) &&queryparams.TryGetValue("c", out c))
                {
                    string v = null; // it's map rating in stars, there is 10 stars max
                    if(queryparams.TryGetValue("v", out v))
                    {
                        // submits the beatmap rate
                        Console.WriteLine($"\n u: {u}\n p: {p}\n c: {c}\n v: {v}");
                        return "ok"; // tbh it requires any response, it can be even 204 result code
                    } else
                    {
                        // checks if you can/already rated the map
                        // if you didn't it responded with ok, if not it responded with anything other than ok
                        Console.WriteLine($"\n u: {u}\n p: {p}\n c: {c}\n");
                        return "ok";
                    }
                }
                return "Not OK";
            });
            http.Get("/web/osu-getscores2.php", "text/html", queryparams =>
            {
                string c = null; // beatmap checksum
                string f = null; // beatmap filename

                // onlineId(int)|playerName(string?)|totalScore(int)|maxCombo(int)|count50(int)|count100(int)|count300(int)|countMiss(int)|countKatu(int)|countGeki(int)|perfect(bool)|enabledMods(int)|user.Id(int)|user.AvatarFilename(String?)|date(DateTime)
                if (queryparams.TryGetValue("c", out c))
                {
                    return database.HandleScores(connection, c);
                } else
                {
                    return "NoArgs";
                }
                
            });
            http.Get("/web/osu-submit.php", "text/html", queryparams =>
            {
                string score = null; // look at BeatMapSendData Struct
                string pass = null; // hashed password;
                if(queryparams.TryGetValue("score", out score)&&queryparams.TryGetValue("pass",out pass))
                {
                    string[] _splitted = score.Split(':');
                    ScoreSubmitStruct data = new ScoreSubmitStruct
                    {
                        fileChecksum = _splitted[0],
                        sUsername = _splitted[1],
                        onlineScoreChecksum = _splitted[2],
                        count300 = Int32.Parse(_splitted[3]),
                        count100 = Int32.Parse(_splitted[4]),
                        count50 = Int32.Parse(_splitted[5]),
                        countGeki = Int32.Parse(_splitted[6]),
                        countKatu = Int32.Parse(_splitted[7]),
                        countMiss = Int32.Parse(_splitted[8]),
                        totalScore = Int32.Parse(_splitted[9]),
                        maxCombo = Int32.Parse(_splitted[10]),
                        perfect = bool.Parse(_splitted[11]),
                        ranking = _splitted[12],
                        enabledMods = Int32.Parse(_splitted[13]),
                        pass = bool.Parse(_splitted[14])

                    };
                    Console.WriteLine(data);
                    return "ok"; // also client doesn't care what you gonna response with

                } else
                {
                    return "parameters missing";
                }
            });
            http.Get("/forum/download.php", "application/octet-stream", queryparams =>
            {
                if(queryparams.TryGetValue("avatar", out var avatar))
                {
                    if(avatar != null)
                    {
                        //return Convert.ToBase64String(System.IO.File.ReadAllBytes($"./avatars/{avatar}"));
                        return Convert.ToBase64String(System.IO.File.ReadAllBytes($"./avatars/1.png"));
                    } else
                    {
                        return Convert.ToBase64String(Encoding.UTF8.GetBytes("server does NOT know what are you yapping about"));
                    }
                } else
                {
                    return Convert.ToBase64String(Encoding.UTF8.GetBytes("server does NOT know what are you yapping about"));
                }
            });

            Console.ReadKey(); // we don't want to auto-close the server after initalizing
        }
    }
    public struct ScoreSubmitStruct
    {
        public string fileChecksum; // probably replay checksum, this one i know how to check but i will be too lazy to fix
        public string sUsername; // username
        public string onlineScoreChecksum; // score checksum, idk how to check it
        public int count300; // 300's
        public int count100; // 100's
        public int count50; // 50's
        public int countGeki; // 300 but with that weird chinese letter
        public int countKatu; // 100 but with that weird chinese letter
        public int countMiss; // Misses
        public int totalScore; // total score
        public int maxCombo; // max combo
        public bool perfect; // is Perfect(SS)
        public string ranking; // ranking grade(X, S, A, B, C, F, D, XH, SH) this does not affect leaderboard, because the leaderboard grade is calculated by client itself
        public int enabledMods; // enabled mods
        public bool pass; // passed?
    }
    
    
}
