using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

const string defaultLogName = "mytop_log.log";
var logPath = args.Length > 0 ? args[0] : FindInputFile(defaultLogName);
var outputPath = args.Length > 1 ? args[1] : Path.Combine(Environment.CurrentDirectory, "top_dashboard.html");

if (logPath is null || !File.Exists(logPath))
{
    Console.Error.WriteLine($"Log file not found. Pass its path as the first argument (default: {defaultLogName}).");
    return 1;
}

var report = TopLogParser.Parse(logPath);
if (report.Snapshots.Count == 0)
{
    Console.Error.WriteLine("No valid top snapshots were found in the log.");
    return 1;
}

File.WriteAllText(outputPath, Dashboard.Build(report), new UTF8Encoding(false));
Console.WriteLine($"Parsed {report.Snapshots.Count:N0} snapshots and {report.Processes.Count:N0} processes.");
Console.WriteLine($"Dashboard: {Path.GetFullPath(outputPath)}");
return 0;

static string? FindInputFile(string fileName)
{
    var candidates = new[]
    {
        Path.Combine(Environment.CurrentDirectory, fileName),
        Path.Combine(AppContext.BaseDirectory, fileName),
        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", fileName)
    };
    return candidates.FirstOrDefault(File.Exists);
}

sealed record TopReport(List<TopSnapshot> Snapshots, List<ProcessSummary> Processes);

sealed class TopSnapshot
{
    public int Number { get; init; }
    public string Time { get; init; } = "";
    public double Load1 { get; set; }
    public double Load5 { get; set; }
    public double Load15 { get; set; }
    public int TotalTasks { get; set; }
    public int RunningTasks { get; set; }
    public int SleepingTasks { get; set; }
    public int StoppedTasks { get; set; }
    public int ZombieTasks { get; set; }
    public double CpuUser { get; set; }
    public double CpuSystem { get; set; }
    public double CpuNice { get; set; }
    public double CpuIdle { get; set; }
    public double CpuWait { get; set; }
    public double CpuHardware { get; set; }
    public double CpuSoftware { get; set; }
    public double CpuSteal { get; set; }
    public double MemoryTotal { get; set; }
    public double MemoryFree { get; set; }
    public double MemoryUsed { get; set; }
    public double MemoryCache { get; set; }
    public double MemoryAvailable { get; set; }
    public double SwapTotal { get; set; }
    public double SwapFree { get; set; }
    public double SwapUsed { get; set; }
}

sealed record ProcessSummary(string Command, int Samples, double AverageCpu, double PeakCpu, double AverageMemory, double PeakMemory);

static class TopLogParser
{
    private static readonly Regex TopRegex = new(@"^top\s+-\s+(?<time>\d\d:\d\d:\d\d).*load average:\s*(?<l1>[\d.,]+),\s*(?<l5>[\d.,]+),\s*(?<l15>[\d.,]+)", RegexOptions.Compiled);
    private static readonly Regex TasksRegex = new(@"Tasks:\s*(?<total>\d+)\s+total,\s*(?<running>\d+)\s+running,\s*(?<sleeping>\d+)\s+sleeping,\s*(?<stopped>\d+)\s+stopped,\s*(?<zombie>\d+)\s+zombie", RegexOptions.Compiled);
    private static readonly Regex CpuRegex = new(@"%Cpu\(s\):\s*(?<us>[\d.,]+)\s*us,\s*(?<sy>[\d.,]+)\s*sy,\s*(?<ni>[\d.,]+)\s*ni,\s*(?<id>[\d.,]+)\s*id,\s*(?<wa>[\d.,]+)\s*wa,\s*(?<hi>[\d.,]+)\s*hi,\s*(?<si>[\d.,]+)\s*si,\s*(?<st>[\d.,]+)\s*st", RegexOptions.Compiled);
    private static readonly Regex MemoryRegex = new(@"MiB Mem\s*:\s*(?<total>[\d.,]+)\s*total,\s*(?<free>[\d.,]+)\s*free,\s*(?<used>[\d.,]+)\s*used,\s*(?<cache>[\d.,]+)\s*buff/cache", RegexOptions.Compiled);
    private static readonly Regex AvailableRegex = new(@"(?<available>[\d.,]+)\s*avail Mem", RegexOptions.Compiled);
    private static readonly Regex SwapRegex = new(@"MiB Swap:\s*(?<total>[\d.,]+)\s*total,\s*(?<free>[\d.,]+)\s*free,\s*(?<used>[\d.,]+)\s*used", RegexOptions.Compiled);
    private static readonly Regex ProcessRegex = new(@"^\s*(?<pid>\d+)\s+\S+\s+\S+\s+\S+\s+\S+\s+\S+\s+\S+\s+\S+\s+(?<cpu>[\d.,]+)\s+(?<mem>[\d.,]+)\s+\S+\s+(?<command>.+?)\s*$", RegexOptions.Compiled);

    public static TopReport Parse(string path)
    {
        var snapshots = new List<TopSnapshot>();
        var processes = new Dictionary<string, ProcessAccumulator>(StringComparer.Ordinal);
        TopSnapshot? current = null;

        foreach (var line in File.ReadLines(path))
        {
            var top = TopRegex.Match(line);
            if (top.Success)
            {
                if (current is not null)
                    snapshots.Add(current);
                current = new TopSnapshot
                {
                    Number = snapshots.Count + 1,
                    Time = top.Groups["time"].Value,
                    Load1 = Number(top, "l1"),
                    Load5 = Number(top, "l5"),
                    Load15 = Number(top, "l15")
                };
                continue;
            }

            if (current is null)
                continue;

            var tasks = TasksRegex.Match(line);
            if (tasks.Success)
            {
                current.TotalTasks = Int(tasks, "total");
                current.RunningTasks = Int(tasks, "running");
                current.SleepingTasks = Int(tasks, "sleeping");
                current.StoppedTasks = Int(tasks, "stopped");
                current.ZombieTasks = Int(tasks, "zombie");
                continue;
            }

            var cpu = CpuRegex.Match(line);
            if (cpu.Success)
            {
                current.CpuUser = Number(cpu, "us");
                current.CpuSystem = Number(cpu, "sy");
                current.CpuNice = Number(cpu, "ni");
                current.CpuIdle = Number(cpu, "id");
                current.CpuWait = Number(cpu, "wa");
                current.CpuHardware = Number(cpu, "hi");
                current.CpuSoftware = Number(cpu, "si");
                current.CpuSteal = Number(cpu, "st");
                continue;
            }

            var memory = MemoryRegex.Match(line);
            if (memory.Success)
            {
                current.MemoryTotal = Number(memory, "total");
                current.MemoryFree = Number(memory, "free");
                current.MemoryUsed = Number(memory, "used");
                current.MemoryCache = Number(memory, "cache");
                continue;
            }

            var available = AvailableRegex.Match(line);
            if (available.Success)
            {
                current.MemoryAvailable = Number(available, "available");
                continue;
            }

            var swap = SwapRegex.Match(line);
            if (swap.Success)
            {
                current.SwapTotal = Number(swap, "total");
                current.SwapFree = Number(swap, "free");
                current.SwapUsed = Number(swap, "used");
                continue;
            }

            var process = ProcessRegex.Match(line);
            if (process.Success)
            {
                var command = process.Groups["command"].Value.Trim();
                if (!processes.TryGetValue(command, out var accumulator))
                    processes[command] = accumulator = new ProcessAccumulator();
                accumulator.Add(Number(process, "cpu"), Number(process, "mem"));
            }
        }

        if (current is not null)
            snapshots.Add(current);

        var summaries = processes
            .Select(pair => pair.Value.ToSummary(pair.Key))
            .OrderByDescending(process => process.PeakCpu)
            .ThenByDescending(process => process.AverageCpu)
            .Take(100)
            .ToList();
        return new TopReport(snapshots, summaries);
    }

    private static double Number(Match match, string group) =>
        double.Parse(match.Groups[group].Value.Replace(',', '.'), CultureInfo.InvariantCulture);

    private static int Int(Match match, string group) =>
        int.Parse(match.Groups[group].Value, CultureInfo.InvariantCulture);

    private sealed class ProcessAccumulator
    {
        private double cpu;
        private double peakCpu;
        private double memory;
        private double peakMemory;
        private int samples;

        public void Add(double processCpu, double processMemory)
        {
            samples++;
            cpu += processCpu;
            memory += processMemory;
            peakCpu = Math.Max(peakCpu, processCpu);
            peakMemory = Math.Max(peakMemory, processMemory);
        }

        public ProcessSummary ToSummary(string command) =>
            new(command, samples, cpu / samples, peakCpu, memory / samples, peakMemory);
    }
}

static class Dashboard
{
    public static string Build(TopReport report)
    {
        var json = JsonSerializer.Serialize(report, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var html = """
<!doctype html>
<html lang="en">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>Linux top resource dashboard</title>
<style>
body{font:14px system-ui,sans-serif;margin:0;background:#f4f6f8;color:#17202a}main{max-width:1500px;margin:auto;padding:24px}
h1{margin:0 0 6px}.muted{color:#667085}.grid{display:grid;grid-template-columns:repeat(auto-fit,minmax(460px,1fr));gap:16px;margin-top:18px}
section{background:white;border:1px solid #dce1e7;border-radius:10px;padding:14px;box-shadow:0 2px 8px #0000000d}h2{font-size:16px;margin:0 0 4px}
canvas{width:100%;height:260px;display:block}.wide{grid-column:1/-1}.controls{margin:14px 0}.controls label{margin-right:12px}
select{padding:5px;border:1px solid #ccd3dc;border-radius:5px}table{width:100%;border-collapse:collapse;font-size:12px}th,td{text-align:right;padding:5px;border-bottom:1px solid #edf0f2}th:first-child,td:first-child{text-align:left;max-width:420px;overflow:hidden;text-overflow:ellipsis;white-space:nowrap}
</style>
</head>
<body><main><h1>Linux top resource dashboard</h1><div class="muted" id="summary"></div>
<div class="controls">Show last <select id="range"><option value="0">all snapshots</option><option value="100">100</option><option value="300">300</option><option value="1000">1000</option></select></div>
<div class="grid">
<section><h2>CPU breakdown (%)</h2><canvas id="cpu"></canvas></section>
<section><h2>Memory (MiB)</h2><canvas id="memory"></canvas></section>
<section><h2>Swap (MiB)</h2><canvas id="swap"></canvas></section>
<section><h2>Load average</h2><canvas id="load"></canvas></section>
<section><h2>Tasks</h2><canvas id="tasks"></canvas></section>
<section class="wide"><h2>Processes (aggregated from all snapshots)</h2><table><thead><tr><th>Command</th><th>Samples</th><th>Avg CPU %</th><th>Peak CPU %</th><th>Avg MEM %</th><th>Peak MEM %</th></tr></thead><tbody id="processes"></tbody></table></section>
</div></main>
<script>
const report = __DATA__;
const colors=['#2563eb','#dc2626','#16a34a','#d97706','#9333ea','#0891b2','#db2777','#475569'];
function draw(id,title,series){
 const c=document.getElementById(id),x=c.getContext('2d'),d=devicePixelRatio||1,w=c.clientWidth,h=c.clientHeight;
 c.width=w*d;c.height=h*d;x.scale(d,d);x.clearRect(0,0,w,h);let n=report.snapshots.length,start=0;
 const r=+document.getElementById('range').value;if(r>0)start=Math.max(0,n-r);let data=report.snapshots.slice(start), max=Math.max(...series.flatMap(s=>data.map(v=>v[s.key]||0)),1);
 x.font='11px system-ui';x.strokeStyle='#e5e7eb';x.fillStyle='#667085';for(let i=0;i<=4;i++){let y=24+(h-48)*i/4;x.beginPath();x.moveTo(42,y);x.lineTo(w-10,y);x.stroke();x.fillText((max*(1-i/4)).toFixed(1),4,y+4)}
 series.forEach((s,j)=>{x.strokeStyle=colors[j];x.lineWidth=1.7;x.beginPath();data.forEach((v,i)=>{let px=42+(w-52)*(data.length<2?0:i/(data.length-1)),py=24+(h-48)*(1-(v[s.key]||0)/max);i?x.lineTo(px,py):x.moveTo(px,py)});x.stroke();x.fillStyle=colors[j];x.fillText(s.name,50+j*100,14)});
 x.fillStyle='#667085';x.fillText(data[0]?.time||'',42,h-8);x.fillText(data[data.length-1]?.time||'',w-58,h-8);
}
function render(){draw('cpu','',[
 {name:'user',key:'cpuUser'},{name:'system',key:'cpuSystem'},{name:'wait',key:'cpuWait'},{name:'idle',key:'cpuIdle'}]);
 draw('memory','',[{name:'used',key:'memoryUsed'},{name:'cache',key:'memoryCache'},{name:'available',key:'memoryAvailable'},{name:'free',key:'memoryFree'}]);
 draw('swap','',[{name:'used',key:'swapUsed'},{name:'free',key:'swapFree'}]);
 draw('load','',[{name:'1 min',key:'load1'},{name:'5 min',key:'load5'},{name:'15 min',key:'load15'}]);
 draw('tasks','',[{name:'total',key:'totalTasks'},{name:'running',key:'runningTasks'},{name:'sleeping',key:'sleepingTasks'},{name:'zombie',key:'zombieTasks'}]);
}
document.getElementById('summary').textContent=`${report.snapshots.length.toLocaleString()} snapshots | ${report.processes.length.toLocaleString()} process names | ${report.snapshots[0].time} - ${report.snapshots.at(-1).time}`;
document.getElementById('processes').innerHTML=report.processes.map(p=>`<tr><td title="${esc(p.command)}">${esc(p.command)}</td><td>${p.samples}</td><td>${p.averageCpu.toFixed(2)}</td><td>${p.peakCpu.toFixed(2)}</td><td>${p.averageMemory.toFixed(2)}</td><td>${p.peakMemory.toFixed(2)}</td></tr>`).join('');
function esc(v){return v.replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]))}
document.getElementById('range').addEventListener('change',render);addEventListener('resize',render);render();
</script></body></html>
""";
        return html.Replace("__DATA__", json, StringComparison.Ordinal);
    }
}
