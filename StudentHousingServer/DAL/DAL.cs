using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Utility;

namespace StudentHousing.DAL
{
	public class DAL
	{
		//private const string CONNECTION_STRING = @"Server=localhost;Database=StudentHousing;User Id=admin;Password=admin2017;";
		private const string CONNECTION_STRING = @"Server=tcp:cstudenthousing.database.windows.net,1433;Initial Catalog=StudentHousing;Persist Security Info=False;User ID=mcocca;Password=Conestoga1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

		public static EnumerableRowCollection<DataRow> SelectFrom(string selectFields, string tableName, string condition)
		{
			using (SqlConnection conn = new SqlConnection())
			{
				try
				{
					conn.ConnectionString = CONNECTION_STRING;
					conn.Open();
					SqlCommand command = new SqlCommand(string.Format("SELECT {0} FROM [{1}] WHERE {2}", selectFields, tableName, condition), conn);
					using (SqlDataReader reader = command.ExecuteReader())
					{
						var dataTable = new DataTable();
						dataTable.Load(reader);
						return dataTable.AsEnumerable();
					}
				}
				catch (Exception e)
				{
					Logging.Log("DAL", "SelectFrom", e.Message, false);

				}
				return null;
			}

		}
		public static int InsertInto(string insertFields, string tableName, string fieldsValue)
		{

			using (SqlConnection conn = new SqlConnection())
			{
				try
				{
					conn.ConnectionString = CONNECTION_STRING;
					conn.Open();
					SqlCommand command;
					if (tableName == "Users" || tableName == "Property")
					{
						command = new SqlCommand(string.Format("SET IDENTITY_INSERT {0} ON;INSERT INTO {0} ({1}) VALUES ({2});SET IDENTITY_INSERT {0} OFF;", tableName, insertFields, fieldsValue), conn);
						command.ExecuteNonQuery();
					}
					else
					{
						command = new SqlCommand(string.Format("INSERT INTO {0} ({1}) VALUES ({2});", tableName, insertFields, fieldsValue), conn);
						command.ExecuteNonQuery();
					}
					return 0;
				}
				catch (Exception e)
				{
					Logging.Log("DAL", "InsertInto", e.Message, false);

				}
				return 1;

			}
		}

		public static int UpdateSet(string updateValues, string tableName, string conditionColumnName, object conditionValue)
		{

			using (SqlConnection conn = new SqlConnection())
			{
				try
				{
					conn.ConnectionString = CONNECTION_STRING;
					conn.Open();
					SqlCommand command = new SqlCommand(string.Format("UPDATE {0} SET {1} WHERE {2} = {3}", tableName, updateValues, conditionColumnName, conditionValue), conn);
					command.ExecuteNonQuery();
					return 0;
				}
				catch (Exception e)
				{
					Logging.Log("DAL", "UpdateSet", e.Message, false);
				}
				return 1;
			}
		}

		public static string ToString(object obj)
		{
			if (obj == DBNull.Value)
				return string.Empty;
			else
				return (string)obj;
		}
		public static byte[] GetImage(int id)
		{
			using (SqlConnection conn = new SqlConnection())
			{
				try
				{
					conn.ConnectionString = CONNECTION_STRING;
					conn.Open();
					SqlCommand cmd = new SqlCommand("SELECT propertyimage FROM Property WHERE PropertyID = " + id, conn);
					byte[] img = (byte[])cmd.ExecuteScalar();
					return img;
				}
				catch (Exception e)
				{
					Logging.Log("DAL", "GetImage", e.Message, false);
				}
				return null;
			}
		}

		public static int ToInt(object obj)
		{
			if (obj == DBNull.Value)
				return int.MinValue;
			else
				return (int)obj;
		}

		public static DateTime ToDateTime(object obj)
		{
			if (obj == DBNull.Value)
				return DateTime.MinValue;
			else
				return (DateTime)obj;
		}
	}
}
