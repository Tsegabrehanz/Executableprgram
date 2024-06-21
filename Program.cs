using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static string targetFilePath;
    private static string lastFileContent = "";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Enter the path of the target text file:");
        targetFilePath = Console.ReadLine();

        if (!File.Exists(targetFilePath))
        {
            Console.WriteLine("File does not exist. Please enter a valid file path.");
            return;
        }

        // Read the initial content of the file
        lastFileContent = await ReadFileContentAsync(targetFilePath);

        // Start monitoring the file for changes
        Console.WriteLine("Monitoring file for changes. Press Ctrl+C to exit.");
        MonitorFileChanges();

        // Prevent the console app from closing immediately
        await Task.Delay(Timeout.Infinite);
    }

    private static void MonitorFileChanges()
    {
        Timer timer = new Timer(CheckFileChanges, null, 0, 15000);
    }

    private static async void CheckFileChanges(object state)
    {
        try
        {
            string currentFileContent = await ReadFileContentAsync(targetFilePath);

            if (currentFileContent != lastFileContent)
            {
                Console.WriteLine("File changed at: " + DateTime.Now);
                Console.WriteLine("Changes:");
                ReportChanges(lastFileContent, currentFileContent);
                lastFileContent = currentFileContent;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while checking file changes: " + ex.Message);
        }
    }

    private static async Task<string> ReadFileContentAsync(string filePath)
    {
        using (var reader = new StreamReader(filePath, Encoding.UTF8))
        {
            return await reader.ReadToEndAsync();
        }
    }

    private static void ReportChanges(string oldContent, string newContent)
    {
        Console.WriteLine("Old Content:");
        Console.WriteLine(oldContent);
        Console.WriteLine("New Content:");
        Console.WriteLine(newContent);
    }
}
