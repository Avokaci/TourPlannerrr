using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Test
{
    /// <summary>
    /// NameGenerator class that was created for testing purposes, in order to create a random tour item and random tourlog. 
    /// </summary>
    public class NameGeneratorMOCK
    {
        /// <summary>
        /// Method to generate a string consisting of consonants and vowels. 
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public string GenerateName(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;
        }
    }
}
