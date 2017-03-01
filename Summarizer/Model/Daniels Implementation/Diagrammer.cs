using Summarizer.Model.Utils;
using Summarizer.Model.Utils.Stemming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Daniels_Implementation
{
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
            string[] allVerbs = System.IO.File
                .ReadAllLines(@"..\..\..\Resources\Bible KJV PoS\Verbs.txt");
            string[] allAdverbs = System.IO.File
                .ReadAllLines(@"..\..\..\Resources\Bible KJV PoS\Adverbs.txt");
            string[] allAdjectives = System.IO.File
                .ReadAllLines(@"..\..\..\Resources\Bible KJV PoS\Adjectives.txt");
            string[] allPronouns = System.IO.File
                .ReadAllLines(@"..\..\..\Resources\Bible KJV PoS\Pronouns.txt");
            string[] allPrepositions = System.IO.File
                .ReadAllLines(@"..\..\..\Resources\Bible KJV PoS\Prepositions.txt");
            string[] allProperNouns = System.IO.File
                .ReadAllLines(@"..\..\..\Resources\Bible KJV PoS\Proper Nouns.txt");
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
            
            System.IO.File.WriteAllLines(@"..\..\..\Resources\Bible KJV PoS\Not Found.txt",
                others.ToArray());
            System.IO.File.WriteAllLines(@"..\..\..\Resources\Bible KJV PoS\Verbs.txt",
                verbs.ToArray());
            System.IO.File.WriteAllLines(@"..\..\..\Resources\Bible KJV PoS\Adverbs.txt",
                adverbs.ToArray());
            System.IO.File.WriteAllLines(@"..\..\..\Resources\Bible KJV PoS\Adjectives.txt",
                adjectives.ToArray());
            System.IO.File.WriteAllLines(@"..\..\..\Resources\Bible KJV PoS\Pronouns.txt",
                pronouns.ToArray());
            System.IO.File.WriteAllLines(@"..\..\..\Resources\Bible KJV PoS\Prepositions.txt",
                prepositions.ToArray());
            System.IO.File.WriteAllLines(@"..\..\..\Resources\Bible KJV PoS\Articles.txt",
                articles.ToArray());
            //System.IO.File.WriteAllLines(@"..\..\..\Resources\Bible KJV PoS\Proper Nouns.txt",
                //properNouns.ToArray());
            System.IO.File.WriteAllLines(@"..\..\..\Resources\Bible KJV PoS\Posessives.txt",
                posessives.ToArray());
        }


    }
}
