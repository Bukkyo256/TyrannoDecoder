using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TyrannoDecoder
{
    public class Coder
    {
        public Coder()
        {

        }

        public Coder(Encoding encoding)
        {
            TextEncoding = encoding;
        }

        public Encoding TextEncoding { get; private set; } = Encoding.UTF8;

        public void ActionSelector(string[] args)
        {
            if(args.Length == 0)
            {
                string path = FindSaveFile();
                FileInfo f = new FileInfo(path);
                DumpSaves(f);
            }
            if (args.Length > 0)
            {
                Console.WriteLine(args[0]);
                FileInfo f = new FileInfo(args[0]);
                string fileName = f.Name;
                switch (f.Extension)
                {
                    case ".json":
                        string position = fileName.Replace(f.Extension, "");
                        int pos = int.Parse(position);
                        string save = File.ReadAllText(f.FullName, TextEncoding);
                        InsertSave(FindSaveFile(), save, pos, true);
                        break;
                    case ".sav":
                        DumpSaves(f);
                        break;
                }
            }
        }

        protected string FindSaveFile()
        {
            string[] results = Directory.GetFiles(Directory.GetCurrentDirectory(), "*_tyrano_data.sav");
            return results.FirstOrDefault();
        }

        public void DumpSaves(FileInfo file, bool decode = true)
        {
            string decodedText = LoadSave(file.FullName, decode);
            dynamic obj = JsonConvert.DeserializeObject(decodedText);
            int i = 0;
            foreach (var a in obj.data)
            {
                if (a.stat.f != null)
                {
                    string save = JsonConvert.SerializeObject(a.stat.f, Formatting.Indented);
                    string folderName = file.FullName.Substring(0, file.FullName.IndexOf(file.Name));
                    File.WriteAllText(Path.Combine(folderName, $"{i}.json"), save);
                }
                i++;
            }
        }

        public void InsertSave(string file, string save, int position, bool decode = true)
        {
            string decodedText = LoadSave(file, decode);
            dynamic obj = JsonConvert.DeserializeObject(decodedText);
            dynamic savePart = JsonConvert.DeserializeObject(save);
            if(obj.data != null)
            {
                obj.data[position].stat.f = savePart;
                string json = JsonConvert.SerializeObject(obj, Formatting.None);
                StoreSave(file, json);
                Console.WriteLine($"Replaced {position}");
            }
        }

        public void StoreSave(string file, string save)
        {
            string encoded = Encode(save);
            File.WriteAllText(file, encoded, TextEncoding);
        }

        public string LoadSave(string file, bool decode = true)
        {
            string text = File.ReadAllText(file, TextEncoding);
            if(decode)
            {
                text = Decode(text);
            }
            return (text);
        }

        public string Decode(string input)
        {
            string urlDecode = HttpUtility.UrlDecode(input, TextEncoding);
            return (urlDecode);
        }

        public string Encode(string input)
        {
            var encoded = EncodeNonAsciiCharacters(input).Replace("%2F", "/").Replace("%2A", "*");
            return (encoded);
        }

        protected string EncodeNonAsciiCharacters(string value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                if ((c >= 48 && c <= 57) || //0-9  
            (c >= 65 && c <= 90) || //a-z  
            (c >= 97 && c <= 122) || //A-Z                    
            (c == 45 || c == 46 || c == 95 || c == 126)) // period, hyphen, underscore, tilde  
                {
                    sb.Append(c);
                }
                else
                {
                    if (c > 127)
                    {
                        // This character is too big for ASCII
                        string encodedValue = "%u" + ((int)c).ToString("x4");
                        sb.Append(encodedValue);
                    }
                    else
                    {

                        sb.AppendFormat("%{0:X2}", ((byte)c));
                    }

                }

            }
            return sb.ToString();
        }
    }
}
