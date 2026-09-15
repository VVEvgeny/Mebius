using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;


#pragma warning disable



var dir = new DirectoryInfo(@".\data");

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Encoding enc = Encoding.GetEncoding(866);

var task = "29";

foreach (var f in dir.GetFiles())
{
    if (f.Name.StartsWith("prof") && f.Name.Contains(".") && f.Name.Split(".")[0].EndsWith("_" + task))
    {
        var fc = new StreamReader(f.FullName, encoding: enc).ReadToEnd();
        if (fc.Length == 0)
            continue;

        if (task == "29")
        {
            string startTime = "";
            string endTime = "";
            string formNumber = "";
            string formTime = "";


            foreach (var l in fc.Split("\n"))
            {
                //[-] |                               Время работы задачи task29 |        1| 100.00|00:00:02.078889341|00:00:02.078889341|00:00:02.078889341|00:00:02.078889341|13:17:25.618|13:17:27.697|00:00:02.079|
                parseIf(l, "[-] |", "Время работы задачи task29", 8, ref startTime);
                //[-] |                               Время работы задачи task29 |        1| 100.00|00:00:02.078889341|00:00:02.078889341|00:00:02.078889341|00:00:02.078889341|13:17:25.618|13:17:27.697|00:00:02.079|
                parseIf(l, "[-] |", "Время работы задачи task29", 9, ref endTime);

                //[-] |                                Время печати формы 401108 |        1| 100.00|00:00:04.512149094|00:00:04.512149094|00:00:04.512149094|00:00:04.512149094|18:13:27.822|18:13:32.335|00:00:04.512|
                parseIf(l, "[-] |", "Время печати формы", 1, ref formNumber);
                parseIf(l, "[-] |", "Время печати формы", 4, ref formTime);
            }

            //Console.WriteLine($"File:{f.Name} Form:{formNumber.Split(" ")[3]} Start:{formatTime(parseTime(startTime))} End:{formatTime(parseTime(endTime))} Duration:{formatTime(parseTime(formTime))}");
            Console.WriteLine($"{f.Name};{formNumber.Split(" ")[3]};{formatTime(parseTime(startTime))};{formatTime(parseTime(endTime))};{formatTime(parseTime(formTime))}");
        
        }
    }
}


void parseIf(string l, string l1, string l2, int idx, ref string ret)
{
    if (l.StartsWith(l1) && l.Split('|')[1].Trim().StartsWith(l2))
        ret = l.Split("|")[idx].Trim();
}
string formatTime(TimeSpan t)
{
    // Show total hours (not modulo 24) so long runs are clear
    var totalHours = (int)t.TotalHours;
    return $"{totalHours}:{t.Minutes:00}:{t.Seconds:00}";
}
string formatTimeShort(TimeSpan t)
{
    // Show total minutes (hours converted to minutes) to keep shorter-time displays monotonic
    var totalMinutes = (int)t.TotalMinutes;
    return $"{totalMinutes}:{t.Seconds:00}";
}

TimeSpan parseTime(string input)
{
    int dotIndex = input.IndexOf('.');
    if (dotIndex != -1 && input.Length > dotIndex + 8)
    {
        input = input.Substring(0, dotIndex + 8);
    }

    if (TimeSpan.TryParse(input, out var t))
        return t;

    return TimeSpan.Zero;
}