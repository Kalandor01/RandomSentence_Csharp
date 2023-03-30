using System.Media;
using System.Runtime.InteropServices;

namespace RandomSentence
{
    /// <summary>
    /// Object for writing out text letter-by-letter, optionaly with sound, with a lot of controll.<br/>
    /// If the OS is not Windows, the sound will not play (by deffault).
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
        public double delay;
        /// <summary>
        /// If false, <c>delay</c> is how many seconds it should take to write out the entire text.
        /// </summary>
        public bool isDelayPerLetter;
        /// <summary>
        /// A sound object.<br/>
        /// It will play before the text starts printing out.
        /// </summary>
        public RSSoundPlayer? soundBegin;
        /// <summary>
        /// A sound object.<br/>
        /// It will play every time a letter is printed.
        /// </summary>
        public RSSoundPlayer? sound;
        /// <summary>
        /// Whether the object should throw a <c>NotSupportedException</c>, if the platform is not Windows, or just not play the sound.
        /// </summary>
        public bool throwExeption;
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
        /// <param name="throwExeption"><inheritdoc cref="throwExeption" path="//summary"/></param>
        /// <exception cref="PlatformNotSupportedException">Exeption thrown if the OS isn't Windows, and <c>throwExeption</c> is true.</exception>
        public Typewriter(string text, double delay = 4, bool isDelayPerLetter = true, RSSoundPlayer? soundBegin = null, RSSoundPlayer? sound = null, bool throwExeption = false)
        {
            if (
                !RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
                (soundBegin is not null || sound is not null) &&
                throwExeption
            )
            {
                throw new PlatformNotSupportedException("Sounds can only be played os Windows.");
            }

            this.text = text;
            this.delay = delay;
            this.isDelayPerLetter = isDelayPerLetter;
            this.soundBegin = soundBegin;
            this.sound = sound;
            this.throwExeption = throwExeption;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Writes out the text with the settings from the object.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Exeption thrown if the OS isn't Windows, and <c>throwExeption</c> is true.</exception>
        public void Write()
        {
            // sound?
            var soundError = false;
            if (
                !RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
                (soundBegin is not null || sound is not null)
            )
            {
                if (throwExeption)
                {
                    throw new PlatformNotSupportedException("Sounds can only be played os Windows.");
                }
                else
                {
                    soundError = true;
                }
            }
            // begin sound
            if (soundBegin is not null && !soundError)
            {
                soundBegin.Play();
            }
            // typewriter
            for (var x = 0; x < text.Length; x++)
            {
                Console.Write(text[x]);
                if (x != text.Length - 1)
                {
                    // sound
                    if (sound is not null && !soundError)
                    {
                        sound.Play();
                    }
                    // delay type
                    if (isDelayPerLetter)
                    {
                        Thread.Sleep((int)delay);
                    }
                    else
                    {
                        Thread.Sleep((int)(delay / text.Length * 1000));
                    }
                }
            }
        }
        #endregion
    }
}
