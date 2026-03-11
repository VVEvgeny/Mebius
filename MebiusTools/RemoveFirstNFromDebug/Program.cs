using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        // Register code pages encoding provider for CP866 support
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Console.WriteLine("Remove First N Symbols from Each Line");
        Console.WriteLine("=====================================\n");

        // Get file path from user
        Console.Write("Enter the path to the text file: ");
        string? filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("Error: File path cannot be empty.");
            return;
        }

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File not found: {filePath}");
            return;
        }

        // Get number of symbols to remove
        int n = 22;
        Console.Write($"Enter the number of symbols to remove from each line (default: {n}): ");
        string? nInput = Console.ReadLine();


        if (!string.IsNullOrWhiteSpace(nInput) && !int.TryParse(nInput, out n))
        {
            Console.WriteLine($"Invalid number. Using default: {n}");
        }

        try
        {
            // Read all lines from the file with CP866 encoding
            string[] lines = File.ReadAllLines(filePath, Encoding.GetEncoding("CP866"));

            // Process lines
            var processedLines = new StringBuilder();
            int processedCount = 0;

            foreach (string line in lines)
            {
                if (line.Length > 0)
                {
                    if(line.StartsWith(" "))
                    {
                        // If line starts with space 
                        processedLines.AppendLine(line);
                    }
                    else
                    {
                        if(line.Length > n)
                            // Remove first n characters
                            processedLines.AppendLine(line.Substring(n));
                    }
                }
                else if (line.Length > 0)
                {
                    // If line is shorter than n, keep it empty
                    processedLines.AppendLine("");
                }
                else
                {
                    // Empty line stays empty
                    processedLines.AppendLine(line);
                }
                processedCount++;
            }

            // Ask user where to save the result
            Console.Write("\nEnter the output file path (or press Enter to add '_processed' to the original name): ");
            string? outputPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                string directory = Path.GetDirectoryName(filePath) ?? ".";
                string filename = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                outputPath = Path.Combine(directory, $"{filename}_processed{extension}");
            }

            // Write the result to the output file with CP866 encoding
            File.WriteAllText(outputPath, processedLines.ToString(), Encoding.GetEncoding("CP866"));

            Console.WriteLine($"\nSuccess! Processed {processedCount} lines.");
            Console.WriteLine($"Result saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
