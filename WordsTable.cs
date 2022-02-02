using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Dictionary
{
    public class WordsTable : IRepository<Word>
    {
        public string TableName { get; set; }
        public WordsTable(string tableName)
        {
            TableName = tableName;
        }
        public Word GetById(int id)
        {
            Word word = null;
            SqlConnection conn = DatabaseSingleton.GetInstance();

            using (SqlCommand command = new SqlCommand("SELECT * FROM " + TableName + " WHERE id = @Id", conn))
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Id";
                param.Value = id;

                command.Parameters.Add(param);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    word = new Word
                    {
                        Id = Convert.ToInt32(reader[0].ToString()),
                        _Word = reader[1].ToString(),
                    };
                }
                reader.Close();
                return word;
            }
        }

        public IEnumerable<Word> GetAll()
        {
            SqlConnection conn = DatabaseSingleton.GetInstance();

            using (SqlCommand command = new SqlCommand("SELECT * FROM " + TableName, conn))
            {
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Word word = new Word
                    {
                        Id = Convert.ToInt32(reader[0].ToString()),
                        _Word = reader[1].ToString(),
                    };
                    yield return word;
                }
                reader.Close();
            }
        }

        public void Save(Word word)
        {
            SqlConnection conn = DatabaseSingleton.GetInstance();

            SqlCommand command = null;

            if (word.Id < 1)
            {
                using (command = new SqlCommand("INSERT INTO " + TableName + " VALUES (@word)", conn))
                {
                    command.Parameters.Add(new SqlParameter("@word", word._Word));
                    command.ExecuteNonQuery();
                    command.CommandText = "Select @@Identity";
                    word.Id = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            else
            {
                using (command = new SqlCommand("UPDATE " + TableName + " SET word = @word WHERE id = @id", conn))
                {
                    command.Parameters.Add(new SqlParameter("@word", word._Word));
                    command.Parameters.Add(new SqlParameter("@id", word.Id));
                    command.ExecuteNonQuery();
                }
            }

        }

        public void Remove(Word word)
        {
            SqlConnection conn = DatabaseSingleton.GetInstance();

            using (SqlCommand command = new SqlCommand("DELETE FROM" + TableName + "WHERE id = @id", conn))
            {
                command.Parameters.Add(new SqlParameter("@id", word.Id));
                command.ExecuteNonQuery();
                word.Id = 0;
            }
        }

        public int GetIdByName(string word)
        {
            SqlConnection conn = DatabaseSingleton.GetInstance();
            int result = 0;

            using (SqlCommand command = new SqlCommand("SELECT * FROM " + TableName + " WHERE word = @word", conn))
            {
                command.Parameters.Add(new SqlParameter("@word", word));

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result = Convert.ToInt32(reader[0].ToString());
                }
                reader.Close();
                return result;
            }
        }
        
    }
}
