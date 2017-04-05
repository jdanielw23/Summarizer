using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model.Ryan_s_Implementation;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;
using System.Diagnostics;

namespace SummarizerTest.ModelTest
{
    [TestClass]
    public class SummarizerRRTest
    {
        [TestMethod]
        public void TestSummarizerTooShort()
        {
            SummarizerRR summarizer = new SummarizerRR();     
            string summarizerTooShort = summarizer.Summarize("This text is way too short.");
            Assert.AreEqual(summarizerTooShort, SummarizerRR.TooShortMessage);
        }

        [TestMethod]
        public void TestSummarizerNotEnoughWords()
        {
            SummarizerRR summarizer = new SummarizerRR();
            string summarizerTooShort = summarizer.Summarize("This text is way too short. But it needs. At least three sentences to work.");
            Assert.AreEqual(summarizerTooShort, SummarizerRR.NotEnoughWordsMessage);
        }

        [TestMethod]
        public void TestPOSTagger()
        {
            /*  CC Coordinating conjunction
                CD Cardinal number
                DT Determiner
                EX Existential there
                FW Foreign word
                IN Preposition or subordinating conjunction
                JJ Adjective
                JJR Adjective, comparative
                JJS Adjective, superlative
                LS List item marker
                MD Modal
                NN Noun, singular or mass
                NNS Noun, plural
                NNP Proper noun, singular
                NNPS Proper noun, plural
                PDT Predeterminer
                POS Possessive ending
                PRP Personal pronoun
                PRP$ Possessive pronoun
                RB Adverb
                RBR Adverb, comparative
                RBS Adverb, superlative
                RP Particle
                SYM Symbol
                TO to
                UH Interjection
                VB Verb, base form
                VBD Verb, past tense
                VBG Verb, gerund or present participle
                VBN Verb, past participle
                VBP Verb, non­3rd person singular present
                VBZ Verb, 3rd person singular present
                WDT Wh­determiner
                WP Wh­pronoun
                WP$ Possessive wh­pronoun
                WRB Wh­adverb
            //*/

            var jarRoot = @"..\..\Resources\stanford-postagger-full-2016-10-31";
            var modelsDirectory = jarRoot + @"\models";

            // Loading POS Tagger
            var tagger = new MaxentTagger(modelsDirectory + @"\wsj-0-18-bidirectional-nodistsim.tagger");

            const string text = "The quick brown fox jumped over the lazy dog.";
            string taggedString = tagger.tagString(text);

            Assert.AreEqual("The_DT quick_JJ brown_JJ fox_NN jumped_VBD over_IN the_DT lazy_JJ dog_NN ._. ", taggedString);
        }
    }


}
