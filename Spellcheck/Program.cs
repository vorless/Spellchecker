using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Spellcheck
{
    public class Spellchecker
    {

        private readonly Dictionary<string, List<string>> _dict;
        private Random rand;


        public Spellchecker(FileStream inStream)
        {
            try
            {
                _dict = new Dictionary<string, List<string>>();
                rand = new Random();
                BuildDictionary(inStream);
            }
            catch (IOException e)
            {

                throw new IOException(e.Message);
            }

        }

        public void Execute()
        {
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input != null)
                {
                    input = input.ToLower();
                    string word = RemoveRepeatLetters(input);
                    word = ReplaceVowels(word);
                    string corrected = Check(word);
                    if (corrected != null)
                    {
                        Console.WriteLine(corrected);
                    }
                    else
                    {
                        word = ReplaceVowels(input);
                        word = RemoveRepeatLetters(word);
                        if ((corrected = Check(word)) == null)
                        {
                            Console.WriteLine("NO SUGGESTION");
                        }
                        else
                        {
                            Console.WriteLine(corrected);
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void BuildDictionary(FileStream inStream)
        {
            string line;
            using (StreamReader reader = new StreamReader(inStream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    string word = line.Trim();
                    string replacedWord = ReplaceVowels(RemoveRepeatLetters(word.ToLower()));
                    if (!_dict.ContainsKey(replacedWord))
                    {
                        _dict.Add(replacedWord, new List<string>());
                    }
                    _dict[replacedWord].Add(word);
                }
            }
        }

        private string Check(string input)
        {

            try
            {
                var match = _dict[input];
                return match[rand.Next(0, match.Count)];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }

        }

        private string RemoveRepeatLetters(string input)
        {
            char[] word = input.ToCharArray();
            string result = word[0].ToString(CultureInfo.InvariantCulture);
            for (int i = 0, j = 0; i < word.Length; i++)
            {
                if (word[i] == '!' || word[i] != result[j])
                {
                    result += word[i];
                    j++;
                }
            }
            return result;
        }

        private string ReplaceVowels(string input)
        {
            char[] word = input.ToCharArray();
            string result = "";
            for (int i = 0; i < word.Length; i++)
            {
                if (isVowel(word[i]))
                {
                    result += '!';
                }
                else
                {
                    result += word[i];
                }
            }
            return result;
        }

        private bool isVowel(char c)
        {
            return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u' || c == 'y';
        }

    }


    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please provide the path to the dictionary file.");
            }
            else
            {
                string dictionary = args[0];
                new Spellchecker(new FileStream(dictionary, FileMode.Open)).Execute();
            }
        }
    }
}
