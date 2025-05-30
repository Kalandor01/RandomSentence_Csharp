using NAudio.Wave;

namespace RandomSentence
{
    /// <summary>
    /// Object for playing a .wav sound.
    /// </summary>
    public class RSSoundPlayer : ISimpleSoundPlayer
    {
        #region Public properties
        public bool Wait { get; set; }
        public string SoundPath { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="RSSoundPlayer"/>
        /// </summary>
        /// <param name="soundPath"><inheritdoc cref="SoundPath" path="//summary"/></param>
        /// <param name="wait"><inheritdoc cref="Wait" path="//summary"/></param>
        public RSSoundPlayer(string soundPath, bool wait = false)
        {
            SoundPath = soundPath;
            Wait = wait;
        }
        #endregion

        #region Public methods
        public void Play(bool? wait = null)
        {
            wait ??= Wait;
            var fileReader = new AudioFileReader(SoundPath);
            var soundPlayer = new WaveOutEvent();
            soundPlayer.Init(fileReader);

            soundPlayer.Play();
            if ((bool)wait)
            {
                while (soundPlayer.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(1);
                }
                soundPlayer.Dispose();
                fileReader.Dispose();
            }
            else
            {
                soundPlayer.PlaybackStopped += (s, e) => SoundStopped(fileReader, soundPlayer);
            }
        }

        private static void SoundStopped(AudioFileReader fileReader, WaveOutEvent soundPlayer)
        {
            soundPlayer.Dispose();
            fileReader.Dispose();
        }
        #endregion
    }
}
