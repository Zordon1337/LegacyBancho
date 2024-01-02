using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyBancho.Handlers
{
    public class Static
    {
        /*
         * basic ui, probably gonna get replaced with bit modded
         * old osu website(like in anchor project)
         */
        public static string HandleIndex()
        {
            string Index = @"<pre><p style='color:purple'>
  _                                 ____                   _           
 | |                               |  _ \                 | |          
 | |     ___  __ _  __ _  ___ _   _| |_) | __ _ _ __   ___| |__   ___  
 | |    / _ \/ _` |/ _` |/ __| | | |  _ < / _` | '_ \ / __| '_ \ / _ \ 
 | |___|  __/ (_| | (_| | (__| |_| | |_) | (_| | | | | (__| | | | (_) |
 |______\___|\__, |\__,_|\___|\__, |____/ \__,_|_| |_|\___|_| |_|\___/ 
              __/ |            __/ |                                   
             |___/            |___/                                    
</p>

LegacyBancho(W.I.P) - Osu! b222 Server powered by C# and libHTTP library
LegacyBancho: <a href='https://github.com/Zordon1337/LegacyBancho'>https://github.com/Zordon1337/LegacyBancho</a>
libHTTP: <a href='https://github.com/Zordon1337/LibHTTP'>https://github.com/Zordon1337/LibHTTP</a>
Register: <a href='/register'>HERE</a></pre>

";
            return Index;
        }
        public static string HandleRegPage()
        {
            string Reg = @"
<div style='text-align:center;'>
<pre>
<form action='/register' method='POST'>
<p> username </p>
<input name='u'/>
<p> password </p>
<input name='p' type='password'/>
<br/>
<input type='submit' value='register'/>
</form>
</div>";
            return Reg;
        }
    }
    
}
