using System.IO;
using System.Linq;
using System.Text;

var folderPath = @"c:\_Code\Mebius\MebiusTools\Profilers\04082026\-old\";

if (!Directory.Exists(folderPath))
{
    Console.Error.WriteLine($"Directory not found: {folderPath}");
    return;
}

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Encoding enc = Encoding.GetEncoding(866);

foreach (var filePath in Directory.EnumerateFiles(folderPath))
{
    var fileName = Path.GetFileName(filePath);
    try
    {
        var fc = new StreamReader(filePath, encoding: enc).ReadToEnd();
        
        int i=0;
        foreach (var l in fc.Split("\n"))
        {
            i++;
            if(i==4)
            {

                var startTime = l.Split("|")[9];
                var fullTime = l.Split("|")[5];

                Console.WriteLine($"{fileName}: start:{startTime}");
                break;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{fileName}: [error reading file] {ex.Message}");
    }
}
