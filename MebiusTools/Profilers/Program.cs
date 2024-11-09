using System.Reflection.Emit;
using System.Text;




int mode = 0;//0-all,1-nop,2-mop 
bool showByFile = true;
int logTo = 2; //1-console, 2-file


var dir = new DirectoryInfo(@"c:\_Code\Mebius\MebiusTools\Profilers\prof_x");

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
                    Console.WriteLine("Файл:"+f.Name+" - время обработки "+packs +" пакетов "+"(документов: "+dmains+"): " +fullTime
                +", сброс в БД: "+dbTime +" ("+percent(TimeSpan.Parse(fullTime),TimeSpan.Parse(dbTime))+" % от общего времени)"
                //+", завершение транзакции: "+transTime +" ("+percent(TimeSpan.Parse(fullTime),TimeSpan.Parse(transTime))+" % от общего времени)"
                +(string.IsNullOrEmpty(accCnt) ?"":  (", ожидание блокировки "+accCnt+" счетов: "+accTime +" ("+percent(TimeSpan.Parse(fullTime),TimeSpan.Parse(accTime))+" % от общего времени)"))
                );
            }
            else
            {
                sw.WriteLine("Файл:"+f.Name+" - время обработки "+packs +" пакетов "+"(документов: "+dmains+"): " +fullTime
                +", сброс в БД: "+dbTime +" ("+percent(TimeSpan.Parse(fullTime),TimeSpan.Parse(dbTime))+" % от общего времени)"
                //+", завершение транзакции: "+transTime +" ("+percent(TimeSpan.Parse(fullTime),TimeSpan.Parse(transTime))+" % от общего времени)"
                +(string.IsNullOrEmpty(accCnt) ?"":  (", ожидание блокировки "+accCnt+" счетов: "+accTime +" ("+percent(TimeSpan.Parse(fullTime),TimeSpan.Parse(accTime))+" % от общего времени)"))
                );
            }

        }

        totalPacks+=long.Parse(packs);
        totalDmains+=long.Parse(dmains);
        totalFullTime+=TimeSpan.Parse(fullTime);
        totalDBTime+=TimeSpan.Parse(dbTime);
        totalTransTime+=TimeSpan.Parse(transTime);
        if(!string.IsNullOrEmpty(accCnt))
        {
            totalAccCnt+=long.Parse(accCnt);
            totalAccTime+=TimeSpan.Parse(accTime);

            minAccCnt = Math.Min(minAccCnt,long.Parse(accCnt));
            maxAccCnt = Math.Max(maxAccCnt,long.Parse(accCnt));
            minAccTime = minAccTime < TimeSpan.Parse(accTime) ? minAccTime : TimeSpan.Parse(accTime);
            maxAccTime = maxAccTime > TimeSpan.Parse(accTime) ? maxAccTime : TimeSpan.Parse(accTime);
        }
        minPacks = Math.Min(minPacks,long.Parse(packs));
        maxPacks = Math.Max(maxPacks,long.Parse(packs));
        minDmain = Math.Min(minDmain,long.Parse(dmains));
        maxDmain = Math.Max(maxDmain,long.Parse(dmains));
        minFullTime = minFullTime < TimeSpan.Parse(fullTime) ? minFullTime : TimeSpan.Parse(fullTime);
        maxFullTime = maxFullTime > TimeSpan.Parse(fullTime) ? maxFullTime : TimeSpan.Parse(fullTime);
        minDBTime = minDBTime < TimeSpan.Parse(dbTime) ? minDBTime : TimeSpan.Parse(dbTime);
        maxDBTime = maxDBTime > TimeSpan.Parse(dbTime) ? maxDBTime : TimeSpan.Parse(dbTime);
        minTransTime = minTransTime < TimeSpan.Parse(transTime) ? minTransTime : TimeSpan.Parse(transTime);
        maxTransTime = maxTransTime > TimeSpan.Parse(transTime) ? maxTransTime : TimeSpan.Parse(transTime);
/*
        double cntPerSecPacket = long.Parse(dmains) / ((TimeSpan.Parse(end) - TimeSpan.Parse(start)).TotalSeconds);
        Console.WriteLine("start:"+start+" end:"+end+" cntPerSecPacket:"+cntPerSecPacket);
        for(var dt = TimeSpan.Parse(start); dt < TimeSpan.Parse(end); dt = dt.Add(TimeSpan.FromSeconds(1)))
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