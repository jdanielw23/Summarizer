using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Summarizer.Model.Utils
{
    public static class Constants
    {
        private static IDictionary<string, IDictionary<int, IDictionary<int, string>>> bible;

        public static IDictionary<string, IDictionary<int, IDictionary<int, string>>> Bible
        {
            get
            {
                if (bible == null)
                    bible = BuildBible();
                return bible;
            }
        }

        public static IDictionary<string, int> BibleBooks
        {
            get
            {
                return new Dictionary<string, int>()
                {
                    { "Genesis", 50 },
                    { "Exodus", 40 },
                    { "Leviticus", 27 },
                    { "Numbers", 36 },
                    { "Deuteronomy", 34 },
                    { "Joshua", 24 },
                    { "Judges", 21 },
                    { "Ruth", 4 },
                    { "1 Samuel", 31 },
                    { "2 Samuel", 24 },
                    { "1 Kings", 22 },
                    { "2 Kings", 25 },
                    { "1 Chronicles", 29 },
                    { "2 Chronicles", 36 },
                    { "Ezra", 10 },
                    { "Nehemiah", 13 },
                    { "Esther", 10 },
                    { "Job", 42 },
                    { "Psalm", 150 },
                    { "Proverbs", 31 },
                    { "Ecclesiastes", 12 },
                    { "Song of Solomon", 8 },
                    { "Isaiah", 66 },
                    { "Jeremiah", 52 },
                    { "Lamentations", 5 },
                    { "Ezekiel", 48 },
                    { "Daniel", 12 },
                    { "Hosea", 14 },
                    { "Joel", 3 },
                    { "Amos", 9 },
                    { "Obadiah", 1 },
                    { "Jonah", 4 },
                    { "Micah", 7 },
                    { "Nahum", 3 },
                    { "Habakkuk", 3 },
                    { "Zephaniah", 3 },
                    { "Haggai", 2 },
                    { "Zechariah", 14 },
                    { "Malachi", 4 },
                    { "Matthew", 28 },
                    { "Mark", 16 },
                    { "Luke", 24 },
                    { "John", 21 },
                    { "Acts", 28 },
                    { "Romans", 16 },
                    { "1 Corinthians", 16 },
                    { "2 Corinthians", 13 },
                    { "Galatians", 6 },
                    { "Ephesians", 6 },
                    { "Philippians", 4 },
                    { "Colossians", 4 },
                    { "1 Thessalonians", 5 },
                    { "2 Thessalonians", 3 },
                    { "1 Timothy", 6 },
                    { "2 Timothy", 4 },
                    { "Titus", 3 },
                    { "Philemon", 1 },
                    { "Hebrews", 13 },
                    { "James", 5 },
                    { "1 Peter", 5 },
                    { "2 Peter", 3 },
                    { "1 John", 5 },
                    { "2 John", 1 },
                    { "3 John", 1 },
                    { "Jude", 1 },
                    { "Revelation", 22 }
                };
            }
        }

        public static string[] LongStopWordList
        {
            get
            {
                return new string[]
                {
                    "a","about","above","across","after","again","against","all",
                    "almost","alone","along","already","also","although",
                    "always","among","an","and","another","any","anybody","anyone",
                    "anything","anywhere","are","area","areas","around",
                    "as","ask","asked","asking","asks","at","away","b","back",
                    "backed","backing","backs","be","became","because","become",
                    "becomes","been","before","began","behind","being","beings",
                    "best","better","between","big","both","but","by","c","came",
                    "can","cannot","case","cases","certain","certainly","clear",
                    "clearly","come","could","d","did","differ","different",
                    "differently","do","does","done","down","downed","downing",
                    "downs","during","e","each","early","either","end","ended",
                    "ending","ends","enough","even","evenly","ever","every",
                    "everybody","everyone","everything","everywhere","f","face",
                    "faces","fact","facts","far","felt","few","find","finds",
                    "first","for","four","from","full","fully","further","furthered",
                    "furthering","furthers","g","gave","general","generally",
                    "get","gets","give","given","gives","go","going","good",
                    "goods","got","great","greater","greatest","group","grouped",
                    "grouping","groups","h","had","has","have","having","he",
                    "her","here","herself","high","higher","highest","him",
                    "himself","his","how","however","i","if","important","in","interest",
                    "interested","interesting","interests","into","is","it","its",
                    "itself","j","just","k","keep","keeps","kind","knew","know",
                    "known","knows","l","large","largely","last","later","latest",
                    "least","less","let","lets","like","likely","long","longer",
                    "longest","m","made","make","making","man","many","may","me",
                    "member","members","men","might","more","most","mostly",
                    "mr","mrs","much","must","my","myself","n","necessary","need",
                    "needed","needing","needs","never","new","newer","newest",
                    "next","no","nobody","non","noone","not","nothing","now",
                    "nowhere","number","numbers","o","of","off","often","old",
                    "older","oldest","on","once","one","only","open","opened",
                    "opening","opens","or","order","ordered","ordering","orders",
                    "other","others","our","out","over","p","part","parted",
                    "parting","parts","per","perhaps","place","places","point",
                    "pointed","pointing","points","possible","present","presented",
                    "presenting","presents","problem","problems","put",
                    "puts","q","quite","r","rather","really","right","room",
                    "rooms","s","said","same","saw","say","says","second","seconds",
                    "see","seem","seemed","seeming","seems","sees","several",
                    "shall","she","should","show","showed","showing","shows","side",
                    "sides","since","small","smaller","smallest","so","some",
                    "somebody","someone","something","somewhere","state","states","still",
                    "such","sure","t","take","taken","than","that","the","their",
                    "them","then","there","therefore","these","they","thing","things",
                    "think","thinks","this","those","though","thought","thoughts",
                    "three","through","thus","to","today","together","too","took",
                    "toward","turn","turned","turning","turns","two","u","under",
                    "until","up","upon","us","use","used","uses","v","very","w","want",
                    "wanted","wanting","wants","was","way","ways","we","well","wells",
                    "went","were","what","when","where","whether","which","while",
                    "who","whole","whose","why","will","with","within","without",
                    "work","worked","working","works","would","x","y","year","years",
                    "yet","you","young","younger","youngest","your","yours","z"
                };
            }
        }

        public static string[] ShorterStopWordList
        {
            get
            {
                return new string[]
                {
                    "a","about","above","after","again","against","all","am",
                    "an","and","any","are","aren't","as","at","be","because",
                    "been","before","being","below","between","both","but",
                    "by","can't","cannot","could","couldn't","did","didn't",
                    "do","does","doesn't","doing","don't","down","during",
                    "each","few","for","from","further","had","hadn't",
                    "has","hasn't","have","haven't","having","he",
                    "he'd","he'll","he's","her","here","here's","hers",
                    "herself","him","himself","his","how","how's","i",
                    "i'd","i'll","i'm","i've","if","in","into","is","isn't",
                    "it","it's","its","itself","let's","me","more","most",
                    "mustn't","my","myself","no","nor","not","of","off",
                    "on","once","only","or","other",
                    "ought","our","ours","ourselves","out","over","own",
                    "same","shan't","she","she'd","she'll","she's","should",
                    "shouldn't","so","some",
                    "such","than","that","that's","the","their","theirs",
                    "them","themselves","then","there","there's","these",
                    "they","they'd","they'll",
                    "they're","they've","this","those","through","to","too",
                    "under","until","up","very","was","wasn't","we","we'd",
                    "we'll","we're","we've",
                    "were","weren't","what","what's","when","when's","where",
                    "where's","which","while","who","who's","whom","why",
                    "why's","with","won't",
                    "would","wouldn't","you","you'd","you'll","you're",
                    "you've","your","yours","yourself","yourselves"
                };
            }
        }

        public static string[] BibleStopWordList
        {
            get
            {
                return new string[]
                {
                    "a","about","above","after","again","against","all","am",
                    "an","and","any","are","aren't","as","at","be",
                    "because","been","before",
                    "being","below","between","both","but","by","can't",
                    "cannot","could","couldn't","did","didn't","do","does",
                    "doesn't","doing","don't",
                    "down","during","each","few","for","from","further",
                    "had","hadn't","has","hasn't","have","haven't","having",
                    "he","he'd","he'll","he's",
                    "her","here","here's","hers","herself","him","himself",
                    "his","how","how's","i","i'd","i'll","i'm","i've","if",
                    "in","into","is","isn't",
                    "it","it's","its","itself","let's","me","more","most",
                    "mustn't","my","myself","no","nor","not","of","off","on",
                    "once","only","or","other",
                    "ought","our","ours","ourselves","out","over","own",
                    "same","shan't","she","she'd","she'll","she's",
                    "should","shouldn't","so","some",
                    "such","than","that","that's","the","their","theirs",
                    "them","themselves","then","there","there's","these",
                    "they","they'd","they'll",
                    "they're","they've","this","those","through","to",
                    "too","under","until","up","very","was","wasn't","we",
                    "we'd","we'll","we're","we've",
                    "were","weren't","what","what's","when","when's",
                    "where","where's","which","while","who","who's","whom",
                    "why","why's","with","won't",
                    "would","wouldn't","you","you'd","you'll","you're",
                    "you've","your","yours","yourself","yourselves",
                    "thee","thou","shalt","shall","thus","unto","thy",
                    "amen","hast","said","saying","upon","let","hath",
                    "may","yet","way","art","say","betwixt",
                    "thi","thing","come","came","mani"
                };
            }
        }
       
        public static string[] Punctuation
        {
            get
            {
                return new string[]
                {
                    // Feel free to add to this, I just put it here to
                    // help remove punctuation from
                    // my implementation - Ryan
                    ".", "!", "?", ",", ";", ":", "'", "\"",
                    "\n", "(", ")", "<", ">"

                };
            }
        }

        private static IDictionary<string, IDictionary<int,
                            IDictionary<int, string>>> BuildBible()
        {
            IDictionary<string, IDictionary<int, 
                       IDictionary<int, string>>> result 
                       = new Dictionary<string, IDictionary<int, IDictionary<int, string>>>();
            List<string> BibleNames = Constants.BibleBooks.Keys.ToList();

            int bkInd = 0;
            foreach (string path in Directory
                .EnumerateFiles(@"..\..\..\Summarizer\Documents\The Bible txt - Original"))
            {
                string text = System.IO.File.ReadAllText(path);
                string book = BibleNames[bkInd];
                result[book] = new Dictionary<int, IDictionary<int, string>>();

                for (int chap = 1; chap <= Constants.BibleBooks[BibleNames[bkInd]]; chap++)
                {
                    result[book][chap] = new Dictionary<int, string>();

                    string regexPattern = string.Format(@"{0}:[\d]+[\D]*", chap);

                    int verse = 1;
                    foreach (var match in Regex.Matches(text, regexPattern))
                    {
                        int txtVerse = Int32.Parse(match.ToString().Split(':')[1].Split(' ')[0]);
                        if (txtVerse < verse)
                            break;

                        result[book][chap][verse] = match.ToString();
                        verse++;
                    }
                }
                bkInd++;
                if (bkInd == 66)
                    break;
            }
            return result;
        }
    }
}
