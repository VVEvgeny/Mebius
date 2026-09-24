using System.Text;
using System.Text.RegularExpressions;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var timePattern = new Regex(
    @"(?<hour>\d{1,2}):(?<minute>[0-5]\d):(?<second>[0-5]\d)\.(?<milliseconds>\d{4,})",
    RegexOptions.Compiled);

Console.Write("Введите путь к файлу или папке: ");
var enteredPath = Console.ReadLine()?.Trim().Trim('\uFEFF').Trim('"');

if (string.IsNullOrWhiteSpace(enteredPath))
{
    Console.Error.WriteLine("Путь не указан.");
    return 1;
}

var inputPath = Path.GetFullPath(enteredPath);
var encoding = Encoding.GetEncoding(866);

if (File.Exists(inputPath))
{
    ProcessFile(inputPath, encoding, timePattern);
}
else if (Directory.Exists(inputPath))
{
    var files = Directory
        .GetFiles(inputPath)
        .Where(path => !IsTrimmedFile(path))
        .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
        .ToArray();

    if (files.Length == 0)
    {
        Console.Error.WriteLine("В папке нет файлов для обработки.");
        return 1;
    }

    foreach (var file in files)
    {
        ProcessFile(file, encoding, timePattern);
    }
}
else
{
    Console.Error.WriteLine($"Файл или папка не найдены: {inputPath}");
    return 1;
}

Console.WriteLine("Обработка завершена.");
return 0;

static void ProcessFile(string inputPath, Encoding encoding, Regex timePattern)
{
    var outputPath = GetOutputPath(inputPath);
    var content = File.ReadAllText(inputPath, encoding);
    var trimmedContent = timePattern.Replace(
        content,
        match => $"{match.Groups["hour"].Value}:{match.Groups["minute"].Value}:{match.Groups["second"].Value}.{match.Groups["milliseconds"].Value[..3]}");

    File.WriteAllText(outputPath, trimmedContent, encoding);
    Console.WriteLine($"Готово: {outputPath}");
}

static bool IsTrimmedFile(string path) =>
    Path.GetFileNameWithoutExtension(path).StartsWith("trimmed_", StringComparison.OrdinalIgnoreCase);

static string GetOutputPath(string inputPath)
{
    var directory = Path.GetDirectoryName(inputPath) ?? Environment.CurrentDirectory;
    var fileName = Path.GetFileName(inputPath);
    var extension = Path.GetExtension(fileName);
    var name = Path.GetFileNameWithoutExtension(fileName);

    return Path.Combine(directory, $"trimmed_{name}{extension}");
}
