using System;
using System.Collections.Generic;
using System.Text;

namespace Dictionary
{
    class Translate : IBaseClass
    {
        public int Id { get; set; }
        public Word EnglishWord { get; set; }
        public Word ForeignWord { get; set; }

        public Translate(int id, Word eWord, Word fWord)
        {
            Id = id;
            EnglishWord = eWord;
            ForeignWord = fWord;
        }

        public Translate(Word eWord, Word fWord)
        {
            EnglishWord = eWord;
            ForeignWord = fWord;
        }

        public Translate(string eWord, string fWord)
        {
            EnglishWord = new Word(eWord);
            ForeignWord = new Word(fWord);
        }

        public Translate() { }

        public string GetTranslate(string word)
        {
            string result = "Incorrect word";

            word = word.ToLower();
            if (EnglishWord._Word.ToLower() == word)
            {
                result = ForeignWord._Word;
            }
            if (ForeignWord._Word.ToLower() == word)
            {
                result = EnglishWord._Word;
            }

            return result;
        }
        public override string ToString()
        {
            return Id + ". "+ EnglishWord._Word + " - " + ForeignWord._Word;
        }
    }
}
