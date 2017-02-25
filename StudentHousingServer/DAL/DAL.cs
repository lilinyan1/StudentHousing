using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace StudentHousing.DAL
{
    class DAL
    {
        private const string CONNECTION_STRING = @"Server=localhost;Database=StudentHousing;User Id=admin;Password=admin2017;";
        //private const string CONNECTION_STRING = @"Server=tcp:cstudenthousing.database.windows.net,1433;Initial Catalog=StudentHousing;Persist Security Info=False;User ID=mcocca;Password=Conestoga1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static DataTable SelectFrom(string selectFields, string tableName, string conditionColumnName, object conditionValue)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                try
                {
                    conn.ConnectionString = CONNECTION_STRING;
                    conn.Open();
                    SqlCommand command = new SqlCommand(string.Format("SELECT {0} FROM [{1}] WHERE {2} = @CONDITION_VALUE", selectFields, tableName, conditionColumnName), conn);
                    command.Parameters.Add(new SqlParameter("CONDITION_VALUE", conditionValue));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
                catch (Exception e)
                {
                    Logging.Log("DAL", "SelectFrom", e.Message, false);
                }
                return null;
            }
        }
    }
}
