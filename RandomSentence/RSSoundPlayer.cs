using System.Media;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace RandomSentence
{
    /// <summary>
    /// Object for playing a sound.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class RSSoundPlayer
    {
        #region Public fields
        /// <summary>
        /// Whether to wait for the sound to finish playing, before continuing.
        /// </summary>
        public bool wait;
        #endregion

        #region Private fields
        /// <summary>
        /// The sound player.
        /// </summary>
        private SoundPlayer _soundPlayer;
        /// <summary>
        /// The path to the sound.
        /// </summary>
        private string _soundPath;
        #endregion

        #region Public properties
        /// <summary>
        /// <inheritdoc cref="_soundPath" path="//summary"/>
        /// </summary>
        public string SoundPath
        {
            get => _soundPath;
            set => SetSound(value);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="RSSoundPlayer"/>
        /// </summary>
        /// <param name="soundPath"><inheritdoc cref="SoundPath" path="//summary"/></param>
        /// <param name="wait"><inheritdoc cref="wait" path="//summary"/></param>
        /// <exception cref="ArgumentException">Exeption thrown if the file doesn't exist or it's not a .wav file.</exception>
        /// <exception cref="PlatformNotSupportedException">Exeption thrown if the OS isn't Windows.</exception>
        public RSSoundPlayer(string soundPath, bool wait = false)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("Sounds can only be played os Windows.");
            }
            SoundPath = soundPath;
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
            if (wait)
            {
                _soundPlayer.PlaySync();
            }
            else
            {
                _soundPlayer.Play();
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
                _soundPath = soundPath;
                _soundPlayer = new SoundPlayer(_soundPath);
            }
            else
            {
                throw new ArgumentException("Existing .wav file not found.");
            }
        }
        #endregion
    }
}
