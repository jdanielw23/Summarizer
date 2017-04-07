using Summarizer.Model.Utils;
using Summarizer.Model.Utils.Stemming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Daniels_Implementation
{
    /// <summary>
    /// Created by J. Daniel Worthington
    /// Not being used. At one point, I was considering implementing my own sentence diagrammer.
    /// </summary>
    public class Diagrammer
    {
        private HashSet<string> verbs = new HashSet<string>();
        private HashSet<string> adverbs = new HashSet<string>();
        private HashSet<string> adjectives = new HashSet<string>();
        private HashSet<string> pronouns = new HashSet<string>();
        private HashSet<string> articles = new HashSet<string>();
        private HashSet<string> prepositions = new HashSet<string>();
        private HashSet<string> posessives = new HashSet<string>();
        private HashSet<string> properNouns = new HashSet<string>();
        private HashSet<string> others = new HashSet<string>();

        public Diagrammer(string filePath)
        {
            // To hold all words
            HashSet<string> words = new HashSet<string>();

            // For referencing
            string folderpath = "";
#if DEBUG
            folderpath = @"..\..\..\Resources\Bible KJV PoS\";
#else
            folderpath = @".\Documents\Bible KJV PoS\";
#endif
            string[] allVerbs = System.IO.File
                .ReadAllLines(folderpath + "Verbs.txt");
            string[] allAdverbs = System.IO.File
                .ReadAllLines(folderpath + "Adverbs.txt");
            string[] allAdjectives = System.IO.File
                .ReadAllLines(folderpath + "Adjectives.txt");
            string[] allPronouns = System.IO.File
                .ReadAllLines(folderpath + "Pronouns.txt");
            string[] allPrepositions = System.IO.File
                .ReadAllLines(folderpath + "Prepositions.txt");
            string[] allProperNouns = System.IO.File
                .ReadAllLines(folderpath + "Proper Nouns.txt");
            string[] allArticles = { "an", "a", "and", "the", "thou" };

            // Read in the text
            string text = System.IO.File.ReadAllText(filePath);

            foreach (string rawWord in text.Split(' '))
            {
                string word = rawWord.ToLower().Trim().TrimStart('(','\'','"')
                    .TrimEnd(':', ';', '"', '\'', '?', '.', ',', '!', ')');

                if (word.ContainsOnlyLetters() && !string.IsNullOrEmpty(word))
                {
                    if (allPronouns.Contains(word))
                        pronouns.Add(word);
                    else if (word.Contains("'"))
                        posessives.Add(word);
                    else if (allArticles.Contains(word))
                        articles.Add(word);
                    else if (allPrepositions.Contains(word))
                        prepositions.Add(word);
                    else if (allVerbs.Contains(word))
                        verbs.Add(word);
                    else if (allAdverbs.Contains(word))
                        adverbs.Add(word);
                    else if (allAdjectives.Contains(word))
                        adjectives.Add(word);
                    else if (allProperNouns.Contains(word))
                        properNouns.Add(word);
                    else
                        others.Add(word);
                }
            }
            
            System.IO.File.WriteAllLines(folderpath + "Not Found.txt",
                others.ToArray());
            System.IO.File.WriteAllLines(folderpath + "Verbs.txt",
                verbs.ToArray());
            System.IO.File.WriteAllLines(folderpath + "Adverbs.txt",
                adverbs.ToArray());
            System.IO.File.WriteAllLines(folderpath + "Adjectives.txt",
                adjectives.ToArray());
            System.IO.File.WriteAllLines(folderpath + "Pronouns.txt",
                pronouns.ToArray());
            System.IO.File.WriteAllLines(folderpath + "Prepositions.txt",
                prepositions.ToArray());
            System.IO.File.WriteAllLines(folderpath + "Articles.txt",
                articles.ToArray());
            //System.IO.File.WriteAllLines(folderpath + "Proper Nouns.txt",
            //properNouns.ToArray());
            System.IO.File.WriteAllLines(folderpath + "Posessives.txt",
                posessives.ToArray());
        }


    }
}
