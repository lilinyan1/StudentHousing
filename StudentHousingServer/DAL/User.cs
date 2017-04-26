/*
* FILE:             User.cs
* PROJECT:          PROG2020 - Project Development - Capstone
* PROGRAMMER:       Becky Linyan Li, Matthew Cocca
* AVAILABLE DATE:   26-4-2017
* DESCRIPTION:      Contains reusable database request handler that 
*                   - create, get users
*                   - add/update bookmarks              
*/

using System;
using System.Collections.Generic;
using System.Data;
using StudentHousing.Dto;
using System.Linq;
using Utility;

namespace StudentHousing.DAL
{
	public class User : Dto.UserDto
	{
		public int Create()
		{
			var fieldsValue = string.Empty;
			var fieldsName = string.Empty;

			foreach (var p in this.GetType().GetProperties())
			{
				if (p.Name != "ID")
				{
					fieldsName += "[" + p.Name + "],";

					var tmp = p.GetValue(this, null).ToString();

					if (tmp == string.Empty)
					{
						tmp = "null";
					}

					fieldsValue += "'" + tmp + "',";
				}
			}

			if (fieldsName != string.Empty)
			{
				fieldsName = fieldsName.TrimEnd(',');
			}

			if (fieldsValue != string.Empty)
			{
				fieldsValue = fieldsValue.TrimEnd(',');
			}

			var ret = BaseDAL.InsertInto(fieldsName, "Users", fieldsValue);

			return ret;
		}

        public static User GetByID(int id)
        {
            string fieldsName = BaseDAL.GetProperties(typeof(UserDto));
            var dataRows = BaseDAL.SelectFrom(fieldsName, "Users", string.Format("[id] = {0}", id));

            if (dataRows != null && dataRows.Any())
            {
                var row = dataRows.First();
                return Load(row);
            }
            else
            {
                return null;
            }
        }

        private static User Load(System.Data.DataRow row)
        {
            return new User
            {
                ID = (int)row["id"],
                roleID = (int)row["roleID"],
                firstName = (string)row["firstName"],
                lastName = (string)row["lastName"],
                phone = BaseDAL.ToString(row["phone"]),
                email = (string)row["email"],
                pass = (string)row["pass"]
            };
        }

        // using email address to login
        public static User Login(string inEmail, string inPass)
		{
			var fieldsName = string.Empty;
			var ur = new User();

			//fieldsName = "*";
			foreach (var p in ur.GetType().GetProperties())
			{
				fieldsName += "[" + p.Name + "],";
			}

			if (fieldsName != string.Empty)
			{
				fieldsName = fieldsName.TrimEnd(',');
			}

			var dataRows = BaseDAL.SelectFrom(fieldsName, "Users", string.Format("[email] = '{0}'", inEmail));

			if (dataRows != null && dataRows.Any())
			{
				var row = dataRows.First();
				var user = new User
				{
					ID = (int)row["ID"],
					roleID = (int)row["roleID"],
					firstName = (string)row["firstName"],
					lastName = (string)row["lastName"],
					phone = (string)row["phone"],
					email = (string)row["email"],
					pass = (string)row["pass"]
				};

                string decryptedUP = Utility.Decryption.DecryptString(user.pass);
                string decryptedIP = Utility.Decryption.DecryptString(inPass);

                if (decryptedIP != decryptedUP)
				{
                    // password does not match
                    var user2 = new User
                    {
                        ID = 0,
                        roleID = 0,
                        firstName = null,
                        lastName = null,
                        phone = null,
                        email = null,
                        pass = null
                    };
                    return user2;
				}
				else
				{
                    // return the found user
					return user;
				}
			}
			else
			{
				// could not find the user record by the email address
				return null;
			}

		}

		public int CreateBookmark(int propertyID, string comment)
		{
			var fieldsValue = string.Empty;
			var fieldsName = string.Empty;

			DateTime today = DateTime.Today;
			string sqlFormattedDate = today.ToString("yyyy-MM-dd HH:mm:ss.fff");

			foreach (var property in typeof(BookmarkDto).GetProperties())
			{
				fieldsName += string.Format("[{0}],", property.Name);
			}

			if (fieldsName != string.Empty)
			{
				fieldsName = fieldsName.TrimEnd(',');
			}

			fieldsValue = string.Format("'{0}','{1}','{2}','{3}'", this.ID, propertyID, sqlFormattedDate, comment);

			var ret = BaseDAL.InsertInto(fieldsName, "Bookmark", fieldsValue);

			return ret;
		}

		public List<BookmarkDto> GetBookmark()
		{
			var fieldsName = string.Empty;

			fieldsName = "*";

			var dataRows = BaseDAL.SelectFrom(fieldsName, "Bookmark", string.Format("[UserID] = '{0}'", this.ID));

			if (dataRows != null)
			{
				List<BookmarkDto> bmlist = new List<BookmarkDto>();

				foreach (DataRow row in dataRows)
				{
					var bookmark = new BookmarkDto
					{
						userID = (int)row["userID"],
						propertyID = (int)row["propertyID"],
						bookmarkDate = (DateTime)row["bookmarkDate"],
						comment = (string)row["comment"]
					};
					bmlist.Add(bookmark);
				}

				return bmlist;
			}
			else
			{
				// could not find any bookmark
				return null;
			}
		}

	}
}
