using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary	
{
	public class DatabaseSingleton
	{
		private static SqlConnection conn = null;
		private DatabaseSingleton()
		{
		}
		public static SqlConnection GetInstance()
		{
			if (conn == null)
			{
				SqlConnectionStringBuilder consStringBuilder = new SqlConnectionStringBuilder();
				consStringBuilder.UserID = Stat.GetInfo("UserID");
				consStringBuilder.Password = Stat.GetInfo("Password");
				consStringBuilder.InitialCatalog = Stat.GetInfo("Database");
				consStringBuilder.DataSource = Stat.GetInfo("DataSource");
				consStringBuilder.ConnectTimeout = 30;
				conn = new SqlConnection(consStringBuilder.ConnectionString);
				conn.Open();
			}
			return conn;
		}
		public static void CloseConnection()
		{
			if (conn != null)
			{
				conn.Close();
				conn.Dispose();
			}
		}
	}
}