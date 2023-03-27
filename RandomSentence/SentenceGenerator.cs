using System.Text;
using NPrng;
using NPrng.Generators;

namespace RandomSentence
{
    /// <summary>
    /// Contains functions for generating a sentence.
    /// </summary>
    public static class SentenceGenerator
    {
        #region Private Constants
        private static readonly char[] DEFAULT_LETTERS = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
        private static readonly IPseudoRandomGenerator DEFAULT_GENERATOR = new SplittableRandom();

        private static readonly object[] W_NOUN = { " this", " that", " car", " cat", " ice cream", " building", " house", " freezer", " doll", " art", " computer", " code", " table", " chair", " mouse", " keyboard", " monitor", " processor", " ram", " fruit", " vegetable", " desk", " pen", " pencil", " gun", " death", " paint", " brush", " shoe", " pants", " shirt", " glasses", " glass", " nose", " hair", " head", " eye", " leg", "arm", " sofa", " brain", " neuron", " dog", " parrot", " snake", " python", " hamster", " bird", " mamal", " human", " robot", " AI", " pig", " horse", " reptile", " box", " knee", " shoulder", " toe", " finger", " lamp", " rock", " mountain", " gease", " swan", " boulder", " spear", " phone", " letter", " word", " sentance", " board", " plane", " helicopter", " rocket", " space ship", " space station", " suit", " space suit", " sand", " concrete", " steel", " fire", " engine", " gas", " liquid", " water", " lava", " magma", " volcano", " universe", " galaxy", " star", " wood", " oxygen", " hydrogen", " door", " lazer", " Earth", " hat", " ball", " globe", " sphere", " Sun", " Europe", " Amerika", " moon", " city", " bridge", " village", " fuel", " explosion", " root", " tree", " plastic", " gold", " money", " diamond", " teeth", " glue", " medal", " cup" };
        private static readonly object[] W_PRONOUN = { new string[] { " I am", " I was", " I will be", " I" }, new string[] { " you are", " you were", " you will be", " you" }, new string[] { " he is", " he was", " he will be", " he" }, new string[] { " she is", " she was", " she will be", " she" }, new string[] { " it is", " it was", " it will be", " it" }, new string[] { " they are", " they were", " they will be", " they" }, " them", " me", new string[] { " we are", " we were", " we will be", " we" }, " us" };
        private static readonly object[] W_VERB = { new string[] { " eating", " ate", " eating", " eat" }, new string[] { " giving", " gave", " giving", " give" }, " take", " do", " make", " destroy", " code", " can", " slap", " kick", " leave", " go", " morn", " capture", " run", " walk", " jog", " climb", " move", " sense", " hear", " see", " taste", " lick", " die", " answer", " fight", " travel", " touch", " feel", " live", " become", " pray", " cry", " clap", " think", " kill", " build", " laugh", " train", " excercierse", " read", " teach", " count", " begin", " bend", " break", " drink", " disapear", " shout", " transform", " finish", " restart", " imagine", " create", " lift", " bounce", " fall", " reach" };
        private static readonly object[] W_ADJECTIVE = { " a", " an", " the", " cute", " nice", " wrong", " medium", " big", " small", " hairy", " fat", " fast", " slow", " easy", " hard", " blue", " red", " green", " white", " black", " tall", " short", " wide", " thin", " angry", " happy", " sad", " growling", " surprised", " moved", " transparrent", " soft", " golden", " tough", " conductive", " light", " heavy", " pale", " matt", " shiny", " hungry", " full", " missing", " found", " interesting", " broken", " fixed", " trapped", " freed", " free", " boring", " automatic", " dramatic", " horifying", " agrovating", " stupid", " smart", " dumb" };
        private static readonly object[] W_ADVERB = { " then", " quickly", " slowly", " now", " soon", " lately", " easily", " surprisingly", " accidentaly", " worryingly", " gently", " extremely", " carefully", " well", " amazingly", " totaly", " acrobaticly" };


        private static readonly string[] W_ASK = { " who", " when", " why", " where", " what", " are", " is" };
        private static readonly string[] W_BETWEEN = { " is", " with", " at", " on", " in", " are", " have" };
        private static readonly string[] W_END = { ".", "!", "?", "?!", "!?", " :)", " :(", " :D", " :C", " XD", "", "..." };
        private static readonly string[] W_END_ASK = { "?", "?!", "!?" };

        private static readonly Dictionary<WordType, object[]> WORDS = new() {
            [WordType.NOUN] = W_NOUN,
            [WordType.PRONOUN] = W_PRONOUN,
            [WordType.VERB] = W_VERB,
            [WordType.ADJECTIVE] = W_ADJECTIVE,
            [WordType.ADVERB] = W_ADVERB,
            [WordType.ASK] = W_ASK,
            [WordType.BETWEEN] = W_BETWEEN,
            [WordType.END] = W_END,
            [WordType.END_ASK] = W_END_ASK
        };

        private static readonly Dictionary<WordType, WordType[]> SENTENCE_STRUCTURE = new()
        {
            [WordType.NOUN] = new WordType[] { WordType.VERB, WordType.BETWEEN },
            [WordType.PRONOUN] = new WordType[] { WordType.VERB, WordType.BETWEEN },
            [WordType.VERB] = new WordType[] { WordType.NOUN, WordType.VERB, WordType.BETWEEN },
            [WordType.ADJECTIVE] = new WordType[] { WordType.NOUN },
            [WordType.ADVERB] = new WordType[] { WordType.VERB, WordType.ADJECTIVE, WordType.ADVERB },
            [WordType.ASK] = new WordType[] { WordType.ADJECTIVE, WordType.BETWEEN },
            [WordType.BETWEEN] = new WordType[] { WordType.NOUN, WordType.VERB, WordType.ADJECTIVE },
            [WordType.END] = new WordType[] { WordType.END },
            [WordType.END_ASK] = new WordType[] { WordType.END_ASK },
            [WordType.BEGINNING] = new WordType[] { WordType.NOUN, WordType.PRONOUN, WordType.VERB, WordType.ADJECTIVE, WordType.ADVERB, WordType.ASK }
        };

        private static readonly WordType[] ENDING_WORD = { WordType.NOUN, WordType.VERB };
        // present, past, future, continous
        private static readonly int[] SENTANCE_TIMES = { 0, 1, 2, 3 };
        #endregion

        #region Public functions
        /// <summary>
        /// Returns a random number of random letters (and simbols).<br/>
        /// A characterset can be passed in as an array of characters to use.
        /// </summary>
        /// <param name="minLength">The minimum number of characters to use.</param>
        /// <param name="maxLength">The maximum number of characters to use.</param>
        /// <param name="letters">The caracterset to use.
        /// By default, it uses all english lower and uppder case letters, and space.</param>
        /// <param name="randomGenerator">The generator to use, for generating the letters.</param>
        /// <returns></returns>
        public static string UnstructuredRandom(long minLength = 1, long maxLength = 1000, char[]? letters = null, IPseudoRandomGenerator? randomGenerator = null)
        {
            letters ??= DEFAULT_LETTERS;
            randomGenerator ??= DEFAULT_GENERATOR;

            var text = new StringBuilder();
            var num = randomGenerator.GenerateInRange(minLength, maxLength);
            for (var x = 0; x < num; x++)
            {
                text.Append(letters[randomGenerator.GenerateInRange(0, letters.LongLength - 1)]);
            }
            return text.ToString();
        }

        /// <summary>
        /// Returns a random string that "tries" to match a sentance structure.<br/>
        /// A Word length of 1 produces only a punctuation mark. (0 produces nothing).
        /// </summary>
        /// <param name="minLength">The minimum number of characters to use.</param>
        /// <param name="maxLength">The maximum number of characters to use.</param>
        /// <param name="endOnGoodWord">If true, the sentance will not end until the word type is an ending word.</param>
        /// <param name="generateEnd">If true, the sentance will have an ending punctuation at the end.</param>
        /// <param name="randomGenerator">The generator to use, for generating the words.</param>
        /// <returns></returns>
        public static string StructuredSentence(long minLength = 1, long maxLength = 100, bool endOnGoodWord = true, bool generateEnd = true, IPseudoRandomGenerator? randomGenerator = null)
        {
            randomGenerator ??= DEFAULT_GENERATOR;

            var sentenceType = SENTANCE_TIMES[randomGenerator.GenerateInRange(0, SENTANCE_TIMES.LongLength - 1)];
            var text = new StringBuilder();
            var sentenceLength = randomGenerator.GenerateInRange(minLength, maxLength);
            // nothing
            if (sentenceLength > 0)
            {
                // just end
                WordType? beginningWord = null;
                if (!generateEnd || sentenceLength > 1)
                {
                    // beginning
                    var (previousType, word) = GetNextWord(randomGenerator, WordType.BEGINNING, sentenceType);
                    beginningWord = previousType;
                    if (word.StartsWith(" "))
                    {
                        word = word[1..];
                    }
                    text.Append(char.ToUpper(word[0]) + word[1..]);
                    // words
                    var currentWordNumber = 0;
                    while (currentWordNumber < sentenceLength - 2 || (!ENDING_WORD.Contains(previousType) && endOnGoodWord))
                    {
                        currentWordNumber++;
                        (previousType, word) = GetNextWord(randomGenerator, previousType, sentenceType);
                        text.Append(word);
                    }
                }
                if (generateEnd)
                {
                    // end
                    if (beginningWord is not null && beginningWord == WordType.ASK)
                    {
                        text.Append(GetNextWord(randomGenerator, WordType.END_ASK).word);
                    }
                    else
                    {
                        text.Append(GetNextWord(randomGenerator, WordType.END).word);
                    }
                }
            }
            return text.ToString();
        }
        #endregion

        #region Private functions
        /// <summary>
        /// Returns the next random word, and its type from the previous word type, according to the sentance structure array, and an array of posible words.
        /// </summary>
        /// <param name="randomGenerator">The generator to use, for generating the word.</param>
        /// <param name="previousType">The type of the previous word.</param>
        /// <param name="sentenceType">The type of the sentence.</param>
        /// <returns></returns>
        private static (WordType wordType, string word) GetNextWord(IPseudoRandomGenerator randomGenerator, WordType previousType, int sentenceType = 0)
        {
            // next word type
            var typeNumber = randomGenerator.GenerateInRange(0, SENTENCE_STRUCTURE[previousType].LongLength - 1);
            var wordType = SENTENCE_STRUCTURE[previousType][typeNumber];
            // next word
            var word = WORDS[wordType][randomGenerator.GenerateInRange(0, WORDS[wordType].LongLength - 1)];
            string wordString;
            if (word is string[] wordList && wordList.Any())
            {
                if (wordList.Length > sentenceType)
                {
                    wordString = wordList[sentenceType];
                }
                else
                {
                    wordString = wordList[0];
                }
            }
            else if (word is string wordStr)
            {
                wordString = wordStr;
            }
            else
            {
                wordString = "";
            }
            return (wordType, wordString);
        }
        #endregion
    }
}
