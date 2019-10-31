using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoiceWav
{
    public class SpeechVocie
    {
        private static SpeechLib.SpVoice spVoice = new SpeechLib.SpVoice();
        public static void Speak(string text, int rate = 1)
        {
            try
            {
                spVoice.Rate = rate;
                spVoice.Speak(text, SpeechLib.SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechLib.SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
            }
            catch
            {

            }
        }
        public static void SpeakToFile(string text, string filename, int rate = 1)
        {
            SpeechLib.SpVoice voice = spVoice;
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
}
