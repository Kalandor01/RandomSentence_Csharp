using System.Media;
using System.Runtime.InteropServices;

namespace RandomSentence
{
    public class TestClass
    {
        /// <summary>
        /// Plays sound.
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public static void Test(string soundPath)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var sound = new SoundPlayer(soundPath);
                sound.PlaySync();
                //sound.Play();
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}