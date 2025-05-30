namespace RandomSentence
{
    /// <summary>
    /// Object for writing out text letter-by-letter, optionaly with sound, with a lot of controll.
    /// </summary>
    public class Typewriter
    {
        #region Private fields
        /// <summary>
        /// The text to write,
        /// </summary>
        public string text;
        /// <summary>
        /// Controlls how many millisecond it should wait between writing out letters (or how long it should take to write out the text).
        /// </summary>
        public int delay;
        /// <summary>
        /// If false, <see cref="delay"/> is how long it should take to write out the entire text.
        /// </summary>
        public bool isDelayPerLetter;
        /// <summary>
        /// The sound to play before the text starts printing out.
        /// </summary>
        public ISimpleSoundPlayer? soundBegin;
        /// <summary>
        /// The sound to play every time a letter is printed.
        /// </summary>
        public ISimpleSoundPlayer? sound;
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="Typewriter"/>
        /// </summary>
        /// <param name="text"><inheritdoc cref="text" path="//summary"/></param>
        /// <param name="delay"><inheritdoc cref="delay" path="//summary"/></param>
        /// <param name="isDelayPerLetter"><inheritdoc cref="isDelayPerLetter" path="//summary"/></param>
        /// <param name="soundBegin"><inheritdoc cref="soundBegin" path="//summary"/></param>
        /// <param name="sound"><inheritdoc cref="sound" path="//summary"/></param>
        public Typewriter(
            string text,
            int delay = 4,
            bool isDelayPerLetter = true,
            ISimpleSoundPlayer? soundBegin = null,
            ISimpleSoundPlayer? sound = null
        )
        {
            this.text = text;
            this.delay = delay;
            this.isDelayPerLetter = isDelayPerLetter;
            this.soundBegin = soundBegin;
            this.sound = sound;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Writes out the text with the settings from the object.
        /// </summary>
        public void Write()
        {
            soundBegin?.Play();

            var soundDelay = isDelayPerLetter ? delay : delay / text.Length;
            for (var x = 0; x < text.Length; x++)
            {
                Console.Write(text[x]);
                if (x != text.Length - 1)
                {
                    sound?.Play();

                    // delay type
                    if (isDelayPerLetter)
                    {
                        Thread.Sleep(soundDelay);
                    }
                    else
                    {
                        Thread.Sleep(soundDelay);
                    }
                }
            }
        }
        #endregion
    }
}
