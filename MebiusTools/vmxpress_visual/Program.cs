using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;


Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Encoding enc = Encoding.GetEncoding(866);

var fc = new StreamReader(@"c:\_Code\Mebius\MebiusTools\vmxpress_visual\vmxpress.log", encoding: enc).ReadToEnd();
if (fc.Length == 0)
    return;


//new message in q
//- 17.06.2026 15:36:58.361116 [70669] XKernel: New message 201/261117 (1001/22222 44525974/0 999999999/0 2 4525974000/0 4583001999/0 4525974000/-1)
//- 17.06.2026 15:36:58.361116 [70669] XKernel: New message 201/261117 (1001/22222 44525974/0 999999999/0 2 4525974000/0 4583001999/0 4525974000/-1)

//- 17.06.2026 15:36:58.607738 [70669] XKernel: Move msg 201/261117 (1001/22222 44525974/0 999999999/0 2 4525974000/0 4583001999/0 4525974000/-1) to 1002

//calc time between - 
//- 17.06.2026 15:37:33.748323 [70669] XKernel: New message 201/261580 (1001/22222 
//and 
//- 17.06.2026 15:37:36.387962 [70669] XKernel: Move msg 201/261580 (1001/22222 44525225/0 999999999/0 2 4525225000/44 4583001999/0 2202603000/-1) to 1009

var ordinaryFilter = (string line) =>    line.Contains(" (1001/0 ") || line.Contains(" (1101/0 ");
var previousDaysFilter = (string line) =>
    line.Contains(" (1001/22222 ") || line.Contains(" (1101/22222 ") ||
    line.Contains(" (119/22222 ") || line.Contains(" (1410/22222 ");
var allFilter = (string line) => ordinaryFilter(line) || previousDaysFilter(line);

var allMessages = BuildStatistics(fc, allFilter);
var ordinaryMessages = BuildStatistics(fc, ordinaryFilter);
var previousDaysMessages = BuildStatistics(fc, previousDaysFilter);

GenerateHtmlReport(new[]
{
    CreateChartGroup("Все сообщения", allMessages),
    CreateChartGroup("Обычные сообщения", ordinaryMessages),
    CreateChartGroup("Сообщения из прошлых ОД", previousDaysMessages)
});


static MessageStatistics BuildStatistics(string log, Func<string, bool> packetFilter)
{
var messages = new Dictionary<string, (DateTime Start, DateTime End)>();

foreach (var line in log.Split('\n'))
{
    if (string.IsNullOrWhiteSpace(line) || !packetFilter(line) ||
        (!line.Contains(" XKernel: New message ") && !line.Contains(" XKernel: Move msg ")))
        continue;

    var timestamp = DateTime.Parse(line.Substring(2, 23));
    var messageId = line.Split(' ')[7].Split('/')[1];

    if (line.Contains(" XKernel: New message "))
        messages[messageId] = (timestamp, DateTime.MinValue);
    else if (line.Contains(" XKernel: Move msg ") && messages.ContainsKey(messageId))
        messages[messageId] = (messages[messageId].Start, timestamp);
}

var times = messages.Values
    .Where(x => x.End != DateTime.MinValue)
    .Select(x => (StartTime: x.Start, EndTime: x.End, TimeInQ: x.End - x.Start))
    .ToList();

return new MessageStatistics(
    times.GroupBy(x => x.StartTime.AddMilliseconds(-x.StartTime.Millisecond))
        .Select(g => (g.Key, g.Count())).OrderBy(x => x.Key).ToList(),
    times.GroupBy(x => x.EndTime.AddMilliseconds(-x.EndTime.Millisecond))
        .Select(g => (g.Key, g.Count())).OrderBy(x => x.Key).ToList(),
    times.GroupBy(x => x.EndTime.AddMilliseconds(-x.EndTime.Millisecond))
        .Select(g => (g.Key, (int)Math.Round(g.Average(x => x.TimeInQ.TotalSeconds))))
        .OrderBy(x => x.Key).ToList());
}

static ChartGroup CreateChartGroup(string name, MessageStatistics statistics)
{
return new ChartGroup(name, new[]
{
    new ChartSeries(statistics.NewMessages, "New Messages in Queue Per Second", "Messages Count"),
    new ChartSeries(statistics.MovedMessages, "Moved Messages Per Second", "Messages Count"),
    new ChartSeries(statistics.AverageLag, "AVG Lag Seconds Per Second", "Average Lag (seconds)")
});
}

static void GenerateHtmlReport(IEnumerable<ChartGroup> groups)
{
try
{
        var html = new StringBuilder("""
            <!doctype html>
            <html lang="en">
            <head>
              <meta charset="utf-8">
              <meta name="viewport" content="width=device-width, initial-scale=1">
              <title>VMXpress statistics</title>
              <style>
                :root { color-scheme: light; font-family: Arial, sans-serif; }
                body { margin: 0; padding: 24px; background: #f4f6f8; color: #202124; }
                main { max-width: 1280px; margin: 0 auto; }
                h1 { font-size: 24px; margin: 0 0 20px; }
                label { font-weight: bold; margin-right: 8px; }
                select { font-size: 16px; padding: 6px 10px; margin-bottom: 20px; }
                .chart-group { display: none; }
                .chart-group.active { display: block; }
                section { background: white; margin: 0 0 24px; padding: 16px 20px 12px; border-radius: 8px;
                          box-shadow: 0 1px 4px #0002; overflow-x: auto; }
                h2 { font-size: 18px; margin: 0 0 8px; }
                svg { display: block; width: 100%; min-width: 720px; height: auto; }
                .axis-label { fill: #4d5156; font-size: 12px; }
                .grid { stroke: #e1e5e9; stroke-width: 1; }
                .axis { stroke: #6b7280; stroke-width: 1; }
                .line { fill: none; stroke: #2196f3; stroke-width: 2.5; }
                .point { fill: #2196f3; }
              </style>
            </head>
            <body>
            <main>
              <h1>VMXpress statistics</h1>
              <label for="message-filter">Показать:</label>
              <select id="message-filter">
            """);

        var groupList = groups.ToList();
        for (var i = 0; i < groupList.Count; i++)
            html.AppendLine($"""<option value="group-{i}">{WebUtility.HtmlEncode(groupList[i].Name)}</option>""");

        html.AppendLine("""
              </select>
            """);

        for (var i = 0; i < groupList.Count; i++)
        {
            html.AppendLine($"""<div id="group-{i}" class="chart-group{(i == 0 ? " active" : "")}">""");
            html.AppendLine($"""<h2>{WebUtility.HtmlEncode(groupList[i].Name)}</h2>""");
            foreach (var chart in groupList[i].Charts)
            {
                if (chart.Data.Count > 0)
                    AppendChart(html, chart.Data, chart.Title, chart.LeftText);
            }
            html.AppendLine("</div>");
        }

        html.AppendLine("""
              <script>
                const selector = document.getElementById('message-filter');
                const groups = document.querySelectorAll('.chart-group');
                selector.addEventListener('change', () => {
                  groups.forEach(group => group.classList.toggle('active', group.id === selector.value));
                });
              </script>
            """);

        html.AppendLine("""
            </main>
            </body>
            </html>
            """);

        var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "vmxpress_statistics.html");
        File.WriteAllText(outputPath, html.ToString(), new UTF8Encoding(false));
        Console.WriteLine($"\nHTML report saved to: {Path.GetFullPath(outputPath)}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error generating HTML report: {ex.Message}");
    }
}

static void AppendChart(
    StringBuilder html,
    IReadOnlyList<(DateTime Time, int Count)> data,
    string title,
    string leftText)
{
    const double width = 1200;
    const double height = 360;
    const double left = 72;
    const double right = 24;
    const double top = 22;
    const double bottom = 72;
    var plotWidth = width - left - right;
    var plotHeight = height - top - bottom;
    var maxValue = Math.Max(1, data.Max(x => x.Count));
    var firstTime = data[0].Time;
    var lastTime = data[^1].Time;
    var timeRange = Math.Max(1, (lastTime - firstTime).TotalSeconds);

    double X(int index) => left + (data[index].Time - firstTime).TotalSeconds / timeRange * plotWidth;
    double Y(int value) => top + plotHeight - value / (double)maxValue * plotHeight;

    html.AppendLine($"<section><h2>{WebUtility.HtmlEncode(title)}</h2>");
    html.AppendLine($"""<svg viewBox="0 0 {width} {height}" role="img" aria-label="{WebUtility.HtmlEncode(title)}">""");

    const int gridLines = 5;
    for (var i = 0; i <= gridLines; i++)
    {
        var value = maxValue * i / (double)gridLines;
        var y = Y((int)Math.Round(value));
        html.AppendLine($"""<line class="grid" x1="{left:F2}" y1="{y:F2}" x2="{width - right:F2}" y2="{y:F2}" />""");
        html.AppendLine($"""<text class="axis-label" x="{left - 10:F2}" y="{y + 4:F2}" text-anchor="end">{value:F0}</text>""");
    }

    html.AppendLine($"""<line class="axis" x1="{left}" y1="{top}" x2="{left}" y2="{top + plotHeight}" />""");
    html.AppendLine($"""<line class="axis" x1="{left}" y1="{top + plotHeight}" x2="{width - right}" y2="{top + plotHeight}" />""");
    html.AppendLine($"""<text class="axis-label" transform="translate(16 {top + plotHeight / 2}) rotate(-90)" text-anchor="middle">{WebUtility.HtmlEncode(leftText)}</text>""");
    html.AppendLine($"""<text class="axis-label" x="{left + plotWidth / 2:F2}" y="{height - 8}" text-anchor="middle">Time</text>""");

    var path = string.Join(" ", data.Select((point, index) =>
        $"{(index == 0 ? "M" : "L")} {X(index):F2},{Y(point.Count):F2}"));
    html.AppendLine($"""<path class="line" d="{path}" />""");
    foreach (var (point, index) in data.Select((point, index) => (point, index)))
        html.AppendLine($"""<circle class="point" cx="{X(index):F2}" cy="{Y(point.Count):F2}" r="3"><title>{point.Time:yyyy-MM-dd HH:mm:ss}: {point.Count}</title></circle>""");

    var labelStep = Math.Max(1, (int)Math.Ceiling(data.Count / 10.0));
    for (var index = 0; index < data.Count; index += labelStep)
        AppendTimeLabel(html, data[index].Time, X(index), top + plotHeight + 20);
    if (data.Count > 1 && (data.Count - 1) % labelStep != 0)
        AppendTimeLabel(html, data[^1].Time, X(data.Count - 1), top + plotHeight + 20);

    html.AppendLine("</svg></section>");
}

static void AppendTimeLabel(StringBuilder html, DateTime time, double x, double y)
{
    html.AppendLine($"""<text class="axis-label" x="{x:F2}" y="{y:F2}" text-anchor="middle">{time:HH:mm:ss}</text>""");
}

record MessageStatistics(
    IReadOnlyList<(DateTime Time, int Count)> NewMessages,
    IReadOnlyList<(DateTime Time, int Count)> MovedMessages,
    IReadOnlyList<(DateTime Time, int Count)> AverageLag);

record ChartGroup(string Name, IReadOnlyList<ChartSeries> Charts);

record ChartSeries(
    IReadOnlyList<(DateTime Time, int Count)> Data,
    string Title,
    string LeftText);