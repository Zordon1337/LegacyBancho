using LibHTTP;
using System;
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
            http.Get("/web/osu-login.php", "text/html", queryparams => "1");
            http.Get("/web/osu-statoth.php", "text/html", queryparams =>
            {
                string u = null; // username
                string c = null; // it is some sort of md5 encrypted string which is combined AcutalName(idk peppy meant with this) and
                int Score = 1337;
                float Accuracy = 1f; // accuracy is multiplied by osu client by 100, so for example 100% accuracy is 1f and 1% accuracy is 0.01f etc.
                string unknown1 = " "; // osu requires it but doesn't use it so i don't really know for what purpose is that
                string unknown2 = " "; // same as above
                int CurrentRank = 1; // idk how it works yet
                int UserID = 1; // used to fetch avatar but i don't know how to implement it yet
                if (queryparams.TryGetValue("u", out u) && queryparams.TryGetValue("c", out c))
                {
                    string PreparedString = $"{Score}|{Accuracy}|{unknown1}|{unknown2}|{CurrentRank}|{UserID}";
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
                        
                        Console.WriteLine($"\n u: {u}\n p: {p}\n c: {c}\n v: {v}");
                        return "ok";
                    } else
                    {
                        // submiting score probably
                        Console.WriteLine($"\n u: {u}\n p: {p}\n c: {c}\n");
                        return "ok";
                    }
                }
                return "Not OK";
            });
            http.Get("/web/osu-getscores2.php", "text/html", queryparams =>
            {
                // onlineId(int)|playerName(string?)|totalScore(int)|maxCombo(int)|count50(int)|count100(int)|count300(int)|countMiss(int)|countKatu(int)|countGeki(int)|perfect(bool)|enabledMods(int)|user.Id(int)|user.AvatarFilename(String?)|date(DateTime)
                string result = "1|Zordon|1337|420|0|0|420|0|0|0|true|8|1|avatar.png|0\n";
                for(int i = 0; i <= 250;i++)
                {
                    result += $"{new Random().Next(1,2500)}|{new Random().Next(99999, 99999999)}|1337|420|0|0|420|0|0|0|true|8|{new Random().Next(1, 2500)}|avatar.png|0\n";
                }
                return result;
            });


            Console.ReadKey(); // we don't want to auto-close the server after initalizing
        }
    }
}
