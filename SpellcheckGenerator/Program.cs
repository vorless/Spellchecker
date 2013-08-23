using System;
using System.Collections.Generic;
using System.IO;

namespace SpellcheckGenerator
{
    public class Generator
    {
        private string[] bases = {"honk", "stoat", "phantasm", "hyper", "fireball"};
        private Random rand;

        public Generator()
        {
            rand = new Random();

        }

        public List<string> Generate()
        {
            List<string> inputs = new List<string>();
            foreach (string b in bases)
            {
                inputs.Add(b.ToUpper());
                string w = MixVowels(b);
                inputs.Add(w);
                w = RepeatLetters(w);
                inputs.Add(w);
            }
            return inputs;
        }

        private string RepeatLetters(string input)
        {
            char[] word = input.ToCharArray();
            string output = "";
            for (int i = 0; i < word.Length; i++)
            {
                for (int j = 0; j < rand.Next(1, 4); j++)
                {
                    output += word[i];
                }
            }

            return output;
        }

        private string MixVowels(string input)
        {
            char[] word = input.ToCharArray();
            string output = "";
            for (int i = 0; i < word.Length; i++)
            {
                if (isVowel(word[i]))
                {
                    output += 'O';
                }
                else
                {
                    output += word[i];
                }
            }
            return output;
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
            Generator g = new Generator();
            List<string> ins = g.Generate();
            using (StreamWriter sw = new StreamWriter("words.txt"))
            {
                foreach (string @in in ins)
                {
                    sw.WriteLine(@in);
                }
            }
        }
    }
}
