using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace _315Downgrade
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string logText = "================================" + Environment.NewLine;
            logText += "Time "+DateTime.Now + Environment.NewLine;
            void logNewLine(string s) 
            {
                logText += s + Environment.NewLine;
                Console.WriteLine(s);
            }
            
            int numOfChanges = 0;
            try
            {
                string[] allfiles = Directory.GetDirectories(Directory.GetCurrentDirectory());
                foreach (string folder in allfiles)
                {
                    logNewLine("Folder: " + folder);
                    
                    string[] AllFiles = Directory.GetFiles(folder, "*.info", SearchOption.AllDirectories);
                    foreach (string filename in AllFiles)
                    {

                        logNewLine("Check file: " + filename);
                        string str = string.Empty;
                        using (System.IO.StreamReader reader = System.IO.File.OpenText(filename))
                        {
                            str = reader.ReadToEnd();
                        }
                        if (str.Contains("<Simple name=\"CreatedByVersion\" value=\"2020.12.22.315\" />"))
                        {
                            logNewLine("File good");
                        }
                        else
                        {
                            Regex regex = new Regex(@"<Simple name=""CreatedByVersion"" value=""(\S*)"" />", RegexOptions.IgnoreCase);

                            string cleanString = regex.Replace(str, "<Simple name=\"CreatedByVersion\" value=\"2020.12.22.315\" />");

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
                            {
                                file.Write(cleanString);
                            }
                            logNewLine("Changed file!!!");
                            numOfChanges++;
                        }
                    }
                }
            }
            catch 
            {
                logNewLine("Error");
            }

            logNewLine("==================");
            logNewLine("Files changed: " + numOfChanges);
            logNewLine("==================");

            string path = Directory.GetCurrentDirectory()+"\\LogDowngrade.txt";

            if (!File.Exists(path))
            {
                string createText = "File create: "+DateTime.Now + Environment.NewLine;
                File.WriteAllText(path, createText);
            }

            File.AppendAllText(path, logText);
            Console.ReadKey(true);
        }
    }
}
