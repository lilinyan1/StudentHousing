using System;
using System.Collections.Generic;
using System.Data;
using StudentHousing.Dto;

namespace StudentHousing.DAL
{
	public class User:Dto.UserDto
	{
		public int Create()
		{
			var fieldsValue = string.Empty;
			var fieldsName = string.Empty;

			foreach (var p in this.GetType().GetProperties())
			{
				if (p.Name != "ID")
				{
					fieldsName += "[" + p.Name + "]";

					var tmp = p.GetValue(this, null).ToString();

					// what if the value is number's minvalue or date's minvalue
					if (tmp == string.Empty)
					{
						tmp = "null";
					}

					fieldsValue += tmp;

					if (p.Name != "pass")
					{
						fieldsName += ",";
						fieldsValue += ",";
					}
				}
			}

			var ret = DAL.InsertInto(fieldsName, "Users", fieldsValue);

			return ret;
		}

		// using email address to login
		public static User Login(string inEmail, string inPass)
		{
			var fieldsName = string.Empty;
			var ur = new User();

			//fieldsName = "*";
			foreach (var p in ur.GetType().GetProperties())
			{
				fieldsName += "[" + p.Name + "]";
				if (p.Name != "pass")
				{
					fieldsName += ",";
				}
			}

			var dataTable = DAL.SelectFrom(fieldsName, "Users", "[email]", inEmail);

			if (dataTable != null)
			{
				var row = dataTable.Select()[0];
				var user = new User
				{
					ID = (int)row["ID"],
					roldID = (int)row["roldID"],
					firstName = (string)row["firstName"],
					lastName = (string)row["lastName"],
					phone = (string)row["phone"],
					email = (string)row["email"],
					pass = (string)row["pass"]
				};

				if (inPass != user.pass)
				{
					// password does not match, maybe return sth else here
					return null;
				}
				else
				{
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

			foreach (var property in typeof(BookmarkDto).GetProperties())
			{
				fieldsName += string.Format("[{0}],", property.Name);
			}
			if (fieldsName != string.Empty)
			{
				fieldsName.TrimEnd(',');
			}

			//fieldsName = "[UserID],[propertyID],[bookmarkDate],[comment]";
			fieldsValue = string.Format("{0},{1},{2},{3}", this.ID, propertyID, today.ToString("D"), comment);

			var ret = DAL.InsertInto(fieldsName, "Bookmark", fieldsValue);

			return ret;
		}

		public List<BookmarkDto> GetBookmark()
		{
			var fieldsName = string.Empty;

			fieldsName = "*";

			var dataTable = DAL.SelectFrom(fieldsName, "Bookmark", "[UserID]", this.ID);

			if (dataTable != null)
			{
				List<BookmarkDto> bmlist = new List<BookmarkDto>();

				foreach (DataRow row in dataTable.Rows)
				{
					var bookmark = new BookmarkDto
					{
						userID = (int)row["ID"],
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
