using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Dictionary
{
    class Vocabulary : IRepository<Translate>
    {
        private static string name = Stat.GetInfo("DictionaryTable");
        private static string tableE = Stat.GetInfo("EnglishTable");
        private static string tableF = Stat.GetInfo("ForeignTable");

        private static string inners = $"INNER JOIN {tableE} ON {tableE}.id = {name}.{tableE}_id" +
                $" INNER JOIN {tableF} ON {tableF}.id = {name}.{tableF}_id";

        public Translate GetById(int id)
        {
            Translate result = null;
            SqlConnection conn = DatabaseSingleton.GetInstance();

            using (SqlCommand command = new SqlCommand($"SELECT * FROM {name} {inners} WHERE {name}.id = @id", conn))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result = new Translate
                    {
                        Id = Convert.ToInt32(reader[0].ToString()),
                        EnglishWord = new Word
                        {
                            Id = Convert.ToInt32(reader[3].ToString()),
                            _Word = reader[4].ToString()
                        },
                        ForeignWord = new Word
                        {
                            Id = Convert.ToInt32(reader[5].ToString()),
                            _Word = reader[6].ToString()
                        }
                    };
                }
                reader.Close();
                return result;
            }
        }

        public IEnumerable<Translate> GetAll()
        {
            SqlConnection conn = DatabaseSingleton.GetInstance();

            using (SqlCommand command = new SqlCommand($"SELECT * FROM {name} {inners}", conn))
            {
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Translate result = new Translate
                    {
                        Id = Convert.ToInt32(reader[0].ToString()),
                        EnglishWord = new Word
                        {
                            Id = Convert.ToInt32(reader[3].ToString()),
                            _Word = reader[4].ToString()
                        },
                        ForeignWord = new Word
                        {
                            Id = Convert.ToInt32(reader[5].ToString()),
                            _Word = reader[6].ToString()
                        }
                    };
                    yield return result;
                }
                reader.Close();
            }
        }

        public void Save(Translate translate)
        {
            SqlConnection conn = DatabaseSingleton.GetInstance();
            SqlCommand command = null;

            WordsTable ETable = new WordsTable(Stat.GetInfo("EnglishTable"));
            WordsTable FTable = new WordsTable(Stat.GetInfo("ForeignTable"));

            int e_id = ETable.GetIdByName(translate.EnglishWord._Word);
            int f_id = FTable.GetIdByName(translate.ForeignWord._Word);

            if (e_id > 0) {
                translate.EnglishWord = ETable.GetById(e_id);
            }
            else{
                ETable.Save(translate.EnglishWord);
            }

            if (f_id > 0) {
                translate.ForeignWord = FTable.GetById(f_id);
            }
            else {
                FTable.Save(translate.ForeignWord);
            }

            if (translate.Id < 1)
            {
                using (command = new SqlCommand($"INSERT INTO {name} VALUES (@eword_id, @fword_id)", conn))
                {
                    command.Parameters.Add(new SqlParameter("@eword_id", translate.EnglishWord.Id));
                    command.Parameters.Add(new SqlParameter("@fword_id", translate.ForeignWord.Id));
                    command.ExecuteNonQuery();
                    command.CommandText = "Select @@Identity";
                    translate.Id = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            else
            {
                using (command = new SqlCommand($"UPDATE {name} SET {tableE}_id = @e_id, {tableF}_id = @f_id WHERE {name}.id = @id", conn))
                {
                    command.Parameters.Add(new SqlParameter("@id", translate.Id));
                    command.Parameters.Add(new SqlParameter("@e_id", translate.EnglishWord.Id));
                    command.Parameters.Add(new SqlParameter("@f_id", translate.ForeignWord.Id));
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Remove(Translate translate)
        {
            SqlConnection conn = DatabaseSingleton.GetInstance();

            using (SqlCommand command = new SqlCommand($"DELETE FROM {name} WHERE {name}.id = @id",conn))
            {
                command.Parameters.Add(new SqlParameter("@id", translate.Id));
                command.ExecuteNonQuery();
                translate.Id = 0;
            }
        }
        
        public string TranslateToEng(string word)
        {
            SqlConnection conn = DatabaseSingleton.GetInstance();
            string result = string.Empty;

            WordsTable FWord = new WordsTable(Stat.GetInfo("ForeignTable"));
            int f_id = FWord.GetIdByName(word);

            if (f_id > 0)
            {
                List<Translate> list = new List<Translate>();
                using (SqlCommand command = new SqlCommand($"SELECT * FROM {name} {inners} WHERE {tableF}_id = @Fid", conn))
                {
                    command.Parameters.Add(new SqlParameter("@Fid", f_id));
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read()) 
                    {
                        Translate translate = new Translate
                        {
                            Id = Convert.ToInt32(reader[0].ToString()),
                            EnglishWord = new Word
                            {
                                Id = Convert.ToInt32(reader[3].ToString()),
                                _Word = reader[4].ToString()
                            },
                            ForeignWord = new Word
                            {
                                Id = Convert.ToInt32(reader[5].ToString()),
                                _Word = reader[6].ToString()
                            }
                        };

                        list.Add(translate);
                    }
                    reader.Close();
                    result = word + " - ";

                    foreach (Translate x in list)
                    {
                        result += x.EnglishWord._Word + ", ";
                    }

                    result = result.Remove(result.Length - 2);
                }
            }
            else
            {
                result = "The word doesn't exist";
            }

            return result;
        }

        public string TranslateToFgn(string word)
        {
            SqlConnection conn = DatabaseSingleton.GetInstance();
            string result = string.Empty;

            WordsTable EWord = new WordsTable(Stat.GetInfo("EnglishTable"));
            int f_id = EWord.GetIdByName(word);

            if (f_id > 0)
            {
                List<Translate> list = new List<Translate>();
                using (SqlCommand command = new SqlCommand($"SELECT * FROM {name} {inners} WHERE {tableE}_id = @Fid", conn))
                {
                    command.Parameters.Add(new SqlParameter("@Fid", f_id));
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Translate translate = new Translate
                        {
                            Id = Convert.ToInt32(reader[0].ToString()),
                            EnglishWord = new Word
                            {
                                Id = Convert.ToInt32(reader[3].ToString()),
                                _Word = reader[4].ToString()
                            },
                            ForeignWord = new Word
                            {
                                Id = Convert.ToInt32(reader[5].ToString()),
                                _Word = reader[6].ToString()
                            }
                        };

                        list.Add(translate);
                    }
                    reader.Close();
                    result = word + " - ";

                    foreach (Translate x in list)
                    {
                        result += x.ForeignWord._Word + ", ";
                    }

                    result = result.Remove(result.Length - 2);
                }
            }
            else
            {
                result = "The word doesn't exist";
            }

            return result;
        }

    }
}
