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
        static string ReadAllText(string filename)
        {
            byte[] bs = File.ReadAllBytes(filename);
            int len = bs.Length;
            if (len >= 3 && bs[0] == 0xEF && bs[1] == 0xBB && bs[2] == 0xBF)
            {
                return Encoding.UTF8.GetString(bs, 3, len - 3);
            }
            int[] cs = { 7, 5, 4, 3, 2, 1, 0, 6, 14, 30, 62, 126 };
            for (int i = 0; i < len; i++)
            {
                int bits = -1;
                for (int j = 0; j < 6; j++)
                {
                    if (bs[i] >> cs[j] == cs[j + 6])
                    {
                        bits = j;
                        break;
                    }
                }
                if (bits == -1)
                {
                    return Encoding.Default.GetString(bs);
                }
                while (bits-- > 0)
                {
                    i++;
                    if (i == len || bs[i] >> 6 != 2)
                    {
                        return Encoding.Default.GetString(bs);
                    }
                }
            }
            return Encoding.UTF8.GetString(bs);
        }

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
                using FileStream fs = new FileStream(args[1], FileMode.Open, FileAccess.Read);

                string content = ReadAllText(args[1]);

                //string content = File.ReadAllText(args[1]);
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
