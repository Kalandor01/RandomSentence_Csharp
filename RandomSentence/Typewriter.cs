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
        readonly string text;
        /// <summary>
        /// Controlls how many millisecond it should wait between writing out letters (or how long it should take to write out the text).
        /// </summary>
        readonly double delay;
        /// <summary>
        /// If false, <c>delay</c> is how many seconds it should take to write out the entire text.
        /// </summary>
        readonly bool isDelayPerLetter;
        /// <summary>
        /// Path to a sound file.<br/>
        /// It will play before the text starts printing out.
        /// </summary>
        readonly string? soundBegin;
        /// <summary>
        /// Path to a sound file.<br/>
        /// It will play every time a letter is printed.
        /// </summary>
        readonly string? sound;
        /// <summary>
        ///  Controlls if the method should wait for the sound defined if <c>soundBegin</c>, to finnish playing before continuing.
        /// </summary>
        readonly bool soundBeginWait;
        /// <summary>
        ///  Controlls if the method should wait for the sound defined if <c>sound</c>, to finnish playing before continuing.
        /// </summary>
        readonly bool soundWait;
        /// <summary>
        /// Whether the object should throw a <c>NotSupportedException</c>, if the platform is not Windows, or just not play the sound.
        /// </summary>
        readonly bool throwExeption;
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
        /// <param name="soundBeginWait"><inheritdoc cref="soundBeginWait" path="//summary"/></param>
        /// <param name="soundWait"><inheritdoc cref="soundWait" path="//summary"/></param>
        /// <param name="throwExeption"><inheritdoc cref="throwExeption" path="//summary"/></param>
        public Typewriter(string text, double delay = 4, bool isDelayPerLetter = true, string? soundBegin = null, string? sound = null, bool soundBeginWait = false, bool soundWait = false, bool throwExeption = false)
        {
            this.text = text;
            this.delay = delay;
            this.isDelayPerLetter = isDelayPerLetter;
            this.soundBegin = soundBegin;
            this.sound = sound;
            this.soundBeginWait = soundBeginWait;
            this.soundWait = soundWait;
            this.throwExeption = throwExeption;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Writes out the text with the settings from the object.
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public void Write()
        {
            // sound?
            var soundError = false;
            var playBeginSound = !string.IsNullOrWhiteSpace(soundBegin);
            var playMidSound = !string.IsNullOrWhiteSpace(sound);
            if (
                !RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
                (playBeginSound || playMidSound)
            )
            {
                if (throwExeption)
                {
                    throw new NotSupportedException("Sound can only be played os Windows.");
                }
                else
                {
                    soundError = true;
                }
            }
            // begin sound
            if (playBeginSound && !soundError)
            {
                var beginSound = new SoundPlayer(soundBegin);
                if (soundBeginWait)
                {
                    beginSound.PlaySync();
                }
                else
                {
                    beginSound.Play();
                }
            }
            // typewriter
            for (var x = 0; x < text.Length; x++)
            {
                Console.Write(text[x]);
                if (x != text.Length - 1)
                {
                    // sound
                    if (playMidSound && !soundError)
                    {
                        var midSound = new SoundPlayer(sound);
                        if (soundWait)
                        {
                            midSound.PlaySync();
                        }
                        else
                        {
                            midSound.Play();
                        }
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
