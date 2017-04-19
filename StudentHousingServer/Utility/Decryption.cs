using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlowFishCS;

namespace Utility
{
    public class Decryption
    {
        public static string DecryptString(string str)
        {
            BlowFish b = new BlowFish("04B915BA43FEB5B6");
            return b.Decrypt_CBC(str);
        }
    }
}
