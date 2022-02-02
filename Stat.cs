using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace Dictionary
{
    static class Stat
    {
		public static string GetInfo(this string key)
		{
			var appSettings = ConfigurationManager.AppSettings;
			string result = appSettings[key];
			return result;
		}

		public static void RefreshTables()
		{
			SqlConnection conn = DatabaseSingleton.GetInstance();
			string TableE = "CREATE TABLE " + Stat.GetInfo("EnglishTable") + " (id int primary key identity(1,1)," +
				"word varchar(30) not null)";

			string TableF = "CREATE TABLE " + Stat.GetInfo("ForeignTable") + " (id int primary key identity(1,1)," +
				"word nvarchar(30) not null)";

			string TableD = "CREATE TABLE " + Stat.GetInfo("DictionaryTable") + " (id int primary key identity(1,1)," +
					"EWord_id int FOREIGN KEY REFERENCES EWord(id)," +
					"FWord_id int FOREIGN KEY REFERENCES FWord(id))";

			using (conn)
			{
				try
				{
					Console.WriteLine("Connected");

					SqlCommand dropTableD = new SqlCommand("DROP TABLE Dictionary", conn);
					dropTableD.ExecuteNonQuery();

					SqlCommand dropTableE = new SqlCommand("DROP TABLE EWord", conn);
					dropTableE.ExecuteNonQuery();

					SqlCommand dropTableC = new SqlCommand("DROP TABLE FWord", conn);
					dropTableC.ExecuteNonQuery();

					SqlCommand commandE = new SqlCommand(TableE, conn);
					commandE.ExecuteNonQuery();

					SqlCommand commandF = new SqlCommand(TableF, conn);
					commandF.ExecuteNonQuery();

					SqlCommand commandD = new SqlCommand(TableD, conn);
					commandD.ExecuteNonQuery();

					Console.WriteLine("Done");
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}
	}
}
