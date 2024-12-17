using System;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Linq;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: CopyToClipboard <file1> <file2> ...");
            return;
        }

        try
        {
            StringCollection paths = new StringCollection();
            
            foreach (string filePath in args)
            {
                if (File.Exists(filePath))
                {
                    paths.Add(Path.GetFullPath(filePath));
                    Console.WriteLine($"Added to clipboard: {filePath}");
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                }
            }

            if (paths.Count > 0)
            {
                // Copy files to clipboard
                Clipboard.SetFileDropList(paths);
                Console.WriteLine($"Total files copied to clipboard: {paths.Count}");

                // Convert StringCollection to string array
                string[] filePathsArray = new string[paths.Count];
                paths.CopyTo(filePathsArray, 0);

                // Create and show tooltip
                ShowTooltip(filePathsArray);
            }
            else
            {
                Console.WriteLine("No valid files to copy.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ShowTooltip(string[] filePaths)
    {
        Application.EnableVisualStyles();
        // Create tooltip form
        Form tooltipForm = new Form
        {
            FormBorderStyle = FormBorderStyle.None,
            ShowInTaskbar = false,
            StartPosition = FormStartPosition.Manual,
            Size = new Size(Screen.PrimaryScreen.WorkingArea.Width / 3, Screen.PrimaryScreen.WorkingArea.Height / 8),
            BackColor = Color.Black,
            Opacity = 0.9
        };

        // Position the form at bottom right of screen
        tooltipForm.Location = new Point(
            Screen.PrimaryScreen.WorkingArea.Width - tooltipForm.Width - 5,
            Screen.PrimaryScreen.WorkingArea.Height - tooltipForm.Height - 5
        );

        // Create label for file paths
        Label label = new Label
        {
            Dock = DockStyle.Fill,
            Text = $"{filePaths.Length} Copied files:\n" + string.Join("\n", filePaths),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.TopLeft,
            Font = new Font("Consolas", 10) 
            //Font = new Font(FontFamily.GenericSansSerif, 10)
        };

        tooltipForm.Controls.Add(label);

        // Show the form
        tooltipForm.Show();

        // Close the form after 500ms
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer
        {
            Interval = 1000
        };
        timer.Tick += (sender, e) =>
        {
            tooltipForm.Close();
            timer.Stop();
        };
        timer.Start();

        // Run the message loop to ensure the form displays
        Application.Run(tooltipForm);
    }
}