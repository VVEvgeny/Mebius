using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ScottPlot;


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

var d = new Dictionary<string, (DateTime start, DateTime end)>();
var dState = new Dictionary<DateTime, long>();

foreach (var l in fc.Split("\n"))
{
    if(string.IsNullOrWhiteSpace(l))
        continue;
    if(l.Contains(" (1001/22222 ") || l.Contains(" (1101/22222 ") || l.Contains(" (119/22222 ") || l.Contains(" (1410/22222 "))//packets + 101 + OD1
    //if(l.Contains(" (695/0 ")||l.Contains(" (690/0 ")||l.Contains(" (691/0 "))
    {
        if(l.Contains(" XKernel: New message "))
        {
            // Parse the timestamp and message ID
            var timestamp = DateTime.Parse(l.Substring(2, 23));
            var messageId = l.Split(" ")[7].Split("/")[1];
            d[messageId] = (timestamp, DateTime.MinValue);

            if (!dState.ContainsKey(timestamp))
            {
                dState[timestamp.AddMilliseconds(-timestamp.Millisecond)] = 1;
            }
            else
            {
                dState[timestamp.AddMilliseconds(-timestamp.Millisecond)]++;
            }
        }
        else if(l.Contains(" XKernel: Move msg "))
        {
            // Parse the timestamp and message ID
            var timestamp = DateTime.Parse(l.Substring(2, 23));
            var messageId = l.Split(" ")[7].Split("/")[1];
            if (d.ContainsKey(messageId))
            {
                d[messageId] = (d[messageId].start, timestamp);
            }

            if (dState.ContainsKey(timestamp))
            {
                dState[timestamp.AddMilliseconds(-timestamp.Millisecond)]--;
            }
        }
    }
}

var times = d.Where(kv => kv.Value.end != DateTime.MinValue)
            .OrderBy(x=>x.Value.start)
            .Select(kv => (MessageId: kv.Key, StartTime: kv.Value.start, EndTime: kv.Value.end, TimeInQ: kv.Value.end - kv.Value.start))
            //.OrderByDescending(x=>x.StartTime)
            .ToList();

var inInSecond = times.GroupBy(x => x.StartTime.AddMilliseconds(-x.StartTime.Millisecond))
                    .Select(g => new { StartTime = g.Key, Count = g.Count() })
                    .OrderBy(x => x.StartTime);

var movedInSecond = times.GroupBy(x => x.EndTime.AddMilliseconds(-x.EndTime.Millisecond))
                    .Select(g => new { EndTime = g.Key, Count = g.Count() })
                    .OrderBy(x => x.EndTime);

int i = 0;
/*
Console.WriteLine($"New messages in queue per second:");
foreach(var s in inInSecond)
{
    Console.WriteLine($"Time: {s.StartTime}, Count: {s.Count}");
}
*/
/*
Console.WriteLine($"Moved messages per second:");
foreach(var s in movedInSecond)
{
    Console.WriteLine($"Time: {s.EndTime}, Count: {s.Count}");
}
*/
/*
Console.WriteLine($"Messages in queue per second:");
foreach(var s in dState.OrderBy(x=>x.Key))
{
    Console.WriteLine($"Time: {s.Key}, Count: {s.Value}");
}
*/

// Generate PNG chart visualization
GenerateChart(inInSecond.Select(x => (x.StartTime, x.Count)), "New Messages in Queue Per Second");
GenerateChart(movedInSecond.Select(x => (x.EndTime, x.Count)), "Moved Messages Per Second");


static void GenerateChart(IEnumerable<(DateTime Time, int Count)> data, string title)
{
    try
    {
        var dataList = data.ToList();
        if (dataList.Count == 0)
            return;

        var plot = new Plot();
        
        // Prepare data for plotting
        double[] xValues = new double[dataList.Count];
        double[] yValues = new double[dataList.Count];
        string[] timeLabels = new string[dataList.Count];
        
        // Get the first time as reference point
        DateTime firstTime = dataList[0].Time;
        
        for (int i = 0; i < dataList.Count; i++)
        {
            // Convert to seconds elapsed from first message
            xValues[i] = (dataList[i].Time - firstTime).TotalSeconds;
            yValues[i] = dataList[i].Count;
            timeLabels[i] = dataList[i].Time.ToString("HH:mm:ss");
        }

        // Add scatter plot with lines
        var scatter = plot.Add.Scatter(xValues, yValues);
        scatter.Color = new Color(33, 150, 243); // Blue
        scatter.LineWidth = 2;
        scatter.MarkerSize = 4;

        // Customize plot
        plot.Title(title);
        plot.XLabel($"Time: {dataList[0].Time:HH:mm:ss} - {dataList.Last().Time:HH:mm:ss} (seconds elapsed)");
        plot.YLabel("Message Count");

        // Save as PNG
        string outputPath = Path.Combine(Directory.GetCurrentDirectory(), $"{title.Replace(" ", "_")}.png");
        plot.SavePng(outputPath, width: 1200, height: 600);
        Console.WriteLine($"\nChart saved to: {Path.GetFullPath(outputPath)}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error generating chart: {ex.Message}");
    }
}