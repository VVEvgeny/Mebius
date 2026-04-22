using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

#pragma warning disable

const string settingsFile = "profiler-settings.txt";
var settings = LoadSettings(settingsFile);

// If user requested --file <path>, skip interactive prompts and redirect output.
var promptUser = true;
var outputFile = (string?)null;
for (var i = 0; i < args.Length; i++)
{
    if (args[i] == "--file" && i + 1 < args.Length)
    {
        outputFile = args[i + 1];
        promptUser = false;
        break;
    }
}

if (outputFile != null)
{
    var sw = new StreamWriter(outputFile) { AutoFlush = true };
    Console.SetOut(sw);
}

var (mode, modeChanged) = ReadIntOption("Mode (0=all, 1=nop, 2=mop)", settings.Mode, new[] { 0, 1, 2 }, promptUser);
var (showByFile, showByFileChanged) = ReadBoolOption("Show per-file table? (y/n)", settings.ShowByFile, promptUser);
var (showStats, showStatsChanged) = ReadBoolOption("Show Docs/min stats? (y/n)", settings.ShowStats, promptUser);
var (task, taskChanged) = ReadStringOption("Task (812/837/881/880)", settings.Task, new[] { "812", "881","837","880" }, promptUser);
var (dirPath, dirPathChanged) = ReadStringOptionFree("Directory Path", settings.DirPath, promptUser);

if (modeChanged || showByFileChanged || showStatsChanged || taskChanged || dirPathChanged)
{
    SaveSettings(settingsFile, new Settings(mode, showByFile, showStats, task, dirPath));
}

Console.WriteLine($"Selected: mode={mode}, showByFile={(showByFile ? "yes" : "no")}, showStats={(showStats ? "yes" : "no")}, task={task}, dir={dirPath}");

var dir = new DirectoryInfo(dirPath);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Encoding enc = Encoding.GetEncoding(866);

long totalPacks = 0;
long totalDmains = 0;
TimeSpan totalFullTime = TimeSpan.Zero;
TimeSpan totalMaxTime = TimeSpan.Zero;
TimeSpan totalDBTime = TimeSpan.Zero;
long totalAccCnt = 0;
TimeSpan totalAccTime = TimeSpan.Zero;
TimeSpan totalTransTime = TimeSpan.Zero;

long minPacks = long.MaxValue, maxPacks = 0;
long minDmain = long.MaxValue, maxDmain = 0;
long minAccCnt = long.MaxValue, maxAccCnt = 0;
TimeSpan minFullTime = TimeSpan.MaxValue, maxFullTime = TimeSpan.Zero;
TimeSpan minDBTime = TimeSpan.MaxValue, maxDBTime = TimeSpan.Zero;
TimeSpan minTransTime = TimeSpan.MaxValue, maxTransTime = TimeSpan.Zero;
TimeSpan minAccTime = TimeSpan.MaxValue, maxAccTime = TimeSpan.Zero;

long totalSelectCount = 0;
long totalInsertCount = 0;
long totalUpdateCount = 0;
long totalDeleteCount = 0;
long totalOtherCount = 0;
TimeSpan totalSelectTime = TimeSpan.Zero;
TimeSpan totalInsertTime = TimeSpan.Zero;
TimeSpan totalUpdateTime = TimeSpan.Zero;

long totalSysLogCount = 0;
TimeSpan totalSysLogTime = TimeSpan.Zero;

var perFileStats = new List<FileStats>();

foreach (var f in dir.GetFiles())
{
    if (f.Name.StartsWith("prof") && f.Name.Contains(".") && f.Name.Split(".")[0].EndsWith("_" + task))
    {
        var fc = new StreamReader(f.FullName, encoding: enc).ReadToEnd();
        if (fc.Length == 0)
            continue;

        if (task == "812" || task == "837")
        {
            string fullTime = "";
            string maxTime = "";
            string packs = "0";
            string dmains = "";
            string dbTime = "";

            string accCnt = "";
            string accTime = "";

            string transTime = "";

            string selectCount = "";
            string selectTime = "";
            string insertCount = "";
            string insertTime = "";
            string updateCount = "";
            string updateTime = "";
            string deleteCount = "";
            string otherCount = "";

            string sysLogCount = "";
            string sysLogTime = "";


            foreach (var l in fc.Split("\n"))
            {
                parseIf(l, "[-] |", "AppHandler::process - message_handler", 4, ref fullTime);
                parseIf(l, "[-] |", "AppHandler::process - message_handler", 5, ref maxTime);

                parseIf(l, "[-] |", "AppHandler812::GetPack", 2, ref packs);

                parseIf(l, "[-] |", "Сохранение документа Doc::__New", 2, ref dmains);

                parseIf(l, "[-] |", "DocPackageClearance::CommitClearance()", 4, ref dbTime);

                parseIf(l, "[-] |", "DocPackageClearance::LockAccounts() блокировка счетов", 2, ref accCnt);
                parseIf(l, "[-] |", "DocPackageClearance::LockAccounts() блокировка счетов", 4, ref accTime);

                parseIf(l, "[-] |", "Библиотека QUEST: Завершение транзакции", 4, ref transTime);


                parseIf(l, "[-] |", "Библиотека QUEST: Выбор из БД (выполнение SELECT FIRST)", 2, ref selectCount);
                parseIf(l, "[-] |", "Библиотека QUEST: Выбор из БД (выполнение SELECT)", 2, ref selectCount);//old profiler format
                parseIf(l, "[-] |", "Библиотека QUEST: Выбор из БД (выполнение SELECT FIRST)", 4, ref selectTime);
                parseIf(l, "[-] |", "Библиотека QUEST: Выбор из БД (выполнение SELECT)", 4, ref selectTime);//old profiler format

                parseIf(l, "[-] |", "Библиотека QUEST: Запись блока данных (выполнение FLUSH)", 2, ref insertCount);
                parseIf(l, "[-] |", "Библиотека QUEST: Запись блока данных (выполнение FLUSH)", 4, ref insertTime);

                parseIf(l, "[-] |", "Библиотека QUEST: Изменение в БД (выполнение UPDATE)", 2, ref updateCount);
                parseIf(l, "[-] |", "Библиотека QUEST: Изменение в БД (выполнение UPDATE)", 4, ref updateTime);

                parseIf(l, "[-] |", "Библиотека QUEST: Удаление из БД (выполнение DELETE)", 2, ref deleteCount);

                parseIf(l, "[-] |", "Библиотека QUEST: Другие операции", 2, ref otherCount);

                parseIf(l, "[-] |", "Запись события в syslog. JournalSys::Event", 2, ref sysLogCount);
                parseIf(l, "[-] |", "Запись события в syslog. JournalSys::Event", 4, ref sysLogTime);
            }

            if (string.IsNullOrEmpty(fullTime) || string.IsNullOrEmpty(dmains))
            {
                if (showByFile)
                {
                    Console.WriteLine("Файл:" + f.Name + " - пустой, пропускаю");
                }
                continue;
            }

            if (mode == 0) { }
            else if (mode == 1)
            {
                if (string.IsNullOrEmpty(accCnt))
                    continue; //ignore MOPS
            }
            else if (mode == 2)
            {
                if (!string.IsNullOrEmpty(accCnt))
                    continue; //ignore MOPS
            }

            // if (showByFile)
            // {
            //     Console.WriteLine("Файл:" + f.Name + " - время обработки " + packs + " пакетов " + "(документов: " + dmains + "): " + fullTime
            //     + ", сброс в БД: " + dbTime + " (" + percent(parseTime(fullTime), parseTime(dbTime)) + " % от общего времени)"
            //     + (string.IsNullOrEmpty(accCnt) ? "" : (", ожидание блокировки " + accCnt + " счетов: " + accTime + " (" + percent(parseTime(fullTime), parseTime(accTime)) + " % от общего времени)"))
            //     + (sqlControls ? (", SELECTs: " + selectCount + ", INSERTs: " + insertCount + ", UPDATEs: " + updateCount + ", DELETEs: " + deleteCount + ", OTHERs: " + otherCount) : "")
            //     );
            // }

            perFileStats.Add(new FileStats(
                f.Name,
                long.Parse(packs),
                long.Parse(dmains),
                parseTime(fullTime),
                parseTime(maxTime),
                string.IsNullOrEmpty(accCnt) ? 0 : long.Parse(accCnt),
                parseTime(accTime),
                string.IsNullOrEmpty(selectCount) ? 0 : long.Parse(selectCount),
                parseTime(selectTime),
                string.IsNullOrEmpty(insertCount) ? 0 : long.Parse(insertCount),
                parseTime(insertTime),
                string.IsNullOrEmpty(updateCount) ? 0 : long.Parse(updateCount),
                parseTime(updateTime),
                string.IsNullOrEmpty(deleteCount) ? 0 : long.Parse(deleteCount),
                string.IsNullOrEmpty(otherCount) ? 0 : long.Parse(otherCount),
                string.IsNullOrEmpty(sysLogCount) ? 0 : long.Parse(sysLogCount),
                parseTime(sysLogTime)
            ));

            totalPacks += long.Parse(packs);
            totalDmains += long.Parse(dmains);
            totalFullTime += parseTime(fullTime);
            totalDBTime += parseTime(dbTime);
            totalTransTime += parseTime(transTime);
            if (!string.IsNullOrEmpty(accCnt))
            {
                totalAccCnt += long.Parse(accCnt);
                totalAccTime += parseTime(accTime);

                minAccCnt = Math.Min(minAccCnt, long.Parse(accCnt));
                maxAccCnt = Math.Max(maxAccCnt, long.Parse(accCnt));
                minAccTime = minAccTime < parseTime(accTime) ? minAccTime : parseTime(accTime);
                maxAccTime = maxAccTime > parseTime(accTime) ? maxAccTime : parseTime(accTime);
            }
            minPacks = Math.Min(minPacks, long.Parse(packs));
            maxPacks = Math.Max(maxPacks, long.Parse(packs));
            minDmain = Math.Min(minDmain, long.Parse(dmains));
            maxDmain = Math.Max(maxDmain, long.Parse(dmains));
            minFullTime = minFullTime < parseTime(fullTime) ? minFullTime : parseTime(fullTime);
            maxFullTime = maxFullTime > parseTime(fullTime) ? maxFullTime : parseTime(fullTime);
            minDBTime = minDBTime < parseTime(dbTime) ? minDBTime : parseTime(dbTime);
            maxDBTime = maxDBTime > parseTime(dbTime) ? maxDBTime : parseTime(dbTime);
            minTransTime = minTransTime < parseTime(transTime) ? minTransTime : parseTime(transTime);
            maxTransTime = maxTransTime > parseTime(transTime) ? maxTransTime : parseTime(transTime);

            totalSelectCount += string.IsNullOrEmpty(selectCount) ? 0 : long.Parse(selectCount);
            totalInsertCount += string.IsNullOrEmpty(insertCount) ? 0 : long.Parse(insertCount);
            totalUpdateCount += string.IsNullOrEmpty(updateCount) ? 0 : long.Parse(updateCount);
            totalDeleteCount += string.IsNullOrEmpty(deleteCount) ? 0 : long.Parse(deleteCount);
            totalOtherCount += string.IsNullOrEmpty(otherCount) ? 0 : long.Parse(otherCount);

            totalSelectTime += parseTime(selectTime);
            totalInsertTime += parseTime(insertTime);
            totalUpdateTime += parseTime(updateTime);

            totalSysLogCount += string.IsNullOrEmpty(sysLogCount) ? 0 : long.Parse(sysLogCount);
            totalSysLogTime += parseTime(sysLogTime);
        }
        else if (task == "881" || task == "880")
        {
            string fullTime = "";
            string maxTime = "";
            string packs = "0";
            string dmains = "";
            string dbTime = "";

            string accCnt = "";
            string accTime = "";

            string transTime = "";

            string selectCount = "";
            string selectTime = "";
            string insertCount = "";
            string insertTime = "";
            string updateCount = "";
            string updateTime = "";
            string deleteCount = "";
            string otherCount = "";
            string sysLogCount = "";
            string sysLogTime = "";

            foreach (var l in fc.Split("\n"))
            {
                parseIf(l, "[-] |", "ActualMessageHandler", 4, ref fullTime);
                parseIf(l, "[-] |", "ActualMessageHandler", 5, ref maxTime);

                parseIf(l, "[-] |", "count_new_doc", 2, ref dmains);

                parseIf(l, "[-] |", "SELECT_ACCOUNT", 2, ref accCnt);
                parseIf(l, "[-] |", "SELECT_ACCOUNT", 4, ref accTime);

                parseIf(l, "[-] |", "Библиотека QUEST: Завершение транзакции", 4, ref transTime);


                parseIf(l, "[-] |", "Библиотека QUEST: Выбор из БД (выполнение SELECT FIRST)", 2, ref selectCount);
                parseIf(l, "[-] |", "Библиотека QUEST: Выбор из БД (выполнение SELECT)", 2, ref selectCount);//old profiler format
                parseIf(l, "[-] |", "Библиотека QUEST: Выбор из БД (выполнение SELECT FIRST)", 4, ref selectTime);
                parseIf(l, "[-] |", "Библиотека QUEST: Выбор из БД (выполнение SELECT)", 4, ref selectTime);//old profiler format

                parseIf(l, "[-] |", "Библиотека QUEST: Добавление в БД (выполнение INSERT)", 2, ref insertCount);
                parseIf(l, "[-] |", "Библиотека QUEST: Добавление в БД (выполнение INSERT)", 4, ref insertTime);

                parseIf(l, "[-] |", "Библиотека QUEST: Изменение в БД (выполнение UPDATE)", 2, ref updateCount);
                parseIf(l, "[-] |", "Библиотека QUEST: Изменение в БД (выполнение UPDATE)", 4, ref updateTime);

                parseIf(l, "[-] |", "Библиотека QUEST: Удаление из БД (выполнение DELETE)", 2, ref deleteCount);

                parseIf(l, "[-] |", "Библиотека QUEST: Другие операции", 2, ref otherCount);

                parseIf(l, "[-] |", "Запись события в syslog. JournalSys::Event", 2, ref sysLogCount);
                parseIf(l, "[-] |", "Запись события в syslog. JournalSys::Event", 4, ref sysLogTime);
            }

            if (string.IsNullOrEmpty(fullTime) || string.IsNullOrEmpty(dmains))
            {
                if (showByFile)
                {
                    Console.WriteLine("Файл:" + f.Name + " - пустой, пропускаю");
                }
                continue;
            }

            perFileStats.Add(new FileStats(
                f.Name,
                long.Parse(packs),
                long.Parse(dmains),
                parseTime(fullTime),
                parseTime(maxTime),
                string.IsNullOrEmpty(accCnt) ? 0 : long.Parse(accCnt),
                parseTime(accTime),
                string.IsNullOrEmpty(selectCount) ? 0 : long.Parse(selectCount),
                parseTime(selectTime),
                string.IsNullOrEmpty(insertCount) ? 0 : long.Parse(insertCount),
                parseTime(insertTime),
                string.IsNullOrEmpty(updateCount) ? 0 : long.Parse(updateCount),
                parseTime(updateTime),
                string.IsNullOrEmpty(deleteCount) ? 0 : long.Parse(deleteCount),
                string.IsNullOrEmpty(otherCount) ? 0 : long.Parse(otherCount),
                string.IsNullOrEmpty(sysLogCount) ? 0 : long.Parse(sysLogCount),
                parseTime(sysLogTime)
            ));

            totalPacks += long.Parse(packs);
            totalDmains += long.Parse(dmains);
            totalFullTime += parseTime(fullTime);
            totalMaxTime = parseTime(maxTime);
            totalDBTime += parseTime(dbTime);
            totalTransTime += parseTime(transTime);
            if (!string.IsNullOrEmpty(accCnt))
            {
                totalAccCnt += long.Parse(accCnt);
                totalAccTime += parseTime(accTime);

                minAccCnt = Math.Min(minAccCnt, long.Parse(accCnt));
                maxAccCnt = Math.Max(maxAccCnt, long.Parse(accCnt));
                minAccTime = minAccTime < parseTime(accTime) ? minAccTime : parseTime(accTime);
                maxAccTime = maxAccTime > parseTime(accTime) ? maxAccTime : parseTime(accTime);
            }
            minPacks = Math.Min(minPacks, long.Parse(packs));
            maxPacks = Math.Max(maxPacks, long.Parse(packs));
            minDmain = Math.Min(minDmain, long.Parse(dmains));
            maxDmain = Math.Max(maxDmain, long.Parse(dmains));
            minFullTime = minFullTime < parseTime(fullTime) ? minFullTime : parseTime(fullTime);
            maxFullTime = maxFullTime > parseTime(fullTime) ? maxFullTime : parseTime(fullTime);
            minDBTime = minDBTime < parseTime(dbTime) ? minDBTime : parseTime(dbTime);
            maxDBTime = maxDBTime > parseTime(dbTime) ? maxDBTime : parseTime(dbTime);
            minTransTime = minTransTime < parseTime(transTime) ? minTransTime : parseTime(transTime);
            maxTransTime = maxTransTime > parseTime(transTime) ? maxTransTime : parseTime(transTime);

            totalSelectCount += string.IsNullOrEmpty(selectCount) ? 0 : long.Parse(selectCount);
            totalInsertCount += string.IsNullOrEmpty(insertCount) ? 0 : long.Parse(insertCount);
            totalUpdateCount += string.IsNullOrEmpty(updateCount) ? 0 : long.Parse(updateCount);
            totalDeleteCount += string.IsNullOrEmpty(deleteCount) ? 0 : long.Parse(deleteCount);
            totalOtherCount += string.IsNullOrEmpty(otherCount) ? 0 : long.Parse(otherCount);

            totalSelectTime += parseTime(selectTime);
            totalInsertTime += parseTime(insertTime);
            totalUpdateTime += parseTime(updateTime);

            totalSysLogCount += string.IsNullOrEmpty(sysLogCount) ? 0 : long.Parse(sysLogCount);
            totalSysLogTime += parseTime(sysLogTime);
        }
    }
}

PrintPerFileTable();

int percent(TimeSpan total, TimeSpan selected)
{
    if (total.Ticks / 100 == 0)
        return 0;

    return (int)(selected.Ticks / (total.Ticks / 100));
}

void parseIf(string l, string l1, string l2, int idx, ref string ret)
{
    if (l.StartsWith(l1) && l.Split('|')[1].Trim().StartsWith(l2))
        ret = l.Split("|")[idx].Trim();
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

string formatTimeWithPercent(TimeSpan t, TimeSpan total)
{
    // Always return a fixed-width string so table columns stay aligned
    // Percent is capped at 99 and shown with 2 digits (00..99)

    if (total.Ticks == 0)
        return formatTimeShort(t).PadLeft(13);

    var p = percent(total, t);
    if (p > 99) p = 99;
    var totalMinutes = (int)t.TotalMinutes;
    return $"{totalMinutes,3}:{t.Seconds:00} ({p,2}%)";
}

void PrintPerFileTable()
{
    if (perFileStats.Count == 0)
        return;

    const string headerFmt = "{0,-38} {1,8} {2,8} {3,10} {4,10} {5,7} {6,13} {7,9} {8,13} {9,9} {10,13} {11,8} {12,13} {13,8} {14,13}";

    Console.WriteLine(headerFmt, "File", "Packs", "Docs", "FullTime", "MaxTime", "Accs", "AccTime", "SELECT", "Select time", "INSERT", "Insert time", "UPDATE", "Update time", "SysLog", "SysLog time");

    if(showByFile)
    {
        Console.WriteLine(new string('-', 185));
        foreach (var s in perFileStats.OrderByDescending(s => s.FullTime))
        {
            Console.WriteLine(headerFmt,
                s.FileName,
                s.Packs,
                s.Dmains,
                formatTime(s.FullTime),
                formatTime(s.MaxTime),
                s.AccCnt,
                formatTimeWithPercent(s.AccTime, s.FullTime),
                s.SelectCount,
                formatTimeWithPercent(s.SelectTime - s.AccTime, s.FullTime), //subtract account lock time from select time for better visibility
                s.InsertCount,
                formatTimeWithPercent(s.InsertTime, s.FullTime),
                s.UpdateCount,
                formatTimeWithPercent(s.UpdateTime, s.FullTime),
                s.SysLogCount,
                formatTimeWithPercent(s.SysLogTime, s.FullTime)
                );
        }
        Console.WriteLine(new string('-', 185));
    }

    // Summary row
    var totalPacks = perFileStats.Sum(s => s.Packs);
    var totalDmains = perFileStats.Sum(s => s.Dmains);
    var totalFullTime = new TimeSpan(perFileStats.Sum(s => s.FullTime.Ticks));
    var totalMaxTime = new TimeSpan(perFileStats.Max(s => s.MaxTime.Ticks));
    var totalAccCnt = perFileStats.Sum(s => s.AccCnt);
    var totalAccTime = new TimeSpan(perFileStats.Sum(s => s.AccTime.Ticks));
    var totalSelectCount = perFileStats.Sum(s => s.SelectCount);
    var totalSelectTime = new TimeSpan(perFileStats.Sum(s => s.SelectTime.Ticks));
    var totalInsertCount = perFileStats.Sum(s => s.InsertCount);
    var totalInsertTime = new TimeSpan(perFileStats.Sum(s => s.InsertTime.Ticks));
    var totalUpdateCount = perFileStats.Sum(s => s.UpdateCount);
    var totalUpdateTime = new TimeSpan(perFileStats.Sum(s => s.UpdateTime.Ticks));
    var totalDbPercent = percent(totalFullTime, totalDBTime);
    var totalSysLogCount = perFileStats.Sum(s => s.SysLogCount);
    var totalSysLogTime = new TimeSpan(perFileStats.Sum(s => s.SysLogTime.Ticks));

    Console.WriteLine(headerFmt,
        "TOTAL",
        totalPacks,
        totalDmains,
        formatTime(totalFullTime),
        formatTime(totalMaxTime),
        totalAccCnt,
        formatTimeWithPercent(totalAccTime, totalFullTime),
        totalSelectCount,
        formatTimeWithPercent(totalSelectTime - totalAccTime, totalFullTime), //subtract account lock time from select time for better visibility
        totalInsertCount,
        formatTimeWithPercent(totalInsertTime, totalFullTime),
        totalUpdateCount,
        formatTimeWithPercent(totalUpdateTime, totalFullTime),
        totalSysLogCount,
        formatTimeWithPercent(totalSysLogTime, totalFullTime)
        );

    long per1min = (long)(totalDmains / totalFullTime.TotalSeconds) * 60;
    if (showStats)
    {
        Console.WriteLine("STATS:"
            +"\n\tDocs per 1 min: " + per1min.ToString().PadLeft(6)
            +"\n\tDocs per 5 min: " + (per1min * 5).ToString().PadLeft(6)
        );
    }
}

(int value, bool changed) ReadIntOption(string prompt, int defaultValue, int[] allowed, bool promptUser)
{
    if (!promptUser)
        return (defaultValue, false);

    while (true)
    {
        Console.Write($"{prompt} [{defaultValue}]: ");
        var line = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(line))
            return (defaultValue, false);
        if (int.TryParse(line, out var v) && allowed.Contains(v))
            return (v, true);
        Console.WriteLine($"Invalid value. Allowed: {string.Join(", ", allowed)}");
    }
}

(bool value, bool changed) ReadBoolOption(string prompt, bool defaultValue, bool promptUser)
{
    if (!promptUser)
        return (defaultValue, false);

    while (true)
    {
        Console.Write($"{prompt} [{(defaultValue ? "Y" : "N")}]: ");
        var line = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(line))
            return (defaultValue, false);
        if (line.Equals("y", StringComparison.OrdinalIgnoreCase) || line.Equals("yes", StringComparison.OrdinalIgnoreCase))
            return (true, true);
        if (line.Equals("n", StringComparison.OrdinalIgnoreCase) || line.Equals("no", StringComparison.OrdinalIgnoreCase))
            return (false, true);
        Console.WriteLine("Invalid value. Please enter y or n.");
    }
}

(string value, bool changed) ReadStringOption(string prompt, string defaultValue, string[] allowed, bool promptUser)
{
    if (!promptUser)
        return (defaultValue, false);

    while (true)
    {
        Console.Write($"{prompt} [{defaultValue}]: ");
        var line = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(line))
            return (defaultValue, false);
        if (allowed.Contains(line))
            return (line, true);
        Console.WriteLine($"Invalid value. Allowed: {string.Join(", ", allowed)}");
    }
}
Settings LoadSettings(string path)
{
    var settings = new Settings(0, true, false, "812", @"c:\_Code\Mebius\MebiusTools\Profilers\prof_12_03_2026_efimov_split\");
    if (!File.Exists(path))
        return settings;

    foreach (var line in File.ReadAllLines(path))
    {
        var parts = line.Split('=', 2);
        if (parts.Length != 2)
            continue;
        var key = parts[0].Trim();
        var value = parts[1].Trim();

        if (key.Equals("Mode", StringComparison.OrdinalIgnoreCase) && int.TryParse(value, out var m))
            settings = settings with { Mode = m };
        else if (key.Equals("ShowByFile", StringComparison.OrdinalIgnoreCase) && bool.TryParse(value, out var b))
            settings = settings with { ShowByFile = b };
        else if (key.Equals("Task", StringComparison.OrdinalIgnoreCase))
            settings = settings with { Task = value };
        else if (key.Equals("DirPath", StringComparison.OrdinalIgnoreCase))
            settings = settings with { DirPath = value };
        else if (key.Equals("ShowStats", StringComparison.OrdinalIgnoreCase) && bool.TryParse(value, out var ss))
            settings = settings with { ShowStats = ss };
    }

    return settings;
}

void SaveSettings(string path, Settings settings)
{
    var lines = new[]
    {
        $"Mode={settings.Mode}",
        $"ShowByFile={settings.ShowByFile}",
        $"ShowStats={settings.ShowStats}",
        $"Task={settings.Task}",
        $"DirPath={settings.DirPath}",
    };

    File.WriteAllLines(path, lines);
}

(string value, bool changed) ReadStringOptionFree(string prompt, string defaultValue, bool promptUser)
{
    if (!promptUser)
        return (defaultValue, false);

    Console.Write($"{prompt} [{defaultValue}]: ");
    var line = Console.ReadLine()?.Trim();
    if (string.IsNullOrEmpty(line))
        return (defaultValue, false);
    return (line, true);
}

record Settings(int Mode, bool ShowByFile, bool ShowStats, string Task, string DirPath);

record FileStats(string FileName, long Packs, long Dmains, TimeSpan FullTime, TimeSpan MaxTime, long AccCnt, TimeSpan AccTime, long SelectCount, TimeSpan SelectTime, long InsertCount, TimeSpan InsertTime, long UpdateCount, TimeSpan UpdateTime, long DeleteCount, long OtherCount, long SysLogCount, TimeSpan SysLogTime);