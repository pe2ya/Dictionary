using System;
using System.Collections.Generic;
using System.Text;

namespace Dictionary
{
     public class Word :  IBaseClass
    {
        public int Id { get; set; }
        public string _Word { get; set; }

        public Word(int id, string word)
        {
            Id = id;
            _Word = char.ToUpper(word[0]) + word.ToLower().Substring(1);
        }

        public Word(string word)
        {
            if (word.Length > 2)
            {
                _Word = char.ToUpper(word[0]) + word.ToLower().Substring(1);
            }
        }

        public Word() { }

        public override string ToString()
        {
            return Id + ". " + _Word;
        }
    }
}
