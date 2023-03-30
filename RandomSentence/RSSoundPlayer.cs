using System.Media;
using System.Runtime.InteropServices;

namespace RandomSentence
{
    /// <summary>
    /// Object for playing a sound.
    /// </summary>
    public class RSSoundPlayer
    {

        #region Public fields
        /// <summary>
        /// Whether to wait for the sound to finish playing, before continuing.
        /// </summary>
        public bool wait;
        #endregion

        #region Public properties
        /// <summary>
        /// The path to the sound.
        /// </summary>
        public string SoundPath
        {
            get { return soundPath; }
            set => SetSound(value);
        }
        #endregion

        #region Private fields
        private SoundPlayer sound;
        private string soundPath;
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="RSSoundPlayer"/>
        /// </summary>
        /// <param name="soundPath"><inheritdoc cref="SoundPath" path="//summary"/></param>
        /// <param name="wait"><inheritdoc cref="wait" path="//summary"/></param>
        /// <exception cref="ArgumentException">Exeption thrown if the file doesn't exist or it's not a .wav file.</exception>
        /// <exception cref="PlatformNotSupportedException">Exeption thrown if the OS isn't Windows.</exception>
        public RSSoundPlayer(string soundPath, bool wait=false)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("Sounds can only be played os Windows.");
            }
            SetSound(soundPath);
            this.wait = wait;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Plays ste sound.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Exeption thrown if the OS isn't Windows.</exception>
        public void Play()
        {
            if (this.wait)
            {
                sound.PlaySync();
            }
            else
            {
                sound.Play();
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Sets the sound path and the sound.
        /// </summary>
        /// <param name="soundPath"><inheritdoc cref="SoundPath" path="//summary"/></param>
        /// <exception cref="ArgumentException">Exeption thrown in the file doesn't exist or it's not a .wav file.</exception>
        /// <exception cref="PlatformNotSupportedException">Exeption thrown if the OS isn't Windows.</exception>
        private void SetSound(string soundPath)
        {
            if (File.Exists(soundPath) && Path.GetExtension(soundPath) != "wav")
            {
                this.soundPath = soundPath;
                sound = new SoundPlayer(this.soundPath);
            }
            else
            {
                throw new ArgumentException("Existing .wav file not found.");
            }
        }
        #endregion
    }
}
