namespace RandomSentence
{
    public interface ISimpleSoundPlayer
    {
        /// <summary>
        /// Whether to wait for the sound to finish playing, before continuing.
        /// </summary>
        public bool Wait { get; set; }

        #region Public properties
        /// <summary>
        /// The path to the sound.
        /// </summary>
        public string SoundPath { get; }
        #endregion

        /// <summary>
        /// Plays the sound.
        /// </summary>
        /// <param name="wait"><inheritdoc cref="Wait" path="//summary"/><br/>
        /// If null it does what <see cref="Wait"/> is set to.</param>
        public void Play(bool? wait = null);
    }
}