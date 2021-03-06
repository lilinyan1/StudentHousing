﻿/*
* FILE:             BaseDAL.cs
* PROJECT:          PROG2020 - Project Development - Capstone
* PROGRAMMER:       Becky Linyan Li, Matthew Cocca, Xingguang Zhen
* AVAILABLE DATE:   26-4-2017
* DESCRIPTION:      Contains reusable database request handler such as 
*                   - insert, update and delete entities
*                   - object to string, object to int
*                   - insert image
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Utility;

namespace StudentHousing.DAL
{
	public class BaseDAL
	{
		//private const string CONNECTION_STRING = @"Server=localhost;Database=StudentHousing;User Id=admin;Password=admin;";
		private const string CONNECTION_STRING = @"Server=tcp:studenthousingdb.database.windows.net,1433;Initial Catalog = StudentHousingDB; Persist Security Info=False;User ID = HouseAdmin; Password=Secret@5; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";

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
                var ret = int.MinValue;
                try
				{
					conn.ConnectionString = CONNECTION_STRING;
					conn.Open();
					SqlCommand command;
                    
                    command = new SqlCommand(string.Format("INSERT INTO {0} ({1}) output INSERTED.ID VALUES ({2});", tableName, insertFields, fieldsValue), conn);
                    ret = (int)command.ExecuteScalar();

                }
				catch (Exception e)
				{
					Logging.Log("DAL", "InsertInto", e.Message, false);

				}
				return ret;
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
				return int.MinValue;
			}
		}

        public static bool Delete(string tableName, string condition)
        {

            using (SqlConnection conn = new SqlConnection())
            {
                try
                {
                    conn.ConnectionString = CONNECTION_STRING;
                    conn.Open();
                    SqlCommand command = new SqlCommand(string.Format("DELETE FROM {0} WHERE {1}", tableName, condition), conn);
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Logging.Log("DAL", "Delete", e.Message, false);
                    return false;
                }
                return true;
            }
        }
        public static string GetProperties(Type type)
        {
            var properties = string.Empty;
            foreach (var property in type.GetProperties())
            {
                properties = string.Format("{0}[{1}],", properties, property.Name);
            }
            if (properties != string.Empty)
            {
                properties = properties.TrimEnd(',');
            }
            return properties;
           
        }

        public static string ToString(object obj)
		{
			if (obj == DBNull.Value)
				return string.Empty;
			else
				return (string)obj;
		}

        // Store a byte array & description for a property
        public static int AddImage(int id, string description, byte[] img)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                try
                {
                    conn.ConnectionString = CONNECTION_STRING;
                    string str = string.Format("INSERT INTO images (propertyID, imageDescription, img) VALUES ('{0}', '{1}', (@img))", id, description);
                    SqlCommand cmd = new SqlCommand(str);
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@img", img);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return 0;
                }
                catch (Exception e)
                {
                    Logging.Log("DAL", "GetImage", e.Message, false);
                }
                return int.MinValue;
            }
        }

        // Read all images for a property id and return them as byte arrays
		public static List<byte[]> GetImage(int id)
		{
			using (SqlConnection conn = new SqlConnection())
			{
				try
				{
					conn.ConnectionString = CONNECTION_STRING;
					conn.Open();
					SqlCommand cmd = new SqlCommand("SELECT img FROM images WHERE propertyID = " + id, conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    List<byte[]> images = new List<byte[]>();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            byte[] imgOut = (byte[])dr["img"];
                            images.Add(imgOut);
                        }                   
                        return images;
                    }
                    else
                    {
                        return null;
                    }               
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
