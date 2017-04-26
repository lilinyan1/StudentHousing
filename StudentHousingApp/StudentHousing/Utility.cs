/*
* FILE:             Utility.cs
* PROJECT:          PROG2020 - Project Development - Capstone
* PROGRAMMER:       Becky Linyan Li, Xingguang Zhen
* AVAILABLE DATE:   26-4-2017
* DESCRIPTION:      A container for reusable utilities
*/
using System;
using BlowFishCS;

namespace StudentHousing
{
    public class Utility
    {
        /// <summary>
        /// Convert int to string
        /// </summary>
        /// <param name="integer"></param>
        /// <returns></returns>
        public static string IntToString(int integer)
        {
            if (integer == int.MinValue)
                return string.Empty;
            else
                return integer.ToString();
        }

        /// <summary>
        /// Convert datetime to string
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
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