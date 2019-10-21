using SpeechLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transvoice
{
    #region tts 文本朗读类 SAPI
    public class SpeechVocie
    {
        public static void Speak(string text, int rate=1)
        {
            try
            {
                var voice = new SpeechLib.SpVoice();
                voice.Rate = rate;
                voice.Speak(text, SpeechLib.SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
                voice.WaitUntilDone(System.Threading.Timeout.Infinite);
            }
            catch
            {

            }
        }
        public static void SpeakToFile(string text, string filename, int rate=1)
        {
            SpeechLib.SpVoice voice = new SpeechLib.SpVoice();
            voice.Rate = rate;
            SpeechLib.SpFileStream sfs = new SpeechLib.SpFileStream();
            try
            {
                sfs.Open(filename, SpeechLib.SpeechStreamFileMode.SSFMCreateForWrite, false);
                voice.AudioOutputStream = sfs;
                voice.Voice = voice.GetVoices(string.Empty, string.Empty).Item(0);
                voice.Speak("<LANG   LANGID= '804'> " + text, SpeechLib.SpeechVoiceSpeakFlags.SVSFlagsAsync);
                voice.WaitUntilDone(System.Threading.Timeout.Infinite);
            }
            catch
            {
            }
            finally
            {
                sfs.Close();
            }
        }
    }
    #endregion
}
