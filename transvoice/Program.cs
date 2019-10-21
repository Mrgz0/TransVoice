using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace transvoice
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                DispHelp();
                return;
            }

            string opt = args[0];

            if (opt == "-v")
            {
                if (args.Length!=2 && args.Length != 3)
                {
                    DispHelp();
                    return;
                }
                string content = args[1];
                int rate = 1;
                if (args.Length == 3)
                {
                    rate = Int32.Parse(args[2]);
                }

                SpeechVocie.Speak(content, rate);
            }
            else if (opt=="-f")
            {
                if (args.Length != 3 && args.Length != 4)
                {
                    DispHelp();
                    return;
                }
                string content = File.ReadAllText(args[1]);
                var arr = content.Replace("\r", "").Split(new char[] { '\n' });
                string basePath = args[2];
                int rate = 1;
                if (args.Length == 4)
                {
                    rate = Int32.Parse(args[3]);
                }
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                foreach (var item in arr)
                {
                    var savePath = Path.Combine(basePath, SafeFileName(item) + ".wav");
                    SpeechVocie.SpeakToFile(item, savePath, rate);
                    Console.WriteLine("Speek wav " + item);
                }
            }
            else
            {
                DispHelp();
                return;
            }
        }

        static string SafeFileName(string s)
        {
            //名称处理\:/*?"<>|
            return System.Text.RegularExpressions.Regex.Replace(s, "[\\\\:/*?\"<>\r\n\\s]", "_");
        }
        static void DispHelp()
        {
            string[] help = {
                "Usage transvoice",
                "-v <content> [rate]",
                "-f <inputfile> <outputdir> [rate]"
            };
            foreach (var item in help)
            {
                Console.WriteLine(item);
            }
        }
    }
}
