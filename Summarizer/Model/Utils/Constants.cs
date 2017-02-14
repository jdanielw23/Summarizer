﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Utils
{
    public static class Constants
    {
        public static string[] LongStopWordList
        {
            get
            {
                return new string[]
                {
                    "a","about","above","across","after","again","against","all","almost","alone","along","already","also","although",
                    "always","among","an","and","another","any","anybody","anyone","anything","anywhere","are","area","areas","around",
                    "as","ask","asked","asking","asks","at","away","b","back","backed","backing","backs","be","became","because","become",
                    "becomes","been","before","began","behind","being","beings","best","better","between","big","both","but","by","c","came",
                    "can","cannot","case","cases","certain","certainly","clear","clearly","come","could","d","did","differ","different",
                    "differently","do","does","done","down","downed","downing","downs","during","e","each","early","either","end","ended","ending",
                    "ends","enough","even","evenly","ever","every","everybody","everyone","everything","everywhere","f","face","faces",
                    "fact","facts","far","felt","few","find","finds","first","for","four","from","full","fully","further","furthered",
                    "furthering","furthers","g","gave","general","generally","get","gets","give","given","gives","go","going","good",
                    "goods","got","great","greater","greatest","group","grouped","grouping","groups","h","had","has","have","having","he",
                    "her","here","herself","high","higher","highest","him","himself","his","how","however","i","if","important","in","interest",
                    "interested","interesting","interests","into","is","it","its","itself","j","just","k","keep","keeps","kind","knew","know",
                    "known","knows","l","large","largely","last","later","latest","least","less","let","lets","like","likely","long","longer",
                    "longest","m","made","make","making","man","many","may","me","member","members","men","might","more","most","mostly",
                    "mr","mrs","much","must","my","myself","n","necessary","need","needed","needing","needs","never","new","newer","newest",
                    "next","no","nobody","non","noone","not","nothing","now","nowhere","number","numbers","o","of","off","often","old",
                    "older","oldest","on","once","one","only","open","opened","opening","opens","or","order","ordered","ordering","orders",
                    "other","others","our","out","over","p","part","parted","parting","parts","per","perhaps","place","places","point",
                    "pointed","pointing","points","possible","present","presented","presenting","presents","problem","problems","put",
                    "puts","q","quite","r","rather","really","right","room","rooms","s","said","same","saw","say","says","second","seconds",
                    "see","seem","seemed","seeming","seems","sees","several","shall","she","should","show","showed","showing","shows","side",
                    "sides","since","small","smaller","smallest","so","some","somebody","someone","something","somewhere","state","states","still",
                    "such","sure","t","take","taken","than","that","the","their","them","then","there","therefore","these","they","thing","things",
                    "think","thinks","this","those","though","thought","thoughts","three","through","thus","to","today","together","too","took",
                    "toward","turn","turned","turning","turns","two","u","under","until","up","upon","us","use","used","uses","v","very","w","want",
                    "wanted","wanting","wants","was","way","ways","we","well","wells","went","were","what","when","where","whether","which","while",
                    "who","whole","whose","why","will","with","within","without","work","worked","working","works","would","x","y","year","years",
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
                    "a","about","above","after","again","against","all","am","an","and","any","are","aren't","as","at","be","because","been","before",
                    "being","below","between","both","but","by","can't","cannot","could","couldn't","did","didn't","do","does","doesn't","doing","don't",
                    "down","during","each","few","for","from","further","had","hadn't","has","hasn't","have","haven't","having","he","he'd","he'll","he's",
                    "her","here","here's","hers","herself","him","himself","his","how","how's","i","i'd","i'll","i'm","i've","if","in","into","is","isn't",
                    "it","it's","its","itself","let's","me","more","most","mustn't","my","myself","no","nor","not","of","off","on","once","only","or","other",
                    "ought","our","ours","ourselves","out","over","own","same","shan't","she","she'd","she'll","she's","should","shouldn't","so","some",
                    "such","than","that","that's","the","their","theirs","them","themselves","then","there","there's","these","they","they'd","they'll",
                    "they're","they've","this","those","through","to","too","under","until","up","very","was","wasn't","we","we'd","we'll","we're","we've",
                    "were","weren't","what","what's","when","when's","where","where's","which","while","who","who's","whom","why","why's","with","won't",
                    "would","wouldn't","you","you'd","you'll","you're","you've","your","yours","yourself","yourselves"
                };
            }
        }

        public static string[] BibleStopWordList
        {
            get
            {
                return new string[]
                {
                    "a","about","above","after","again","against","all","am","an","and","any","are","aren't","as","at","be","because","been","before",
                    "being","below","between","both","but","by","can't","cannot","could","couldn't","did","didn't","do","does","doesn't","doing","don't",
                    "down","during","each","few","for","from","further","had","hadn't","has","hasn't","have","haven't","having","he","he'd","he'll","he's",
                    "her","here","here's","hers","herself","him","himself","his","how","how's","i","i'd","i'll","i'm","i've","if","in","into","is","isn't",
                    "it","it's","its","itself","let's","me","more","most","mustn't","my","myself","no","nor","not","of","off","on","once","only","or","other",
                    "ought","our","ours","ourselves","out","over","own","same","shan't","she","she'd","she'll","she's","should","shouldn't","so","some",
                    "such","than","that","that's","the","their","theirs","them","themselves","then","there","there's","these","they","they'd","they'll",
                    "they're","they've","this","those","through","to","too","under","until","up","very","was","wasn't","we","we'd","we'll","we're","we've",
                    "were","weren't","what","what's","when","when's","where","where's","which","while","who","who's","whom","why","why's","with","won't",
                    "would","wouldn't","you","you'd","you'll","you're","you've","your","yours","yourself","yourselves",
                    "thee","thou","shalt","shall","thus","unto","thy","amen","hast","said","saying","upon","let","hath","may","yet","way","art","say","betwixt",
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
                        // Feel free to add to this, I just put it here to help remove punctuation from
                        // my implementation - Ryan
                        ".", "!", "?", ",", ";", ":", "'", "\"", "\n", "(", ")", "<", ">"

                };
            }
        }
    }
}
