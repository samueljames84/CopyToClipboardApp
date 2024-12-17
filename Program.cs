using System;
using System.Collections.Specialized;
using System.Windows;

partial class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: CopyToClipboard <file_path>");
            return;
        }
        string filePath = args[0];
        try
        {
            StringCollection paths = new StringCollection();
            paths.Add(filePath);
            Clipboard.SetFileDropList(paths);
            Console.WriteLine($"File path copied to clipboard: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
