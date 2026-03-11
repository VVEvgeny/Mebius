using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

#pragma warning disable


int mode = 0;//0-all,1-nop,2-mop 
bool showByFile = true;
int logTo = 1; //1-console, 2-file
bool sqlControls = true;

var dir = new DirectoryInfo(@"c:\_Code\Mebius\MebiusTools\Profilers\prof_efimov_06_03_2026\");

StreamWriter sw = null;
if(logTo == 2)
{
    sw = new StreamWriter(Path.Combine(dir.FullName,"prof_log_"+(mode==0?"all":mode==1?"nop":"mop")+".txt"));
}

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Encoding enc = Encoding.GetEncoding(866); 



//Console.WriteLine("files:"+dir.GetFiles().Length);

long totalPacks = 0;
long totalDmains = 0;
TimeSpan totalFullTime = TimeSpan.Zero;
TimeSpan totalDBTime = TimeSpan.Zero;
long totalAccCnt = 0;
TimeSpan totalAccTime = TimeSpan.Zero;
TimeSpan totalTransTime = TimeSpan.Zero;

long minPacks = long.MaxValue, maxPacks = 0;
long minDmain = long.MaxValue, maxDmain  = 0;
long minAccCnt = long.MaxValue, maxAccCnt  = 0;
TimeSpan minFullTime = TimeSpan.MaxValue, maxFullTime = TimeSpan.Zero;
TimeSpan minDBTime = TimeSpan.MaxValue, maxDBTime = TimeSpan.Zero;
TimeSpan minTransTime = TimeSpan.MaxValue, maxTransTime = TimeSpan.Zero;
TimeSpan minAccTime = TimeSpan.MaxValue, maxAccTime = TimeSpan.Zero;

long totalSelectCount = 0;
long totalInsertCount = 0;
long totalUpdateCount = 0;
long totalDeleteCount = 0;
long totalOtherCount = 0;


var perFileStats = new List<FileStats>();

//start time
//end time
//count docs
//count / end - start = cnt per second => dictionary go from start to end and add/save this value...
//var cntPerSec = new Dictionary<TimeSpan, double>();

foreach(var f in dir.GetFiles())
{

    if(f.Name.StartsWith("prof") && f.Name.Contains(".") && f.Name.Split(".")[0].EndsWith("_812"))
    {
        //Console.WriteLine("File:"+f.Name);
        var fc = new StreamReader(f.FullName, encoding: enc).ReadToEnd();
        if(fc.Length == 0)
            continue;

        //Console.WriteLine("lines:"+fc.Split("\n").Length);

        string fullTime = "";
        string packs = "0";
        string dmains = "";
        string dbTime = "";

        string accCnt = "";
        string accTime = ""; 

        string transTime = "";

        string selectCount = "";
        string insertCount = "";
        string updateCount = "";
        string deleteCount = "";
        string otherCount = "";

        //string start = "", end = "";

        foreach(var l in fc.Split("\n"))
        {
            parseIf(l, "[-] |","AppHandler::process - message_handler", 4, ref fullTime);
            //parseIf(l, "[-] |","AppHandler::process - message_handler", 8, ref start);
            //parseIf(l, "[-] |","AppHandler::process - message_handler", 9, ref end);

            parseIf(l, "[-] |","AppHandler812::GetPack", 2, ref packs);

            parseIf(l, "[-] |","Сохранение документа Doc::__New", 2, ref dmains);

            parseIf(l, "[-] |","DocPackageClearance::CommitClearance()", 4, ref dbTime);

            parseIf(l, "[-] |","DocPackageClearance::LockAccounts() блокировка счетов", 2, ref accCnt);
            parseIf(l, "[-] |","DocPackageClearance::LockAccounts() блокировка счетов", 4, ref accTime);

            parseIf(l, "[-] |","Библиотека QUEST: Завершение транзакции", 4, ref transTime);


            parseIf(l, "[-] |","Библиотека QUEST: Выбор из БД (выполнение SELECT FIRST)", 2, ref selectCount);
            parseIf(l, "[-] |","Библиотека QUEST: Выбор из БД (выполнение SELECT)", 2, ref selectCount);//old profiler format
            parseIf(l, "[-] |","Библиотека QUEST: Запись блока данных (выполнение FLUSH)", 2, ref insertCount);
            parseIf(l, "[-] |","Библиотека QUEST: Изменение в БД (выполнение UPDATE)", 2, ref updateCount);
            parseIf(l, "[-] |","Библиотека QUEST: Удаление из БД (выполнение DELETE)", 2, ref deleteCount);

            parseIf(l, "[-] |","Библиотека QUEST: Другие операции", 2, ref otherCount);

        }
        
        /*
        Console.WriteLine("fullTime:"+fullTime);
        Console.WriteLine("packs:"+packs);
        Console.WriteLine("dmains:"+dmains);
        Console.WriteLine("dbTime:"+dbTime);
        Console.WriteLine("accCnt:"+accCnt);
        Console.WriteLine("accTime:"+accTime);
        */
        
                   
        if(string.IsNullOrEmpty(fullTime) || string.IsNullOrEmpty(dmains))
        {
            if(showByFile)
            {
                if(logTo == 1)
                    Console.WriteLine("Файл:"+f.Name+" - пустой, пропускаю");
                else sw.WriteLine("Файл:"+f.Name+" - пустой, пропускаю");
            }
            continue;
        }

        if(mode == 0){}
        else if(mode == 1)
        {
            if(string.IsNullOrEmpty(accCnt))
              continue; //ignore MOPS
        }
        else if(mode == 2)
        {
            if(!string.IsNullOrEmpty(accCnt))
              continue; //ignore MOPS
        }

        if(showByFile)
        {
            if(logTo == 1)
            {
                    Console.WriteLine("Файл:"+f.Name+" - время обработки "+packs +" пакетов "+"(документов: "+dmains+"): " +formatTime(parseTime(fullTime))
                +", сброс в БД: "+ formatTime(parseTime(dbTime)) +" ("+percent(parseTime(fullTime),parseTime(dbTime))+" % от общего времени)"
                //+", завершение транзакции: "+transTime +" ("+percent(parseTime(fullTime),parseTime(transTime))+" % от общего времени)"
                +(string.IsNullOrEmpty(accCnt) ?"":  (", ожидание блокировки "+accCnt+" счетов: "+formatTime(parseTime(accTime)) +" ("+percent(parseTime(fullTime),parseTime(accTime))+" % от общего времени)"))
                +(sqlControls ? (", SELECTs: "+selectCount+", INSERTs: "+insertCount+", UPDATEs: "+updateCount+", DELETEs: "+deleteCount+", OTHERs: "+otherCount) : "")
                );
            }
            else
            {
                sw.WriteLine("Файл:"+f.Name+" - время обработки "+packs +" пакетов "+"(документов: "+dmains+"): " +fullTime
                +", сброс в БД: "+dbTime +" ("+percent(parseTime(fullTime),parseTime(dbTime))+" % от общего времени)"
                //+", завершение транзакции: "+transTime +" ("+percent(parseTime(fullTime),parseTime(transTime))+" % от общего времени)"
                +(string.IsNullOrEmpty(accCnt) ?"":  (", ожидание блокировки "+accCnt+" счетов: "+accTime +" ("+percent(parseTime(fullTime),parseTime(accTime))+" % от общего времени)"))
                +(sqlControls ? (", SELECTs: "+selectCount+", INSERTs: "+insertCount+", UPDATEs: "+updateCount+", DELETEs: "+deleteCount+", OTHERs: "+otherCount) : "")
                );
            }

        }

        perFileStats.Add(new FileStats(
            f.Name,
            long.Parse(packs),
            long.Parse(dmains),
            parseTime(fullTime),
            parseTime(dbTime),
            string.IsNullOrEmpty(accCnt) ? 0 : long.Parse(accCnt),
            parseTime(accTime),
            string.IsNullOrEmpty(selectCount) ? 0 : long.Parse(selectCount),
            string.IsNullOrEmpty(insertCount) ? 0 : long.Parse(insertCount),
            string.IsNullOrEmpty(updateCount) ? 0 : long.Parse(updateCount),
            string.IsNullOrEmpty(deleteCount) ? 0 : long.Parse(deleteCount),
            string.IsNullOrEmpty(otherCount) ? 0 : long.Parse(otherCount)
        ));

        totalPacks+=long.Parse(packs);
        totalDmains+=long.Parse(dmains);
        totalFullTime+=parseTime(fullTime);
        totalDBTime+=parseTime(dbTime);
        totalTransTime+=parseTime(transTime);
        if(!string.IsNullOrEmpty(accCnt))
        {
            totalAccCnt+=long.Parse(accCnt);
            totalAccTime+=parseTime(accTime);

            minAccCnt = Math.Min(minAccCnt,long.Parse(accCnt));
            maxAccCnt = Math.Max(maxAccCnt,long.Parse(accCnt));
            minAccTime = minAccTime < parseTime(accTime) ? minAccTime : parseTime(accTime);
            maxAccTime = maxAccTime > parseTime(accTime) ? maxAccTime : parseTime(accTime);
        }
        minPacks = Math.Min(minPacks,long.Parse(packs));
        maxPacks = Math.Max(maxPacks,long.Parse(packs));
        minDmain = Math.Min(minDmain,long.Parse(dmains));
        maxDmain = Math.Max(maxDmain,long.Parse(dmains));
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
/*
        double cntPerSecPacket = long.Parse(dmains) / ((parseTime(end) - parseTime(start)).TotalSeconds);
        Console.WriteLine("start:"+start+" end:"+end+" cntPerSecPacket:"+cntPerSecPacket);
        for(var dt = parseTime(start); dt < parseTime(end); dt = dt.Add(TimeSpan.FromSeconds(1)))
        {
            if(!cntPerSec.ContainsKey(dt))
                cntPerSec.Add(dt,0);
                
            cntPerSec[dt] += cntPerSecPacket;
        }
        */
    }
}


/*
long minPacks = long.MaxValue, maxPacks = 0;
long minDmain = long.MaxValue, maxDmain  = 0;
TimeSpan minFullTime = TimeSpan.MinValue, maxFullTime = TimeSpan.Zero;
TimeSpan minDBTime = TimeSpan.MinValue, maxDBTime = TimeSpan.Zero;
TimeSpan minTransTime = TimeSpan.MinValue, maxTransTime = TimeSpan.Zero;
*/

        string FormReport(bool useMinMax)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Общее - время обработки "+totalPacks.ToString().PadLeft(6)+" пакетов");
            if(useMinMax) sb.Append($" (min:{minPacks},max:{maxPacks})");

            sb.Append(" (документов: "+totalDmains.ToString().PadLeft(8));
            if(useMinMax) sb.Append($" (min:{minDmain},max:{maxDmain}))");
            sb.Append("): ");

            sb.Append(totalFullTime.ToString(@"hh\:mm\:ss\.fff"));
            sb.Append(", сброс в БД: "+totalDBTime.ToString(@"hh\:mm\:ss\.fff"));
            sb.Append(" ("+percent(totalFullTime,totalDBTime).ToString().PadLeft(3)+" % от общего времени)");
            
            //sb.Append(", завершение транзакции: "+totalTransTime.ToString(@"hh\:mm\:ss\.fff"));
            //sb.Append(" ("+percent(totalFullTime,totalTransTime)+" % от общего времени)");
        
            if(totalAccCnt != 0)
            {
                sb.Append(", ожидание блокировки "+totalAccCnt.ToString().PadLeft(5)+" счетов "
                //+$"(min:{minAccCnt},max:{maxAccCnt})" 
                +": ");

                sb.Append(totalAccTime.ToString(@"hh\:mm\:ss\.fff") 
                //+$" (min:{minAccTime.ToString(@"hh\:mm\:ss\.fff")},max:{maxAccTime.ToString(@"hh\:mm\:ss\.fff")})"
                );
                sb.Append(" ("+percent(totalFullTime,totalAccTime).ToString().PadLeft(3)+" % от общего времени)");
            }
            if(sqlControls)
                sb.Append(" SQLS: Выборка:"+totalSelectCount.ToString().PadLeft(10)+", Вставка:"+totalInsertCount.ToString().PadLeft(10)+
                ", Обновление:"+totalUpdateCount.ToString().PadLeft(10)+", Удаление:"+totalDeleteCount.ToString().PadLeft(10)+
                ", Прочее:"+totalOtherCount.ToString().PadLeft(10));


/*

        +totalFullTime.ToString(@"hh\:mm\:ss\.fff")+$" (min:{minFullTime.ToString(@"hh\:mm\:ss\.fff")},max:{maxFullTime.ToString(@"hh\:mm\:ss\.fff")})"
        +", сброс в БД: "+totalDBTime.ToString(@"hh\:mm\:ss\.fff")+$" (min:{minDBTime.ToString(@"hh\:mm\:ss\.fff")},max:{maxDBTime.ToString(@"hh\:mm\:ss\.fff")})"
        +" ("+percent(totalFullTime,totalDBTime)+" % от общего времени)"
        +", завершение транзакции: "+totalTransTime.ToString(@"hh\:mm\:ss\.fff")+$" (min:{minTransTime.ToString(@"hh\:mm\:ss\.fff")},max:{maxTransTime.ToString(@"hh\:mm\:ss\.fff")})"
        +" ("+percent(totalFullTime,totalTransTime)+" % от общего времени)"
        +(totalAccCnt == 0 ?"":  (", ожидание блокировки "+totalAccCnt+" счетов "+$"(min:{minAccCnt},max:{maxAccCnt})" +": "
        +totalAccTime.ToString(@"hh\:mm\:ss\.fff") +$" (min:{minAccTime.ToString(@"hh\:mm\:ss\.fff")},max:{maxAccTime.ToString(@"hh\:mm\:ss\.fff")})"
        +" ("+percent(totalFullTime,totalAccTime)+" % от общего времени)"))
        );
        */

            return sb.ToString();
        }

    
        Console.WriteLine(FormReport(false));

        PrintPerFileTable();

        if(logTo == 2)
        {
            sw.WriteLine(FormReport(false));
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
/*
        Console.WriteLine("Times:"+cntPerSec.Count);
        foreach(var d in cntPerSec)
        {
            Console.WriteLine(d.Key.ToString(@"hh\:mm\:ss") + " - "+(long)d.Value);
        }
*/




int percent(TimeSpan total, TimeSpan selected)
{
    if(total.Ticks/100 == 0) 
        return 0;

    return (int)(selected.Ticks / (total.Ticks/100));
}

void parseIf(string l, string l1, string l2, int idx, ref string ret)
{
    if(l.StartsWith(l1) && l.Split('|')[1].Trim().StartsWith(l2))
        ret = l.Split("|")[idx].Trim();
}

TimeSpan parseTime(string input)
{
    int dotIndex = input.IndexOf('.');
    if (dotIndex != -1 && input.Length > dotIndex + 8)
    {
        input = input.Substring(0, dotIndex + 8);
    }

    if(TimeSpan.TryParse(input, out var t))
        return t;

    return TimeSpan.Zero;
}

string formatTime(TimeSpan t)
{
    return t.ToString(@"hh\:mm\:ss\.fff");
}

void PrintPerFileTable()
{
    if (!showByFile || perFileStats.Count == 0)
        return;

    const string headerFmt = "{0,-30} {1,6} {2,10} {3,15} {4,15} {5,5} {6,8} {7,10} {8,10} {9,10} {10,6} {11,6}";
    const string rowFmt = "{0,-30} {1,6} {2,10} {3,15} {4,15} {5,5} {6,8} {7,10} {8,10} {9,10} {10,6} {11,6}";

    Console.WriteLine();
    Console.WriteLine("Per-file summary:");
    Console.WriteLine(headerFmt, "File", "Packs", "Docs", "FullTime", "DBTime", "DB%", "AccCnt", "SELECT", "INSERT", "UPDATE", "DELETE", "OTHER");
    Console.WriteLine(new string('-', 150));

    foreach (var s in perFileStats.OrderByDescending(s => s.FullTime))
    {
        var dbPercent = percent(s.FullTime, s.DBTime);
        Console.WriteLine(rowFmt,
            s.FileName,
            s.Packs,
            s.Dmains,
            formatTime(s.FullTime),
            formatTime(s.DBTime),
            dbPercent,
            s.AccCnt,
            s.SelectCount,
            s.InsertCount,
            s.UpdateCount,
            s.DeleteCount,
            s.OtherCount);
    }

    // Summary row
    Console.WriteLine(new string('-', 150));
    var totalPacks = perFileStats.Sum(s => s.Packs);
    var totalDmains = perFileStats.Sum(s => s.Dmains);
    var totalFullTime = new TimeSpan(perFileStats.Sum(s => s.FullTime.Ticks));
    var totalDBTime = new TimeSpan(perFileStats.Sum(s => s.DBTime.Ticks));
    var totalAccCnt = perFileStats.Sum(s => s.AccCnt);
    var totalSelectCount = perFileStats.Sum(s => s.SelectCount);
    var totalInsertCount = perFileStats.Sum(s => s.InsertCount);
    var totalUpdateCount = perFileStats.Sum(s => s.UpdateCount);
    var totalDeleteCount = perFileStats.Sum(s => s.DeleteCount);
    var totalOtherCount = perFileStats.Sum(s => s.OtherCount);
    var totalDbPercent = percent(totalFullTime, totalDBTime);

    Console.WriteLine(rowFmt,
        "TOTAL",
        totalPacks,
        totalDmains,
        formatTime(totalFullTime),
        formatTime(totalDBTime),
        totalDbPercent,
        totalAccCnt,
        totalSelectCount,
        totalInsertCount,
        totalUpdateCount,
        totalDeleteCount,
        totalOtherCount);
}

record FileStats(string FileName, long Packs, long Dmains, TimeSpan FullTime, TimeSpan DBTime, long AccCnt, TimeSpan AccTime, long SelectCount, long InsertCount, long UpdateCount, long DeleteCount, long OtherCount);
