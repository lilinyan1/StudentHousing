using System;
using BlowFishCS;

namespace StudentHousing
{
    public class Utility
    {
        public static string IntToString(int integer)
        {
            if (integer == int.MinValue)
                return string.Empty;
            else
                return integer.ToString();
        }

        public static string DateTimeToString(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
                return string.Empty;
            else
                return dateTime.ToShortDateString();
        }

	public static string EncryptString(string str)
	{
		BlowFish b = new BlowFish("04B915BA43FEB5B6");
		string encryptedstr = b.Encrypt_CBC(str);
		return encryptedstr;
	}
    }
}