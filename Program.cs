using System;
using System.Data.SqlClient;

namespace Dictionary
{
    class Program
    {
        static void Main(string[] args)
        {
			//Stat.RefreshTables();
	
			Vocabulary voc = new Vocabulary();
			WordsTable eWord = new WordsTable(Stat.GetInfo("EnglishTable"));
			WordsTable fWord = new WordsTable(Stat.GetInfo("ForeignTable"));

			int x = 1;
			while (x != 0)
			{
				try
				{
					Console.WriteLine("1 - Add new Translate, 2 - Update translate, 3 - Delete translate, 4 - Translate to English, 5 - Translate to Czech, 0 - quit");

					x = Convert.ToInt32(Console.ReadLine());
					while (x < 0 && x > 5)
					{
						Console.WriteLine("Invalit data");
						x = Convert.ToInt32(Console.ReadLine());
					}

					switch (x)
					{
						case 1:
							Console.WriteLine("\nSet an English word:");
							string engWord = Console.ReadLine();
							while (engWord.Length < 3)
							{
								Console.WriteLine("The word is too short");
								engWord = Console.ReadLine();
							}

							Console.WriteLine("\nSet a Czech word:");
							string fgnWord = Console.ReadLine();
							while (fgnWord.Length < 3)
							{
								Console.WriteLine("The word is too short");
								fgnWord = Console.ReadLine();
							}

							Translate newTranslate = new Translate(engWord, fgnWord);

							voc.Save(newTranslate);
							break;
						case 2:
							Console.WriteLine("Choose a translate\n");

							foreach (var t in voc.GetAll())
							{
								Console.WriteLine(t);
							}

							int num = Convert.ToInt32(Console.ReadLine());
							Translate tr = voc.GetById(num);

							Console.WriteLine("\nYou choose " + tr);

							Console.WriteLine("\nUpdate an English word:");
							engWord = Console.ReadLine();
							while (engWord.Length < 2)
							{
								Console.WriteLine("The word is too short");
								engWord = Console.ReadLine();
							}

							Console.WriteLine("\nUpdate a Czech word:");
						    fgnWord = Console.ReadLine();
							while (fgnWord.Length < 2)
							{
								Console.WriteLine("The word is too short");
								fgnWord = Console.ReadLine();
							}

							tr.EnglishWord = new Word(engWord);
							tr.ForeignWord = new Word(fgnWord);

							Console.WriteLine("Do you want save changes? (1 - Yes, 2 - No)");

							int choise = Convert.ToInt32(Console.ReadLine());
							while (choise < 1 && choise > 2)
							{
								Console.WriteLine("Invalit data");
								choise = Convert.ToInt32(Console.ReadLine());
							}

							if (choise == 1)
							{
								voc.Save(tr);
								Console.WriteLine("Changes saved");
							}
							else {
								Console.WriteLine("Changes rejected");
							}
							break;
						case 3:
							Console.WriteLine("Choose a translate\n");

							foreach (var t in voc.GetAll())
							{
								Console.WriteLine(t);
							}

							num = Convert.ToInt32(Console.ReadLine());
							tr = voc.GetById(num);

							Console.WriteLine("\nYou choose " + tr);

							Console.WriteLine("Do you want to remove translate? (1 - Yes, 2 - No)");

							choise = Convert.ToInt32(Console.ReadLine());
							while (choise < 1 && choise > 2)
							{
								Console.WriteLine("Invalit data");
								choise = Convert.ToInt32(Console.ReadLine());
							}

							if (choise == 1)
							{
								voc.Remove(tr);
								Console.WriteLine("Translate removed");
							}
							else
							{
								Console.WriteLine("Changes rejected");
							}
							break;
						case 4:
							Console.WriteLine("Enter a word:");
							string enter = Console.ReadLine();

							Console.WriteLine(voc.TranslateToEng(enter));
							break;
						case 5:
							Console.WriteLine("Enter a word:");
							enter = Console.ReadLine();

							Console.WriteLine(voc.TranslateToFgn(enter));
							break;
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
			DatabaseSingleton.CloseConnection();
		}
    }
}
