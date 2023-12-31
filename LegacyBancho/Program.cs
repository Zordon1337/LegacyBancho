﻿using LegacyBancho.Helpers;
using LibHTTP;
using MySqlConnector;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace LegacyBancho
{
    /*
     * Thank you for looking into this code
     * it's mess but it works
     * 
     * Made by Zordon 2023-2023
     */
    internal class Program
    {

        static void Main(string[] args)
        {
            // initalizing logs
            Helpers.Logging log = new Helpers.Logging();
            // ascii art :)
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
              _                                 ____                   _           
             | |                               |  _ \                 | |          
             | |     ___  __ _  __ _  ___ _   _| |_) | __ _ _ __   ___| |__   ___  
             | |    / _ \/ _` |/ _` |/ __| | | |  _ < / _` | '_ \ / __| '_ \ / _ \ 
             | |___|  __/ (_| | (_| | (__| |_| | |_) | (_| | | | | (__| | | | (_) |
             |______\___|\__, |\__,_|\___|\__, |____/ \__,_|_| |_|\___|_| |_|\___/ 
                          __/ |            __/ |                                   
                         |___/            |___/            
            
            ");
            Console.ForegroundColor = ConsoleColor.White;
            log.LogInfo("Initalzing HTTP Server");
            // Initalizing HTTP Server
            HTTP http = new HTTP();
            // Creating Thread so it will continue executing rest of the script(including http.GET)
            Thread ServerThread = new Thread(() => http.ListenMA(new string[] { "http://127.0.0.1:80/", "http://localhost:80/"}));
            // starting the thread
            log.LogInfo("Starting HTTP server thread");
            ServerThread.Start();
            // initalizing db connection
            log.LogInfo("Initalzing DB connection");
            MySqlConnection connection = new MySqlConnection(DB.builder.ConnectionString);
            DB.Init(connection);
            http.get("/web/osu-login.php", "text/html", queryparams =>
            {
                try
                {
                    string u = null; // user(username)
                    string p = null; // user password
                    if (queryparams.TryGetValue("username", out u) && queryparams.TryGetValue("password", out p))
                    {
                        
                        if (Handlers.Auth.CheckLogin(connection, u,p))
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
                        return "You forgot about args";
                    }
                } catch(Exception e) { return e.Message; }
            });
            http.get("/web/osu-statoth.php", "text/html", queryparams =>
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
                    string PreparedString = Handlers.Stats.HandleStatoth(connection, u);
                    return PreparedString;
                }
                else
                {
                    return "Fail";
                }
            });
            http.get("/rating/ingame-rate.php", "text/html", queryparams =>
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
            http.get("/web/osu-getscores2.php", "text/html", queryparams =>
            {
                string c = null; // beatmap checksum
                //string f = null; // beatmap filename

                // onlineId(int)|playerName(string?)|totalScore(int)|maxCombo(int)|count50(int)|count100(int)|count300(int)|countMiss(int)|countKatu(int)|countGeki(int)|perfect(bool)|enabledMods(int)|user.Id(int)|user.AvatarFilename(String?)|date(DateTime)
                if (queryparams.TryGetValue("c", out c))
                {
                    return Handlers.Score.GetScores(connection, c);
                } else
                {
                    return "NoArgs";
                }
                
            });
            http.post("/web/osu-submit.php", "text/html", queryparams =>
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
                    Handlers.Score.WriteScores(connection, data);
                    return "ok"; // also client doesn't care what you gonna response with

                } else
                {
                    return "parameters missing";
                }
            });
            http.get("/forum/download.php", "application/octet-stream", queryparams =>
            {
                if(queryparams.TryGetValue("avatar", out var avatar))
                {
                    if(avatar != null)
                    {
                        //return Convert.ToBase64String(System.IO.File.ReadAllBytes($"./avatars/{avatar}"));
                        return Convert.ToBase64String(new byte[] { 0x0,0x1}); // random shit to just not crash server(the greatest(worst*) dev in the universe
                    } else
                    {
                        return Convert.ToBase64String(Encoding.UTF8.GetBytes("server does NOT know what are you yapping about"));
                    }
                } else
                {
                    return Convert.ToBase64String(Encoding.UTF8.GetBytes("server does NOT know what are you yapping about"));
                }
            });
            http.get("/", "text/html", queryparams =>
            {
                return Handlers.Static.HandleIndex();
            });
            http.get("/register", "text/html", queryparams =>
            {
                return Handlers.Static.HandleRegPage();
            });
            http.post("/register", "text/plain", (POSTPARAM) =>
            {
                POSTPARAM.TryGetValue("u", out var u);
                POSTPARAM.TryGetValue("p", out var p);
                /*return ($"user:{u}\npass:{Calculate.ComputeMD5Hash(p)}");*/
                return Handlers.Auth.CreateAccount(connection, u, p);

            });
            /*http.post("/web/osu-bmsubmit-getid2.php", "application/x-www-form-urlencoded", param =>
            {
                // Access GET values
                param.TryGetValue("u", out var u);
                param.TryGetValue("p", out var p);
                param.TryGetValue("r", out var r);
                param.TryGetValue("osu", out var file);

                
                Console.WriteLine($"u: {u}, p: {p}, r: {r}, file: {file}");
                Console.WriteLine(file);
                return Handlers.BeatmapEditor.HandleMapUpload(connection, u, p, r);


            });*/
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

        public override string ToString()
        {
            return $"fileChecksum: {fileChecksum}, sUsername: {sUsername}, onlineScoreChecksum: {onlineScoreChecksum}, " +
                   $"count300: {count300}, count100: {count100}, count50: {count50}, countGeki: {countGeki}, " +
                   $"countKatu: {countKatu}, countMiss: {countMiss}, totalScore: {totalScore}, maxCombo: {maxCombo}, " +
                   $"perfect: {perfect}, ranking: {ranking}, enabledMods: {enabledMods}, pass: {pass}";
        }
    }



}
