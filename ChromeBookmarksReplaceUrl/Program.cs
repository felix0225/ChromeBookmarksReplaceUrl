using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ChromeBookmarksReplaceUrl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var filePath = ConfigurationManager.AppSettings["BookmarksPath"];
            var filebakPath = ConfigurationManager.AppSettings["BookmarksPath"] + DateTime.Today.ToString("yyyyMMdd") + ".bak";

            var sw = new Stopwatch();
            sw.Start();

            //先將檔案備份
            var f = new FileInfo(filebakPath);
            if (f.Exists)
                f.Delete();
            File.Copy(filePath, filebakPath);

            var lines = new List<string>(File.ReadAllLines(filePath));

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];

                //修改點部落的網址
                if (line.Contains("dotblogs.com.tw"))
                {
                    Console.WriteLine($"{i}");
                    Console.WriteLine($"原本網址：{line}");

                    line = line.Replace("http://", "https://");
                    line = line.Replace("www.dotblogs.com.tw", "dotblogs.com.tw");
                    line = line.Replace("archive/", "");
                    line = line.Replace(".aspx", "");

                    Console.WriteLine($"新的網址：{line}");
                    lines[i] = line;
                }

                //將網址的 http 改成 https
                if (line.Contains("blog.miniasp.com") && line.Contains("http://"))
                {
                    Console.WriteLine($"{i}");
                    Console.WriteLine($"原本網址：{line}");

                    line = line.Replace("http://", "https://");

                    Console.WriteLine($"新的網址：{line}");
                    lines[i] = line;
                }
            }

            //將修改後的資料回寫
            File.WriteAllLines(filePath, lines.ToArray());

            sw.Stop();
            Console.WriteLine($"總共花費{sw.Elapsed.Hours}:{sw.Elapsed.Minutes}:{sw.Elapsed.Seconds}");

            Console.WriteLine("Enter any key!");
            Console.ReadKey();
        }
    }
}
