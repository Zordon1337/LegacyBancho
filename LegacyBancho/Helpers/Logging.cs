using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyBancho.Helpers
{
    public class Logging
    {
        public void LogError(string ERROR)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n[ERROR] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(ERROR);
        }
        public void LogInfo(string ERROR)
        {
            Console.Write("\n[Info] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(ERROR);
        }
    }
}
