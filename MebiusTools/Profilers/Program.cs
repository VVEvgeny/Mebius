using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

#pragma warning disable


int mode = 0;//0-all,1-nop,2-mop 
bool showByFile = true;
bool sqlControls = true;

var dir = new DirectoryInfo(@"c:\_Code\Mebius\MebiusTools\Profilers\prof_12_03_no_virt\");

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Encoding enc = Encoding.GetEncoding(866);

long totalPacks = 0;
long totalDmains = 0;
TimeSpan totalFullTime = TimeSpan.Zero;
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

var perFileStats = new List<FileStats>();

foreach (var f in dir.GetFiles())
{

    if (f.Name.StartsWith("prof") && f.Name.Contains(".") && f.Name.Split(".")[0].EndsWith("_812"))
    {
        var fc = new StreamReader(f.FullName, encoding: enc).ReadToEnd();
        if (fc.Length == 0)
            continue;

        string fullTime = "";
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


        foreach (var l in fc.Split("\n"))
        {
            parseIf(l, "[-] |", "AppHandler::process - message_handler", 4, ref fullTime);

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
            string.IsNullOrEmpty(accCnt) ? 0 : long.Parse(accCnt),
            parseTime(accTime),
            string.IsNullOrEmpty(selectCount) ? 0 : long.Parse(selectCount),
            parseTime(selectTime),
            string.IsNullOrEmpty(insertCount) ? 0 : long.Parse(insertCount),
            parseTime(insertTime),
            string.IsNullOrEmpty(updateCount) ? 0 : long.Parse(updateCount),
            parseTime(updateTime),
            string.IsNullOrEmpty(deleteCount) ? 0 : long.Parse(deleteCount),
            string.IsNullOrEmpty(otherCount) ? 0 : long.Parse(otherCount)
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

    }
}

// string FormReport(bool useMinMax)
// {
//     StringBuilder sb = new StringBuilder();

//     sb.Append("Общее - время обработки " + totalPacks.ToString().PadLeft(6) + " пакетов");
//     if (useMinMax) sb.Append($" (min:{minPacks},max:{maxPacks})");

//     sb.Append(" (документов: " + totalDmains.ToString().PadLeft(8));
//     if (useMinMax) sb.Append($" (min:{minDmain},max:{maxDmain}))");
//     sb.Append("): ");

//     sb.Append(totalFullTime.ToString(@"hh\:mm\:ss\.fff"));
//     sb.Append(", сброс в БД: " + totalDBTime.ToString(@"hh\:mm\:ss\.fff"));
//     sb.Append(" (" + percent(totalFullTime, totalDBTime).ToString().PadLeft(3) + " % от общего времени)");


//     if (totalAccCnt != 0)
//     {
//         sb.Append(", ожидание блокировки " + totalAccCnt.ToString().PadLeft(5) + " счетов : ");
//         sb.Append(totalAccTime.ToString(@"hh\:mm\:ss\.fff"));
//         sb.Append(" (" + percent(totalFullTime, totalAccTime).ToString().PadLeft(3) + " % от общего времени)");
//     }
//     if (sqlControls)
//         sb.Append(" SQLS: Выборка:" + totalSelectCount.ToString().PadLeft(10) + ", Вставка:" + totalInsertCount.ToString().PadLeft(10) +
//         ", Обновление:" + totalUpdateCount.ToString().PadLeft(10) + ", Удаление:" + totalDeleteCount.ToString().PadLeft(10) +
//         ", Прочее:" + totalOtherCount.ToString().PadLeft(10));

//     return sb.ToString();
// }


// Console.WriteLine(FormReport(false));

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
    if (!showByFile || perFileStats.Count == 0)
        return;

    const string headerFmt = "{0,-38} {1,8} {2,8} {3,10} {4,7} {5,13} {6,9} {7,13} {8,9} {9,13} {10,8} {11,13}";

    Console.WriteLine(headerFmt, "File", "Packs", "Docs", "FullTime", "Accs", "AccTime", "SELECT", "Select time", "INSERT", "Insert time", "UPDATE", "Update time");
    Console.WriteLine(new string('-', 160));

    foreach (var s in perFileStats.OrderByDescending(s => s.FullTime))
    {
        Console.WriteLine(headerFmt,
            s.FileName,
            s.Packs,
            s.Dmains,
            formatTime(s.FullTime),
            s.AccCnt,
            formatTimeWithPercent(s.AccTime, s.FullTime),
            s.SelectCount,
            formatTimeWithPercent(s.SelectTime - s.AccTime, s.FullTime), //subtract account lock time from select time for better visibility
            s.InsertCount,
            formatTimeWithPercent(s.InsertTime, s.FullTime),
            s.UpdateCount,
            formatTimeWithPercent(s.UpdateTime, s.FullTime));
    }

    // Summary row
    Console.WriteLine(new string('-', 160));
    var totalPacks = perFileStats.Sum(s => s.Packs);
    var totalDmains = perFileStats.Sum(s => s.Dmains);
    var totalFullTime = new TimeSpan(perFileStats.Sum(s => s.FullTime.Ticks));
    var totalAccCnt = perFileStats.Sum(s => s.AccCnt);
    var totalAccTime = new TimeSpan(perFileStats.Sum(s => s.AccTime.Ticks));
    var totalSelectCount = perFileStats.Sum(s => s.SelectCount);
    var totalSelectTime = new TimeSpan(perFileStats.Sum(s => s.SelectTime.Ticks));
    var totalInsertCount = perFileStats.Sum(s => s.InsertCount);
    var totalInsertTime = new TimeSpan(perFileStats.Sum(s => s.InsertTime.Ticks));
    var totalUpdateCount = perFileStats.Sum(s => s.UpdateCount);
    var totalUpdateTime = new TimeSpan(perFileStats.Sum(s => s.UpdateTime.Ticks));
    var totalDbPercent = percent(totalFullTime, totalDBTime);

    Console.WriteLine(headerFmt,
        "TOTAL",
        totalPacks,
        totalDmains,
        formatTime(totalFullTime),
        totalAccCnt,
        formatTimeWithPercent(totalAccTime, totalFullTime),
        totalSelectCount,
        formatTimeWithPercent(totalSelectTime - totalAccTime, totalFullTime), //subtract account lock time from select time for better visibility
        totalInsertCount,
        formatTimeWithPercent(totalInsertTime, totalFullTime),
        totalUpdateCount,
        formatTimeWithPercent(totalUpdateTime, totalFullTime));
}

record FileStats(string FileName, long Packs, long Dmains, TimeSpan FullTime, long AccCnt, TimeSpan AccTime, long SelectCount, TimeSpan SelectTime, long InsertCount, TimeSpan InsertTime, long UpdateCount, TimeSpan UpdateTime, long DeleteCount, long OtherCount);
