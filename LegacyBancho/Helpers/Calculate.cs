using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyBancho.Helpers
{
    public class Calculate
    {
        /* 
         * Not sure if it actually works correctly, because at time of writing this
         * im unable to get SS lol
         */
        public static bool CalculatePerfect(ScoreSubmitStruct score)
        {

            return score.ranking == "X";

        }
            
    }
}
